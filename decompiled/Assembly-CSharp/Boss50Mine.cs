using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss50Mine : MonoBehaviour
{
	public enum MineState
	{
		Flying,
		Landed,
		Triggered,
		Exploded
	}

	public MineState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("表现")]
	public float gravity;

	public float flyTime;

	public Transform tsf_Layer;

	public SpriteRenderer sr_BulletHead;

	public Sprite spriteFly;

	public Sprite spriteDeploy;

	public Shadow shadow;

	public ParticleSystem trailParticle;

	public ParticleSystem deployParticle;

	public ParticleSystem fadeParticle;

	public VariableFloat startRotation;

	public float shakeFrequency;

	public float shakeAmplitude;

	[Header("Mine")]
	public float detectRadius;

	public float triggerDelay;

	public float maxLandExistTime;

	[Header("数值")]
	public float damage;

	public float knockBack;

	public float damageRadius;

	public ShockParam shockParam;

	private Vector3 startPoint;

	private Vector3 endPoint;

	private Vector3 originLayerLocalPosition;

	private Vector3 horizontalDirection;

	private float horizontalSpeed;

	private float currentUpSpeed;

	private Vector2 berlinSeed;

	private bool stageClearRecycle;

	private float triggerTimer;

	private WarningArea warning;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public MineState state
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
		}
	}

	public void Initialize(Vector3 endPoint)
	{
		sr_BulletHead.enabled = true;
		originLayerLocalPosition = tsf_Layer.localPosition;
		startPoint = base.transform.position;
		this.endPoint = Tool2D.IgnoreZPoint(endPoint);
		Vector3 vector = Tool2D.IgnoreZPoint(this.endPoint - startPoint);
		horizontalDirection = vector.normalized;
		horizontalSpeed = vector.magnitude / flyTime;
		currentUpSpeed = GeneralTool.CannonInitialSpeed(startPoint.z - this.endPoint.z, gravity, flyTime);
		trailParticle.Play();
		startRotation.RandomResult();
		startRotation.result *= GeneralTool.HalfChanceNPOne();
		berlinSeed = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
		shadow.Show();
		state = MineState.Flying;
		stageClearRecycle = false;
		EventMgr.EndlessStageClear = (Action)Delegate.Combine(EventMgr.EndlessStageClear, new Action(Recycle));
		sr_BulletHead.sprite = spriteFly;
	}

	private void OnDisable()
	{
		EventMgr.EndlessStageClear = (Action)Delegate.Remove(EventMgr.EndlessStageClear, new Action(Recycle));
		tsf_Layer.localPosition = originLayerLocalPosition;
	}

	private void Recycle()
	{
		stageClearRecycle = true;
	}

	private void Update()
	{
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
		case MineState.Flying:
		{
			currentUpSpeed += gravity * Time.deltaTime;
			Vector3 vector2 = horizontalDirection * horizontalSpeed + Vector3.back * currentUpSpeed;
			base.transform.position += vector2 * Time.deltaTime;
			tsf_Layer.position = Tool2D.GetLayerPoint(base.transform.position);
			Vector3 vector3 = vector2;
			vector3.y -= vector3.z;
			vector3.z = 0f;
			tsf_Layer.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(startRotation.result, 0f, stateExistTime / flyTime));
			if (stateExistTime > flyTime)
			{
				state = MineState.Landed;
			}
			break;
		}
		case MineState.Landed:
			if (changedState)
			{
				sr_BulletHead.sprite = spriteDeploy;
				base.transform.position = endPoint;
				tsf_Layer.position = Tool2D.GetLayerPoint(base.transform.position);
				tsf_Layer.eulerAngles = Vector3.zero;
				deployParticle.Play();
				trailParticle.Stop();
				SEMgr.Inst.monster310_Jump.PlaySE();
				if (Tool2D.GetNavMeshPointIngoreZ(endPoint) != endPoint)
				{
					state = MineState.Exploded;
					stageClearRecycle = true;
				}
			}
			if (stageClearRecycle)
			{
				state = MineState.Exploded;
			}
			if (stateExistTime > maxLandExistTime || HasTriggerTarget())
			{
				if (stateExistTime < maxLandExistTime)
				{
					SEMgr.Inst.boss13MineTrigger.PlaySE();
				}
				state = MineState.Triggered;
			}
			break;
		case MineState.Triggered:
		{
			if (changedState)
			{
				originLayerLocalPosition = tsf_Layer.localPosition;
				warning = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle", base.transform.position).GetComponent<WarningArea>();
				warning.Initialize(damageRadius, triggerDelay);
			}
			Vector2 vector = berlinSeed * stateExistTime * shakeFrequency;
			float x = Mathf.PerlinNoise(vector.x, vector.y) - 0.5f;
			float y = Mathf.PerlinNoise(vector.y, vector.x) - 0.5f;
			tsf_Layer.localPosition = originLayerLocalPosition + new Vector3(x, y, 0f) * shakeAmplitude * stateExistTime / triggerDelay;
			if (stateExistTime > triggerDelay || stageClearRecycle)
			{
				ObjPoolMgr.Inst.RecycleGO(warning.gameObject);
				state = MineState.Exploded;
			}
			break;
		}
		case MineState.Exploded:
			if (changedState)
			{
				deployParticle.Stop();
				shadow.Hide();
				if (stageClearRecycle)
				{
					fadeParticle.Play();
					SEMgr.Inst.endlessMonsterDead.PlaySE(base.transform.position);
				}
				else
				{
					DealDamage();
				}
				ObjPoolMgr.Inst.RecycleGO(base.gameObject, 2f);
				sr_BulletHead.enabled = false;
			}
			break;
		}
	}

	private bool HasTriggerTarget()
	{
		float radius = ((detectRadius > 0f) ? detectRadius : damageRadius);
		triggerTimer += Time.deltaTime;
		if (triggerTimer > 0.1f)
		{
			triggerTimer -= 0.1f;
			distanceHits.Clear();
			UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, radius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
			for (int i = 0; i < distanceHits.Count; i++)
			{
				uint layer = UnitDotsSyncSystem.GetLayer(distanceHits[i].entity);
				if (layer == 512 || layer == 2097152)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void DealDamage()
	{
		CamController.Inst.SetShock(shockParam);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss50Explosion", base.transform.position, Quaternion.identity, Vector3.one * 1.67f, 3f);
		SEMgr.Inst.monster34Explosion.PlaySE();
		distanceHits.Clear();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, damageRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			Entity entity = distanceHits[i].entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss50.Inst.myPpt.myEntity);
			switch (layer)
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 2097152u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, base.transform.position) * knockBack;
				info.damage = damage;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
				break;
			case 131072u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, base.transform.position) * knockBack;
				info.damage = damage * 9999f;
				info.ignoreFloatText = true;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
				break;
			}
		}
	}
}
