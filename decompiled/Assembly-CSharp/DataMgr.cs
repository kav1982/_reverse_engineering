using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.Android;

public class DataMgr
{
	public class MobileCloudSaveData
	{
		public WorldData WorldData01;

		public WorldData WorldData02;

		public WorldData WorldData03;
	}

	public interface IConfigInitializer
	{
		bool IsDone { get; }

		void ReadJson();

		void Parse();

		void UpdateData();
	}

	public abstract class ConfigInitializer<T> : IConfigInitializer
	{
		protected string _json;

		protected List<T> rawData;

		public bool IsDone { get; protected set; }

		public virtual void ReadJson()
		{
			_json = GetConfigText<T>();
		}

		public virtual void Parse()
		{
			rawData = ParseJsonList<T>(_json);
			ApplyResult(rawData);
			IsDone = true;
			_json = "";
		}

		public void UpdateData()
		{
			ApplyResult(rawData);
		}

		public abstract void ApplyResult(List<T> result);
	}

	public static FinishEndlessGameBuilds finishEndlessGameBuilds;

	public static FinishGameBuilds finishGameBuilds;

	public static int currentSelectWorldIndex;

	public static string dataPath;

	public static string worldData0FileName = "WorldData0.Json";

	public static string worldData1FileName = "WorldData1.Json";

	public static string worldData2FileName = "WorldData2.Json";

	public static string finishBuildsFileName = "FinishBuilds.Json";

	public static string finishEndlessBuildsFileName = "FinishEndlessBuilds.Json";

	public static string settingDataFileName = "SettingData.Json";

	public static string mobileCloudSaveDataFileName = "MobileCloudSaveData";

	public static string worldDataNameWithOutEx = "WorldData";

	public static string worldDataBackupWithOutEx = "BackupData";

	public static string finishBuildsBackupFileName = "BackUpFinishBuilds";

	public static string finishEndlessBuildsBackupFileName = "BackUpFinishEndlessBuilds";

	public static string DataBackup0FileName = "BackupData1";

	public static string DataBackup1FileName = "BackupData2";

	public static string DataBackup2FileName = "BackupData3";

	public static char[] keyChars = new char[7] { 'E', 'n', 'c', 'r', 'y', 'p', 't' };

	private static List<IConfigInitializer> _initializers;

	private static ConfigInitializer<UnitConfig> unitConfigInitializer;

	public static WorldData selectedWorldData { get; private set; }

	public static WorldData WorldData0 { get; private set; }

	public static WorldData WorldData1 { get; private set; }

	public static WorldData WorldData2 { get; private set; }

	public static SettingData settingData { get; private set; }

	public static bool InitializeIsDone()
	{
		return _initializers.All((IConfigInitializer e) => e.IsDone);
	}

