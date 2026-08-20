using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster12 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		CloneCooling,
		CloneReady,
		Clone,
		CloneWait,
		Flying,
		Landing,
		Crawl
	}

	[Space(50f)]
	public Shadow shadow;

	public VariableFloat bodySize;

	public Transform tsf_Scale;

	public MeshRenderer mr;

	public Sprite sprite_Normal;

	public Sprite sprite_Attack;

	public Sprite sprite_Crawl;

	public float cloneShootDistance;

	public float cloneOffset;

	public float cloneHeight;

	public float cloneUpSpeed;

	public float cloneGravity;

	[Range(0f, 1f)]
	public float cloneHPRatio;

	[Range(0f, 1f)]
	public float cloneSelfHPRatio;

	public float cloneRecoil;

	public float cloneWaitTime;

	[Range(0f, 1f)]
	[Header("Crawl")]
	public float crawlHPRatio;

	public float crawlInterval;

	public float crwalDrag;

	private float originalCCRadius;

	private MonsterState state;

	private Vector3 clonePoint;

	private float cloneWaitTimer;

	public override void SingleInitialCallback()
	{
		originalCCRadius = base.CC_Self.radius;
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		cloneWaitTimer = 0f;
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
		UpdateSize();
		base.Anima.Play("Monster12_Idle");
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
				state = MonsterState.CloneCooling;
				base.Anima.SetTrigger("CloneCooling");
			}
			break;
		case MonsterState.CloneCooling:
			SetMove(Vector3.zero);
			break;
		case MonsterState.CloneReady:
			SetMove(Vector3.zero);
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget && ToTargetDistanceSqr() < cloneShootDistance * cloneShootDistance)
				{
					state = MonsterState.Clone;
					base.Anima.SetTrigger("Clone");
					clonePoint = Tool2D.IgnoreZPoint(base.TargetPoint);
				}
			}
			break;
		case MonsterState.Clone:
			SetMove(Vector3.zero);
			break;
		case MonsterState.CloneWait:
			SetMove(Vector3.zero);
			cloneWaitTimer += Time.deltaTime;
			if (cloneWaitTimer >= cloneWaitTime)
			{
				cloneWaitTimer = 0f;
				state = MonsterState.CloneCooling;
				base.Anima.SetTrigger("CloneCooling");
			}
			break;
		case MonsterState.Flying:
			if (base.transform.position.z > 0f)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				LocalTransform componentData = GetComponentData<LocalTransform>();
				componentData.Position = base.transform.position;
				SetComponentData(componentData);
				state = MonsterState.Landing;
				base.Anima.SetTrigger("Landing");
				JumpStop_Dots();
				if (base.CurrentHPRatio >= crawlHPRatio)
				{
					mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
				}
				SEMgr.Inst.monster12Land.PlaySE();
			}
			break;
		case MonsterState.Landing:
			SetMove(Vector3.zero);
			break;
		case MonsterState.Crawl:
			SetMove(Vector3.zero);
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private unsafe void UpdateSize()
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		float t = componentData.unitCfg.currentHP / componentData.unitCfg.maxHP;
		float num = Mathf.Lerp(bodySize.value1, bodySize.value2, t);
		tsf_Scale.localScale = Vector3.one * num;
		base.CC_Self.radius = originalCCRadius * num;
		PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
		Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)componentData2.ColliderPtr;
		CapsuleGeometry geometry = colliderPtr->Geometry;
		geometry.Radius = base.CC_Self.radius;
		colliderPtr->Geometry = geometry;
		SetComponentData(componentData2);
		if (state != MonsterState.Flying && state != MonsterState.Crawl && base.CurrentHPRatio < crawlHPRatio)
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Crawl.texture);
			state = MonsterState.Crawl;
			base.Anima.SetTrigger("Crawl");
		}
		shadow.CreateShadow();
		shadow.SetScale(base.CC_Self.radius * 2f);
	}

	public void SetFly(Vector3 landPoint)
	{
		state = MonsterState.Flying;
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Crawl.texture);
		base.Anima.Play("Monster12_Fly");
		base.Rigid.linearVelocity = ToPointDir(landPoint) * GeneralTool.CannonSpeed(cloneUpSpeed, 0f - base.transform.position.z, cloneGravity, Tool2D.IgnoreZDistance(base.transform.position, landPoint));
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
		JumpStart_Dots(cloneUpSpeed, cloneGravity);
		UpdateSize();
	}

	public unsafe override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "CoolingFinish":
			base.Anima.SetTrigger("CloneReady");
			state = MonsterState.CloneReady;
			break;
		case "Clone":
			if (base.HaveTarget)
			{
				Vector3 vector = base.transform.position + ToTargetDir() * cloneOffset;
				vector.z = 0f - cloneHeight;
				Monster12 component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + myPpt.unitCfg.id, vector).GetComponent<Monster12>();
				LocalTransform componentData2 = component.GetComponentData<LocalTransform>();
				componentData2.Position = vector;
				component.SetComponentData(componentData2);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				UnitProperty_Dots componentData3 = component.GetComponentData<UnitProperty_Dots>();
				componentData3.unitCfg.currentHP = componentData.unitCfg.currentHP * cloneHPRatio;
				component.SetComponentData(componentData3);
				component.SetFly(clonePoint);
				componentData.unitCfg.currentHP *= cloneSelfHPRatio;
				componentData.TakeKnockback(-ToPointDir(clonePoint) * cloneRecoil);
				SetComponentData(componentData);
				mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Attack.texture);
				float t = componentData.unitCfg.currentHP / componentData.unitCfg.maxHP;
				float num = Mathf.Lerp(bodySize.value1, bodySize.value2, t);
				tsf_Scale.localScale = Vector3.one * num;
				base.CC_Self.radius = originalCCRadius * num;
				PhysicsCollider componentData4 = GetComponentData<PhysicsCollider>();
				Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)componentData4.ColliderPtr;
				CapsuleGeometry geometry = colliderPtr->Geometry;
				geometry.Radius = base.CC_Self.radius;
				colliderPtr->Geometry = geometry;
				SetComponentData(componentData4);
				shadow.CreateShadow();
				shadow.SetScale(base.CC_Self.radius * 2f);
				SEMgr.Inst.monster12Split.PlaySE();
			}
			break;
		case "CloneFinish":
			if (base.CurrentHPRatio < crawlHPRatio)
			{
				mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Crawl.texture);
				state = MonsterState.Crawl;
				base.Anima.SetTrigger("Crawl");
			}
			else
			{
				state = MonsterState.CloneWait;
				mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
			}
			break;
		case "LandingFinish":
			if (base.CurrentHPRatio < crawlHPRatio)
			{
				state = MonsterState.Crawl;
				base.Anima.SetTrigger("Crawl");
			}
			else
			{
				state = MonsterState.CloneCooling;
				base.Anima.SetTrigger("CloneCooling");
			}
			break;
		case "CrawlAddForce":
		{
			GetNearestTarget();
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			if (base.HaveTarget)
			{
				componentData.TakeKnockback(ToTargetDir() * base.MoveSpeed / componentData.unitCfg.knockbackRatio);
			}
			else
			{
				componentData.TakeKnockback(Tool2D.GetDir() * base.MoveSpeed / componentData.unitCfg.knockbackRatio);
			}
			SetComponentData(componentData);
			break;
		}
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		UpdateSize();
	}
}
