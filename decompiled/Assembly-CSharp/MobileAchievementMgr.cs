using System;
using UnityEngine;

public class MobileAchievementMgr : MonoBehaviour
{
	private static MobileAchievementMgr inst;

	public Sprite[] achieveUnactiveIcons;

	public Sprite[] achieveActiveIcons;

	public string[] achieveName;

	[Multiline]
	public string[] achieveDescription;

	public static MobileAchievementMgr Inst
	{
		get
		{
			if (inst == null)
			{
				return null;
			}
			return inst;
		}
	}

	private void Awake()
	{
		if (inst == null)
		{
			inst = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else if (inst != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Debug.Log("Awake AchievementMgr");
	}

	public void UnlockAchievement(SteamAchievementType achievementType)
	{
		if (inst == null)
		{
			inst = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else if (inst != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!DataMgr.settingData.mobileAchievement.ContainsKey(achievementType))
		{
			string value = DateTime.UtcNow.ToString("yyyy年MM月dd日");
			DataMgr.settingData.mobileAchievement.Add(achievementType, value);
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/LoadEnterScene/Camp/UIAchievementPopOut").GetComponent<UIAchievementPopOut>().Init(achieveActiveIcons[(int)achievementType], achieveName[(int)achievementType], achieveDescription[(int)achievementType]);
		}
	}
}
