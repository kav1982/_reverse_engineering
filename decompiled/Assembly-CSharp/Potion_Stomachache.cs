using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Potion_Stomachache : LayerCorrect
{
	private enum PotionState
	{
		Fart,
		Stop
	}

	[Space(50f)]
	public float offset;

	public float height;

	public float fartInterval;

	public float radius;

	public float angle;

	public float recoil;

	public float effectDuration;

	public float mucusMoveRatio;

	public int venomStack;

	public int damage;

	public ParticleSystem[] pss;

	public AudioSource as_Loop;

	private PotionState state;

	private PotionConfig potionCfg;

	private float fartTimer;

	private float durationTimer;

	private bool isRegisterImmnue;

	private NativeList<Entity> targetEttList = new NativeList<Entity>(Allocator.Persistent);

	private TakeDamageInfo_Dots damageInfo;

	private EntityManager ettMgr;

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
		as_Loop.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		damageInfo = TakeDamageInfo_Dots.NewInfo(PlayerMgr.Inst.PlayerEtt);
		damageInfo.damage = damage;
	}

	private void Update()
	{
		switch (state)
		{
		case PotionState.Fart:
			base.transform.position = PlayerMgr.Inst.PlayerPoint - PlayerMgr.Inst.PlayerCtrller.CurrentDir * offset + new Vector3(0f, 0f, 0f - height);
			base.transform.up = PlayerMgr.Inst.PlayerCtrller.CurrentDir;
			fartTimer += Time.deltaTime;
			if (fartTimer >= fartInterval)
			{
				fartTimer = 0f;
				UnitDotsSyncSystem.GetAttackableEntitiesInRange(base.transform.position, radius, UnitType.Player, containsBrittleness: true, ref targetEttList);
				foreach (Entity targetEtt2 in targetEttList)
				{
					Entity targetEtt = targetEtt2;
					if (!ettMgr.HasComponent<LocalTransform>(targetEtt))
					{
						continue;
					}
					Vector3 vector = ettMgr.GetComponentData<LocalTransform>(targetEtt).Position;
					if ((vector - base.transform.position).sqrMagnitude < radius * radius && Vector3.Angle(-PlayerMgr.Inst.PlayerCtrller.CurrentDir, vector - base.transform.position) <= angle / 2f)
					{
						UnitDotsSyncSystem.TryAttackEntity(in targetEtt, in damageInfo, ettMgr);
						if (ettMgr.HasComponent<UnitProperty_Dots>(targetEtt))
						{
							UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(targetEtt);
							componentData.SetMucus(effectDuration, mucusMoveRatio, 1f);
							componentData.SetVenom(effectDuration, venomStack);
							ettMgr.SetComponentData(targetEtt, componentData);
						}
					}
				}
				PlayerMgr.Inst.PlayerCtrller.TakeKnockback(PlayerMgr.Inst.PlayerCtrller.CurrentDir * recoil);
			}
			durationTimer += Time.deltaTime;
			if (durationTimer >= potionCfg.float1)
			{
				durationTimer = 0f;
				state = PotionState.Stop;
				for (int i = 0; i < pss.Length; i++)
				{
					pss[i].Stop();
				}
				as_Loop.Stop();
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case PotionState.Stop:
			break;
		}
	}

	public void Initialize(PotionConfig potionCfg)
	{
		this.potionCfg = potionCfg;
		state = PotionState.Fart;
		durationTimer = 0f;
		for (int i = 0; i < pss.Length; i++)
		{
			pss[i].Play();
		}
		as_Loop.Play();
		if (!isRegisterImmnue)
		{
			isRegisterImmnue = true;
			PlayerMgr.Inst.ImmuneMucusRegister();
			PlayerMgr.Inst.ImmuneVenomRegister();
		}
	}

	public void DestroySelf()
	{
		if (targetEttList.IsCreated)
		{
			targetEttList.Dispose();
		}
		PlayerMgr.Inst.ImmuneMucusUnregister();
		PlayerMgr.Inst.ImmuneVenomUnregister();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (targetEttList.IsCreated)
		{
			targetEttList.Dispose();
		}
	}
}
