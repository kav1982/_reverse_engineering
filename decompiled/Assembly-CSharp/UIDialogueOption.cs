using System;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueOption : MonoBehaviour
{
	public Text optionText;

	public int id;

	public int siblingIndex;

	public int parentID;

	public Action actionInherit;

	public bool returnSibling;

	public bool canForceStop;

	public Image background;

	public Image imageGamepadSelected;

	public void OnClick()
	{
		UIDialogueMgr.EndDialoguePart();
		GameUISingletonMono<UIDialogueMgr>.Inst.HideOptions();
		GameUISingletonMono<UIDialogueMgr>.Inst.conversationRecord[parentID][siblingIndex] = true;
		if (base.transform.parent.childCount == 1 && returnSibling)
		{
			Debug.LogWarning("最后一个回答了还不是结束对话?强制认为是结束对话");
			GameUISingletonMono<UIDialogueMgr>.Inst.backToOptions = false;
		}
		else
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.backToOptions = returnSibling;
		}
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(id, actionInherit);
		background.color = GameUISingletonMono<UIDialogueMgr>.Inst.colorChoosen;
		optionText.color = GameUISingletonMono<UIDialogueMgr>.Inst.textColorChoosen;
	}

	public void GamepadSelected()
	{
		imageGamepadSelected.gameObject.SetActive(value: true);
	}

	public void GamepadUnselected()
	{
		imageGamepadSelected.gameObject.SetActive(value: false);
	}
}
