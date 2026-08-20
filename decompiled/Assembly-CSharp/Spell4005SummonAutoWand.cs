using UnityEngine;

public class Spell4005SummonAutoWand : SpellBase
{
	[HideInInspector]
	public Wand targetWand;

	public override void InitializeCallback()
	{
		if (ownerPpt.unitCfg.unitType != 0)
		{
			Debug.LogError($"{ownerPpt.unitCfg.unitType} \ufffd\ufffd\ufffd\ufffd\ufffdٻ\ufffd\ufffd\ufffd\ufffd\ufffd");
			PoolRecycle();
			return;
		}
		GameObject gO = PlayerMgr.Inst.MiniPool.GetGO("Prefabs/Units/" + base.spellCfg.summonID, Tool2D.GetNavMeshPointIngoreZ(base.transform.position), Quaternion.identity, LevelMgr.Inst.CurrentRoomT.parent);
		Teammate52 component = gO.GetComponent<Teammate52>();
		component.SummonsInitial(this);
		component.SetWandDate(targetWand);
		targetWand.passiveAutoWandShooterData = component.InitialWandShootData();
		PlayerMgr.Inst.autoWandList.Add(targetWand, gO.GetComponent<UnitProperty>());
	}

	protected override void PlayShootSound()
	{
	}
}
