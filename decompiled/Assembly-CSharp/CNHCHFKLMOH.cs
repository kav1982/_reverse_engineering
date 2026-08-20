using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using GameServer;
using UnityEngine;
using UnityEngine.Networking;

public static class CNHCHFKLMOH
{
	[CompilerGenerated]
	private sealed class CDJDGFIFPGO
	{
		public Action onSuccess;

		internal void _003CCheckRedirection_003Eb__0(Response<ServerAPI.Redirection> response)
		{
			Debug.Log(response.code);
			if (!ProcessLogMagicraftServerStatue(response.code).success)
			{
				GameUISingletonMono<UICommonHint>.HideIfInited();
				return;
			}
			if (response.data.redirection)
			{
				ClientSettings.Servers = response.data.servers;
			}
			onSuccess?.Invoke();
		}

		internal void _003CCheckRedirection_003Eb__1(UnityWebRequest error)
		{
			Debug.LogError("检查重定向时网络错误 err:" + error.error);
			GameUISingletonMono<UICommonHint>.HideIfInited();
			RetryRedirect(onSuccess);
		}
	}

	[CompilerGenerated]
	private sealed class CIGMKJNFEOI
	{
		public Action onRedirectionSuccess;

		public Action _003C_003E9__1;

		internal void _003CRetryRedirect_003Eb__0()
		{
			CheckRedirection(delegate
			{
				onRedirectionSuccess?.Invoke();
			});
			GameUISingletonMono<UICommonHintRetryOrQuit>.Inst.Hide();
		}

		internal void _003CRetryRedirect_003Eb__1()
		{
			onRedirectionSuccess?.Invoke();
		}
	}

	[CompilerGenerated]
	private sealed class GGMMCAHABEK
	{
		public int crtVersion;

		public bool haveForceUpdate;

		internal void _003CShowUpdate_003Eb__0(Response<ServerAPI.GameVersion[]> response)
		{
			if (!ProcessLogMagicraftServerStatue(response.code).success || response.data == null)
			{
				return;
			}
			if (response.data.Length == 0)
			{
				Debug.Log("没有更新的");
				return;
			}
			ServerAPI.GameVersion gameVersion = (FMLBOHOHHAN = response.data.Max());
			Debug.Log($"uiMainMenu.WaitForLogCheck -> 当前版本{crtVersion},服务器最新版本{gameVersion.version}");
			if (gameVersion.version <= crtVersion)
			{
				Debug.Log("没有更新的");
				return;
			}
			haveForceUpdate = gameVersion.force;
			UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate.Show((FMLBOHOHHAN.notice, (!gameVersion.force) ? UpdateNoticeType.Update : UpdateNoticeType.ForceUpdate));
		}

		internal void _003CShowUpdate_003Eb__1(UnityWebRequest error)
		{
			haveForceUpdate = true;
			Debug.LogError("uiMainMenu.WaitForLogCheck -> 检查版本错误" + error.error);
			GameUISingletonMono<UICommonHintRetryOrQuit>.ShowInit((Action)delegate
			{
				GameMgr.Inst.StartCoroutine(ShowUpdate());
				GameUISingletonMono<UICommonHintRetryOrQuit>.Inst.Hide();
			});
		}
	}

	[CompilerGenerated]
	private sealed class AOOGDHPCDFM
	{
		public Response verifyResult;

		public NEHJMMMHNNL serverLogCheckResult;

		public int checkVerifyCounter;

		public Response<ServerAPI.CheckVerifyResultData> checkResult;

		internal void _003CServerLogCheck_003Eb__0(Response response)
		{
			Debug.Log("魔法服务器请求成功");
			verifyResult = response;
			if (verifyResult.code == StatusCode.ServerUnderMaintenance)
			{
				verifyResult.code = StatusCode.Success;
			}
			(bool, string) tuple = ProcessLogMagicraftServerStatue(verifyResult.code, BNMDCFCCBNK: false);
			serverLogCheckResult.PNMGNIBFIFI = tuple.Item1;
			serverLogCheckResult.GJEHNKLPHMG += (serverLogCheckResult.PNMGNIBFIFI ? " " : tuple.Item2);
		}