	public static void Initialize()
	{
		TextConfig.dic.Clear();
		unitConfigInitializer = new UnitConfig.Initializer();
		unitConfigInitializer.ReadJson();
		unitConfigInitializer.Parse();
		_initializers = new List<IConfigInitializer>
		{
			new HardDialogueConfig.Initializer(),
			new ResourceConfig.Initializer(),
			new WandConfig.Initializer(),
			new PotionConfig.Initializer(),
			new CurseConfig.Initializer(),
			new SpecialObjConfig.Initializer(),
			new ResearchConfig.Initializer(),
			new SetConfig.Initializer(),
			new HandbookConfig.Initializer(),
			new ActivateGirlConfig.Initializer(),
			new MultiDifficultyConfig.Initializer(),
			new SpellConfig.Initializer(),
			new RelicConfig.Initializer(),
			new RoomConfig.Initializer(),
			new RelicGroupConfig.Initializer(),
			new RawEndlessWaveConfig.Initializer(),
			new EndlessUnitConfig.Initializer()
		};
		_initializers.AddRange(TextConfig.Initializer.GetAllInitializer());
		foreach (IConfigInitializer initializer in _initializers)
		{
			initializer.ReadJson();
		}
		new Thread((ThreadStart)delegate
		{
			foreach (IConfigInitializer initializer2 in _initializers)
			{
				initializer2.Parse();
			}
		}).Start();
		dataPath = "";
		if (Application.isMobilePlatform)
		{
			if (!Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE"))
			{
				dataPath = Application.persistentDataPath;
				dataPath += "/Magicraft/";
			}
			else
			{
				dataPath = Application.persistentDataPath;
				dataPath += "/Magicraft/";
			}
		}
		else
		{
			dataPath = Application.persistentDataPath + "/";
		}
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		SetAllFileAttributes();
		LoadWorldData();
		LoadFinishBuild();
		LoadEndlessFinishBuild();
		selectedWorldData = WorldData0;
		LoadSettingData();
		GameInit.Pin("DataMgr Method Initialize end");
	}

	public static void CombineToCloudSaveFile()
	{
		if (!Directory.Exists(dataPath + mobileCloudSaveDataFileName))
		{
			Directory.CreateDirectory(dataPath + mobileCloudSaveDataFileName);
		}
		string text = dataPath + mobileCloudSaveDataFileName + "/" + mobileCloudSaveDataFileName + ".json";
		if (!File.Exists(text))
		{
			MobileCloudSaveData value = new MobileCloudSaveData
			{
				WorldData01 = WorldData0,
				WorldData02 = WorldData1,
				WorldData03 = WorldData2
			};
			File.WriteAllText(text, Encrypt(JsonConvert.SerializeObject(value)));
			Debug.Log("写入云存档到" + text);
		}
	}

	public static bool OverlayCurrentSaveData()
	{
		if (!Directory.Exists(dataPath + mobileCloudSaveDataFileName))
		{
			Debug.Log("新建文件夹" + dataPath + mobileCloudSaveDataFileName);
			Directory.CreateDirectory(dataPath + mobileCloudSaveDataFileName);
			return false;
		}
		string text = dataPath + mobileCloudSaveDataFileName + "/" + mobileCloudSaveDataFileName + ".json";
		Debug.Log("存档路径：" + text + "是否存在" + File.Exists(text));
		if (File.Exists(text))
		{
			MobileCloudSaveData? mobileCloudSaveData = JsonConvert.DeserializeObject<MobileCloudSaveData>(Encrypt(File.ReadAllText(text)));
			DeleteWorldData(0);
			DeleteWorldData(1);
			DeleteWorldData(2);
			WorldData0 = mobileCloudSaveData!.WorldData01;
			WorldData1 = mobileCloudSaveData!.WorldData02;
			WorldData2 = mobileCloudSaveData!.WorldData03;
			SaveWorldData(WorldData0);
			SaveWorldData(WorldData1);
			SaveWorldData(WorldData2);
			File.Delete(text);
			Debug.Log("覆盖存档成功");
			return true;
		}
		return false;
	}

	public static void UpdateData()
	{
		TextConfig.dic = new Dictionary<int, TextConfig>();
		TextConfig.list = new List<TextConfig>();
		foreach (IConfigInitializer initializer in _initializers)
		{
			initializer.UpdateData();
		}
		unitConfigInitializer.UpdateData();
		TextConfig.RegetAllText();
	}

	public static string GetConfigText<T>()
	{
		return GetConfigText(typeof(T).Name);
	}

	public static string[] GetSplitConfigText<T>(int split)
	{
		string[] array = new string[split];
		for (int i = 0; i < split; i++)
		{
			array[i] = GetConfigText(typeof(T).Name + $"-{i}");
		}
		return array;
	}

	public static string GetConfigText(string configName)
	{
		return ABResources.LoadAsset<TextAsset>("Configs/" + configName).text;
	}

	public static List<T> ParseJsonList<T>(string json)
	{
		return JsonConvert.DeserializeObject<List<T>>(json);
	}

	public static List<T> GetList<T>()
	{
		return GetList<T>(typeof(T).Name);
	}

	public static List<T> GetList<T>(string configName)
	{
		return JsonConvert.DeserializeObject<List<T>>(ABResources.LoadAsset<TextAsset>("Configs/" + configName).text);
	}

	public static void LoadWorldData()
	{
		if (File.Exists(dataPath + worldData0FileName) || File.Exists(dataPath + worldData1FileName) || File.Exists(dataPath + worldData2FileName))
		{
			WorldData0 = LoadWorldDataComparedBackup(0);
			WorldData1 = LoadWorldDataComparedBackup(1);
			WorldData2 = LoadWorldDataComparedBackup(2);
		}
		else
		{
			Debug.Log("三个存档文件都不存在");
			WorldData0 = LoadWorldDataComparedBackup(0);
			WorldData1 = LoadWorldDataComparedBackup(1);
			WorldData2 = LoadWorldDataComparedBackup(2);
		}
		WorldData0.CorrectData();
		WorldData1.CorrectData();
		WorldData2.CorrectData();
	}

	public static WorldData LoadWorldDataComparedBackup(int id)
	{
		string text = dataPath + worldDataNameWithOutEx + id + ".Json";
		string text2 = dataPath + worldDataBackupWithOutEx + (id + 1);
		WorldData worldData = TryLoadEncryptWorldData(text);
		WorldData worldData2 = TryLoadEncryptWorldData(text2);
		if (worldData == null)
		{
			SaveWorldDataToErrorBackup(text);
			if (worldData2 == null)
			{
				SaveWorldDataToErrorBackup(text2);
				Debug.Log("备份存档文件不存在或读取失败,新建存档");
				worldData = new WorldData();
				File.WriteAllText(text, Encrypt(JsonConvert.SerializeObject(worldData)));
				try
				{
					File.Copy(text, text2, overwrite: true);
					return worldData;
				}
				catch (IOException ex)
				{
					Debug.LogError("文件复制失败：" + ex.Message);
					return worldData;
				}
			}
			Debug.Log("备份存档文件读取成功");
			try
			{
				File.Copy(text2, text, overwrite: true);
				return worldData2;
			}
			catch (IOException ex2)
			{
				Debug.LogError("文件复制失败：" + ex2.Message);
				return worldData2;
			}
		}
		if (worldData2 == null)
		{
			Debug.Log("存档文件存在,备份存档不存在或读取失败,更新备份存档,跳过后面的比较");
			try
			{
				File.Copy(text, text2, overwrite: true);
				return worldData;
			}
			catch (IOException ex3)
			{
				Debug.LogError("文件复制失败：" + ex3.Message);
				return worldData;
			}
		}
		Debug.Log("比较:" + worldData.playTime + "和" + worldData2.playTime);
		if (Mathf.Approximately(worldData.playTime, worldData2.playTime))
		{
			return worldData;
		}
		if (worldData.playTime > worldData2.playTime)
		{
			try
			{
				File.Copy(text, text2, overwrite: true);
				return worldData;
			}
			catch (IOException ex4)
			{
				Debug.LogError("文件复制失败：" + ex4.Message);
				return worldData;
			}
		}
		if (worldData.playTime < worldData2.playTime)
		{
			try
			{
				File.Copy(text2, text, overwrite: true);
				return worldData2;
			}
			catch (IOException ex5)
			{
				Debug.LogError("文件复制失败：" + ex5.Message);
				return worldData2;
			}
		}
		Debug.Log("怎么会运行到这里?");
		return worldData;
	}

	private static WorldData TryLoadEncryptWorldData(string stringFileName)
	{
		if (!File.Exists(stringFileName))
		{
			Debug.Log("文件不存在:" + stringFileName);
			return null;
		}
		string text = File.ReadAllText(stringFileName);
		try
		{
			return JsonConvert.DeserializeObject<WorldData>(Encrypt(text));
		}
		catch (Exception ex)
		{
			Debug.LogError("存档(" + stringFileName + ")解密读取失败，尝试不解密读取\n" + ex.Message + "\n" + ex.StackTrace);
			try
			{
				WorldData result = JsonConvert.DeserializeObject<WorldData>(text);
				try
				{
					Debug.Log("不解密后读取成功,保存加密");
					File.WriteAllText(stringFileName, Encrypt(text));
				}
				catch (Exception ex2)
				{
					Debug.LogError("读取不加密成功，写入加密失败" + ex2.Message);
				}
				return result;
			}
			catch (Exception ex3)
			{
				Debug.LogError("不解密也读不了,新建WorldData" + ex3.Message);
				return null;
			}
		}
	}

	private static void SaveWorldDataToErrorBackup(string sourceFilePath)
	{
		if (!File.Exists(sourceFilePath))
		{
			Debug.Log("文件不存在:" + sourceFilePath);
			return;
		}
		try
		{
			string text = $"Error({DateTime.Now:yyyyMMdd_HHmmss})_{Path.GetFileName(sourceFilePath)}";
			string destFileName = Path.Join(Path.GetDirectoryName(sourceFilePath), text);
			File.Copy(sourceFilePath, destFileName, overwrite: true);
		}
		catch (Exception ex)
		{
			Debug.LogError("保存出错存档备份失败：" + ex.Message + "\n" + ex.StackTrace);
		}
	}

	public static void LoadSettingData()
	{
		if (File.Exists(dataPath + settingDataFileName))
		{
			string text = File.ReadAllText(dataPath + settingDataFileName);
			try
			{
				settingData = JsonConvert.DeserializeObject<SettingData>(Encrypt(text));
			}
			catch (Exception ex)
			{
				Debug.LogError("设置解密读取失败，尝试不解密读取。" + ex.Message);
				try
				{
					settingData = JsonConvert.DeserializeObject<SettingData>(text);
					try
					{
						File.WriteAllText(dataPath + settingDataFileName, Encrypt(text));
						try
						{
							Debug.Log("设置不解密后读取成功,保存加密,并备份");
							SaveSettingData();
						}
						catch (Exception ex2)
						{
							Debug.LogError("设置不解密后读取成功后保存加密并备份失败。" + ex2.Message);
						}
					}
					catch (Exception ex3)
					{
						Debug.LogError("设置不加密成功，写入加密失败。" + ex3.Message);
					}
				}
				catch (Exception ex4)
				{
					Debug.LogError("不解密也读不了设置，等后面读备份。" + ex4.Message);
				}
			}
			if (settingData == null)
			{
				settingData = new SettingData();
				SaveSettingData();
			}
		}
		else
		{
			settingData = new SettingData();
			if (GameMgr.IsMobile_Static)
			{
				switch (QualityPreset.QualitySet)
				{
				case 0:
					settingData.resTypeMobile = ResolutionTypeMobile.Res40;
					settingData.MobileTargetFrameRate = MobileTargetFrameRate.Target60;
					break;
				case 1:
					settingData.resTypeMobile = ResolutionTypeMobile.Res60;
					settingData.MobileTargetFrameRate = MobileTargetFrameRate.Target60;
					break;
				case 2:
					settingData.resTypeMobile = ResolutionTypeMobile.Res80;
					settingData.MobileTargetFrameRate = MobileTargetFrameRate.Target60;
					break;
				}
			}
			SaveSettingData();
		}
		if (ScriptableObjMgr.Inst.testCtrller.publishTesting || GameMgr.IsMobile_Static)
		{
			settingData.language = LanguageType.ChineseS;
		}
		if (GameMgr.IsMobile_Static && GameMgr.IsAndroidSimulator)
		{
			settingData.resTypeMobile = ResolutionTypeMobile.Res80;
			settingData.MobileTargetFrameRate = MobileTargetFrameRate.Target90;
		}
		int num = Enum.GetNames(typeof(VirtualStickSizeAdjust.AdjustType)).Length;
		if (settingData.Mobiledata.virtualStickData2.Count < num)
		{
			while (settingData.Mobiledata.virtualStickData2.Count < num)
			{
				settingData.Mobiledata.virtualStickData2.Add(new MobileVirtualButtonData());
			}
		}
	}

	public static void LoadFinishBuild()
	{
		if (File.Exists(dataPath + finishBuildsFileName))
		{
			string text = "";
			try
			{
				text = File.ReadAllText(dataPath + finishBuildsFileName);
				finishGameBuilds = JsonConvert.DeserializeObject<FinishGameBuilds>(Encrypt(text));
			}
			catch (Exception ex)
			{
				Debug.LogError("通关数据解密读取失败，尝试不解密读取" + ex.Message);
				try
				{
					finishGameBuilds = JsonConvert.DeserializeObject<FinishGameBuilds>(text);
					try
					{
						File.WriteAllText(dataPath + finishBuildsFileName, Encrypt(text));
						try
						{
							Debug.Log("通关数据不解密后读取成功,保存加密,并备份");
							SaveBuildDatas();
							SaveBuildBackUp();
						}
						catch (Exception ex2)
						{
							Debug.LogError("通关数据不解密后读取成功后保存加密并备份失败" + ex2.Message);
						}
					}
					catch (Exception ex3)
					{
						Debug.LogError("读取不加密成功，写入加密失败" + ex3.Message);
					}
				}
				catch (Exception ex4)
				{
					Debug.LogError("不解密也读不了" + ex4.Message);
				}
			}
			if (finishGameBuilds == null)
			{
				Debug.Log("尝试读取备份");
				if (!LoadFinishBuildBackup())
				{
					Debug.Log("生成新的通关记录");
					finishGameBuilds = new FinishGameBuilds();
				}
			}
		}
		else if (!LoadFinishBuildBackup())
		{
			Debug.Log("生成新的通关记录");
			finishGameBuilds = new FinishGameBuilds();
			if (finishGameBuilds == null || finishGameBuilds.finishGameBuilds == null)
			{
				Debug.Log("通关数据不存在");
			}
			else
			{
				Debug.Log("通关数据存在");
			}
		}
		static bool LoadFinishBuildBackup()
		{
			if (File.Exists(dataPath + finishBuildsBackupFileName))
			{
				try
				{
					finishGameBuilds = JsonConvert.DeserializeObject<FinishGameBuilds>(Encrypt(File.ReadAllText(dataPath + finishBuildsBackupFileName)));
					Debug.Log("通关记录备份读取成功,替换存档");
					File.Copy(dataPath + finishBuildsBackupFileName, dataPath + finishBuildsFileName, overwrite: true);
					return true;
				}
				catch (Exception ex5)
				{
					Debug.LogError(ex5.Message);
					Debug.Log("通关记录备份读取失败");
					return false;
				}
			}
			Debug.Log("通关记录备份不存在");
			return false;
		}
	}

	public static void SaveBuildDatas()
	{
		try
		{
			string data = JsonConvert.SerializeObject(finishGameBuilds);
			File.WriteAllText(dataPath + finishBuildsFileName, Encrypt(data));
		}
		catch (Exception ex)
		{
			Debug.LogError("写入游戏通关记录出错。" + ex.Message);
		}
	}

	public static void SaveBuildBackUp()
	{
		try
		{
			File.Copy(dataPath + finishBuildsFileName, dataPath + finishBuildsBackupFileName, overwrite: true);
		}
		catch (Exception ex)
		{
			Debug.LogError("备份游戏通关记录出错。" + ex.Message);
		}
	}

	public static void SaveEndlessBuildDatas()
	{
		try
		{
			string data = JsonConvert.SerializeObject(finishEndlessGameBuilds);
			File.WriteAllText(dataPath + finishEndlessBuildsFileName, Encrypt(data));
		}
		catch (Exception ex)
		{
			Debug.LogError("写入无尽游戏通关记录出错。" + ex.Message);
		}
	}

	public static void SaveEndlessBuildBackUp()
	{
		try
		{
			File.Copy(dataPath + finishEndlessBuildsFileName, dataPath + finishEndlessBuildsBackupFileName, overwrite: true);
		}
		catch (Exception ex)
		{
			Debug.LogError("备份无尽游戏通关记录出错。" + ex.Message);
		}
	}

	public static void LoadEndlessFinishBuild()
	{
		if (File.Exists(dataPath + finishEndlessBuildsFileName))
		{
			string text = "";
			try
			{
				text = File.ReadAllText(dataPath + finishEndlessBuildsFileName);
				finishEndlessGameBuilds = JsonConvert.DeserializeObject<FinishEndlessGameBuilds>(Encrypt(text));
			}
			catch (Exception ex)
			{
				Debug.LogError("通关无尽数据解密读取失败，尝试不解密读取" + ex.Message);
				try
				{
					finishEndlessGameBuilds = JsonConvert.DeserializeObject<FinishEndlessGameBuilds>(text);
					try
					{
						File.WriteAllText(dataPath + finishEndlessBuildsFileName, Encrypt(text));
						try
						{
							Debug.Log("无尽通关数据不解密后读取成功,保存加密,并备份");
							SaveEndlessBuildDatas();
							SaveEndlessBuildBackUp();
						}
						catch (Exception ex2)
						{
							Debug.LogError("无尽通关数据不解密后读取成功后保存加密并备份失败" + ex2.Message);
						}
					}
					catch (Exception ex3)
					{
						Debug.LogError("读取不加密成功，写入加密失败" + ex3.Message);
					}
				}
				catch (Exception ex4)
				{
					Debug.LogError("不解密也读不了" + ex4.Message);
				}
			}
			if (finishEndlessGameBuilds == null)
			{
				Debug.Log("尝试读取无尽备份");
				if (!LoadFinishEndlessBuildBackup())
				{
					Debug.Log("生成新的无尽通关记录");
					finishEndlessGameBuilds = new FinishEndlessGameBuilds();
				}
			}
		}
		else if (!LoadFinishEndlessBuildBackup())
		{
			Debug.Log("生成新的无尽结算记录");
			finishEndlessGameBuilds = new FinishEndlessGameBuilds();
		}
		static bool LoadFinishEndlessBuildBackup()
		{
			if (File.Exists(dataPath + finishEndlessBuildsBackupFileName))
			{
				try
				{
					finishEndlessGameBuilds = JsonConvert.DeserializeObject<FinishEndlessGameBuilds>(Encrypt(File.ReadAllText(dataPath + finishEndlessBuildsBackupFileName)));
					Debug.Log("无尽记录备份读取成功,替换存档");
					File.Copy(dataPath + finishEndlessBuildsBackupFileName, dataPath + finishEndlessBuildsFileName, overwrite: true);
					return true;
				}
				catch (Exception ex5)
				{
					Debug.LogError(ex5.Message);
					Debug.Log("无尽记录备份读取失败");
					return false;
				}
			}
			Debug.Log("无尽记录备份不存在");
			return false;
		}
	}

	public static void SaveWorldData(WorldData worldData)
	{
		Debug.Log("SaveData");
		if (worldData == null)
		{
			Debug.LogError("!");
			return;
		}
		worldData.PlayTimeSettle();
		string data;
		if (worldData.inBattle9)
		{
			Curse_RandomCurse curse_RandomCurseCommon = PlayerMgr.Inst.ItemCtrller.curse_RandomCurseCommon;
			Curse_RandomCurse curse_RandomCurseRare = PlayerMgr.Inst.ItemCtrller.curse_RandomCurseRare;
			if (curse_RandomCurseCommon != null)
			{
				int index = worldData.battleData9.curseIDs.IndexOf(curse_RandomCurseCommon.RandomCurseID);
				if (worldData.battleData9.curseLevels[index] > 1)
				{
					worldData.battleData9.curseLevels[index]--;
				}
				else
				{
					worldData.battleData9.curseIDs.RemoveAt(index);
					worldData.battleData9.curseLevels.RemoveAt(index);
				}
				worldData.battleData9.curseIDs.Add(curse_RandomCurseCommon.CurseCfg.id);
				worldData.battleData9.curseLevels.Add(1);
				if (curse_RandomCurseCommon.RandomCurseID == 35)
				{
					int num = 0;
					num = ((!worldData.battleData9.curseIDs.Contains(35)) ? CurseConfig.dic[35].int1.value : CurseConfig.dic[35].int1.valueUpgrade);
					worldData.battleData9.bagCount += num;
					for (int i = 0; i < num; i++)
					{
						PlayerMgr.Inst.BaData.bagSpellDatas.Add(null);
					}
				}
			}
			if (curse_RandomCurseRare != null)
			{
				int index2 = worldData.battleData9.curseIDs.IndexOf(curse_RandomCurseRare.RandomCurseID);
				if (worldData.battleData9.curseLevels[index2] > 1)
				{
					worldData.battleData9.curseLevels[index2]--;
				}
				else
				{
					worldData.battleData9.curseIDs.RemoveAt(index2);
					worldData.battleData9.curseLevels.RemoveAt(index2);
				}
				worldData.battleData9.curseIDs.Add(curse_RandomCurseRare.CurseCfg.id);
				worldData.battleData9.curseLevels.Add(1);
			}
			if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
			{
				worldData.battleData9.playerCfg = playerPpt.unitCfg;
			}
			data = JsonConvert.SerializeObject(worldData);
			if (curse_RandomCurseCommon != null)
			{
				int index3 = worldData.battleData9.curseIDs.IndexOf(curse_RandomCurseCommon.CurseCfg.id);
				worldData.battleData9.curseIDs.RemoveAt(index3);
				worldData.battleData9.curseLevels.RemoveAt(index3);
				if (worldData.battleData9.curseIDs.Contains(curse_RandomCurseCommon.RandomCurseID))
				{
					int index4 = worldData.battleData9.curseIDs.IndexOf(curse_RandomCurseCommon.RandomCurseID);
					worldData.battleData9.curseLevels[index4]++;
				}
				else
				{
					worldData.battleData9.curseIDs.Add(curse_RandomCurseCommon.RandomCurseID);
					worldData.battleData9.curseLevels.Add(1);
				}
				if (curse_RandomCurseCommon.RandomCurseID == 35)
				{
					int num2 = 0;
					int index5 = worldData.battleData9.curseIDs.IndexOf(35);
					num2 = ((worldData.battleData9.curseLevels[index5] != 1) ? CurseConfig.dic[35].int1.valueUpgrade : CurseConfig.dic[35].int1.value);
					worldData.battleData9.bagCount -= num2;
					for (int j = 0; j < num2; j++)
					{
						PlayerMgr.Inst.BaData.bagSpellDatas.RemoveAt(PlayerMgr.Inst.BaData.bagSpellDatas.Count - 1);
					}
				}
			}
			if (curse_RandomCurseRare != null)
			{
				int index6 = worldData.battleData9.curseIDs.IndexOf(curse_RandomCurseRare.CurseCfg.id);
				worldData.battleData9.curseIDs.RemoveAt(index6);
				worldData.battleData9.curseLevels.RemoveAt(index6);
				if (worldData.battleData9.curseIDs.Contains(curse_RandomCurseRare.RandomCurseID))
				{
					int index7 = worldData.battleData9.curseIDs.IndexOf(curse_RandomCurseRare.RandomCurseID);
					worldData.battleData9.curseLevels[index7]++;
				}
				else
				{
					worldData.battleData9.curseIDs.Add(curse_RandomCurseRare.RandomCurseID);
					worldData.battleData9.curseLevels.Add(1);
				}
			}
		}
		else
		{
			BattleData battleData = worldData.battleData9;
			worldData.battleData9 = null;
			data = JsonConvert.SerializeObject(worldData);
			worldData.battleData9 = battleData;
		}
		string text = "";
		string value = Encrypt(data);
		if (worldData == WorldData0)
		{
			text = dataPath + worldData0FileName;
		}
		else if (worldData == WorldData1)
		{
			text = dataPath + worldData1FileName;
		}
		else
		{
			if (worldData != WorldData2)
			{
				Debug.LogError("!");
				return;
			}
			text = dataPath + worldData2FileName;
		}
		using FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write);
		using StreamWriter streamWriter = new StreamWriter(fileStream);
		streamWriter.Write(value);
		streamWriter.Flush();
		fileStream.Flush(flushToDisk: true);
	}

