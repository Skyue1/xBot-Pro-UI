using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace xBot_Pro_UI;

public class CpuID
{
	private const int PAGE_EXECUTE_READWRITE = 64;

	[DllImport("user32", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern IntPtr CallWindowProcW([In] byte[] bytes, IntPtr hWnd, int msg, [In][Out] byte[] wParam, IntPtr lParam);

	[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool VirtualProtect([In] byte[] bytes, IntPtr size, int newProtect, out int oldProtect);

	public static string ProcessorId()
	{
		byte[] result = new byte[8];
		if (!ExecuteCode(ref result))
		{
			return "ND";
		}
		return string.Format("{0}{1}", BitConverter.ToUInt32(result, 4).ToString("X8"), BitConverter.ToUInt32(result, 0).ToString("X8"));
	}

	private static bool ExecuteCode(ref byte[] result)
	{
		byte[] array = new byte[26]
		{
			85, 137, 229, 87, 139, 125, 16, 106, 1, 88,
			83, 15, 162, 137, 7, 137, 87, 4, 91, 95,
			137, 236, 93, 194, 16, 0
		};
		byte[] array2 = new byte[19]
		{
			83, 72, 199, 192, 1, 0, 0, 0, 15, 162,
			65, 137, 0, 65, 137, 80, 4, 91, 195
		};
		byte[] array3 = ((!IsX64Process()) ? array : array2);
		IntPtr size = new IntPtr(array3.Length);
		if (!VirtualProtect(array3, size, 64, out var _))
		{
			Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
		}
		size = new IntPtr(result.Length);
		try
		{
			return CallWindowProcW(array3, IntPtr.Zero, 0, result, size) != IntPtr.Zero;
		}
		catch
		{
			MessageBox.Show("Err_cid_asm");
			return false;
		}
	}

	private static bool IsX64Process()
	{
		return IntPtr.Size == 8;
	}
}
