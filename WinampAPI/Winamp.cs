using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WinampAPI
{
	public class Winamp
	{
		public IntPtr Handle;

		public Winamp()
		{
			getWindowHandle();
		}

		public Winamp(IntPtr handle)
		{
			Handle = handle;
		}

		public void getWindowHandle()
		{
			Handle = FindWindow("Winamp v1.x", IntPtr.Zero);
		}

		public void Play()
		{
			SendMessage(Handle, 273, 40046, 0);
		}

		public void Pause()
		{
			SendMessage(Handle, 273, 40046, 0);
		}

		public int GetLength()
		{
			return SendMessage(Handle, 0x0400, 1, 105);
		}

		public int GetPos()
		{
			return SendMessage(Handle, 0x0400, 0, 105);
		}

		public string GetWindowTitle()
		{
			StringBuilder wTitle = new StringBuilder(1024);
			GetWindowText(Handle, wTitle, wTitle.Capacity);
			return wTitle.ToString();
		}

		#region Extern calls
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr FindWindow(string strClassName, IntPtr nptWindowName);

		[DllImport("User32.dll", EntryPoint = "SendMessage")]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		//[DllImport("user32.dll", CharSet = CharSet.Auto)]
		//public static extern int SendMessageA(IntPtr hwnd, int wMsg, int wParam, uint lParam);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		//EnumWindows(new EnumWindowsProc(Report), IntPtr.Zero);
		//public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
		//private EnumWindowsProc callBackPtr;

		//[DllImport("user32.dll")]
		//[return: MarshalAs(UnmanagedType.Bool)]
		//static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

		//public static bool Report(IntPtr hwnd, IntPtr lParam)
		//{
		//  //MessageBox.Show(hwnd + "");
		//  StringBuilder sb = new StringBuilder(1024);
		//  GetClassName(hwnd, sb, sb.Capacity);
		//  StringBuilder sb2 = new StringBuilder(1024);
		//  GetWindowText(hwnd, sb2, sb2.Capacity);
		//  if (sb.ToString().Contains("Winamp"))
		//  {
		//  }
		//  return true;
		//}
		#endregion

	}
}
