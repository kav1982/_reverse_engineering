using UnityEngine;

public class SpecialObj21 : InteractiveObj
{
	[Space(50f)]
	public GameObject go_HighLight;

	public Animator anima;

	public AnimaEvent animaEvent;

	public int fixedUsage;

	public float[] brokenChance;

	public Vector3 brokenEFCenter;

	public Vector2 brokenEFOffset;

	public int useTimer;

	private void Start()
	{
		animaEvent.DoAction = AnimaAction;
		if (DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Recycler) == 0)
		{
			Object.Destroy(base.gameObject);
		}
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
		useTimer++;
		if (useTimer <= fixedUsage)
		{
			if (useTimer < fixedUsage)
			{
				anima.Play("Sell", 0, 0f);
			}
			else
			{
				anima.Play("BeforeBroken", 0, 0f);
			}
			return false;
		}
		int num = useTimer - fixedUsage;
		if (Random.value <= brokenChance[num - 1])
		{
			anima.Play("Broken", 0, 0f);
			base.tag = "Untagged";
			return true;
		}
		anima.Play("BeforeBroken", 0, 0f);
		return false;
	}

	public string GetName()
	{
		return 1001320.GetText();
	}

	public string GetDesc()
	{
		return 1001321.GetText();
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
		if ((bool)UIBattleMgr.Inst)
		{
			GameUISingletonMono<UISell>.ShowInit(this);
		}
	}
}
