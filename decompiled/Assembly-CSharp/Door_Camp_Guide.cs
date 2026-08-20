using UnityEngine;

public class Door_Camp_Guide : InteractiveObj
{
	[Space(50f)]
	public GameObject go_Outline;

	public GameObject go_Portal;

	public GameObject go_Mask;

	public void Show()
	{
		base.tag = "InteractiveObj";
		go_Portal.SetActive(value: true);
		go_Mask.SetActive(value: false);
		SEMgr.Inst.puzzleSucceed.PlaySE();
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
		GuideMgr.Inst.EnterDoor();
	}
}
