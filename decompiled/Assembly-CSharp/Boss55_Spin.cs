using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss55_Spin : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider cc;

	[Header("数值")]
	public float startDamageTime;

	public float damage;

	public float damageInterval;

	public ParticleSystem spinParticle;

	public ParticleSystem endParticle;

	public bool ended;

	private float endTimer;

	private float startDamageTimer;

	private List<Entity> attackedEntity = new List<Entity>();

	private List<float> attackedTimer = new List<float>();

	[Header("音效")]
	public AudioSource AS_Loop;

	public Entity thisEntity { get; set; }

	private void SoundVolumeChange()
	{
		AS_Loop.volume = DataMgr.settingData.GetFinalSound();
	}

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
		ended = false;
		endTimer = 0f;
		startDamageTimer = 0f;
		attackedEntity.Clear();
		attackedTimer.Clear();
		endParticle.Stop();
		endParticle.Clear();
		spinParticle.Play();
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228736u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, cc);
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		spinParticle.Stop();
		spinParticle.Clear();
		endParticle.Stop();
		endParticle.Clear();
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		startDamageTimer += Time.deltaTime;
		if (ended)
		{
			endTimer += Time.deltaTime;
			if (endTimer > 1f)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			return;
		}
		base.transform.position = Boss55.Inst.transform.position;
		for (int num = attackedEntity.Count - 1; num >= 0; num--)
		{
			attackedTimer[num] -= Time.deltaTime;
			if (attackedTimer[num] < 0f)
			{
				attackedTimer.RemoveAt(num);
				attackedEntity.RemoveAt(num);
			}
		}
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
	}

	public void OnTriggerStay_Dots(Entity other)
	{
		if (!ended)
		{
			DealDamage(other);
		}
	}

	private void DealDamage(Entity other)
	{
		if (!(startDamageTimer < startDamageTime) && !attackedEntity.Contains(other))
		{
			uint layer = UnitDotsSyncSystem.GetLayer(other);
			bool flag = false;
			switch (layer)
			{
			case 131072u:
			{
				TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(Boss55.Inst.myPpt.myEntity);
				info2.damage = damage * 10f;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info2);
				flag = true;
				break;
			}
			case 512u:
			case 2097152u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss55.Inst.myPpt.myEntity);
				info.damage = damage;
				Vector3 rootPoint = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite14_BladeHit", Tool2D.GetLayerPoint(rootPoint), Quaternion.identity, Vector3.one, 3f);
				SEMgr.Inst.boss55SwordHit.PlaySE();
				flag = true;
				break;
			}
			}
			if (flag)
			{
				attackedEntity.Add(other);
				attackedTimer.Add(damageInterval);
			}
		}
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}

	public void End()
	{
		ended = true;
		endTimer = 0f;
		spinParticle.Stop();
		endParticle.Play();
		AS_Loop.Stop();
	}
}
