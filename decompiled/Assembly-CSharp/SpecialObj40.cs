using UnityEngine;

public class SpecialObj40 : InteractiveObj
{
	public Animator animator;

	public GameObject go_Outline;

	public int tipsCountPC;

	public int tipsCountMobile;

	private int currentTipsID = -1;

	private float _interval;

	public int tipsTotall
	{
		get
		{
			if (GameMgr.IsMobile_Static)
			{
				return tipsCountMobile;
			}
			return tipsCountPC;
		}
	}

	public override void Interact()
	{
		animator.Play("interact", 0, 0f);
		ShowRandomTips();
	}

	private void ShowRandomTips()
	{
		int num = 0;
		int num2 = 0;
		do
		{
			num2++;
			if (num2 >= 100)
			{
				Debug.LogError("SpecialObj40死循环");
				break;
			}
			num = ((!GameMgr.IsMobile_Static) ? (Random.Range(0, tipsTotall) + 1003801) : (Random.Range(0, tipsTotall) + 1003901));
		}
		while (num == currentTipsID);
		currentTipsID = num;
		GameUISingletonMono<UIDialogueMgr>.Inst.MDShow(currentTipsID, base.transform);
	}
}
