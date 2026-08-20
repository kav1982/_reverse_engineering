using System.Collections.Generic;
using UnityEngine;

public class Monster32_Invisible : UnitBase
{
	public VariableFloat attackInterval;

	public float attackBodyInterval;

	public int minBodyCount;

	public AIPattern pattern;

	private float attackIntervalTimer;

	[Header("困难变异加速")]
	public float speedBuffRadio;

	public float speedBuffTime;

	public VariableFloat speedBuffCD;

	private float speedBuffTimer;

	private float speedBuffCDTimer;

	[Header("加速速度线表现")]
	public List<Transform> speedLines;

	public int speedLinePerBody;

	public bool useLargeSpeedLine;

	[Header("噩梦变异分裂")]
	public List<Monster32> monster32s = new List<Monster32>();

	public List<Monster32_Invisible> allSplits = new List<Monster32_Invisible>();

	public Monster32_Invisible master;

	public bool canSplit;

	public bool isSplit;

	public bool bodylessKilled;

	private float nowSpeedBuff => Mathf.Lerp(1f, speedBuffRadio, speedBuffTimer / 1f);

	public override void EveryInitialCallback()
	{
		myPpt.CanTouch = false;
		isSplit = false;
		master = null;
		myPpt.CanTouch = false;
		allSplits.Clear();
		monster32s.Clear();
		attackIntervalTimer = 0f;
		attackInterval.RandomResult();
		speedBuffCD.RandomResult();
		speedBuffCDTimer = Random.Range(0f, speedBuffCD.result);
		speedBuffTimer = 0f;
		bodylessKilled = false;
	}

	public override void Frame1InitialCallback()
	{
		if (monster32s.Count > 1)
		{
			monster32s[monster32s.Count - 1].ChangeToTail();
		}
	}