		internal void _003CServerLogCheck_003Eb__1(UnityWebRequest error)
		{
			Debug.LogError(error.error);
			serverLogCheckResult.DGNDBFIOOMF = true;
			serverLogCheckResult.PNMGNIBFIFI = false;
			serverLogCheckResult.GJEHNKLPHMG = "服务器连接失败";
		}

		internal void _003CServerLogCheck_003Eb__2(Response<ServerAPI.CheckVerifyResultData> checkVerifyResult)
		{
			checkVerifyCounter++;
			checkResult = checkVerifyResult;
			serverLogCheckResult.PNMGNIBFIFI = false;
			Debug.Log("B站服务器请求成功");
		}

		internal void _003CServerLogCheck_003Eb__3(UnityWebRequest error)
		{
			Debug.LogError("B站服务器请求失败" + error.error);
			serverLogCheckResult.PNMGNIBFIFI = false;
			serverLogCheckResult.DGNDBFIOOMF = true;
			serverLogCheckResult.GJEHNKLPHMG = "检查登录失败";
			checkVerifyCounter = 999;
		}

		internal void _003CServerLogCheck_003Eb__11(Response response)
		{
			serverLogCheckResult.PNMGNIBFIFI = true;
			if (response.code == StatusCode.Success)
			{
				Debug.Log("创建角色成功");
			}
			else if (response.code == StatusCode.RoleAlreadyCreated)
			{
				Debug.Log("角色已经存在");
			}
		}

		internal void _003CServerLogCheck_003Eb__12(UnityWebRequest error)
		{
			Debug.LogError(error.error);
			serverLogCheckResult.DGNDBFIOOMF = true;
			serverLogCheckResult.PNMGNIBFIFI = false;
			serverLogCheckResult.GJEHNKLPHMG += "创建角色请求失败";
		}

		internal void _003CServerLogCheck_003Eb__4(Response<ServerAPI.CheckHarmoniousResult> checkHarmoniousResult)
		{
			serverLogCheckResult.GKPHBHBIELH = checkHarmoniousResult.data == null || checkHarmoniousResult.data.harmonious;
		}

		internal void _003CServerLogCheck_003Eb__5(UnityWebRequest error)
		{
			Debug.LogError(error.error);
			serverLogCheckResult.DGNDBFIOOMF = true;
			serverLogCheckResult.GJEHNKLPHMG = "服务器连接失败";
		}

		internal void _003CServerLogCheck_003Eb__7(UnityWebRequest error)
		{
			serverLogCheckResult.DGNDBFIOOMF = true;
			serverLogCheckResult.GJEHNKLPHMG = "服务器连接失败";
		}
	}

	[CompilerGenerated]
	private sealed class EMMCDDEOGBA
	{
		public NEHJMMMHNNL result;

		internal void _003CServerLogCheck_003Eb__14(Response<ServerAPI.Notice[]> maintainNotices)
		{
			Debug.Log("登录展示公告");
			if (maintainNotices.data == null || maintainNotices.data.Length == 0)
			{
				GameMgr.Inst.StartCoroutine(ShowUpdate());
			}
			else if (maintainNotices.data.Length != 0)
			{
				UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate.gameObject.SetActive(value: true);
				UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate.Show((maintainNotices.data[0], UpdateNoticeType.Login));
				UINeedUpdatePenel uINeedUpdate = UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate;
				uINeedUpdate.ActionOnClose = (Action)Delegate.Combine(uINeedUpdate.ActionOnClose, (Action)delegate
				{
					GameMgr.Inst.StartCoroutine(ShowUpdate());
				});
			}
			if (result.JMHJBKIFDNP)
			{
				GameUISingletonMono<UICommonHint>.ShowInit(result.GJEHNKLPHMG);
			}
			else
			{
				GameUISingletonMono<UICommonHint>.HideIfInited();
			}
		}

		internal void _003CServerLogCheck_003Eb__15(UnityWebRequest error)
		{
			result.GJEHNKLPHMG += "\n获取公告失败";
			GameUISingletonMono<UICommonHint>.ShowInit(result.GJEHNKLPHMG);
		}

		internal void _003CServerLogCheck_003Eb__16(Response<ServerAPI.Notice> maintainNotice)
		{
			if (maintainNotice.data == null)
			{
				GameUISingletonMono<UICommonHint>.ShowInit(result.GJEHNKLPHMG);
				return;
			}
			GameUISingletonMono<UICommonHint>.HideIfInited();
			UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate.gameObject.SetActive(value: true);
			UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate.Show((maintainNotice.data, UpdateNoticeType.Maintain));
		}

