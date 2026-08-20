using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowHandle : MonoBehaviour
{
	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

	public static IntPtr GetApplicationWindowHandle()
	{
		string productName = Application.productName;
		IntPtr intPtr = FindWindow(null, productName);
		if (intPtr == IntPtr.Zero)
		{
			Debug.LogError("Failed to find Unity window handle. Ensure the window title is correct.");
		}
		Debug.Log("Unity window handle:@" + productName + " " + intPtr);
		return intPtr;
	}

	[DllImport("user32.dll", SetLastError = true)]
	private static extern IntPtr GetActiveWindow();

	public static IntPtr GetActiveWindowHandle()
	{
		Debug.Log(GetActiveWindow());
		return GetActiveWindow();
	}
}