	public override void Update()
	{
		if (pattern > AIPattern.Pattern2)
		{
			if (speedBuffTimer < 0f)
			{
				speedBuffCDTimer += Time.deltaTime;
			}
			else
			{
				speedBuffTimer -= Time.deltaTime;
			}
			if (speedBuffCDTimer > speedBuffCD.result)
			{
				if (monster32s.Count > 0)
				{
					SEMgr.Inst.monster32SpeedUp.PlaySE();
					speedLines.Clear();
					int num = Mathf.Max(0, monster32s.Count - 2) / speedLinePerBody + 1;
					for (int i = 0; i < num; i++)
					{
						if (useLargeSpeedLine)
						{
							speedLines.Add(ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster32_SpeedLineLarge", monster32s[speedLinePerBody * i].transform.position, 4f).transform);
						}
						else
						{
							speedLines.Add(ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster32_SpeedLine", monster32s[speedLinePerBody * i].transform.position, 4f).transform);
						}
					}
				}
				speedBuffCD.RandomResult();
				speedBuffCDTimer = 0f;
				speedBuffTimer = speedBuffTime;
			}
			for (int j = 0; j < monster32s.Count; j++)
			{
				if (speedBuffTimer > 0f && monster32s[j].Front == null)
				{
					monster32s[j].borderRenderer.enabled = true;
				}
				else
				{
					monster32s[j].borderRenderer.enabled = false;
				}
			}
			for (int k = 0; k < speedLines.Count; k++)
			{
				if (speedLinePerBody * k < monster32s.Count && !monster32s[speedLinePerBody * k].myPpt.AlreadyDead)
				{
					speedLines[k].transform.position = monster32s[speedLinePerBody * k].transform.position;
				}
			}
		}
		base.Update();
		if (monster32s.Count > 0)
		{
			monster32s[0].nowSpeedBuff = nowSpeedBuff;
			base.transform.position = monster32s[0].transform.position;
		}
		else if (allSplits.Count > 0)
		{
			base.transform.position = allSplits[0].transform.position;
		}
		attackIntervalTimer += Time.deltaTime;
		if (attackIntervalTimer >= attackInterval.result)
		{
			attackIntervalTimer = 0f;
			attackInterval.RandomResult();
			for (int l = 0; l < monster32s.Count; l++)
			{
				monster32s[l].Attack((float)l * attackBodyInterval);
			}
		}
	}

	public void SplitUnregister(Monster32_Invisible deadSplit, ref TakeDamageInfo_Dots info)
	{
		if (isSplit)
		{
			Debug.LogError("蜈蚣假身不应该使用这个方法");
			return;
		}
		allSplits.Remove(deadSplit);
		if (monster32s.Count == 0 && allSplits.Count == 0 && !myPpt.AlreadyDead)
		{
			DotsAnnouncedDeath();
			info.isTriggerDeadEvent = true;
		}
	}

	public void BodyRegister(Monster32 monster32)
	{
		monster32s.Add(monster32);
	}

	public void SplitInitialize(Monster32_Invisible master, List<Monster32> newMmonster32s)
	{
		isSplit = true;
		this.master = master;
		attackIntervalTimer = this.master.attackIntervalTimer;
		speedBuffCDTimer = this.master.speedBuffCDTimer;
		speedBuffTimer = this.master.speedBuffTimer;
		useLargeSpeedLine = this.master.useLargeSpeedLine;
		monster32s = newMmonster32s.Copy();
		for (int i = 0; i < monster32s.Count; i++)
		{
			monster32s[i].GetComponent<Monster32>().invisiblePpt = this;
			if (i == 0)
			{
				monster32s[i].GetComponent<Monster32>().SplitHeadReset();
			}
		}
	}

	public void BodyUnregister(Monster32 monster32, ref TakeDamageInfo_Dots info)
	{
		if (bodylessKilled)
		{
			return;
		}
		if (!canSplit)
		{
			monster32s.Remove(monster32);
		}
		if (monster32s.Count < minBodyCount)
		{
			bodylessKilled = true;
			for (int num = monster32s.Count - 1; num >= 0; num--)
			{
				monster32s[num].DotsAnnouncedDeath();
				monster32s.RemoveAt(num);
			}
		}
		else if (canSplit)
		{
			int num2 = monster32s.IndexOf(monster32);
			monster32s.Remove(monster32);
			if (monster32s.Count < minBodyCount)
			{
				bodylessKilled = true;
				for (int num3 = monster32s.Count - 1; num3 >= 0; num3--)
				{
					monster32s[num3].DotsAnnouncedDeath();
					monster32s.RemoveAt(num3);
				}
			}
			else if (monster32s.Count > minBodyCount * 2 + 1 && num2 < monster32s.Count - minBodyCount + 1 && num2 > minBodyCount - 1)
			{
				Monster32_Invisible component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/103295", base.transform.position).GetComponent<Monster32_Invisible>();
				if (isSplit)
				{
					component.SplitInitialize(master, monster32s.GetRange(num2, monster32s.Count - num2));
					master.allSplits.Add(component);
				}
				else
				{
					component.SplitInitialize(this, monster32s.GetRange(num2, monster32s.Count - num2));
					allSplits.Add(component);
				}
				for (int num4 = monster32s.Count - 1; num4 >= num2; num4--)
				{
					monster32s.RemoveAt(num4);
				}
				if (monster32s.Count > 1)
				{
					monster32s[monster32s.Count - 1].ChangeToTail();
				}
				return;
			}
		}
		if (monster32s.Count > 1)
		{
			monster32s[monster32s.Count - 1].ChangeToTail();
		}
		if (monster32s.Count != 0 || allSplits.Count != 0 || myPpt.AlreadyDead)
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

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		for (int num = monster32s.Count - 1; num >= 0; num--)
		{
			if (monster32s.Count > 0)
			{
				monster32s[num].DotsAnnouncedDeath();
			}
		}
		if (allSplits.Count > 0)
		{
			for (int num2 = allSplits.Count - 1; num2 >= 0; num2--)
			{
				allSplits[num2].DotsAnnouncedDeath();
			}
		}
	}
}
