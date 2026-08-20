using UnityEngine;

public class Monster23 : UnitBase
{
	private enum UnitState
	{
		BornIdle,
		Roam
	}

	public VariableFloat roamRadius;

	public VariableInt deadSpellCount;

	[Header("Tentacle")]
	public MeshRenderer[] mr_Tantacles;

	public VariableFloat tantacleWaveSpeed;

	public VariableFloat tantacleWaveRatio;

	public AIPattern pattern;

	[Header("pattern2")]
	public VariableInt beHitSpellCount;

	[Header("Spell Butterfly")]
	public float spellHeight;

	public float spellOffset;

	public VariableFloat spellSpeed;

	public float spellDuration;

	public float spellMinSpeed;

	public float spellDamage;

	private float spellCountRatio;

	private UnitState state;

	private SpellSpawnParams ssp;

	public override void SingleInitialCallback()
	{
		spellCountRatio = (GameMgr.IsMobile_Static ? 0.6f : 1f);
		for (int i = 0; i < mr_Tantacles.Length; i++)
		{
			mr_Tantacles[i].material.SetFloat("_WaveSpeed", tantacleWaveSpeed.RandomResult());
			mr_Tantacles[i].material.SetFloat("_WaveRatio", tantacleWaveRatio.RandomResult());
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10031);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void EveryInitialCallback()
	{
		state = UnitState.BornIdle;
		roamRadius.RandomResult();
		SetNavMeshArea(8);
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case UnitState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = UnitState.Roam;
				GetNavInfo(LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(base.transform.position + Tool2D.GetDir() * roamRadius.RandomResult()));
			}
			break;
		case UnitState.Roam:
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(base.transform.position + Tool2D.GetDir() * roamRadius.RandomResult()));
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			CheckNavInfo();
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		base.Anima.SetTrigger("BeHit");
		if (pattern == AIPattern.Pattern2)
		{
			beHitSpellCount.RandomResult();
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			float num = Random.Range(0, 360);
			for (int i = 0; (float)i < (float)beHitSpellCount.result * spellCountRatio; i++)
			{
				Vector3 dir = Tool2D.GetDir(num + 360f / (float)beHitSpellCount.result * (float)i);
				Vector3 vector = (sSPModifier.SpawnPosition = base.transform.position + dir * spellOffset + new Vector3(0f, 0f, 0f - spellHeight));
				sSPModifier.Speed = spellSpeed.RandomResult();
				sSPModifier.Float2 = spellMinSpeed / sSPModifier.Speed;
				sSPModifier.Direction = dir;
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		deadSpellCount.RandomResult();
		float num = Random.Range(0, 360);
		for (int i = 0; (float)i < (float)deadSpellCount.result * spellCountRatio; i++)
		{
			Vector3 dir = Tool2D.GetDir(num + 360f / (float)deadSpellCount.result * (float)i);
			Vector3 vector = (sSPModifier.SpawnPosition = base.transform.position + dir * spellOffset + new Vector3(0f, 0f, 0f - spellHeight));
			sSPModifier.Speed = spellSpeed.RandomResult();
			sSPModifier.Float2 = spellMinSpeed / sSPModifier.Speed;
			sSPModifier.Direction = dir;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
	}
}
