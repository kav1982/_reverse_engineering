using System;
using Unity.Entities;
using UnityEngine;

public class Boss99Interaction : InteractiveObj
{
	[Space(50f)]
	public GameObject go_Outline;

	public Boss99 boss99;

	public CapsuleCollider cc;

	private Entity thisEntity;

	public override void OnEnable()
	{
		base.OnEnable();
		thisEntity = RegisterDotsInteractiveObj(cc, InteractiveObjType.Boss99Interaction);
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
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(3400, (Action)delegate
		{
			boss99.Transition();
			CloseDotsObj(thisEntity);
		});
	}
}
