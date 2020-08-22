using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public static class Native
{
	public static class Window
	{
		[DllImport("user32.dll", EntryPoint = "SetWindowText")]
		static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
	
		[DllImport("user32.dll", EntryPoint = "FindWindow")]
		static extern System.IntPtr FindWindow(System.String className, System.String windowName);
		

		[Conditional("UNITY_STANDALONE_WIN")]
		public static void SetWindowText(string text)
		{
			IntPtr windowPtr = FindWindow(null, Application.productName);
			SetWindowText(windowPtr, text);	
		}
	}
	
	
#if UNITY_WEBGL
	public static class WebGL
	{
		//https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html?_ga=2.242629824.379029621.1598020523-1939043715.1580177978
		[DllImport("__Internal")]
		private static extern void OpenWindow(string url);
	
		[Conditional("UNITY_WEBGL")]
		public static void Open(string url)
		{
			OpenWindow(url);
		}
	}
#endif

	public static class Global
	{
		public static void OpenURL(string url)
		{
#if UNITY_EDITOR
			Application.OpenURL(url);
#elif UNITY_STANDALONE_WIN
			Application.OpenURL(url);

#elif UNITY_WEBGL
			WebGL.Open(url);	
#else

#endif
		}
	}
}