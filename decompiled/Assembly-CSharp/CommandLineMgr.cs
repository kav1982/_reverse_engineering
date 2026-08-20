using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OpenBLive.Runtime.Data;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CommandLineMgr : MonoBehaviour
{
	private const string PlayerPrefs_CMD_History_Key = "CMD_History";

	public GameObject go_CommandLine;

	public Text debugText;

	public ScrollRect debugScrollUI;

	public TMP_InputField debugCmdInputField;

	public bool debugSysActive;

	public string color_Info = "#B2B2B2";

	private Dictionary<string, string> cmdList = new Dictionary<string, string>();

	private Dictionary<string, object> debugValues = new Dictionary<string, object>();

	private List<string> cmdHistory = new List<string>();

	private int historyArrow = -1;

	private bool isGodMode;

	private bool autoCommandIsRun;

	private EntityManager ettMgr;

	public static CommandLineMgr Inst { get; private set; }

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (PlayerPrefs.HasKey("CMD_History"))
		{
			string @string = PlayerPrefs.GetString("CMD_History");
			cmdHistory.AddRange(from e in @string.Split("\n")
				select e.Trim() into e
				where e.Length > 0
				select e);
			historyArrow = cmdHistory.Count - 1;
		}
	}

	private void Update()
	{
		if (!ScriptableObjMgr.Inst.testCtrller.CommandLine)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			goto IL_0056;
		}
		if (debugCmdInputField.isFocused)
		{
			string text = debugCmdInputField.text.Trim();
			if (text == "`" || text == "·")
			{
				goto IL_0056;
			}
		}
		goto IL_00ac;
		IL_0056:
		go_CommandLine.SetActive(!go_CommandLine.activeSelf);
		if (!EventSystem.current.alreadySelecting)
		{
			EventSystem.current.SetSelectedGameObject(null);
			debugCmdInputField.ActivateInputField();
		}
		debugCmdInputField.text = string.Empty;
		debugCmdInputField.ForceLabelUpdate();
		goto IL_00ac;
		IL_00ac:
		if (isGodMode)
		{
			if (PlayerMgr.Inst.BaData == null)
			{
				return;
			}
			if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt) && playerPpt.unitCfg.currentHP < playerPpt.unitCfg.maxHP)
			{
				playerPpt.unitCfg.currentHP = playerPpt.unitCfg.maxHP;
				ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
				UIPlayerDataMgr.Inst.UpdateHP();
			}
			for (int i = 0; i < PlayerMgr.Inst.Wands.Count; i++)
			{
				if (PlayerMgr.Inst.Wands[i].WandCfg != null)
				{
					PlayerMgr.Inst.Wands[i].WandCfg.maxMP = 5000;
					PlayerMgr.Inst.Wands[i].CurrentMP = PlayerMgr.Inst.Wands[i].MaxMP;
				}
			}
		}
		if (!go_CommandLine.activeSelf)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (historyArrow > 0 && cmdHistory.Count > 0 && historyArrow < cmdHistory.Count)
			{
				debugCmdInputField.text = cmdHistory[historyArrow];
				debugCmdInputField.caretPosition = 99999;
				historyArrow--;
			}
			else
			{
				if (historyArrow == 0)
				{
					debugCmdInputField.text = cmdHistory[0];
					debugCmdInputField.caretPosition = 99999;
				}
				else
				{
					debugCmdInputField.text = "";
				}
				historyArrow = cmdHistory.Count - 1;
			}
			debugCmdInputField.Select();
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (historyArrow < cmdHistory.Count && cmdHistory.Count > 0 && historyArrow >= 0)
			{
				debugCmdInputField.text = cmdHistory[historyArrow];
				debugCmdInputField.caretPosition = 99999;
				historyArrow++;
			}
			else
			{
				historyArrow = 0;
				debugCmdInputField.text = "";
			}
			debugCmdInputField.Select();
		}
		RunAutoRunCommand();
	}

	public void Initialize()
	{
		Inst = this;
		cmdList.Clear();
		autoCommandIsRun = false;
		cmdList.Add("help", "列出所有指令");
		cmdList.Add("coin", "获取指定数量的金币，不填参数默认获得1");
		cmdList.Add("key", "获取指定数量的钥匙，不填参数默认获得1");
		cmdList.Add("crystal", "获取指定数量的水晶，不填参数默认获得1");
		cmdList.Add("blood", "获取指定数量的旧神之血，不填参数默认获得1");
		cmdList.Add("core", "获取指定数量的混沌核心，不填参数默认获得1");
		cmdList.Add("allres", "获取指定数量的所有资源，不填参数默认获得1");
		cmdList.Add("gear", "获取指定数量的齿轮，不填参数默认获得1");
		cmdList.Add("wand", "获取指定id的法杖，不填id从系统所有法杖中随机获得1把");
		cmdList.Add("allwand", "获取所有法杖");
		cmdList.Add("wandstage", "获取指定阶段的所有法杖，不填数字默认1阶段法杖");
		cmdList.Add("spell", "获取指定id的法术，无参数则获得随机法术");
		cmdList.Add("allspell", "获取所有指定等级的法术，无参数默认刷出1级");
		cmdList.Add("relic", "获取指定id的遗物");
		cmdList.Add("removerelic", "给指定id的遗物降级直到移除");
		cmdList.Add("allrelic", "获取所有遗物");
		cmdList.Add("potion", "获取指定id的药水");
		cmdList.Add("allpotion", "获取所有药水");
		cmdList.Add("curse", "获取指定id的诅咒");
		cmdList.Add("unit", "创建指定ID的单位");
		cmdList.Add("listwand", "列出所有法杖.");
		cmdList.Add("listspell", "列出所有法术");
		cmdList.Add("listpotion", "列出所有药剂");
		cmdList.Add("listrelic", "列出所有遗物");
		cmdList.Add("listcurse", "列出所有诅咒");
		cmdList.Add("listunit", "列出所有单位");
		cmdList.Add("removecurse", "清除所有诅咒");
		cmdList.Add("clear", "清除所有地面道具");
		cmdList.Add("god", "满状态，不会耗蓝不会掉血");
		cmdList.Add("fps", "设置游戏最大帧率");
		cmdList.Add("room", "加载指定房间");
		cmdList.Add("listroom", "列出所有房间名称与ID");
		cmdList.Add("stage", "跳转到指定阶段房间");
		cmdList.Add("gallery", "解锁/恢复 所有图鉴，需重进营地");
		cmdList.Add("finishnothing", "未完成任何难度");
		cmdList.Add("finisheasy", "完成简单难度，可触发剧情");
		cmdList.Add("finishnormal", "完成普通难度，可触发剧情");
		cmdList.Add("finishnormal2", "完成普通难度2。完成回城剧情，未完成npc7出场");
		cmdList.Add("finishnormal3", "完成普通难度3。完成npc7出场，未完成npc7开放功能");
		cmdList.Add("finishnormal4", "完成普通难度4。彻底完成npc7相关功能");
		cmdList.Add("finishhard", "完成困难难度");
		cmdList.Add("finishnightmare1", "完成噩梦难度1");
		cmdList.Add("finishnightmare2", "完成噩梦难度2");
		cmdList.Add("finishnightmare3", "完成噩梦难度3");
		cmdList.Add("allnpc", "解锁/恢复 所有npc，需重进营地");
		cmdList.Add("npc3", "解救/恢复 npc3,但不完成营地对话，需重进营地");
		cmdList.Add("npc4", "解救/恢复 npc4,但不完成营地对话，需重进营地");
		cmdList.Add("npc5", "解救/恢复 npc5,但不完成营地对话，需重进营地");
		cmdList.Add("npc6", "解救/恢复 npc6,但不完成营地对话，需重进营地");
		cmdList.Add("clearrank", "清除当前帐号的排行记录");
		cmdList.Add("loop", "重复执行指令，举例：loop 10 unit 101721");
		cmdList.Add("findset", "解锁指定id的套装，不填参数默认解锁所有套装");
		cmdList.Add("autorun", "执行自动执行的指令");
		cmdList.Add("clearpool", "清空对象池");
		cmdList.Add("enterbattletime", "设置进入战场次数");
		cmdList.Add("easy", "设置当前难度为easy");
		cmdList.Add("normal", "设置当前难度为normal");
		cmdList.Add("hard", "设置当前难度为hard");
		cmdList.Add("nightmare1", "设置当前难度为nightmare1");
		cmdList.Add("nightmare2", "设置当前难度为nightmare2");
		cmdList.Add("nightmare3", "设置当前难度为nightmare3");
		cmdList.Add("blive", "断开或连接直播间（通过身份码）");
		cmdList.Add("danmaku", "模拟一条直播弹幕，空格后面写弹幕内容");
		cmdList.Add("like", "模拟有人给主播点赞");
		cmdList.Add("gift", "模拟有人给主播送礼物，后面跟礼物名字和用户名");
		cmdList.Add("gremovewand + id", "从图鉴里移除指定法杖，不填移除所有");
		cmdList.Add("gremovespell + id", "从图鉴里移除指定法术，不填移除所有");
		cmdList.Add("gremoverelic + id", "从图鉴里移除指定遗物，不填移除所有");
		cmdList.Add("gremovepotion + id", "从图鉴里移除指定药水，不填移除所有");
		cmdList.Add("gremovecurse + id", "从图鉴里移除指定诅咒，不填移除所有");
		cmdList.Add("showfps", "开关FPS显示");
		cmdList.Add("dtyh", "动态优化");
		cmdList.Add("resource", "所有资源掉地上");
		cmdList.Add("paytest", "支付测试:ProductName,ID,amount,orderNum,extensionInfo,notifyUrl");
		cmdList.Add("mobilecam", "镜头高度测试");
		cmdList.Add("hd", "测试对话");
		cmdList.Add("changeres", "移动端修改分辨率");
		cmdList.Add("hp", "修改血量");
		cmdList.Add("maxhp", "修改最大血量");
		cmdList.Add("shield", "修改护盾");
		cmdList.Add("tempshield", "修改临时护盾");
		cmdList.Add("mp", "修改MP");
		cmdList.Add("maxmp", "修改最大MP");
		cmdList.Add("crash", "unity崩溃测试");
		cmdList.Add("crashnative", "安卓或者苹果崩溃");
		cmdList.Add("enterdoordestroy", "摧毁掉所有进门需要摧毁的东西");
		cmdList.Add("aidata_server", "启动ai数据服务器，端口 1234");
		cmdList.Add("androidbuildid", "获取buildid");
		cmdList.Add("clearett", "清除所有ett的内存");
		cmdList.Add("closecam", "切换主摄像机层级");
		cmdList.Add("closeuicam", "切换ui摄像机层级");
		cmdList.Add("cputest", "cpu性能测试");
		cmdList.Add("lastcpucost", "最后一次cpu测试结果");
		cmdList.Add("savecfg", "保存法术配置为字符串");
		cmdList.Add("loadcfg", "根据字符串读取法术配置");
	}

	public void RunAutoRunCommand()
	{
		if (!autoCommandIsRun && ScriptableObjMgr.Inst.testCtrller.CommandLine && !autoCommandIsRun)
		{
			autoCommandIsRun = true;
			debugCmdInputField.text = ScriptableObjMgr.Inst.testCtrller.autoCommand;
			FinishCmd();
		}
	}

	public void SetDebugValue(string key, object val)
	{
		debugValues[key] = val;
	}

	public object GetDebugValue(string key)
	{
		if (debugValues.ContainsKey(key))
		{
			return debugValues[key];
		}
		return null;
	}

	public void PrintLog(string content)
	{
		string text = debugText.text + "\n<color=aqua>[" + SceneManager.GetActiveScene().name + "]";
		if (SceneManager.GetActiveScene().name == "Battle")
		{
			text = text + "[" + LevelMgr.Inst.CurrentRoomCfg.id + "]";
		}
		text = text + "[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "]</color>\n  " + content;
		if (text.Length > 15000)
		{
			text = text.Substring(text.Length - 15000);
		}
		debugText.text = text;
		Canvas.ForceUpdateCanvases();
		debugScrollUI.verticalNormalizedPosition = 0f;
	}

	public unsafe void RunDebugCmd(string cmdString)
	{
		if (!EventSystem.current.alreadySelecting)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		debugCmdInputField.Select();
		if (cmdString == "" || cmdString == "`")
		{
			return;
		}
		string[] array = (from e in cmdString.Split(' ')
			select e.Trim() into e
			where e.Length > 0
			select e).ToArray();
		if (array.Length != 0)
		{
			switch (array[0])
			{
			case "help":
			{
				StringBuilder stringBuilder4 = new StringBuilder("命令列表:\n");
				foreach (KeyValuePair<string, string> cmd in cmdList)
				{
					stringBuilder4.Append("<color=yellow>  " + cmd.Key + "</color>  " + cmd.Value + "\n");
				}
				PrintLog(stringBuilder4.ToString());
				break;
			}
			case "wand":
				if (array.Length == 1)
				{
					int id6 = WandConfig.list[UnityEngine.Random.Range(0, WandConfig.list.Count)].id;
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Wand, id6), PlayerMgr.Inst.PlayerPointIgnoreZ);
					PrintLog("已随机获得魔杖 " + WandConfig.dic[id6].GetName() + "(" + id6 + ")");
				}
				else if (array.Length == 2)
				{
					int num55 = int.Parse(array[1]);
					if (WandConfig.dic.ContainsKey(num55))
					{
						QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Wand, num55), PlayerMgr.Inst.PlayerPointIgnoreZ);
						PrintLog("已获得魔杖" + WandConfig.dic[num55].GetName() + "(" + num55 + ")");
					}
					else
					{
						PrintLog("没有指定ID的魔杖");
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "allwand":
			{
				Dictionary<int, List<ItemInfo>> dictionary2 = new Dictionary<int, List<ItemInfo>>();
				for (int num35 = 0; num35 < WandConfig.list.Count; num35++)
				{
					if (1 <= WandConfig.list[num35].dropStage && WandConfig.list[num35].dropStage <= 20)
					{
						if (!dictionary2.ContainsKey(WandConfig.list[num35].dropStage))
						{
							dictionary2.Add(WandConfig.list[num35].dropStage, new List<ItemInfo>
							{
								new ItemInfo(ItemType.Wand, WandConfig.list[num35].id)
							});
						}
						else
						{
							dictionary2[WandConfig.list[num35].dropStage].Add(new ItemInfo(ItemType.Wand, WandConfig.list[num35].id));
						}
					}
				}
				int num36 = 0;
				float num37 = 2f;
				foreach (KeyValuePair<int, List<ItemInfo>> item in dictionary2)
				{
					Vector3[] circleDancePoints6 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3((float)(-(dictionary2.Count - 1)) / 2f * num37 + (float)num36 * num37, 0f, 0f), item.Value.Count, 0.25f);
					for (int num38 = 0; num38 < circleDancePoints6.Length; num38++)
					{
						QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, item.Value[num38], circleDancePoints6[num38]);
					}
					num36++;
				}
				break;
			}
			case "wandstage":
				if (array.Length == 1)
				{
					List<ItemInfo> list3 = new List<ItemInfo>();
					for (int num13 = 0; num13 < WandConfig.list.Count; num13++)
					{
						if (WandConfig.list[num13].dropStage == 1)
						{
							list3.Add(new ItemInfo(ItemType.Wand, WandConfig.list[num13].id));
						}
					}
					Vector3[] circleDancePoints3 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.firstRoomPoint, list3.Count, 0.25f);
					for (int num14 = 0; num14 < circleDancePoints3.Length; num14++)
					{
						QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list3[num14], circleDancePoints3[num14]);
					}
					PrintLog("已获得1阶段法杖" + list3.Count + "个");
				}
				else if (array.Length == 2)
				{
					int num15 = int.Parse(array[1]);
					List<ItemInfo> list4 = new List<ItemInfo>();
					for (int num16 = 0; num16 < WandConfig.list.Count; num16++)
					{
						if (WandConfig.list[num16].dropStage == num15)
						{
							list4.Add(new ItemInfo(ItemType.Wand, WandConfig.list[num16].id));
						}
					}
					Vector3[] circleDancePoints4 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.firstRoomPoint, list4.Count, 0.25f);
					for (int num17 = 0; num17 < circleDancePoints4.Length; num17++)
					{
						QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list4[num17], circleDancePoints4[num17]);
					}
					PrintLog("已获得" + num15 + "阶段法杖" + list4.Count + "个");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "room":
				if (array.Length == 2)
				{
					int num24 = int.Parse(array[1]);
					if (RoomConfig.dic.ContainsKey(num24) && num24 < 110000)
					{
						GameMgr.Inst.RecycleAllPool();
						Dictionary<Vector2Int, RoomConfig> dictionary = new Dictionary<Vector2Int, RoomConfig>();
						RoomConfig config = RoomConfig.GetConfig(num24);
						config.isFinalRoom = true;
						if (UnityEngine.Random.Range(0, 2) == 0 && config.type != RoomType.Boss)
						{
							config.ReverseX();
						}
						config.generateRO = UnityEngine.Random.Range(0, 2) == 0;
						dictionary.Add(Vector2Int.zero, config);
						LevelMgr.Inst.CreateLevel(dictionary, LevelMgr.Inst.CurrentRewardType, LevelMgr.Inst.NextRewardTypes, PlayerMgr.Inst.ItemCtrller.relic_ExtraDoor ? LevelRewardType.Spell : LevelRewardType.None, fadeDisappear: true);
						PrintLog("跳转到指定房间:" + config.name + "\tID" + config.id);
					}
					else
					{
						PrintLog("房间ID不存在");
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "stage":
				if (array.Length == 2)
				{
					GameMgr.Inst.ClearAllPool();
					DataMgr.selectedWorldData.inBattle9 = true;
					DataMgr.selectedWorldData.battleData9.currentStage = int.Parse(array[1]);
					DataMgr.selectedWorldData.battleData9.currentLevel = 1;
					DataMgr.selectedWorldData.battleData9.currentRoomID = 111;
					DataMgr.SaveSelectedWorldData();
					SceneManager.LoadScene("Battle");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "spell":
				if (array.Length == 1)
				{
					List<int> list9 = new List<int>();
					for (int num39 = 0; num39 < SpellConfig.list.Count; num39++)
					{
						if (SpellConfig.list[num39].dropType != 0)
						{
							list9.Add(SpellConfig.list[num39].id);
						}
					}
					int num40 = list9[UnityEngine.Random.Range(0, list9.Count)];
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Spell, num40), PlayerMgr.Inst.PlayerPointIgnoreZ);
					PrintLog("已随机获得法术 " + SpellConfig.dic[num40].GetName() + "(" + num40 + ")");
				}
				else if (array.Length == 2)
				{
					int num41 = int.Parse(array[1]);
					if (SpellConfig.dic.ContainsKey(num41))
					{
						QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Spell, num41), PlayerMgr.Inst.PlayerPointIgnoreZ);
						PrintLog("已获得法术 " + SpellConfig.dic[num41].GetName() + "(" + num41 + ")");
					}
					else
					{
						PrintLog("没有指定ID的法术");
					}
				}
				else if (array.Length == 3)
				{
					int num42 = int.Parse(array[1]);
					if (SpellConfig.dic.ContainsKey(num42))
					{
						int num43 = int.Parse(array[2]);
						for (int num44 = 0; num44 < num43; num44++)
						{
							QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Spell, num42), PlayerMgr.Inst.PlayerPointIgnoreZ);
						}
						PrintLog("已获得法术 " + SpellConfig.dic[num42].GetName() + "(" + num42 + ")");
					}
					else
					{
						PrintLog("没有指定ID的法术");
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "allspell":
			{
				int num25 = 1;
				if (array.Length > 1)
				{
					num25 = int.Parse(array[1]);
				}
				int num26 = 0;
				List<ItemInfo> list5 = new List<ItemInfo>();
				List<ItemInfo> list6 = new List<ItemInfo>();
				List<ItemInfo> list7 = new List<ItemInfo>();
				List<ItemInfo> list8 = new List<ItemInfo>();
				for (int num27 = 0; num27 < SpellConfig.list.Count; num27++)
				{
					if (SpellConfig.list[num27].level == num25 && SpellConfig.list[num27].dropType != 0)
					{
						switch (SpellConfig.list[num27].useType)
						{
						case SpellType.Missile:
							list5.Add(new ItemInfo(ItemType.Spell, SpellConfig.list[num27].id));
							num26++;
							break;
						case SpellType.Summon:
							list6.Add(new ItemInfo(ItemType.Spell, SpellConfig.list[num27].id));
							num26++;
							break;
						case SpellType.Enhance:
							list7.Add(new ItemInfo(ItemType.Spell, SpellConfig.list[num27].id));
							num26++;
							break;
						case SpellType.Passive:
							list8.Add(new ItemInfo(ItemType.Spell, SpellConfig.list[num27].id));
							num26++;
							break;
						}
					}
				}
				Vector3[] circleDancePoints5 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(-6f, 0f, 0f), list5.Count, 0.25f);
				for (int num28 = 0; num28 < circleDancePoints5.Length; num28++)
				{
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list5[num28], circleDancePoints5[num28]);
				}
				circleDancePoints5 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(-2f, 0f, 0f), list6.Count, 0.25f);
				for (int num29 = 0; num29 < circleDancePoints5.Length; num29++)
				{
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list6[num29], circleDancePoints5[num29]);
				}
				circleDancePoints5 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(2f, 0f, 0f), list7.Count, 0.25f);
				for (int num30 = 0; num30 < circleDancePoints5.Length; num30++)
				{
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list7[num30], circleDancePoints5[num30]);
				}
				circleDancePoints5 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(6f, 0f, 0f), list8.Count, 0.25f);
				for (int num31 = 0; num31 < circleDancePoints5.Length; num31++)
				{
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list8[num31], circleDancePoints5[num31]);
				}
				Debug.Log("已掉落所有" + num25 + "级法术，共" + num26 + "个");
				break;
			}
			case "relic":
				if (array.Length == 1)
				{
					int id5 = RelicConfig.list[UnityEngine.Random.Range(0, RelicConfig.list.Count)].id;
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Relic, id5), PlayerMgr.Inst.PlayerPointIgnoreZ);
					PrintLog("已随机获得遗物 " + RelicConfig.dic[id5].GetName() + "(" + id5 + ")");
				}
				else if (array.Length == 2)
				{
					int num52 = int.Parse(array[1]);
					if (RelicConfig.dic.ContainsKey(num52))
					{
						QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Relic, num52), PlayerMgr.Inst.PlayerPointIgnoreZ);
						PrintLog("已获得遗物 " + RelicConfig.dic[num52].GetName() + "(" + num52 + ")");
					}
					else
					{
						PrintLog("没有指定ID的遗物");
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "removerelic":
				PlayerMgr.Inst.ItemCtrller.RelicRemove(int.Parse(array[1]), 1);
				PrintLog("降级遗物 " + array[1]);
				break;
			case "allrelic":
				if (array.Length == 1)
				{
					List<ItemInfo> list10 = new List<ItemInfo>();
					for (int num46 = 0; num46 < RelicConfig.list.Count; num46++)
					{
						list10.Add(new ItemInfo(ItemType.Relic, RelicConfig.list[num46].id));
					}
					int num47 = 10;
					int num48 = Mathf.CeilToInt((float)list10.Count / (float)num47);
					for (int num49 = 0; num49 < list10.Count; num49++)
					{
						float num50 = Mathf.FloorToInt((float)num49 / (float)num47);
						float num51 = num49 % num47;
						Vector3 vector = new Vector3(((float)(-num48) / 2f + num50) * 1.5f, (num51 - (float)num47 / 2f) * 0.4f);
						QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list10[num49], LevelMgr.Inst.firstRoomPoint + vector);
					}
					PrintLog("已获得所有遗物");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "potion":
				if (array.Length == 1)
				{
					int id4 = PotionConfig.list[UnityEngine.Random.Range(0, PotionConfig.list.Count)].id;
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Potion, id4), PlayerMgr.Inst.PlayerPointIgnoreZ);
					PrintLog("已随机获得药水 " + PotionConfig.dic[id4].GetName() + "(" + id4 + ")");
				}
				else if (array.Length == 2)
				{
					int num33 = int.Parse(array[1]);
					if (PotionConfig.dic.ContainsKey(num33))
					{
						QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Potion, num33), PlayerMgr.Inst.PlayerPointIgnoreZ);
						PrintLog("已获得药水 " + PotionConfig.dic[num33].GetName() + "(" + num33 + ")");
					}
					else
					{
						PrintLog("没有指定ID的药水");
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "allpotion":
				if (array.Length == 1)
				{
					List<ItemInfo> list = new List<ItemInfo>();
					for (int k = 0; k < PotionConfig.list.Count; k++)
					{
						list.Add(new ItemInfo(ItemType.Potion, PotionConfig.list[k].id));
					}
					Vector3[] circleDancePoints = Tool2D.GetCircleDancePoints(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint, list.Count, 0.25f);
					for (int l = 0; l < circleDancePoints.Length; l++)
					{
						QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list[l], circleDancePoints[l]);
					}
					PrintLog("已获得所有药水");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "curse":
				if (array.Length == 1)
				{
					int id3;
					for (id3 = CurseConfig.list[UnityEngine.Random.Range(0, CurseConfig.list.Count)].id; id3 == 29; id3 = CurseConfig.list[UnityEngine.Random.Range(0, CurseConfig.list.Count)].id)
					{
					}
					PlayerMgr.Inst.ItemCtrller.CurseAdd(id3, textFloat: true);
					PrintLog("已随机获得诅咒 " + CurseConfig.dic[id3].GetName() + "(" + id3 + ")");
				}
				else if (array.Length == 2)
				{
					int num23 = int.Parse(array[1]);
					if (CurseConfig.dic.ContainsKey(num23))
					{
						PlayerMgr.Inst.ItemCtrller.CurseAdd(num23, textFloat: true);
						PrintLog("已获得诅咒 " + CurseConfig.dic[num23].GetName() + "(" + num23 + ")");
					}
					else
					{
						PrintLog("没有指定ID的诅咒");
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "key":
				if (array.Length == 1)
				{
					PlayerMgr.Inst.ChangeKey(1);
					PrintLog("已获得" + 1 + "钥匙");
				}
				else if (array.Length == 2)
				{
					int value4 = int.Parse(array[1]);
					PlayerMgr.Inst.ChangeKey(value4);
					PrintLog("已获得" + value4 + "钥匙");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "coin":
				if (array.Length == 1)
				{
					PlayerMgr.Inst.ChangeCoin(1);
					PrintLog("已获得" + 1 + "金币");
				}
				else if (array.Length == 2)
				{
					int value = int.Parse(array[1]);
					PlayerMgr.Inst.ChangeCoin(value);
					PrintLog("已获得" + value + "金币");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "crystal":
				if (array.Length == 1)
				{
					PlayerMgr.Inst.ChangeMagicCrystal(1);
					PrintLog("已获得" + 1 + "水晶");
				}
				else if (array.Length == 2)
				{
					int value6 = int.Parse(array[1]);
					PlayerMgr.Inst.ChangeMagicCrystal(value6);
					PrintLog("已获得" + value6 + "水晶");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "blood":
				if (array.Length == 1)
				{
					PlayerMgr.Inst.ChangeAncientBlood(1);
					PrintLog("已获得" + 1 + "旧神之血");
				}
				else if (array.Length == 2)
				{
					int value5 = int.Parse(array[1]);
					PlayerMgr.Inst.ChangeAncientBlood(value5);
					PrintLog("已获得" + value5 + "旧神之血");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "core":
				if (array.Length == 1)
				{
					PlayerMgr.Inst.ChangeChaosCore(1);
					PrintLog("已获得" + 1 + "混沌核心");
				}
				else if (array.Length == 2)
				{
					int value3 = int.Parse(array[1]);
					PlayerMgr.Inst.ChangeChaosCore(value3);
					PrintLog("已获得" + value3 + "混沌核心");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "allres":
				if (array.Length == 1)
				{
					PlayerMgr.Inst.ChangeCoin(1);
					PlayerMgr.Inst.ChangeKey(1);
					PlayerMgr.Inst.ChangeMagicCrystal(1);
					PlayerMgr.Inst.ChangeAncientBlood(1);
					PlayerMgr.Inst.ChangeChaosCore(1);
					PrintLog("已获得" + 1 + "所有资源");
				}
				else if (array.Length == 2)
				{
					int value8 = int.Parse(array[1]);
					PlayerMgr.Inst.ChangeCoin(value8);
					PlayerMgr.Inst.ChangeKey(value8);
					PlayerMgr.Inst.ChangeMagicCrystal(value8);
					PlayerMgr.Inst.ChangeAncientBlood(value8);
					PlayerMgr.Inst.ChangeChaosCore(value8);
					PrintLog("已获得" + value8 + "所有资源");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "gear":
				if (array.Length == 1)
				{
					PlayerMgr.Inst.ChangeGear(1);
					PrintLog("已获得" + 1 + "齿轮");
				}
				else if (array.Length == 2)
				{
					int value7 = int.Parse(array[1]);
					PlayerMgr.Inst.ChangeGear(value7);
					PrintLog("已获得" + value7 + "齿轮");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "unit":
				if (array.Length == 2)
				{
					int key = int.Parse(array[1]);
					if (UnitConfig.map[key].inGallery)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + key, LevelMgr.Inst.CurrentRoomCtrller.CenterPoint);
						PrintLog("已创建单位：" + UnitConfig.map[key].GetName() + "(" + key + ")");
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "listwand":
			{
				StringBuilder stringBuilder5 = new StringBuilder();
				for (int num21 = 0; num21 < WandConfig.list.Count; num21++)
				{
					stringBuilder5.Append("[" + WandConfig.list[num21].dropStage + "]" + WandConfig.list[num21].GetName() + "(" + WandConfig.list[num21].id + ")\n");
				}
				PrintLog(stringBuilder5.ToString());
				break;
			}
			case "listspell":
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				for (int num12 = 0; num12 < SpellConfig.list.Count; num12++)
				{
					stringBuilder3.Append("\n" + SpellConfig.list[num12].GetName() + "(" + SpellConfig.list[num12].id + ")");
				}
				PrintLog(stringBuilder3.ToString());
				break;
			}
			case "listroom":
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				for (int num7 = 0; num7 < RoomConfig.list.Count; num7++)
				{
					stringBuilder2.Append("\n" + RoomConfig.list[num7].name + "阶段" + RoomConfig.list[num7].belongStage + "(房间ID" + RoomConfig.list[num7].id + ")");
				}
				PrintLog(stringBuilder2.ToString());
				break;
			}
			case "listrelic":
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int m = 0; m < RelicConfig.list.Count; m++)
				{
					stringBuilder.Append(RelicConfig.list[m].GetName() + "(" + RelicConfig.list[m].id + ")\n<color=" + color_Info + ">" + RelicConfig.list[m].GetInfo(includeExtraInfo: false, upgrade: false) + "</color>\n");
				}
				PrintLog(stringBuilder.ToString());
				break;
			}
			case "listpotion":
			{
				StringBuilder stringBuilder7 = new StringBuilder();
				for (int num54 = 0; num54 < PotionConfig.list.Count; num54++)
				{
					stringBuilder7.Append(PotionConfig.list[num54].GetName() + "(" + PotionConfig.list[num54].id + ")\n<color=" + color_Info + ">" + PotionConfig.list[num54].GetInfo() + "</color>\n");
				}
				PrintLog(stringBuilder7.ToString());
				break;
			}
			case "listcurse":
			{
				StringBuilder stringBuilder8 = new StringBuilder();
				for (int num56 = 0; num56 < CurseConfig.list.Count; num56++)
				{
					stringBuilder8.Append(CurseConfig.list[num56].GetName() + "(" + CurseConfig.list[num56].id + ")\n<color=" + color_Info + ">" + CurseConfig.list[num56].GetInfo() + "</color>\n");
				}
				PrintLog(stringBuilder8.ToString());
				break;
			}
			case "listunit":
			{
				StringBuilder stringBuilder6 = new StringBuilder();
				foreach (UnitConfig item2 in UnitConfig.list)
				{
					if (item2.inGallery && (array.Length != 2 || item2.GetName().Contains(array[1])))
					{
						stringBuilder6.Append("\n" + item2.GetName() + "(" + item2.id + ")");
					}
				}
				PrintLog(stringBuilder6.ToString());
				break;
			}
			case "removecurse":
			{
				for (int num34 = PlayerMgr.Inst.BaData.curseIDs.Count - 1; num34 >= 0; num34--)
				{
					PlayerMgr.Inst.ItemCtrller.CurseRemoveByIndex(num34);
				}
				PrintLog("已移除所有诅咒。数量+" + PlayerMgr.Inst.BaData.curseIDs.Count);
				break;
			}
			case "clear":
				ObjPoolMgr.Inst.RecycleAll();
				PrintLog("已回收所有物品");
				break;
			case "fullhp":
				PlayerMgr.Inst.PlayerPpt.HPRecovery(PlayerMgr.Inst.PlayerPpt.unitCfg.maxHP);
				PrintLog("已回满生命");
				break;
			case "god":
			{
				isGodMode = !isGodMode;
				if (isGodMode)
				{
					PlayerMgr.Inst?.PlayerPpt?.InvincibleRegister();
					PrintLog("神仙模式已部署");
					break;
				}
				PlayerMgr.Inst?.PlayerPpt?.InvincibleUnregister();
				PrintLog("神仙模式已关闭");
				for (int num20 = 0; num20 < PlayerMgr.Inst.BaData.wandCfgs.Count; num20++)
				{
					if (PlayerMgr.Inst.Wands[num20].WandCfg != null)
					{
						PlayerMgr.Inst.BaData.wandCfgs[num20].maxMP = WandConfig.dic[PlayerMgr.Inst.BaData.wandCfgs[num20].id].maxMP;
					}
				}
				break;
			}
			case "fps":
				if (array.Length == 1)
				{
					Application.targetFrameRate = 0;
					PrintLog("已取消帧率限制");
				}
				else
				{
					PrintLog("已限制最大帧率：" + (Application.targetFrameRate = int.Parse(array[1])));
				}
				break;
			case "gallery":
				ScriptableObjMgr.Inst.testCtrller.UnlockAllGallery = !ScriptableObjMgr.Inst.testCtrller.UnlockAllGallery;
				if (ScriptableObjMgr.Inst.testCtrller.UnlockAllGallery)
				{
					PrintLog("已解锁所有图鉴，需重进营地");
				}
				else
				{
					PrintLog("已恢复所有图鉴，需重进营地");
				}
				break;
			case "finishnothing":
				DataMgr.selectedWorldData.finishedDifficulty.Clear();
				DataMgr.selectedWorldData.storyKillChapter3BossPickup = false;
				DataMgr.selectedWorldData.storyNormalFinishBackCamp = false;
				DataMgr.selectedWorldData.storyHardBossDropPickup = false;
				DataMgr.selectedWorldData.storyHardFinishBackCamp = false;
				DataMgr.selectedWorldData.storyHardFinishNPC7Appearance = false;
				DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction = false;
				DataMgr.selectedWorldData.storyFinishHardDropPickup = false;
				DataMgr.selectedWorldData.storyFinishHardBackCamp = false;
				DataMgr.selectedWorldData.isReachChatper2 = false;
				DataMgr.selectedWorldData.isReachChatper3 = false;
				DataMgr.selectedWorldData.isReachChatper4 = false;
				DataMgr.selectedWorldData.isReachChatper5 = false;
				DataMgr.selectedWorldData.canSetUpgrade = false;
				PrintLog("目前已重置为：未完成任何难度");
				break;
			case "finisheasy":
			case "finishnormal":
			case "finishnormal2":
			case "finishnormal3":
			case "finishnormal4":
			case "finishhard":
			case "finishnightmare1":
			case "finishnightmare2":
			case "finishnightmare3":
				DataMgr.selectedWorldData.finishedDifficulty.Clear();
				DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Easy);
				DataMgr.selectedWorldData.storyKillChapter3BossPickup = false;
				DataMgr.selectedWorldData.storyNormalFinishBackCamp = false;
				DataMgr.selectedWorldData.storyHardBossDropPickup = false;
				DataMgr.selectedWorldData.storyHardFinishBackCamp = false;
				DataMgr.selectedWorldData.storyHardFinishNPC7Appearance = false;
				DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction = false;
				DataMgr.selectedWorldData.storyFinishHardDropPickup = false;
				DataMgr.selectedWorldData.storyFinishHardBackCamp = false;
				DataMgr.selectedWorldData.storyFinishNightmare1BackCamp = false;
				DataMgr.selectedWorldData.storyFinishNightmare2BackCamp = false;
				DataMgr.selectedWorldData.storyFinishNightmare3BackCamp = false;
				DataMgr.selectedWorldData.isReachChatper2 = false;
				DataMgr.selectedWorldData.isReachChatper3 = false;
				DataMgr.selectedWorldData.isReachChatper4 = false;
				DataMgr.selectedWorldData.isReachChatper5 = false;
				DataMgr.selectedWorldData.canSetUpgrade = false;
				if (array[0] == "finisheasy")
				{
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = true;
					DataMgr.selectedWorldData.isReachChatper2 = true;
					DataMgr.selectedWorldData.isReachChatper3 = true;
					DataMgr.selectedWorldData.BackCampCheckPlot();
					PrintLog("目前已重置为：完成了简单");
				}
				else if (array[0] == "finishnormal")
				{
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Normal);
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = true;
					DataMgr.selectedWorldData.storyNormalFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardBossDropPickup = true;
					DataMgr.selectedWorldData.isReachChatper2 = true;
					DataMgr.selectedWorldData.isReachChatper3 = true;
					DataMgr.selectedWorldData.isReachChatper4 = true;
					DataMgr.selectedWorldData.canSetUpgrade = true;
					DataMgr.selectedWorldData.BackCampCheckPlot();
					PrintLog("目前已重置为：完成了简单，普通");
				}
				else if (array[0] == "finishnormal2")
				{
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Normal);
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = true;
					DataMgr.selectedWorldData.storyNormalFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardBossDropPickup = true;
					DataMgr.selectedWorldData.storyHardFinishBackCamp = true;
					DataMgr.selectedWorldData.isReachChatper2 = true;
					DataMgr.selectedWorldData.isReachChatper3 = true;
					DataMgr.selectedWorldData.isReachChatper4 = true;
					DataMgr.selectedWorldData.canSetUpgrade = true;
					PrintLog("目前已重置为：完成了简单，普通2");
				}
				else if (array[0] == "finishnormal3")
				{
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Normal);
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = true;
					DataMgr.selectedWorldData.storyNormalFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardBossDropPickup = true;
					DataMgr.selectedWorldData.storyHardFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardFinishNPC7Appearance = true;
					DataMgr.selectedWorldData.isReachChatper2 = true;
					DataMgr.selectedWorldData.isReachChatper3 = true;
					DataMgr.selectedWorldData.isReachChatper4 = true;
					DataMgr.selectedWorldData.canSetUpgrade = true;
					PrintLog("目前已重置为：完成了简单，普通3");
				}
				else if (array[0] == "finishnormal4")
				{
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Normal);
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = true;
					DataMgr.selectedWorldData.storyNormalFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardBossDropPickup = true;
					DataMgr.selectedWorldData.storyHardFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardFinishNPC7Appearance = true;
					DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction = true;
					DataMgr.selectedWorldData.isReachChatper2 = true;
					DataMgr.selectedWorldData.isReachChatper3 = true;
					DataMgr.selectedWorldData.isReachChatper4 = true;
					DataMgr.selectedWorldData.canSetUpgrade = true;
					PrintLog("目前已重置为：完成了简单，普通4");
				}
				else if (array[0] == "finishhard")
				{
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Normal);
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Hard);
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = true;
					DataMgr.selectedWorldData.storyNormalFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardBossDropPickup = true;
					DataMgr.selectedWorldData.storyHardFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardFinishNPC7Appearance = true;
					DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction = true;
					DataMgr.selectedWorldData.storyFinishHardDropPickup = true;
					DataMgr.selectedWorldData.isReachChatper2 = true;
					DataMgr.selectedWorldData.isReachChatper3 = true;
					DataMgr.selectedWorldData.isReachChatper4 = true;
					DataMgr.selectedWorldData.isReachChatper5 = true;
					DataMgr.selectedWorldData.canSetUpgrade = true;
					DataMgr.selectedWorldData.BackCampCheckPlot();
					PrintLog("目前已重置为：完成了简单，普通，困难");
				}
				else if (array[0] == "finishnightmare1")
				{
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Normal);
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Hard);
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Nightmare1);
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = true;
					DataMgr.selectedWorldData.storyNormalFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardBossDropPickup = true;
					DataMgr.selectedWorldData.storyHardFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardFinishNPC7Appearance = true;
					DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction = true;
					DataMgr.selectedWorldData.storyFinishHardDropPickup = true;
					DataMgr.selectedWorldData.storyFinishHardBackCamp = true;
					DataMgr.selectedWorldData.storyFinishNightmare1 = true;
					DataMgr.selectedWorldData.isReachChatper2 = true;
					DataMgr.selectedWorldData.isReachChatper3 = true;
					DataMgr.selectedWorldData.isReachChatper4 = true;
					DataMgr.selectedWorldData.isReachChatper5 = true;
					DataMgr.selectedWorldData.canSetUpgrade = true;
					DataMgr.selectedWorldData.BackCampCheckPlot();
					PrintLog("目前已重置为：完成了简单，普通，困难，噩梦1");
				}
				else if (array[0] == "finishnightmare2")
				{
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Normal);
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Hard);
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Nightmare1);
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Nightmare2);
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = true;
					DataMgr.selectedWorldData.storyNormalFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardBossDropPickup = true;
					DataMgr.selectedWorldData.storyHardFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardFinishNPC7Appearance = true;
					DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction = true;
					DataMgr.selectedWorldData.storyFinishHardDropPickup = true;
					DataMgr.selectedWorldData.storyFinishHardBackCamp = true;
					DataMgr.selectedWorldData.storyFinishNightmare1 = true;
					DataMgr.selectedWorldData.storyFinishNightmare1BackCamp = true;
					DataMgr.selectedWorldData.storyFinishNightmare2 = true;
					DataMgr.selectedWorldData.isReachChatper2 = true;
					DataMgr.selectedWorldData.isReachChatper3 = true;
					DataMgr.selectedWorldData.isReachChatper4 = true;
					DataMgr.selectedWorldData.isReachChatper5 = true;
					DataMgr.selectedWorldData.canSetUpgrade = true;
					DataMgr.selectedWorldData.BackCampCheckPlot();
					PrintLog("目前已重置为：完成了简单，普通，困难，噩梦1，噩梦2");
				}
				else if (array[0] == "finishnightmare3")
				{
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Normal);
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Hard);
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Nightmare1);
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Nightmare2);
					DataMgr.selectedWorldData.finishedDifficulty.Add(DifficultyType.Nightmare3);
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = true;
					DataMgr.selectedWorldData.storyNormalFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardBossDropPickup = true;
					DataMgr.selectedWorldData.storyHardFinishBackCamp = true;
					DataMgr.selectedWorldData.storyHardFinishNPC7Appearance = true;
					DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction = true;
					DataMgr.selectedWorldData.storyFinishHardDropPickup = true;
					DataMgr.selectedWorldData.storyFinishHardBackCamp = true;
					DataMgr.selectedWorldData.storyFinishNightmare1 = true;
					DataMgr.selectedWorldData.storyFinishNightmare1BackCamp = true;
					DataMgr.selectedWorldData.storyFinishNightmare2 = true;
					DataMgr.selectedWorldData.storyFinishNightmare2BackCamp = true;
					DataMgr.selectedWorldData.storyFinishNightmare3 = true;
					DataMgr.selectedWorldData.isReachChatper2 = true;
					DataMgr.selectedWorldData.isReachChatper3 = true;
					DataMgr.selectedWorldData.isReachChatper4 = true;
					DataMgr.selectedWorldData.isReachChatper5 = true;
					DataMgr.selectedWorldData.canSetUpgrade = true;
					DataMgr.selectedWorldData.BackCampCheckPlot();
					PrintLog("目前已重置为：完成了简单，普通，困难，噩梦1，噩梦2，噩梦3");
				}
				DataMgr.SaveSelectedWorldData();
				break;
			case "allnpc":
				ScriptableObjMgr.Inst.testCtrller.UnlockAllNPC = !ScriptableObjMgr.Inst.testCtrller.UnlockAllNPC;
				if (ScriptableObjMgr.Inst.testCtrller.UnlockAllNPC)
				{
					PrintLog("已解锁所有npc，需重进营地");
				}
				else
				{
					PrintLog("已恢复所有npc，需重进营地");
				}
				break;
			case "npc3":
				if (DataMgr.selectedWorldData.story2Open)
				{
					DataMgr.selectedWorldData.story2Open = false;
					DataMgr.selectedWorldData.story2Finish = false;
					PrintLog("未解锁npc3，需重进营地");
				}
				else
				{
					DataMgr.selectedWorldData.story2Open = true;
					PrintLog("已解锁npc3，需重进营地");
				}
				if (DataMgr.selectedWorldData.battleData9 == null)
				{
					DataMgr.selectedWorldData.battleData9 = new BattleData();
				}
				DataMgr.selectedWorldData.battleData9.currentStage = 2;
				DataMgr.selectedWorldData.BackCampCheckPlot();
				DataMgr.SaveSelectedWorldData();
				break;
			case "npc4":
				if (DataMgr.selectedWorldData.story3NPC4Rescued)
				{
					DataMgr.selectedWorldData.story3PlayerRoomEnter = false;
					DataMgr.selectedWorldData.story3NPC4Rescued = false;
					DataMgr.selectedWorldData.story3Finish = false;
					DataMgr.selectedWorldData.story3NPC4GiveCloth = false;
					PrintLog("未解锁npc4，需重进营地");
				}
				else
				{
					DataMgr.selectedWorldData.story3PlayerRoomEnter = true;
					DataMgr.selectedWorldData.story3NPC4Rescued = true;
					PrintLog("已解锁npc4，需重进营地");
				}
				if (DataMgr.selectedWorldData.battleData9 == null)
				{
					DataMgr.selectedWorldData.battleData9 = new BattleData();
				}
				DataMgr.selectedWorldData.battleData9.currentStage = 3;
				DataMgr.selectedWorldData.BackCampCheckPlot();
				DataMgr.SaveSelectedWorldData();
				break;
			case "npc5":
				if (DataMgr.selectedWorldData.story4NPC5Rescued)
				{
					DataMgr.selectedWorldData.story4PlayerRoomEnter = false;
					DataMgr.selectedWorldData.story4NPC5Rescued = false;
					DataMgr.selectedWorldData.story4Finish = false;
					PrintLog("未解锁npc5，需重进营地");
				}
				else
				{
					DataMgr.selectedWorldData.story4PlayerRoomEnter = true;
					DataMgr.selectedWorldData.story4NPC5Rescued = true;
					PrintLog("已解锁npc5，需重进营地");
				}
				if (DataMgr.selectedWorldData.battleData9 == null)
				{
					DataMgr.selectedWorldData.battleData9 = new BattleData();
				}
				DataMgr.selectedWorldData.battleData9.currentStage = 5;
				DataMgr.selectedWorldData.BackCampCheckPlot();
				DataMgr.SaveSelectedWorldData();
				break;
			case "npc6":
				if (DataMgr.selectedWorldData.storyKillChapter3BossPickup)
				{
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = false;
					DataMgr.selectedWorldData.storyNormalFinishBackCamp = false;
					PrintLog("未解锁npc6，需重进营地");
				}
				else
				{
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = true;
					DataMgr.selectedWorldData.storyNormalFinishBackCamp = false;
					PrintLog("已解锁npc6，需重进营地");
				}
				DataMgr.SaveSelectedWorldData();
				break;
			case "clearrank":
				if (SteamManager.Initialized)
				{
					SteamLeadBoardManager.Inst.GetLeadBoard(0);
					SteamLeadBoardManager.Inst.UploadScoreForceOverWride(SteamLeadBoardManager.Inst.int_currentleaderboard, 9999);
				}
				break;
			case "loop":
			{
				int num57 = int.Parse(array[1]);
				string cmdString2 = string.Join(" ", array.Where((string e, int i) => i >= 2));
				for (int num58 = 0; num58 < num57; num58++)
				{
					RunDebugCmd(cmdString2);
				}
				PrintLog("循环指令执行完毕");
				break;
			}
			case "findset":
				if (array.Length == 1)
				{
					if (!DataMgr.selectedWorldData.FindSet3)
					{
						DataMgr.selectedWorldData.SetFindSet3();
						DataMgr.selectedWorldData.SetFindSet4();
						DataMgr.selectedWorldData.SetFindSet5();
						DataMgr.selectedWorldData.SetFindSet6(9999);
						DataMgr.selectedWorldData.SetFindSet7();
						DataMgr.selectedWorldData.SetFindSet8();
						DataMgr.selectedWorldData.SetFindSet9();
						if (DataMgr.selectedWorldData.galleryPotionUseTimes.ContainsKey(1))
						{
							DataMgr.selectedWorldData.galleryPotionUseTimes[1] = 9999;
						}
						else
						{
							DataMgr.selectedWorldData.galleryPotionUseTimes.Add(1, 9999);
						}
						DataMgr.selectedWorldData.SetFindSet10();
						DataMgr.selectedWorldData.SetFindSet12();
						PrintLog("已找到套装3 4 5 6 7 8 9 10 12");
					}
					else
					{
						DataMgr.selectedWorldData.FindSet3 = false;
						DataMgr.selectedWorldData.FindSet4 = false;
						DataMgr.selectedWorldData.FindSet5 = false;
						DataMgr.selectedWorldData.FindSet6 = false;
						DataMgr.selectedWorldData.FindSet7 = false;
						DataMgr.selectedWorldData.FindSet8 = false;
						DataMgr.selectedWorldData.FindSet9 = false;
						DataMgr.selectedWorldData.FindSet10 = false;
						DataMgr.selectedWorldData.FindSet12 = false;
						if (DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(3))
						{
							DataMgr.selectedWorldData.setUnlockedSets.Remove(3);
						}
						if (DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(4))
						{
							DataMgr.selectedWorldData.setUnlockedSets.Remove(4);
						}
						if (DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(5))
						{
							DataMgr.selectedWorldData.setUnlockedSets.Remove(5);
						}
						if (DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(6))
						{
							DataMgr.selectedWorldData.setUnlockedSets.Remove(6);
						}
						if (DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(7))
						{
							DataMgr.selectedWorldData.setUnlockedSets.Remove(7);
						}
						if (DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(8))
						{
							DataMgr.selectedWorldData.setUnlockedSets.Remove(8);
						}
						if (DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(9))
						{
							DataMgr.selectedWorldData.setUnlockedSets.Remove(9);
						}
						if (DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(10))
						{
							if (DataMgr.selectedWorldData.galleryPotionUseTimes.ContainsKey(1))
							{
								DataMgr.selectedWorldData.galleryPotionUseTimes[1] = 1;
							}
							DataMgr.selectedWorldData.setUnlockedSets.Remove(10);
						}
						if (DataMgr.selectedWorldData.setUnlockedSets.ContainsKey(12))
						{
							DataMgr.selectedWorldData.setUnlockedSets.Remove(12);
						}
						PrintLog("未找到任何套装");
					}
					if ((bool)CampMgr.Inst)
					{
						GameUISingletonMono<UISet>.Inst.UpdateSet();
					}
				}
				else if (array.Length == 2)
				{
					int num11 = int.Parse(array[1]);
					if (3 <= num11 && num11 <= 9)
					{
						switch (num11)
						{
						case 3:
							DataMgr.selectedWorldData.SetFindSet3();
							break;
						case 4:
							DataMgr.selectedWorldData.SetFindSet4();
							break;
						case 5:
							DataMgr.selectedWorldData.SetFindSet5();
							break;
						case 6:
							DataMgr.selectedWorldData.SetFindSet6(9999);
							break;
						case 7:
							DataMgr.selectedWorldData.SetFindSet7();
							break;
						case 8:
							DataMgr.selectedWorldData.SetFindSet8();
							break;
						case 9:
							DataMgr.selectedWorldData.SetFindSet9();
							break;
						case 10:
							if (DataMgr.selectedWorldData.galleryPotionUseTimes.ContainsKey(1))
							{
								DataMgr.selectedWorldData.galleryPotionUseTimes[1] = 9999;
							}
							else
							{
								DataMgr.selectedWorldData.galleryPotionUseTimes.Add(1, 9999);
							}
							DataMgr.selectedWorldData.SetFindSet10();
							break;
						default:
							Debug.LogError("!");
							break;
						}
						PrintLog("已找到套装" + num11);
						if ((bool)CampMgr.Inst)
						{
							GameUISingletonMono<UISet>.Inst.UpdateSet();
						}
					}
					else
					{
						PrintLog("没有指定的ID套装" + num11);
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "autorun":
				autoCommandIsRun = false;
				RunAutoRunCommand();
				break;
			case "popWandSpell":
			{
				int index2 = int.Parse(array[1]);
				PrintLog(PlayerMgr.Inst.Wands[index2].PopWandSpellToBag(WandSlotType.Normal, int.Parse(array[2])).ToString());
				break;
			}
			case "wandLock":
			{
				int index = int.Parse(array[1]);
				int slotIndex = int.Parse(array[2]);
				bool lockState = int.Parse(array[3]) == 1;
				PlayerMgr.Inst.Wands[index].SetWandSlotLockState(WandSlotType.Normal, slotIndex, lockState);
				break;
			}
			case "addWand":
			{
				int num8 = int.Parse(array[1]);
				PlayerMgr.Inst.AddExtraWand((num8 == 0) ? null : WandConfig.GetConfig(num8), fullMp: true);
				break;
			}
			case "setWand":
			{
				int wandIndex2 = int.Parse(array[1]);
				int id = int.Parse(array[2]);
				PlayerMgr.Inst.SetWand(wandIndex2, WandConfig.GetConfig(id));
				break;
			}
			case "dropWand":
			{
				int wandIndex = int.Parse(array[1]);
				PlayerMgr.Inst.DropWand(wandIndex, spawnOnGround: true);
				break;
			}
			case "replaceWand":
			{
				int wandIndex3 = int.Parse(array[1]);
				int id2 = int.Parse(array[2]);
				PlayerMgr.Inst.ReplaceWand(wandIndex3, WandConfig.GetConfig(id2));
				break;
			}
			case "clearpool":
				ObjPoolMgr.Inst.ClearAllPool();
				break;
			case "enterbattletime":
				if (array.Length == 1)
				{
					DataMgr.selectedWorldData.enterBattleTime = 0;
					PrintLog("enterBattleTime现在为：" + DataMgr.selectedWorldData.enterBattleTime);
				}
				else if (array.Length == 2)
				{
					DataMgr.selectedWorldData.enterBattleTime = int.Parse(array[1]);
					PrintLog("enterBattleTime现在为：" + DataMgr.selectedWorldData.enterBattleTime);
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "easy":
				DataMgr.selectedWorldData.selectedDifficulty = DifficultyType.Easy;
				DataMgr.SaveSelectedWorldData();
				PrintLog("当前难度为easy");
				break;
			case "normal":
				DataMgr.selectedWorldData.selectedDifficulty = DifficultyType.Normal;
				DataMgr.SaveSelectedWorldData();
				PrintLog("当前难度为normal");
				break;
			case "hard":
				DataMgr.selectedWorldData.selectedDifficulty = DifficultyType.Hard;
				DataMgr.SaveSelectedWorldData();
				PrintLog("当前难度为hard");
				break;
			case "nightmare1":
				DataMgr.selectedWorldData.selectedDifficulty = DifficultyType.Nightmare1;
				DataMgr.SaveSelectedWorldData();
				PrintLog("当前难度为nightmare1");
				break;
			case "nightmare2":
				DataMgr.selectedWorldData.selectedDifficulty = DifficultyType.Nightmare2;
				DataMgr.SaveSelectedWorldData();
				PrintLog("当前难度为nightmare2");
				break;
			case "nightmare3":
				DataMgr.selectedWorldData.selectedDifficulty = DifficultyType.Nightmare3;
				DataMgr.SaveSelectedWorldData();
				PrintLog("当前难度为nightmare3");
				break;
			case "danmaku":
				BLiveMgr.Inst.OnDanmaku(new Dm
				{
					msg = array[1]
				});
				PrintLog("发送了弹幕 " + array[1]);
				break;
			case "like":
				BLiveMgr.Inst.OnLike(default(Like));
				PrintLog("给主播点赞了");
				break;
			case "gift":
			{
				SendGift sendGift = default(SendGift);
				sendGift.giftName = array[1];
				sendGift.userName = array[2];
				sendGift.giftNum = 1L;
				SendGift gift = sendGift;
				BLiveMgr.Inst.OnGift(gift);
				PrintLog($"{gift.userName} 给主播送了 {gift.giftName} x {gift.giftNum}");
				break;
			}
			case "gremovewand":
				if (array.Length == 1)
				{
					DataMgr.selectedWorldData.galleryUnlockedWands.Clear();
					PrintLog("已从图鉴中移除全部法杖");
				}
				else if (array.Length == 2)
				{
					int num53 = int.Parse(array[1]);
					if (DataMgr.selectedWorldData.galleryUnlockedWands.Contains(num53))
					{
						DataMgr.selectedWorldData.galleryUnlockedWands.Remove(num53);
						PrintLog("已从图鉴中移除法杖ID:" + num53 + " " + WandConfig.dic[num53].GetName());
					}
					else
					{
						PrintLog("图鉴中没有法杖ID:" + num53 + " " + WandConfig.dic[num53].GetName());
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "gremovespell":
				if (array.Length == 1)
				{
					DataMgr.selectedWorldData.galleryUnlockedSpells.Clear();
					PrintLog("已从图鉴中移除全部遗物");
				}
				else if (array.Length == 2)
				{
					int num45 = int.Parse(array[1]);
					if (DataMgr.selectedWorldData.galleryUnlockedSpells.Contains(num45))
					{
						DataMgr.selectedWorldData.galleryUnlockedSpells.Remove(num45);
						PrintLog("已从图鉴中移除法术ID:" + num45 + " " + SpellConfig.dic[num45].GetName());
					}
					else
					{
						PrintLog("图鉴中没有法术ID:" + num45 + " " + SpellConfig.dic[num45].GetName());
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "gremoverelic":
				if (array.Length == 1)
				{
					DataMgr.selectedWorldData.galleryUnlockedRelics.Clear();
					PrintLog("已从图鉴中移除全部遗物");
				}
				else if (array.Length == 2)
				{
					int num32 = int.Parse(array[1]);
					if (DataMgr.selectedWorldData.galleryUnlockedRelics.Contains(num32))
					{
						DataMgr.selectedWorldData.galleryUnlockedRelics.Remove(num32);
						PrintLog("已从图鉴中移除遗物ID:" + num32 + " " + RelicConfig.dic[num32].GetName());
					}
					else
					{
						PrintLog("图鉴中没有遗物ID:" + num32 + " " + RelicConfig.dic[num32].GetName());
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "gremovepotion":
				if (array.Length == 1)
				{
					DataMgr.selectedWorldData.galleryUnlockedPotions.Clear();
					PrintLog("已从图鉴中移除全部药水");
				}
				else if (array.Length == 2)
				{
					int num22 = int.Parse(array[1]);
					if (DataMgr.selectedWorldData.galleryUnlockedPotions.Contains(num22))
					{
						DataMgr.selectedWorldData.galleryUnlockedPotions.Remove(num22);
						PrintLog("已从图鉴中移除药水ID:" + num22 + " " + PotionConfig.dic[num22].GetName());
					}
					else
					{
						PrintLog("图鉴中没有药水ID:" + num22 + " " + PotionConfig.dic[num22].GetName());
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "gremovecurse":
				if (array.Length == 1)
				{
					DataMgr.selectedWorldData.galleryUnlockedCurses.Clear();
					PrintLog("已从图鉴中移除全部诅咒");
				}
				else if (array.Length == 2)
				{
					int num18 = int.Parse(array[1]);
					if (DataMgr.selectedWorldData.galleryUnlockedCurses.Contains(num18))
					{
						DataMgr.selectedWorldData.galleryUnlockedCurses.Remove(num18);
						PrintLog("已从图鉴中移除诅咒ID:" + num18 + " " + CurseConfig.dic[num18].GetName());
					}
					else
					{
						PrintLog("图鉴中没有诅咒ID:" + num18 + " " + CurseConfig.dic[num18].GetName());
					}
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "gc":
				GC.Collect();
				break;
			case "unloadresources":
				Resources.UnloadUnusedAssets().completed += delegate
				{
					PrintLog("Unload assets done");
				};
				break;
			case "random_dead_dialog":
				DataMgr.selectedWorldData.TryActiveDeadRandomDialog();
				break;
			case "random_reenter_dialog":
				DataMgr.selectedWorldData.TryActiveReEnterGameRandomDialog();
				break;
			case "showfps":
				ScriptableObjMgr.Inst.testCtrller.ShowFps = !ScriptableObjMgr.Inst.testCtrller.ShowFps;
				break;
			case "dtyh":
				ScriptableObjMgr.Inst.testCtrller.DisableLowFrameDynamicOptimize = !ScriptableObjMgr.Inst.testCtrller.DisableLowFrameDynamicOptimize;
				PrintLog(ScriptableObjMgr.Inst.testCtrller.DisableLowFrameDynamicOptimize ? "动态优化已关闭" : "动态优化已启用");
				break;
			case "resource":
				if (array.Length == 1)
				{
					List<ItemInfo> list2 = new List<ItemInfo>();
					for (int num9 = 0; num9 < ResourceConfig.list.Count; num9++)
					{
						list2.Add(new ItemInfo(ItemType.Resource, ResourceConfig.list[num9].id));
					}
					Vector3[] circleDancePoints2 = Tool2D.GetCircleDancePoints(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint, list2.Count, 0.25f);
					for (int num10 = 0; num10 < circleDancePoints2.Length; num10++)
					{
						QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, list2[num10], circleDancePoints2[num10]);
					}
					PrintLog("已获得所有药水");
				}
				else
				{
					PrintLog("指令错误");
				}
				break;
			case "paytest":
				MobileMgr.inst.PluginActivity.PayTest(array[1], (array[2] == "_") ? "" : array[2], int.Parse(array[3]), array[4], (array[5] == "_") ? "" : array[5], (array[6] == "_") ? "" : array[6]);
				break;
			case "mobilecam":
			{
				float num6 = float.Parse(array[1]);
				MobileMgr.inst.FocusCamLong = num6;
				for (int n = 0; n < MobileMgr.inst.FocusCamLongInBattle.Count; n++)
				{
					MobileMgr.inst.FocusCamLongInBattle[n] = num6;
				}
				PrintLog("应用成功");
				break;
			}
			case "hd":
			{
				int num5 = int.Parse(array[1]);
				if (num5 == 312)
				{
					DataMgr.selectedWorldData.selectedSetID = 1;
					GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(num5, (Action)delegate
					{
						DataMgr.selectedWorldData.selectedSetID = 11;
					});
				}
				else
				{
					GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(num5, (Action)delegate
					{
					});
				}
				break;
			}
			case "changeres":
			{
				Vector2Int mobileRes = SettingData.GetMobileRes(float.Parse(array[1]));
				Screen.SetResolution(mobileRes.x, mobileRes.y, FullScreenMode.ExclusiveFullScreen);
				PrintLog($"应用成功:{float.Parse(array[1])}->{mobileRes}");
				break;
			}
			case "maxhp":
			{
				int num4 = int.Parse(array[1]);
				PlayerMgr.Inst.ChangeHPMax(num4);
				break;
			}
			case "hp":
			{
				int num3 = int.Parse(array[1]);
				PlayerMgr.Inst.ChangeHPCurrent(num3);
				break;
			}
			case "shield":
			{
				int num2 = int.Parse(array[1]);
				PlayerMgr.Inst.ChangeShield(num2);
				break;
			}
			case "tempshield":
			{
				int num = int.Parse(array[1]);
				PlayerMgr.Inst.ChangeShieldTemp(num);
				break;
			}
			case "maxmp":
			{
				int value2 = int.Parse(array[1]);
				PlayerMgr.Inst.ChangeMPMax(value2);
				break;
			}
			case "crash":
			{
				int* ptr = null;
				*ptr = 0;
				break;
			}
			case "crashnative":
				PluginActivity.Inst.TestCrashNative();
				break;
			case "enterdoordestroy":
			{
				using (EntityQuery entityQuery5 = ettMgr.CreateEntityQuery(typeof(EnterDoorDestroy)))
				{
					NativeArray<Entity> nativeArray = entityQuery5.ToEntityArray(Allocator.Temp);
					foreach (Entity item3 in nativeArray)
					{
						ettMgr.DestroyEntity(item3);
					}
					nativeArray.Dispose();
				}
				break;
			}
			case "aidata_server":
				AIDataMgr.StartHttpServer(1234);
				PrintLog("已启动ai服务器，访问 http://localhost:1234/ 查看");
				break;
			case "androidbuildid":
				PrintLog("\"" + PluginActivity.Inst.GetBuildID() + "\"");
				break;
			case "clearett":
			{
				EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
				using (EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(SceneEttBED)))
				{
					DynamicBuffer<SceneEttBED> singletonBuffer = entityQuery.GetSingletonBuffer<SceneEttBED>();
					for (int j = 0; j < singletonBuffer.Length; j++)
					{
						if (singletonBuffer[j].ett_AccessD != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_AccessD);
						}
						if (singletonBuffer[j].ett_AccessL != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_AccessL);
						}
						if (singletonBuffer[j].ett_AccessR != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_AccessR);
						}
						if (singletonBuffer[j].ett_AccessU != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_AccessU);
						}
						if (singletonBuffer[j].ett_Boundary != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Boundary);
						}
						if (singletonBuffer[j].ett_Boundary2 != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Boundary2);
						}
						if (singletonBuffer[j].ett_Door != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Door);
						}
						if (singletonBuffer[j].ett_Tile0 != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Tile0);
						}
						if (singletonBuffer[j].ett_Tile1 != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Tile1);
						}
						if (singletonBuffer[j].ett_Tile2 != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Tile2);
						}
						if (singletonBuffer[j].ett_Tile3 != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Tile3);
						}
						if (singletonBuffer[j].ett_Tile4 != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Tile4);
						}
						if (singletonBuffer[j].ett_Tile5 != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Tile5);
						}
						if (singletonBuffer[j].ett_Tile6 != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Tile6);
						}
						if (singletonBuffer[j].ett_Tile7 != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Tile7);
						}
						if (singletonBuffer[j].ett_Tile8 != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Tile8);
						}
						if (singletonBuffer[j].ett_Tile9 != Entity.Null)
						{
							entityCommandBuffer.DestroyEntity(singletonBuffer[j].ett_Tile9);
						}
					}
				}
				using (EntityQuery entityQuery2 = ettMgr.CreateEntityQuery(typeof(AllUnitEtt)))
				{
					foreach (KVPair<int, Entity> item4 in entityQuery2.GetSingleton<AllUnitEtt>().map)
					{
						entityCommandBuffer.DestroyEntity(item4.Value);
					}
				}
				using (EntityQuery entityQuery3 = ettMgr.CreateEntityQuery(typeof(AllSpecialObjEtt)))
				{
					foreach (KVPair<int, Entity> item5 in entityQuery3.GetSingleton<AllSpecialObjEtt>().map)
					{
						entityCommandBuffer.DestroyEntity(item5.Value);
					}
				}
				using (EntityQuery entityQuery4 = ettMgr.CreateEntityQuery(typeof(AllMixedEtt)))
				{
					foreach (KVPair<FixedString128Bytes, Entity> item6 in entityQuery4.GetSingleton<AllMixedEtt>().map)
					{
						entityCommandBuffer.DestroyEntity(item6.Value);
					}
				}
				entityCommandBuffer.Playback(ettMgr);
				entityCommandBuffer.Dispose();
				PrintLog("已清空ett");
				break;
			}
			case "closecam":
				GameObject.Find("Cam_Main").GetComponent<Camera>().cullingMask = 0;
				break;
			case "closeuicam":
				GameObject.Find("Cam_UI").GetComponent<Camera>().cullingMask = 0;
				break;
			case "cputest":
				QualityPreset.CpuTest(delegate(float score)
				{
					GameUISingletonMono<UICommonHint>.ShowInit(score.ToString("F5"));
				});
				break;
			case "lastcpucost":
				PrintLog(QualityPreset.LastTestScore.ToString(CultureInfo.InvariantCulture));
				break;
			case "savecfg":
				GameMgr.Inst.SaveDataToString();
				break;
			case "loadcfg":
				if (array.Length == 2)
				{
					StringSaveData data = SaveDataCompressor.DecompressSaveData(array[1]);
					GameMgr.Inst.LoadDataFromStringSaveData(data);
				}
				else
				{
					PrintLog("<color=red>格式错误！！！</color>");
				}
				break;
			default:
				PrintLog("<color=red>不存在指令或指令参数有误</color>");
				break;
			case "npc7":
				break;
			}
		}
		else
		{
			PrintLog("没有这个指令：" + cmdString[0]);
		}
		debugCmdInputField.text = "";
	}

	public void FinishCmd()
	{
		string text = debugCmdInputField.text.Trim();
		if (text.Length == 0)
		{
			return;
		}
		bool flag = !(text == "`") && !(text == "·");
		if (cmdHistory.Count > 0)
		{
			List<string> list = cmdHistory;
			if (list[list.Count - 1] == text)
			{
				flag = false;
			}
		}
		if (flag)
		{
			cmdHistory.Add(text);
			while (cmdHistory.Count > 30)
			{
				cmdHistory.RemoveAt(0);
			}
			PlayerPrefs.SetString("CMD_History", string.Join("\n", cmdHistory));
		}
		historyArrow = cmdHistory.Count - 1;
		string[] array = (from e in debugCmdInputField.text.Trim().Split(";")
			select e.Trim() into e
			where e.Length > 0
			select e).ToArray();
		foreach (string cmdString in array)
		{
			RunDebugCmd(cmdString);
		}
	}

	public void TestButtonBackQuote()
	{
		go_CommandLine.SetActive(!go_CommandLine.activeSelf);
		if (!EventSystem.current.alreadySelecting)
		{
			EventSystem.current.SetSelectedGameObject(null);
			debugCmdInputField.ActivateInputField();
		}
		debugCmdInputField.text = string.Empty;
		debugCmdInputField.ForceLabelUpdate();
	}
}
