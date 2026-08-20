using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingData
{
	public enum WindowsMode
	{
		BoardlessWindows,
		Windows,
		FullScreen
	}

	public enum FrameLimit
	{
		limit30,
		limit60,
		NoLimit
	}

	public bool firstCreate = true;

	public float music = 1f;

	public float sound = 1f;

	public float mainvolume = 1f;

	public bool SafeMode;

	public bool BattleUIControl;

	public bool Vsync;

	public bool ControllerAimAssist = true;

	public bool hardwareCursor = true;

	public float CursorSize = 1f;

	public bool fullScreen = true;

	public WindowsMode windowsMode;

	public bool textFloat = true;

	public bool AiSummon;

	public bool isTouristMode;

	public float screenShockRatio = 1f;

	public float SpellTransparent = 1f;

	public float SummonTransparent = 1f;

	public int VirtualStickType;

	public LanguageType language;

	public ResolutionTypeSteamDeck resTypeSteamDeck2;

	public ResolutionTypeMobile resTypeMobile = ResolutionTypeMobile.Res60;

	public MobileTargetFrameRate MobileTargetFrameRate = MobileTargetFrameRate.Target60;

	public ResolutionType resType = ResolutionType.Res1920;

	public FrameLimit frameLimit = FrameLimit.limit60;

	public ControlData controldata = new ControlData();

	public List<int> DisableRelicSkins = new List<int>();

	public MobileData Mobiledata = new MobileData();

	public Dictionary<SteamAchievementType, string> mobileAchievement = new Dictionary<SteamAchievementType, string>();

	public float FinalSpellTransparent => Mathf.Pow(SpellTransparent, 2f);

	public float FinalSummonTransparent => Mathf.Pow(SummonTransparent, 2f);

	public void GeneralReset()
	{
		music = 1f;
		sound = 1f;
		fullScreen = true;
		windowsMode = WindowsMode.FullScreen;
		textFloat = true;
		screenShockRatio = 1f;
		if (GameMgr.IsMobile_Static)
		{
			resTypeMobile = ResolutionTypeMobile.Res60;
			MobileTargetFrameRate = MobileTargetFrameRate.Target60;
		}
		else if (GameMgr.IsSteamDeck_Static)
		{
			resTypeSteamDeck2 = ResolutionTypeSteamDeck.Res1;
		}
		else
		{
			resType = ResolutionType.Res1920;
		}
	}

	public Vector2Int GetResolution()
	{
		if (GameMgr.IsMobile_Static)
		{
			return GetResolutionCurrentScreen((int)resTypeMobile);
		}
		if (GameMgr.IsSteamDeck_Static)
		{
			return GetResolutionCurrentScreen((int)resTypeSteamDeck2);
		}
		return GetResolutionCurrentScreen((int)resType);
	}

	public static Vector2Int GetResolutionCurrentScreen(int level)
	{
		if (GameMgr.IsMobile_Static)
		{
			return level switch
			{
				0 => GetMobileRes(0.4f), 
				1 => GetMobileRes(0.5f), 
				2 => GetMobileRes(0.7f), 
				_ => GetMobileRes(1f), 
			};
		}
		if (GameMgr.IsSteamDeck_Static)
		{
			return level switch
			{
				0 => GetMobileRes(1f), 
				1 => new Vector2Int(1200, 720), 
				_ => GetMobileRes(1f), 
			};
		}
		if (DataMgr.settingData.windowsMode == WindowsMode.FullScreen)
		{
			float num = (float)Display.displays[0].systemWidth / (float)Display.displays[0].systemHeight;
			switch (level)
			{
			case 0:
				return new Vector2Int(640, Mathf.RoundToInt(640f / num));
			case 1:
				return new Vector2Int(960, Mathf.RoundToInt(960f / num));
			case 2:
				return new Vector2Int(1280, Mathf.RoundToInt(1280f / num));
			case 3:
				return new Vector2Int(1600, Mathf.RoundToInt(1600f / num));
			case 4:
				return new Vector2Int(1920, Mathf.RoundToInt(1920f / num));
			case 5:
				return new Vector2Int(2560, Mathf.RoundToInt(2560f / num));
			case 6:
				return new Vector2Int(3840, Mathf.RoundToInt(3840f / num));
			default:
				Debug.LogError(level);
				return new Vector2Int(1920, Mathf.RoundToInt(1920f / num));
			}
		}
		switch (level)
		{
		case 0:
			return new Vector2Int(640, 360);
		case 1:
			return new Vector2Int(960, 540);
		case 2:
			return new Vector2Int(1280, 720);
		case 3:
			return new Vector2Int(1600, 900);
		case 4:
			return new Vector2Int(1920, 1080);
		case 5:
			return new Vector2Int(2560, 1440);
		case 6:
			return new Vector2Int(3840, 2160);
		default:
			Debug.LogError(level);
			return new Vector2Int(1920, 1080);
		}
	}

	public static Vector2Int GetMobileRes(float x)
	{
		return new Vector2Int((int)(MobileMgr.inst.ScreenRes.x * x), (int)(MobileMgr.inst.ScreenRes.y * x));
	}

	public int GetNextLanguageIndex()
	{
		int num = (int)(language + 1);
		if (num > Enum.GetNames(typeof(LanguageType)).Length - 1)
		{
			num = 0;
		}
		return num;
	}

	public int GetPreviousLanguageIndex()
	{
		int num = (int)(language - 1);
		if (num < 0)
		{
			num = Enum.GetNames(typeof(LanguageType)).Length - 1;
		}
		return num;
	}

	public float GetFinalMusic()
	{
		return music * mainvolume;
	}

	public float GetFinalSound()
	{
		return sound * mainvolume;
	}
}
