using System;
using System.Runtime.InteropServices;
using Steamworks;
using UnityEngine;

public class SteamHTMLSurfaceTest : MonoBehaviour
{
	private const int WidthOffset = 400;

	private const int HeightOffset = 100;

	private bool m_Init;

	private HHTMLBrowser m_HHTMLBrowser;

	private string m_URL;

	private Texture2D m_Texture;

	private uint m_Width;

	private uint m_Height;

	private bool m_CanGoBack;

	private bool m_CanGoForward;

	private Rect m_Rect;

	private Vector2 m_LastMousePos;

	private uint m_VerticalScrollMax;

	private uint m_VeritcalScrollCurrent;

	private uint m_HorizontalScrollMax;

	private uint m_HorizontalScrollCurrent;

	private bool m_SetKeyFocus;

	private string m_Find;

	private bool m_CurrentlyInFind;

	private float m_ScaleFactor;

	private bool m_BackgroundMode;

	protected Callback<HTML_NeedsPaint_t> m_HTML_NeedsPaint;

	protected Callback<HTML_StartRequest_t> m_HTML_StartRequest;

	protected Callback<HTML_CloseBrowser_t> m_HTML_CloseBrowser;

	protected Callback<HTML_URLChanged_t> m_HTML_URLChanged;

	protected Callback<HTML_FinishedRequest_t> m_HTML_FinishedRequest;

	protected Callback<HTML_OpenLinkInNewTab_t> m_HTML_OpenLinkInNewTab;

	protected Callback<HTML_ChangedTitle_t> m_HTML_ChangedTitle;

	protected Callback<HTML_SearchResults_t> m_HTML_SearchResults;

	protected Callback<HTML_CanGoBackAndForward_t> m_HTML_CanGoBackAndForward;

	protected Callback<HTML_HorizontalScroll_t> m_HTML_HorizontalScroll;

	protected Callback<HTML_VerticalScroll_t> m_HTML_VerticalScroll;

	protected Callback<HTML_LinkAtPosition_t> m_HTML_LinkAtPosition;

	protected Callback<HTML_JSAlert_t> m_HTML_JSAlert;

	protected Callback<HTML_JSConfirm_t> m_HTML_JSConfirm;

	protected Callback<HTML_FileOpenDialog_t> m_HTML_FileOpenDialog;

	protected Callback<HTML_NewWindow_t> m_HTML_NewWindow;

	protected Callback<HTML_SetCursor_t> m_HTML_SetCursor;

	protected Callback<HTML_StatusText_t> m_HTML_StatusText;

	protected Callback<HTML_ShowToolTip_t> m_HTML_ShowToolTip;

	protected Callback<HTML_UpdateToolTip_t> m_HTML_UpdateToolTip;

	protected Callback<HTML_HideToolTip_t> m_HTML_HideToolTip;

	protected Callback<HTML_BrowserRestarted_t> m_HTML_BrowserRestarted;

	private CallResult<HTML_BrowserReady_t> OnHTML_BrowserReadyCallResult;

