using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAchievementListItem : MonoBehaviour
{
	public Image icon;

	public TextMeshProUGUI achieveName;

	public TextMeshProUGUI achieveDescription;

	public TextMeshProUGUI unlockTime;

	public int achieveIndex;

	public void UpdateInfo()
	{
		if (DataMgr.settingData.mobileAchievement.Keys.Contains((SteamAchievementType)achieveIndex))
		{
			icon.sprite = MobileAchievementMgr.Inst.achieveActiveIcons[achieveIndex];
			unlockTime.text = "解锁时间\n" + DataMgr.settingData.mobileAchievement[(SteamAchievementType)achieveIndex];
		}
		else
		{
			icon.sprite = MobileAchievementMgr.Inst.achieveUnactiveIcons[achieveIndex];
			unlockTime.text = "";
		}
		achieveName.text = MobileAchievementMgr.Inst.achieveName[achieveIndex];
		achieveDescription.text = MobileAchievementMgr.Inst.achieveDescription[achieveIndex];
	}
}
