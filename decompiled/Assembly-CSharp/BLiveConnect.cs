using System;
using System.Threading.Tasks;
using NativeWebSocket;
using Newtonsoft.Json;
using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEngine;

public class BLiveConnect : MonoBehaviour
{
	private WebSocketBLiveClient m_WebSocketBLiveClient;

	private InteractivePlayHeartBeat m_PlayHeartBeat;

	private string m_GameId;

	public string accessKeySecret;

	public string accessKeyId;

	public string appId;

	public bool Connected => m_WebSocketBLiveClient != null;

	public async Task LinkStart(string code, BLiveMgr mgr)
	{
		SignUtility.accessKeySecret = accessKeySecret;
		SignUtility.accessKeyId = accessKeyId;
		AppStartInfo appStartInfo = JsonConvert.DeserializeObject<AppStartInfo>(await BApi.StartInteractivePlay(code, appId));
		if (appStartInfo.Code != 0)
		{
			Debug.LogError(appStartInfo.Message);
			throw new Exception("连接失败(" + appStartInfo.Message + ")");
		}
		m_WebSocketBLiveClient = new WebSocketBLiveClient(appStartInfo.GetWssLink(), appStartInfo.GetAuthBody());
		try
		{
			m_WebSocketBLiveClient.Connect(TimeSpan.FromSeconds(1.0), 1000000);
			Debug.Log("连接成功");
		}
		catch (Exception)
		{
			Debug.Log("连接失败");
			throw;
		}
		m_WebSocketBLiveClient.OnDanmaku += mgr.OnDanmaku;
		m_WebSocketBLiveClient.OnGift += mgr.OnGift;
		m_WebSocketBLiveClient.OnLike += mgr.OnLike;
		m_GameId = appStartInfo.GetGameId();
		m_PlayHeartBeat = new InteractivePlayHeartBeat(m_GameId);
		m_PlayHeartBeat.HeartBeatError += mgr.OnLinkError;
		m_PlayHeartBeat.Start();
		Application.runInBackground = true;
	}

	public async Task LinkEnd()
	{
		m_WebSocketBLiveClient?.Dispose();
		m_PlayHeartBeat?.Dispose();
		await BApi.EndInteractivePlay(appId, m_GameId);
		m_WebSocketBLiveClient = null;
		m_PlayHeartBeat = null;
		Application.runInBackground = false;
	}

	private void Update()
	{
		WebSocketBLiveClient webSocketBLiveClient = m_WebSocketBLiveClient;
		if (webSocketBLiveClient != null)
		{
			WebSocket ws = webSocketBLiveClient.ws;
			if (ws != null && ws.State == WebSocketState.Open)
			{
				m_WebSocketBLiveClient.ws.DispatchMessageQueue();
			}
		}
	}

	private async void OnDestroy()
	{
		await LinkEnd();
	}
}