	public static void SaveSelectedWorldData()
	{
		SaveWorldData(selectedWorldData);
	}

	public static void SaveWorldDataBackup()
	{
		Debug.Log("保存备份存档");
		try
		{
			if (selectedWorldData == WorldData0)
			{
				File.Copy(dataPath + worldData0FileName, dataPath + DataBackup0FileName, overwrite: true);
			}
			else if (selectedWorldData == WorldData1)
			{
				File.Copy(dataPath + worldData1FileName, dataPath + DataBackup1FileName, overwrite: true);
			}
			else if (selectedWorldData == WorldData2)
			{
				File.Copy(dataPath + worldData2FileName, dataPath + DataBackup2FileName, overwrite: true);
			}
			else
			{
				Debug.LogError("!");
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("文件复制失败：" + ex.Message);
		}
	}

	public static void SaveSettingData()
	{
		try
		{
			File.WriteAllText(dataPath + settingDataFileName, Encrypt(JsonConvert.SerializeObject(settingData)));
		}
		catch (Exception ex)
		{
			Debug.LogError("保存设置出错。" + ex.Message);
		}
	}

	private static void SetAllFileAttributes()
	{
		SetFileAttributesNotHiddenReadWrite(dataPath + worldData0FileName);
		SetFileAttributesNotHiddenReadWrite(dataPath + worldData1FileName);
		SetFileAttributesNotHiddenReadWrite(dataPath + worldData2FileName);
		SetFileAttributesNotHiddenReadWrite(dataPath + DataBackup0FileName);
		SetFileAttributesNotHiddenReadWrite(dataPath + DataBackup1FileName);
		SetFileAttributesNotHiddenReadWrite(dataPath + DataBackup2FileName);
		SetFileAttributesNotHiddenReadWrite(dataPath + settingDataFileName);
		SetFileAttributesNotHiddenReadWrite(dataPath + finishBuildsFileName);
		SetFileAttributesNotHiddenReadWrite(dataPath + finishBuildsBackupFileName);
	}

	public static void SetFileAttributesNotHiddenReadWrite(string path)
	{
		if (File.Exists(path))
		{
			FileAttributes attributes = File.GetAttributes(path);
			if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				Debug.Log("文件是只读的");
				SetReadWrite(path);
			}
			if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
			{
				Debug.Log("文件是隐藏的");
				SetNotHidden(path);
			}
		}
	}

