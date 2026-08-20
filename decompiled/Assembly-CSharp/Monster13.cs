using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

public class Monster13 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	private enum MonsterState
	{
		BornIdle,
		Crawl,
		JumpBefore,
		Jumping
	}

	public VariableFloat crawlTime;

	public float toWallSpeedRatio;

	public Transform tsf_Rotate;

	public Transform tsf_Shadow;

	public Transform tsf_Jump;

	public Transform tsf_Motion;

	public float rotateSpeed;

	public float rayExtraDistance;

	public float rayBackAngle;

	public float jumpForceDontCheckWallTime;

	[Header("Jump")]
	public float jumpForce;

	public AIPattern pattern;

	[Header("Pattern2 Pattern3")]
	public float spellDistance;

	[Header("Spell")]
	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	private SpellSpawnParams ssp;

	private float selfRadius;

	private Vector3 shadowOriginalLocalPoint;

	private MonsterState state;

	private Vector3 lastPoint;

	private Vector3 normalDir;

	private float crawlTimer;

	private float lastPointTimer;

	private float jumpForceDontCheckWallTimer;

	private Vector3 lastSpellPoint;

	public CollisionFilter Filter_Wall = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073741824u,
		CollidesWith = 256u
	};

	public Entity thisEntity { get; set; }

	public override void SingleInitialCallback()
	{
		selfRadius = base.CC_Self.radius * base.transform.localScale.x;
		shadowOriginalLocalPoint = tsf_Shadow.localPosition;
		if (GameMgr.IsMobile_Static)
		{
			jumpForce *= 0.8f;
			spellDuration *= 0.8f;
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public unsafe override void EveryInitialCallback()
	{
		crawlTime.RandomResult();
		state = MonsterState.BornIdle;
		crawlTimer = 0f;
		lastPointTimer = 0f;
		jumpForceDontCheckWallTimer = 0f;
		tsf_Rotate.SetParent(tsf_Motion);
		tsf_Rotate.localPosition = Vector3.zero;
		tsf_Rotate.localScale = Vector3.one;
		tsf_Shadow.SetParent(tsf_Motion);
		tsf_Shadow.localPosition = shadowOriginalLocalPoint;
		tsf_Shadow.localScale = Vector3.one;
		lastPoint = base.transform.position;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.IsVelocityDeclice = false;
		SetComponentData(componentData);
		PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
		CollisionFilter collisionFilter = componentData2.ColliderPtr->GetCollisionFilter();
		collisionFilter.CollidesWith ^= 4096u;
		componentData2.ColliderPtr->SetCollisionFilter(collisionFilter);
		SetComponentData(componentData2);
	}

	public override void Update()
	{
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
				state = MonsterState.Crawl;
				if (UnitDotsSyncSystem.Raycast(new UnityEngine.Ray(base.transform.position, -ToPointDir(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint)), 999f, Filter_Wall, out var result))
				{
					base.transform.position = Tool2D.IgnoreZPoint(result.point) + result.normal * selfRadius;
					normalDir = result.normal;
				}
				else
				{
					Debug.LogError("What the fuck,不可能射不中");
				}
			}
			break;
		case MonsterState.Crawl:
		{
			UnityEngine.Ray ray = new UnityEngine.Ray(base.transform.position, -normalDir);
			UnityEngine.Ray ray2 = new UnityEngine.Ray(base.transform.position, Tool2D.GetDir(normalDir, 90f));
			if (UnitDotsSyncSystem.Raycast(ray2, selfRadius + rayExtraDistance, Filter_Wall, out var result2))
			{
				normalDir = result2.normal;
			}
			else if (UnitDotsSyncSystem.Raycast(ray, selfRadius + rayExtraDistance, Filter_Wall, out result2))
			{
				normalDir = result2.normal;
			}
			else
			{
				ray = new UnityEngine.Ray(base.transform.position, Tool2D.GetDir(-normalDir, 0f - rayBackAngle));
				if (UnitDotsSyncSystem.Raycast(ray, selfRadius + rayExtraDistance, Filter_Wall, out result2))
				{
					normalDir = result2.normal;
				}
				else
				{
					ray = new UnityEngine.Ray(base.transform.position, Tool2D.GetDir(-normalDir, rayBackAngle));
					if (UnitDotsSyncSystem.Raycast(ray, selfRadius + rayExtraDistance, Filter_Wall, out result2))
					{
						normalDir = result2.normal;
					}
					else
					{
						crawlTimer = 0f;
						crawlTime.RandomResult();
						SetJump();
					}
				}
			}
			Vector3 vector = Tool2D.GetDir(normalDir, 90f) * base.MoveSpeed;
			vector += -normalDir * base.MoveSpeed * toWallSpeedRatio;
			base.Rigid.linearVelocity = vector;
			PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
			componentData.Linear = vector;
			SetComponentData(componentData);
			Debug.DrawLine(ray2.origin, ray2.origin + ray2.direction * (selfRadius + rayExtraDistance), Color.yellow, 0.01f);
			Debug.DrawLine(ray.origin, ray.origin + ray.direction, Color.blue, 0.01f);
			Debug.DrawLine(base.transform.position, base.transform.position + normalDir, Color.red, 0.01f);
			tsf_Rotate.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
			tsf_Shadow.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
			crawlTimer += Time.deltaTime;
			if (crawlTimer >= crawlTime.result)
			{
				crawlTimer = 0f;
				crawlTime.RandomResult();
				SetJump();
			}
			if (pattern == AIPattern.Pattern3 && (base.transform.position - lastSpellPoint).sqrMagnitude > spellDistance * spellDistance)
			{
				lastSpellPoint = base.transform.position;
				UnitSpellModifier sSPModifier2 = UnitBase.GetSSPModifier(in ssp);
				sSPModifier2.SpawnPosition = base.transform.position;
				sSPModifier2.Direction = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier2.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			break;
		}
		case MonsterState.JumpBefore:
		{
			base.Rigid.linearVelocity = Vector3.zero;
			PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
			componentData.Linear = Vector3.zero;
			SetComponentData(componentData);
			break;
		}
		case MonsterState.Jumping:
		{
			jumpForceDontCheckWallTimer += Time.deltaTime;
			if (Mathf.Abs(base.Rigid.linearVelocity.sqrMagnitude - jumpForce * jumpForce) > 0.010000001f)
			{
				base.Rigid.linearVelocity = normalDir * jumpForce;
			}
			PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
			componentData.Linear = base.Rigid.linearVelocity;
			SetComponentData(componentData);
			if ((pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern3) && (base.transform.position - lastSpellPoint).sqrMagnitude > spellDistance * spellDistance)
			{
				lastSpellPoint = base.transform.position;
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.Direction = Vector3.up;
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		}
		if (state == MonsterState.BornIdle)
		{
			return;
		}
		lastPointTimer += Time.deltaTime;
		if (!(lastPointTimer >= 2f))
		{
			return;
		}
		lastPointTimer = 0f;
		if (base.transform.position == lastPoint)
		{
			crawlTimer = 0f;
			tsf_Rotate.SetParent(tsf_Motion);
			tsf_Rotate.localPosition = Vector3.zero;
			tsf_Rotate.localScale = Vector3.one;
			tsf_Shadow.SetParent(tsf_Motion);
			tsf_Shadow.localPosition = shadowOriginalLocalPoint;
			tsf_Shadow.localScale = Vector3.one;
			for (int i = 0; i < 8; i++)
			{
				if (UnitDotsSyncSystem.Raycast(base.transform.position, Tool2D.GetDir(45 * i), selfRadius + rayExtraDistance, Filter_Wall, out var result3))
				{
					normalDir = result3.normal;
					Vector3 linearVelocity = Tool2D.GetDir(normalDir, 90f) * base.MoveSpeed;
					linearVelocity += -normalDir * base.MoveSpeed * toWallSpeedRatio;
					base.Rigid.linearVelocity = linearVelocity;
					PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>();
					componentData2.Linear = base.Rigid.linearVelocity;
					SetComponentData(componentData2);
					break;
				}
			}
			SetJump();
		}
		else
		{
			lastPoint = base.transform.position;
		}
	}

	private void SetJump()
	{
		tsf_Jump.localPosition = -normalDir * selfRadius;
		tsf_Jump.forward = normalDir;
		tsf_Rotate.SetParent(tsf_Jump);
		tsf_Shadow.SetParent(tsf_Jump);
		state = MonsterState.JumpBefore;
		base.Anima.SetTrigger("Jump");
		base.Rigid.linearVelocity = Vector3.zero;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
	}

	private void OnCollisionEnter(Collision collision)
	{
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "Jump")
		{
			state = MonsterState.Jumping;
			base.Rigid.linearVelocity = normalDir * jumpForce;
			PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
			componentData.Linear = base.Rigid.linearVelocity;
			SetComponentData(componentData);
		}
		else
		{
			Debug.LogError(animaName);
		}
	}

	unsafe void IDotsCollisionReceiver.OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		if (jumpForceDontCheckWallTimer >= jumpForceDontCheckWallTime)
		{
			uint belongsTo = GetComponentData<PhysicsCollider>(collision.GetOtherEntity(myPpt.myEntity)).ColliderPtr->GetCollisionFilter().BelongsTo;
			bool flag = belongsTo == 256;
			bool flag2 = belongsTo == 131072;
			if ((flag || flag2) && (state == MonsterState.JumpBefore || state == MonsterState.Jumping))
			{
				jumpForceDontCheckWallTimer = 0f;
				state = MonsterState.Crawl;
				base.Rigid.linearVelocity = Vector3.zero;
				PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
				componentData.Linear = base.Rigid.linearVelocity;
				SetComponentData(componentData);
				tsf_Rotate.SetParent(tsf_Motion);
				tsf_Rotate.localPosition = Vector3.zero;
				tsf_Rotate.localScale = Vector3.one;
				tsf_Shadow.SetParent(tsf_Motion);
				tsf_Shadow.localPosition = shadowOriginalLocalPoint;
				tsf_Shadow.localScale = Vector3.one;
				normalDir = -collision.GetNormalFrom(myPpt.myEntity);
			}
		}
	}

	void IDotsCollisionReceiver.OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	void IDotsCollisionReceiver.OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}
}
