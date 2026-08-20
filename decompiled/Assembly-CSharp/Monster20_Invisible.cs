using System.Collections.Generic;
using UnityEngine;

public class Monster20_Invisible : UnitBase
{
	public bool canSplit;

	public bool isSplit;

	public List<UnitProperty> bodyPpts = new List<UnitProperty>();

	public List<Monster20_Invisible> allSplits = new List<Monster20_Invisible>();

	public Monster20_Invisible master;

	public override void EveryInitialCallback()
	{
		isSplit = false;
		master = null;
		myPpt.CanTouch = false;
		bodyPpts.Clear();
		allSplits.Clear();
	}

	public override void Update()
	{
		if (bodyPpts.Count > 0)
		{
			base.transform.position = bodyPpts[0].transform.position;
		}
		else if (allSplits.Count > 0)
		{
			base.transform.position = allSplits[0].transform.position;
		}
		SyncDotsPosition();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (bodyPpts.Count > 0)
		{
			for (int num = bodyPpts.Count - 1; num >= 0; num--)
			{
				bodyPpts[num].UnitBas.DotsAnnouncedDeath();
			}
		}
		if (allSplits.Count > 0)
		{
			for (int num2 = allSplits.Count - 1; num2 >= 0; num2--)
			{
				allSplits[num2].DotsAnnouncedDeath();
			}
		}
		bodyPpts.Clear();
		allSplits.Clear();
	}

	public void BodyRegister(UnitProperty bodyPpt)
	{
		bodyPpts.Add(bodyPpt);
	}

	public void SplitUnregister(Monster20_Invisible deadSplit, ref TakeDamageInfo_Dots info)
	{
		if (isSplit)
		{
			Debug.LogError("蜈蚣假身不应该使用这个方法");
			return;
		}
		allSplits.Remove(deadSplit);
		if (bodyPpts.Count == 0 && allSplits.Count == 0 && !myPpt.AlreadyDead)
		{
			DotsAnnouncedDeath();
			info.isTriggerDeadEvent = true;
		}
	}

	public void SplitInitialize(Monster20_Invisible master, List<UnitProperty> splitBodyPpts)
	{
		isSplit = true;
		this.master = master;
		bodyPpts = splitBodyPpts.Copy();
		for (int i = 0; i < bodyPpts.Count; i++)
		{
			bodyPpts[i].GetComponent<Monster20>().invisiblePpt = this;
			if (i == 0)
			{
				bodyPpts[i].GetComponent<Monster20>().SplitHeadReset();
			}
		}
	}

	public void BodyUnregister(UnitProperty bodyPpt, ref TakeDamageInfo_Dots info)
	{
		if (!canSplit)
		{
			bodyPpts.Remove(bodyPpt);
		}
		else
		{
			int num = bodyPpts.IndexOf(bodyPpt);
			bodyPpts.Remove(bodyPpt);
			if (bodyPpts.Count > 0 && num <= bodyPpts.Count && num > 0)
			{
				Monster20_Invisible component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/102095", base.transform.position).GetComponent<Monster20_Invisible>();
				if (isSplit)
				{
					component.SplitInitialize(master, bodyPpts.GetRange(num, bodyPpts.Count - num));
					master.allSplits.Add(component);
				}
				else
				{
					component.SplitInitialize(this, bodyPpts.GetRange(num, bodyPpts.Count - num));
					allSplits.Add(component);
				}
				for (int num2 = bodyPpts.Count - 1; num2 >= num; num2--)
				{
					bodyPpts.RemoveAt(num2);
				}
				return;
			}
		}
		if (bodyPpts.Count != 0 || allSplits.Count != 0 || myPpt.AlreadyDead)
		{
			return;
		}
		DotsAnnouncedDeath();
		if (isSplit)
		{
			if (master != null)
			{
				master.SplitUnregister(this, ref info);
			}
		}
		else
		{
			info.isTriggerDeadEvent = true;
		}
	}
}
