using Steamworks;
using UnityEngine;

public class SteamPartiesTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	private uint m_BeaconIndex;

	private PartyBeaconID_t m_PartyBeaconID;

	private uint m_NumLocations;

	private SteamPartyBeaconLocation_t[] m_BeaconLocationList;

	private CSteamID m_OtherUsersSteamID;

	protected Callback<ReservationNotificationCallback_t> m_ReservationNotificationCallback;

	protected Callback<AvailableBeaconLocationsUpdated_t> m_AvailableBeaconLocationsUpdated;

	protected Callback<ActiveBeaconsUpdated_t> m_ActiveBeaconsUpdated;

	private CallResult<JoinPartyCallback_t> OnJoinPartyCallbackCallResult;

	private CallResult<CreateBeaconCallback_t> OnCreateBeaconCallbackCallResult;

	private CallResult<ChangeNumOpenSlotsCallback_t> OnChangeNumOpenSlotsCallbackCallResult;

	public void OnEnable()
	{
		m_BeaconIndex = 0u;
		m_NumLocations = 0u;
		m_PartyBeaconID = PartyBeaconID_t.Invalid;
		m_OtherUsersSteamID = CSteamID.Nil;
		m_ReservationNotificationCallback = Callback<ReservationNotificationCallback_t>.Create(OnReservationNotificationCallback);
		m_AvailableBeaconLocationsUpdated = Callback<AvailableBeaconLocationsUpdated_t>.Create(OnAvailableBeaconLocationsUpdated);
		m_ActiveBeaconsUpdated = Callback<ActiveBeaconsUpdated_t>.Create(OnActiveBeaconsUpdated);
		OnJoinPartyCallbackCallResult = CallResult<JoinPartyCallback_t>.Create(OnJoinPartyCallback);
		OnCreateBeaconCallbackCallResult = CallResult<CreateBeaconCallback_t>.Create(OnCreateBeaconCallback);
		OnChangeNumOpenSlotsCallbackCallResult = CallResult<ChangeNumOpenSlotsCallback_t>.Create(OnChangeNumOpenSlotsCallback);
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_BeaconIndex: " + m_BeaconIndex);
		PartyBeaconID_t partyBeaconID = m_PartyBeaconID;
		GUILayout.Label("m_PartyBeaconID: " + partyBeaconID.ToString());
		GUILayout.Label("m_NumLocations: " + m_NumLocations);
		GUILayout.Label("m_BeaconLocationList: " + m_BeaconLocationList);
		CSteamID otherUsersSteamID = m_OtherUsersSteamID;
		GUILayout.Label("m_OtherUsersSteamID: " + otherUsersSteamID.ToString());
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		GUILayout.Label("GetNumActiveBeacons() : " + SteamParties.GetNumActiveBeacons());
		if (GUILayout.Button("GetBeaconByIndex(m_BeaconIndex)"))
		{
			m_PartyBeaconID = SteamParties.GetBeaconByIndex(m_BeaconIndex);
			string text = m_BeaconIndex.ToString();
			partyBeaconID = m_PartyBeaconID;
			MonoBehaviour.print("SteamParties.GetBeaconByIndex(" + text + ") : " + partyBeaconID.ToString());
		}
		if (GUILayout.Button("GetBeaconDetails(m_PartyBeaconID, out m_OtherUsersSteamID, out m_BeaconLocationList[0], out Metadata, 1024)"))
		{
			m_BeaconLocationList = new SteamPartyBeaconLocation_t[1];
			string pchMetadata;
			bool beaconDetails = SteamParties.GetBeaconDetails(m_PartyBeaconID, out m_OtherUsersSteamID, out m_BeaconLocationList[0], out pchMetadata, 1024);
			string[] obj = new string[12]
			{
				"SteamParties.GetBeaconDetails(", null, null, null, null, null, null, null, null, null,
				null, null
			};
			partyBeaconID = m_PartyBeaconID;
			obj[1] = partyBeaconID.ToString();
			obj[2] = ", out m_OtherUsersSteamID, out m_BeaconLocationList[0], out Metadata, ";
			obj[3] = 1024.ToString();
			obj[4] = ") : ";
			obj[5] = beaconDetails.ToString();
			obj[6] = " -- ";
			otherUsersSteamID = m_OtherUsersSteamID;
			obj[7] = otherUsersSteamID.ToString();
			obj[8] = " -- ";
			obj[9] = m_BeaconLocationList[0].ToString();
			obj[10] = " -- ";
			obj[11] = pchMetadata;
			MonoBehaviour.print(string.Concat(obj));
		}
		if (GUILayout.Button("JoinParty(m_PartyBeaconID)"))
		{
			SteamAPICall_t steamAPICall_t = SteamParties.JoinParty(m_PartyBeaconID);
			OnJoinPartyCallbackCallResult.Set(steamAPICall_t);
			partyBeaconID = m_PartyBeaconID;
			string text2 = partyBeaconID.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
			MonoBehaviour.print("SteamParties.JoinParty(" + text2 + ") : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("GetNumAvailableBeaconLocations(out m_NumLocations)"))
		{
			MonoBehaviour.print("SteamParties.GetNumAvailableBeaconLocations(out m_NumLocations) : " + SteamParties.GetNumAvailableBeaconLocations(out m_NumLocations) + " -- " + m_NumLocations);
		}
		if (GUILayout.Button("GetAvailableBeaconLocations(m_BeaconLocationList, m_NumLocations)"))
		{
			m_BeaconLocationList = new SteamPartyBeaconLocation_t[m_NumLocations];
			bool availableBeaconLocations = SteamParties.GetAvailableBeaconLocations(m_BeaconLocationList, m_NumLocations);
			MonoBehaviour.print("SteamParties.GetAvailableBeaconLocations(" + m_BeaconLocationList?.ToString() + ", " + m_NumLocations + ") : " + availableBeaconLocations);
		}
		if (GUILayout.Button("CreateBeacon(1, ref m_BeaconLocationList[0], \"TestConnectString\", \"TestMetadata\")"))
		{
			SteamAPICall_t steamAPICall_t3 = SteamParties.CreateBeacon(1u, ref m_BeaconLocationList[0], "TestConnectString", "TestMetadata");
			OnCreateBeaconCallbackCallResult.Set(steamAPICall_t3);
			string[] obj2 = new string[6]
			{
				"SteamParties.CreateBeacon(",
				1.ToString(),
				", ref m_BeaconLocationList[0], \"TestConnectString\", \"TestMetadata\") : ",
				null,
				null,
				null
			};
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t3;
			obj2[3] = steamAPICall_t2.ToString();
			obj2[4] = " -- ";
			obj2[5] = m_BeaconLocationList[0].ToString();
			MonoBehaviour.print(string.Concat(obj2));
		}
		if (GUILayout.Button("OnReservationCompleted(m_PartyBeaconID, m_OtherUsersSteamID)"))
		{
			SteamParties.OnReservationCompleted(m_PartyBeaconID, m_OtherUsersSteamID);
			string[] obj3 = new string[5] { "SteamParties.OnReservationCompleted(", null, null, null, null };
			partyBeaconID = m_PartyBeaconID;
			obj3[1] = partyBeaconID.ToString();
			obj3[2] = ", ";
			otherUsersSteamID = m_OtherUsersSteamID;
			obj3[3] = otherUsersSteamID.ToString();
			obj3[4] = ")";
			MonoBehaviour.print(string.Concat(obj3));
		}
		if (GUILayout.Button("CancelReservation(m_PartyBeaconID, m_OtherUsersSteamID)"))
		{
			SteamParties.CancelReservation(m_PartyBeaconID, m_OtherUsersSteamID);
			string[] obj4 = new string[5] { "SteamParties.CancelReservation(", null, null, null, null };
			partyBeaconID = m_PartyBeaconID;
			obj4[1] = partyBeaconID.ToString();
			obj4[2] = ", ";
			otherUsersSteamID = m_OtherUsersSteamID;
			obj4[3] = otherUsersSteamID.ToString();
			obj4[4] = ")";
			MonoBehaviour.print(string.Concat(obj4));
		}
		if (GUILayout.Button("ChangeNumOpenSlots(m_PartyBeaconID, 2)"))
		{
			SteamAPICall_t steamAPICall_t4 = SteamParties.ChangeNumOpenSlots(m_PartyBeaconID, 2u);
			OnChangeNumOpenSlotsCallbackCallResult.Set(steamAPICall_t4);
			string[] obj5 = new string[6] { "SteamParties.ChangeNumOpenSlots(", null, null, null, null, null };
			partyBeaconID = m_PartyBeaconID;
			obj5[1] = partyBeaconID.ToString();
			obj5[2] = ", ";
			obj5[3] = 2.ToString();
			obj5[4] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t4;
			obj5[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj5));
		}
		if (GUILayout.Button("DestroyBeacon(m_PartyBeaconID)"))
		{
			bool flag = SteamParties.DestroyBeacon(m_PartyBeaconID);
			m_PartyBeaconID = PartyBeaconID_t.Invalid;
			partyBeaconID = m_PartyBeaconID;
			MonoBehaviour.print("SteamParties.DestroyBeacon(" + partyBeaconID.ToString() + ") : " + flag);
		}
		if (GUILayout.Button("GetBeaconLocationData(m_BeaconLocationList[0], ESteamPartyBeaconLocationData.k_ESteamPartyBeaconLocationDataName, out DataString, 1024)"))
		{
			string pchDataStringOut;
			bool beaconLocationData = SteamParties.GetBeaconLocationData(m_BeaconLocationList[0], ESteamPartyBeaconLocationData.k_ESteamPartyBeaconLocationDataName, out pchDataStringOut, 1024);
			MonoBehaviour.print("SteamParties.GetBeaconLocationData(" + m_BeaconLocationList[0].ToString() + ", " + ESteamPartyBeaconLocationData.k_ESteamPartyBeaconLocationDataName.ToString() + ", out DataString, " + 1024 + ") : " + beaconLocationData + " -- " + pchDataStringOut);
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnJoinPartyCallback(JoinPartyCallback_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[10]
		{
			"[",
			5301.ToString(),
			" - JoinPartyCallback] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null,
			null,
			null
		};
		PartyBeaconID_t ulBeaconID = pCallback.m_ulBeaconID;
		obj[5] = ulBeaconID.ToString();
		obj[6] = " -- ";
		CSteamID steamIDBeaconOwner = pCallback.m_SteamIDBeaconOwner;
		obj[7] = steamIDBeaconOwner.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_rgchConnectString;
		Debug.Log(string.Concat(obj));
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			m_PartyBeaconID = pCallback.m_ulBeaconID;
			m_OtherUsersSteamID = pCallback.m_SteamIDBeaconOwner;
		}
	}

	private void OnCreateBeaconCallback(CreateBeaconCallback_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[6]
		{
			"[",
			5302.ToString(),
			" - CreateBeaconCallback] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null
		};
		PartyBeaconID_t ulBeaconID = pCallback.m_ulBeaconID;
		obj[5] = ulBeaconID.ToString();
		Debug.Log(string.Concat(obj));
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			m_PartyBeaconID = pCallback.m_ulBeaconID;
		}
	}

	private void OnReservationNotificationCallback(ReservationNotificationCallback_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			5303.ToString(),
			" - ReservationNotificationCallback] - ",
			null,
			null,
			null
		};
		PartyBeaconID_t ulBeaconID = pCallback.m_ulBeaconID;
		obj[3] = ulBeaconID.ToString();
		obj[4] = " -- ";
		CSteamID steamIDJoiner = pCallback.m_steamIDJoiner;
		obj[5] = steamIDJoiner.ToString();
		Debug.Log(string.Concat(obj));
		m_PartyBeaconID = pCallback.m_ulBeaconID;
		m_OtherUsersSteamID = pCallback.m_steamIDJoiner;
	}

	private void OnChangeNumOpenSlotsCallback(ChangeNumOpenSlotsCallback_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 5304 + " - ChangeNumOpenSlotsCallback] - " + pCallback.m_eResult);
	}

	private void OnAvailableBeaconLocationsUpdated(AvailableBeaconLocationsUpdated_t pCallback)
	{
		Debug.Log("[" + 5305 + " - AvailableBeaconLocationsUpdated]");
		bool numAvailableBeaconLocations = SteamParties.GetNumAvailableBeaconLocations(out m_NumLocations);
		MonoBehaviour.print("SteamParties.GetNumAvailableBeaconLocations(out m_NumLocations) : " + numAvailableBeaconLocations + " -- " + m_NumLocations);
		m_BeaconLocationList = new SteamPartyBeaconLocation_t[m_NumLocations];
		SteamParties.GetAvailableBeaconLocations(m_BeaconLocationList, m_NumLocations);
		MonoBehaviour.print("SteamParties.GetAvailableBeaconLocations(" + m_BeaconLocationList?.ToString() + ", " + m_NumLocations + ") : " + numAvailableBeaconLocations);
	}

	private void OnActiveBeaconsUpdated(ActiveBeaconsUpdated_t pCallback)
	{
		Debug.Log("[" + 5306 + " - ActiveBeaconsUpdated]");
	}
}