		internal void _003CServerLogCheck_003Eb__17(UnityWebRequest error)
		{
			GameUISingletonMono<UICommonHint>.ShowInit(result.GJEHNKLPHMG);
		}
	}

	[CompilerGenerated]
	private sealed class MLNEABMLOND
	{
		public Action onPassCheck;

		public Action onError;

		internal void _003CCheckServerEnvironment_003Eb__0(Response<string> response)
		{
			if (ProcessLogMagicraftServerStatue(response.code).success)
			{
				GIOEIPNCMEF = response.data;
				onPassCheck?.Invoke();
				Debug.Log("-------------当前SeverEnviroment：" + GIOEIPNCMEF + "-------------");
			}
		}

		internal void _003CCheckServerEnvironment_003Eb__1(UnityWebRequest err)
		{
			Debug.LogError("ServerMgr.SyncServerEnvironment -> 请求错误 " + err.error);
			onError?.Invoke();
		}
	}

	[CompilerGenerated]
	private sealed class BILIPBAOPAL
	{
		public Action<ServerAPI.GameVersion> newestNoticeVersion;

		internal void _003CGetNewestNoticeId_003Eb__0(Response<ServerAPI.GameVersion[]> response)
		{
			if (ProcessLogMagicraftServerStatue(response.code).success)
			{
				ServerAPI.GameVersion obj = response.data.Max();
				newestNoticeVersion(obj);
			}
		}
	}

	public static string GIOEIPNCMEF;

	public static string HEEOAGAGKHF;

	public static string[] CKKOFIICIKF;

	public static readonly string PLACCDNLMJH = "disableCDkey";

	public static ServerAPI.GameVersion FMLBOHOHHAN;

	public static void CheckServerEnvironment(Action MEPIKPPAEBN = null, Action DOBHACDACLB = null)
	{
		Debug.Log("ServerMgr.CheckServerEnvironment -> 检查服务器环境");
		IEnumerator serverName = ServerAPI.GetServerName(delegate(Response<string> response)
		{
			if (ProcessLogMagicraftServerStatue(response.code).success)
			{
				GIOEIPNCMEF = response.data;
				MEPIKPPAEBN?.Invoke();
				Debug.Log("-------------当前SeverEnviroment：" + GIOEIPNCMEF + "-------------");
			}
		}, delegate(UnityWebRequest err)
		{
			Debug.LogError("ServerMgr.SyncServerEnvironment -> 请求错误 " + err.error);
			DOBHACDACLB?.Invoke();
		});
		GameMgr.Inst.StartCoroutine(serverName);
	}

