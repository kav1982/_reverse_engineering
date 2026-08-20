using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster48 : UnitBase
{
	[Space(50f)]
	public float invincibleRadius;

	public float invincibleCheckInterval;

	[Header("Connection")]
	public int connectionNodeCount;

	public float connectionStartPointHeight;

	public float connectionMiddlePointHeight;

	public float connectionMiddle2PointHeight;

	public float connectionEndPointHeight;

	private List<Entity> invincibleTargets = new List<Entity>();

	private List<GameObject> invincibleEFs = new List<GameObject>();

	private List<LineRenderer> connections = new List<LineRenderer>();

	private float checkIntervalTimer;

	public float protectableTime;

	private float lifeTime;

	private bool muted;

	public float EffectScale;

	[Header("悬浮岩石，只是为了好看")]
	public List<Transform> rockTransform;

	private List<float> rockStartPhase = new List<float>();

	private List<Vector3> rockStartPosition = new List<Vector3>();

	private List<float> rockAmplitude = new List<float>();

	private List<float> rockFrequency = new List<float>();

	public VariableFloat amplitude;

	public VariableFloat frequency;

	public VariableFloat startPhase;

	public override void SingleInitialCallback()
	{
		for (int i = 0; i < rockTransform.Count; i++)
		{
			rockStartPosition.Add(rockTransform[i].localPosition);
		}
	}

	public override void EveryInitialCallback()
	{
		rockStartPhase.Clear();
		rockAmplitude.Clear();
		rockFrequency.Clear();
		for (int i = 0; i < rockTransform.Count; i++)
		{
			rockStartPhase.Add(startPhase.RandomResult());
			rockAmplitude.Add(amplitude.RandomResult());
			rockFrequency.Add(frequency.RandomResult());
		}
		lifeTime = 0f;
		muted = false;
	}

	public unsafe override void Update()
	{
		lifeTime += Time.deltaTime;
		if (lifeTime > protectableTime)
		{
			if (muted)
			{
				return;
			}
			muted = true;
			for (int num = invincibleTargets.Count - 1; num >= 0; num--)
			{
				if (EntityIsValid(invincibleTargets[num]))
				{
					UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(invincibleTargets[num]);
					componentData.InvincibleUnregister();
					SetComponentData(componentData, invincibleTargets[num]);
				}
				ObjPoolMgr.Inst.RecycleGO(invincibleEFs[num]);
				ObjPoolMgr.Inst.RecycleGO(connections[num].gameObject);
			}
			invincibleTargets.Clear();
			connections.Clear();
			invincibleEFs.Clear();
			return;
		}
		for (int i = 0; i < rockTransform.Count; i++)
		{
			rockTransform[i].localPosition = rockStartPosition[i] + new Vector3(0f, rockAmplitude[i] * Mathf.Sin(rockStartPhase[i] + rockFrequency[i] * 2f * MathF.PI * Time.time), 0f);
		}
		for (int num2 = invincibleTargets.Count - 1; num2 >= 0; num2--)
		{
			Entity entity = invincibleTargets[num2];
			bool flag = EntityIsValid(entity);
			Vector3 vector = Vector3.zero;
			if (flag)
			{
				vector = GetComponentData<LocalTransform>(invincibleTargets[num2]).Position;
			}
			if (flag && (base.transform.position - vector).sqrMagnitude <= invincibleRadius * invincibleRadius)
			{
				connections[num2].enabled = true;
				Vector3 vector2 = base.transform.position + new Vector3(0f, 0f, 0f - connectionStartPointHeight);
				Vector3 vector3 = vector + new Vector3(0f, 0f, 0f - connectionEndPointHeight);
				Vector3 v = vector2 + new Vector3(0f, 0f, 0f - connectionMiddlePointHeight);
				Vector3 v2 = vector3 + new Vector3(0f, 0f, 0f - connectionMiddle2PointHeight);
				for (int j = 0; j < connectionNodeCount; j++)
				{
					Vector3 rootPoint = GeneralTool.CubicBezierCurve(vector2, v, v2, vector3, (float)j / ((float)connectionNodeCount - 1f));
					connections[num2].SetPosition(j, Tool2D.GetLayerPoint(rootPoint));
				}
				invincibleEFs[num2].transform.position = vector;
			}
			else
			{
				if (flag)
				{
					UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>(invincibleTargets[num2]);
					componentData2.InvincibleUnregister();
					componentData2.CanBeTarget = true;
					SetComponentData(componentData2, invincibleTargets[num2]);
				}
				ObjPoolMgr.Inst.RecycleGO(invincibleEFs[num2].gameObject);
				ObjPoolMgr.Inst.RecycleGO(connections[num2].gameObject);
				invincibleTargets.RemoveAt(num2);
				invincibleEFs.RemoveAt(num2);
				connections.RemoveAt(num2);
			}
		}
		checkIntervalTimer += Time.deltaTime;
		if (!(checkIntervalTimer >= invincibleCheckInterval))
		{
			return;
		}
		checkIntervalTimer = 0f;
		List<Entity> targetableEttList = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList;
		for (int k = 0; k < targetableEttList.Count; k++)
		{
			Entity entity2 = targetableEttList[k];
			UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>(entity2);
			if (Mathf.Abs(componentData3.unitCfg.id - 105321) <= 10 || (componentData3.unitCfg.id - 104801 <= 100 && componentData3.unitCfg.id - 104801 >= 0))
			{
				continue;
			}
			Vector3 vector4 = GetComponentData<LocalTransform>(entity2).Position;
			if (!((base.transform.position - vector4).sqrMagnitude < invincibleRadius * invincibleRadius) || invincibleTargets.Contains(entity2))
			{
				continue;
			}
			componentData3.CanBeTarget = false;
			componentData3.InvincibleRegister();
			SetComponentData(componentData3, entity2);
			GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster48_Invincible", vector4);
			if (UnitDotsSyncSystem.TryGetComponent<PhysicsCollider>(entity2, out var result))
			{
				bool num3 = result.ColliderPtr->Type == ColliderType.Capsule;
				float physicsColliderRadius = DTool.GetPhysicsColliderRadius(in result);
				if (num3)
				{
					gO.transform.localScale = Vector3.one * physicsColliderRadius * 2f * EffectScale;
				}
				LineRenderer component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster48_Connection", vector4).GetComponent<LineRenderer>();
				component.positionCount = connectionNodeCount;
				invincibleTargets.Add(entity2);
				invincibleEFs.Add(gO);
				connections.Add(component);
				component.enabled = false;
			}
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		for (int num = invincibleTargets.Count - 1; num >= 0; num--)
		{
			if (EntityIsValid(invincibleTargets[num]))
			{
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(invincibleTargets[num]);
				componentData.CanBeTarget = true;
				componentData.InvincibleUnregister();
				SetComponentData(componentData, invincibleTargets[num]);
			}
			ObjPoolMgr.Inst.RecycleGO(invincibleEFs[num]);
			ObjPoolMgr.Inst.RecycleGO(connections[num].gameObject);
		}
		invincibleTargets.Clear();
		connections.Clear();
		invincibleEFs.Clear();
		for (int i = 0; i < 10; i++)
		{
			Vector3 point = base.transform.position + new Vector3(0f, 0f, 0f - Mathf.Lerp(0f, connectionStartPointHeight, (float)i / 10f)) + Tool2D.GetDir() * UnityEngine.Random.Range(0f, 0.3f);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Smoke", point, 2f);
		}
	}
}
