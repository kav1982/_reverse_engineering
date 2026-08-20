using System.Collections.Generic;
using UnityEngine;

public class GameConstManaged
{
	public static readonly List<int> SpecialLongWandIdList = new List<int> { 101, 91, 2013, 2033, 6001, 6009 };

	public static readonly List<float> endlessStageMonsterDamageRatio = new List<float> { 1f, 1.5f, 2f, 2.5f, 3f, 4f };

	public static readonly List<EndlessBattleSpawnInfo.EndlessUnitType> endlessGroupSummonType = new List<EndlessBattleSpawnInfo.EndlessUnitType>
	{
		EndlessBattleSpawnInfo.EndlessUnitType.M_Walker,
		EndlessBattleSpawnInfo.EndlessUnitType.M_Charger,
		EndlessBattleSpawnInfo.EndlessUnitType.M_Fly,
		EndlessBattleSpawnInfo.EndlessUnitType.M_SpeedWalker,
		EndlessBattleSpawnInfo.EndlessUnitType.M_Jumper,
		EndlessBattleSpawnInfo.EndlessUnitType.M_Tp1,
		EndlessBattleSpawnInfo.EndlessUnitType.M_FireBomb,
		EndlessBattleSpawnInfo.EndlessUnitType.M_Repair,
		EndlessBattleSpawnInfo.EndlessUnitType.M_Helicopter
	};

	public static readonly List<int> SpecialRelicList = new List<int> { 961, 962, 963, 964, 965, 966 };

	public static readonly List<int> LostCastleRuneLevelThreshold = new List<int> { 5, 15, 25, 35 };

	public static readonly List<int> LostCastleRuneID = new List<int> { 40251, 40261, 40271 };

	public static readonly Dictionary<LanguageType, string> LanguageStrings = new Dictionary<LanguageType, string>
	{
		{
			LanguageType.ChineseS,
			"zh-CN"
		},
		{
			LanguageType.ChineseT,
			"zh-TW"
		},
		{
			LanguageType.English,
			"en-US"
		},
		{
			LanguageType.Japanese,
			"ja-JP"
		},
		{
			LanguageType.German,
			"de-DE"
		},
		{
			LanguageType.Korean,
			"ko-KR"
		},
		{
			LanguageType.Russian,
			"ru-RU"
		},
		{
			LanguageType.Spanish_spain,
			"es-ES"
		},
		{
			LanguageType.French,
			"fr-FR"
		},
		{
			LanguageType.Portuguese_brazil,
			"pt-BR"
		},
		{
			LanguageType.Swedish,
			"sv-SE"
		},
		{
			LanguageType.Polish,
			"pl-PL"
		},
		{
			LanguageType.Turkish,
			"tr-TR"
		},
		{
			LanguageType.Italian,
			"it-IT"
		},
		{
			LanguageType.Czech,
			"cs-CZ"
		}
	};

	public static int baseMapIndex = Shader.PropertyToID("_BaseMap");

	public static int shaderTextureIndex = Shader.PropertyToID("_MainTex");

	public static int shaderBaseMapIndex = Shader.PropertyToID("_BaseMap");

	public static int shaderPPUIndex = Shader.PropertyToID("_PixelPerUnit");

	public static int shaderCenterIndex = Shader.PropertyToID("_Center");

	public static int shaderBlendIndex = Shader.PropertyToID("_Blend");

	public static int shaderTransparencyIndex = Shader.PropertyToID("_Transparency");

	public static int shaderFlipXIndex = Shader.PropertyToID("_FlipX");

	public static int shaderColorIndex = Shader.PropertyToID("_Color");

	public static int shaderSpriteColorIndex = Shader.PropertyToID("_SpriteColor");

	public static int shaderBaseColorIndex = Shader.PropertyToID("_BaseColor");

	public static int shaderGroundHeightIndex = Shader.PropertyToID("_GroundHeight");

	public static Shader unitReboundShader = Shader.Find("Shader Graphs/SG_Unit_Lit_Dots");

	public static float endlessMonsterDamageRatio
	{
		get
		{
			int num = Mathf.Clamp(BattleMgr.Inst.EndlessCurrentStage, 1, 6);
			return 1f * endlessStageMonsterDamageRatio[num - 1];
		}
	}

	public static float EndlessBossDamageRatio => 1f;

	public static string bgm_Boss
	{
		get
		{
			if (DataMgr.selectedWorldData.InDaveRoom)
			{
				return "BGM_Boss_Dave";
			}
			if (GameMgr.InEndlessMode)
			{
				return "BGM_EndlessBoss";
			}
			return "BGM_Boss";
		}
	}

	public static float GetEndlessHpRatio(int currentLevel)
	{
		return currentLevel switch
		{
			1 => 1f, 
			2 => 1.3f * GetEndlessHpRatio(currentLevel - 1), 
			3 => 1.31f * GetEndlessHpRatio(currentLevel - 1), 
			4 => 1.32f * GetEndlessHpRatio(currentLevel - 1), 
			5 => 1.33f * GetEndlessHpRatio(currentLevel - 1), 
			6 => 1.34f * GetEndlessHpRatio(currentLevel - 1), 
			7 => 1.35f * GetEndlessHpRatio(currentLevel - 1), 
			8 => 1.36f * GetEndlessHpRatio(currentLevel - 1), 
			9 => 1.37f * GetEndlessHpRatio(currentLevel - 1), 
			10 => 1.38f * GetEndlessHpRatio(currentLevel - 1), 
			11 => 1.39f * GetEndlessHpRatio(currentLevel - 1), 
			_ => 1.4f * GetEndlessHpRatio(currentLevel - 1), 
		};
	}
}
