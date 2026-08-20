using UnityEngine;

public class Destructible10 : UnitBase, IRoomCtrller
{
	[Range(0f, 1f)]
	[Space(50f)]
	public float summonChance;

	public int summonID;

	public float[] efHeights;

	private RoomController belongCtrller;

	private bool shouldSummon;

	private bool summoned;

	public override void EveryInitialCallback()
	{
		summoned = false;
		if (!DataMgr.selectedWorldData.battleData9.isDestructible10Comfirm && Random.value <= summonChance)
		{
			DataMgr.selectedWorldData.battleData9.isDestructible10Comfirm = true;
			shouldSummon = true;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			myPpt.unitCfg.maxHP = 100000000f;
			myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP;
			componentData.unitCfg.maxHP = 100000000f;
			componentData.unitCfg.currentHP = myPpt.unitCfg.maxHP;
			SetComponentData(componentData);
			base.tag = "Destructible";
		}
		else
		{
			shouldSummon = false;
			UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
			componentData2.unitCfg.isSolidObj = true;
			SetComponentData(componentData2);
			base.tag = "SolidObj";
		}
	}

	public override void Frame1InitialCallback()
	{
		if (belongCtrller.roomCfg.isFlipped)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "TransformFinish")
		{
			if (LevelMgr.Inst.CurrentRoomCtrller == belongCtrller)
			{
				for (int i = 0; i < efHeights.Length; i++)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Smoke", base.transform.position + new Vector3(0f, 0f, 0f - efHeights[i]), 2f);
				}
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + summonID, base.transform.position);
				DotsAnnouncedDeath();
				if (belongCtrller.IsFinish)
				{
					belongCtrller.AllAccessClose();
				}
			}
		}
		else
		{
			Debug.LogError(animaName);
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		base.AfterTakeDamage_Dots(ref info);
		if (shouldSummon && !summoned)
		{
			summoned = true;
			base.Anima.Play("SO10_Transform");
		}
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}
}
