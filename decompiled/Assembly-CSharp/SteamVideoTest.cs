using Steamworks;
using UnityEngine;

public class SteamVideoTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	protected Callback<GetVideoURLResult_t> m_GetVideoURLResult;

	protected Callback<GetOPFSettingsResult_t> m_GetOPFSettingsResult;

	public void OnEnable()
	{
		m_GetVideoURLResult = Callback<GetVideoURLResult_t>.Create(OnGetVideoURLResult);
		m_GetOPFSettingsResult = Callback<GetOPFSettingsResult_t>.Create(OnGetOPFSettingsResult);
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		if (GUILayout.Button("GetVideoURL(TestConstants.Instance.k_AppId_FreeToPlay)"))
		{
			SteamVideo.GetVideoURL(TestConstants.Instance.k_AppId_FreeToPlay);
			AppId_t k_AppId_FreeToPlay = TestConstants.Instance.k_AppId_FreeToPlay;
			MonoBehaviour.print("SteamVideo.GetVideoURL(" + k_AppId_FreeToPlay.ToString() + ")");
		}
		GUILayout.Label("IsBroadcasting(out NumViewers) : " + SteamVideo.IsBroadcasting(out var pnNumViewers) + " -- " + pnNumViewers);
		if (GUILayout.Button("GetOPFSettings(TestConstants.Instance.k_AppId_FreeToPlay)"))
		{
			SteamVideo.GetOPFSettings(TestConstants.Instance.k_AppId_FreeToPlay);
			AppId_t k_AppId_FreeToPlay = TestConstants.Instance.k_AppId_FreeToPlay;
			MonoBehaviour.print("SteamVideo.GetOPFSettings(" + k_AppId_FreeToPlay.ToString() + ")");
		}
		if (GUILayout.Button("GetOPFStringForApp(TestConstants.Instance.k_AppId_FreeToPlay, out Buffer, ref ValueBufferSize)"))
		{
			int pnBufferSize = 0;
			string pchBuffer;
			bool oPFStringForApp = SteamVideo.GetOPFStringForApp(TestConstants.Instance.k_AppId_FreeToPlay, out pchBuffer, ref pnBufferSize);
			if (oPFStringForApp)
			{
				oPFStringForApp = SteamVideo.GetOPFStringForApp(TestConstants.Instance.k_AppId_FreeToPlay, out pchBuffer, ref pnBufferSize);
			}
			string[] obj = new string[8] { "SteamVideo.GetOPFStringForApp(", null, null, null, null, null, null, null };
			AppId_t k_AppId_FreeToPlay = TestConstants.Instance.k_AppId_FreeToPlay;
			obj[1] = k_AppId_FreeToPlay.ToString();
			obj[2] = ", out Buffer, ref ValueBufferSize) : ";
			obj[3] = oPFStringForApp.ToString();
			obj[4] = " -- ";
			obj[5] = pchBuffer;
			obj[6] = " -- ";
			obj[7] = pnBufferSize.ToString();
			MonoBehaviour.print(string.Concat(obj));
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnGetVideoURLResult(GetVideoURLResult_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			4611.ToString(),
			" - GetVideoURLResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		AppId_t unVideoAppID = pCallback.m_unVideoAppID;
		obj[5] = unVideoAppID.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_rgchURL;
		Debug.Log(string.Concat(obj));
	}

	private void OnGetOPFSettingsResult(GetOPFSettingsResult_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			4624.ToString(),
			" - GetOPFSettingsResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null
		};
		AppId_t unVideoAppID = pCallback.m_unVideoAppID;
		obj[5] = unVideoAppID.ToString();
		Debug.Log(string.Concat(obj));
	}
}
