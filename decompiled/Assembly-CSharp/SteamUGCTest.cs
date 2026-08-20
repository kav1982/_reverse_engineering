using Steamworks;
using UnityEngine;

public class SteamUGCTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	private UGCQueryHandle_t m_UGCQueryHandle;

	private PublishedFileId_t m_PublishedFileId;

	private UGCUpdateHandle_t m_UGCUpdateHandle;

	protected Callback<ItemInstalled_t> m_ItemInstalled;

	protected Callback<DownloadItemResult_t> m_DownloadItemResult;

	protected Callback<UserSubscribedItemsListChanged_t> m_UserSubscribedItemsListChanged;

	private CallResult<SteamUGCQueryCompleted_t> OnSteamUGCQueryCompletedCallResult;

	private CallResult<SteamUGCRequestUGCDetailsResult_t> OnSteamUGCRequestUGCDetailsResultCallResult;

	private CallResult<CreateItemResult_t> OnCreateItemResultCallResult;

	private CallResult<SubmitItemUpdateResult_t> OnSubmitItemUpdateResultCallResult;

	private CallResult<UserFavoriteItemsListChanged_t> OnUserFavoriteItemsListChangedCallResult;

	private CallResult<SetUserItemVoteResult_t> OnSetUserItemVoteResultCallResult;

	private CallResult<GetUserItemVoteResult_t> OnGetUserItemVoteResultCallResult;

	private CallResult<StartPlaytimeTrackingResult_t> OnStartPlaytimeTrackingResultCallResult;

	private CallResult<StopPlaytimeTrackingResult_t> OnStopPlaytimeTrackingResultCallResult;

	private CallResult<AddUGCDependencyResult_t> OnAddUGCDependencyResultCallResult;

	private CallResult<RemoveUGCDependencyResult_t> OnRemoveUGCDependencyResultCallResult;

	private CallResult<AddAppDependencyResult_t> OnAddAppDependencyResultCallResult;

	private CallResult<RemoveAppDependencyResult_t> OnRemoveAppDependencyResultCallResult;

	private CallResult<GetAppDependenciesResult_t> OnGetAppDependenciesResultCallResult;

	private CallResult<DeleteItemResult_t> OnDeleteItemResultCallResult;

	private CallResult<WorkshopEULAStatus_t> OnWorkshopEULAStatusCallResult;

	private CallResult<RemoteStorageSubscribePublishedFileResult_t> OnRemoteStorageSubscribePublishedFileResultCallResult;

	private CallResult<RemoteStorageUnsubscribePublishedFileResult_t> OnRemoteStorageUnsubscribePublishedFileResultCallResult;

	public void OnEnable()
	{
		OnRemoteStorageSubscribePublishedFileResultCallResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create(OnRemoteStorageSubscribePublishedFileResult);
		OnRemoteStorageUnsubscribePublishedFileResultCallResult = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create(OnRemoteStorageUnsubscribePublishedFileResult);
		m_ItemInstalled = Callback<ItemInstalled_t>.Create(OnItemInstalled);
		m_DownloadItemResult = Callback<DownloadItemResult_t>.Create(OnDownloadItemResult);
		m_UserSubscribedItemsListChanged = Callback<UserSubscribedItemsListChanged_t>.Create(OnUserSubscribedItemsListChanged);
		OnSteamUGCQueryCompletedCallResult = CallResult<SteamUGCQueryCompleted_t>.Create(OnSteamUGCQueryCompleted);
		OnSteamUGCRequestUGCDetailsResultCallResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(OnSteamUGCRequestUGCDetailsResult);
		OnCreateItemResultCallResult = CallResult<CreateItemResult_t>.Create(OnCreateItemResult);
		OnSubmitItemUpdateResultCallResult = CallResult<SubmitItemUpdateResult_t>.Create(OnSubmitItemUpdateResult);
		OnUserFavoriteItemsListChangedCallResult = CallResult<UserFavoriteItemsListChanged_t>.Create(OnUserFavoriteItemsListChanged);
		OnSetUserItemVoteResultCallResult = CallResult<SetUserItemVoteResult_t>.Create(OnSetUserItemVoteResult);
		OnGetUserItemVoteResultCallResult = CallResult<GetUserItemVoteResult_t>.Create(OnGetUserItemVoteResult);
		OnStartPlaytimeTrackingResultCallResult = CallResult<StartPlaytimeTrackingResult_t>.Create(OnStartPlaytimeTrackingResult);
		OnStopPlaytimeTrackingResultCallResult = CallResult<StopPlaytimeTrackingResult_t>.Create(OnStopPlaytimeTrackingResult);
		OnAddUGCDependencyResultCallResult = CallResult<AddUGCDependencyResult_t>.Create(OnAddUGCDependencyResult);
		OnRemoveUGCDependencyResultCallResult = CallResult<RemoveUGCDependencyResult_t>.Create(OnRemoveUGCDependencyResult);
		OnAddAppDependencyResultCallResult = CallResult<AddAppDependencyResult_t>.Create(OnAddAppDependencyResult);
		OnRemoveAppDependencyResultCallResult = CallResult<RemoveAppDependencyResult_t>.Create(OnRemoveAppDependencyResult);
		OnGetAppDependenciesResultCallResult = CallResult<GetAppDependenciesResult_t>.Create(OnGetAppDependenciesResult);
		OnDeleteItemResultCallResult = CallResult<DeleteItemResult_t>.Create(OnDeleteItemResult);
		OnWorkshopEULAStatusCallResult = CallResult<WorkshopEULAStatus_t>.Create(OnWorkshopEULAStatus);
	}

	private void OnRemoteStorageSubscribePublishedFileResult(RemoteStorageSubscribePublishedFileResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[6]
		{
			"[",
			1313.ToString(),
			" - RemoteStorageSubscribePublishedFileResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		Debug.Log(string.Concat(obj));
		m_PublishedFileId = pCallback.m_nPublishedFileId;
	}

	private void OnRemoteStorageUnsubscribePublishedFileResult(RemoteStorageUnsubscribePublishedFileResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[6]
		{
			"[",
			1315.ToString(),
			" - RemoteStorageUnsubscribePublishedFileResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		Debug.Log(string.Concat(obj));
		m_PublishedFileId = pCallback.m_nPublishedFileId;
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		UGCQueryHandle_t uGCQueryHandle = m_UGCQueryHandle;
		GUILayout.Label("m_UGCQueryHandle: " + uGCQueryHandle.ToString());
		PublishedFileId_t publishedFileId = m_PublishedFileId;
		GUILayout.Label("m_PublishedFileId: " + publishedFileId.ToString());
		UGCUpdateHandle_t uGCUpdateHandle = m_UGCUpdateHandle;
		GUILayout.Label("m_UGCUpdateHandle: " + uGCUpdateHandle.ToString());
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		if (GUILayout.Button("CreateQueryUserUGCRequest(SteamUser.GetSteamID().GetAccountID(), EUserUGCList.k_EUserUGCList_Published, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Screenshots, EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderDesc, AppId_t.Invalid, SteamUtils.GetAppID(), 1)"))
		{
			m_UGCQueryHandle = SteamUGC.CreateQueryUserUGCRequest(SteamUser.GetSteamID().GetAccountID(), EUserUGCList.k_EUserUGCList_Published, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Screenshots, EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderDesc, AppId_t.Invalid, SteamUtils.GetAppID(), 1u);
			string[] obj = new string[16]
			{
				"SteamUGC.CreateQueryUserUGCRequest(",
				SteamUser.GetSteamID().GetAccountID().ToString(),
				", ",
				EUserUGCList.k_EUserUGCList_Published.ToString(),
				", ",
				EUGCMatchingUGCType.k_EUGCMatchingUGCType_Screenshots.ToString(),
				", ",
				EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderDesc.ToString(),
				", ",
				null,
				null,
				null,
				null,
				null,
				null,
				null
			};
			AppId_t invalid = AppId_t.Invalid;
			obj[9] = invalid.ToString();
			obj[10] = ", ";
			obj[11] = SteamUtils.GetAppID().ToString();
			obj[12] = ", ";
			obj[13] = 1.ToString();
			obj[14] = ") : ";
			uGCQueryHandle = m_UGCQueryHandle;
			obj[15] = uGCQueryHandle.ToString();
			MonoBehaviour.print(string.Concat(obj));
		}
		if (GUILayout.Button("CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByPublicationDate, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items, AppId_t.Invalid, SteamUtils.GetAppID(), 1)"))
		{
			m_UGCQueryHandle = SteamUGC.CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByPublicationDate, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items, AppId_t.Invalid, SteamUtils.GetAppID(), 1u);
			string[] obj2 = new string[12]
			{
				"SteamUGC.CreateQueryAllUGCRequest(",
				EUGCQuery.k_EUGCQuery_RankedByPublicationDate.ToString(),
				", ",
				EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items.ToString(),
				", ",
				null,
				null,
				null,
				null,
				null,
				null,
				null
			};
			AppId_t invalid = AppId_t.Invalid;
			obj2[5] = invalid.ToString();
			obj2[6] = ", ";
			obj2[7] = SteamUtils.GetAppID().ToString();
			obj2[8] = ", ";
			obj2[9] = 1.ToString();
			obj2[10] = ") : ";
			uGCQueryHandle = m_UGCQueryHandle;
			obj2[11] = uGCQueryHandle.ToString();
			MonoBehaviour.print(string.Concat(obj2));
		}
		if (GUILayout.Button("CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByPublicationDate, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items, AppId_t.Invalid, SteamUtils.GetAppID(), null)"))
		{
			m_UGCQueryHandle = SteamUGC.CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByPublicationDate, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items, AppId_t.Invalid, SteamUtils.GetAppID());
			string[] obj3 = new string[10]
			{
				"SteamUGC.CreateQueryAllUGCRequest(",
				EUGCQuery.k_EUGCQuery_RankedByPublicationDate.ToString(),
				", ",
				EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items.ToString(),
				", ",
				null,
				null,
				null,
				null,
				null
			};
			AppId_t invalid = AppId_t.Invalid;
			obj3[5] = invalid.ToString();
			obj3[6] = ", ";
			obj3[7] = SteamUtils.GetAppID().ToString();
			obj3[8] = ", ) : ";
			uGCQueryHandle = m_UGCQueryHandle;
			obj3[9] = uGCQueryHandle.ToString();
			MonoBehaviour.print(string.Concat(obj3));
		}
		if (GUILayout.Button("CreateQueryUGCDetailsRequest(PublishedFileIDs, (uint)PublishedFileIDs.Length)"))
		{
			PublishedFileId_t[] array = new PublishedFileId_t[1] { TestConstants.Instance.k_PublishedFileId_Champions };
			m_UGCQueryHandle = SteamUGC.CreateQueryUGCDetailsRequest(array, (uint)array.Length);
			string[] obj4 = new string[6]
			{
				"SteamUGC.CreateQueryUGCDetailsRequest(",
				array?.ToString(),
				", ",
				((uint)array.Length).ToString(),
				") : ",
				null
			};
			uGCQueryHandle = m_UGCQueryHandle;
			obj4[5] = uGCQueryHandle.ToString();
			MonoBehaviour.print(string.Concat(obj4));
		}
		if (GUILayout.Button("SendQueryUGCRequest(m_UGCQueryHandle)"))
		{
			SteamAPICall_t steamAPICall_t = SteamUGC.SendQueryUGCRequest(m_UGCQueryHandle);
			OnSteamUGCQueryCompletedCallResult.Set(steamAPICall_t);
			uGCQueryHandle = m_UGCQueryHandle;
			string text = uGCQueryHandle.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t;
			MonoBehaviour.print("SteamUGC.SendQueryUGCRequest(" + text + ") : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("GetQueryUGCResult(m_UGCQueryHandle, 0, out Details)"))
		{
			SteamUGCDetails_t pDetails;
			bool queryUGCResult = SteamUGC.GetQueryUGCResult(m_UGCQueryHandle, 0u, out pDetails);
			string[] array2 = new string[51];
			publishedFileId = pDetails.m_nPublishedFileId;
			array2[0] = publishedFileId.ToString();
			array2[1] = " -- ";
			array2[2] = pDetails.m_eResult.ToString();
			array2[3] = " -- ";
			array2[4] = pDetails.m_eFileType.ToString();
			array2[5] = " -- ";
			AppId_t invalid = pDetails.m_nCreatorAppID;
			array2[6] = invalid.ToString();
			array2[7] = " -- ";
			invalid = pDetails.m_nConsumerAppID;
			array2[8] = invalid.ToString();
			array2[9] = " -- ";
			array2[10] = pDetails.m_rgchTitle;
			array2[11] = " -- ";
			array2[12] = pDetails.m_rgchDescription;
			array2[13] = " -- ";
			array2[14] = pDetails.m_ulSteamIDOwner.ToString();
			array2[15] = " -- ";
			array2[16] = pDetails.m_rtimeCreated.ToString();
			array2[17] = " -- ";
			array2[18] = pDetails.m_rtimeUpdated.ToString();
			array2[19] = " -- ";
			array2[20] = pDetails.m_rtimeAddedToUserList.ToString();
			array2[21] = " -- ";
			array2[22] = pDetails.m_eVisibility.ToString();
			array2[23] = " -- ";
			array2[24] = pDetails.m_bBanned.ToString();
			array2[25] = " -- ";
			array2[26] = pDetails.m_bAcceptedForUse.ToString();
			array2[27] = " -- ";
			array2[28] = pDetails.m_bTagsTruncated.ToString();
			array2[29] = " -- ";
			array2[30] = pDetails.m_rgchTags;
			array2[31] = " -- ";
			UGCHandle_t hFile = pDetails.m_hFile;
			array2[32] = hFile.ToString();
			array2[33] = " -- ";
			hFile = pDetails.m_hPreviewFile;
			array2[34] = hFile.ToString();
			array2[35] = " -- ";
			array2[36] = pDetails.m_pchFileName;
			array2[37] = " -- ";
			array2[38] = pDetails.m_nFileSize.ToString();
			array2[39] = " -- ";
			array2[40] = pDetails.m_nPreviewFileSize.ToString();
			array2[41] = " -- ";
			array2[42] = pDetails.m_rgchURL;
			array2[43] = " -- ";
			array2[44] = pDetails.m_unVotesUp.ToString();
			array2[45] = " -- ";
			array2[46] = pDetails.m_unVotesDown.ToString();
			array2[47] = " -- ";
			array2[48] = pDetails.m_flScore.ToString();
			array2[49] = " -- ";
			array2[50] = pDetails.m_unNumChildren.ToString();
			MonoBehaviour.print(string.Concat(array2));
			string[] obj5 = new string[8] { "SteamUGC.GetQueryUGCResult(", null, null, null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj5[1] = uGCQueryHandle.ToString();
			obj5[2] = ", ";
			obj5[3] = 0.ToString();
			obj5[4] = ", out Details) : ";
			obj5[5] = queryUGCResult.ToString();
			obj5[6] = " -- ";
			obj5[7] = pDetails.ToString();
			MonoBehaviour.print(string.Concat(obj5));
		}
		if (GUILayout.Button("GetQueryUGCNumTags(m_UGCQueryHandle, 0)"))
		{
			uint queryUGCNumTags = SteamUGC.GetQueryUGCNumTags(m_UGCQueryHandle, 0u);
			string[] obj6 = new string[6] { "SteamUGC.GetQueryUGCNumTags(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj6[1] = uGCQueryHandle.ToString();
			obj6[2] = ", ";
			obj6[3] = 0.ToString();
			obj6[4] = ") : ";
			obj6[5] = queryUGCNumTags.ToString();
			MonoBehaviour.print(string.Concat(obj6));
		}
		if (GUILayout.Button("GetQueryUGCTag(m_UGCQueryHandle, 0, 0, out Tag, 1024)"))
		{
			string pchValue;
			bool queryUGCTag = SteamUGC.GetQueryUGCTag(m_UGCQueryHandle, 0u, 0u, out pchValue, 1024u);
			string[] obj7 = new string[12]
			{
				"SteamUGC.GetQueryUGCTag(", null, null, null, null, null, null, null, null, null,
				null, null
			};
			uGCQueryHandle = m_UGCQueryHandle;
			obj7[1] = uGCQueryHandle.ToString();
			obj7[2] = ", ";
			obj7[3] = 0.ToString();
			obj7[4] = ", ";
			obj7[5] = 0.ToString();
			obj7[6] = ", out Tag, ";
			obj7[7] = 1024.ToString();
			obj7[8] = ") : ";
			obj7[9] = queryUGCTag.ToString();
			obj7[10] = " -- ";
			obj7[11] = pchValue;
			MonoBehaviour.print(string.Concat(obj7));
		}
		if (GUILayout.Button("GetQueryUGCTagDisplayName(m_UGCQueryHandle, 0, 0, out DisplayName, 1024)"))
		{
			string pchValue2;
			bool queryUGCTagDisplayName = SteamUGC.GetQueryUGCTagDisplayName(m_UGCQueryHandle, 0u, 0u, out pchValue2, 1024u);
			string[] obj8 = new string[12]
			{
				"SteamUGC.GetQueryUGCTagDisplayName(", null, null, null, null, null, null, null, null, null,
				null, null
			};
			uGCQueryHandle = m_UGCQueryHandle;
			obj8[1] = uGCQueryHandle.ToString();
			obj8[2] = ", ";
			obj8[3] = 0.ToString();
			obj8[4] = ", ";
			obj8[5] = 0.ToString();
			obj8[6] = ", out DisplayName, ";
			obj8[7] = 1024.ToString();
			obj8[8] = ") : ";
			obj8[9] = queryUGCTagDisplayName.ToString();
			obj8[10] = " -- ";
			obj8[11] = pchValue2;
			MonoBehaviour.print(string.Concat(obj8));
		}
		if (GUILayout.Button("GetQueryUGCPreviewURL(m_UGCQueryHandle, 0, out URL, 1024)"))
		{
			string pchURL;
			bool queryUGCPreviewURL = SteamUGC.GetQueryUGCPreviewURL(m_UGCQueryHandle, 0u, out pchURL, 1024u);
			string[] obj9 = new string[10] { "SteamUGC.GetQueryUGCPreviewURL(", null, null, null, null, null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj9[1] = uGCQueryHandle.ToString();
			obj9[2] = ", ";
			obj9[3] = 0.ToString();
			obj9[4] = ", out URL, ";
			obj9[5] = 1024.ToString();
			obj9[6] = ") : ";
			obj9[7] = queryUGCPreviewURL.ToString();
			obj9[8] = " -- ";
			obj9[9] = pchURL;
			MonoBehaviour.print(string.Concat(obj9));
		}
		if (GUILayout.Button("GetQueryUGCMetadata(m_UGCQueryHandle, 0, out Metadata, Constants.k_cchDeveloperMetadataMax)"))
		{
			string pchMetadata;
			bool queryUGCMetadata = SteamUGC.GetQueryUGCMetadata(m_UGCQueryHandle, 0u, out pchMetadata, 5000u);
			string[] obj10 = new string[10] { "SteamUGC.GetQueryUGCMetadata(", null, null, null, null, null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj10[1] = uGCQueryHandle.ToString();
			obj10[2] = ", ";
			obj10[3] = 0.ToString();
			obj10[4] = ", out Metadata, ";
			obj10[5] = 5000.ToString();
			obj10[6] = ") : ";
			obj10[7] = queryUGCMetadata.ToString();
			obj10[8] = " -- ";
			obj10[9] = pchMetadata;
			MonoBehaviour.print(string.Concat(obj10));
		}
		if (GUILayout.Button("GetQueryUGCChildren(m_UGCQueryHandle, 0, PublishedFileIDs, (uint)PublishedFileIDs.Length)"))
		{
			PublishedFileId_t[] array3 = new PublishedFileId_t[1];
			bool queryUGCChildren = SteamUGC.GetQueryUGCChildren(m_UGCQueryHandle, 0u, array3, (uint)array3.Length);
			string[] obj11 = new string[10] { "SteamUGC.GetQueryUGCChildren(", null, null, null, null, null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj11[1] = uGCQueryHandle.ToString();
			obj11[2] = ", ";
			obj11[3] = 0.ToString();
			obj11[4] = ", ";
			obj11[5] = array3?.ToString();
			obj11[6] = ", ";
			obj11[7] = ((uint)array3.Length).ToString();
			obj11[8] = ") : ";
			obj11[9] = queryUGCChildren.ToString();
			MonoBehaviour.print(string.Concat(obj11));
		}
		if (GUILayout.Button("GetQueryUGCStatistic(m_UGCQueryHandle, 0, EItemStatistic.k_EItemStatistic_NumSubscriptions, out StatValue)"))
		{
			ulong pStatValue;
			bool queryUGCStatistic = SteamUGC.GetQueryUGCStatistic(m_UGCQueryHandle, 0u, EItemStatistic.k_EItemStatistic_NumSubscriptions, out pStatValue);
			string[] obj12 = new string[10] { "SteamUGC.GetQueryUGCStatistic(", null, null, null, null, null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj12[1] = uGCQueryHandle.ToString();
			obj12[2] = ", ";
			obj12[3] = 0.ToString();
			obj12[4] = ", ";
			obj12[5] = EItemStatistic.k_EItemStatistic_NumSubscriptions.ToString();
			obj12[6] = ", out StatValue) : ";
			obj12[7] = queryUGCStatistic.ToString();
			obj12[8] = " -- ";
			obj12[9] = pStatValue.ToString();
			MonoBehaviour.print(string.Concat(obj12));
		}
		if (GUILayout.Button("GetQueryUGCNumAdditionalPreviews(m_UGCQueryHandle, 0)"))
		{
			uint queryUGCNumAdditionalPreviews = SteamUGC.GetQueryUGCNumAdditionalPreviews(m_UGCQueryHandle, 0u);
			string[] obj13 = new string[6] { "SteamUGC.GetQueryUGCNumAdditionalPreviews(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj13[1] = uGCQueryHandle.ToString();
			obj13[2] = ", ";
			obj13[3] = 0.ToString();
			obj13[4] = ") : ";
			obj13[5] = queryUGCNumAdditionalPreviews.ToString();
			MonoBehaviour.print(string.Concat(obj13));
		}
		if (GUILayout.Button("GetQueryUGCAdditionalPreview(m_UGCQueryHandle, 0, 0, out pchURLOrVideoID, 1024, out pchOriginalFileName, 260, out pPreviewType)"))
		{
			string pchURLOrVideoID;
			string pchOriginalFileName;
			EItemPreviewType pPreviewType;
			bool queryUGCAdditionalPreview = SteamUGC.GetQueryUGCAdditionalPreview(m_UGCQueryHandle, 0u, 0u, out pchURLOrVideoID, 1024u, out pchOriginalFileName, 260u, out pPreviewType);
			string[] obj14 = new string[18]
			{
				"SteamUGC.GetQueryUGCAdditionalPreview(", null, null, null, null, null, null, null, null, null,
				null, null, null, null, null, null, null, null
			};
			uGCQueryHandle = m_UGCQueryHandle;
			obj14[1] = uGCQueryHandle.ToString();
			obj14[2] = ", ";
			obj14[3] = 0.ToString();
			obj14[4] = ", ";
			obj14[5] = 0.ToString();
			obj14[6] = ", out pchURLOrVideoID, ";
			obj14[7] = 1024.ToString();
			obj14[8] = ", out pchOriginalFileName, ";
			obj14[9] = 260.ToString();
			obj14[10] = ", out pPreviewType) : ";
			obj14[11] = queryUGCAdditionalPreview.ToString();
			obj14[12] = " -- ";
			obj14[13] = pchURLOrVideoID;
			obj14[14] = " -- ";
			obj14[15] = pchOriginalFileName;
			obj14[16] = " -- ";
			obj14[17] = pPreviewType.ToString();
			MonoBehaviour.print(string.Concat(obj14));
		}
		if (GUILayout.Button("GetQueryUGCNumKeyValueTags(m_UGCQueryHandle, 0)"))
		{
			uint queryUGCNumKeyValueTags = SteamUGC.GetQueryUGCNumKeyValueTags(m_UGCQueryHandle, 0u);
			string[] obj15 = new string[6] { "SteamUGC.GetQueryUGCNumKeyValueTags(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj15[1] = uGCQueryHandle.ToString();
			obj15[2] = ", ";
			obj15[3] = 0.ToString();
			obj15[4] = ") : ";
			obj15[5] = queryUGCNumKeyValueTags.ToString();
			MonoBehaviour.print(string.Concat(obj15));
		}
		if (GUILayout.Button("GetQueryUGCKeyValueTag(m_UGCQueryHandle, 0, 0, out Key, 260, out Value, 260)"))
		{
			string pchKey;
			string pchValue3;
			bool queryUGCKeyValueTag = SteamUGC.GetQueryUGCKeyValueTag(m_UGCQueryHandle, 0u, 0u, out pchKey, 260u, out pchValue3, 260u);
			string[] obj16 = new string[16]
			{
				"SteamUGC.GetQueryUGCKeyValueTag(", null, null, null, null, null, null, null, null, null,
				null, null, null, null, null, null
			};
			uGCQueryHandle = m_UGCQueryHandle;
			obj16[1] = uGCQueryHandle.ToString();
			obj16[2] = ", ";
			obj16[3] = 0.ToString();
			obj16[4] = ", ";
			obj16[5] = 0.ToString();
			obj16[6] = ", out Key, ";
			obj16[7] = 260.ToString();
			obj16[8] = ", out Value, ";
			obj16[9] = 260.ToString();
			obj16[10] = ") : ";
			obj16[11] = queryUGCKeyValueTag.ToString();
			obj16[12] = " -- ";
			obj16[13] = pchKey;
			obj16[14] = " -- ";
			obj16[15] = pchValue3;
			MonoBehaviour.print(string.Concat(obj16));
		}
		if (GUILayout.Button("GetQueryUGCKeyValueTag(m_UGCQueryHandle, 0, \"TestKey\", out Value, 260)"))
		{
			string pchValue4;
			bool queryUGCKeyValueTag2 = SteamUGC.GetQueryUGCKeyValueTag(m_UGCQueryHandle, 0u, "TestKey", out pchValue4, 260u);
			string[] obj17 = new string[10] { "SteamUGC.GetQueryUGCKeyValueTag(", null, null, null, null, null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj17[1] = uGCQueryHandle.ToString();
			obj17[2] = ", ";
			obj17[3] = 0.ToString();
			obj17[4] = ", \"TestKey\", out Value, ";
			obj17[5] = 260.ToString();
			obj17[6] = ") : ";
			obj17[7] = queryUGCKeyValueTag2.ToString();
			obj17[8] = " -- ";
			obj17[9] = pchValue4;
			MonoBehaviour.print(string.Concat(obj17));
		}
		if (GUILayout.Button("GetNumSupportedGameVersions(m_UGCQueryHandle, 0)"))
		{
			uint numSupportedGameVersions = SteamUGC.GetNumSupportedGameVersions(m_UGCQueryHandle, 0u);
			string[] obj18 = new string[6] { "SteamUGC.GetNumSupportedGameVersions(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj18[1] = uGCQueryHandle.ToString();
			obj18[2] = ", ";
			obj18[3] = 0.ToString();
			obj18[4] = ") : ";
			obj18[5] = numSupportedGameVersions.ToString();
			MonoBehaviour.print(string.Concat(obj18));
		}
		if (GUILayout.Button("GetSupportedGameVersionData(m_UGCQueryHandle, 0, 0, out pchGameBranchMin, out pchGameBranchMax, 128)"))
		{
			string pchGameBranchMin;
			string pchGameBranchMax;
			bool supportedGameVersionData = SteamUGC.GetSupportedGameVersionData(m_UGCQueryHandle, 0u, 0u, out pchGameBranchMin, out pchGameBranchMax, 128u);
			string[] obj19 = new string[14]
			{
				"SteamUGC.GetSupportedGameVersionData(", null, null, null, null, null, null, null, null, null,
				null, null, null, null
			};
			uGCQueryHandle = m_UGCQueryHandle;
			obj19[1] = uGCQueryHandle.ToString();
			obj19[2] = ", ";
			obj19[3] = 0.ToString();
			obj19[4] = ", ";
			obj19[5] = 0.ToString();
			obj19[6] = ", out pchGameBranchMin, out pchGameBranchMax, ";
			obj19[7] = 128.ToString();
			obj19[8] = ") : ";
			obj19[9] = supportedGameVersionData.ToString();
			obj19[10] = " -- ";
			obj19[11] = pchGameBranchMin;
			obj19[12] = " -- ";
			obj19[13] = pchGameBranchMax;
			MonoBehaviour.print(string.Concat(obj19));
		}
		if (GUILayout.Button("GetQueryUGCContentDescriptors(m_UGCQueryHandle, 0, pvecDescriptors, (uint)pvecDescriptors.Length)"))
		{
			EUGCContentDescriptorID[] array4 = new EUGCContentDescriptorID[100];
			uint queryUGCContentDescriptors = SteamUGC.GetQueryUGCContentDescriptors(m_UGCQueryHandle, 0u, array4, (uint)array4.Length);
			string[] obj20 = new string[10] { "SteamUGC.GetQueryUGCContentDescriptors(", null, null, null, null, null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj20[1] = uGCQueryHandle.ToString();
			obj20[2] = ", ";
			obj20[3] = 0.ToString();
			obj20[4] = ", ";
			obj20[5] = array4?.ToString();
			obj20[6] = ", ";
			obj20[7] = ((uint)array4.Length).ToString();
			obj20[8] = ") : ";
			obj20[9] = queryUGCContentDescriptors.ToString();
			MonoBehaviour.print(string.Concat(obj20));
		}
		if (GUILayout.Button("ReleaseQueryUGCRequest(m_UGCQueryHandle)"))
		{
			bool flag = SteamUGC.ReleaseQueryUGCRequest(m_UGCQueryHandle);
			uGCQueryHandle = m_UGCQueryHandle;
			MonoBehaviour.print("SteamUGC.ReleaseQueryUGCRequest(" + uGCQueryHandle.ToString() + ") : " + flag);
		}
		if (GUILayout.Button("AddRequiredTag(m_UGCQueryHandle, \"Co-op\")"))
		{
			bool flag2 = SteamUGC.AddRequiredTag(m_UGCQueryHandle, "Co-op");
			uGCQueryHandle = m_UGCQueryHandle;
			MonoBehaviour.print("SteamUGC.AddRequiredTag(" + uGCQueryHandle.ToString() + ", \"Co-op\") : " + flag2);
		}
		if (GUILayout.Button("AddRequiredTagGroup(m_UGCQueryHandle, new string[] {\"Sorry\"})"))
		{
			bool flag3 = SteamUGC.AddRequiredTagGroup(m_UGCQueryHandle, new string[1] { "Sorry" });
			string[] obj21 = new string[6] { "SteamUGC.AddRequiredTagGroup(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj21[1] = uGCQueryHandle.ToString();
			obj21[2] = ", ";
			obj21[3] = new string[1] { "Sorry" }?.ToString();
			obj21[4] = ") : ";
			obj21[5] = flag3.ToString();
			MonoBehaviour.print(string.Concat(obj21));
		}
		if (GUILayout.Button("AddExcludedTag(m_UGCQueryHandle, \"Co-op\")"))
		{
			bool flag4 = SteamUGC.AddExcludedTag(m_UGCQueryHandle, "Co-op");
			uGCQueryHandle = m_UGCQueryHandle;
			MonoBehaviour.print("SteamUGC.AddExcludedTag(" + uGCQueryHandle.ToString() + ", \"Co-op\") : " + flag4);
		}
		if (GUILayout.Button("SetReturnOnlyIDs(m_UGCQueryHandle, true)"))
		{
			bool flag5 = SteamUGC.SetReturnOnlyIDs(m_UGCQueryHandle, bReturnOnlyIDs: true);
			string[] obj22 = new string[6] { "SteamUGC.SetReturnOnlyIDs(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj22[1] = uGCQueryHandle.ToString();
			obj22[2] = ", ";
			obj22[3] = true.ToString();
			obj22[4] = ") : ";
			obj22[5] = flag5.ToString();
			MonoBehaviour.print(string.Concat(obj22));
		}
		if (GUILayout.Button("SetReturnKeyValueTags(m_UGCQueryHandle, true)"))
		{
			bool flag6 = SteamUGC.SetReturnKeyValueTags(m_UGCQueryHandle, bReturnKeyValueTags: true);
			string[] obj23 = new string[6] { "SteamUGC.SetReturnKeyValueTags(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj23[1] = uGCQueryHandle.ToString();
			obj23[2] = ", ";
			obj23[3] = true.ToString();
			obj23[4] = ") : ";
			obj23[5] = flag6.ToString();
			MonoBehaviour.print(string.Concat(obj23));
		}
		if (GUILayout.Button("SetReturnLongDescription(m_UGCQueryHandle, true)"))
		{
			bool flag7 = SteamUGC.SetReturnLongDescription(m_UGCQueryHandle, bReturnLongDescription: true);
			string[] obj24 = new string[6] { "SteamUGC.SetReturnLongDescription(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj24[1] = uGCQueryHandle.ToString();
			obj24[2] = ", ";
			obj24[3] = true.ToString();
			obj24[4] = ") : ";
			obj24[5] = flag7.ToString();
			MonoBehaviour.print(string.Concat(obj24));
		}
		if (GUILayout.Button("SetReturnMetadata(m_UGCQueryHandle, true)"))
		{
			bool flag8 = SteamUGC.SetReturnMetadata(m_UGCQueryHandle, bReturnMetadata: true);
			string[] obj25 = new string[6] { "SteamUGC.SetReturnMetadata(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj25[1] = uGCQueryHandle.ToString();
			obj25[2] = ", ";
			obj25[3] = true.ToString();
			obj25[4] = ") : ";
			obj25[5] = flag8.ToString();
			MonoBehaviour.print(string.Concat(obj25));
		}
		if (GUILayout.Button("SetReturnChildren(m_UGCQueryHandle, true)"))
		{
			bool flag9 = SteamUGC.SetReturnChildren(m_UGCQueryHandle, bReturnChildren: true);
			string[] obj26 = new string[6] { "SteamUGC.SetReturnChildren(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj26[1] = uGCQueryHandle.ToString();
			obj26[2] = ", ";
			obj26[3] = true.ToString();
			obj26[4] = ") : ";
			obj26[5] = flag9.ToString();
			MonoBehaviour.print(string.Concat(obj26));
		}
		if (GUILayout.Button("SetReturnAdditionalPreviews(m_UGCQueryHandle, true)"))
		{
			bool flag10 = SteamUGC.SetReturnAdditionalPreviews(m_UGCQueryHandle, bReturnAdditionalPreviews: true);
			string[] obj27 = new string[6] { "SteamUGC.SetReturnAdditionalPreviews(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj27[1] = uGCQueryHandle.ToString();
			obj27[2] = ", ";
			obj27[3] = true.ToString();
			obj27[4] = ") : ";
			obj27[5] = flag10.ToString();
			MonoBehaviour.print(string.Concat(obj27));
		}
		if (GUILayout.Button("SetReturnTotalOnly(m_UGCQueryHandle, true)"))
		{
			bool flag11 = SteamUGC.SetReturnTotalOnly(m_UGCQueryHandle, bReturnTotalOnly: true);
			string[] obj28 = new string[6] { "SteamUGC.SetReturnTotalOnly(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj28[1] = uGCQueryHandle.ToString();
			obj28[2] = ", ";
			obj28[3] = true.ToString();
			obj28[4] = ") : ";
			obj28[5] = flag11.ToString();
			MonoBehaviour.print(string.Concat(obj28));
		}
		if (GUILayout.Button("SetReturnPlaytimeStats(m_UGCQueryHandle, 7)"))
		{
			bool flag12 = SteamUGC.SetReturnPlaytimeStats(m_UGCQueryHandle, 7u);
			string[] obj29 = new string[6] { "SteamUGC.SetReturnPlaytimeStats(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj29[1] = uGCQueryHandle.ToString();
			obj29[2] = ", ";
			obj29[3] = 7.ToString();
			obj29[4] = ") : ";
			obj29[5] = flag12.ToString();
			MonoBehaviour.print(string.Concat(obj29));
		}
		if (GUILayout.Button("SetLanguage(m_UGCQueryHandle, \"english\")"))
		{
			bool flag13 = SteamUGC.SetLanguage(m_UGCQueryHandle, "english");
			uGCQueryHandle = m_UGCQueryHandle;
			MonoBehaviour.print("SteamUGC.SetLanguage(" + uGCQueryHandle.ToString() + ", \"english\") : " + flag13);
		}
		if (GUILayout.Button("SetAllowCachedResponse(m_UGCQueryHandle, 5)"))
		{
			bool flag14 = SteamUGC.SetAllowCachedResponse(m_UGCQueryHandle, 5u);
			string[] obj30 = new string[6] { "SteamUGC.SetAllowCachedResponse(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj30[1] = uGCQueryHandle.ToString();
			obj30[2] = ", ";
			obj30[3] = 5.ToString();
			obj30[4] = ") : ";
			obj30[5] = flag14.ToString();
			MonoBehaviour.print(string.Concat(obj30));
		}
		if (GUILayout.Button("SetAdminQuery(m_UGCUpdateHandle, true)"))
		{
			bool flag15 = SteamUGC.SetAdminQuery(m_UGCUpdateHandle, bAdminQuery: true);
			string[] obj31 = new string[6] { "SteamUGC.SetAdminQuery(", null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj31[1] = uGCUpdateHandle.ToString();
			obj31[2] = ", ";
			obj31[3] = true.ToString();
			obj31[4] = ") : ";
			obj31[5] = flag15.ToString();
			MonoBehaviour.print(string.Concat(obj31));
		}
		if (GUILayout.Button("SetCloudFileNameFilter(m_UGCQueryHandle, \"\")"))
		{
			bool flag16 = SteamUGC.SetCloudFileNameFilter(m_UGCQueryHandle, "");
			uGCQueryHandle = m_UGCQueryHandle;
			MonoBehaviour.print("SteamUGC.SetCloudFileNameFilter(" + uGCQueryHandle.ToString() + ", \"\") : " + flag16);
		}
		if (GUILayout.Button("SetMatchAnyTag(m_UGCQueryHandle, true)"))
		{
			bool flag17 = SteamUGC.SetMatchAnyTag(m_UGCQueryHandle, bMatchAnyTag: true);
			string[] obj32 = new string[6] { "SteamUGC.SetMatchAnyTag(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj32[1] = uGCQueryHandle.ToString();
			obj32[2] = ", ";
			obj32[3] = true.ToString();
			obj32[4] = ") : ";
			obj32[5] = flag17.ToString();
			MonoBehaviour.print(string.Concat(obj32));
		}
		if (GUILayout.Button("SetSearchText(m_UGCQueryHandle, \"Test\")"))
		{
			bool flag18 = SteamUGC.SetSearchText(m_UGCQueryHandle, "Test");
			uGCQueryHandle = m_UGCQueryHandle;
			MonoBehaviour.print("SteamUGC.SetSearchText(" + uGCQueryHandle.ToString() + ", \"Test\") : " + flag18);
		}
		if (GUILayout.Button("SetRankedByTrendDays(m_UGCQueryHandle, 7)"))
		{
			bool flag19 = SteamUGC.SetRankedByTrendDays(m_UGCQueryHandle, 7u);
			string[] obj33 = new string[6] { "SteamUGC.SetRankedByTrendDays(", null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj33[1] = uGCQueryHandle.ToString();
			obj33[2] = ", ";
			obj33[3] = 7.ToString();
			obj33[4] = ") : ";
			obj33[5] = flag19.ToString();
			MonoBehaviour.print(string.Concat(obj33));
		}
		if (GUILayout.Button("SetTimeCreatedDateRange(m_UGCQueryHandle, 0, 0)"))
		{
			bool flag20 = SteamUGC.SetTimeCreatedDateRange(m_UGCQueryHandle, 0u, 0u);
			string[] obj34 = new string[8] { "SteamUGC.SetTimeCreatedDateRange(", null, null, null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj34[1] = uGCQueryHandle.ToString();
			obj34[2] = ", ";
			obj34[3] = 0.ToString();
			obj34[4] = ", ";
			obj34[5] = 0.ToString();
			obj34[6] = ") : ";
			obj34[7] = flag20.ToString();
			MonoBehaviour.print(string.Concat(obj34));
		}
		if (GUILayout.Button("SetTimeUpdatedDateRange(m_UGCQueryHandle, 0, 0)"))
		{
			bool flag21 = SteamUGC.SetTimeUpdatedDateRange(m_UGCQueryHandle, 0u, 0u);
			string[] obj35 = new string[8] { "SteamUGC.SetTimeUpdatedDateRange(", null, null, null, null, null, null, null };
			uGCQueryHandle = m_UGCQueryHandle;
			obj35[1] = uGCQueryHandle.ToString();
			obj35[2] = ", ";
			obj35[3] = 0.ToString();
			obj35[4] = ", ";
			obj35[5] = 0.ToString();
			obj35[6] = ") : ";
			obj35[7] = flag21.ToString();
			MonoBehaviour.print(string.Concat(obj35));
		}
		if (GUILayout.Button("AddRequiredKeyValueTag(m_UGCQueryHandle, \"TestKey\", \"TestValue\")"))
		{
			bool flag22 = SteamUGC.AddRequiredKeyValueTag(m_UGCQueryHandle, "TestKey", "TestValue");
			uGCQueryHandle = m_UGCQueryHandle;
			MonoBehaviour.print("SteamUGC.AddRequiredKeyValueTag(" + uGCQueryHandle.ToString() + ", \"TestKey\", \"TestValue\") : " + flag22);
		}
		if (GUILayout.Button("RequestUGCDetails(m_PublishedFileId, 5)"))
		{
			SteamAPICall_t steamAPICall_t3 = SteamUGC.RequestUGCDetails(m_PublishedFileId, 5u);
			OnSteamUGCRequestUGCDetailsResultCallResult.Set(steamAPICall_t3);
			OnSteamUGCRequestUGCDetailsResultCallResult.Set(steamAPICall_t3);
			string[] obj36 = new string[6] { "SteamUGC.RequestUGCDetails(", null, null, null, null, null };
			publishedFileId = m_PublishedFileId;
			obj36[1] = publishedFileId.ToString();
			obj36[2] = ", ";
			obj36[3] = 5.ToString();
			obj36[4] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t3;
			obj36[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj36));
		}
		if (GUILayout.Button("CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeCommunity)"))
		{
			SteamAPICall_t steamAPICall_t4 = SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeFirst);
			OnCreateItemResultCallResult.Set(steamAPICall_t4);
			string[] obj37 = new string[6]
			{
				"SteamUGC.CreateItem(",
				SteamUtils.GetAppID().ToString(),
				", ",
				EWorkshopFileType.k_EWorkshopFileTypeFirst.ToString(),
				") : ",
				null
			};
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t4;
			obj37[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj37));
		}
		if (GUILayout.Button("StartItemUpdate(SteamUtils.GetAppID(), m_PublishedFileId)"))
		{
			m_UGCUpdateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), m_PublishedFileId);
			string[] obj38 = new string[6]
			{
				"SteamUGC.StartItemUpdate(",
				SteamUtils.GetAppID().ToString(),
				", ",
				null,
				null,
				null
			};
			publishedFileId = m_PublishedFileId;
			obj38[3] = publishedFileId.ToString();
			obj38[4] = ") : ";
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj38[5] = uGCUpdateHandle.ToString();
			MonoBehaviour.print(string.Concat(obj38));
		}
		if (GUILayout.Button("SetItemTitle(m_UGCUpdateHandle, \"This is a Test\")"))
		{
			bool flag23 = SteamUGC.SetItemTitle(m_UGCUpdateHandle, "This is a Test");
			uGCUpdateHandle = m_UGCUpdateHandle;
			MonoBehaviour.print("SteamUGC.SetItemTitle(" + uGCUpdateHandle.ToString() + ", \"This is a Test\") : " + flag23);
		}
		if (GUILayout.Button("SetItemDescription(m_UGCUpdateHandle, \"This is the test description.\")"))
		{
			bool flag24 = SteamUGC.SetItemDescription(m_UGCUpdateHandle, "This is the test description.");
			uGCUpdateHandle = m_UGCUpdateHandle;
			MonoBehaviour.print("SteamUGC.SetItemDescription(" + uGCUpdateHandle.ToString() + ", \"This is the test description.\") : " + flag24);
		}
		if (GUILayout.Button("SetItemUpdateLanguage(m_UGCUpdateHandle, \"english\")"))
		{
			bool flag25 = SteamUGC.SetItemUpdateLanguage(m_UGCUpdateHandle, "english");
			uGCUpdateHandle = m_UGCUpdateHandle;
			MonoBehaviour.print("SteamUGC.SetItemUpdateLanguage(" + uGCUpdateHandle.ToString() + ", \"english\") : " + flag25);
		}
		if (GUILayout.Button("SetItemMetadata(m_UGCUpdateHandle, \"This is the test metadata.\")"))
		{
			bool flag26 = SteamUGC.SetItemMetadata(m_UGCUpdateHandle, "This is the test metadata.");
			uGCUpdateHandle = m_UGCUpdateHandle;
			MonoBehaviour.print("SteamUGC.SetItemMetadata(" + uGCUpdateHandle.ToString() + ", \"This is the test metadata.\") : " + flag26);
		}
		if (GUILayout.Button("SetItemVisibility(m_UGCUpdateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic)"))
		{
			bool flag27 = SteamUGC.SetItemVisibility(m_UGCUpdateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic);
			string[] obj39 = new string[6] { "SteamUGC.SetItemVisibility(", null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj39[1] = uGCUpdateHandle.ToString();
			obj39[2] = ", ";
			obj39[3] = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic.ToString();
			obj39[4] = ") : ";
			obj39[5] = flag27.ToString();
			MonoBehaviour.print(string.Concat(obj39));
		}
		if (GUILayout.Button("SetItemTags(m_UGCUpdateHandle, new string[] {\"Tag One\", \"Tag Two\", \"Test Tags\", \"Sorry\"})"))
		{
			bool flag28 = SteamUGC.SetItemTags(m_UGCUpdateHandle, new string[4] { "Tag One", "Tag Two", "Test Tags", "Sorry" });
			string[] obj40 = new string[6] { "SteamUGC.SetItemTags(", null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj40[1] = uGCUpdateHandle.ToString();
			obj40[2] = ", ";
			obj40[3] = new string[4] { "Tag One", "Tag Two", "Test Tags", "Sorry" }?.ToString();
			obj40[4] = ") : ";
			obj40[5] = flag28.ToString();
			MonoBehaviour.print(string.Concat(obj40));
		}
		if (GUILayout.Button("SetItemContent(m_UGCUpdateHandle, Application.dataPath + \"/Scenes\")"))
		{
			bool flag29 = SteamUGC.SetItemContent(m_UGCUpdateHandle, Application.dataPath + "/Scenes");
			string[] obj41 = new string[6] { "SteamUGC.SetItemContent(", null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj41[1] = uGCUpdateHandle.ToString();
			obj41[2] = ", ";
			obj41[3] = Application.dataPath;
			obj41[4] = "/Scenes) : ";
			obj41[5] = flag29.ToString();
			MonoBehaviour.print(string.Concat(obj41));
		}
		if (GUILayout.Button("SetItemPreview(m_UGCUpdateHandle, Application.dataPath + \"/PreviewImage.jpg\")"))
		{
			bool flag30 = SteamUGC.SetItemPreview(m_UGCUpdateHandle, Application.dataPath + "/PreviewImage.jpg");
			string[] obj42 = new string[6] { "SteamUGC.SetItemPreview(", null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj42[1] = uGCUpdateHandle.ToString();
			obj42[2] = ", ";
			obj42[3] = Application.dataPath;
			obj42[4] = "/PreviewImage.jpg) : ";
			obj42[5] = flag30.ToString();
			MonoBehaviour.print(string.Concat(obj42));
		}
		if (GUILayout.Button("SetAllowLegacyUpload(m_UGCUpdateHandle, true)"))
		{
			bool flag31 = SteamUGC.SetAllowLegacyUpload(m_UGCUpdateHandle, bAllowLegacyUpload: true);
			string[] obj43 = new string[6] { "SteamUGC.SetAllowLegacyUpload(", null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj43[1] = uGCUpdateHandle.ToString();
			obj43[2] = ", ";
			obj43[3] = true.ToString();
			obj43[4] = ") : ";
			obj43[5] = flag31.ToString();
			MonoBehaviour.print(string.Concat(obj43));
		}
		if (GUILayout.Button("RemoveAllItemKeyValueTags(m_UGCUpdateHandle)"))
		{
			bool flag32 = SteamUGC.RemoveAllItemKeyValueTags(m_UGCUpdateHandle);
			uGCUpdateHandle = m_UGCUpdateHandle;
			MonoBehaviour.print("SteamUGC.RemoveAllItemKeyValueTags(" + uGCUpdateHandle.ToString() + ") : " + flag32);
		}
		if (GUILayout.Button("RemoveItemKeyValueTags(m_UGCUpdateHandle, \"TestKey\")"))
		{
			bool flag33 = SteamUGC.RemoveItemKeyValueTags(m_UGCUpdateHandle, "TestKey");
			uGCUpdateHandle = m_UGCUpdateHandle;
			MonoBehaviour.print("SteamUGC.RemoveItemKeyValueTags(" + uGCUpdateHandle.ToString() + ", \"TestKey\") : " + flag33);
		}
		if (GUILayout.Button("AddItemKeyValueTag(m_UGCUpdateHandle, \"TestKey\", \"TestValue\")"))
		{
			bool flag34 = SteamUGC.AddItemKeyValueTag(m_UGCUpdateHandle, "TestKey", "TestValue");
			uGCUpdateHandle = m_UGCUpdateHandle;
			MonoBehaviour.print("SteamUGC.AddItemKeyValueTag(" + uGCUpdateHandle.ToString() + ", \"TestKey\", \"TestValue\") : " + flag34);
		}
		if (GUILayout.Button("AddItemPreviewFile(m_UGCUpdateHandle, Application.dataPath + \"/PreviewImage.jpg\", EItemPreviewType.k_EItemPreviewType_Image)"))
		{
			bool flag35 = SteamUGC.AddItemPreviewFile(m_UGCUpdateHandle, Application.dataPath + "/PreviewImage.jpg", EItemPreviewType.k_EItemPreviewType_Image);
			string[] obj44 = new string[8] { "SteamUGC.AddItemPreviewFile(", null, null, null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj44[1] = uGCUpdateHandle.ToString();
			obj44[2] = ", ";
			obj44[3] = Application.dataPath;
			obj44[4] = "/PreviewImage.jpg, ";
			obj44[5] = EItemPreviewType.k_EItemPreviewType_Image.ToString();
			obj44[6] = ") : ";
			obj44[7] = flag35.ToString();
			MonoBehaviour.print(string.Concat(obj44));
		}
		if (GUILayout.Button("AddItemPreviewVideo(m_UGCUpdateHandle, \"jHgZh4GV9G0\")"))
		{
			bool flag36 = SteamUGC.AddItemPreviewVideo(m_UGCUpdateHandle, "jHgZh4GV9G0");
			uGCUpdateHandle = m_UGCUpdateHandle;
			MonoBehaviour.print("SteamUGC.AddItemPreviewVideo(" + uGCUpdateHandle.ToString() + ", \"jHgZh4GV9G0\") : " + flag36);
		}
		if (GUILayout.Button("UpdateItemPreviewFile(m_UGCUpdateHandle, 0, Application.dataPath + \"/PreviewImage.jpg\")"))
		{
			bool flag37 = SteamUGC.UpdateItemPreviewFile(m_UGCUpdateHandle, 0u, Application.dataPath + "/PreviewImage.jpg");
			string[] obj45 = new string[8] { "SteamUGC.UpdateItemPreviewFile(", null, null, null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj45[1] = uGCUpdateHandle.ToString();
			obj45[2] = ", ";
			obj45[3] = 0.ToString();
			obj45[4] = ", ";
			obj45[5] = Application.dataPath;
			obj45[6] = "/PreviewImage.jpg) : ";
			obj45[7] = flag37.ToString();
			MonoBehaviour.print(string.Concat(obj45));
		}
		if (GUILayout.Button("UpdateItemPreviewVideo(m_UGCUpdateHandle, 0, \"jHgZh4GV9G0\")"))
		{
			bool flag38 = SteamUGC.UpdateItemPreviewVideo(m_UGCUpdateHandle, 0u, "jHgZh4GV9G0");
			string[] obj46 = new string[6] { "SteamUGC.UpdateItemPreviewVideo(", null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj46[1] = uGCUpdateHandle.ToString();
			obj46[2] = ", ";
			obj46[3] = 0.ToString();
			obj46[4] = ", \"jHgZh4GV9G0\") : ";
			obj46[5] = flag38.ToString();
			MonoBehaviour.print(string.Concat(obj46));
		}
		if (GUILayout.Button("RemoveItemPreview(m_UGCUpdateHandle, 0)"))
		{
			bool flag39 = SteamUGC.RemoveItemPreview(m_UGCUpdateHandle, 0u);
			string[] obj47 = new string[6] { "SteamUGC.RemoveItemPreview(", null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj47[1] = uGCUpdateHandle.ToString();
			obj47[2] = ", ";
			obj47[3] = 0.ToString();
			obj47[4] = ") : ";
			obj47[5] = flag39.ToString();
			MonoBehaviour.print(string.Concat(obj47));
		}
		if (GUILayout.Button("AddContentDescriptor(m_UGCUpdateHandle, EUGCContentDescriptorID.k_EUGCContentDescriptor_AnyMatureContent)"))
		{
			bool flag40 = SteamUGC.AddContentDescriptor(m_UGCUpdateHandle, EUGCContentDescriptorID.k_EUGCContentDescriptor_AnyMatureContent);
			string[] obj48 = new string[6] { "SteamUGC.AddContentDescriptor(", null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj48[1] = uGCUpdateHandle.ToString();
			obj48[2] = ", ";
			obj48[3] = EUGCContentDescriptorID.k_EUGCContentDescriptor_AnyMatureContent.ToString();
			obj48[4] = ") : ";
			obj48[5] = flag40.ToString();
			MonoBehaviour.print(string.Concat(obj48));
		}
		if (GUILayout.Button("RemoveContentDescriptor(m_UGCUpdateHandle, EUGCContentDescriptorID.k_EUGCContentDescriptor_AnyMatureContent)"))
		{
			bool flag41 = SteamUGC.RemoveContentDescriptor(m_UGCUpdateHandle, EUGCContentDescriptorID.k_EUGCContentDescriptor_AnyMatureContent);
			string[] obj49 = new string[6] { "SteamUGC.RemoveContentDescriptor(", null, null, null, null, null };
			uGCUpdateHandle = m_UGCUpdateHandle;
			obj49[1] = uGCUpdateHandle.ToString();
			obj49[2] = ", ";
			obj49[3] = EUGCContentDescriptorID.k_EUGCContentDescriptor_AnyMatureContent.ToString();
			obj49[4] = ") : ";
			obj49[5] = flag41.ToString();
			MonoBehaviour.print(string.Concat(obj49));
		}
		if (GUILayout.Button("SetRequiredGameVersions(m_UGCUpdateHandle, \"\", \"\")"))
		{
			bool flag42 = SteamUGC.SetRequiredGameVersions(m_UGCUpdateHandle, "", "");
			uGCUpdateHandle = m_UGCUpdateHandle;
			MonoBehaviour.print("SteamUGC.SetRequiredGameVersions(" + uGCUpdateHandle.ToString() + ", \"\", \"\") : " + flag42);
		}
		if (GUILayout.Button("SubmitItemUpdate(m_UGCUpdateHandle, \"Test Changenote\")"))
		{
			SteamAPICall_t steamAPICall_t5 = SteamUGC.SubmitItemUpdate(m_UGCUpdateHandle, "Test Changenote");
			OnSubmitItemUpdateResultCallResult.Set(steamAPICall_t5);
			uGCUpdateHandle = m_UGCUpdateHandle;
			string text2 = uGCUpdateHandle.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t5;
			MonoBehaviour.print("SteamUGC.SubmitItemUpdate(" + text2 + ", \"Test Changenote\") : " + steamAPICall_t2.ToString());
		}
		ulong punBytesProcessed;
		ulong punBytesTotal;
		EItemUpdateStatus itemUpdateProgress = SteamUGC.GetItemUpdateProgress(m_UGCUpdateHandle, out punBytesProcessed, out punBytesTotal);
		GUILayout.Label("GetItemUpdateProgress(m_UGCUpdateHandle, out BytesProcessed, out BytesTotal) : " + itemUpdateProgress.ToString() + " -- " + punBytesProcessed + " -- " + punBytesTotal);
		if (GUILayout.Button("SetUserItemVote(TestConstants.Instance.k_PublishedFileId_Champions, true)"))
		{
			SteamAPICall_t steamAPICall_t6 = SteamUGC.SetUserItemVote(TestConstants.Instance.k_PublishedFileId_Champions, bVoteUp: true);
			OnSetUserItemVoteResultCallResult.Set(steamAPICall_t6);
			string[] obj50 = new string[6] { "SteamUGC.SetUserItemVote(", null, null, null, null, null };
			publishedFileId = TestConstants.Instance.k_PublishedFileId_Champions;
			obj50[1] = publishedFileId.ToString();
			obj50[2] = ", ";
			obj50[3] = true.ToString();
			obj50[4] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t6;
			obj50[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj50));
		}
		if (GUILayout.Button("GetUserItemVote(TestConstants.Instance.k_PublishedFileId_Champions)"))
		{
			SteamAPICall_t userItemVote = SteamUGC.GetUserItemVote(TestConstants.Instance.k_PublishedFileId_Champions);
			OnGetUserItemVoteResultCallResult.Set(userItemVote);
			publishedFileId = TestConstants.Instance.k_PublishedFileId_Champions;
			string text3 = publishedFileId.ToString();
			SteamAPICall_t steamAPICall_t2 = userItemVote;
			MonoBehaviour.print("SteamUGC.GetUserItemVote(" + text3 + ") : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("AddItemToFavorites(SteamUtils.GetAppID(), TestConstants.Instance.k_PublishedFileId_Champions)"))
		{
			SteamAPICall_t steamAPICall_t7 = SteamUGC.AddItemToFavorites(SteamUtils.GetAppID(), TestConstants.Instance.k_PublishedFileId_Champions);
			OnUserFavoriteItemsListChangedCallResult.Set(steamAPICall_t7);
			string[] obj51 = new string[6]
			{
				"SteamUGC.AddItemToFavorites(",
				SteamUtils.GetAppID().ToString(),
				", ",
				null,
				null,
				null
			};
			publishedFileId = TestConstants.Instance.k_PublishedFileId_Champions;
			obj51[3] = publishedFileId.ToString();
			obj51[4] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t7;
			obj51[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj51));
		}
		if (GUILayout.Button("RemoveItemFromFavorites(SteamUtils.GetAppID(), TestConstants.Instance.k_PublishedFileId_Champions)"))
		{
			SteamAPICall_t steamAPICall_t8 = SteamUGC.RemoveItemFromFavorites(SteamUtils.GetAppID(), TestConstants.Instance.k_PublishedFileId_Champions);
			OnUserFavoriteItemsListChangedCallResult.Set(steamAPICall_t8);
			string[] obj52 = new string[6]
			{
				"SteamUGC.RemoveItemFromFavorites(",
				SteamUtils.GetAppID().ToString(),
				", ",
				null,
				null,
				null
			};
			publishedFileId = TestConstants.Instance.k_PublishedFileId_Champions;
			obj52[3] = publishedFileId.ToString();
			obj52[4] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t8;
			obj52[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj52));
		}
		if (GUILayout.Button("SubscribeItem(TestConstants.Instance.k_PublishedFileId_Champions)"))
		{
			SteamAPICall_t steamAPICall_t9 = SteamUGC.SubscribeItem(TestConstants.Instance.k_PublishedFileId_Champions);
			OnRemoteStorageSubscribePublishedFileResultCallResult.Set(steamAPICall_t9);
			publishedFileId = TestConstants.Instance.k_PublishedFileId_Champions;
			string text4 = publishedFileId.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t9;
			MonoBehaviour.print("SteamUGC.SubscribeItem(" + text4 + ") : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("UnsubscribeItem(TestConstants.Instance.k_PublishedFileId_Champions)"))
		{
			SteamAPICall_t steamAPICall_t10 = SteamUGC.UnsubscribeItem(TestConstants.Instance.k_PublishedFileId_Champions);
			OnRemoteStorageUnsubscribePublishedFileResultCallResult.Set(steamAPICall_t10);
			publishedFileId = TestConstants.Instance.k_PublishedFileId_Champions;
			string text5 = publishedFileId.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t10;
			MonoBehaviour.print("SteamUGC.UnsubscribeItem(" + text5 + ") : " + steamAPICall_t2.ToString());
		}
		GUILayout.Label("GetNumSubscribedItems() : " + SteamUGC.GetNumSubscribedItems());
		if (GUILayout.Button("GetSubscribedItems(PublishedFileID, (uint)PublishedFileID.Length)"))
		{
			PublishedFileId_t[] array5 = new PublishedFileId_t[1];
			uint subscribedItems = SteamUGC.GetSubscribedItems(array5, (uint)array5.Length);
			m_PublishedFileId = array5[0];
			MonoBehaviour.print("SteamUGC.GetSubscribedItems(" + array5?.ToString() + ", " + (uint)array5.Length + ") : " + subscribedItems);
			MonoBehaviour.print(m_PublishedFileId);
		}
		GUILayout.Label("GetItemState(PublishedFileID) : " + (EItemState)SteamUGC.GetItemState(m_PublishedFileId));
		ulong punSizeOnDisk;
		string pchFolder;
		uint punTimeStamp;
		bool itemInstallInfo = SteamUGC.GetItemInstallInfo(m_PublishedFileId, out punSizeOnDisk, out pchFolder, 1024u, out punTimeStamp);
		GUILayout.Label("GetItemInstallInfo(m_PublishedFileId, out SizeOnDisk, out Folder, 1024, out punTimeStamp) : " + itemInstallInfo + " -- " + punSizeOnDisk + " -- " + pchFolder + " -- " + punTimeStamp);
		if (GUILayout.Button("GetItemDownloadInfo(m_PublishedFileId, out BytesDownloaded, out BytesTotal)"))
		{
			ulong punBytesDownloaded;
			ulong punBytesTotal2;
			bool itemDownloadInfo = SteamUGC.GetItemDownloadInfo(m_PublishedFileId, out punBytesDownloaded, out punBytesTotal2);
			string[] obj53 = new string[8] { "SteamUGC.GetItemDownloadInfo(", null, null, null, null, null, null, null };
			publishedFileId = m_PublishedFileId;
			obj53[1] = publishedFileId.ToString();
			obj53[2] = ", out BytesDownloaded, out BytesTotal) : ";
			obj53[3] = itemDownloadInfo.ToString();
			obj53[4] = " -- ";
			obj53[5] = punBytesDownloaded.ToString();
			obj53[6] = " -- ";
			obj53[7] = punBytesTotal2.ToString();
			MonoBehaviour.print(string.Concat(obj53));
		}
		if (GUILayout.Button("DownloadItem(m_PublishedFileId, true)"))
		{
			bool flag43 = SteamUGC.DownloadItem(m_PublishedFileId, bHighPriority: true);
			string[] obj54 = new string[6] { "SteamUGC.DownloadItem(", null, null, null, null, null };
			publishedFileId = m_PublishedFileId;
			obj54[1] = publishedFileId.ToString();
			obj54[2] = ", ";
			obj54[3] = true.ToString();
			obj54[4] = ") : ";
			obj54[5] = flag43.ToString();
			MonoBehaviour.print(string.Concat(obj54));
		}
		if (GUILayout.Button("BInitWorkshopForGameServer((DepotId_t)481, \"C:/UGCTest\")"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamUGC.BInitWorkshopForGameServer((DepotId_t)481u, "C:/UGCTest").ToString(), str0: "SteamUGC.BInitWorkshopForGameServer(", str1: ((DepotId_t)481u).ToString(), str2: ", \"C:/UGCTest\") : "));
		}
		if (GUILayout.Button("SuspendDownloads(true)"))
		{
			SteamUGC.SuspendDownloads(bSuspend: true);
			MonoBehaviour.print("SteamUGC.SuspendDownloads(" + true + ")");
		}
		if (GUILayout.Button("StartPlaytimeTracking(PublishedFileIds, (uint)PublishedFileIds.Length)"))
		{
			PublishedFileId_t[] array6 = new PublishedFileId_t[1] { TestConstants.Instance.k_PublishedFileId_Champions };
			SteamAPICall_t steamAPICall_t11 = SteamUGC.StartPlaytimeTracking(array6, (uint)array6.Length);
			OnStartPlaytimeTrackingResultCallResult.Set(steamAPICall_t11);
			string[] obj56 = new string[6]
			{
				"SteamUGC.StartPlaytimeTracking(",
				array6?.ToString(),
				", ",
				((uint)array6.Length).ToString(),
				") : ",
				null
			};
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t11;
			obj56[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj56));
		}
		if (GUILayout.Button("StopPlaytimeTracking(PublishedFileIds, (uint)PublishedFileIds.Length)"))
		{
			PublishedFileId_t[] array7 = new PublishedFileId_t[1] { TestConstants.Instance.k_PublishedFileId_Champions };
			SteamAPICall_t steamAPICall_t12 = SteamUGC.StopPlaytimeTracking(array7, (uint)array7.Length);
			OnStopPlaytimeTrackingResultCallResult.Set(steamAPICall_t12);
			string[] obj57 = new string[6]
			{
				"SteamUGC.StopPlaytimeTracking(",
				array7?.ToString(),
				", ",
				((uint)array7.Length).ToString(),
				") : ",
				null
			};
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t12;
			obj57[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj57));
		}
		if (GUILayout.Button("StopPlaytimeTrackingForAllItems()"))
		{
			SteamAPICall_t steamAPICall_t13 = SteamUGC.StopPlaytimeTrackingForAllItems();
			OnStopPlaytimeTrackingResultCallResult.Set(steamAPICall_t13);
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t13;
			MonoBehaviour.print("SteamUGC.StopPlaytimeTrackingForAllItems() : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("AddDependency(m_PublishedFileId, TestConstants.Instance.k_PublishedFileId_Champions)"))
		{
			SteamAPICall_t steamAPICall_t14 = SteamUGC.AddDependency(m_PublishedFileId, TestConstants.Instance.k_PublishedFileId_Champions);
			OnAddUGCDependencyResultCallResult.Set(steamAPICall_t14);
			string[] obj58 = new string[6] { "SteamUGC.AddDependency(", null, null, null, null, null };
			publishedFileId = m_PublishedFileId;
			obj58[1] = publishedFileId.ToString();
			obj58[2] = ", ";
			publishedFileId = TestConstants.Instance.k_PublishedFileId_Champions;
			obj58[3] = publishedFileId.ToString();
			obj58[4] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t14;
			obj58[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj58));
		}
		if (GUILayout.Button("RemoveDependency(m_PublishedFileId, TestConstants.Instance.k_PublishedFileId_Champions)"))
		{
			SteamAPICall_t steamAPICall_t15 = SteamUGC.RemoveDependency(m_PublishedFileId, TestConstants.Instance.k_PublishedFileId_Champions);
			OnRemoveUGCDependencyResultCallResult.Set(steamAPICall_t15);
			string[] obj59 = new string[6] { "SteamUGC.RemoveDependency(", null, null, null, null, null };
			publishedFileId = m_PublishedFileId;
			obj59[1] = publishedFileId.ToString();
			obj59[2] = ", ";
			publishedFileId = TestConstants.Instance.k_PublishedFileId_Champions;
			obj59[3] = publishedFileId.ToString();
			obj59[4] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t15;
			obj59[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj59));
		}
		if (GUILayout.Button("AddAppDependency(m_PublishedFileId, SteamUtils.GetAppID())"))
		{
			SteamAPICall_t steamAPICall_t16 = SteamUGC.AddAppDependency(m_PublishedFileId, SteamUtils.GetAppID());
			OnAddAppDependencyResultCallResult.Set(steamAPICall_t16);
			string[] obj60 = new string[6] { "SteamUGC.AddAppDependency(", null, null, null, null, null };
			publishedFileId = m_PublishedFileId;
			obj60[1] = publishedFileId.ToString();
			obj60[2] = ", ";
			obj60[3] = SteamUtils.GetAppID().ToString();
			obj60[4] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t16;
			obj60[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj60));
		}
		if (GUILayout.Button("RemoveAppDependency(m_PublishedFileId, SteamUtils.GetAppID())"))
		{
			SteamAPICall_t steamAPICall_t17 = SteamUGC.RemoveAppDependency(m_PublishedFileId, SteamUtils.GetAppID());
			OnRemoveAppDependencyResultCallResult.Set(steamAPICall_t17);
			string[] obj61 = new string[6] { "SteamUGC.RemoveAppDependency(", null, null, null, null, null };
			publishedFileId = m_PublishedFileId;
			obj61[1] = publishedFileId.ToString();
			obj61[2] = ", ";
			obj61[3] = SteamUtils.GetAppID().ToString();
			obj61[4] = ") : ";
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t17;
			obj61[5] = steamAPICall_t2.ToString();
			MonoBehaviour.print(string.Concat(obj61));
		}
		if (GUILayout.Button("GetAppDependencies(m_PublishedFileId)"))
		{
			SteamAPICall_t appDependencies = SteamUGC.GetAppDependencies(m_PublishedFileId);
			OnGetAppDependenciesResultCallResult.Set(appDependencies);
			publishedFileId = m_PublishedFileId;
			string text6 = publishedFileId.ToString();
			SteamAPICall_t steamAPICall_t2 = appDependencies;
			MonoBehaviour.print("SteamUGC.GetAppDependencies(" + text6 + ") : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("DeleteItem(m_PublishedFileId)"))
		{
			SteamAPICall_t steamAPICall_t18 = SteamUGC.DeleteItem(m_PublishedFileId);
			OnDeleteItemResultCallResult.Set(steamAPICall_t18);
			publishedFileId = m_PublishedFileId;
			string text7 = publishedFileId.ToString();
			SteamAPICall_t steamAPICall_t2 = steamAPICall_t18;
			MonoBehaviour.print("SteamUGC.DeleteItem(" + text7 + ") : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("ShowWorkshopEULA()"))
		{
			MonoBehaviour.print("SteamUGC.ShowWorkshopEULA() : " + SteamUGC.ShowWorkshopEULA());
		}
		if (GUILayout.Button("GetWorkshopEULAStatus()"))
		{
			SteamAPICall_t workshopEULAStatus = SteamUGC.GetWorkshopEULAStatus();
			OnWorkshopEULAStatusCallResult.Set(workshopEULAStatus);
			SteamAPICall_t steamAPICall_t2 = workshopEULAStatus;
			MonoBehaviour.print("SteamUGC.GetWorkshopEULAStatus() : " + steamAPICall_t2.ToString());
		}
		if (GUILayout.Button("GetUserContentDescriptorPreferences(pvecDescriptors, (uint)pvecDescriptors.Length)"))
		{
			EUGCContentDescriptorID[] array8 = new EUGCContentDescriptorID[100];
			uint userContentDescriptorPreferences = SteamUGC.GetUserContentDescriptorPreferences(array8, (uint)array8.Length);
			MonoBehaviour.print("SteamUGC.GetUserContentDescriptorPreferences(" + array8?.ToString() + ", " + (uint)array8.Length + ") : " + userContentDescriptorPreferences);
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnSteamUGCQueryCompleted(SteamUGCQueryCompleted_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[14]
		{
			"[",
			3401.ToString(),
			" - SteamUGCQueryCompleted] - ",
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
		UGCQueryHandle_t handle = pCallback.m_handle;
		obj[3] = handle.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eResult.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_unNumResultsReturned.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_unTotalMatchingResults.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_bCachedData.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.m_rgchNextCursor;
		Debug.Log(string.Concat(obj));
	}

	private void OnSteamUGCRequestUGCDetailsResult(SteamUGCRequestUGCDetailsResult_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 3402 + " - SteamUGCRequestUGCDetailsResult] - " + pCallback.m_details.ToString() + " -- " + pCallback.m_bCachedData);
		string[] array = new string[51];
		PublishedFileId_t nPublishedFileId = pCallback.m_details.m_nPublishedFileId;
		array[0] = nPublishedFileId.ToString();
		array[1] = " -- ";
		array[2] = pCallback.m_details.m_eResult.ToString();
		array[3] = " -- ";
		array[4] = pCallback.m_details.m_eFileType.ToString();
		array[5] = " -- ";
		AppId_t nCreatorAppID = pCallback.m_details.m_nCreatorAppID;
		array[6] = nCreatorAppID.ToString();
		array[7] = " -- ";
		nCreatorAppID = pCallback.m_details.m_nConsumerAppID;
		array[8] = nCreatorAppID.ToString();
		array[9] = " -- ";
		array[10] = pCallback.m_details.m_rgchTitle;
		array[11] = " -- ";
		array[12] = pCallback.m_details.m_rgchDescription;
		array[13] = " -- ";
		array[14] = pCallback.m_details.m_ulSteamIDOwner.ToString();
		array[15] = " -- ";
		array[16] = pCallback.m_details.m_rtimeCreated.ToString();
		array[17] = " -- ";
		array[18] = pCallback.m_details.m_rtimeUpdated.ToString();
		array[19] = " -- ";
		array[20] = pCallback.m_details.m_rtimeAddedToUserList.ToString();
		array[21] = " -- ";
		array[22] = pCallback.m_details.m_eVisibility.ToString();
		array[23] = " -- ";
		array[24] = pCallback.m_details.m_bBanned.ToString();
		array[25] = " -- ";
		array[26] = pCallback.m_details.m_bAcceptedForUse.ToString();
		array[27] = " -- ";
		array[28] = pCallback.m_details.m_bTagsTruncated.ToString();
		array[29] = " -- ";
		array[30] = pCallback.m_details.m_rgchTags;
		array[31] = " -- ";
		UGCHandle_t hFile = pCallback.m_details.m_hFile;
		array[32] = hFile.ToString();
		array[33] = " -- ";
		hFile = pCallback.m_details.m_hPreviewFile;
		array[34] = hFile.ToString();
		array[35] = " -- ";
		array[36] = pCallback.m_details.m_pchFileName;
		array[37] = " -- ";
		array[38] = pCallback.m_details.m_nFileSize.ToString();
		array[39] = " -- ";
		array[40] = pCallback.m_details.m_nPreviewFileSize.ToString();
		array[41] = " -- ";
		array[42] = pCallback.m_details.m_rgchURL;
		array[43] = " -- ";
		array[44] = pCallback.m_details.m_unVotesUp.ToString();
		array[45] = " -- ";
		array[46] = pCallback.m_details.m_unVotesDown.ToString();
		array[47] = " -- ";
		array[48] = pCallback.m_details.m_flScore.ToString();
		array[49] = " -- ";
		array[50] = pCallback.m_details.m_unNumChildren.ToString();
		Debug.Log(string.Concat(array));
	}

	private void OnCreateItemResult(CreateItemResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			3403.ToString(),
			" - CreateItemResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement.ToString();
		Debug.Log(string.Concat(obj));
		m_PublishedFileId = pCallback.m_nPublishedFileId;
	}

	private void OnSubmitItemUpdateResult(SubmitItemUpdateResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			3404.ToString(),
			" - SubmitItemUpdateResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement.ToString(),
			" -- ",
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[7] = nPublishedFileId.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnItemInstalled(ItemInstalled_t pCallback)
	{
		string[] obj = new string[10]
		{
			"[",
			3405.ToString(),
			" - ItemInstalled] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		AppId_t unAppID = pCallback.m_unAppID;
		obj[3] = unAppID.ToString();
		obj[4] = " -- ";
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		UGCHandle_t hLegacyContent = pCallback.m_hLegacyContent;
		obj[7] = hLegacyContent.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_unManifestID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnDownloadItemResult(DownloadItemResult_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			3406.ToString(),
			" - DownloadItemResult] - ",
			null,
			null,
			null,
			null,
			null
		};
		AppId_t unAppID = pCallback.m_unAppID;
		obj[3] = unAppID.ToString();
		obj[4] = " -- ";
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_eResult.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnUserFavoriteItemsListChanged(UserFavoriteItemsListChanged_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			3407.ToString(),
			" - UserFavoriteItemsListChanged] - ",
			null,
			null,
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[3] = nPublishedFileId.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eResult.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bWasAddRequest.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnSetUserItemVoteResult(SetUserItemVoteResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			3408.ToString(),
			" - SetUserItemVoteResult] - ",
			null,
			null,
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[3] = nPublishedFileId.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eResult.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bVoteUp.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnGetUserItemVoteResult(GetUserItemVoteResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[12]
		{
			"[",
			3409.ToString(),
			" - GetUserItemVoteResult] - ",
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
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[3] = nPublishedFileId.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eResult.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bVotedUp.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_bVotedDown.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_bVoteSkipped.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnStartPlaytimeTrackingResult(StartPlaytimeTrackingResult_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 3410 + " - StartPlaytimeTrackingResult] - " + pCallback.m_eResult);
	}

	private void OnStopPlaytimeTrackingResult(StopPlaytimeTrackingResult_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 3411 + " - StopPlaytimeTrackingResult] - " + pCallback.m_eResult);
	}

	private void OnAddUGCDependencyResult(AddUGCDependencyResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			3412.ToString(),
			" - AddUGCDependencyResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		nPublishedFileId = pCallback.m_nChildPublishedFileId;
		obj[7] = nPublishedFileId.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoveUGCDependencyResult(RemoveUGCDependencyResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			3413.ToString(),
			" - RemoveUGCDependencyResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		nPublishedFileId = pCallback.m_nChildPublishedFileId;
		obj[7] = nPublishedFileId.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnAddAppDependencyResult(AddAppDependencyResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			3414.ToString(),
			" - AddAppDependencyResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		AppId_t nAppID = pCallback.m_nAppID;
		obj[7] = nAppID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoveAppDependencyResult(RemoveAppDependencyResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			3415.ToString(),
			" - RemoveAppDependencyResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		AppId_t nAppID = pCallback.m_nAppID;
		obj[7] = nAppID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnGetAppDependenciesResult(GetAppDependenciesResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[12]
		{
			"[",
			3416.ToString(),
			" - GetAppDependenciesResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_rgAppIDs?.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_nNumAppDependencies.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_nTotalNumAppDependencies.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnDeleteItemResult(DeleteItemResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[6]
		{
			"[",
			3417.ToString(),
			" - DeleteItemResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnUserSubscribedItemsListChanged(UserSubscribedItemsListChanged_t pCallback)
	{
		string text = 3418.ToString();
		AppId_t nAppID = pCallback.m_nAppID;
		Debug.Log("[" + text + " - UserSubscribedItemsListChanged] - " + nAppID.ToString());
	}

	private void OnWorkshopEULAStatus(WorkshopEULAStatus_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[14]
		{
			"[",
			3420.ToString(),
			" - WorkshopEULAStatus] - ",
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
			null
		};
		AppId_t nAppID = pCallback.m_nAppID;
		obj[5] = nAppID.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_unVersion.ToString();
		obj[8] = " -- ";
		RTime32 rtAction = pCallback.m_rtAction;
		obj[9] = rtAction.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_bAccepted.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.m_bNeedsAction.ToString();
		Debug.Log(string.Concat(obj));
	}
}