	public void OnEnable()
	{
		m_HHTMLBrowser = HHTMLBrowser.Invalid;
		m_URL = "http://steamworks.github.io";
		m_Texture = null;
		m_Find = "Steamworks";
		m_CurrentlyInFind = false;
		m_ScaleFactor = 0f;
		m_BackgroundMode = false;
		m_Init = SteamHTMLSurface.Init();
		MonoBehaviour.print("SteamHTMLSurface.Init() : " + m_Init);
		m_HTML_NeedsPaint = Callback<HTML_NeedsPaint_t>.Create(OnHTML_NeedsPaint);
		m_HTML_StartRequest = Callback<HTML_StartRequest_t>.Create(OnHTML_StartRequest);
		m_HTML_CloseBrowser = Callback<HTML_CloseBrowser_t>.Create(OnHTML_CloseBrowser);
		m_HTML_URLChanged = Callback<HTML_URLChanged_t>.Create(OnHTML_URLChanged);
		m_HTML_FinishedRequest = Callback<HTML_FinishedRequest_t>.Create(OnHTML_FinishedRequest);
		m_HTML_OpenLinkInNewTab = Callback<HTML_OpenLinkInNewTab_t>.Create(OnHTML_OpenLinkInNewTab);
		m_HTML_ChangedTitle = Callback<HTML_ChangedTitle_t>.Create(OnHTML_ChangedTitle);
		m_HTML_SearchResults = Callback<HTML_SearchResults_t>.Create(OnHTML_SearchResults);
		m_HTML_CanGoBackAndForward = Callback<HTML_CanGoBackAndForward_t>.Create(OnHTML_CanGoBackAndForward);
		m_HTML_HorizontalScroll = Callback<HTML_HorizontalScroll_t>.Create(OnHTML_HorizontalScroll);
		m_HTML_VerticalScroll = Callback<HTML_VerticalScroll_t>.Create(OnHTML_VerticalScroll);
		m_HTML_LinkAtPosition = Callback<HTML_LinkAtPosition_t>.Create(OnHTML_LinkAtPosition);
		m_HTML_JSAlert = Callback<HTML_JSAlert_t>.Create(OnHTML_JSAlert);
		m_HTML_JSConfirm = Callback<HTML_JSConfirm_t>.Create(OnHTML_JSConfirm);
		m_HTML_FileOpenDialog = Callback<HTML_FileOpenDialog_t>.Create(OnHTML_FileOpenDialog);
		m_HTML_NewWindow = Callback<HTML_NewWindow_t>.Create(OnHTML_NewWindow);
		m_HTML_SetCursor = Callback<HTML_SetCursor_t>.Create(OnHTML_SetCursor);
		m_HTML_StatusText = Callback<HTML_StatusText_t>.Create(OnHTML_StatusText);
		m_HTML_ShowToolTip = Callback<HTML_ShowToolTip_t>.Create(OnHTML_ShowToolTip);
		m_HTML_UpdateToolTip = Callback<HTML_UpdateToolTip_t>.Create(OnHTML_UpdateToolTip);
		m_HTML_HideToolTip = Callback<HTML_HideToolTip_t>.Create(OnHTML_HideToolTip);
		m_HTML_BrowserRestarted = Callback<HTML_BrowserRestarted_t>.Create(OnHTML_BrowserRestarted);
		OnHTML_BrowserReadyCallResult = CallResult<HTML_BrowserReady_t>.Create(OnHTML_BrowserReady);
	}

	public void OnDisable()
	{
		RemoveBrowser();
		SteamHTMLSurface.Shutdown();
	}

