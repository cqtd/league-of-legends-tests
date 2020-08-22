using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class Native
{
	[DllImport("user32.dll", EntryPoint = "SetWindowText")]
	public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
	
	[DllImport("user32.dll", EntryPoint = "FindWindow")]
	public static extern System.IntPtr FindWindow(System.String className, System.String windowName);

	[Conditional("UNITY_STANDALONE_WIN")]
	public static void SetWindowText(string text)
	{
		IntPtr windowPtr = FindWindow(null, Application.productName);
		SetWindowText(windowPtr, text);	
	}

	public class WebGL
	{
		[DllImport("__Internal")]
		private static extern void OpenWindow(string url);

		[Conditional("UNITY_WEBGL")]
		public static void Open(string url)
		{
			OpenWindow(url);
		}
	}
}