using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace GameServer;

public class Request
{
	private struct HeadersModel
	{
		public string uid;

		public string token;

		public string sign;

		public int area;

		public int brand;

		public int channel;

		public int client_version;
	}

	private static int retryCounter;

	public string api;

	public Dictionary<string, object> parameters;

	public string jsonBody;

	public RequestMethod method;

	public Request(string api, Dictionary<string, object> parameters = null)
	{
		this.api = api;
		this.parameters = parameters;
		method = RequestMethod.Get;
	}

	public Request(string api, string jsonBody, Dictionary<string, object> parameters = null)
	{
		this.api = api;
		this.parameters = parameters;
		this.jsonBody = jsonBody;
		method = RequestMethod.Post;
	}

	public IEnumerator SendAndPlayback<T>(Action<T> callback, Action<UnityWebRequest> errorCallback)
	{
		int currentRequestRetryCounter = 0;
		UnityWebRequest request = Build();
		while (currentRequestRetryCounter < 3)
		{
			yield return request.SendWebRequest();
			if (request.responseCode != 0L)
			{
				break;
			}
			currentRequestRetryCounter++;
			retryCounter++;
			Debug.LogError(request.url + " 请求失败");
			request = Build();
		}
		if (request.responseCode != 200)
		{
			errorCallback(request);
			yield break;
		}
		try
		{
			string text = request.downloadHandler.text;
			if (request.GetResponseHeaders().TryGetValue("x-k", out var value))
			{
				text = EncryptDecrypt(text, value + ClientSettings.EncryptKey);
			}
			T val = JsonConvert.DeserializeObject<T>(text);
			if (val == null)
			{
				errorCallback(request);
			}
			else
			{
				callback(val);
			}
		}
		catch (JsonReaderException ex)
		{
			errorCallback(request);
			Debug.LogError("Request.SendAndPlayback -> 解析Json出错" + request.downloadHandler.text + " " + ex);
			throw;
		}
	}

	private UnityWebRequest Build()
	{
		string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
		object obj = method switch
		{
			RequestMethod.Get => BuildGetRequire(api, timestamp, parameters), 
			RequestMethod.Post => BuildPostRequire(api, jsonBody, timestamp, parameters), 
			_ => null, 
		};
		((UnityWebRequest)obj).timeout = 8;
		return (UnityWebRequest)obj;
	}

	private static UnityWebRequest BuildPostRequire(string api, string json, string timestamp, Dictionary<string, object> parameters = null)
	{
		UnityWebRequest unityWebRequest = UnityWebRequest.PostWwwForm(GetServerUrl(api, timestamp, parameters), json);
		unityWebRequest.timeout = 8;
		ApplyClientSettingsToHeader(unityWebRequest, timestamp);
		return unityWebRequest;
	}

	private static UnityWebRequest BuildGetRequire(string api, string timestamp, Dictionary<string, object> parameters = null)
	{
		UnityWebRequest unityWebRequest = UnityWebRequest.Get(GetServerUrl(api, timestamp, parameters));
		unityWebRequest.timeout = 8;
		ApplyClientSettingsToHeader(unityWebRequest, timestamp);
		return unityWebRequest;
	}

	private static string GetServerUrl(string api, string timestamp, Dictionary<string, object> parameters = null)
	{
		string text = GetServerUrlPrefix() + api;
		if (parameters != null)
		{
			string text2 = string.Join("&", parameters.Select((KeyValuePair<string, object> e) => e.Key + "=" + e.Value));
			text = ((!ClientSettings.Encrypt) ? (text + "?" + text2) : (text + "?q=" + ToBase64UrlSafe(EncryptDecrypt(text2, ClientSettings.EncryptKey + timestamp))));
		}
		return text;
	}

	private static string GetServerUrlPrefix()
	{
		int num = retryCounter % ClientSettings.Servers.Length;
		return ClientSettings.Servers[num];
	}

	private static void ApplyClientSettingsToHeader(UnityWebRequest request, string timestamp)
	{
		HeadersModel headersModel = default(HeadersModel);
		headersModel.area = ClientSettings.AreaId;
		headersModel.brand = ClientSettings.BrandId;
		headersModel.channel = ClientSettings.ChannelId;
		headersModel.token = ClientSettings.Token;
		headersModel.uid = ClientSettings.Uid;
		headersModel.client_version = 0;
		HeadersModel headersModel2 = headersModel;
		if (!string.IsNullOrWhiteSpace(ClientSettings.SignKey))
		{
			headersModel2.sign = GetMD5Hash(timestamp + ClientSettings.SignKey);
		}
		if (ClientSettings.Encrypt)
		{
			string input = JsonConvert.SerializeObject(headersModel2);
			request.SetRequestHeader("d", ToBase64UrlSafe(EncryptDecrypt(input, ClientSettings.EncryptKey + timestamp)));
			request.SetRequestHeader("t", timestamp);
			return;
		}
		request.SetRequestHeader("t", timestamp);
		request.SetRequestHeader("area", headersModel2.area.ToString());
		request.SetRequestHeader("brand", headersModel2.brand.ToString());
		request.SetRequestHeader("channel", headersModel2.channel.ToString());
		request.SetRequestHeader("token", headersModel2.token);
		request.SetRequestHeader("uid", headersModel2.uid);
		request.SetRequestHeader("client_version", headersModel2.client_version.ToString());
		request.SetRequestHeader("sign", headersModel2.sign);
		request.SetRequestHeader("timestamp", timestamp);
	}

	private static string GetMD5Hash(string input)
	{
		using MD5 mD = MD5.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.Append(b.ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	private static string EncryptDecrypt(string input, string key)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] bytes2 = Encoding.UTF8.GetBytes(key);
		byte[] array = new byte[bytes.Length];
		for (int i = 0; i < bytes.Length; i++)
		{
			array[i] = (byte)(bytes[i] ^ bytes2[i % bytes2.Length]);
		}
		return Encoding.UTF8.GetString(array);
	}

	private static string ToBase64UrlSafe(string plainText)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText)).Replace('+', '-').Replace('/', '_');
	}
}
