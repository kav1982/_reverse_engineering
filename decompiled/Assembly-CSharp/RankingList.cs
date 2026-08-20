using UnityEngine;

public class RankingList : InteractiveObj
{
	[Space(50f)]
	public GameObject go_Outline;

	public void Start()
	{
		if (ScriptableObjMgr.Inst.testCtrller.publishTesting || ScriptableObjMgr.Inst.testCtrller.DisableSteam)
		{
			base.gameObject.SetActive(value: false);
		}
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
		if (!GameMgr.IsHarmony_Static)
		{
			GameUISingletonMono<UI_RankingList>.ShowInit();
		}
	}
}
