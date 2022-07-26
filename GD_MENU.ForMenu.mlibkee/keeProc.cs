using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace GD_MENU.ForMenu.mlibkee;

public class keeProc
{
	[DllImport("kernel32.dll")]
	public static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesRead);

	public static int ReadAddress(string Process_Name, string Address_Offsets)
	{
		Process[] processesByName;
		if ((processesByName = Process.GetProcessesByName(Process_Name)).Length == 0)
		{
			return -1;
		}
		int num = -1;
		while (Address_Offsets.Contains("  "))
		{
			Address_Offsets = Address_Offsets.Replace("  ", " ");
		}
		int num2 = -1;
		while ((num2 = Address_Offsets.IndexOf("0x", StringComparison.OrdinalIgnoreCase)) != -1)
		{
			Address_Offsets = Address_Offsets.Replace(Address_Offsets.Substring(num2, 2), "");
		}
		string[] array = Address_Offsets.Split(' ');
		if (array[0].Contains("+"))
		{
			string[] array2 = array[0].Split('+');
			foreach (ProcessModule module in processesByName[0].Modules)
			{
				if (module.ModuleName.ToLower() == array2[0].ToLower())
				{
					num = module.BaseAddress.ToInt32() + int.Parse(array2[1], NumberStyles.HexNumber);
				}
			}
		}
		else
		{
			num = int.Parse(array[0], NumberStyles.HexNumber);
		}
		if (array.Length == 1)
		{
			return num;
		}
		byte[] array3 = new byte[4];
		ReadProcessMemory(processesByName[0].Handle, num, array3, 4, 0);
		num = BitConverter.ToInt32(array3, 0);
		for (int i = 1; i < array.Length; i++)
		{
			int num3 = int.Parse(array[i], NumberStyles.HexNumber);
			ReadProcessMemory(processesByName[0].Handle, num + num3, array3, 4, 0);
			num = ((i != array.Length - 1) ? BitConverter.ToInt32(array3, 0) : (num += num3));
		}
		return num;
	}
}