	private static IEnumerator RequestIPAddress(Action<string> ENKANKKKIPF)
	{
		string uri = "https://api.ipify.org";
		using UnityWebRequest request = UnityWebRequest.Get(uri);
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			ENKANKKKIPF(request.downloadHandler.text);
		}
		else
		{
			Debug.LogError("Failed to get public IP: " + request.error);
		}
	}

	public static void SyncIpAddress()
	{
		IEnumerator routine = RequestIPAddress(delegate(string response)
		{
			HEEOAGAGKHF = response;
		});
		GameMgr.Inst.StartCoroutine(routine);
	}

	public static (bool success, string errorInfo) ProcessLogMagicraftServerStatue(StatusCode AFEBAPNGBOH, bool BNMDCFCCBNK = true)
	{
		string text = "";
		bool flag = false;
		switch (AFEBAPNGBOH)
		{
		case StatusCode.Success:
		case StatusCode.LoginTooOften:
			flag = true;
			text = "";
			break;
		case StatusCode.DisabledAccount:
			text = "账号被禁用";
			break;
		case StatusCode.NoVerifyInfo:
		case StatusCode.UnknownArea:
		case StatusCode.VersionExists:
		case StatusCode.LostFiles:
		case StatusCode.VersionNotFound:
			text = AFEBAPNGBOH.ToString();
			break;
		case StatusCode.AccountNotFound:
			text = "未找到账户";
			break;
		case StatusCode.RoleAlreadyCreated:
			text = "角色已创建";
			break;
		case StatusCode.CDKeyNotFound:
		case StatusCode.CDKeyUnavailable:
		case StatusCode.CDKeyAlreadyUsed:
			text = "激活码不可用";
			break;
		case StatusCode.NoticeNotFound:
			text = "获取更新公告失败";
			break;
		case StatusCode.ServerUnderMaintenance:
			text = "服务器正在维护";
			break;
		case StatusCode.ServerUnderMaintenanceDisableTester:
			text = "服务器正在维护";
			break;
		case StatusCode.ServerNotUnderMaintenance:
			text = "获取公告失败";
			break;
		case StatusCode.CommodityNotFound:
		case StatusCode.CommodityAlreadyHave:
			text = "无法购买商品";
			break;
		case StatusCode.OrderNotFound:
			text = "订单验证失败";
			break;
		}
		if (!flag && BNMDCFCCBNK)
		{
			GameUISingletonMono<UIServerCode>.ShowInit(text);
		}
		return (flag, text);
	}

	public static void GetNewestNoticeId(Action<ServerAPI.GameVersion> MBGJCLKDCHK)
	{
		IEnumerator routine = ServerAPI.CheckNewVersion(VersionSO.Inst.AsInt(), delegate(Response<ServerAPI.GameVersion[]> response)
		{
			if (ProcessLogMagicraftServerStatue(response.code).success)
			{
				ServerAPI.GameVersion obj = response.data.Max();
				MBGJCLKDCHK(obj);
			}
		}, delegate(UnityWebRequest error)
		{
			Debug.LogError("uiMainMenu.WaitForLogCheck -> 检查版本错误" + error.error);
		});
		GameMgr.Inst.StartCoroutine(routine);
	}

	public static void StartLog()
	{
		if (ScriptableObjMgr.Inst.testCtrller.UseServer)
		{
			uiMainMenu.SetStartButtonToLog();
			CheckRedirection(delegate
			{
				CheckServerEnvironment(Login, delegate
				{
					RetryRedirect(Login);
				});
			});
		}
		else
		{
			Login();
		}
		static void Login()
		{
		}
	}

	public static void CheckRedirection(Action IKGBPGLMDBM)
	{
		GameUISingletonMono<UICommonHint>.ShowInit(("检查服务器状态", false));
		GameMgr.Inst.StartCoroutine(ServerAPI.CheckRedirection(VersionSO.Inst.AsInt(), delegate(Response<ServerAPI.Redirection> response)
		{
			Debug.Log(response.code);
			if (!ProcessLogMagicraftServerStatue(response.code).success)
			{
				GameUISingletonMono<UICommonHint>.HideIfInited();
			}
			else
			{
				if (response.data.redirection)
				{
					ClientSettings.Servers = response.data.servers;
				}
				IKGBPGLMDBM?.Invoke();
			}
		}, delegate(UnityWebRequest error)
		{
			Debug.LogError("检查重定向时网络错误 err:" + error.error);
			GameUISingletonMono<UICommonHint>.HideIfInited();
			RetryRedirect(IKGBPGLMDBM);
		}));
	}

	public static void RetryRedirect(Action KHIKEGCMDCL = null)
	{
		GameUISingletonMono<UICommonHintRetryOrQuit>.ShowInit((Action)delegate
		{
			CheckRedirection(delegate
			{
				KHIKEGCMDCL?.Invoke();
			});
			GameUISingletonMono<UICommonHintRetryOrQuit>.Inst.Hide();
		});
	}

	public static void ServerCheckAfterLog()
	{
		MobileMgr.inst.PluginActivity.SetClientSetting();
		if (!ScriptableObjMgr.Inst.testCtrller.UseServer)
		{
			Debug.Log("可以开始游戏");
			uiMainMenu.SetStartButtonToStart();
			return;
		}
		uiMainMenu.SetStartButtonToLog();
		if (GameMgr.Inst.IElogCheck == null)
		{
			GameUISingletonMono<UICommonHint>.ShowInit(("正在登陆检查", false));
			GameMgr.Inst.IElogCheck = GameMgr.Inst.StartCoroutine(ServerLogCheck());
		}
	}

	public static void ShowCurrentVersion()
	{
		UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate.Show(VersionSO.Inst.AsInt());
	}

	public static IEnumerator ShowUpdate()
	{
		Debug.Log("uiMainMenu.WaitForLogCheck -> 检查更新");
		bool haveForceUpdate = false;
		int crtVersion = VersionSO.Inst.AsInt();
		FMLBOHOHHAN = new ServerAPI.GameVersion
		{
			version = crtVersion
		};
		yield return ServerAPI.CheckNewVersion(crtVersion, delegate(Response<ServerAPI.GameVersion[]> response)
		{
			if (ProcessLogMagicraftServerStatue(response.code).success && response.data != null)
			{
				if (response.data.Length == 0)
				{
					Debug.Log("没有更新的");
				}
				else
				{
					ServerAPI.GameVersion gameVersion = (FMLBOHOHHAN = response.data.Max());
					Debug.Log($"uiMainMenu.WaitForLogCheck -> 当前版本{crtVersion},服务器最新版本{gameVersion.version}");
					if (gameVersion.version <= crtVersion)
					{
						Debug.Log("没有更新的");
					}
					else
					{
						haveForceUpdate = gameVersion.force;
						UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate.Show((FMLBOHOHHAN.notice, (!gameVersion.force) ? UpdateNoticeType.Update : UpdateNoticeType.ForceUpdate));
					}
				}
			}
		}, delegate(UnityWebRequest error)
		{
			haveForceUpdate = true;
			Debug.LogError("uiMainMenu.WaitForLogCheck -> 检查版本错误" + error.error);
			GameUISingletonMono<UICommonHintRetryOrQuit>.ShowInit((Action)delegate
			{
				GameMgr.Inst.StartCoroutine(ShowUpdate());
				GameUISingletonMono<UICommonHintRetryOrQuit>.Inst.Hide();
			});
		});
	}

	private static IEnumerator ServerLogCheck()
	{
		Response<ServerAPI.CheckVerifyResultData> checkResult = null;
		NEHJMMMHNNL serverLogCheckResult = new NEHJMMMHNNL
		{
			DGNDBFIOOMF = false,
			PNMGNIBFIFI = false,
			GKPHBHBIELH = false,
			GJEHNKLPHMG = "",
			FHJABPPANLJ = false,
			JMHJBKIFDNP = false,
			LJAOAFHCFCI = false
		};
		int checkVerifyCounter = 0;
		Response verifyResult;
		yield return ServerAPI.StartVerify(delegate(Response response)
		{
			Debug.Log("魔法服务器请求成功");
			verifyResult = response;
			if (verifyResult.code == StatusCode.ServerUnderMaintenance)
			{
				verifyResult.code = StatusCode.Success;
			}
			(bool, string) tuple2 = ProcessLogMagicraftServerStatue(verifyResult.code, BNMDCFCCBNK: false);
			serverLogCheckResult.PNMGNIBFIFI = tuple2.Item1;
			serverLogCheckResult.GJEHNKLPHMG += (serverLogCheckResult.PNMGNIBFIFI ? " " : tuple2.Item2);
		}, delegate(UnityWebRequest error)
		{
			Debug.LogError(error.error);
			serverLogCheckResult.DGNDBFIOOMF = true;
			serverLogCheckResult.PNMGNIBFIFI = false;
			serverLogCheckResult.GJEHNKLPHMG = "服务器连接失败";
		});
		if (serverLogCheckResult.PNMGNIBFIFI)
		{
			Debug.Log("检查登录状态");
			do
			{
				yield return new WaitForSeconds(0.5f);
				yield return ServerAPI.CheckVerifyResult(delegate(Response<ServerAPI.CheckVerifyResultData> checkVerifyResult)
				{
					checkVerifyCounter++;
					checkResult = checkVerifyResult;
					serverLogCheckResult.PNMGNIBFIFI = false;
					Debug.Log("B站服务器请求成功");
				}, delegate(UnityWebRequest error)
				{
					Debug.LogError("B站服务器请求失败" + error.error);
					serverLogCheckResult.PNMGNIBFIFI = false;
					serverLogCheckResult.DGNDBFIOOMF = true;
					serverLogCheckResult.GJEHNKLPHMG = "检查登录失败";
					checkVerifyCounter = 999;
				});
			}
			while (checkVerifyCounter < 5 && checkResult.code == StatusCode.NoVerifyInfo);
		}
		if (checkResult != null)
		{
			(bool, string) tuple = ProcessLogMagicraftServerStatue(checkResult.code, BNMDCFCCBNK: false);
			serverLogCheckResult.PNMGNIBFIFI = tuple.Item1;
			serverLogCheckResult.GJEHNKLPHMG = tuple.Item2;
			if (serverLogCheckResult.PNMGNIBFIFI)
			{
				if (checkResult.data.biliStatus == BiliStatusCode.Success)
				{
					if (checkResult.data.ban)
					{
						serverLogCheckResult.GJEHNKLPHMG = "登录失败:账号被禁";
					}
					else if (checkResult.data.dev)
					{
						serverLogCheckResult.GJEHNKLPHMG = "登录成功:控制台开启";
					}
					else if (checkResult.data.tester)
					{
						serverLogCheckResult.GJEHNKLPHMG = "登录成功:测试用户";
					}
					ICJNOGPFMAM.KEMAJLGHMEL.SetAllItem(checkResult.data.items);
					ICJNOGPFMAM.KEMAJLGHMEL.SetExtensionInfo(checkResult.data.extension_info);
					serverLogCheckResult.FHJABPPANLJ = checkResult.data.tester;
					serverLogCheckResult.JMHJBKIFDNP = checkResult.data.dev;
					serverLogCheckResult.PNMGNIBFIFI = !checkResult.data.ban;
					serverLogCheckResult.LJAOAFHCFCI = checkResult.data.created_role;
					if (!serverLogCheckResult.LJAOAFHCFCI)
					{
						Debug.Log("创建角色");
						MobileMgr.inst.PluginActivity.CreateCharacter();
						yield return ServerAPI.CreateRole(delegate(Response response)
						{
							serverLogCheckResult.PNMGNIBFIFI = true;
							if (response.code == StatusCode.Success)
							{
								Debug.Log("创建角色成功");
							}
							else if (response.code == StatusCode.RoleAlreadyCreated)
							{
								Debug.Log("角色已经存在");
							}
						}, delegate(UnityWebRequest error)
						{
							Debug.LogError(error.error);
							serverLogCheckResult.DGNDBFIOOMF = true;
							serverLogCheckResult.PNMGNIBFIFI = false;
							serverLogCheckResult.GJEHNKLPHMG += "创建角色请求失败";
						});
					}
					MobileMgr.inst.PluginActivity.NotifyZone();
				}
				else
				{
					serverLogCheckResult.PNMGNIBFIFI = false;
					serverLogCheckResult.GJEHNKLPHMG = "登陆失败";
					Debug.Log(checkResult.data.biliStatus);
				}
			}
		}
		if (checkResult != null && serverLogCheckResult.PNMGNIBFIFI)
		{
			yield return ServerAPI.CheckHarmonious(delegate(Response<ServerAPI.CheckHarmoniousResult> checkHarmoniousResult)
			{
				serverLogCheckResult.GKPHBHBIELH = checkHarmoniousResult.data == null || checkHarmoniousResult.data.harmonious;
			}, delegate(UnityWebRequest error)
			{
				Debug.LogError(error.error);
				serverLogCheckResult.DGNDBFIOOMF = true;
				serverLogCheckResult.GJEHNKLPHMG = "服务器连接失败";
			});
			yield return ServerAPI.GetServerTags(delegate(Response<string[]> getServerTagsResult)
			{
				if (getServerTagsResult.data != null)
				{
					CKKOFIICIKF = getServerTagsResult.data;
				}
			}, delegate
			{
				serverLogCheckResult.DGNDBFIOOMF = true;
				serverLogCheckResult.GJEHNKLPHMG = "服务器连接失败";
			});
			yield return ServerAPI.GetAllCommodity(delegate(Response<ServerAPI.CommodityInfo[]> response)
			{
				ICJNOGPFMAM.KEMAJLGHMEL.DGNJLGDEMAP = response.data;
			}, delegate
			{
				Debug.Log("获取服务器价格表失败,将显示默认价格");
				ICJNOGPFMAM.KEMAJLGHMEL.DGNJLGDEMAP = null;
			});
		}
		Debug.Log("请求结束");
		yield return ProcessResult(serverLogCheckResult);
		IEnumerator ProcessResult(NEHJMMMHNNL result)
		{
			if (result.DGNDBFIOOMF)
			{
				GameUISingletonMono<UICommonHintRetryOrQuit>.ShowInit((Action)delegate
				{
					GameMgr.Inst.StartCoroutine(ServerLogCheck());
					GameUISingletonMono<UICommonHintRetryOrQuit>.Inst.Hide();
				});
			}
			else if (result.PNMGNIBFIFI)
			{
				ScriptableObjMgr.Inst.testCtrller.harmonyScale = (result.GKPHBHBIELH ? TestHarmonyScale.HarmonyForPlayer14OrOlder : TestHarmonyScale.NoHarmony);
				ScriptableObjMgr.Inst.testCtrller.publishTesting = result.GKPHBHBIELH;
				DataMgr.UpdateData();
				uiMainMenu.SetStartButtonToStart();
				UIMgr.Inst.uiSetting.goMobileTestButton.SetActive(result.JMHJBKIFDNP);
				ScriptableObjMgr.Inst.testCtrller.Shortcut = result.JMHJBKIFDNP;
				ScriptableObjMgr.Inst.testCtrller.CommandLine = result.JMHJBKIFDNP;
				PluginActivity.ServerLogged = true;
				yield return ServerAPI.GetDirectDisplayNotices(delegate(Response<ServerAPI.Notice[]> maintainNotices)
				{
					Debug.Log("登录展示公告");
					if (maintainNotices.data == null || maintainNotices.data.Length == 0)
					{
						GameMgr.Inst.StartCoroutine(ShowUpdate());
					}
					else if (maintainNotices.data.Length != 0)
					{
						UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate.gameObject.SetActive(value: true);
						UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate.Show((maintainNotices.data[0], UpdateNoticeType.Login));
						UINeedUpdatePenel uINeedUpdate = UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate;
						uINeedUpdate.ActionOnClose = (Action)Delegate.Combine(uINeedUpdate.ActionOnClose, (Action)delegate
						{
							GameMgr.Inst.StartCoroutine(ShowUpdate());
						});
					}
					if (result.JMHJBKIFDNP)
					{
						GameUISingletonMono<UICommonHint>.ShowInit(result.GJEHNKLPHMG);
					}
					else
					{
						GameUISingletonMono<UICommonHint>.HideIfInited();
					}
				}, delegate
				{
					result.GJEHNKLPHMG += "\n获取公告失败";
					GameUISingletonMono<UICommonHint>.ShowInit(result.GJEHNKLPHMG);
				});
			}
			else
			{
				if (checkResult != null)
				{
					StatusCode code = checkResult.code;
					if (code == StatusCode.ServerUnderMaintenanceDisableTester || code == StatusCode.ServerUnderMaintenance)
					{
						Debug.Log("显示维护公告");
						yield return ServerAPI.GetMaintainNotice(delegate(Response<ServerAPI.Notice> maintainNotice)
						{
							if (maintainNotice.data == null)
							{
								GameUISingletonMono<UICommonHint>.ShowInit(result.GJEHNKLPHMG);
							}
							else
							{
								GameUISingletonMono<UICommonHint>.HideIfInited();
								UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate.gameObject.SetActive(value: true);
								UIMainMenuMgr.Inst.uiMainMenu.UINeedUpdate.Show((maintainNotice.data, UpdateNoticeType.Maintain));
							}
						}, delegate
						{
							GameUISingletonMono<UICommonHint>.ShowInit(result.GJEHNKLPHMG);
						});
						goto IL_01c6;
					}
				}
				GameUISingletonMono<UICommonHint>.ShowInit(result.GJEHNKLPHMG);
			}
			goto IL_01c6;
			IL_01c6:
			GameMgr.Inst.IElogCheck = null;
		}
	}

	public static bool CheckCurrentServerTag(string NDCJNNJKCLD)
	{
		if (CKKOFIICIKF == null || CKKOFIICIKF.Length == 0)
		{
			return false;
		}
		string channleID = PluginActivity.channleID;
		string[] cKKOFIICIKF = CKKOFIICIKF;
		for (int i = 0; i < cKKOFIICIKF.Length; i++)
		{
			string[] array = cKKOFIICIKF[i].Split(':');
			if (array.Length >= 2 && !(array[0] != NDCJNNJKCLD) && ("," + array[1] + ",").Contains("," + channleID + ","))
			{
				return true;
			}
		}
		return false;
	}
}
