using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRelic : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public enum UIRelicShowType
	{
		Default,
		Build,
		PlayerStatus,
		LeaderBoard
	}

	public Image image_Icon;

	public Image image_Icon2;

	public Image image_Highlight;

	public Text text_level;

	public GameObject disableRelicShowTip;

	public UIRelicShowType showType;

	public GameObject mobileControllerAppearanceButton;

	private RelicConfig buildShowRelicCfg;

	private int index;

	public RelicConfig RelicCfg
	{
		get
		{
			UIRelicShowType uIRelicShowType = showType;
			if (uIRelicShowType != UIRelicShowType.Build && uIRelicShowType != UIRelicShowType.PlayerStatus && uIRelicShowType != UIRelicShowType.LeaderBoard)
			{
				return PlayerMgr.Inst.BaData.relicCfgs[index];
			}
			return buildShowRelicCfg;
		}
	}

	public void Initialize(int index)
	{
		this.index = index;
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(RelicCfg.GetIconPath());
		image_Icon2.sprite = image_Icon.sprite;
		image_Highlight.sprite = image_Icon.sprite;
		if (RelicCfg.level == 1)
		{
			text_level.text = "";
		}
		else
		{
			text_level.text = RelicCfg.level.ToString();
		}
		disableRelicShowTip.SetActive(DataMgr.settingData.DisableRelicSkins.Contains(RelicCfg.id));
	}

	public void InitializeToBuildShow(int index, FinishGameBuild build, UIRelicShowType _showType)
	{
		buildShowRelicCfg = build.relicCfgs[index];
		this.index = index;
		showType = _showType;
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(buildShowRelicCfg.GetIconPath());
		image_Icon2.sprite = image_Icon.sprite;
		image_Highlight.sprite = image_Icon.sprite;
		if (buildShowRelicCfg.level == 1)
		{
			text_level.text = "";
		}
		else
		{
			text_level.text = buildShowRelicCfg.level.ToString();
		}
		disableRelicShowTip.SetActive(DataMgr.settingData.DisableRelicSkins.Contains(RelicCfg.id));
		if (showType == UIRelicShowType.LeaderBoard)
		{
			disableRelicShowTip.SetActive(value: false);
		}
		else
		{
			disableRelicShowTip.SetActive(DataMgr.settingData.DisableRelicSkins.Contains(RelicCfg.id));
		}
	}

	public void Hover()
	{
		image_Highlight.enabled = true;
	}

	public void Unhover()
	{
		image_Highlight.enabled = false;
	}

	public void Click()
	{
		if (!GameMgr.IsMobile_Static)
		{
			SwitchRelicShow();
			return;
		}
		UIPlayerDataMgr.Inst.uiInfoCurseHover.gameObject.SetActive(value: false);
		UIPlayerDataMgr.Inst.UICurseExit();
		UIRelic uiRelic_Hover = UIPlayerDataMgr.Inst.uiRelic_Hover;
		if (uiRelic_Hover != null)
		{
			UIPlayerDataMgr.Inst.UIRelicExit();
			if (uiRelic_Hover == this)
			{
				return;
			}
		}
		bool flag = showType == UIRelicShowType.PlayerStatus;
		UIInfoRelic panel = UIPlayerDataMgr.Inst.UIRelicEnterBuildShow(this, buildShowRelicCfg, flag);
		if (flag && (DataMgr.settingData.DisableRelicSkins.Contains(RelicCfg.id) || PlayerSkinMgr.IsCanHideRelic(RelicCfg.id)))
		{
			if ((bool)mobileControllerAppearanceButton)
			{
				mobileControllerAppearanceButton.SetActive(MobileMgr.inst.gamepadPlugged);
				mobileControllerAppearanceButton.GetComponent<UpdatButtonShow>().UpdateButton();
			}
			Action refreshText = delegate
			{
				panel.buttonText.text = (DataMgr.settingData.DisableRelicSkins.Contains(RelicCfg.id) ? "点击显示外观" : "点击隐藏外观");
			};
			refreshText();
			panel.MobileShowRelicHideButton(delegate
			{
				SwitchRelicShow();
				refreshText();
			});
		}
	}

	public void SwitchRelicShow()
	{
		UIRelicShowType uIRelicShowType = showType;
		if (uIRelicShowType != UIRelicShowType.Build && uIRelicShowType != UIRelicShowType.LeaderBoard && (DataMgr.settingData.DisableRelicSkins.Contains(RelicCfg.id) || PlayerSkinMgr.IsCanHideRelic(RelicCfg.id)))
		{
			if (DataMgr.settingData.DisableRelicSkins.Contains(RelicCfg.id))
			{
				DataMgr.settingData.DisableRelicSkins.Remove(RelicCfg.id);
			}
			else
			{
				DataMgr.settingData.DisableRelicSkins.Add(RelicCfg.id);
			}
			DataMgr.SaveSettingData();
			OnPointerExit(null);
			OnPointerEnter(null);
			PlayerMgr.Inst.UpdateSkin();
			disableRelicShowTip.SetActive(DataMgr.settingData.DisableRelicSkins.Contains(RelicCfg.id));
			if ((bool)PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed)
			{
				PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed.UpdateDisplay();
			}
			if (showType == UIRelicShowType.PlayerStatus)
			{
				UIMgr.Inst.UIMenu.finishBuildShow.UpdateSkeletonGraphic();
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (GameMgr.IsMobile_Static)
		{
			if (ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				Click();
			}
		}
		else if (showType == UIRelicShowType.PlayerStatus)
		{
			UIPlayerDataMgr.Inst.UIRelicEnterBuildShow(this, buildShowRelicCfg, showSkinText: true);
		}
		else if (showType == UIRelicShowType.Build || showType == UIRelicShowType.LeaderBoard)
		{
			UIPlayerDataMgr.Inst.UIRelicEnterBuildShow(this, buildShowRelicCfg, showSkinText: false);
		}
		else
		{
			UIPlayerDataMgr.Inst.UIRelicEnter(this);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!GameMgr.IsMobile_Static || ControlMgr.Inst.InputType != 0)
		{
			UIPlayerDataMgr.Inst.UIRelicExit(this);
		}
	}
}
