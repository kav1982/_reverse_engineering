using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;
using UnityEngine.AI;

public class Elite12_FallRock : MonoBehaviour, IComparable, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum RockState
	{
		Stand,
		ShadowShow,
		Fall
	}

	[Header("状态")]
	public RockState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("影子警告")]
	public Transform tsf_Shadow;

	public VariableFloat shadowWarningTime;

	[Header("下落")]
	public float fallTransparencyTime;

	public Transform tsf_Model;

	public ParticleSystem fallParticle;

	public SpriteRenderer sr_Rock;

	public SpriteRenderer sr_Bottom;

	public List<Sprite> spriteFall;

	public List<Sprite> spriteLand;

	public List<Sprite> spriteBottom;

	private int spriteIndex;

	public float startFallSpeed;

	public float finalFallSpeed;

	public float fallTime;

	private float startHeight;

	[Header("落地伤害")]
	public ParticleSystem dropParticle;

	public ParticleSystem dropParticle_H;

	public float damageRadius;

	public int damage;

	public float knockback;

	public ShockParam shock;

	private bool fallDamaged;

	[Header("落地之后")]
	public UnityEngine.Collider thisCollider;

	public NavMeshObstacle thisObstacle;

	[Header("带刺落石")]
	public float knockbackForce;

	private string warningAreaName;

	private CollisionFilter filter = new CollisionFilter
	{
		BelongsTo = 256u,
		CollidesWith = 256u,
		GroupIndex = 0
	};

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public RockState state
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

	public Entity thisEntity { get; set; }

	public int CompareTo(object obj)
	{
		Vector3 roomCenterPoint = Elite12_1.Inst.roomCenterPoint;
		float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, base.transform.position - roomCenterPoint);
		if (num < 0f)
		{
			num += 360f;
		}
		float num2 = Tool2D.IgnoreZAngleWithSign(Vector3.up, (obj as Elite12_FallRock).transform.position - roomCenterPoint);
		if (num2 < 0f)
		{
			num2 += 360f;
		}
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	public void Initialize(bool isStand, bool hasSpike = false)
	{
		filter.CollidesWith = DTool.GetCollidesWith(256u) | 0x100u;
		if (!isStand)
		{
			state = RockState.ShadowShow;
			fallDamaged = false;
		}
		else
		{
			state = RockState.Stand;
			stateQuit = true;
			fallDamaged = true;
		}
		startHeight = (startFallSpeed + finalFallSpeed) / 2f * fallTime;
		sr_Rock.enabled = false;
		sr_Rock.flipX = GeneralTool.ChanceResult(0.5f);
		sr_Bottom.enabled = false;
		tsf_Shadow.transform.localScale = Vector3.zero;
		spriteIndex = UnityEngine.Random.Range(0, spriteFall.Count);
		thisCollider.enabled = false;
		thisObstacle.enabled = false;
		warningAreaName = "Prefabs/Mixed/WarningArea_Circle";
		if (GameMgr.IsChAge14_Static)
		{
			warningAreaName += " purple";
			dropParticle = dropParticle_H;
		}
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
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
		tsf_Shadow.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		switch (state)
		{
		case RockState.ShadowShow:
			if (changedState)
			{
				shadowWarningTime.RandomResult();
				ObjPoolMgr.Inst.GetGO(warningAreaName, base.transform.position).GetComponent<WarningArea>().Initialize(damageRadius, shadowWarningTime.result + fallTime);
			}
			tsf_Shadow.transform.localScale = Vector3.one * stateExistTime / shadowWarningTime.result;
			if (stateExistTime > shadowWarningTime.result)
			{
				state = RockState.Fall;
			}
			break;
		case RockState.Fall:
		{
			if (changedState)
			{
				float num = (fallTime - stateExistTime) * (Mathf.Lerp(startFallSpeed, finalFallSpeed, stateExistTime / fallTime) + finalFallSpeed) / 2f;
				tsf_Model.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * num);
				fallParticle.Play();
				sr_Rock.enabled = true;
				sr_Rock.sprite = spriteFall[spriteIndex];
			}
			if (stateExistTime > fallTime)
			{
				state = RockState.Stand;
				break;
			}
			sr_Rock.color = new Color(1f, 1f, 1f, stateExistTime / fallTransparencyTime);
			float num2 = (fallTime - stateExistTime) * (Mathf.Lerp(startFallSpeed, finalFallSpeed, stateExistTime / fallTime) + finalFallSpeed) / 2f;
			tsf_Model.position = Tool2D.GetLayerPoint(base.transform.position);
			sr_Rock.transform.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * num2);
			break;
		}
		case RockState.Stand:
			if (changedState)
			{
				sr_Rock.enabled = true;
				sr_Bottom.enabled = true;
				fallParticle.Stop();
				sr_Rock.sprite = spriteLand[spriteIndex];
				sr_Bottom.sprite = spriteBottom[spriteIndex];
				thisCollider.enabled = true;
				thisObstacle.enabled = true;
				tsf_Shadow.transform.localScale = Vector3.zero;
			}
			if (stateExistTime > 0.05f && !fallDamaged)
			{
				fallDamaged = true;
				FallToGround();
			}
			tsf_Model.position = Tool2D.GetLayerPoint(base.transform.position);
			sr_Rock.transform.position = Tool2D.GetLayerPoint(base.transform.position);
			sr_Bottom.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffectLow);
			break;
		}
	}

	public void Die()
	{
		Elite12_1.MiniPool.GetGO("Prefabs/EF/EF_Elite12_PillarDead" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position, 2f);
		Elite12_1.MiniPool.RecycleGO(base.gameObject);
		SEMgr.Inst.monster37_KnockWall.PlaySE(SEPlayMode.Replay, 3, 0.2f);
	}

	public void OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
	}

	public unsafe void OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
		if (UnitDotsSyncSystem.GetComponentData<PhysicsCollider>(collision.GetOtherEntity(thisEntity)).ColliderPtr->GetCollisionFilter().BelongsTo == 256)
		{
			Die();
		}
	}

	public void OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}

	private void OnCollisionStay(Collision collision)
	{
	}

	public void FallToGround()
	{
		CamController.Inst.SetShock(shock);
		SEMgr.Inst.elite12_RockLand.PlaySE(SEPlayMode.Replay, 3, 0.2f);
		dropParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		dropParticle.Play();
		UnitDotsSyncSystem.GetCollidersInRange(Tool2D.IgnoreZPoint(base.transform.position), damageRadius, GameConst.Filter_MonsterEffectBullet, results);
		for (int i = 0; i < results.Count; i++)
		{
			Entity entity = results[i].entity;
			_ = results[i];
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 256u:
			{
				for (int j = 0; j < Elite12_1.Inst.rocks.Count; j++)
				{
					if (Elite12_1.Inst.rocks[j].thisEntity == entity && Elite12_1.Inst.rocks[j] != this)
					{
						Elite12_1.Inst.rocks[j].Die();
					}
				}
				break;
			}
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite12_1.Inst.myPpt.myEntity);
					info.damage = damage;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(results[i].point, base.transform.position) * knockback;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				}
				break;
			}
		}
	}
}
