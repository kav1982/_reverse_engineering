using System.Text;
using Steamworks;
using UnityEngine;

public class SteamRemoteStorageTest : MonoBehaviour
{
	private const string MESSAGE_FILE_NAME = "message.dat";

	private Vector2 m_ScrollPos;

	private string m_Message;

	private int m_FileCount;

	private int m_FileChangeCount;

	private int m_FileSize;

	private ulong m_TotalBytes;

	private int m_FileSizeInBytes;

	private bool m_CloudEnabled;

	private UGCFileWriteStreamHandle_t m_FileStream;

	private UGCHandle_t m_UGCHandle;

	private PublishedFileId_t m_PublishedFileId;

	private PublishedFileUpdateHandle_t m_PublishedFileUpdateHandle;

	private SteamAPICall_t m_FileReadAsyncHandle;

	protected Callback<RemoteStoragePublishedFileSubscribed_t> m_RemoteStoragePublishedFileSubscribed;

	protected Callback<RemoteStoragePublishedFileUnsubscribed_t> m_RemoteStoragePublishedFileUnsubscribed;

	protected Callback<RemoteStoragePublishedFileDeleted_t> m_RemoteStoragePublishedFileDeleted;

	protected Callback<RemoteStoragePublishedFileUpdated_t> m_RemoteStoragePublishedFileUpdated;

	protected Callback<RemoteStorageLocalFileChange_t> m_RemoteStorageLocalFileChange;

	private CallResult<RemoteStorageFileShareResult_t> OnRemoteStorageFileShareResultCallResult;

	private CallResult<RemoteStoragePublishFileResult_t> OnRemoteStoragePublishFileResultCallResult;

	private CallResult<RemoteStorageDeletePublishedFileResult_t> OnRemoteStorageDeletePublishedFileResultCallResult;

	private CallResult<RemoteStorageEnumerateUserPublishedFilesResult_t> OnRemoteStorageEnumerateUserPublishedFilesResultCallResult;

	private CallResult<RemoteStorageSubscribePublishedFileResult_t> OnRemoteStorageSubscribePublishedFileResultCallResult;

	private CallResult<RemoteStorageEnumerateUserSubscribedFilesResult_t> OnRemoteStorageEnumerateUserSubscribedFilesResultCallResult;

	private CallResult<RemoteStorageUnsubscribePublishedFileResult_t> OnRemoteStorageUnsubscribePublishedFileResultCallResult;

	private CallResult<RemoteStorageUpdatePublishedFileResult_t> OnRemoteStorageUpdatePublishedFileResultCallResult;

	private CallResult<RemoteStorageDownloadUGCResult_t> OnRemoteStorageDownloadUGCResultCallResult;

	private CallResult<RemoteStorageGetPublishedFileDetailsResult_t> OnRemoteStorageGetPublishedFileDetailsResultCallResult;

	private CallResult<RemoteStorageEnumerateWorkshopFilesResult_t> OnRemoteStorageEnumerateWorkshopFilesResultCallResult;

	private CallResult<RemoteStorageGetPublishedItemVoteDetailsResult_t> OnRemoteStorageGetPublishedItemVoteDetailsResultCallResult;

	private CallResult<RemoteStorageUpdateUserPublishedItemVoteResult_t> OnRemoteStorageUpdateUserPublishedItemVoteResultCallResult;

	private CallResult<RemoteStorageUserVoteDetails_t> OnRemoteStorageUserVoteDetailsCallResult;

	private CallResult<RemoteStorageEnumerateUserSharedWorkshopFilesResult_t> OnRemoteStorageEnumerateUserSharedWorkshopFilesResultCallResult;

	private CallResult<RemoteStorageSetUserPublishedFileActionResult_t> OnRemoteStorageSetUserPublishedFileActionResultCallResult;

	private CallResult<RemoteStorageEnumeratePublishedFilesByUserActionResult_t> OnRemoteStorageEnumeratePublishedFilesByUserActionResultCallResult;

	private CallResult<RemoteStoragePublishFileProgress_t> OnRemoteStoragePublishFileProgressCallResult;

	private CallResult<RemoteStorageFileWriteAsyncComplete_t> OnRemoteStorageFileWriteAsyncCompleteCallResult;

	private CallResult<RemoteStorageFileReadAsyncComplete_t> OnRemoteStorageFileReadAsyncCompleteCallResult;

