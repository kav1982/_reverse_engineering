using System;
using UnityEngine;

public struct BLiveGiftMessage
{
	public string User;

	public BLiveGiftType Type;

	public int Id;

	public Action<RectTransform> OnAction;

	public string FormatedUserName
	{
		get
		{
			if (User.Length < 16)
			{
				return User;
			}
			return User.Substring(0, 15) + "...";
		}
	}

	public string FormatedMessage
	{
		get
		{
			switch (Type)
			{
			case BLiveGiftType.AddRelic:
			{
				RelicConfig relicConfig = RelicConfig.dic[Id];
				string name3 = relicConfig.GetName();
				string text = ((relicConfig.dropType == ItemDropType.Epic) ? "#ffaa33" : "#8888ff");
				return "<color=#8888ff>" + FormatedUserName + "</color> 送来 <color=" + text + ">遗物：" + name3 + "</color>";
			}
			case BLiveGiftType.AddCurse:
			{
				string name2 = CurseConfig.dic[Id].GetName();
				return "<color=#8888ff>" + FormatedUserName + "</color> 送来 <color=#ff44ff>诅咒：" + name2 + "</color>";
			}
			case BLiveGiftType.RemoveCurse:
				if (Id >= 0)
				{
					string name = CurseConfig.dic[Id].GetName();
					return "<color=#8888ff>" + FormatedUserName + "</color> <color=#44ff44>清除</color> 了诅咒：" + name;
				}
				return "<color=#8888ff>" + FormatedUserName + "</color> 尝试清除诅咒";
			default:
				Debug.LogError("??");
				return "<color=#8888ff>" + FormatedUserName + "</color> 送了个啥";
			}
		}
	}
}
