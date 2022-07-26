using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace gh;

internal class ghapi
{
	[Flags]
	public enum ProcessAccessFlags : uint
	{
		All = 0x1F0FFFu,
		Terminate = 1u,
		CreateThread = 2u,
		VirtualMemoryOperation = 8u,
		VirtualMemoryRead = 0x10u,
		VirtualMemoryWrite = 0x20u,
		DuplicateHandle = 0x40u,
		CreateProcess = 0x80u,
		SetQuota = 0x100u,
		SetInformation = 0x200u,
		QueryInformation = 0x400u,
		QueryLimitedInformation = 0x1000u,
		Synchronize = 0x100000u
	}

	[Flags]
	private enum SnapshotFlags : uint
	{
		HeapList = 1u,
		Process = 2u,
		Thread = 4u,
		Module = 8u,
		Module32 = 0x10u,
		Inherit = 0x80000000u,
		All = 0x1Fu,
		NoHeaps = 0x40000000u
	}

	public struct PROCESSENTRY32
	{
		public uint dwSize;

		public uint cntUsage;

		public uint th32ProcessID;

		public IntPtr th32DefaultHeapID;

		public uint th32ModuleID;

		public uint cntThreads;

		public uint th32ParentProcessID;

		public int pcPriClassBase;

		public uint dwFlags;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szExeFile;
	}

	public struct MODULEENTRY32
	{
		internal uint dwSize;

		internal uint th32ModuleID;

		internal uint th32ProcessID;

		internal uint GlblcntUsage;

		internal uint ProccntUsage;

		internal IntPtr modBaseAddr;

		internal uint modBaseSize;

		internal IntPtr hModule;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		internal string szModule;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		internal string szExePath;
	}

	[Flags]
	public enum AllocationType
	{
		Commit = 0x1000,
		Reserve = 0x2000,
		Decommit = 0x4000,
		Release = 0x8000,
		Reset = 0x80000,
		Physical = 0x400000,
		TopDown = 0x100000,
		WriteWatch = 0x200000,
		LargePages = 0x20000000
	}

	[Flags]
	public enum MemoryProtection
	{
		Execute = 0x10,
		ExecuteRead = 0x20,
		ExecuteReadWrite = 0x40,
		ExecuteWriteCopy = 0x80,
		NoAccess = 1,
		ReadOnly = 2,
		ReadWrite = 4,
		WriteCopy = 8,
		GuardModifierflag = 0x100,
		NoCacheModifierflag = 0x200,
		WriteCombineModifierflag = 0x400
	}

	public const int MAX_PATH = 260;

	private const int INVALID_HANDLE_VALUE = -1;

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesRead);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [MarshalAs(UnmanagedType.AsAny)] object lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

	[DllImport("kernel32.dll")]
	private static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

	[DllImport("kernel32.dll")]
	private static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

	[DllImport("kernel32.dll")]
	private static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

	[DllImport("kernel32.dll")]
	private static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool CloseHandle(IntPtr hHandle);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr GetModuleHandle(string moduleName);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr CreateToolhelp32Snapshot(SnapshotFlags dwFlags, int th32ProcessID);

	[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
	private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

	[DllImport("kernel32.dll")]
	private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

	public static IntPtr GetModuleBaseAddress(Process proc, string modName)
	{
		IntPtr zero = IntPtr.Zero;
		foreach (ProcessModule module in proc.Modules)
		{
			if (module.ModuleName == modName)
			{
				return module.BaseAddress;
			}
		}
		return zero;
	}

	public static IntPtr GetModuleBaseAddress(int procId, string modName)
	{
		IntPtr result = IntPtr.Zero;
		IntPtr intPtr = CreateToolhelp32Snapshot(SnapshotFlags.Module | SnapshotFlags.Module32, procId);
		if (intPtr.ToInt64() != -1)
		{
			MODULEENTRY32 lpme = default(MODULEENTRY32);
			lpme.dwSize = (uint)Marshal.SizeOf(typeof(MODULEENTRY32));
			if (Module32First(intPtr, ref lpme))
			{
				do
				{
					if (lpme.szModule.Equals(modName))
					{
						result = lpme.modBaseAddr;
						break;
					}
				}
				while (Module32Next(intPtr, ref lpme));
			}
		}
		CloseHandle(intPtr);
		return result;
	}

	public static int GetProcId(string procname)
	{
		int result = 0;
		IntPtr intPtr = CreateToolhelp32Snapshot(SnapshotFlags.Process, 0);
		if (intPtr.ToInt64() != -1)
		{
			PROCESSENTRY32 lppe = default(PROCESSENTRY32);
			lppe.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));
			if (Process32First(intPtr, ref lppe))
			{
				do
				{
					if (lppe.szExeFile.Equals(procname))
					{
						result = (int)lppe.th32ProcessID;
						break;
					}
				}
				while (Process32Next(intPtr, ref lppe));
			}
		}
		CloseHandle(intPtr);
		return result;
	}

	public static IntPtr FindDMAAddy(IntPtr hProc, IntPtr ptr, int[] offsets)
	{
		byte[] array = new byte[IntPtr.Size];
		foreach (int offset in offsets)
		{
			ReadProcessMemory(hProc, ptr, array, array.Length, out var _);
			ptr = ((IntPtr.Size == 4) ? IntPtr.Add(new IntPtr(BitConverter.ToInt32(array, 0)), offset) : (ptr = IntPtr.Add(new IntPtr(BitConverter.ToInt64(array, 0)), offset)));
		}
		return ptr;
	}

	public static bool InjectDLL(string dllpath, string procname)
	{
		Process[] processesByName = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(procname));
		if (processesByName.Length == 0)
		{
			return false;
		}
		Process process = processesByName[0];
		GetProcId(procname);
		OpenProcess(ProcessAccessFlags.All, bInheritHandle: false, process.Id);
		if (process.Handle != IntPtr.Zero)
		{
			IntPtr intPtr = VirtualAllocEx(process.Handle, IntPtr.Zero, 260u, AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ReadWrite);
			if (intPtr.Equals(0))
			{
				return false;
			}
			IntPtr lpNumberOfBytesWritten = IntPtr.Zero;
			if (!WriteProcessMemory(process.Handle, intPtr, dllpath.ToCharArray(), dllpath.Length, out lpNumberOfBytesWritten) || lpNumberOfBytesWritten.Equals(0))
			{
				return false;
			}
			IntPtr procAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
			procAddress = GetProcAddress(GetModuleBaseAddress(process.Id, "KERNEL32.DLL"), "LoadLibraryA");
			IntPtr lpThreadId;
			IntPtr hHandle = CreateRemoteThread(process.Handle, IntPtr.Zero, 0u, procAddress, intPtr, 0u, out lpThreadId);
			if (!hHandle.Equals(0))
			{
				CloseHandle(hHandle);
				process.Dispose();
				return true;
			}
			return false;
		}
		return false;
	}
}
