using System.Collections;
using System.IO;
using Steamworks;
using UnityEngine;

public class SteamScreenshotsTest : MonoBehaviour
{
	public static SteamScreenshotsTest Inst;

	private Vector2 m_ScrollPos;

	private ScreenshotHandle m_ScreenshotHandle;

	private bool m_Hooked;

	protected Callback<ScreenshotReady_t> m_ScreenshotReady;

	protected Callback<ScreenshotRequested_t> m_ScreenshotRequested;

	public void Init()
	{
		if (Inst == null)
		{
			Inst = this;
		}
		m_ScreenshotReady = Callback<ScreenshotReady_t>.Create(OnScreenshotReady);
		m_ScreenshotRequested = Callback<ScreenshotRequested_t>.Create(OnScreenshotRequested);
	}

	public void ScreenShoot()
	{
		StartCoroutine(WriteScreenshot());
	}

	private IEnumerator WriteScreenshot()
	{
		yield return new WaitForEndOfFrame();
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, mipChain: false);
		texture2D.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0, recalculateMipMaps: false);
		Texture2D texture2D2 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, mipChain: false);
		for (int i = 0; i < Screen.width; i++)
		{
			for (int j = 0; j < Screen.height; j++)
			{
				texture2D2.SetPixel(i, j, texture2D.GetPixel(i, Screen.height - 1 - j));
			}
		}
		texture2D2.Apply(updateMipmaps: false);
		Color[] pixels = texture2D2.GetPixels();
		byte[] array = new byte[pixels.Length * 3];
		int num = 0;
		int num2 = 0;
		while (num < array.Length)
		{
			array[num] = (byte)(pixels[num2].r * 255f);
			array[num + 1] = (byte)(pixels[num2].g * 255f);
			array[num + 2] = (byte)(pixels[num2].b * 255f);
			num += 3;
			num2++;
		}
		Object.Destroy(texture2D);
		m_ScreenshotHandle = SteamScreenshots.WriteScreenshot(array, (uint)array.Length, Screen.width, Screen.height);
		string[] obj = new string[10]
		{
			"SteamScreenshots.WriteScreenshot(",
			array?.ToString(),
			", ",
			((uint)array.Length).ToString(),
			", ",
			Screen.width.ToString(),
			", ",
			Screen.height.ToString(),
			") : ",
			null
		};
		ScreenshotHandle screenshotHandle = m_ScreenshotHandle;
		obj[9] = screenshotHandle.ToString();
		MonoBehaviour.print(string.Concat(obj));
	}

	private IEnumerator AddScreenshotToLibrary()
	{
		while (!File.Exists(Application.dataPath + "/screenshot.png"))
		{
			yield return null;
		}
		m_ScreenshotHandle = SteamScreenshots.AddScreenshotToLibrary(Application.dataPath + "/screenshot.png", "", Screen.width, Screen.height);
		string[] obj = new string[6]
		{
			"SteamScreenshots.AddScreenshotToLibrary(\"screenshot.png\", \"\", ",
			Screen.width.ToString(),
			", ",
			Screen.height.ToString(),
			") : ",
			null
		};
		ScreenshotHandle screenshotHandle = m_ScreenshotHandle;
		obj[5] = screenshotHandle.ToString();
		MonoBehaviour.print(string.Concat(obj));
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		ScreenshotHandle screenshotHandle = m_ScreenshotHandle;
		GUILayout.Label("m_ScreenshotHandle: " + screenshotHandle.ToString());
		GUILayout.Label("m_Hooked: " + m_Hooked);
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		if (GUILayout.Button("WriteScreenshot(RGB, (uint)RGB.Length, Screen.width, Screen.height)"))
		{
			StartCoroutine(WriteScreenshot());
		}
		if (GUILayout.Button("AddScreenshotToLibrary(ScreenCapture.dataPath + \"/screenshot.png\", \"\", Screen.width, Screen.height)"))
		{
			ScreenCapture.CaptureScreenshot("screenshot.png");
			StartCoroutine(AddScreenshotToLibrary());
		}
		if (GUILayout.Button("TriggerScreenshot()"))
		{
			SteamScreenshots.TriggerScreenshot();
			MonoBehaviour.print("SteamScreenshots.TriggerScreenshot()");
		}
		if (GUILayout.Button("HookScreenshots(!m_Hooked)"))
		{
			SteamScreenshots.HookScreenshots(!m_Hooked);
			MonoBehaviour.print("SteamScreenshots.HookScreenshots(" + !m_Hooked + ")");
			m_Hooked = !m_Hooked;
		}
		if (GUILayout.Button("SetLocation(m_ScreenshotHandle, \"LocationTest\")"))
		{
			bool flag = SteamScreenshots.SetLocation(m_ScreenshotHandle, "LocationTest");
			screenshotHandle = m_ScreenshotHandle;
			MonoBehaviour.print("SteamScreenshots.SetLocation(" + screenshotHandle.ToString() + ", \"LocationTest\") : " + flag);
		}
		if (GUILayout.Button("TagUser(m_ScreenshotHandle, TestConstants.Instance.k_SteamId_rlabrecque)"))
		{
			bool flag2 = SteamScreenshots.TagUser(m_ScreenshotHandle, TestConstants.Instance.k_SteamId_rlabrecque);
			string[] obj = new string[6] { "SteamScreenshots.TagUser(", null, null, null, null, null };
			screenshotHandle = m_ScreenshotHandle;
			obj[1] = screenshotHandle.ToString();
			obj[2] = ", ";
			CSteamID k_SteamId_rlabrecque = TestConstants.Instance.k_SteamId_rlabrecque;
			obj[3] = k_SteamId_rlabrecque.ToString();
			obj[4] = ") : ";
			obj[5] = flag2.ToString();
			MonoBehaviour.print(string.Concat(obj));
		}
		if (GUILayout.Button("TagPublishedFile(m_ScreenshotHandle, PublishedFileId_t.Invalid)"))
		{
			bool flag3 = SteamScreenshots.TagPublishedFile(m_ScreenshotHandle, PublishedFileId_t.Invalid);
			string[] obj2 = new string[6] { "SteamScreenshots.TagPublishedFile(", null, null, null, null, null };
			screenshotHandle = m_ScreenshotHandle;
			obj2[1] = screenshotHandle.ToString();
			obj2[2] = ", ";
			PublishedFileId_t invalid = PublishedFileId_t.Invalid;
			obj2[3] = invalid.ToString();
			obj2[4] = ") : ";
			obj2[5] = flag3.ToString();
			MonoBehaviour.print(string.Concat(obj2));
		}
		GUILayout.Label("IsScreenshotsHooked() : " + SteamScreenshots.IsScreenshotsHooked());
		if (GUILayout.Button("AddVRScreenshotToLibrary(EVRScreenshotType.k_EVRScreenshotType_None, null, null)"))
		{
			ScreenshotHandle screenshotHandle2 = SteamScreenshots.AddVRScreenshotToLibrary(EVRScreenshotType.k_EVRScreenshotType_None, null, null);
			string text = EVRScreenshotType.k_EVRScreenshotType_None.ToString();
			screenshotHandle = screenshotHandle2;
			MonoBehaviour.print("SteamScreenshots.AddVRScreenshotToLibrary(" + text + ", , ) : " + screenshotHandle.ToString());
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnScreenshotReady(ScreenshotReady_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			2301.ToString(),
			" - ScreenshotReady] - ",
			null,
			null,
			null
		};
		ScreenshotHandle hLocal = pCallback.m_hLocal;
		obj[3] = hLocal.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eResult.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnScreenshotRequested(ScreenshotRequested_t pCallback)
	{
		Debug.Log("[" + 2302 + " - ScreenshotRequested]");
	}
}
