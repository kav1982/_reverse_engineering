using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using IFix.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInit : MonoBehaviour
{
	public UICompanyLogoShow UICompanyLogoShow;

	private static Stopwatch _stopwatch;

	private IEnumerator Start()
	{
		Application.runInBackground = true;
		InitBugly();
		_stopwatch = Stopwatch.StartNew();
		Pin("Start Game Init");
		if (GameMgr.IsMobile_Static)
		{
			GameMgr.IsAndroidSimulator = PluginActivity.Inst.GetBuildID().Contains("V417IR");
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			ABResources.Init();
			yield return HotFixInit();
		}
		if (GameMgr.IsMobile_Static && !File.Exists(DataMgr.dataPath + DataMgr.settingDataFileName))
		{
			QualityPreset.SetQuality();
		}
		while (QualityPreset.cpuTesting)
		{
			yield return new WaitForEndOfFrame();
		}
		yield return DataMgrInit();
		Pin("Switch Scene To Entry...");
		if (GameMgr.IsMobile_Static)
		{
			UICompanyLogoShow.sliderSpeed = 0.8f;
			while (UICompanyLogoShow.mobileLoadSlider.value < 1f)
			{
				yield return new WaitForEndOfFrame();
			}
		}
		while (!UICompanyLogoShow.finishSplashScreen)
		{
			yield return new WaitForEndOfFrame();
		}
		SceneManager.LoadScene("Entry");
	}

	private static void InitBugly()
	{
		BuglyAgent.EnableExceptionHandler();
	}

	private void Update()
	{
		if (Time.unscaledDeltaTime > 0.5f)
		{
			Pin("卡顿帧：" + Time.unscaledDeltaTime);
		}
	}

	private static IEnumerator HotFixInit()
	{
		Pin("Start load HotFixBundle...");
		yield return HotFixBundle.InitAsync();
		Pin("HotFixBundle load finish");
		Pin("Start load CS Patch...");
		foreach (HotFixBundle item in HotFixBundle.Loaded.OrderBy((HotFixBundle e) => -e.Version))
		{
			string codePatchFilePath = item.GetCodePatchFilePath();
			if (!string.IsNullOrEmpty(codePatchFilePath))
			{
				PatchManager.Load(codePatchFilePath);
				break;
			}
		}
		Pin("CS Patch load finish");
	}

	private static IEnumerator DataMgrInit()
	{
		Pin("Start datamgr init...");
		DataMgr.Initialize();
		while (!DataMgr.InitializeIsDone())
		{
			yield return null;
		}
		foreach (WandConfig item in WandConfig.list)
		{
			item.OnLoadedAfter();
		}
	}

	public static void Pin(string message)
	{
		UnityEngine.Debug.Log(string.Format("[INIT {0,10:F4}] {1}", (float)_stopwatch.ElapsedMilliseconds / 1000f, message));
	}
}
