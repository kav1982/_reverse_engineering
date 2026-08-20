using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell1015ArcaneNova : LauncherSpellBase
{
	[Serializable]
	public class SubGroupModifyData
	{
		public float SizeRatio = 1f;

		public float SpeedRatio = 0.85f;

		public float KnockBackRatio = 0.7f;

		[Min(45f)]
		public int ExterScatter = 45;
	}

	[Header("\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdӷ\ufffd\ufffd\ufffd\ufffdĲ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd")]
	public SubGroupModifyData SubGroupModify = new SubGroupModifyData();

	private int remainShootCount;

	public AudioSource as_Loop;

	private float shootInterval;

	private float shootIntervalTimer;

	protected override float FallingReboundForce => 0f - base.CurrentUpSpeed;

	protected override float InFallingReboundingGravity => base.InFallingReboundingGravity * 0.08f;

	public override void InitializeCallback()
	{
		remainShootCount = base.spellCfg.int1;
		shootIntervalTimer = shootInterval;
		if (!base.SIP.spellIsFall)
		{
			base.penetrateTime = 10000;
			base.rebounceTime = 10000;
			shootInterval = base.spellCfg.duration / (float)base.spellCfg.int1;
		}
		else
		{
			shootInterval = base.Height / (0f - base.CurrentUpSpeed) / (float)base.spellCfg.int1;
			int num = base.rebounceTime + base.remainRefractCount;
			if (num > 0)
			{
				shootInterval *= 1f + (float)num * 0.58f;
			}
		}
		base.DurationTimer -= shootInterval;
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		ApplySpeedToVelocity();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	public void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	public override void Update()
	{
		base.Update();
		if (!base.SIP.spellIsFall)
		{
			base.DurationTimer += Time.deltaTime;
		}
		if (base.DurationTimer > base.spellCfg.duration && !base.SIP.spellIsFall)
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
				return;
			}
			if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
			{
				PoolRecycle();
				return;
			}
			base.transform.localScale = Vector3.one * (base.transform.localScale.x - 5f * Time.deltaTime);
			if (base.transform.localScale.x <= 0f)
			{
				PoolRecycle();
			}
		}
		else
		{
			ShootUpdate(Time.deltaTime);
		}
	}

	private void SoundVolumeChange()
	{
		as_Loop.volume = DataMgr.settingData.GetFinalSound();
	}

	private void ShootUpdate(float deltaTime)
	{
		if (remainShootCount > 0)
		{
			shootIntervalTimer += deltaTime;
			if (shootIntervalTimer >= shootInterval)
			{
				shootIntervalTimer -= shootInterval;
				Launch();
				remainShootCount -= GetShootMultiplier();
			}
		}
	}

	private int GetShootMultiplier()
	{
		float fps = GameMgr.Inst.GetFps();
		if (GeneralTool.IsLowFpsOptimizeActive(30f))
		{
			return Mathf.Max(1, Mathf.CeilToInt(5f - fps / 6f));
		}
		return 1;
	}

	private Vector3 GetLaunchSpellTargetPosition()
	{
		Vector3 position = base.transform.position;
		return GetNearestTargetablePpt(position)?.transform.position ?? (position + base.Direction * 2f);
	}

	public override SpellInitialParameter.Builder CreateLaunchParameterBuilder()
	{
		SpellInitialParameter.Builder builder = base.CreateLaunchParameterBuilder();
		builder.OnBuildAfter += delegate(SpellInitialParameter.Builder self, SpellInitialParameter parameter)
		{
			parameter.finalDamageRatio *= GetShootMultiplier();
			parameter.finalSizeRatio *= SubGroupModify.SizeRatio;
			parameter.finalSpeedRatio *= SubGroupModify.SpeedRatio;
			parameter.finalKnockBackRatio *= SubGroupModify.KnockBackRatio;
			parameter.extraScatter += SubGroupModify.ExterScatter;
		};
		return builder;
	}

	protected override IEnumerable<SpellBase> CreateLaunchSpells()
	{
		SpellBase[] array = base.CreateLaunchSpells().ToArray();
		List<SpellBase> list = array.ToList();
		list.Add(this);
		Spell3007ChainSystem.CreateChains(list.ToArray(), base.shooterWand);
		return array;
	}

	public override ShootSpellSpatialInfo GetLaunchSpellSpatialInfo()
	{
		return ShootSpellSpatialInfo.ToPoint(base.transform.position, GetLaunchSpellTargetPosition());
	}
}
