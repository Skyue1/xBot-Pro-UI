using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace xBot_Pro_UI;

public static class RegistryExtensions
{
	public enum RegistryHiveType
	{
		X86,
		X64
	}

	[Flags]
	public enum RegistryAccessMask
	{
		QueryValue = 1,
		SetValue = 2,
		CreateSubKey = 4,
		EnumerateSubKeys = 8,
		Notify = 0x10,
		CreateLink = 0x20,
		WoW6432 = 0x200,
		Wow6464 = 0x100,
		Write = 0x20006,
		Read = 0x20019,
		Execute = 0x20019,
		AllAccess = 0xF003F
	}

	private static Dictionary<RegistryHive, UIntPtr> _hiveKeys = new Dictionary<RegistryHive, UIntPtr>
	{
		{
			RegistryHive.ClassesRoot,
			new UIntPtr(2147483648u)
		},
		{
			RegistryHive.CurrentConfig,
			new UIntPtr(2147483653u)
		},
		{
			RegistryHive.CurrentUser,
			new UIntPtr(2147483649u)
		},
		{
			RegistryHive.DynData,
			new UIntPtr(2147483654u)
		},
		{
			RegistryHive.LocalMachine,
			new UIntPtr(2147483650u)
		},
		{
			RegistryHive.PerformanceData,
			new UIntPtr(2147483652u)
		},
		{
			RegistryHive.Users,
			new UIntPtr(2147483651u)
		}
	};

	private static Dictionary<RegistryHiveType, RegistryAccessMask> _accessMasks = new Dictionary<RegistryHiveType, RegistryAccessMask>
	{
		{
			RegistryHiveType.X64,
			RegistryAccessMask.Wow6464
		},
		{
			RegistryHiveType.X86,
			RegistryAccessMask.WoW6432
		}
	};

	[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
	public static extern int RegOpenKeyEx(UIntPtr hKey, string subKey, uint ulOptions, uint samDesired, out IntPtr hkResult);

	public static RegistryKey OpenBaseKey(RegistryHive registryHive, RegistryHiveType registryType)
	{
		UIntPtr uIntPtr = _hiveKeys[registryHive];
		if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major > 5)
		{
			RegistryAccessMask samDesired = RegistryAccessMask.QueryValue | RegistryAccessMask.SetValue | RegistryAccessMask.CreateSubKey | RegistryAccessMask.EnumerateSubKeys | _accessMasks[registryType];
			IntPtr hkResult = IntPtr.Zero;
			int num = RegOpenKeyEx(uIntPtr, string.Empty, 0u, (uint)samDesired, out hkResult);
			switch (num)
			{
			case 0:
			{
				Type type = typeof(SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle");
				ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[2]
				{
					typeof(IntPtr),
					typeof(bool)
				}, null);
				if (constructor == null)
				{
					constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[2]
					{
						typeof(IntPtr),
						typeof(bool)
					}, null);
				}
				object obj = constructor.Invoke(new object[2] { hkResult, true });
				ConstructorInfo constructor2 = typeof(RegistryKey).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[2]
				{
					type,
					typeof(bool)
				}, null);
				ConstructorInfo constructor3 = typeof(RegistryKey).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[5]
				{
					typeof(IntPtr),
					typeof(bool),
					typeof(bool),
					typeof(bool),
					typeof(bool)
				}, null);
				object obj2 = ((constructor3 != null) ? constructor3.Invoke(new object[5]
				{
					hkResult,
					true,
					false,
					false,
					uIntPtr == _hiveKeys[RegistryHive.PerformanceData]
				}) : ((!(constructor2 != null)) ? typeof(RegistryKey).GetMethod("FromHandle", BindingFlags.Static | BindingFlags.Public, null, new Type[1] { type }, null).Invoke(null, new object[1] { obj }) : constructor2.Invoke(new object[2] { obj, true })));
				FieldInfo field = typeof(RegistryKey).GetField("keyName", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					field.SetValue(obj2, string.Empty);
				}
				return (RegistryKey)obj2;
			}
			case 2:
				return null;
			default:
				throw new Win32Exception(num);
			}
		}
		throw new PlatformNotSupportedException("The platform or operating system must be Windows XP or later.");
	}
}