	private void RemoveBrowser()
	{
		if (m_HHTMLBrowser != HHTMLBrowser.Invalid)
		{
			HHTMLBrowser hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.RemoveBrowser(" + hHTMLBrowser.ToString() + ")");
			SteamHTMLSurface.RemoveBrowser(m_HHTMLBrowser);
			m_HHTMLBrowser = HHTMLBrowser.Invalid;
		}
		m_Texture = null;
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Init: " + m_Init);
		HHTMLBrowser hHTMLBrowser = m_HHTMLBrowser;
		GUILayout.Label("m_HHTMLBrowser: " + hHTMLBrowser.ToString());
		GUILayout.Label("m_URL: " + m_URL);
		GUILayout.Label("m_Texture: " + m_Texture);
		GUILayout.Label("m_Width: " + m_Width);
		GUILayout.Label("m_Height: " + m_Height);
		GUILayout.Label("m_CanGoBack: " + m_CanGoBack);
		GUILayout.Label("m_CanGoForward: " + m_CanGoForward);
		GUILayout.Label("m_Rect: " + m_Rect);
		GUILayout.Label("m_LastMousePos: " + m_LastMousePos);
		GUILayout.Label("m_VerticalScrollMax: " + m_VerticalScrollMax);
		GUILayout.Label("m_VeritcalScrollCurrent: " + m_VeritcalScrollCurrent);
		GUILayout.Label("m_HorizontalScrollMax: " + m_HorizontalScrollMax);
		GUILayout.Label("m_HorizontalScrollCurrent: " + m_HorizontalScrollCurrent);
		GUILayout.Label("m_SetKeyFocus: " + m_SetKeyFocus);
		GUILayout.Label("m_Find: " + m_Find);
		GUILayout.Label("m_CurrentlyInFind: " + m_CurrentlyInFind);
		GUILayout.Label("m_ScaleFactor: " + m_ScaleFactor);
		GUILayout.Label("m_BackgroundMode: " + m_BackgroundMode);
		GUILayout.EndArea();
		if ((bool)m_Texture)
		{
			GUI.DrawTexture(m_Rect, m_Texture);
		}
		if (!m_Init)
		{
			GUILayout.Label("SteamHTMLSurface.Init() returned false");
			return;
		}
		if (GUILayout.Button("CreateBrowser(\"SpaceWars Test\", null)"))
		{
			RemoveBrowser();
			SteamAPICall_t steamAPICall_t = SteamHTMLSurface.CreateBrowser("SpaceWars Test", null);
			OnHTML_BrowserReadyCallResult.Set(steamAPICall_t);
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
			MonoBehaviour.print("SteamHTMLSurface.CreateBrowser(\"SpaceWars Test\", ) : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("RemoveBrowser(m_HHTMLBrowser)"))
		{
			RemoveBrowser();
		}
		m_URL = GUILayout.TextField(m_URL);
		if (GUILayout.Button("LoadURL(m_HHTMLBrowser, m_URL, null)"))
		{
			SteamHTMLSurface.LoadURL(m_HHTMLBrowser, m_URL, null);
			string[] obj = new string[5] { "SteamHTMLSurface.LoadURL(", null, null, null, null };
			hHTMLBrowser = m_HHTMLBrowser;
			obj[1] = hHTMLBrowser.ToString();
			obj[2] = ", ";
			obj[3] = m_URL;
			obj[4] = ", )";
			MonoBehaviour.print(string.Concat(obj));
		}
		if (GUILayout.Button("SetSize(m_HHTMLBrowser, m_Width, m_Height)"))
		{
			m_Width = (uint)(Screen.width - 400);
			m_Height = (uint)(Screen.height - 100);
			m_Rect = new Rect(400f, m_Height + 100, m_Width, 0L - (long)m_Height);
			m_Texture = null;
			SteamHTMLSurface.SetSize(m_HHTMLBrowser, m_Width, m_Height);
			string[] obj2 = new string[7] { "SteamHTMLSurface.SetSize(", null, null, null, null, null, null };
			hHTMLBrowser = m_HHTMLBrowser;
			obj2[1] = hHTMLBrowser.ToString();
			obj2[2] = ", ";
			obj2[3] = m_Width.ToString();
			obj2[4] = ", ";
			obj2[5] = m_Height.ToString();
			obj2[6] = ")";
			MonoBehaviour.print(string.Concat(obj2));
		}
		if (GUILayout.Button("StopLoad(m_HHTMLBrowser)"))
		{
			SteamHTMLSurface.StopLoad(m_HHTMLBrowser);
			hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.StopLoad(" + hHTMLBrowser.ToString() + ")");
		}
		if (GUILayout.Button("Reload(m_HHTMLBrowser)"))
		{
			SteamHTMLSurface.Reload(m_HHTMLBrowser);
			hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.Reload(" + hHTMLBrowser.ToString() + ")");
		}
		GUI.enabled = m_CanGoBack;
		if (GUILayout.Button("GoBack(m_HHTMLBrowser)"))
		{
			SteamHTMLSurface.GoBack(m_HHTMLBrowser);
			hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.GoBack(" + hHTMLBrowser.ToString() + ")");
		}
		GUI.enabled = m_CanGoForward;
		if (GUILayout.Button("GoForward(m_HHTMLBrowser)"))
		{
			SteamHTMLSurface.GoForward(m_HHTMLBrowser);
			hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.GoForward(" + hHTMLBrowser.ToString() + ")");
		}
		GUI.enabled = true;
		if (GUILayout.Button("AddHeader(m_HHTMLBrowser, \"From\", \"test@test.com\")"))
		{
			SteamHTMLSurface.AddHeader(m_HHTMLBrowser, "From", "test@test.com");
			hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.AddHeader(" + hHTMLBrowser.ToString() + ", \"From\", \"test@test.com\")");
		}
		if (GUILayout.Button("ExecuteJavascript(m_HHTMLBrowser, \"window.alert('Test');\")"))
		{
			SteamHTMLSurface.ExecuteJavascript(m_HHTMLBrowser, "window.alert('Test');");
			hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.ExecuteJavascript(" + hHTMLBrowser.ToString() + ", \"window.alert('Test');\")");
		}
		if (GUILayout.Button("SetKeyFocus(m_HHTMLBrowser, !m_SetKeyFocus)"))
		{
			SteamHTMLSurface.SetKeyFocus(m_HHTMLBrowser, !m_SetKeyFocus);
			string[] obj3 = new string[5] { "SteamHTMLSurface.SetKeyFocus(", null, null, null, null };
			hHTMLBrowser = m_HHTMLBrowser;
			obj3[1] = hHTMLBrowser.ToString();
			obj3[2] = ", ";
			obj3[3] = (!m_SetKeyFocus).ToString();
			obj3[4] = ")";
			MonoBehaviour.print(string.Concat(obj3));
			m_SetKeyFocus = !m_SetKeyFocus;
		}
		if (GUILayout.Button("ViewSource(m_HHTMLBrowser)"))
		{
			SteamHTMLSurface.ViewSource(m_HHTMLBrowser);
			hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.ViewSource(" + hHTMLBrowser.ToString() + ")");
		}
		if (GUILayout.Button("CopyToClipboard(m_HHTMLBrowser)"))
		{
			SteamHTMLSurface.CopyToClipboard(m_HHTMLBrowser);
			hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.CopyToClipboard(" + hHTMLBrowser.ToString() + ")");
		}
		if (GUILayout.Button("PasteFromClipboard(m_HHTMLBrowser)"))
		{
			SteamHTMLSurface.PasteFromClipboard(m_HHTMLBrowser);
			hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.PasteFromClipboard(" + hHTMLBrowser.ToString() + ")");
		}
		m_Find = GUILayout.TextField(m_Find);
		if (GUILayout.Button("Find(m_HHTMLBrowser, m_Find, m_CurrentlyInFind, false)"))
		{
			SteamHTMLSurface.Find(m_HHTMLBrowser, m_Find, m_CurrentlyInFind, bReverse: false);
			string[] obj4 = new string[9] { "SteamHTMLSurface.Find(", null, null, null, null, null, null, null, null };
			hHTMLBrowser = m_HHTMLBrowser;
			obj4[1] = hHTMLBrowser.ToString();
			obj4[2] = ", ";
			obj4[3] = m_Find;
			obj4[4] = ", ";
			obj4[5] = m_CurrentlyInFind.ToString();
			obj4[6] = ", ";
			obj4[7] = false.ToString();
			obj4[8] = ")";
			MonoBehaviour.print(string.Concat(obj4));
			m_CurrentlyInFind = true;
		}
		if (GUILayout.Button("StopFind(m_HHTMLBrowser)"))
		{
			SteamHTMLSurface.StopFind(m_HHTMLBrowser);
			hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.StopFind(" + hHTMLBrowser.ToString() + ")");
			m_CurrentlyInFind = false;
		}
		if (GUILayout.Button("GetLinkAtPosition(m_HHTMLBrowser, (500 - WidthOffset), (120 - HeightOffset))"))
		{
			SteamHTMLSurface.GetLinkAtPosition(m_HHTMLBrowser, 100, 20);
			string[] obj5 = new string[7] { "SteamHTMLSurface.GetLinkAtPosition(", null, null, null, null, null, null };
			hHTMLBrowser = m_HHTMLBrowser;
			obj5[1] = hHTMLBrowser.ToString();
			obj5[2] = ", ";
			obj5[3] = 100.ToString();
			obj5[4] = ", ";
			obj5[5] = 20.ToString();
			obj5[6] = ")";
			MonoBehaviour.print(string.Concat(obj5));
		}
		if (GUILayout.Button("SetCookie(m_URL, \"testcookiekey\", \"testcookievalue\")"))
		{
			SteamHTMLSurface.SetCookie(m_URL, "testcookiekey", "testcookievalue");
			MonoBehaviour.print("SteamHTMLSurface.SetCookie(" + m_URL + ", \"testcookiekey\", \"testcookievalue\")");
		}
		m_ScaleFactor = GUILayout.HorizontalScrollbar(m_ScaleFactor, 0.25f, 0f, 2f);
		if (GUILayout.Button("SetPageScaleFactor(m_HHTMLBrowser, m_ScaleFactor, 0, 0)"))
		{
			SteamHTMLSurface.SetPageScaleFactor(m_HHTMLBrowser, m_ScaleFactor, 0, 0);
			string[] obj6 = new string[9] { "SteamHTMLSurface.SetPageScaleFactor(", null, null, null, null, null, null, null, null };
			hHTMLBrowser = m_HHTMLBrowser;
			obj6[1] = hHTMLBrowser.ToString();
			obj6[2] = ", ";
			obj6[3] = m_ScaleFactor.ToString();
			obj6[4] = ", ";
			obj6[5] = 0.ToString();
			obj6[6] = ", ";
			obj6[7] = 0.ToString();
			obj6[8] = ")";
			MonoBehaviour.print(string.Concat(obj6));
		}
		if (GUILayout.Button("SetBackgroundMode(m_HHTMLBrowser, m_BackgroundMode)"))
		{
			SteamHTMLSurface.SetBackgroundMode(m_HHTMLBrowser, m_BackgroundMode);
			string[] obj7 = new string[5] { "SteamHTMLSurface.SetBackgroundMode(", null, null, null, null };
			hHTMLBrowser = m_HHTMLBrowser;
			obj7[1] = hHTMLBrowser.ToString();
			obj7[2] = ", ";
			obj7[3] = m_BackgroundMode.ToString();
			obj7[4] = ")";
			MonoBehaviour.print(string.Concat(obj7));
			m_BackgroundMode = !m_BackgroundMode;
		}
		if (GUILayout.Button("SetDPIScalingFactor(m_HHTMLBrowser, 1.0f)"))
		{
			SteamHTMLSurface.SetDPIScalingFactor(m_HHTMLBrowser, 1f);
			string[] obj8 = new string[5] { "SteamHTMLSurface.SetDPIScalingFactor(", null, null, null, null };
			hHTMLBrowser = m_HHTMLBrowser;
			obj8[1] = hHTMLBrowser.ToString();
			obj8[2] = ", ";
			obj8[3] = 1f.ToString();
			obj8[4] = ")";
			MonoBehaviour.print(string.Concat(obj8));
		}
		if (GUILayout.Button("OpenDeveloperTools(m_HHTMLBrowser)"))
		{
			SteamHTMLSurface.OpenDeveloperTools(m_HHTMLBrowser);
			hHTMLBrowser = m_HHTMLBrowser;
			MonoBehaviour.print("SteamHTMLSurface.OpenDeveloperTools(" + hHTMLBrowser.ToString() + ")");
		}
		if (m_HHTMLBrowser == HHTMLBrowser.Invalid)
		{
			return;
		}
		Event current = Event.current;
		if (current.mousePosition != m_LastMousePos && current.mousePosition.x >= 400f && current.mousePosition.x <= (float)(m_Width + 400) && current.mousePosition.y >= 100f && current.mousePosition.y <= (float)(m_Height + 100))
		{
			m_LastMousePos = current.mousePosition;
			SteamHTMLSurface.MouseMove(m_HHTMLBrowser, (int)(current.mousePosition.x - 400f), (int)(current.mousePosition.y - 100f));
		}
		switch (current.type)
		{
		case EventType.MouseDown:
			SteamHTMLSurface.MouseDown(m_HHTMLBrowser, (EHTMLMouseButton)current.button);
			break;
		case EventType.MouseUp:
			SteamHTMLSurface.MouseUp(m_HHTMLBrowser, (EHTMLMouseButton)current.button);
			break;
		case EventType.ScrollWheel:
			SteamHTMLSurface.MouseWheel(m_HHTMLBrowser, (int)((0f - current.delta.y) * 100f));
			break;
		case EventType.KeyDown:
		{
			EHTMLKeyModifiers eHTMLKeyModifiers = EHTMLKeyModifiers.k_eHTMLKeyModifier_None;
			if (current.alt)
			{
				eHTMLKeyModifiers |= EHTMLKeyModifiers.k_eHTMLKeyModifier_AltDown;
			}
			if (current.shift)
			{
				eHTMLKeyModifiers |= EHTMLKeyModifiers.k_eHTMLKeyModifier_ShiftDown;
			}
			if (current.control)
			{
				eHTMLKeyModifiers |= EHTMLKeyModifiers.k_eHTMLKeyModifier_CtrlDown;
			}
			if (current.keyCode != 0)
			{
				SteamHTMLSurface.KeyDown(m_HHTMLBrowser, (uint)current.keyCode, eHTMLKeyModifiers);
			}
			if (current.character != 0)
			{
				SteamHTMLSurface.KeyChar(m_HHTMLBrowser, current.character, eHTMLKeyModifiers);
			}
			if (current.keyCode == KeyCode.DownArrow)
			{
				m_VeritcalScrollCurrent = Math.Min(m_VeritcalScrollCurrent + 100, m_VerticalScrollMax);
				SteamHTMLSurface.SetVerticalScroll(m_HHTMLBrowser, m_VeritcalScrollCurrent);
			}
			else if (current.keyCode == KeyCode.UpArrow)
			{
				if (m_VeritcalScrollCurrent - 100 > m_VeritcalScrollCurrent)
				{
					m_VeritcalScrollCurrent = 0u;
				}
				else
				{
					m_VeritcalScrollCurrent -= 100u;
				}
				SteamHTMLSurface.SetVerticalScroll(m_HHTMLBrowser, m_VeritcalScrollCurrent);
			}
			else if (current.keyCode == KeyCode.RightArrow)
			{
				m_HorizontalScrollCurrent = Math.Min(m_HorizontalScrollCurrent + 100, m_HorizontalScrollMax);
				SteamHTMLSurface.SetHorizontalScroll(m_HHTMLBrowser, m_HorizontalScrollCurrent);
			}
			else if (current.keyCode == KeyCode.LeftArrow)
			{
				if (m_HorizontalScrollCurrent - 100 > m_HorizontalScrollCurrent)
				{
					m_HorizontalScrollCurrent = 0u;
				}
				else
				{
					m_HorizontalScrollCurrent -= 100u;
				}
				SteamHTMLSurface.SetHorizontalScroll(m_HHTMLBrowser, m_HorizontalScrollCurrent);
			}
			break;
		}
		case EventType.KeyUp:
		{
			EHTMLKeyModifiers eHTMLKeyModifiers = EHTMLKeyModifiers.k_eHTMLKeyModifier_None;
			if (current.alt)
			{
				eHTMLKeyModifiers |= EHTMLKeyModifiers.k_eHTMLKeyModifier_AltDown;
			}
			if (current.shift)
			{
				eHTMLKeyModifiers |= EHTMLKeyModifiers.k_eHTMLKeyModifier_ShiftDown;
			}
			if (current.control)
			{
				eHTMLKeyModifiers |= EHTMLKeyModifiers.k_eHTMLKeyModifier_CtrlDown;
			}
			if (current.keyCode != 0)
			{
				SteamHTMLSurface.KeyUp(m_HHTMLBrowser, (uint)current.keyCode, eHTMLKeyModifiers);
			}
			break;
		}
		case EventType.MouseMove:
		case EventType.MouseDrag:
			break;
		}
	}

	private void OnHTML_BrowserReady(HTML_BrowserReady_t pCallback, bool bIOFailure)
	{
		string text = 4501.ToString();
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		Debug.Log("[" + text + " - HTML_BrowserReady] - " + unBrowserHandle.ToString());
		m_HHTMLBrowser = pCallback.unBrowserHandle;
	}

	private void OnHTML_NeedsPaint(HTML_NeedsPaint_t pCallback)
	{
		string[] obj = new string[26]
		{
			"[",
			4502.ToString(),
			" - HTML_NeedsPaint] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pBGRA.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.unWide.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.unTall.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.unUpdateX.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.unUpdateY.ToString();
		obj[14] = " -- ";
		obj[15] = pCallback.unUpdateWide.ToString();
		obj[16] = " -- ";
		obj[17] = pCallback.unUpdateTall.ToString();
		obj[18] = " -- ";
		obj[19] = pCallback.unScrollX.ToString();
		obj[20] = " -- ";
		obj[21] = pCallback.unScrollY.ToString();
		obj[22] = " -- ";
		obj[23] = pCallback.flPageScale.ToString();
		obj[24] = " -- ";
		obj[25] = pCallback.unPageSerial.ToString();
		Debug.Log(string.Concat(obj));
		if (m_Texture == null)
		{
			m_Texture = new Texture2D((int)pCallback.unWide, (int)pCallback.unTall, TextureFormat.BGRA32, mipChain: false, linear: true);
		}
		int num = (int)(pCallback.unWide * pCallback.unTall * 4);
		byte[] array = new byte[num];
		Marshal.Copy(pCallback.pBGRA, array, 0, num);
		m_Texture.LoadRawTextureData(array);
		m_Texture.Apply();
	}

	private void OnHTML_StartRequest(HTML_StartRequest_t pCallback)
	{
		string[] obj = new string[12]
		{
			"[",
			4503.ToString(),
			" - HTML_StartRequest] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchURL;
		obj[6] = " -- ";
		obj[7] = pCallback.pchTarget;
		obj[8] = " -- ";
		obj[9] = pCallback.pchPostData;
		obj[10] = " -- ";
		obj[11] = pCallback.bIsRedirect.ToString();
		Debug.Log(string.Concat(obj));
		SteamHTMLSurface.AllowStartRequest(pCallback.unBrowserHandle, bAllowed: true);
		MonoBehaviour.print("SteamHTMLSurface.AllowStartRequest(pCallback.unBrowserHandle, true)");
	}

	private void OnHTML_CloseBrowser(HTML_CloseBrowser_t pCallback)
	{
		string text = 4504.ToString();
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		Debug.Log("[" + text + " - HTML_CloseBrowser] - " + unBrowserHandle.ToString());
		m_HHTMLBrowser = HHTMLBrowser.Invalid;
	}

	private void OnHTML_URLChanged(HTML_URLChanged_t pCallback)
	{
		string[] obj = new string[14]
		{
			"[",
			4505.ToString(),
			" - HTML_URLChanged] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchURL;
		obj[6] = " -- ";
		obj[7] = pCallback.pchPostData;
		obj[8] = " -- ";
		obj[9] = pCallback.bIsRedirect.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.pchPageTitle;
		obj[12] = " -- ";
		obj[13] = pCallback.bNewNavigation.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnHTML_FinishedRequest(HTML_FinishedRequest_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			4506.ToString(),
			" - HTML_FinishedRequest] - ",
			null,
			null,
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchURL;
		obj[6] = " -- ";
		obj[7] = pCallback.pchPageTitle;
		Debug.Log(string.Concat(obj));
	}

	private void OnHTML_OpenLinkInNewTab(HTML_OpenLinkInNewTab_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			4507.ToString(),
			" - HTML_OpenLinkInNewTab] - ",
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchURL;
		Debug.Log(string.Concat(obj));
	}

	private void OnHTML_ChangedTitle(HTML_ChangedTitle_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			4508.ToString(),
			" - HTML_ChangedTitle] - ",
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchTitle;
		Debug.Log(string.Concat(obj));
	}

	private void OnHTML_SearchResults(HTML_SearchResults_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			4509.ToString(),
			" - HTML_SearchResults] - ",
			null,
			null,
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.unResults.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.unCurrentMatch.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnHTML_CanGoBackAndForward(HTML_CanGoBackAndForward_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			4510.ToString(),
			" - HTML_CanGoBackAndForward] - ",
			null,
			null,
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.bCanGoBack.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.bCanGoForward.ToString();
		Debug.Log(string.Concat(obj));
		m_CanGoBack = pCallback.bCanGoBack;
		m_CanGoForward = pCallback.bCanGoForward;
	}

	private void OnHTML_HorizontalScroll(HTML_HorizontalScroll_t pCallback)
	{
		string[] obj = new string[14]
		{
			"[",
			4511.ToString(),
			" - HTML_HorizontalScroll] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.unScrollMax.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.unScrollCurrent.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.flPageScale.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.bVisible.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.unPageSize.ToString();
		Debug.Log(string.Concat(obj));
		m_HorizontalScrollMax = pCallback.unScrollMax;
		m_HorizontalScrollCurrent = pCallback.unScrollCurrent;
	}

	private void OnHTML_VerticalScroll(HTML_VerticalScroll_t pCallback)
	{
		string[] obj = new string[14]
		{
			"[",
			4512.ToString(),
			" - HTML_VerticalScroll] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.unScrollMax.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.unScrollCurrent.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.flPageScale.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.bVisible.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.unPageSize.ToString();
		Debug.Log(string.Concat(obj));
		m_VerticalScrollMax = pCallback.unScrollMax;
		m_VeritcalScrollCurrent = pCallback.unScrollCurrent;
	}

	private void OnHTML_LinkAtPosition(HTML_LinkAtPosition_t pCallback)
	{
		string[] obj = new string[14]
		{
			"[",
			4513.ToString(),
			" - HTML_LinkAtPosition] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.x.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.y.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.pchURL;
		obj[10] = " -- ";
		obj[11] = pCallback.bInput.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.bLiveLink.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnHTML_JSAlert(HTML_JSAlert_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			4514.ToString(),
			" - HTML_JSAlert] - ",
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchMessage;
		Debug.Log(string.Concat(obj));
		SteamHTMLSurface.JSDialogResponse(pCallback.unBrowserHandle, bResult: true);
		MonoBehaviour.print("SteamHTMLSurface.JSDialogResponse(pCallback.unBrowserHandle, true)");
	}

	private void OnHTML_JSConfirm(HTML_JSConfirm_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			4515.ToString(),
			" - HTML_JSConfirm] - ",
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchMessage;
		Debug.Log(string.Concat(obj));
		SteamHTMLSurface.JSDialogResponse(pCallback.unBrowserHandle, bResult: true);
		MonoBehaviour.print("SteamHTMLSurface.JSDialogResponse(pCallback.unBrowserHandle, true)");
	}

	private void OnHTML_FileOpenDialog(HTML_FileOpenDialog_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			4516.ToString(),
			" - HTML_FileOpenDialog] - ",
			null,
			null,
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchTitle;
		obj[6] = " -- ";
		obj[7] = pCallback.pchInitialFile;
		Debug.Log(string.Concat(obj));
		SteamHTMLSurface.FileLoadDialogResponse(pCallback.unBrowserHandle, IntPtr.Zero);
	}

	private void OnHTML_NewWindow(HTML_NewWindow_t pCallback)
	{
		string[] obj = new string[16]
		{
			"[",
			4521.ToString(),
			" - HTML_NewWindow] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchURL;
		obj[6] = " -- ";
		obj[7] = pCallback.unX.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.unY.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.unWide.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.unTall.ToString();
		obj[14] = " -- ";
		unBrowserHandle = pCallback.unNewWindow_BrowserHandle_IGNORE;
		obj[15] = unBrowserHandle.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnHTML_SetCursor(HTML_SetCursor_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			4522.ToString(),
			" - HTML_SetCursor] - ",
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.eMouseCursor.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnHTML_StatusText(HTML_StatusText_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			4523.ToString(),
			" - HTML_StatusText] - ",
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchMsg;
		Debug.Log(string.Concat(obj));
	}

	private void OnHTML_ShowToolTip(HTML_ShowToolTip_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			4524.ToString(),
			" - HTML_ShowToolTip] - ",
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchMsg;
		Debug.Log(string.Concat(obj));
	}

	private void OnHTML_UpdateToolTip(HTML_UpdateToolTip_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			4525.ToString(),
			" - HTML_UpdateToolTip] - ",
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.pchMsg;
		Debug.Log(string.Concat(obj));
	}

	private void OnHTML_HideToolTip(HTML_HideToolTip_t pCallback)
	{
		string text = 4526.ToString();
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		Debug.Log("[" + text + " - HTML_HideToolTip] - " + unBrowserHandle.ToString());
	}

	private void OnHTML_BrowserRestarted(HTML_BrowserRestarted_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			4527.ToString(),
			" - HTML_BrowserRestarted] - ",
			null,
			null,
			null
		};
		HHTMLBrowser unBrowserHandle = pCallback.unBrowserHandle;
		obj[3] = unBrowserHandle.ToString();
		obj[4] = " -- ";
		unBrowserHandle = pCallback.unOldBrowserHandle;
		obj[5] = unBrowserHandle.ToString();
		Debug.Log(string.Concat(obj));
	}
}
