using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite13_LightningChain : MonoBehaviour
{
	public enum lightningState
	{
		Waiting,
		Attacking,
		AfterAttack
	}

	[Header("粒子表现")]
	public List<LineRenderer> lr_Attack = new List<LineRenderer>();

	public List<LineRenderer> lr_Shadow = new List<LineRenderer>();

	public List<ParticleSystem> warningParticle = new List<ParticleSystem>();

	public List<ParticleSystem> attackParticle = new List<ParticleSystem>();

	public List<ParticleSystem> afterAttackParticle = new List<ParticleSystem>();

	public float defaultParticleWidth;

	[Header("线表现")]
	public float lineSplitLength;

	public float lineVerticalBaseWidth;

	public float lineHorizontalOffsetRange;

	public float lineVerticalOffsetRange;

	public float lineUpdateInterval;

	private float distancePerLinePoint;

	private int pointCount;

	private Vector3 startPoint;

	private Vector3 endPoint;

	private Vector3 middlePoint;

	private Vector3 dir;

	private Vector3 dirVertical;

	private float distance;

	[Header("震个屏")]
	public ShockParam shock;

	[Header("时间和伤害相关")]
	public int damage;

	public float height;

	public LayerMask attackMask;

	public float damageRadius;

	public float damageCheckInterval;

	private float damageCheckTimer;

	public float waitTime;

	public float damageTime;

	public float afterTime;

	private List<Entity> attackedEntities = new List<Entity>();

	private List<float> attackedTime = new List<float>();

	[Header("状态")]
	public StateVariableMgr varMgr = new StateVariableMgr();

	public lightningState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public lightningState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
			varMgr.Clear();
		}
	}

	public void Initialize(Vector3 startPoint, Vector3 endPoint)
	{
		this.startPoint = startPoint;
		this.endPoint = endPoint;
		middlePoint = (startPoint + endPoint) / 2f;
		distance = (endPoint - startPoint).magnitude;
		dir = (endPoint - startPoint).normalized;
		dirVertical = Tool2D.GetDir(dir, 90f);
		SetEffectsBase();
		state = lightningState.Waiting;
		attackedEntities.Clear();
	}

	private void SetEffectsBase()
	{
		Vector3 layerPoint = Tool2D.GetLayerPoint(middlePoint + new Vector3(0f, 0f, 0f - height));
		Vector3 layerPoint2 = Tool2D.GetLayerPoint(middlePoint);
		Vector3 localEulerAngles = new Vector3(0f, 0f, -90f + Tool2D.IgnoreZAngleWithSign(Vector3.up, endPoint - startPoint));
		for (int i = 0; i < warningParticle.Count; i++)
		{
			warningParticle[i].transform.position = layerPoint2;
			warningParticle[i].transform.localEulerAngles = localEulerAngles;
			ParticleSystem.ShapeModule shape = warningParticle[i].shape;
			shape.radius = distance / 2f;
			ParticleSystem.EmissionModule emission = warningParticle[i].emission;
			emission.rateOverTimeMultiplier = emission.rateOverTimeMultiplier * distance / defaultParticleWidth;
		}
		for (int j = 0; j < attackParticle.Count; j++)
		{
			attackParticle[j].transform.position = layerPoint;
			attackParticle[j].transform.localEulerAngles = localEulerAngles;
			ParticleSystem.ShapeModule shape2 = attackParticle[j].shape;
			shape2.radius = distance / 2f;
			ParticleSystem.EmissionModule emission2 = attackParticle[j].emission;
			emission2.rateOverTimeMultiplier = emission2.rateOverTimeMultiplier * distance / defaultParticleWidth;
		}
		for (int k = 0; k < afterAttackParticle.Count; k++)
		{
			afterAttackParticle[k].transform.position = layerPoint;
			afterAttackParticle[k].transform.localEulerAngles = localEulerAngles;
			ParticleSystem.ShapeModule shape3 = afterAttackParticle[k].shape;
			shape3.radius = distance / 2f;
			ParticleSystem.EmissionModule emission3 = afterAttackParticle[k].emission;
			emission3.rateOverTimeMultiplier = emission3.rateOverTimeMultiplier * distance / defaultParticleWidth;
		}
		pointCount = Mathf.FloorToInt(distance / lineSplitLength) + 1;
		for (int l = 0; l < lr_Attack.Count; l++)
		{
			lr_Attack[l].positionCount = pointCount;
			lr_Shadow[l].positionCount = pointCount;
			lr_Attack[l].enabled = false;
			lr_Shadow[l].enabled = false;
		}
		distancePerLinePoint = distance / (float)(pointCount - 1);
	}

	private void SetLine()
	{
		for (int i = 0; i < lr_Attack.Count; i++)
		{
			for (int j = 0; j < lr_Attack[i].positionCount; j++)
			{
				Vector3 vector = startPoint + dir * distancePerLinePoint * j;
				if (j != 0 && j != pointCount - 1)
				{
					vector += dirVertical * lineVerticalBaseWidth * Random.Range(0f - lineVerticalOffsetRange, lineVerticalOffsetRange) * 0.5f;
					vector += dir * distancePerLinePoint * Random.Range(0f - lineHorizontalOffsetRange, lineHorizontalOffsetRange);
				}
				lr_Attack[i].SetPosition(j, Tool2D.GetLayerPoint(vector + new Vector3(0f, 0f, 0f - height)));
				lr_Shadow[i].SetPosition(j, Tool2D.GetLayerPoint(vector, LayerCorrectType.Shadow));
			}
		}
	}

	private void Update()
	{
		Debug.DrawLine(startPoint, endPoint);
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case lightningState.Waiting:
			if (changedState)
			{
				for (int num = 0; num < warningParticle.Count; num++)
				{
					warningParticle[num].Play();
				}
			}
			if (stateExistTime > waitTime)
			{
				state = lightningState.Attacking;
			}
			break;
		case lightningState.Attacking:
		{
			ref float reference = ref varMgr.RegFloat(0);
			if (changedState)
			{
				CamController.Inst.SetShock(shock);
				SEMgr.Inst.elite13Impact.PlaySE(SEPlayMode.Replay, 2);
				SetLine();
				for (int l = 0; l < warningParticle.Count; l++)
				{
					warningParticle[l].Stop();
				}
				for (int m = 0; m < lr_Attack.Count; m++)
				{
					lr_Attack[m].enabled = true;
					lr_Shadow[m].enabled = true;
				}
				for (int n = 0; n < attackParticle.Count; n++)
				{
					attackParticle[n].Play();
				}
				DealDamage();
				damageCheckTimer = 0f;
				SetLine();
			}
			reference += Time.deltaTime;
			if (reference > lineUpdateInterval)
			{
				SetLine();
				reference -= Time.deltaTime;
			}
			damageCheckTimer += Time.deltaTime;
			if (damageCheckTimer > damageCheckInterval)
			{
				DealDamage();
			}
			if (stateExistTime > damageTime)
			{
				state = lightningState.AfterAttack;
			}
			break;
		}
		case lightningState.AfterAttack:
			if (changedState)
			{
				for (int i = 0; i < lr_Attack.Count; i++)
				{
					lr_Attack[i].enabled = false;
					lr_Shadow[i].enabled = false;
				}
				for (int j = 0; j < attackParticle.Count; j++)
				{
					attackParticle[j].Stop();
				}
				for (int k = 0; k < afterAttackParticle.Count; k++)
				{
					afterAttackParticle[k].Play();
				}
			}
			if (stateExistTime > afterTime)
			{
				Elite13.MiniPool.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < warningParticle.Count; i++)
		{
			ParticleSystem.EmissionModule emission = warningParticle[i].emission;
			emission.rateOverTimeMultiplier = emission.rateOverTimeMultiplier / distance * defaultParticleWidth;
		}
		for (int j = 0; j < attackParticle.Count; j++)
		{
			ParticleSystem.EmissionModule emission2 = attackParticle[j].emission;
			emission2.rateOverTimeMultiplier = emission2.rateOverTimeMultiplier / distance * defaultParticleWidth;
		}
		for (int k = 0; k < afterAttackParticle.Count; k++)
		{
			ParticleSystem.EmissionModule emission3 = afterAttackParticle[k].emission;
			emission3.rateOverTimeMultiplier = emission3.rateOverTimeMultiplier / distance * defaultParticleWidth;
		}
	}

	private void DealDamage()
	{
		UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(startPoint, (endPoint - startPoint).normalized, damageRadius, distance, GameConst.Filter_MonsterAoeNoSpell);
		for (int i = 0; i < array.Length; i++)
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite13.Inst.myPpt.myEntity);
			info.damage = damage;
			info.teammateTakeDamageRatio = 4f;
			Entity entity = array[i].entity;
			if (!attackedEntities.Contains(entity) && UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(entity, out var result))
			{
				if (result.unitCfg.unitType != UnitType.Brittleness)
				{
					Elite13.MiniPool.GetGO("Prefabs/EF/EF_Elite13_Lightning_Hit" + (GameMgr.IsHarmony_Static ? " H" : ""), array[i].point, 3f);
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				attackedEntities.Add(entity);
			}
		}
	}
}
