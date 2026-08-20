using UnityEngine;

public class SpecialObj101Reroll : InteractiveObj
{
	[Space(50f)]
	public GameObject go_HighLight;

	public Transform tsf_Carpet;

	public Animator anima;

	public AnimaEvent animaEvent;

	public int fixedUsage;

	public float[] brokenChance;

	public Vector3 brokenEFCenter;

	public Vector2 brokenEFOffset;

	public int useTimer;

	private void Start()
	{
		fixedUsage += DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.ProcessReroll);
		tsf_Carpet.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile9_AboveAO);
		animaEvent.DoAction = AnimaAction;
	}

	private void AnimaAction(string actionName)
	{
		if (!(actionName == "EF"))
		{
			if (actionName == "Finish")
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_SO101RerollBroken", base.transform.position, 2f);
				SEMgr.Inst.so101_RerollBroken.PlaySE();
			}
			else
			{
				Debug.LogError(actionName);
			}
		}
		else
		{
			Vector3 point = base.transform.position + brokenEFCenter + new Vector3(Random.Range(0f - brokenEFOffset.x, brokenEFOffset.x), Random.Range(0f - brokenEFOffset.y, brokenEFOffset.y), 0f);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Smoke", point, 2f);
			SEMgr.Inst.so101_RerollEF.PlaySE();
		}
	}

	public bool UseOnce()
	{
		if (ScriptableObjMgr.Inst.testCtrller.BattleInfiniteReroll)
		{
			useTimer--;
		}
		useTimer++;
		if (useTimer <= fixedUsage)
		{
			if (useTimer < fixedUsage)
			{
				anima.SetTrigger("Reroll");
			}
			else
			{
				anima.SetTrigger("RerollBeforeBroken");
			}
			return false;
		}
		int num = useTimer - fixedUsage;
		if (Random.value <= brokenChance[num - 1])
		{
			anima.SetTrigger("Broken");
			base.tag = "Untagged";
			return true;
		}
		anima.SetTrigger("RerollBeforeBroken");
		return false;
	}

	public string GetName()
	{
		string text = 1001305.GetText();
		for (int i = 0; i < DataMgr.selectedWorldData.researchedIDs.Count; i++)
		{
			if (ResearchConfig.dic[DataMgr.selectedWorldData.researchedIDs[i]].abilityType == ResearchAbilityType.ProcessReroll)
			{
				text += "+";
			}
		}
		return text;
	}

	public override void Select()
	{
		go_HighLight.SetActive(value: true);
	}

	public override void Unselect()
	{
		go_HighLight.SetActive(value: false);
	}

	public override void Interact()
	{
		GameUISingletonMono<UIReroll>.ShowInit(this);
	}
}