	public void OnEnable()
	{
		m_Message = "";
		m_RemoteStoragePublishedFileSubscribed = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(OnRemoteStoragePublishedFileSubscribed);
		m_RemoteStoragePublishedFileUnsubscribed = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(OnRemoteStoragePublishedFileUnsubscribed);
		m_RemoteStoragePublishedFileDeleted = Callback<RemoteStoragePublishedFileDeleted_t>.Create(OnRemoteStoragePublishedFileDeleted);
		m_RemoteStoragePublishedFileUpdated = Callback<RemoteStoragePublishedFileUpdated_t>.Create(OnRemoteStoragePublishedFileUpdated);
		m_RemoteStorageLocalFileChange = Callback<RemoteStorageLocalFileChange_t>.Create(OnRemoteStorageLocalFileChange);
		OnRemoteStorageFileShareResultCallResult = CallResult<RemoteStorageFileShareResult_t>.Create(OnRemoteStorageFileShareResult);
		OnRemoteStoragePublishFileResultCallResult = CallResult<RemoteStoragePublishFileResult_t>.Create(OnRemoteStoragePublishFileResult);
		OnRemoteStorageDeletePublishedFileResultCallResult = CallResult<RemoteStorageDeletePublishedFileResult_t>.Create(OnRemoteStorageDeletePublishedFileResult);
		OnRemoteStorageEnumerateUserPublishedFilesResultCallResult = CallResult<RemoteStorageEnumerateUserPublishedFilesResult_t>.Create(OnRemoteStorageEnumerateUserPublishedFilesResult);
		OnRemoteStorageSubscribePublishedFileResultCallResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create(OnRemoteStorageSubscribePublishedFileResult);
		OnRemoteStorageEnumerateUserSubscribedFilesResultCallResult = CallResult<RemoteStorageEnumerateUserSubscribedFilesResult_t>.Create(OnRemoteStorageEnumerateUserSubscribedFilesResult);
		OnRemoteStorageUnsubscribePublishedFileResultCallResult = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create(OnRemoteStorageUnsubscribePublishedFileResult);
		OnRemoteStorageUpdatePublishedFileResultCallResult = CallResult<RemoteStorageUpdatePublishedFileResult_t>.Create(OnRemoteStorageUpdatePublishedFileResult);
		OnRemoteStorageDownloadUGCResultCallResult = CallResult<RemoteStorageDownloadUGCResult_t>.Create(OnRemoteStorageDownloadUGCResult);
		OnRemoteStorageGetPublishedFileDetailsResultCallResult = CallResult<RemoteStorageGetPublishedFileDetailsResult_t>.Create(OnRemoteStorageGetPublishedFileDetailsResult);
		OnRemoteStorageEnumerateWorkshopFilesResultCallResult = CallResult<RemoteStorageEnumerateWorkshopFilesResult_t>.Create(OnRemoteStorageEnumerateWorkshopFilesResult);
		OnRemoteStorageGetPublishedItemVoteDetailsResultCallResult = CallResult<RemoteStorageGetPublishedItemVoteDetailsResult_t>.Create(OnRemoteStorageGetPublishedItemVoteDetailsResult);
		OnRemoteStorageUpdateUserPublishedItemVoteResultCallResult = CallResult<RemoteStorageUpdateUserPublishedItemVoteResult_t>.Create(OnRemoteStorageUpdateUserPublishedItemVoteResult);
		OnRemoteStorageUserVoteDetailsCallResult = CallResult<RemoteStorageUserVoteDetails_t>.Create(OnRemoteStorageUserVoteDetails);
		OnRemoteStorageEnumerateUserSharedWorkshopFilesResultCallResult = CallResult<RemoteStorageEnumerateUserSharedWorkshopFilesResult_t>.Create(OnRemoteStorageEnumerateUserSharedWorkshopFilesResult);
		OnRemoteStorageSetUserPublishedFileActionResultCallResult = CallResult<RemoteStorageSetUserPublishedFileActionResult_t>.Create(OnRemoteStorageSetUserPublishedFileActionResult);
		OnRemoteStorageEnumeratePublishedFilesByUserActionResultCallResult = CallResult<RemoteStorageEnumeratePublishedFilesByUserActionResult_t>.Create(OnRemoteStorageEnumeratePublishedFilesByUserActionResult);
		OnRemoteStoragePublishFileProgressCallResult = CallResult<RemoteStoragePublishFileProgress_t>.Create(OnRemoteStoragePublishFileProgress);
		OnRemoteStorageFileWriteAsyncCompleteCallResult = CallResult<RemoteStorageFileWriteAsyncComplete_t>.Create(OnRemoteStorageFileWriteAsyncComplete);
		OnRemoteStorageFileReadAsyncCompleteCallResult = CallResult<RemoteStorageFileReadAsyncComplete_t>.Create(OnRemoteStorageFileReadAsyncComplete);
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Message:");
		m_Message = GUILayout.TextField(m_Message, 40);
		GUILayout.Label("m_FileCount: " + m_FileCount);
		GUILayout.Label("m_FileChangeCount: " + m_FileChangeCount);
		GUILayout.Label("m_FileSize: " + m_FileSize);
		GUILayout.Label("m_TotalBytes: " + m_TotalBytes);
		GUILayout.Label("m_FileSizeInBytes: " + m_FileSizeInBytes);
		GUILayout.Label("m_CloudEnabled: " + m_CloudEnabled);
		UGCFileWriteStreamHandle_t fileStream = m_FileStream;
		GUILayout.Label("m_FileStream: " + fileStream.ToString());
		UGCHandle_t uGCHandle = m_UGCHandle;
		GUILayout.Label("m_UGCHandle: " + uGCHandle.ToString());
		PublishedFileId_t publishedFileId = m_PublishedFileId;
		GUILayout.Label("m_PublishedFileId: " + publishedFileId.ToString());
		PublishedFileUpdateHandle_t publishedFileUpdateHandle = m_PublishedFileUpdateHandle;
		GUILayout.Label("m_PublishedFileUpdateHandle: " + publishedFileUpdateHandle.ToString());
		SteamAPICall_t fileReadAsyncHandle = m_FileReadAsyncHandle;
		GUILayout.Label("m_FileReadAsyncHandle: " + fileReadAsyncHandle.ToString());
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		if (GUILayout.Button("FileWrite(MESSAGE_FILE_NAME, Data, Data.Length)"))
		{
			if ((ulong)Encoding.UTF8.GetByteCount(m_Message) > m_TotalBytes)
			{
				MonoBehaviour.print("Remote Storage: Quota Exceeded! - Bytes: " + Encoding.UTF8.GetByteCount(m_Message) + " - Max: " + m_TotalBytes);
			}
			else
			{
				byte[] array = new byte[Encoding.UTF8.GetByteCount(m_Message)];
				Encoding.UTF8.GetBytes(m_Message, 0, m_Message.Length, array, 0);
				MonoBehaviour.print(string.Concat(str3: SteamRemoteStorage.FileWrite("message.dat", array, array.Length).ToString(), str0: "FileWrite(message.dat, Data, ", str1: array.Length.ToString(), str2: ") - "));
			}
		}
		if (GUILayout.Button("FileRead(MESSAGE_FILE_NAME, Data, Data.Length)"))
		{
			if (m_FileSize > 40)
			{
				byte[] pvData = new byte[1];
				Debug.Log("RemoteStorage: File was larger than expected. . .");
				SteamRemoteStorage.FileWrite("message.dat", pvData, 1);
			}
			else
			{
				byte[] array2 = new byte[40];
				int count = SteamRemoteStorage.FileRead("message.dat", array2, array2.Length);
				m_Message = Encoding.UTF8.GetString(array2, 0, count);
				MonoBehaviour.print("FileRead(message.dat, Data, " + array2.Length + ") - " + count);
			}
		}
		if (GUILayout.Button("FileWriteAsync(MESSAGE_FILE_NAME, Data, (uint)Data.Length)"))
		{
			byte[] array3 = new byte[Encoding.UTF8.GetByteCount(m_Message)];
			Encoding.UTF8.GetBytes(m_Message, 0, m_Message.Length, array3, 0);
			SteamAPICall_t steamAPICall_t = SteamRemoteStorage.FileWriteAsync("message.dat", array3, (uint)array3.Length);
			OnRemoteStorageFileWriteAsyncCompleteCallResult.Set(steamAPICall_t);
			string[] obj2 = new string[6]
			{
				"SteamRemoteStorage.FileWriteAsync(message.dat, ",
				array3?.ToString(),
				", ",
				((uint)array3.Length).ToString(),
				") : ",
				null
			};
			fileReadAsyncHandle = steamAPICall_t;
			obj2[5] = fileReadAsyncHandle.ToString();
			MonoBehaviour.print(string.Concat(obj2));
		}
		if (GUILayout.Button("FileReadAsync(MESSAGE_FILE_NAME, Data, (uint)Data.Length)"))
		{
			if (m_FileSize > 40)
			{
				Debug.Log("RemoteStorage: File was larger than expected. . .");
			}
			else
			{
				m_FileReadAsyncHandle = SteamRemoteStorage.FileReadAsync("message.dat", 0u, (uint)m_FileSize);
				OnRemoteStorageFileReadAsyncCompleteCallResult.Set(m_FileReadAsyncHandle);
				uint fileSize = (uint)m_FileSize;
				string text = fileSize.ToString();
				fileReadAsyncHandle = m_FileReadAsyncHandle;
				MonoBehaviour.print("FileReadAsync(message.dat, 0, " + text + ") - " + fileReadAsyncHandle.ToString());
			}
		}
		if (GUILayout.Button("FileForget(MESSAGE_FILE_NAME)"))
		{
			MonoBehaviour.print("SteamRemoteStorage.FileForget(message.dat) : " + SteamRemoteStorage.FileForget("message.dat"));
		}
		if (GUILayout.Button("FileDelete(MESSAGE_FILE_NAME)"))
		{
			MonoBehaviour.print("SteamRemoteStorage.FileDelete(message.dat) : " + SteamRemoteStorage.FileDelete("message.dat"));
		}
		if (GUILayout.Button("FileShare(MESSAGE_FILE_NAME)"))
		{
			SteamAPICall_t steamAPICall_t2 = SteamRemoteStorage.FileShare("message.dat");
			OnRemoteStorageFileShareResultCallResult.Set(steamAPICall_t2);
			fileReadAsyncHandle = steamAPICall_t2;
			MonoBehaviour.print("SteamRemoteStorage.FileShare(message.dat) : " + fileReadAsyncHandle.ToString());
		}
		if (GUILayout.Button("SetSyncPlatforms(MESSAGE_FILE_NAME, ERemoteStoragePlatform.k_ERemoteStoragePlatformAll)"))
		{
			MonoBehaviour.print(string.Concat(str3: SteamRemoteStorage.SetSyncPlatforms("message.dat", ERemoteStoragePlatform.k_ERemoteStoragePlatformAll).ToString(), str0: "SteamRemoteStorage.SetSyncPlatforms(message.dat, ", str1: ERemoteStoragePlatform.k_ERemoteStoragePlatformAll.ToString(), str2: ") : "));
		}
		if (GUILayout.Button("FileWriteStreamOpen(MESSAGE_FILE_NAME)"))
		{
			m_FileStream = SteamRemoteStorage.FileWriteStreamOpen("message.dat");
			fileStream = m_FileStream;
			MonoBehaviour.print("SteamRemoteStorage.FileWriteStreamOpen(message.dat) : " + fileStream.ToString());
		}
		if (GUILayout.Button("FileWriteStreamWriteChunk(m_FileStream, Data, Data.Length)"))
		{
			if ((ulong)Encoding.UTF8.GetByteCount(m_Message) > m_TotalBytes)
			{
				MonoBehaviour.print("Remote Storage: Quota Exceeded! - Bytes: " + Encoding.UTF8.GetByteCount(m_Message) + " - Max: " + m_TotalBytes);
			}
			else
			{
				byte[] array4 = new byte[Encoding.UTF8.GetByteCount(m_Message)];
				Encoding.UTF8.GetBytes(m_Message, 0, m_Message.Length, array4, 0);
				bool flag = SteamRemoteStorage.FileWriteStreamWriteChunk(m_FileStream, array4, array4.Length);
				string[] obj4 = new string[6] { "FileWriteStreamWriteChunk(", null, null, null, null, null };
				fileStream = m_FileStream;
				obj4[1] = fileStream.ToString();
				obj4[2] = ", Data, ";
				obj4[3] = array4.Length.ToString();
				obj4[4] = ") - ";
				obj4[5] = flag.ToString();
				MonoBehaviour.print(string.Concat(obj4));
			}
		}
		if (GUILayout.Button("FileWriteStreamClose(m_FileStream)"))
		{
			bool flag2 = SteamRemoteStorage.FileWriteStreamClose(m_FileStream);
			fileStream = m_FileStream;
			MonoBehaviour.print("SteamRemoteStorage.FileWriteStreamClose(" + fileStream.ToString() + ") : " + flag2);
		}
		if (GUILayout.Button("FileWriteStreamCancel(m_FileStream)"))
		{
			bool flag3 = SteamRemoteStorage.FileWriteStreamCancel(m_FileStream);
			fileStream = m_FileStream;
			MonoBehaviour.print("SteamRemoteStorage.FileWriteStreamCancel(" + fileStream.ToString() + ") : " + flag3);
		}
		GUILayout.Label("FileExists(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.FileExists("message.dat"));
		GUILayout.Label("FilePersisted(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.FilePersisted("message.dat"));
		GUILayout.Label("GetFileSize(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.GetFileSize("message.dat"));
		GUILayout.Label("GetFileTimestamp(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.GetFileTimestamp("message.dat"));
		GUILayout.Label("GetSyncPlatforms(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.GetSyncPlatforms("message.dat"));
		m_FileCount = SteamRemoteStorage.GetFileCount();
		GUILayout.Label("GetFileCount() : " + m_FileCount);
		for (int i = 0; i < m_FileCount; i++)
		{
			int pnFileSizeInBytes = 0;
			string fileNameAndSize = SteamRemoteStorage.GetFileNameAndSize(i, out pnFileSizeInBytes);
			GUILayout.Label("GetFileNameAndSize(i, out FileSize) : " + fileNameAndSize + " -- " + pnFileSizeInBytes);
			if (fileNameAndSize == "message.dat")
			{
				m_FileSize = pnFileSizeInBytes;
			}
		}
		ulong puAvailableBytes;
		bool quota = SteamRemoteStorage.GetQuota(out m_TotalBytes, out puAvailableBytes);
		GUILayout.Label("GetQuota(out m_TotalBytes, out AvailableBytes) : " + quota + " -- " + m_TotalBytes + " -- " + puAvailableBytes);
		GUILayout.Label("IsCloudEnabledForAccount() : " + SteamRemoteStorage.IsCloudEnabledForAccount());
		m_CloudEnabled = SteamRemoteStorage.IsCloudEnabledForApp();
		GUILayout.Label("IsCloudEnabledForApp() : " + m_CloudEnabled);
		if (GUILayout.Button("SetCloudEnabledForApp(!m_CloudEnabled)"))
		{
			SteamRemoteStorage.SetCloudEnabledForApp(!m_CloudEnabled);
			MonoBehaviour.print("SteamRemoteStorage.SetCloudEnabledForApp(" + !m_CloudEnabled + ")");
		}
		if (GUILayout.Button("UGCDownload(m_UGCHandle, 0)"))
		{
			SteamAPICall_t steamAPICall_t3 = SteamRemoteStorage.UGCDownload(m_UGCHandle, 0u);
			OnRemoteStorageDownloadUGCResultCallResult.Set(steamAPICall_t3);
			string[] obj5 = new string[6] { "SteamRemoteStorage.UGCDownload(", null, null, null, null, null };
			uGCHandle = m_UGCHandle;
			obj5[1] = uGCHandle.ToString();
			obj5[2] = ", ";
			obj5[3] = 0.ToString();
			obj5[4] = ") : ";
			fileReadAsyncHandle = steamAPICall_t3;
			obj5[5] = fileReadAsyncHandle.ToString();
			MonoBehaviour.print(string.Concat(obj5));
		}
		int pnBytesDownloaded;
		int pnBytesExpected;
		bool uGCDownloadProgress = SteamRemoteStorage.GetUGCDownloadProgress(m_UGCHandle, out pnBytesDownloaded, out pnBytesExpected);
		GUILayout.Label("GetUGCDownloadProgress(m_UGCHandle, out BytesDownloaded, out BytesExpected) : " + uGCDownloadProgress + " -- " + pnBytesDownloaded + " -- " + pnBytesExpected);
		if (m_UGCHandle != (UGCHandle_t)0uL)
		{
			AppId_t pnAppID;
			string ppchName;
			CSteamID pSteamIDOwner;
			bool uGCDetails = SteamRemoteStorage.GetUGCDetails(m_UGCHandle, out pnAppID, out ppchName, out m_FileSizeInBytes, out pSteamIDOwner);
			string[] obj6 = new string[10]
			{
				"GetUGCDetails(m_UGCHandle, out AppID, Name, out FileSizeInBytes, out SteamIDOwner) : ",
				uGCDetails.ToString(),
				" -- ",
				null,
				null,
				null,
				null,
				null,
				null,
				null
			};
			AppId_t appId_t = pnAppID;
			obj6[3] = appId_t.ToString();
			obj6[4] = " -- ";
			obj6[5] = ppchName;
			obj6[6] = " -- ";
			obj6[7] = m_FileSizeInBytes.ToString();
			obj6[8] = " -- ";
			CSteamID cSteamID = pSteamIDOwner;
			obj6[9] = cSteamID.ToString();
			GUILayout.Label(string.Concat(obj6));
		}
		else
		{
			GUILayout.Label("GetUGCDetails(m_UGCHandle, out AppID, Name, out FileSizeInBytes, out SteamIDOwner) : ");
		}
		if (GUILayout.Button("UGCRead(m_UGCHandle, Data, m_FileSizeInBytes, 0, EUGCReadAction.k_EUGCRead_Close)"))
		{
			byte[] array5 = new byte[m_FileSizeInBytes];
			int num = SteamRemoteStorage.UGCRead(m_UGCHandle, array5, m_FileSizeInBytes, 0u, EUGCReadAction.k_EUGCRead_Close);
			string[] obj7 = new string[12]
			{
				"SteamRemoteStorage.UGCRead(", null, null, null, null, null, null, null, null, null,
				null, null
			};
			uGCHandle = m_UGCHandle;
			obj7[1] = uGCHandle.ToString();
			obj7[2] = ", ";
			obj7[3] = array5?.ToString();
			obj7[4] = ", ";
			obj7[5] = m_FileSizeInBytes.ToString();
			obj7[6] = ", ";
			obj7[7] = 0.ToString();
			obj7[8] = ", ";
			obj7[9] = EUGCReadAction.k_EUGCRead_Close.ToString();
			obj7[10] = ") : ";
			obj7[11] = num.ToString();
			MonoBehaviour.print(string.Concat(obj7));
		}
		GUILayout.Label("GetCachedUGCCount() : " + SteamRemoteStorage.GetCachedUGCCount());
		GUILayout.Label("GetCachedUGCHandle(0) : " + SteamRemoteStorage.GetCachedUGCHandle(0).ToString());
		if (GUILayout.Button("PublishWorkshopFile(MESSAGE_FILE_NAME, null, SteamUtils.GetAppID(), \"Title!\", \"Description!\", ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic, Tags, EWorkshopFileType.k_EWorkshopFileTypeCommunity)"))
		{
			string[] array6 = new string[3] { "Test1", "Test2", "Test3" };
			SteamAPICall_t steamAPICall_t4 = SteamRemoteStorage.PublishWorkshopFile("message.dat", null, SteamUtils.GetAppID(), "Title!", "Description!", ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic, array6, EWorkshopFileType.k_EWorkshopFileTypeFirst);
			OnRemoteStoragePublishFileProgressCallResult.Set(steamAPICall_t4);
			string[] obj8 = new string[10]
			{
				"SteamRemoteStorage.PublishWorkshopFile(message.dat, , ",
				SteamUtils.GetAppID().ToString(),
				", \"Title!\", \"Description!\", ",
				ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic.ToString(),
				", ",
				array6?.ToString(),
				", ",
				EWorkshopFileType.k_EWorkshopFileTypeFirst.ToString(),
				") : ",
				null
			};
			fileReadAsyncHandle = steamAPICall_t4;
			obj8[9] = fileReadAsyncHandle.ToString();
			MonoBehaviour.print(string.Concat(obj8));
		}
		if (GUILayout.Button("CreatePublishedFileUpdateRequest(m_PublishedFileId)"))
		{
			m_PublishedFileUpdateHandle = SteamRemoteStorage.CreatePublishedFileUpdateRequest(m_PublishedFileId);
			publishedFileId = m_PublishedFileId;
			string text2 = publishedFileId.ToString();
			publishedFileUpdateHandle = m_PublishedFileUpdateHandle;
			MonoBehaviour.print("SteamRemoteStorage.CreatePublishedFileUpdateRequest(" + text2 + ") : " + publishedFileUpdateHandle.ToString());
		}
		if (GUILayout.Button("UpdatePublishedFileFile(m_PublishedFileUpdateHandle, MESSAGE_FILE_NAME)"))
		{
			bool flag4 = SteamRemoteStorage.UpdatePublishedFileFile(m_PublishedFileUpdateHandle, "message.dat");
			publishedFileUpdateHandle = m_PublishedFileUpdateHandle;
			MonoBehaviour.print("SteamRemoteStorage.UpdatePublishedFileFile(" + publishedFileUpdateHandle.ToString() + ", message.dat) : " + flag4);
		}
		if (GUILayout.Button("UpdatePublishedFilePreviewFile(m_PublishedFileUpdateHandle, null)"))
		{
			bool flag5 = SteamRemoteStorage.UpdatePublishedFilePreviewFile(m_PublishedFileUpdateHandle, null);
			publishedFileUpdateHandle = m_PublishedFileUpdateHandle;
			MonoBehaviour.print("SteamRemoteStorage.UpdatePublishedFilePreviewFile(" + publishedFileUpdateHandle.ToString() + ", ) : " + flag5);
		}
		if (GUILayout.Button("UpdatePublishedFileTitle(m_PublishedFileUpdateHandle, \"New Title\")"))
		{
			bool flag6 = SteamRemoteStorage.UpdatePublishedFileTitle(m_PublishedFileUpdateHandle, "New Title");
			publishedFileUpdateHandle = m_PublishedFileUpdateHandle;
			MonoBehaviour.print("SteamRemoteStorage.UpdatePublishedFileTitle(" + publishedFileUpdateHandle.ToString() + ", \"New Title\") : " + flag6);
		}
		if (GUILayout.Button("UpdatePublishedFileDescription(m_PublishedFileUpdateHandle, \"New Description\")"))
		{
			bool flag7 = SteamRemoteStorage.UpdatePublishedFileDescription(m_PublishedFileUpdateHandle, "New Description");
			publishedFileUpdateHandle = m_PublishedFileUpdateHandle;
			MonoBehaviour.print("SteamRemoteStorage.UpdatePublishedFileDescription(" + publishedFileUpdateHandle.ToString() + ", \"New Description\") : " + flag7);
		}
		if (GUILayout.Button("UpdatePublishedFileVisibility(m_PublishedFileUpdateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic)"))
		{
			bool flag8 = SteamRemoteStorage.UpdatePublishedFileVisibility(m_PublishedFileUpdateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic);
			string[] obj9 = new string[6] { "SteamRemoteStorage.UpdatePublishedFileVisibility(", null, null, null, null, null };
			publishedFileUpdateHandle = m_PublishedFileUpdateHandle;
			obj9[1] = publishedFileUpdateHandle.ToString();
			obj9[2] = ", ";
			obj9[3] = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic.ToString();
			obj9[4] = ") : ";
			obj9[5] = flag8.ToString();
			MonoBehaviour.print(string.Concat(obj9));
		}
		if (GUILayout.Button("UpdatePublishedFileTags(m_PublishedFileUpdateHandle, new string[] {\"First\", \"Second\", \"Third\"})"))
		{
			bool flag9 = SteamRemoteStorage.UpdatePublishedFileTags(m_PublishedFileUpdateHandle, new string[3] { "First", "Second", "Third" });
			string[] obj10 = new string[6] { "SteamRemoteStorage.UpdatePublishedFileTags(", null, null, null, null, null };
			publishedFileUpdateHandle = m_PublishedFileUpdateHandle;
			obj10[1] = publishedFileUpdateHandle.ToString();
			obj10[2] = ", ";
			obj10[3] = new string[3] { "First", "Second", "Third" }?.ToString();
			obj10[4] = ") : ";
			obj10[5] = flag9.ToString();
			MonoBehaviour.print(string.Concat(obj10));
		}
		if (GUILayout.Button("CommitPublishedFileUpdate(m_PublishedFileUpdateHandle)"))
		{
			SteamAPICall_t steamAPICall_t5 = SteamRemoteStorage.CommitPublishedFileUpdate(m_PublishedFileUpdateHandle);
			OnRemoteStorageUpdatePublishedFileResultCallResult.Set(steamAPICall_t5);
			publishedFileUpdateHandle = m_PublishedFileUpdateHandle;
			string text3 = publishedFileUpdateHandle.ToString();
			fileReadAsyncHandle = steamAPICall_t5;
			MonoBehaviour.print("SteamRemoteStorage.CommitPublishedFileUpdate(" + text3 + ") : " + fileReadAsyncHandle.ToString());
		}
		if (GUILayout.Button("GetPublishedFileDetails(m_PublishedFileId, 0)"))
		{
			SteamAPICall_t publishedFileDetails = SteamRemoteStorage.GetPublishedFileDetails(m_PublishedFileId, 0u);
			OnRemoteStorageGetPublishedFileDetailsResultCallResult.Set(publishedFileDetails);
			string[] obj11 = new string[6] { "SteamRemoteStorage.GetPublishedFileDetails(", null, null, null, null, null };
			publishedFileId = m_PublishedFileId;
			obj11[1] = publishedFileId.ToString();
			obj11[2] = ", ";
			obj11[3] = 0.ToString();
			obj11[4] = ") : ";
			fileReadAsyncHandle = publishedFileDetails;
			obj11[5] = fileReadAsyncHandle.ToString();
			MonoBehaviour.print(string.Concat(obj11));
		}
		if (GUILayout.Button("DeletePublishedFile(m_PublishedFileId)"))
		{
			SteamAPICall_t steamAPICall_t6 = SteamRemoteStorage.DeletePublishedFile(m_PublishedFileId);
			OnRemoteStorageDeletePublishedFileResultCallResult.Set(steamAPICall_t6);
			publishedFileId = m_PublishedFileId;
			string text4 = publishedFileId.ToString();
			fileReadAsyncHandle = steamAPICall_t6;
			MonoBehaviour.print("SteamRemoteStorage.DeletePublishedFile(" + text4 + ") : " + fileReadAsyncHandle.ToString());
		}
		if (GUILayout.Button("EnumerateUserPublishedFiles(0)"))
		{
			SteamAPICall_t steamAPICall_t7 = SteamRemoteStorage.EnumerateUserPublishedFiles(0u);
			OnRemoteStorageEnumerateUserPublishedFilesResultCallResult.Set(steamAPICall_t7);
			string text5 = 0.ToString();
			fileReadAsyncHandle = steamAPICall_t7;
			MonoBehaviour.print("SteamRemoteStorage.EnumerateUserPublishedFiles(" + text5 + ") : " + fileReadAsyncHandle.ToString());
		}
		if (GUILayout.Button("SubscribePublishedFile(m_PublishedFileId)"))
		{
			SteamAPICall_t steamAPICall_t8 = SteamRemoteStorage.SubscribePublishedFile(m_PublishedFileId);
			OnRemoteStorageSubscribePublishedFileResultCallResult.Set(steamAPICall_t8);
			publishedFileId = m_PublishedFileId;
			string text6 = publishedFileId.ToString();
			fileReadAsyncHandle = steamAPICall_t8;
			MonoBehaviour.print("SteamRemoteStorage.SubscribePublishedFile(" + text6 + ") : " + fileReadAsyncHandle.ToString());
		}
		if (GUILayout.Button("EnumerateUserSubscribedFiles(0)"))
		{
			SteamAPICall_t steamAPICall_t9 = SteamRemoteStorage.EnumerateUserSubscribedFiles(0u);
			OnRemoteStorageEnumerateUserSubscribedFilesResultCallResult.Set(steamAPICall_t9);
			string text7 = 0.ToString();
			fileReadAsyncHandle = steamAPICall_t9;
			MonoBehaviour.print("SteamRemoteStorage.EnumerateUserSubscribedFiles(" + text7 + ") : " + fileReadAsyncHandle.ToString());
		}
		if (GUILayout.Button("UnsubscribePublishedFile(m_PublishedFileId)"))
		{
			SteamAPICall_t steamAPICall_t10 = SteamRemoteStorage.UnsubscribePublishedFile(m_PublishedFileId);
			OnRemoteStorageUnsubscribePublishedFileResultCallResult.Set(steamAPICall_t10);
			publishedFileId = m_PublishedFileId;
			string text8 = publishedFileId.ToString();
			fileReadAsyncHandle = steamAPICall_t10;
			MonoBehaviour.print("SteamRemoteStorage.UnsubscribePublishedFile(" + text8 + ") : " + fileReadAsyncHandle.ToString());
		}
		if (GUILayout.Button("UpdatePublishedFileSetChangeDescription(m_PublishedFileUpdateHandle, \"Changelog!\")"))
		{
			bool flag10 = SteamRemoteStorage.UpdatePublishedFileSetChangeDescription(m_PublishedFileUpdateHandle, "Changelog!");
			publishedFileUpdateHandle = m_PublishedFileUpdateHandle;
			MonoBehaviour.print("SteamRemoteStorage.UpdatePublishedFileSetChangeDescription(" + publishedFileUpdateHandle.ToString() + ", \"Changelog!\") : " + flag10);
		}
		if (GUILayout.Button("GetPublishedItemVoteDetails(m_PublishedFileId)"))
		{
			SteamAPICall_t publishedItemVoteDetails = SteamRemoteStorage.GetPublishedItemVoteDetails(m_PublishedFileId);
			OnRemoteStorageGetPublishedItemVoteDetailsResultCallResult.Set(publishedItemVoteDetails);
			publishedFileId = m_PublishedFileId;
			string text9 = publishedFileId.ToString();
			fileReadAsyncHandle = publishedItemVoteDetails;
			MonoBehaviour.print("SteamRemoteStorage.GetPublishedItemVoteDetails(" + text9 + ") : " + fileReadAsyncHandle.ToString());
		}
		if (GUILayout.Button("UpdateUserPublishedItemVote(m_PublishedFileId, true)"))
		{
			SteamAPICall_t steamAPICall_t11 = SteamRemoteStorage.UpdateUserPublishedItemVote(m_PublishedFileId, bVoteUp: true);
			OnRemoteStorageUpdateUserPublishedItemVoteResultCallResult.Set(steamAPICall_t11);
			string[] obj12 = new string[6] { "SteamRemoteStorage.UpdateUserPublishedItemVote(", null, null, null, null, null };
			publishedFileId = m_PublishedFileId;
			obj12[1] = publishedFileId.ToString();
			obj12[2] = ", ";
			obj12[3] = true.ToString();
			obj12[4] = ") : ";
			fileReadAsyncHandle = steamAPICall_t11;
			obj12[5] = fileReadAsyncHandle.ToString();
			MonoBehaviour.print(string.Concat(obj12));
		}
		if (GUILayout.Button("GetUserPublishedItemVoteDetails(m_PublishedFileId)"))
		{
			SteamAPICall_t userPublishedItemVoteDetails = SteamRemoteStorage.GetUserPublishedItemVoteDetails(m_PublishedFileId);
			OnRemoteStorageGetPublishedItemVoteDetailsResultCallResult.Set(userPublishedItemVoteDetails);
			publishedFileId = m_PublishedFileId;
			string text10 = publishedFileId.ToString();
			fileReadAsyncHandle = userPublishedItemVoteDetails;
			MonoBehaviour.print("SteamRemoteStorage.GetUserPublishedItemVoteDetails(" + text10 + ") : " + fileReadAsyncHandle.ToString());
		}
		if (GUILayout.Button("EnumerateUserSharedWorkshopFiles(SteamUser.GetSteamID(), 0, null, null)"))
		{
			SteamAPICall_t steamAPICall_t12 = SteamRemoteStorage.EnumerateUserSharedWorkshopFiles(SteamUser.GetSteamID(), 0u, null, null);
			OnRemoteStorageEnumerateUserPublishedFilesResultCallResult.Set(steamAPICall_t12);
			string[] obj13 = new string[6]
			{
				"SteamRemoteStorage.EnumerateUserSharedWorkshopFiles(",
				SteamUser.GetSteamID().ToString(),
				", ",
				0.ToString(),
				", , ) : ",
				null
			};
			fileReadAsyncHandle = steamAPICall_t12;
			obj13[5] = fileReadAsyncHandle.ToString();
			MonoBehaviour.print(string.Concat(obj13));
		}
		if (GUILayout.Button("PublishVideo(EWorkshopVideoProvider.k_EWorkshopVideoProviderYoutube, \"William Hunter\", \"Rmvb4Hktv7U\", null, SteamUtils.GetAppID(), \"Test Video\", \"Desc\", ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic, null)"))
		{
			SteamAPICall_t steamAPICall_t13 = SteamRemoteStorage.PublishVideo(EWorkshopVideoProvider.k_EWorkshopVideoProviderYoutube, "William Hunter", "Rmvb4Hktv7U", null, SteamUtils.GetAppID(), "Test Video", "Desc", ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic, null);
			OnRemoteStoragePublishFileProgressCallResult.Set(steamAPICall_t13);
			string[] obj14 = new string[8]
			{
				"SteamRemoteStorage.PublishVideo(",
				EWorkshopVideoProvider.k_EWorkshopVideoProviderYoutube.ToString(),
				", \"William Hunter\", \"Rmvb4Hktv7U\", , ",
				SteamUtils.GetAppID().ToString(),
				", \"Test Video\", \"Desc\", ",
				ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic.ToString(),
				", ) : ",
				null
			};
			fileReadAsyncHandle = steamAPICall_t13;
			obj14[7] = fileReadAsyncHandle.ToString();
			MonoBehaviour.print(string.Concat(obj14));
		}
		if (GUILayout.Button("SetUserPublishedFileAction(m_PublishedFileId, EWorkshopFileAction.k_EWorkshopFileActionPlayed)"))
		{
			SteamAPICall_t steamAPICall_t14 = SteamRemoteStorage.SetUserPublishedFileAction(m_PublishedFileId, EWorkshopFileAction.k_EWorkshopFileActionPlayed);
			OnRemoteStorageSetUserPublishedFileActionResultCallResult.Set(steamAPICall_t14);
			string[] obj15 = new string[6] { "SteamRemoteStorage.SetUserPublishedFileAction(", null, null, null, null, null };
			publishedFileId = m_PublishedFileId;
			obj15[1] = publishedFileId.ToString();
			obj15[2] = ", ";
			obj15[3] = EWorkshopFileAction.k_EWorkshopFileActionPlayed.ToString();
			obj15[4] = ") : ";
			fileReadAsyncHandle = steamAPICall_t14;
			obj15[5] = fileReadAsyncHandle.ToString();
			MonoBehaviour.print(string.Concat(obj15));
		}
		if (GUILayout.Button("EnumeratePublishedFilesByUserAction(EWorkshopFileAction.k_EWorkshopFileActionPlayed, 0)"))
		{
			SteamAPICall_t steamAPICall_t15 = SteamRemoteStorage.EnumeratePublishedFilesByUserAction(EWorkshopFileAction.k_EWorkshopFileActionPlayed, 0u);
			OnRemoteStorageEnumeratePublishedFilesByUserActionResultCallResult.Set(steamAPICall_t15);
			string[] obj16 = new string[6]
			{
				"SteamRemoteStorage.EnumeratePublishedFilesByUserAction(",
				EWorkshopFileAction.k_EWorkshopFileActionPlayed.ToString(),
				", ",
				0.ToString(),
				") : ",
				null
			};
			fileReadAsyncHandle = steamAPICall_t15;
			obj16[5] = fileReadAsyncHandle.ToString();
			MonoBehaviour.print(string.Concat(obj16));
		}
		if (GUILayout.Button("EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType.k_EWorkshopEnumerationTypeRankedByVote, 0, 3, 0, null, null)"))
		{
			SteamAPICall_t steamAPICall_t16 = SteamRemoteStorage.EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType.k_EWorkshopEnumerationTypeRankedByVote, 0u, 3u, 0u, null, null);
			OnRemoteStorageEnumerateWorkshopFilesResultCallResult.Set(steamAPICall_t16);
			string[] obj17 = new string[10]
			{
				"SteamRemoteStorage.EnumeratePublishedWorkshopFiles(",
				EWorkshopEnumerationType.k_EWorkshopEnumerationTypeRankedByVote.ToString(),
				", ",
				0.ToString(),
				", ",
				3.ToString(),
				", ",
				0.ToString(),
				", , ) : ",
				null
			};
			fileReadAsyncHandle = steamAPICall_t16;
			obj17[9] = fileReadAsyncHandle.ToString();
			MonoBehaviour.print(string.Concat(obj17));
		}
		m_FileChangeCount = SteamRemoteStorage.GetLocalFileChangeCount();
		GUILayout.Label("GetLocalFileChangeCount() : " + m_FileChangeCount);
		for (int j = 0; j < m_FileChangeCount; j++)
		{
			ERemoteStorageLocalFileChange pEChangeType = ERemoteStorageLocalFileChange.k_ERemoteStorageLocalFileChange_Invalid;
			ERemoteStorageFilePathType pEFilePathType = ERemoteStorageFilePathType.k_ERemoteStorageFilePathType_Invalid;
			string localFileChange = SteamRemoteStorage.GetLocalFileChange(j, out pEChangeType, out pEFilePathType);
			GUILayout.Label("GetLocalFileChange(i, out ChangeType, out FilePathType) : " + localFileChange + " -- " + pEChangeType.ToString() + " -- " + pEFilePathType);
		}
		if (GUILayout.Button("BeginFileWriteBatch()"))
		{
			MonoBehaviour.print("SteamRemoteStorage.BeginFileWriteBatch() : " + SteamRemoteStorage.BeginFileWriteBatch());
		}
		if (GUILayout.Button("EndFileWriteBatch()"))
		{
			MonoBehaviour.print("SteamRemoteStorage.EndFileWriteBatch() : " + SteamRemoteStorage.EndFileWriteBatch());
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnRemoteStorageFileShareResult(RemoteStorageFileShareResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			1307.ToString(),
			" - RemoteStorageFileShareResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		UGCHandle_t hFile = pCallback.m_hFile;
		obj[5] = hFile.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_rgchFilename;
		Debug.Log(string.Concat(obj));
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			m_UGCHandle = pCallback.m_hFile;
		}
	}

	private void OnRemoteStoragePublishFileResult(RemoteStoragePublishFileResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			1309.ToString(),
			" - RemoteStoragePublishFileResult] - ",
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
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			m_PublishedFileId = pCallback.m_nPublishedFileId;
		}
	}

	private void OnRemoteStorageDeletePublishedFileResult(RemoteStorageDeletePublishedFileResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[6]
		{
			"[",
			1311.ToString(),
			" - RemoteStorageDeletePublishedFileResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoteStorageEnumerateUserPublishedFilesResult(RemoteStorageEnumerateUserPublishedFilesResult_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 1312 + " - RemoteStorageEnumerateUserPublishedFilesResult] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId);
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
	}

	private void OnRemoteStorageEnumerateUserSubscribedFilesResult(RemoteStorageEnumerateUserSubscribedFilesResult_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 1314 + " - RemoteStorageEnumerateUserSubscribedFilesResult] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId?.ToString() + " -- " + pCallback.m_rgRTimeSubscribed);
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
	}

	private void OnRemoteStorageUpdatePublishedFileResult(RemoteStorageUpdatePublishedFileResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			1316.ToString(),
			" - RemoteStorageUpdatePublishedFileResult] - ",
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
	}

	private void OnRemoteStorageDownloadUGCResult(RemoteStorageDownloadUGCResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[14]
		{
			"[",
			1317.ToString(),
			" - RemoteStorageDownloadUGCResult] - ",
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
		UGCHandle_t hFile = pCallback.m_hFile;
		obj[5] = hFile.ToString();
		obj[6] = " -- ";
		AppId_t nAppID = pCallback.m_nAppID;
		obj[7] = nAppID.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_nSizeInBytes.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_pchFileName;
		obj[12] = " -- ";
		obj[13] = pCallback.m_ulSteamIDOwner.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoteStorageGetPublishedFileDetailsResult(RemoteStorageGetPublishedFileDetailsResult_t pCallback, bool bIOFailure)
	{
		string[] array = new string[44];
		array[0] = "[";
		array[1] = 1318.ToString();
		array[2] = " - RemoteStorageGetPublishedFileDetailsResult] - ";
		array[3] = pCallback.m_eResult.ToString();
		array[4] = " -- ";
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		array[5] = nPublishedFileId.ToString();
		array[6] = " -- ";
		AppId_t nCreatorAppID = pCallback.m_nCreatorAppID;
		array[7] = nCreatorAppID.ToString();
		array[8] = " -- ";
		nCreatorAppID = pCallback.m_nConsumerAppID;
		array[9] = nCreatorAppID.ToString();
		array[10] = " -- ";
		array[11] = pCallback.m_rgchTitle;
		array[12] = " -- ";
		array[13] = pCallback.m_rgchDescription;
		array[14] = " -- ";
		UGCHandle_t hFile = pCallback.m_hFile;
		array[15] = hFile.ToString();
		array[16] = " -- ";
		hFile = pCallback.m_hPreviewFile;
		array[17] = hFile.ToString();
		array[18] = " -- ";
		array[19] = pCallback.m_ulSteamIDOwner.ToString();
		array[20] = " -- ";
		array[21] = pCallback.m_rtimeCreated.ToString();
		array[22] = " -- ";
		array[23] = pCallback.m_rtimeUpdated.ToString();
		array[24] = " -- ";
		array[25] = pCallback.m_eVisibility.ToString();
		array[26] = " -- ";
		array[27] = pCallback.m_bBanned.ToString();
		array[28] = " -- ";
		array[29] = pCallback.m_rgchTags;
		array[30] = " -- ";
		array[31] = pCallback.m_bTagsTruncated.ToString();
		array[32] = " -- ";
		array[33] = pCallback.m_pchFileName;
		array[34] = " -- ";
		array[35] = pCallback.m_nFileSize.ToString();
		array[36] = " -- ";
		array[37] = pCallback.m_nPreviewFileSize.ToString();
		array[38] = " -- ";
		array[39] = pCallback.m_rgchURL;
		array[40] = " -- ";
		array[41] = pCallback.m_eFileType.ToString();
		array[42] = " -- ";
		array[43] = pCallback.m_bAcceptedForUse.ToString();
		Debug.Log(string.Concat(array));
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			m_UGCHandle = pCallback.m_hFile;
		}
	}

	private void OnRemoteStorageEnumerateWorkshopFilesResult(RemoteStorageEnumerateWorkshopFilesResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[16]
		{
			"[",
			1319.ToString(),
			" - RemoteStorageEnumerateWorkshopFilesResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			pCallback.m_nResultsReturned.ToString(),
			" -- ",
			pCallback.m_nTotalResultCount.ToString(),
			" -- ",
			pCallback.m_rgPublishedFileId?.ToString(),
			" -- ",
			pCallback.m_rgScore?.ToString(),
			" -- ",
			null,
			null,
			null
		};
		AppId_t nAppId = pCallback.m_nAppId;
		obj[13] = nAppId.ToString();
		obj[14] = " -- ";
		obj[15] = pCallback.m_unStartIndex.ToString();
		Debug.Log(string.Concat(obj));
		for (int i = 0; i < pCallback.m_nResultsReturned; i++)
		{
			string text = i.ToString();
			PublishedFileId_t publishedFileId_t = pCallback.m_rgPublishedFileId[i];
			MonoBehaviour.print(text + ": " + publishedFileId_t.ToString());
		}
		if (pCallback.m_nResultsReturned >= 1)
		{
			m_PublishedFileId = pCallback.m_rgPublishedFileId[0];
		}
	}

	private void OnRemoteStorageGetPublishedItemVoteDetailsResult(RemoteStorageGetPublishedItemVoteDetailsResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[14]
		{
			"[",
			1320.ToString(),
			" - RemoteStorageGetPublishedItemVoteDetailsResult] - ",
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
		PublishedFileId_t unPublishedFileId = pCallback.m_unPublishedFileId;
		obj[5] = unPublishedFileId.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_nVotesFor.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_nVotesAgainst.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_nReports.ToString();
		obj[12] = " -- ";
		obj[13] = pCallback.m_fScore.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoteStoragePublishedFileSubscribed(RemoteStoragePublishedFileSubscribed_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			1321.ToString(),
			" - RemoteStoragePublishedFileSubscribed] - ",
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[3] = nPublishedFileId.ToString();
		obj[4] = " -- ";
		AppId_t nAppID = pCallback.m_nAppID;
		obj[5] = nAppID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoteStoragePublishedFileUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			1322.ToString(),
			" - RemoteStoragePublishedFileUnsubscribed] - ",
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[3] = nPublishedFileId.ToString();
		obj[4] = " -- ";
		AppId_t nAppID = pCallback.m_nAppID;
		obj[5] = nAppID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoteStoragePublishedFileDeleted(RemoteStoragePublishedFileDeleted_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			1323.ToString(),
			" - RemoteStoragePublishedFileDeleted] - ",
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[3] = nPublishedFileId.ToString();
		obj[4] = " -- ";
		AppId_t nAppID = pCallback.m_nAppID;
		obj[5] = nAppID.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoteStorageUpdateUserPublishedItemVoteResult(RemoteStorageUpdateUserPublishedItemVoteResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[6]
		{
			"[",
			1324.ToString(),
			" - RemoteStorageUpdateUserPublishedItemVoteResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoteStorageUserVoteDetails(RemoteStorageUserVoteDetails_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			1325.ToString(),
			" - RemoteStorageUserVoteDetails] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_eVote.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoteStorageEnumerateUserSharedWorkshopFilesResult(RemoteStorageEnumerateUserSharedWorkshopFilesResult_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 1326 + " - RemoteStorageEnumerateUserSharedWorkshopFilesResult] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId);
	}

	private void OnRemoteStorageSetUserPublishedFileActionResult(RemoteStorageSetUserPublishedFileActionResult_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[8]
		{
			"[",
			1327.ToString(),
			" - RemoteStorageSetUserPublishedFileActionResult] - ",
			pCallback.m_eResult.ToString(),
			" -- ",
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[5] = nPublishedFileId.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_eAction.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoteStorageEnumeratePublishedFilesByUserActionResult(RemoteStorageEnumeratePublishedFilesByUserActionResult_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 1328 + " - RemoteStorageEnumeratePublishedFilesByUserActionResult] - " + pCallback.m_eResult.ToString() + " -- " + pCallback.m_eAction.ToString() + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId?.ToString() + " -- " + pCallback.m_rgRTimeUpdated);
	}

	private void OnRemoteStoragePublishFileProgress(RemoteStoragePublishFileProgress_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 1329 + " - RemoteStoragePublishFileProgress] - " + pCallback.m_dPercentFile + " -- " + pCallback.m_bPreview);
	}

	private void OnRemoteStoragePublishedFileUpdated(RemoteStoragePublishedFileUpdated_t pCallback)
	{
		string[] obj = new string[8]
		{
			"[",
			1330.ToString(),
			" - RemoteStoragePublishedFileUpdated] - ",
			null,
			null,
			null,
			null,
			null
		};
		PublishedFileId_t nPublishedFileId = pCallback.m_nPublishedFileId;
		obj[3] = nPublishedFileId.ToString();
		obj[4] = " -- ";
		AppId_t nAppID = pCallback.m_nAppID;
		obj[5] = nAppID.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_ulUnused.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnRemoteStorageFileWriteAsyncComplete(RemoteStorageFileWriteAsyncComplete_t pCallback, bool bIOFailure)
	{
		Debug.Log("[" + 1331 + " - RemoteStorageFileWriteAsyncComplete] - " + pCallback.m_eResult);
	}

	private void OnRemoteStorageFileReadAsyncComplete(RemoteStorageFileReadAsyncComplete_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[10]
		{
			"[",
			1332.ToString(),
			" - RemoteStorageFileReadAsyncComplete] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		SteamAPICall_t hFileReadAsync = pCallback.m_hFileReadAsync;
		obj[3] = hFileReadAsync.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_eResult.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_nOffset.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_cubRead.ToString();
		Debug.Log(string.Concat(obj));
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			byte[] array = new byte[40];
			bool flag = SteamRemoteStorage.FileReadAsyncComplete(pCallback.m_hFileReadAsync, array, pCallback.m_cubRead);
			MonoBehaviour.print("FileReadAsyncComplete(m_FileReadAsyncHandle, Data, pCallback.m_cubRead) : " + flag);
			if (flag)
			{
				m_Message = Encoding.UTF8.GetString(array, (int)pCallback.m_nOffset, (int)pCallback.m_cubRead);
			}
		}
	}

	private void OnRemoteStorageLocalFileChange(RemoteStorageLocalFileChange_t pCallback)
	{
		Debug.Log("[" + 1333 + " - RemoteStorageLocalFileChange]");
	}
}
