using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KMPlayerAPI
{
	public class KMPlayer
	{
		public IntPtr Handle;
		public const int WM_USER = 0x0400;
		public const int WM_COMMAND = 273;
		public const int WM_DESTROY = 0x0002;

		/// <summary>
		/// Initializes and gets the window handle if it is open
		/// </summary>
		public KMPlayer()
		{
			getWindowHandle();
		}

		/// <summary>
		/// Initializes with the specified hwnd
		/// </summary>
		/// <param name="handle">The handle to use</param>
		public KMPlayer(IntPtr handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Gets the Window Handle
		/// </summary>
		public void getWindowHandle()
		{
			Handle = FindWindow("Winamp v1.x", IntPtr.Zero); 
			//EnumWindows(new EnumWindowsProc(Report), IntPtr.Zero);
		}
		
		public void DestroyInstance()
		{
			if (Handle != IntPtr.Zero)
			{
				//SendMessage(Handle, WM_DESTROY, 0, 0);
				TerminateProcess(Handle, 1);
			}
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

		//Todo: method may not work
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

		#region WM_USER Messages
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
		/// Seeks within the current track. TODO: finicky in km player
		/// </summary>
		/// <param name="sec">The offset to seek to (in seconds)</param>
		public int Seek(int sec)
		{
			return SendMessage(Handle, WM_USER, sec * 1000, 106);
		}

		public bool IsStopped()
		{
			int ret = SendMessage(Handle, WM_USER, 0, 104);
			return ret != 1 && ret != 3;
		}

		public bool IsPlaying()
		{
			return SendMessage(Handle, WM_USER, 0, 104) == 1;
		}

		public bool IsPaused()
		{
			return SendMessage(Handle, WM_USER, 0, 104) == 3;
		}
		#endregion

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

		/// <summary>
		/// Open a file in km player (This method may not work in Winamp, but does work in KM Player)
		/// </summary>
		/// <param name="playerLocation">The Path to KM Player.exe</param>
		/// <param name="fileName">The path the file you want to open</param>
		/// <param name="startTime">Optional; the start time to begin playing at (in milliseconds)</param>
		/// <returns></returns>
		public void OpenFile(string playerLocation, string fileName, int startTime = 0)
		{
			string startCmd = startTime > 0 ? " /start \"" + startTime + "\"" : "";
			//DestroyInstance();
			getWindowHandle();
			if (Handle != IntPtr.Zero) startCmd = "";
			System.Diagnostics.Process.Start(playerLocation, "\"" + fileName + "\"" + startCmd);
			if (Handle == IntPtr.Zero) return;
			System.Threading.Thread.Sleep(800);
			Seek(startTime / 1000);
		}

		#region Extern calls
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr FindWindow(string strClassName, IntPtr nptWindowName);

		[DllImport("User32.dll", EntryPoint = "SendMessage")]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

		//EnumWindows(new EnumWindowsProc(Report), IntPtr.Zero);
		public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
		private EnumWindowsProc callBackPtr;

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

		public static bool Report(IntPtr hwnd, IntPtr lParam)
		{
			//MessageBox.Show(hwnd + "");
			StringBuilder sb = new StringBuilder(1024);
			GetClassName(hwnd, sb, sb.Capacity);
			StringBuilder sb2 = new StringBuilder(1024);
			GetWindowText(hwnd, sb2, sb2.Capacity);
			if (sb.ToString().Contains("MediaViewer"))
			{
				MessageBox.Show(sb.ToString());
			}
			return true;
		}
		#endregion

	}
}