	private static void SetReadWrite(string path)
	{
		try
		{
			FileAttributes attributes = File.GetAttributes(path);
			attributes &= ~FileAttributes.ReadOnly;
			File.SetAttributes(path, attributes);
			Debug.Log("移除只读属性");
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			Debug.LogError("没有权限吗?");
		}
	}

	private static void SetNotHidden(string path)
	{
		try
		{
			FileAttributes attributes = File.GetAttributes(path);
			attributes &= ~FileAttributes.Hidden;
			File.SetAttributes(path, attributes);
			Debug.Log("移除隐藏属性");
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			Debug.LogError("没有权限吗?");
		}
	}

	public static void SetSelectedWorldData(int index)
	{
		currentSelectWorldIndex = index;
		switch (index)
		{
		case 0:
			selectedWorldData = WorldData0;
			break;
		case 1:
			selectedWorldData = WorldData1;
			break;
		case 2:
			selectedWorldData = WorldData2;
			break;
		default:
			Debug.LogError(index);
			break;
		}
		if (!ICJNOGPFMAM.FIKDMCBJPCO)
		{
			if (selectedWorldData.campSkinType == CampSkinType.Halloween)
			{
				selectedWorldData.campSkinType = CampSkinType.Default;
			}
			if (selectedWorldData.playerLook == PlayerLook.Halloween)
			{
				selectedWorldData.playerLook = PlayerLook.Default;
			}
		}
		if (!ICJNOGPFMAM.ACPKKMJKOJD)
		{
			if (selectedWorldData.campSkinType == CampSkinType.Spring)
			{
				selectedWorldData.campSkinType = CampSkinType.Default;
			}
			if (selectedWorldData.playerLook == PlayerLook.Horse)
			{
				selectedWorldData.playerLook = PlayerLook.Default;
			}
		}
		if (!ICJNOGPFMAM.BHEHHIFGJOE)
		{
			if (selectedWorldData.campSkinType == CampSkinType.Summer)
			{
				selectedWorldData.campSkinType = CampSkinType.Default;
			}
			if (selectedWorldData.playerLook == PlayerLook.SummerBoy || selectedWorldData.playerLook == PlayerLook.SummerGirl)
			{
				selectedWorldData.playerLook = PlayerLook.Default;
			}
		}
		if (!ICJNOGPFMAM.MADIIMLEMNP)
		{
			if (selectedWorldData.campSkinType == CampSkinType.Christmas)
			{
				selectedWorldData.campSkinType = CampSkinType.Default;
			}
			if (selectedWorldData.playerLook == PlayerLook.SnowMan)
			{
				selectedWorldData.playerLook = PlayerLook.Default;
			}
		}
		if (GameMgr.IsMobile_Static && GameMgr.IsUseBiliOneSDK)
		{
			if (selectedWorldData.playerLook == PlayerLook.HaoYou || selectedWorldData.playerLook == PlayerLook.TapTap)
			{
				selectedWorldData.playerLook = PlayerLook.Default;
			}
			return;
		}
		PlayerLook playerLook = selectedWorldData.playerLook;
		if (playerLook == PlayerLook.HaoYou || playerLook == PlayerLook.TapTap)
		{
			selectedWorldData.playerLook = PlayerLook.Default;
		}
	}

