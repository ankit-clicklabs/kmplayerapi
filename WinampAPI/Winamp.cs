﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WinampAPI
{
	public class Winamp
	{
		public IntPtr Handle;
		public const int WM_USER = 0x0400;
		public const int WM_COMMAND = 273;

		/// <summary>
		/// Initializes and gets the window handle if it is open
		/// </summary>
		public Winamp()
		{
			getWindowHandle();
		}

		/// <summary>
		/// Initializes with the specified hwnd
		/// </summary>
		/// <param name="handle">The handle to use</param>
		public Winamp(IntPtr handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Gets the Window Handle
		/// </summary>
		public void getWindowHandle()
		{
			Handle = FindWindow("Winamp v1.x", IntPtr.Zero);
		}

		#region WM_Command messages
		/// <summary>
		/// Toggles Play/Pause
		/// </summary>
		public void Play()
		{
			SendMessage(Handle, WM_COMMAND, 40046, 0);
		}

		/// <summary>
		/// Plays the current item from the beginning
		/// </summary>
		public void PlayFromBeginning()
		{
			SendMessage(Handle, WM_COMMAND, 40045, 0);
		}

		public void StopAfterCurrent()
		{
			SendMessage(Handle, WM_COMMAND, 40157, 0);
		}

		public void PreviousTrack()
		{
			SendMessage(Handle, WM_COMMAND, 40044, 0);
		}

		public void NextTrack()
		{
			SendMessage(Handle, WM_COMMAND, 40048, 0);
		}
		#endregion

		/// <summary>
		/// Returns the length of the current item in seconds
		/// </summary>
		/// <returns>Returns the length in seconds</returns>
		public int GetLength()
		{
			return SendMessage(Handle, WM_USER, 1, 105);
		}

		/// <summary>
		/// Gets the position of the current item in milliseconds
		/// </summary>
		/// <returns>Returns the length in milliseconds</returns>
		public int GetPos()
		{
			return SendMessage(Handle, WM_USER, 0, 105);
		}

		/// <summary>
		/// Seeks within the current track. TODO: fix method
		/// </summary>
		/// <param name="ms">The offset to seek to (in milliseconds)</param>
		public void Seek(int ms)
		{
			SendMessage(Handle, WM_USER, ms, 106);
		}

		/// <summary>
		/// Gets the title of the window
		/// </summary>
		/// <returns></returns>
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
