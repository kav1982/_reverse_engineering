using System;
using Unity.Entities;
using UnityEngine;

public class Boss99InteractionBuyGame : InteractiveObj
{
	[Space(50f)]
	public GameObject go_Outline;

	public CapsuleCollider cc;

	private Entity thisEntity;

	public override void OnEnable()
	{
		base.OnEnable();
		thisEntity = RegisterDotsInteractiveObj(cc, InteractiveObjType.Boss99InteractionBuyGame);
	}

	public override void Select()
	{
		go_Outline.SetActive(value: true);
	}

	public override void Unselect()
	{
		go_Outline.SetActive(value: false);
	}

	public override void Interact()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(3501, (Action)delegate
		{
			GameUISingletonMono<UIFullGame>.ShowInit();
		});
	}
}
