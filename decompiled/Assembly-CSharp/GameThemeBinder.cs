using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameThemeBinder : MonoBehaviour
{
	[Serializable]
	public class GameThemeSettingGroup
	{
		public string Des;

		public List<GameThemeSetting> GameThemeSettings = new List<GameThemeSetting>();

		public void UseTheme()
		{
			GameThemeSettings.ForEach(delegate(GameThemeSetting x)
			{
				if ((bool)x.gameobject)
				{
					x.gameobject.SetActive(x.appearanceTheme == ScriptableObjMgr.Inst.testCtrller.campSkinType);
				}
				if ((bool)x.spriteRenderer && x.appearanceTheme == ScriptableObjMgr.Inst.testCtrller.campSkinType)
				{
					x.spriteRenderer.sprite = x.sprite;
				}
			});
		}
	}

	[Serializable]
	public class GameThemeSetting
	{
		[FormerlySerializedAs("theme")]
		public CampSkinType appearanceTheme;

		public GameObject gameobject;

		public SpriteRenderer spriteRenderer;

		public Sprite sprite;
	}

	public List<GameThemeSettingGroup> GameThemeSettingGroups;

	private void Awake()
	{
		foreach (GameThemeSettingGroup gameThemeSettingGroup in GameThemeSettingGroups)
		{
			gameThemeSettingGroup.UseTheme();
		}
	}
}
