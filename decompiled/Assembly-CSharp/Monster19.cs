using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;

public class Monster19 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum MonsterState
	{
		BornIdle,
		MoveToFriend,
		NoFriendMotion,
		InTeam,
		Blast
	}

	[Range(0f, 180f)]
	[Space(50f)]
	public float wiggleAngle;

	public float wiggleSpeed;

	public int teamID;

	public float noFriendRotateSpeed;

	public float blastForce;

	public float blastSpeedToMove;

	public AIPattern pattern;

	[Header("防止一直在深渊上")]
	public VariableFloat relocateInterval;

	public float relocationRange;

	private float relocateTimer;

	[Header("Pattern1")]
	public Sprite[] sprites;

	public MeshRenderer mr;

	private int nowSpriteIndex;

	[Header("Pattern2")]
	public Transform tsf_Rotate;

	public LayerMask laserCheckLayer;

	public LayerMask laserAttackLayer;

	public MeshRenderer mr_Eye;

	public MeshRenderer mr_Eye1;

	public UnityEngine.Material mat_mr_Eye_H;

	public UnityEngine.Material mat_mr_Eye1_H;

	public Monster9Laser laser;

	public Monster9Laser laser_H;

	public float laserForwardOffset;

	public float laserHeight;

	public float laserDamageInterval;

	public int laserDamage;

	[Header("Spell")]
	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public static List<Monster19> mates = new List<Monster19>();

	public MonsterState state;

	private Monster19 nearestFriend;

	private float wiggleCounter;

	private float currentDirValue;

	private bool isAttacking;

	private bool isLaserDamageStart;

	private float laserDamageIntervalTimer;

	private SpellSpawnParams ssp;

	private List<StatefulCollisionEvent> collisions = new List<StatefulCollisionEvent>();

	public Entity thisEntity { get; set; }

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Duration = spellDuration;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		navAreaMask = 32;
		if (pattern == AIPattern.Pattern2 && GameMgr.IsHarmony_Static)
		{
			mr_Eye.material = mat_mr_Eye_H;
			mr_Eye1.material = mat_mr_Eye1_H;
			laser = laser_H;
		}
	}

	public override void EveryInitialCallback()
	{
		mates.Add(this);
		state = MonsterState.BornIdle;
		nearestFriend = null;
		wiggleCounter = 0f;
		currentDirValue = 0f;
		base.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		if (pattern == AIPattern.Pattern1)
		{
			base.Anima.SetTrigger("Idle");
		}
		else if (pattern == AIPattern.Pattern2)
		{
			base.Anima.SetTrigger("Idle2");
			isAttacking = false;
			isLaserDamageStart = false;
			laserDamageIntervalTimer = 0f;
			laser.StopImmediately();
		}
		collisions.Clear();
	}

	public override void Update()
	{
		if (currentDirValue > 360f)
		{
			currentDirValue %= 360f;
		}
		else if (currentDirValue < 0f)
		{
			currentDirValue += 360f;
		}
		if (pattern == AIPattern.Pattern1)
		{
			int num = (int)((float)sprites.Length * (currentDirValue / 360f));
			if (num == sprites.Length)
			{
				num--;
			}
			if (nowSpriteIndex != num)
			{
				nowSpriteIndex = num;
				mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[num].texture);
			}
		}
		else if (pattern == AIPattern.Pattern2)
		{
			tsf_Rotate.localRotation = Quaternion.Euler(0f, 0f - currentDirValue, 0f);
			if (isAttacking)
			{
				laserDamageIntervalTimer += Time.deltaTime;
				Vector3 origin = base.transform.position + Tool2D.GetDir(currentDirValue) * laserForwardOffset + new Vector3(0f, 0f, 0f - laserHeight);
				UnityEngine.Ray ray = new UnityEngine.Ray(origin, Tool2D.GetDir(currentDirValue));
				UnitDotsSyncSystem.RayCastHitResult result;
				if (isLaserDamageStart)
				{
					Vector3 point;
					if (!UnitDotsSyncSystem.Raycast(ray, 100f, GameConst.Filter_Laser, out result))
					{
						point = ((!UnitDotsSyncSystem.Raycast(ray, 100f, GameConst.Filter_Wall, out result)) ? (ray.origin + ray.direction * 100f) : result.point);
					}
					else
					{
						point = result.point;
						if (laserDamageIntervalTimer >= laserDamageInterval)
						{
							laserDamageIntervalTimer = 0f;
							if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(result.entity))
							{
								TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
								info.damage = laserDamage;
								UnitDotsSyncSystem.AddTakeDamageRequest(result.entity, info);
							}
						}
					}
					laser.SetLaser(ray.origin, point);
				}
				else
				{
					Vector3 point = ((!UnitDotsSyncSystem.Raycast(ray, 100f, GameConst.Filter_Wall, out result)) ? (ray.origin + ray.direction * 100f) : result.point);
					laser.SetWarning(ray.origin, point);
				}
			}
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				GetNearestFriend();
				if (nearestFriend != null)
				{
					state = MonsterState.MoveToFriend;
					break;
				}
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position));
				state = MonsterState.NoFriendMotion;
			}
			break;
		case MonsterState.MoveToFriend:
			if (nearestFriend.gameObject.activeSelf)
			{
				wiggleCounter += wiggleSpeed * Time.deltaTime;
				Vector3 oldDir = ToPointDir(nearestFriend.transform.position);
				oldDir = Tool2D.GetDir(oldDir, Mathf.Sin(wiggleCounter) * wiggleAngle / 2f);
				SetMove(oldDir * base.MoveSpeed);
				currentDirValue = Mathf.MoveTowards(currentDirValue, ToPointDegree(nearestFriend.transform.position), noFriendRotateSpeed * Time.deltaTime);
			}
			else
			{
				GetNearestFriend();
				if (nearestFriend != null)
				{
					state = MonsterState.MoveToFriend;
					break;
				}
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position));
				state = MonsterState.NoFriendMotion;
			}
			break;
		case MonsterState.NoFriendMotion:
			if (!navInfo.allCornerArrived)
			{
				currentDirValue += Time.deltaTime * noFriendRotateSpeed;
				CheckNavInfo();
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				currentDirValue += Time.deltaTime * noFriendRotateSpeed;
				SetMove(Tool2D.GetDir(currentDirValue) * base.MoveSpeed);
			}
			relocateTimer += Time.deltaTime;
			if (relocateTimer > relocateInterval.result)
			{
				relocateTimer = 0f;
				relocateInterval.RandomResult();
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, relocationRange));
				checkTargetIntervalTimer = 0f;
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				GetNearestFriend();
				if (nearestFriend != null)
				{
					state = MonsterState.MoveToFriend;
				}
			}
			break;
		case MonsterState.InTeam:
			SetMove(Vector3.zero);
			break;
		case MonsterState.Blast:
			SetMove(Vector3.zero);
			if (((Vector3)GetComponentData<PhysicsVelocity>().Linear).sqrMagnitude < blastSpeedToMove * blastSpeedToMove)
			{
				GetNearestFriend();
				if (nearestFriend != null)
				{
					state = MonsterState.MoveToFriend;
					break;
				}
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position));
				state = MonsterState.NoFriendMotion;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void GetNearestFriend()
	{
		nearestFriend = null;
		for (int num = mates.Count - 1; num >= 0; num--)
		{
			if (mates[num] == null || !mates[num].gameObject.activeSelf)
			{
				mates.RemoveAt(num);
			}
		}
		float num2 = 9999999f;
		for (int i = 0; i < mates.Count; i++)
		{
			if (mates[i].pattern != pattern || mates[i] == this)
			{
				continue;
			}
			if (nearestFriend == null)
			{
				nearestFriend = mates[i];
				continue;
			}
			float sqrMagnitude = (base.transform.position - mates[i].transform.position).sqrMagnitude;
			if (sqrMagnitude < num2)
			{
				nearestFriend = mates[i];
				num2 = sqrMagnitude;
			}
		}
	}

	public void LateUpdate()
	{
		if (pattern == AIPattern.Pattern2)
		{
			for (int i = 0; i < myPpt.MR_Models.Length; i++)
			{
				myPpt.MR_Models[i].material.SetColor(GameConstManaged.shaderColorIndex, myPpt.BaseColor);
			}
		}
		if (collisions.Count <= 0)
		{
			return;
		}
		for (int j = 0; j < collisions.Count; j++)
		{
			StatefulCollisionEvent statefulCollisionEvent = collisions[j];
			if (state == MonsterState.Blast || state == MonsterState.InTeam || !base.CC_Self.enabled)
			{
				break;
			}
			Entity otherEntity = statefulCollisionEvent.GetOtherEntity(myPpt.myEntity);
			if (EntityIsValid(otherEntity) && UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(otherEntity))
			{
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(otherEntity);
				if (componentData.unitCfg.id == myPpt.unitCfg.id)
				{
					Monster19_Team component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + teamID, base.transform.position).GetComponent<Monster19_Team>();
					EnterTeam(component);
					(GetComponentObject<UnitPptReference>(otherEntity).unitPpt.UnitBas as Monster19).EnterTeam(component);
				}
				else if (componentData.unitCfg.id == myPpt.unitCfg.id + 20)
				{
					EnterTeam(GetComponentObject<UnitPptReference>(otherEntity).unitPpt.UnitBas as Monster19_Team);
				}
			}
		}
		collisions.Clear();
	}

	public void OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		collisions.Add(collision);
	}

	public void EnterTeam(Monster19_Team team)
	{
		if (state != MonsterState.InTeam && team.MonsterEnter(this))
		{
			state = MonsterState.InTeam;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanBeTarget = false;
			SetComponentData(componentData);
			base.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			base.Rigid.linearVelocity = Vector3.zero;
			PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>();
			componentData2.Linear = Vector3.zero;
			SetComponentData(componentData2);
		}
	}

	public void Attack()
	{
		switch (pattern)
		{
		case AIPattern.Pattern1:
			base.Anima.SetTrigger("Attack");
			break;
		case AIPattern.Pattern2:
			if (!isAttacking)
			{
				isAttacking = true;
				base.Anima.Play("Monster19_Attack2Before");
			}
			break;
		default:
			Debug.LogError(pattern);
			break;
		}
	}

	public void SetBlast()
	{
		state = MonsterState.Blast;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanBeTarget = true;
		SetComponentData(componentData);
		PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>();
		componentData2.Linear += (float3)componentData.unitCfg.knockbackRatio * (float3)Tool2D.GetDir(currentDirValue) * blastForce;
		SetComponentData(componentData2);
		base.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		if (pattern == AIPattern.Pattern1)
		{
			base.Anima.SetTrigger("Idle");
		}
		else if (pattern == AIPattern.Pattern2 && isAttacking)
		{
			isAttacking = false;
			isLaserDamageStart = false;
			laser.Stop();
			base.Anima.Play("Monster19_Idle2");
		}
	}

	public void SetPoint(Vector3 point, float dirValue)
	{
		base.transform.position = point;
		LocalTransform componentData = GetComponentData<LocalTransform>();
		componentData.Position = base.transform.position;
		SetComponentData(componentData);
		currentDirValue = dirValue;
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (state == MonsterState.InTeam)
		{
			info.immuneDamage = true;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		mates.Remove(this);
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Attack"))
		{
			if (animaName == "Attack2BeforeFinish")
			{
				base.Anima.Play("Monster19_Attacking2");
				isLaserDamageStart = true;
			}
			else
			{
				Debug.LogError(animaName);
			}
		}
		else
		{
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.Direction = Tool2D.GetDir(currentDirValue);
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
	}

	public void OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	public void OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}
}
