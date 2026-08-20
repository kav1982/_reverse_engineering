using System;
using UnityEngine;

[Serializable]
public class SettingSlot
{
	public Category category = Category.Other;

	public SettingSlotType SettingSlotType;

	public bool activeMobile;

	public bool activePCKey;

	public bool activePCController;

	public GameObject objRoot;

	public UISettingPointin UISettingPointin;

	public UISettingMobileToggle UISettingMobileToggle;
}