	public static void DeleteWorldData(int index)
	{
		Debug.Log("删除存档");
		DeleteBackupdData(index);
		switch (index)
		{
		case 0:
			WorldData0 = new WorldData();
			SaveWorldData(WorldData0);
			break;
		case 1:
			WorldData1 = new WorldData();
			SaveWorldData(WorldData1);
			break;
		case 2:
			WorldData2 = new WorldData();
			SaveWorldData(WorldData2);
			break;
		default:
			Debug.LogError(index);
			break;
		}
	}

	public static void DeleteBackupdData(int index)
	{
		switch (index)
		{
		case 0:
			if (File.Exists(dataPath + DataBackup0FileName))
			{
				try
				{
					Debug.Log("删除备份文件1");
					File.Delete(dataPath + DataBackup0FileName);
					break;
				}
				catch (Exception ex3)
				{
					Debug.LogError("备份文件1无法删除？" + ex3.Message);
					break;
				}
			}
			Debug.Log("备份不存在");
			break;
		case 1:
			if (File.Exists(dataPath + DataBackup1FileName))
			{
				try
				{
					Debug.Log("删除备份文件2");
					File.Delete(dataPath + DataBackup1FileName);
					break;
				}
				catch (Exception ex2)
				{
					Debug.LogError("备份文件2无法删除？" + ex2.Message);
					break;
				}
			}
			Debug.Log("备份不存在");
			break;
		case 2:
			if (File.Exists(dataPath + DataBackup2FileName))
			{
				try
				{
					File.Delete(dataPath + DataBackup2FileName);
					Debug.Log("删除备份文件3");
					break;
				}
				catch (Exception ex)
				{
					Debug.LogError("备份文件3无法删除？" + ex.Message);
					break;
				}
			}
			Debug.Log("备份不存在");
			break;
		}
	}

