using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell1008ArcaneExplosion : SpellBase
{
	public class ExplosionController
	{
		private readonly Spell1008ArcaneExplosion Spell;

		private readonly Spell1008Effect Effect;

		private readonly Vector3 Position;

		private readonly HashSet<Collider> Attacked;

		private float Timer;

		private float hoverRecheckTargetTimer;

		private int currentDamageIndex;

		public bool IsFinish;

		private float OverDurationTimer => Timer - 0.4f;

		public ExplosionController(Spell1008ArcaneExplosion spell, Vector3 position)
		{
			Spell = spell;
			Effect = (Spell1008Effect)Spell.EffectBase;
			Position = position;
			Attacked = new HashSet<Collider>();
			Effect.Play(this);
		}

		public void Update()
		{
			Timer += Time.deltaTime;
			if (currentDamageIndex < Spell.damageTime.Length && Timer >= Spell.damageTime[currentDamageIndex])
			{
				MakeExplosionDamage(Spell.spellCfg.radius * Spell.damageRadiusRatios[currentDamageIndex]);
				currentDamageIndex++;
			}
			if (OverDurationTimer > 0f && OverDurationTimer < Spell.SpellHoverTime && Spell.SpellHoverTime > 0f && Timer > Spell.particleSlowDownStartTime)
			{
				Effect.SetPause(this, pause: true);
				hoverRecheckTargetTimer += Time.deltaTime;
				if (hoverRecheckTargetTimer >= Spell.hoverRecheckTargetInterval)
				{
					hoverRecheckTargetTimer -= Spell.hoverRecheckTargetInterval;
					MakeExplosionDamage(Spell.spellCfg.radius);
				}
			}
			else if (OverDurationTimer > Spell.SpellHoverTime)
			{
				Effect.SetPause(this, pause: false);
			}
			if (OverDurationTimer > Spell.SpellHoverTime + Spell.recycleTime)
			{
				Over();
			}
		}

		private void MakeExplosionDamage(float radius)
		{
			int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(Position, radius, Spell.targetBuffer, targetTags, Spell.attackLayers);
			for (int i = 0; i < collidersNonAlloc; i++)
			{
				Collider collider = Spell.targetBuffer[i];
				if (!Attacked.Contains(collider))
				{
					Spell.AOETriggerIn(collider);
					Attacked.Add(collider);
				}
			}
		}

		public void Over()
		{
			Effect.Stop(this);
			IsFinish = true;
		}
	}

	[Space(50f)]
	public Transform ground_Layer;

	public ShockParam shock;

	public float[] damageTime;

	public float[] damageRadiusRatios;

	public float recycleTime;

	public float particleSlowDownStartTime;

	public float hoverRecheckTargetInterval;

	private static float AroundOwnerCommonAngle = 0f;

	public float explosionRotateAnglePerSpell;

	public LayerMask attackLayers;

	private bool fallOnGround = true;

	private bool FallOver;

	private readonly List<ExplosionController> Explosions = new List<ExplosionController>();

	private readonly Collider[] targetBuffer = new Collider[256];

	public static readonly HashSet<string> targetTags = new HashSet<string> { "Monster", "Destructible", "RollBall", "Butterfly", "Brittleness" };

	protected override void PlayShootSound()
	{
		if (!base.SIP.spellIsFall)
		{
			base.PlayShootSound();
		}
		else
		{
			PlaySE("FallStart");
		}
	}

	public override void InitializeCallback()
	{
		fallOnGround = !base.SIP.spellIsFall;
		FallOver = !base.SIP.spellIsFall;
		if (base.SIP.spellIsFall)
		{
			ApplySpeedToVelocity();
			base.spellCfg.radius = GetFallingGroundDamageRadius();
		}
		enableFollowMouse = base.SIP.spellIsFall;
		enableFollowTarget = base.SIP.spellIsFall;
		enableNormalTransform = base.SIP.spellIsFall;
		base.enableAroundPlayer = base.SIP.spellIsFall;
		base.DurationTimer = 0f;
		if (!base.spellCfg.isSplitSpell)
		{
			switch (base.currentSpellMovement)
			{
			case SpellSpecialMovementType.Rotation:
			{
				base.spellAroundOwnerCurrentAngle = AroundOwnerCommonAngle;
				AroundOwnerCommonAngle += explosionRotateAnglePerSpell;
				base.spellAroundOwnerCurrentAngle += 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / base.CurrentSpeed) * Time.deltaTime;
				Vector3 vector = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
				base.Direction = ToPointDir(vector);
				base.transform.position = Tool2D.IgnoreZPoint(vector, base.transform.position.z);
				break;
			}
			case SpellSpecialMovementType.ChaseOwner:
				if (!base.SIP.spellIsFall)
				{
					base.transform.position = GetAroundTargetBasePoint();
				}
				break;
			}
		}
		base.transform.localScale = Vector3.one;
		tsf_Layer.localScale = Vector3.one * base.spellCfg.radius;
		ground_Layer.localScale = Vector3.one * base.spellCfg.radius;
		if (fallOnGround)
		{
			MakeDamage();
		}
	}

	public override void Update()
	{
		base.Update();
		foreach (ExplosionController explosion in Explosions)
		{
			explosion.Update();
		}
		for (int i = 0; i < Explosions.Count; i++)
		{
			if (Explosions[i].IsFinish)
			{
				Explosions.RemoveAt(i);
				i--;
			}
		}
		if (Explosions.Count <= 0 && FallOver)
		{
			PoolRecycle();
		}
	}

	private void MakeDamage()
	{
		ExplosionController item = new ExplosionController(this, Tool2D.IgnoreZPoint(base.transform.position));
		Explosions.Add(item);
	}

	public void KillSomeOne(Vector3 deadPoint)
	{
		if (base.spellCfg.int1 != 0)
		{
			SpellConfig configCopy = SpellConfig.GetConfigCopy(base.spellCfg.id);
			SpellBase component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + configCopy.prefab, deadPoint).GetComponent<SpellBase>();
			SpellInitialParameter spellInitialParameter = base.InitialParameter.Copy();
			spellInitialParameter.shootSpellPreSpells = preSpellIDs.Copy();
			spellInitialParameter.finalSizeRatio *= base.spellCfg.float1;
			if (spellInitialParameter.spellIsFall && spellInitialParameter.finalShootSpatialInfo != null)
			{
				ShootSpellSpatialInfo shootSpellSpatialInfo = spellInitialParameter.finalShootSpatialInfo.Copy();
				shootSpellSpatialInfo.Direction = Tool2D.IgnoreZPoint(UnityEngine.Random.onUnitSphere).normalized;
				spellInitialParameter.shootDirection = shootSpellSpatialInfo.Direction;
				shootSpellSpatialInfo.Target = Tool2D.IgnoreZPoint(deadPoint);
				shootSpellSpatialInfo.Start = Tool2D.IgnoreZPoint(deadPoint);
				spellInitialParameter.finalShootSpatialInfo = shootSpellSpatialInfo;
			}
			component.shouldCameraShock = base.shouldCameraShock;
			component.OwnerSpell = base.OwnerSpell;
			if (base.OwnerPoint != Vector3.zero || base.OwnerTsf != null)
			{
				component.OwnerTsf = base.OwnerTsf;
				component.OwnerPoint = base.OwnerPoint;
			}
			component.Initialize(spellInitialParameter);
		}
	}

	private void OnDisable()
	{
		foreach (ExplosionController explosion in Explosions)
		{
			explosion.Over();
		}
		Explosions.Clear();
	}

	protected override void InitSpeedAndPosition()
	{
		if (base.SIP.spellIsFall)
		{
			base.spellCfg.speed = (8f + base.bonusSpeed) * base.speedRatio * base.finalSpeedRatio;
		}
		base.InitSpeedAndPosition();
	}

	protected override void OnFallingGround()
	{
		fallOnGround = true;
		MakeDamage();
		EffectBase.PlayFallingGroundSound();
		if (!OnFallingGroundTryRebound())
		{
			EffectBase.ManualRecycleEffect("Fall");
			EffectBase.ManualRecycleEffect("FallShadow");
			ChangeCurrentSpeed(0f);
			FallOver = true;
		}
	}

	protected override bool TryFallingRefract()
	{
		bool flag = GetFallingGroundDamageTargets().Any((Collider e) => e.CompareTag("Monster"));
		if (base.remainRefractCount <= 0 || !flag)
		{
			return false;
		}
		TryRefract();
		base.CurrentUpSpeed = FallingReboundForce;
		base.InFallRebounding = true;
		return true;
	}

	protected override bool OnAOEHitUnit(UnitProperty unit)
	{
		if (IsSameCamp(unit.unitCfg.unitType))
		{
			return false;
		}
		CreateHitEffect(unit.transform.position);
		TakeDamageInfo takeDamageInfo = MakeDamageToUnit(unit);
		if (takeDamageInfo.isTargetDead && takeDamageInfo.beHitPpt.unitCfg.triggerDeadEvent)
		{
			KillSomeOne(takeDamageInfo.beHitPpt.transform.position);
		}
		return true;
	}

	protected override TakeDamageInfo CreateDefaultTakeDamageInfo(UnitProperty unit)
	{
		TakeDamageInfo takeDamageInfo = base.CreateDefaultTakeDamageInfo(unit);
		takeDamageInfo.canRebound = false;
		return takeDamageInfo;
	}
}
