using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Teammate : UnitBase, ICanLaunchSpellObject
{
	public abstract class TeammateState
	{
		public Teammate Self;

		public virtual void OnEnter()
		{
		}

		public virtual void OnUpdate()
		{
		}

		public virtual void OnFixedUpdate()
		{
		}

		public virtual void OnExit()
		{
		}
	}

	private TeammateState State;

	public readonly FusionProperty FusionData = new FusionProperty();

	private readonly List<SpellBase> _launchedSpells = new List<SpellBase>();

	[HideInInspector]
	public Spell3110LifeLine summonLifeLine;

	protected static bool needDisableEffect => EffectTransparencyController.ControlMode.Summon.GetTransparency() <= 0.01f;

	public IEnumerable<SpellBase> LaunchedSpells => _launchedSpells.Where((SpellBase e) => e.gameObject.activeInHierarchy && e.ownerPpt == myPpt);

	protected bool CanMove { get; set; }

	[HideInInspector]
	public bool beingControlledByTeammate6 { get; protected set; }

	protected virtual void OnEnable()
	{
		myPpt.OnEnterDelayDeathEvent += OnEnterDelayDeathEvent;
		myPpt.OnEnterFuseStageEvent += OnEnterFuseStateEvent;
		ColliderToggle(state: true);
	}

	protected void ColliderToggle(bool state)
	{
		if ((bool)base.CC_Self)
		{
			base.CC_Self.enabled = state;
		}
	}

	public override void Update()
	{
		base.Update();
		if (!base.IsLocked)
		{
			State?.OnUpdate();
		}
	}

	protected virtual void FixedUpdate()
	{
		if (!base.IsLocked)
		{
			State?.OnFixedUpdate();
		}
	}

	public virtual Vector3 GetLaunchSpellTargetPosition()
	{
		if ((object)targetPpt == null)
		{
			return base.transform.position;
		}
		return targetPpt.transform.position;
	}

	public virtual void HideTeammate()
	{
	}

	public virtual void ShowTeammate()
	{
	}

	public virtual ShootSpellSpatialInfo GetLaunchSpellSpatialInfo()
	{
		return ShootSpellSpatialInfo.ToPoint(base.transform.position, GetLaunchSpellTargetPosition());
	}

	public IEnumerable<SpellBase> Launch()
	{
		List<SpellBase> list = CreateLaunchSpells().ToList();
		_launchedSpells.AddRange(list);
		return list;
	}

	protected virtual IEnumerable<SpellBase> CreateLaunchSpells()
	{
		if (GetLaunchGroup() == null)
		{
			return Array.Empty<SpellBase>();
		}
		CreateLaunchParameterBuilder();
		GetLaunchSpellSpatialInfo();
		return Array.Empty<SpellBase>();
	}

	public override void EveryInitialCallback()
	{
		CanMove = true;
		beingControlledByTeammate6 = false;
	}

	public virtual SpellInitialParameter.Builder CreateLaunchParameterBuilder()
	{
		SpellInitialParameter.Builder builder = new SpellInitialParameter.Builder();
		if ((object)base.SummonerSpellBase.shooterWand != null)
		{
			Wand wand = PlayerMgr.Inst.Wands.FirstOrDefault((Wand e) => e.WandCfg == base.SummonerSpellBase.InitialParameter.shooterWandCfg);
			if ((bool)wand)
			{
				builder.ApplyWandEffect(wand, base.SummonerSpellBase.wandChargeData);
			}
		}
		return builder;
	}

	public virtual SpellShootGroup GetLaunchGroup()
	{
		return base.SummonerSpellBase.ShootData?.SubGroup;
	}

	public virtual void OnEnterDelayDeathEvent()
	{
	}

	public virtual void OnEnterFuseStateEvent()
	{
	}

	public void ChangeState(TeammateState newState)
	{
		State?.OnExit();
		State = newState;
		State.Self = this;
		State?.OnEnter();
	}

	protected bool SummonMayThroughMap()
	{
		return base.SummonerSpellBase.SIP.SummonFollowOwnerThroughMapChance >= UnityEngine.Random.Range(0f, 1f);
	}

	public virtual void SummonFollowOwnerThroughMap()
	{
		base.SummonerSpellBase.gameObject.SetActive(value: true);
		base.SummonsThrough();
		if (summonLifeLine != null)
		{
			summonLifeLine.gameObject.SetActive(value: true);
			summonLifeLine.resetTie();
		}
		base.transform.position = Tool2D.GetNavMeshPoint(PlayerMgr.Inst.PlayerPoint + Tool2D.IgnoreZPoint(UnityEngine.Random.insideUnitSphere));
	}
}
