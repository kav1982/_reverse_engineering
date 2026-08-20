using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster305ElectricArea : MonoBehaviour
{
	public Entity attackerEntity;

	public List<LineRenderer> lines = new List<LineRenderer>();

	private int linePointAmount;

	private float areaRadius;

	private float linePointOffset;

	public int originPointAmount;

	public int finalPointAmount;

	public float originRadius;

	public float finalRadius;

	public float originOffset;

	public float finalOffset;

	public float expandDuration;

	public float duration;

	private float durationTimer;

	public float damage;

	private float circleUpdateTimer;

	private float checkIntervalTimer;

	private float attackIntervalTimer;

	public Transform normalParticleTsf;

	public Transform bottomParticleTsf;

	public ParticleSystem existParticle;

	public ParticleSystem fadeParticle;

	public AudioSource as_Loop;

	private bool recycled;

	private List<Entity> attackedEntity = new List<Entity>();

	private List<float> attackedTimer = new List<float>();

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void OnEnable()
	{
		recycled = false;
		foreach (LineRenderer line in lines)
		{
			line.positionCount = linePointAmount;
			line.enabled = true;
		}
		areaRadius = originRadius;
		durationTimer = 0f;
		normalParticleTsf.localScale = Vector3.one * 0.01f;
		bottomParticleTsf.localScale = Vector3.one * 0.01f;
		existParticle.Play();
		EventMgr.EndlessStageClear = (Action)Delegate.Combine(EventMgr.EndlessStageClear, new Action(Return));
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SEMgr.Inst.monster305_Attack.PlaySE(base.transform.position);
		areaRadius = Mathf.Lerp(originRadius, finalRadius, durationTimer / expandDuration);
		linePointOffset = Mathf.Lerp(originOffset, finalOffset, durationTimer / expandDuration);
		linePointAmount = Mathf.FloorToInt(Mathf.Lerp(originPointAmount, finalPointAmount, durationTimer / expandDuration));
		normalParticleTsf.localScale = Vector3.one * durationTimer / expandDuration;
		bottomParticleTsf.localScale = Vector3.one * durationTimer / expandDuration;
		DrawCircle();
		attackedEntity.Clear();
		attackedTimer.Clear();
	}

	private void OnDisable()
	{
		EventMgr.EndlessStageClear = (Action)Delegate.Remove(EventMgr.EndlessStageClear, new Action(Return));
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_Loop.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Update()
	{
		if (!recycled)
		{
			durationTimer += Time.deltaTime;
			circleUpdateTimer += Time.deltaTime;
			if (!UnitDotsSyncSystem.EntityIsValid(attackerEntity))
			{
				Return();
			}
		}
		if (circleUpdateTimer > 0.02f)
		{
			circleUpdateTimer = 0f;
			DrawCircle();
		}
		if (durationTimer < expandDuration)
		{
			float num = durationTimer / expandDuration;
			areaRadius = Mathf.Lerp(originRadius, finalRadius, num);
			linePointOffset = Mathf.Lerp(originOffset, finalOffset, num);
			linePointAmount = Mathf.FloorToInt(Mathf.Lerp(originPointAmount, finalPointAmount, num));
			normalParticleTsf.localScale = Vector3.one * num;
			bottomParticleTsf.localScale = Vector3.one * num;
		}
		else
		{
			float num2 = (expandDuration * 2f - durationTimer) / expandDuration;
			num2 = 1f - (1f - num2) * (1f - num2);
			areaRadius = Mathf.Lerp(originRadius, finalRadius, num2);
			linePointOffset = Mathf.Lerp(originOffset, finalOffset, num2);
			linePointAmount = Mathf.FloorToInt(Mathf.Lerp(originPointAmount, finalPointAmount, num2));
			normalParticleTsf.localScale = Vector3.one * num2;
			bottomParticleTsf.localScale = Vector3.one * num2;
		}
		if (durationTimer >= duration && !recycled)
		{
			Return();
		}
		if (recycled)
		{
			return;
		}
		for (int num3 = attackedEntity.Count - 1; num3 >= 0; num3--)
		{
			attackedTimer[num3] -= Time.deltaTime;
			if (attackedTimer[num3] < 0f)
			{
				attackedTimer.RemoveAt(num3);
				attackedEntity.RemoveAt(num3);
			}
		}
		checkIntervalTimer += Time.deltaTime;
		if (checkIntervalTimer > 0.16f)
		{
			checkIntervalTimer -= 0.16f;
			DealDamage();
		}
	}

	private void Return()
	{
		recycled = true;
		areaRadius = 0f;
		existParticle.Stop();
		as_Loop.Stop();
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, 2f);
		foreach (LineRenderer line in lines)
		{
			line.enabled = false;
		}
	}

	public void DrawCircle()
	{
		foreach (LineRenderer line in lines)
		{
			line.positionCount = linePointAmount;
			for (int i = 0; i < linePointAmount; i++)
			{
				float num = (float)i * 360f / (float)linePointAmount * (MathF.PI / 180f);
				float x = base.transform.position.x + areaRadius * Mathf.Sin(num);
				float y = base.transform.position.y + areaRadius * Mathf.Cos(num);
				Vector3 rootPoint = new Vector3(x, y, 0f);
				Vector3 vector = new Vector3(Mathf.Sin(num), Mathf.Cos(num), 0f) * UnityEngine.Random.Range(0f - linePointOffset, linePointOffset) * 1.5f + new Vector3(Mathf.Sin(num + 90f), Mathf.Cos(num + 90f), 0f) * UnityEngine.Random.Range(0f - linePointOffset, linePointOffset);
				rootPoint += vector;
				rootPoint = Tool2D.GetLayerPoint(rootPoint);
				line.SetPosition(i, rootPoint);
			}
		}
	}

	public void DealDamage()
	{
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, areaRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			Entity entity = distanceHits[i].entity;
			if (attackedEntity.Contains(entity))
			{
				continue;
			}
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			switch (layer)
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 2097152u:
			{
				TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(attackerEntity);
				info2.damage = damage;
				if (layer == 131072)
				{
					info2.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info2);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster305_Hit", distanceHits[i].point + new Vector3(0f, 0f, -0.3f), 1f);
				SEMgr.Inst.monster305_Hit.PlaySE();
				attackedEntity.Add(entity);
				attackedTimer.Add(0.33f);
				break;
			}
			case 32768u:
			case 131072u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(attackerEntity);
				info.damage = damage * 2f;
				if (layer == 131072)
				{
					info.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				attackedEntity.Add(entity);
				attackedTimer.Add(0.33f);
				break;
			}
			}
		}
	}
}
