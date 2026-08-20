using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Spell1007BlackHole : SpellBase
{
	[Space(50f)]
	public AudioSource as_Loop;

	public float dragForceRatioToPlayer;

	public ShockParam fallShock;

	public LayerMask TargetLayer;

	private float damageIntervalTimer;

	private float pullForce;

	private static readonly Collider[] targetBuffer = new Collider[256];

	private float extraRadiu;

	private float initialDamage;

	private static HashSet<string> monsterAttackTags = new HashSet<string> { "Player", "Teammate", "RollBall", "Butterfly", "Brittleness" };

	private static readonly HashSet<string> targetTags = new HashSet<string> { "Monster", "Destructible", "RollBall", "Butterfly", "Brittleness" };

	protected override float FallSpeedRatio => 5f;

	public override void SingleInitialCallback()
	{
		SEMgr.Inst.ChangeASTo3D(as_Loop);
	}

	public override void InitializeCallback()
	{
		if (!base.SIP.spellIsFall)
		{
			base.transform.position = Tool2D.IgnoreZPoint(base.transform);
		}
		ApplySpeedToVelocity();
		pullForce = base.spellCfg.knockback;
		base.spellCfg.knockback = 0f;
		initialDamage = SpellConfig.dic[base.spellCfg.id].damage;
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		if (!base.SIP.spellIsFall)
		{
			EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
			SoundVolumeChange();
			as_Loop.Play();
			EffectBase.ManualCreateEffect("Trail");
			EffectBase.ManualCreateEffect("GroundTrail");
		}
		extraRadiu = 0f;
	}

	public void OnDisable()
	{
		if (!base.SIP.spellIsFall)
		{
			EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
			as_Loop.Stop();
		}
	}

	private void SoundVolumeChange()
	{
		as_Loop.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void Update()
	{
		base.Update();
		UpdateEffectRadius();
		if (!base.SIP.spellIsFall)
		{
			UpdateAttackBehaviour();
		}
		UpdateDurationTime();
	}

	private void UpdateDurationTime()
	{
		if (base.SIP.spellIsFall)
		{
			return;
		}
		base.DurationTimer += Time.deltaTime * GetLowFPSTimeScale();
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				rigid.linearVelocity = Vector3.zero;
				base.CurrentSpeed = 0f;
			}
			if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
			{
				base.SpellHoverTimer += Time.deltaTime;
			}
			else
			{
				PoolRecycle();
			}
		}
	}

	private void UpdateAttackBehaviour()
	{
		damageIntervalTimer += Time.deltaTime * GetLowFPSTimeScale();
		bool flag = false;
		if (damageIntervalTimer >= base.spellCfg.DPSDamageInterval)
		{
			damageIntervalTimer -= base.spellCfg.DPSDamageInterval;
			flag = true;
		}
		if (IsSameCamp(UnitType.Player))
		{
			float num = base.spellCfg.radius * base.spellCfg.radius;
			foreach (UnitProperty targetablePpt in LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts)
			{
				if (targetablePpt.CanTouch && !(Tool2D.IgnoreZDistanceSqr(base.transform.position, targetablePpt.transform.position) > num))
				{
					targetablePpt.TakeKnockback(-ToPointDir(targetablePpt.transform.position) * pullForce * base.spellCfg.DPSDamageInterval * Time.deltaTime);
				}
			}
			if (flag)
			{
				int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(base.transform.position, base.spellCfg.radius, targetBuffer, targetTags, TargetLayer);
				for (int i = 0; i < collidersNonAlloc; i++)
				{
					Collider col = targetBuffer[i];
					AOETriggerIn(col);
				}
			}
			return;
		}
		List<UnitProperty> summonsPpts = PlayerMgr.Inst.summonsPpts;
		for (int j = 0; j < summonsPpts.Count; j++)
		{
			if (summonsPpts[j].CanTouch && (base.transform.position - summonsPpts[j].transform.position).sqrMagnitude < base.spellCfg.radius * base.spellCfg.radius)
			{
				if (summonsPpts[j].unitCfg.unitType == UnitType.Player)
				{
					summonsPpts[j].TakeKnockback(-ToPointDir(summonsPpts[j].transform.position) * pullForce * base.spellCfg.DPSDamageInterval * dragForceRatioToPlayer * Time.deltaTime);
				}
				else
				{
					summonsPpts[j].TakeKnockback(-ToPointDir(summonsPpts[j].transform.position) * pullForce * base.spellCfg.DPSDamageInterval * Time.deltaTime);
				}
			}
		}
		if (PlayerMgr.Inst.PlayerPpt.CanTouch && (base.transform.position - PlayerMgr.Inst.PlayerPoint).sqrMagnitude < base.spellCfg.radius * base.spellCfg.radius)
		{
			PlayerMgr.Inst.PlayerPpt.TakeKnockback(-ToPointDir(PlayerMgr.Inst.PlayerPoint) * pullForce * base.spellCfg.DPSDamageInterval * dragForceRatioToPlayer * Time.deltaTime);
		}
		if (flag)
		{
			int collidersNonAlloc2 = GeneralTool.GetCollidersNonAlloc(base.transform.position, base.spellCfg.radius, targetBuffer, monsterAttackTags);
			for (int k = 0; k < collidersNonAlloc2; k++)
			{
				AOETriggerIn(targetBuffer[k]);
			}
		}
	}

	protected override TakeDamageInfo CreateDefaultTakeDamageInfo(UnitProperty unit)
	{
		TakeDamageInfo takeDamageInfo = base.CreateDefaultTakeDamageInfo(unit);
		takeDamageInfo.canRebound = false;
		takeDamageInfo.damage = (base.SIP.spellIsFall ? base.spellCfg.damage : ((float)Mathf.CeilToInt(base.spellCfg.damage * base.spellCfg.DPSDamageInterval)));
		if (GeneralTool.IsLowFpsOptimizeActive(100f))
		{
			takeDamageInfo.damage *= (int)GeneralTool.GetCommonSpellLowFpsTimeScale(60f, 5f);
		}
		return takeDamageInfo;
	}

	private void UpdateEffectRadius()
	{
		float num = base.spellCfg.float1 / 2f * Time.deltaTime * GetLowFPSTimeScale();
		base.spellCfg.radius += num;
		if (base.SIP.radiuDecreaseRatio < 1f)
		{
			extraRadiu += num * base.SIP.radiuDecreaseRatio;
			float num2 = 1f + GeneralTool.GetSpellRadiusToDamageRatio(extraRadiu, base.SIP.radiuDecreaseRatio, base.SIP.radiuDcreaseTransIntoDamageRatio);
			base.spellCfg.damage = Mathf.Ceil(initialDamage * base.damageRatio * num2 * base.finalDamageRatio) + base.SIP.finalDamageExtra;
		}
		base.transform.localScale = Vector3.one * base.spellCfg.radius * 2f;
	}

	protected override SpellBase CreateSplitSpell(SpellConfig splitCfg, float baseAngle, int index)
	{
		SpellBase spellBase = base.CreateSplitSpell(splitCfg, baseAngle, index);
		if (!base.SIP.spellIsFall)
		{
			Vector3 position = spellBase.transform.position;
			spellBase.transform.position = base.transform.position;
			spellBase.transform.DOMove(position, 0.6f);
		}
		return spellBase;
	}

	public override void TriggerIn(Collider other)
	{
	}

	protected override void OnFallingGround()
	{
		EffectBase.PlayFallingGroundSound();
		EffectBase.ManualCreateEffect("Explosion");
		EffectBase.ManualCreateEffect("ExplosionB");
		List<UnitProperty> targetablePpts = LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts;
		float fallDamageRadiusSqr = GetFallingGroundDamageRadius() * GetFallingGroundDamageRadius();
		foreach (UnitProperty item in targetablePpts.Where((UnitProperty t) => t.CanTouch && (base.transform.position - t.transform.position).sqrMagnitude < fallDamageRadiusSqr))
		{
			item.TakeKnockback(-ToPointDir(item.transform.position) * pullForce * 0.1f);
		}
		MakeFallingGroundDamageToAround(base.transform.position);
		if (!OnFallingGroundTryRebound())
		{
			PoolRecycle();
		}
	}

	protected override void MakeFallingGroundDamageToAround(Vector3? position = null)
	{
		CamController.Inst.SetShock(fallShock);
		base.MakeFallingGroundDamageToAround(position);
	}
}
