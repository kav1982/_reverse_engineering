using System;
using Steamworks;
using UnityEngine;

public class SteamUserTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	private byte[] m_Ticket;

	private uint m_pcbTicket;

	private HAuthTicket m_HAuthTicket;

	private GameObject m_VoiceLoopback;

	protected Callback<SteamServersConnected_t> m_SteamServersConnected;

	protected Callback<SteamServerConnectFailure_t> m_SteamServerConnectFailure;

	protected Callback<SteamServersDisconnected_t> m_SteamServersDisconnected;

	protected Callback<ClientGameServerDeny_t> m_ClientGameServerDeny;

	protected Callback<IPCFailure_t> m_IPCFailure;

	protected Callback<LicensesUpdated_t> m_LicensesUpdated;

	protected Callback<ValidateAuthTicketResponse_t> m_ValidateAuthTicketResponse;

	protected Callback<MicroTxnAuthorizationResponse_t> m_MicroTxnAuthorizationResponse;

	protected Callback<GetAuthSessionTicketResponse_t> m_GetAuthSessionTicketResponse;

	protected Callback<GameWebCallback_t> m_GameWebCallback;

	protected Callback<GetTicketForWebApiResponse_t> m_GetTicketForWebApiResponse;

	private CallResult<EncryptedAppTicketResponse_t> OnEncryptedAppTicketResponseCallResult;

	private CallResult<StoreAuthURLResponse_t> OnStoreAuthURLResponseCallResult;

	private CallResult<MarketEligibilityResponse_t> OnMarketEligibilityResponseCallResult;

	private CallResult<DurationControl_t> OnDurationControlCallResult;

	public void OnEnable()
	{
		m_SteamServersConnected = Callback<SteamServersConnected_t>.Create(OnSteamServersConnected);
		m_SteamServerConnectFailure = Callback<SteamServerConnectFailure_t>.Create(OnSteamServerConnectFailure);
		m_SteamServersDisconnected = Callback<SteamServersDisconnected_t>.Create(OnSteamServersDisconnected);
		m_ClientGameServerDeny = Callback<ClientGameServerDeny_t>.Create(OnClientGameServerDeny);
		m_IPCFailure = Callback<IPCFailure_t>.Create(OnIPCFailure);
		m_LicensesUpdated = Callback<LicensesUpdated_t>.Create(OnLicensesUpdated);
		m_ValidateAuthTicketResponse = Callback<ValidateAuthTicketResponse_t>.Create(OnValidateAuthTicketResponse);
		m_MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(OnMicroTxnAuthorizationResponse);
		m_GetAuthSessionTicketResponse = Callback<GetAuthSessionTicketResponse_t>.Create(OnGetAuthSessionTicketResponse);
		m_GameWebCallback = Callback<GameWebCallback_t>.Create(OnGameWebCallback);
		m_GetTicketForWebApiResponse = Callback<GetTicketForWebApiResponse_t>.Create(OnGetTicketForWebApiResponse);
		OnEncryptedAppTicketResponseCallResult = CallResult<EncryptedAppTicketResponse_t>.Create(OnEncryptedAppTicketResponse);
		OnStoreAuthURLResponseCallResult = CallResult<StoreAuthURLResponse_t>.Create(OnStoreAuthURLResponse);
		OnMarketEligibilityResponseCallResult = CallResult<MarketEligibilityResponse_t>.Create(OnMarketEligibilityResponse);
		OnDurationControlCallResult = CallResult<DurationControl_t>.Create(OnDurationControl);
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Ticket: " + m_Ticket);
		GUILayout.Label("m_pcbTicket: " + m_pcbTicket);
		HAuthTicket hAuthTicket = m_HAuthTicket;
		GUILayout.Label("m_HAuthTicket: " + hAuthTicket.ToString());
		GUILayout.Label("m_VoiceLoopback: " + m_VoiceLoopback);
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		GUILayout.Label("GetHSteamUser() : " + SteamUser.GetHSteamUser().ToString());
		GUILayout.Label("BLoggedOn() : " + SteamUser.BLoggedOn());
		GUILayout.Label("GetSteamID() : " + SteamUser.GetSteamID().ToString());
		GUILayout.Label("GetUserDataFolder(out Buffer, 260) : " + SteamUser.GetUserDataFolder(out var pchBuffer, 260) + " -- " + pchBuffer);
		if (GUILayout.Button("StartVoiceRecording()"))
		{
			SteamUser.StartVoiceRecording();
			MonoBehaviour.print("SteamUser.StartVoiceRecording()");
		}
		if (GUILayout.Button("StopVoiceRecording()"))
		{
			SteamUser.StopVoiceRecording();
			MonoBehaviour.print("SteamUser.StopVoiceRecording()");
		}
		uint pcbCompressed;
		EVoiceResult availableVoice = SteamUser.GetAvailableVoice(out pcbCompressed);
		GUILayout.Label("GetAvailableVoice(out Compressed) : " + availableVoice.ToString() + " -- " + pcbCompressed);
		if (availableVoice == EVoiceResult.k_EVoiceResultOK && pcbCompressed != 0)
		{
			byte[] array = new byte[1024];
			if (SteamUser.GetVoice(bWantCompressed: true, array, 1024u, out var nBytesWritten) == EVoiceResult.k_EVoiceResultOK && nBytesWritten != 0)
			{
				byte[] array2 = new byte[22050];
				if (SteamUser.DecompressVoice(array, nBytesWritten, array2, (uint)array2.Length, out var nBytesWritten2, 11025u) == EVoiceResult.k_EVoiceResultOK && nBytesWritten2 != 0)
				{
					AudioSource audioSource;
					if (!m_VoiceLoopback)
					{
						m_VoiceLoopback = new GameObject("Voice Loopback");
						audioSource = m_VoiceLoopback.AddComponent<AudioSource>();
						audioSource.clip = AudioClip.Create("Testing!", 11025, 1, 11025, stream: false);
					}
					else
					{
						audioSource = m_VoiceLoopback.GetComponent<AudioSource>();
					}
					float[] array3 = new float[11025];
					for (int i = 0; i < array3.Length; i++)
					{
						array3[i] = (float)(short)(array2[i * 2] | (array2[i * 2 + 1] << 8)) / 32768f;
					}
					audioSource.clip.SetData(array3, 0);
					audioSource.Play();
				}
			}
		}
		GUILayout.Label("GetVoiceOptimalSampleRate() : " + SteamUser.GetVoiceOptimalSampleRate());
		if (GUILayout.Button("GetAuthSessionTicket(Ticket, 1024, out pcbTicket)"))
		{
			SteamNetworkingIdentity pSteamNetworkingIdentity = default(SteamNetworkingIdentity);
			m_Ticket = new byte[1024];
			m_HAuthTicket = SteamUser.GetAuthSessionTicket(m_Ticket, 1024, out m_pcbTicket, ref pSteamNetworkingIdentity);
			hAuthTicket = m_HAuthTicket;
			MonoBehaviour.print("SteamUser.GetAuthSessionTicket(Ticket, 1024, out pcbTicket, ref pSteamNetworkingIdentity) - " + hAuthTicket.ToString() + " -- " + m_pcbTicket);
		}
		if (GUILayout.Button("GetAuthTicketForWebApi(null)"))
		{
			HAuthTicket authTicketForWebApi = SteamUser.GetAuthTicketForWebApi(null);
			hAuthTicket = authTicketForWebApi;
			MonoBehaviour.print("SteamUser.GetAuthTicketForWebApi() : " + hAuthTicket.ToString());
		}
		if (GUILayout.Button("BeginAuthSession(m_Ticket, (int)m_pcbTicket, SteamUser.GetSteamID())"))
		{
			if (m_HAuthTicket != HAuthTicket.Invalid && m_pcbTicket != 0)
			{
				EBeginAuthSessionResult eBeginAuthSessionResult = SteamUser.BeginAuthSession(m_Ticket, (int)m_pcbTicket, SteamUser.GetSteamID());
				string[] obj = new string[6] { "SteamUser.BeginAuthSession(m_Ticket, ", null, null, null, null, null };
				int pcbTicket = (int)m_pcbTicket;
				obj[1] = pcbTicket.ToString();
				obj[2] = ", ";
				obj[3] = SteamUser.GetSteamID().ToString();
				obj[4] = ") - ";
				obj[5] = eBeginAuthSessionResult.ToString();
				MonoBehaviour.print(string.Concat(obj));
			}
			else
			{
				MonoBehaviour.print("Call GetAuthSessionTicket first!");
			}
		}
		if (GUILayout.Button("EndAuthSession(SteamUser.GetSteamID())"))
		{
			SteamUser.EndAuthSession(SteamUser.GetSteamID());
			MonoBehaviour.print("SteamUser.EndAuthSession(" + SteamUser.GetSteamID().ToString() + ")");
		}
		if (GUILayout.Button("CancelAuthTicket(m_HAuthTicket)"))
		{
			SteamUser.CancelAuthTicket(m_HAuthTicket);
			hAuthTicket = m_HAuthTicket;
			MonoBehaviour.print("SteamUser.CancelAuthTicket(" + hAuthTicket.ToString() + ")");
		}
		GUILayout.Label("UserHasLicenseForApp(SteamUser.GetSteamID(), SteamUtils.GetAppID()) : " + SteamUser.UserHasLicenseForApp(SteamUser.GetSteamID(), SteamUtils.GetAppID()));
		GUILayout.Label("BIsBehindNAT() : " + SteamUser.BIsBehindNAT());
		if (GUILayout.Button("AdvertiseGame(CSteamID.NonSteamGS, TestConstants.k_IpAddress127_0_0_1_uint, TestConstants.k_Port27015)"))
		{
			SteamUser.AdvertiseGame(CSteamID.NonSteamGS, 2130706433u, 27015);
			string[] obj2 = new string[7] { "SteamUser.AdvertiseGame(", null, null, null, null, null, null };
			CSteamID nonSteamGS = CSteamID.NonSteamGS;
			obj2[1] = nonSteamGS.ToString();
			obj2[2] = ", ";
			obj2[3] = 2130706433u.ToString();
			obj2[4] = ", ";
			obj2[5] = ((ushort)27015).ToString();
			obj2[6] = ")";
			MonoBehaviour.print(string.Concat(obj2));
		}
		if (GUILayout.Button("RequestEncryptedAppTicket(k_unSecretData, sizeof(uint))"))
		{
			byte[] bytes = BitConverter.GetBytes(21572);
			SteamAPICall_t steamAPICall_t = SteamUser.RequestEncryptedAppTicket(bytes, 4);
			OnEncryptedAppTicketResponseCallResult.Set(steamAPICall_t);
			string[] obj3 = new string[6]
			{
				"SteamUser.RequestEncryptedAppTicket(",
				bytes?.ToString(),
				", ",
				4.ToString(),
				") : ",
				null
			};
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
			obj3[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj3));
		}
		if (GUILayout.Button("GetEncryptedAppTicket(rgubTicket, 1024, out cubTicket)"))
		{
			byte[] array4 = new byte[1024];
			uint pcbTicket2;
			bool encryptedAppTicket = SteamUser.GetEncryptedAppTicket(array4, 1024, out pcbTicket2);
			MonoBehaviour.print("SteamUser.GetEncryptedAppTicket(" + array4?.ToString() + ", " + 1024 + ", out cubTicket) : " + encryptedAppTicket + " -- " + pcbTicket2);
		}
		if (GUILayout.Button("GetGameBadgeLevel(1, false)"))
		{
			int gameBadgeLevel = SteamUser.GetGameBadgeLevel(1, bFoil: false);
			MonoBehaviour.print("SteamUser.GetGameBadgeLevel(" + 1 + ", " + false + ") : " + gameBadgeLevel);
		}
		GUILayout.Label("GetPlayerSteamLevel() : " + SteamUser.GetPlayerSteamLevel());
		if (GUILayout.Button("RequestStoreAuthURL(\"https://steampowered.com\")"))
		{
			SteamAPICall_t steamAPICall_t3 = SteamUser.RequestStoreAuthURL("https://steampowered.com");
			OnStoreAuthURLResponseCallResult.Set(steamAPICall_t3);
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t3;
			MonoBehaviour.print("SteamUser.RequestStoreAuthURL(\"https://steampowered.com\") : " + steamAPICall_t2.ToString());
		}
		GUILayout.Label("BIsPhoneVerified() : " + SteamUser.BIsPhoneVerified());
		GUILayout.Label("BIsTwoFactorEnabled() : " + SteamUser.BIsTwoFactorEnabled());
		GUILayout.Label("BIsPhoneIdentifying() : " + SteamUser.BIsPhoneIdentifying());
		GUILayout.Label("BIsPhoneRequiringVerification() : " + SteamUser.BIsPhoneRequiringVerification());
		if (GUILayout.Button("GetMarketEligibility()"))
		{
			SteamAPICall_t marketEligibility = SteamUser.GetMarketEligibility();
			OnMarketEligibilityResponseCallResult.Set(marketEligibility);
			SteamAPICall_t steamAPICall_t2 = marketEligibility;
			MonoBehaviour.print("SteamUser.GetMarketEligibility() : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("GetDurationControl()"))
		{
			SteamAPICall_t durationControl = SteamUser.GetDurationControl();
			OnDurationControlCallResult.Set(durationControl);
			SteamAPICall_t steamAPICall_t2 = durationControl;
			MonoBehaviour.print("SteamUser.GetDurationControl() : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("BSetDurationControlOnlineState(EDurationControlOnlineState.k_EDurationControlOnlineState_Offline)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamUser.BSetDurationControlOnlineState(EDurationControlOnlineState.k_EDurationControlOnlineState_Offline).ToString(), str0: "SteamUser.BSetDurationControlOnlineState(", str1: EDurationControlOnlineState.k_EDurationControlOnlineState_Offline.ToString(), str2: ") : "));
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnSteamServersConnected(SteamServersConnected_t pCallback)
	{
		Debug.Log("[" + 101 + " - SteamServersConnected]");
	}

	private void OnSteamServerConnectFailure(SteamServerConnectFailure_t pCallback)
	{
		Debug.Log("[" + 102 + " - SteamServerConnectFailure] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_bStillRetrying);
	}

	private void OnSteamServersDisconnected(SteamServersDisconnected_t pCallback)
	{
		Debug.Log("[" + 103 + " - SteamServersDisconnected] - " + pCallback.m_eResult);
	}

	private void OnClientGameServerDeny(ClientGameServerDeny_t pCallback)
	{
		Debug.Log("[" + 113 + " - ClientGameServerDeny] - " + pCallback.m_uAppID + " -- " + pCallback.m_unGameServerIP + " -- " + pCallback.m_usGameServerPort + " -- " + pCallback.m_bSecure + " -- " + pCallback.m_uReason);
	}

	private void OnIPCFailure(IPCFailure_t pCallback)
	{
		Debug.Log("[" + 117 + " - IPCFailure] - " + pCallback.m_eFailureType);
	}

	private void OnLicensesUpdated(LicensesUpdated_t pCallback)
	{
		Debug.Log("[" + 125 + " - LicensesUpdated]");
	}

	private void OnValidateAuthTicketResponse(ValidateAuthTicketResponse_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			143.ToString(),
			" - ValidateAuthTicketResponse] - ",
			null,
			null,
			null,
			null,
			null
		};
		CSteamID steamID = pCallback.m_SteamID;
		obj[3] = steamID.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eAuthSessionResponse.ToString();
		obj[6] = " -- ";
		steamID = pCallback.m_OwnerSteamID;
		obj[7] = steamID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t pCallback)
	{
		Debug.Log("[" + 152 + " - MicroTxnAuthorizationResponse] - " + pCallback.m_unAppID + " -- " + pCallback.m_ulOrderID + " -- " + pCallback.m_bAuthorized);
	}

	private void OnEncryptedAppTicketResponse(EncryptedAppTicketResponse_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 154 + " - EncryptedAppTicketResponse] - " + pCallback.m_eResult);
		if (pCallback.m_eResult != EResult.k_EResultOK)
		{
			return;
		}
		byte[] array = new byte[1024];
		SteamUser.GetEncryptedAppTicket(array, 1024, out var pcbTicket);
		byte[] array2 = new byte[32]
		{
			237, 147, 134, 7, 54, 71, 206, 165, 139, 119,
			33, 73, 13, 89, 237, 68, 87, 35, 240, 246,
			110, 116, 20, 225, 83, 59, 163, 60, 216, 3,
			189, 189
		};
		byte[] rgubTicketDecrypted = new byte[1024];
		uint pcubTicketDecrypted = 1024u;
		if (!SteamEncryptedAppTicket.BDecryptTicket(array, pcbTicket, rgubTicketDecrypted, ref pcubTicketDecrypted, array2, array2.Length))
		{
			Debug.Log("Ticket failed to decrypt");
			return;
		}
		if (!SteamEncryptedAppTicket.BIsTicketForApp(rgubTicketDecrypted, pcubTicketDecrypted, SteamUtils.GetAppID()))
		{
			Debug.Log("Ticket for wrong app id");
		}
		SteamEncryptedAppTicket.GetTicketSteamID(rgubTicketDecrypted, pcubTicketDecrypted, out var psteamID);
		if (psteamID != SteamUser.GetSteamID())
		{
			Debug.Log("Ticket for wrong user");
		}
		uint pcubUserData;
		byte[] userVariableData = SteamEncryptedAppTicket.GetUserVariableData(rgubTicketDecrypted, pcubTicketDecrypted, out pcubUserData);
		if (pcubUserData != 4)
		{
			Debug.Log("Secret data size is wrong.");
		}
		Debug.Log(userVariableData.Length);
		Debug.Log(BitConverter.ToUInt32(userVariableData, 0));
		if (BitConverter.ToUInt32(userVariableData, 0) != 21572)
		{
			Debug.Log("Failed to retrieve secret data");
		}
		else
		{
			Debug.Log("Successfully retrieved Encrypted App Ticket");
		}
	}

	private void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			163.ToString(),
			" - GetAuthSessionTicketResponse] - ",
			null,
			null,
			null
		};
		HAuthTicket hAuthTicket = pCallback.m_hAuthTicket;
		obj[3] = hAuthTicket.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eResult.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnGameWebCallback(GameWebCallback_t pCallback)
	{
		Debug.Log("[" + 164 + " - GameWebCallback] - " + pCallback.m_szURL);
	}

	private void OnStoreAuthURLResponse(StoreAuthURLResponse_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 165 + " - StoreAuthURLResponse] - " + pCallback.m_szURL);
	}

	private void OnMarketEligibilityResponse(MarketEligibilityResponse_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[12]
		{
			"[",
			166.ToString(),
			" - MarketEligibilityResponse] - ",
			pCallback.m_bAllowed.ToString(),
			" -- ",
			pCallback.m_eNotAllowedReason.ToString(),
			" -- ",
			null,
			null,
			null,
			null,
			null
		};
		RTime32 rtAllowedAtTime = pCallback.m_rtAllowedAtTime;
		obj[7] = rtAllowedAtTime.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_cdaySteamGuardRequiredDays.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_cdayNewDeviceCooldown.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnDurationControl(DurationControl_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[18]
		{
			"[",
			167.ToString(),
			" - DurationControl] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
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
		AppId_t appid = pCallback.m_appid;
		obj[5] = appid.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bApplicable.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_csecsLast5h.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_progress.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.m_notification.ToString();
		obj[14] = " -- ";
		obj[15] = pCallback.m_csecsToday.ToString();
		obj[16] = " -- ";
		obj[17] = pCallback.m_csecsRemaining.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnGetTicketForWebApiResponse(GetTicketForWebApiResponse_t pCallback)
	{
		string[] obj = new string[10]
		{
			"[",
			168.ToString(),
			" - GetTicketForWebApiResponse] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		HAuthTicket hAuthTicket = pCallback.m_hAuthTicket;
		obj[3] = hAuthTicket.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eResult.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_cubTicket.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_rgubTicket?.ToString();
		Debug.Log(string.Concat(obj));
	}
}