	public static string Encrypt(string data)
	{
		char[] array = data.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			char num = array[i];
			char c = keyChars[i % keyChars.Length];
			char c2 = (array[i] = (char)(num ^ c));
		}
		return new string(array);
	}

	public static FinishEndlessGameBuild WorldDataToEndlessBuildData(WorldData worldData)
	{
		FinishEndlessGameBuild finishEndlessGameBuild = new FinishEndlessGameBuild();
		finishEndlessGameBuild.finishGameBuild = WorlddataToBuildData(worldData);
		finishEndlessGameBuild.EndlessLevel = BattleMgr.Inst.EndlessCurrentLevel;
		finishEndlessGameBuild.GetGearCount = BattleMgr.Inst.EndlessCurrentGear;
		BattleMgr.Inst.MaxEndlessLevel = Math.Max(finishEndlessGameBuild.EndlessLevel, BattleMgr.Inst.MaxEndlessLevel);
		worldData.BestEndlessLevel = BattleMgr.Inst.MaxEndlessLevel;
		BattleMgr.Inst.EndlessCurrentGear = 0;
		return finishEndlessGameBuild;
	}

	public static FinishGameBuild WorlddataToBuildData(WorldData worlddata)
	{
		FinishGameBuild finishGameBuild = new FinishGameBuild();
		finishGameBuild.time = (long)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
		if (SteamManager.Initialized)
		{
			finishGameBuild.username = SteamFriends.GetFriendPersonaName(SteamUser.GetSteamID());
		}
		else
		{
			finishGameBuild.username = 1006101.GetText();
		}
		finishGameBuild.Difficulty = (int)worlddata.selectedDifficulty;
		finishGameBuild.timeuse = worlddata.timeuse;
		PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt);
		finishGameBuild.PlayerConfig = playerPpt.unitCfg;
		finishGameBuild.playerLook = worlddata.playerLook;
		finishGameBuild.wandCfgs = PlayerMgr.Inst.BaData.wandCfgs;
		finishGameBuild.coinCount = PlayerMgr.Inst.BaData.coinCount;
		finishGameBuild.keyCount = PlayerMgr.Inst.BaData.keyCount;
		finishGameBuild.bagCount = PlayerMgr.Inst.BaData.bagCount;
		finishGameBuild.relicCfgs = PlayerMgr.Inst.BaData.relicCfgs;
		finishGameBuild.curseIDs = PlayerMgr.Inst.BaData.curseIDs;
		finishGameBuild.curseLevels = PlayerMgr.Inst.BaData.curseLevels;
		finishGameBuild.potionIDs = PlayerMgr.Inst.BaData.potionIDs;
		finishGameBuild.bagSpellDatas = PlayerMgr.Inst.BaData.bagSpellDatas;
		finishGameBuild.selectedSetID = worlddata.selectedSetID;
		finishGameBuild.damageRatio = PlayerMgr.Inst.ExtraDamageRatio;
		finishGameBuild.moveSpeed = PlayerMgr.Inst.PlayerPpt.MoveSpeed;
		return finishGameBuild;
	}
}
