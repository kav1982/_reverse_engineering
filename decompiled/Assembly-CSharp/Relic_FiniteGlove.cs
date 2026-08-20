using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Relic_FiniteGlove : Relic_FollowObj
{
	[Space(50f)]
	public Animator anima;

	public VariableFloat actionInterval;

	[Header("Sound")]
	public AudioSource as_Prepare;

	public AudioSource as_Snap;

	private float timer;

	private bool inAction;

	private EntityManager ettMgr;

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		actionInterval.RandomResult();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_Prepare.volume = DataMgr.settingData.GetFinalSound();
		as_Snap.volume = as_Prepare.volume;
	}

	private void Update()
	{
		if (!inAction && LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count > 0)
		{
			timer += Time.deltaTime;
			if (timer >= actionInterval.result)
			{
				timer = 0f;
				actionInterval.RandomResult();
				anima.Play("Action");
				inAction = true;
				as_Prepare.Play();
			}
		}
	}

	private void _DoKill()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/Item/Relic_FiniteGlove_SnapEF", base.transform.position, 3f);
		TakeDamageInfo_Dots damageInfo = TakeDamageInfo_Dots.NewInfo(PlayerMgr.Inst.PlayerEtt);
		damageInfo.dontCreateDeadEF = true;
		damageInfo.dontCreatebloodSplat = true;
		damageInfo.dontPlayDeadSE = true;
		for (int i = 0; i < LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count; i++)
		{
			if (i % 2 == 0)
			{
				Entity targetEtt = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList[i];
				if (ettMgr.GetComponentData<UnitProperty_Dots>(targetEtt).unitCfg.unitType == UnitType.Monster)
				{
					LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(targetEtt);
					ObjPoolMgr.Inst.GetGO("Prefabs/Item/Relic_FiniteGlove_Kill", componentData.Position, 3f);
					damageInfo.damage = 99999f;
					UnitDotsSyncSystem.TryAttackEntity(in targetEtt, in damageInfo, ettMgr);
				}
			}
		}
		as_Snap.Play();
	}

	private void _ActionFinish()
	{
		anima.Play("Idle");
		inAction = false;
	}
}
