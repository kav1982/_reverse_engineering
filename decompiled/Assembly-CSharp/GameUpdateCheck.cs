using System;
using System.Collections;
using System.IO;
using System.Linq;
using IFix.Core;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameUpdateCheck : MonoBehaviour
{
	public WebRequestData.FileDownloadInfo[] downloadTest;

	private string updatePackPath;

	public TestController testController;

	public int maxRetryAttempts = 3;

	private void Start()
	{
		updatePackPath = Application.persistentDataPath + "/Packs/";
		if (Application.isMobilePlatform || testController.AndroidTesting)
		{
			StartCoroutine(DownloadPatches(downloadTest));
		}
		else
		{
			SceneManager.LoadScene("Entry");
		}
	}

	private IEnumerator CheckHarmonious()
	{
		Debug.Log("和谐测试");
		UnityWebRequest request = UnityWebRequest.Get("http://192.168.1.125:4455/game/h");
		request.SetRequestHeader("accept", "application/json");
		request.SetRequestHeader("area", "1");
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.LogError(request.error);
			yield break;
		}
		long responseCode = request.responseCode;
		if (responseCode == 200)
		{
			Debug.Log("Request succeeded");
			WebRequestData.Response<WebRequestData.HarmoniousData> response = JsonConvert.DeserializeObject<WebRequestData.Response<WebRequestData.HarmoniousData>>(request.downloadHandler.text);
			if (response != null && response.data != null)
			{
				if (!response.data.harmonious)
				{
				}
			}
			else
			{
				Debug.LogError("返回数据错误");
			}
		}
		else
		{
			Debug.LogError($"连接失败: {responseCode}");
		}
	}

	private IEnumerator DownloadPatches(WebRequestData.FileDownloadInfo[] versions)
	{
		foreach (WebRequestData.FileDownloadInfo fileDownloadInfo in versions)
		{
			bool success = false;
			yield return StartCoroutine(DownloadFileWithRetry(fileDownloadInfo.url, updatePackPath + fileDownloadInfo.version, maxRetryAttempts, delegate(bool result)
			{
				success = result;
			}));
			if (!success)
			{
				Debug.LogError("下载失败");
				yield break;
			}
		}
		Debug.Log("所有下载完成");
		ApplyUpdatePacks();
	}

	private IEnumerator DownloadFileWithRetry(string url, string savePath, int maxAttempts, Action<bool> onComplete)
	{
		Debug.Log(savePath);
		bool success = false;
		int attempts = 0;
		while (attempts < maxAttempts && !success)
		{
			attempts++;
			UnityWebRequest request = UnityWebRequest.Get(url);
			yield return request.SendWebRequest();
			if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.LogError("一次下载失败" + url + ": " + request.error);
			}
			else if (request.responseCode == 200)
			{
				if (!Directory.Exists(savePath))
				{
					Directory.CreateDirectory(savePath);
					Debug.Log("创建了路径:" + savePath);
				}
				string fileName = Path.GetFileName(url);
				File.WriteAllBytes(Path.Combine(savePath, fileName), request.downloadHandler.data);
				Debug.Log("下载" + url + "成功");
				success = true;
			}
			if (!success && attempts < maxAttempts)
			{
				Debug.Log($"重新下载 {url}, (尝试: {attempts + 1}/{maxAttempts})");
				yield return new WaitForSeconds(2f);
			}
		}
		if (!success)
		{
			Debug.Log("下载" + url + "失败");
		}
		onComplete(success);
	}

	private string[] GetDirectories(string path)
	{
		if (Directory.Exists(path))
		{
			string[] directories = Directory.GetDirectories(path);
			for (int i = 0; i < directories.Length; i++)
			{
				directories[i] = Path.GetFileName(directories[i]);
			}
			return directories;
		}
		Debug.LogError("目录不存在" + path);
		return new string[0];
	}

	public void ApplyUpdatePacks()
	{
		int num = GetDirectories(updatePackPath).Count();
		int num2 = 0;
		string[] directories = GetDirectories(updatePackPath);
		foreach (string text in directories)
		{
			Debug.Log("应用版本更新: " + text);
			if (ApplyOnePack(updatePackPath + text))
			{
				num2++;
			}
		}
		if (num == num2)
		{
			Debug.Log("应用了所有更新");
			StartCoroutine(EnterGame());
		}
		else
		{
			Debug.Log("应用更新失败,退出游戏?");
		}
	}

	private bool ApplyOnePack(string path)
	{
		return ScriptInject(path);
	}

	private bool ScriptInject(string dataPath)
	{
		try
		{
			PatchManager.Load(new MemoryStream(File.ReadAllBytes(dataPath + "Assembly-CSharp.patch.bytes")));
			PatchManager.Load(new MemoryStream(File.ReadAllBytes(dataPath + "Assembly-CSharp-firstpass.patch.bytes")));
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			return true;
		}
		return true;
	}

	private IEnumerator EnterGame()
	{
		Debug.Log("进入游戏");
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene("Entry");
	}
}
