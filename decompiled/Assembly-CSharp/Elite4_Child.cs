using UnityEngine;

public class Elite4_Child : UnitBase
{
	private enum MonsterState
	{
		Hatch,
		Idle,
		FollowTarget
	}

	[Space(50f)]
	public MeshRenderer mr;

	public Sprite sprite_Egg;

	public Sprite sprite_Adult;

	public float hatchHPRatio;

	public float hatchTime;

	public float hatchMaxSpeed;

	public string hatchEFStr;

	public string hatchSEStr;

	private MonsterState state;

	private float hatchTimer;

	public override void EveryInitialCallback()
	{
		state = MonsterState.Hatch;
		hatchTimer = 0f;
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Egg.texture);
		myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP * hatchHPRatio;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.unitCfg.currentHP = myPpt.unitCfg.maxHP * hatchHPRatio;
		componentData.ImmuneKnockbackRegister();
		SetComponentData(componentData);
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
		case MonsterState.Hatch:
			hatchTimer += Time.deltaTime;
			base.Anima.SetFloat("HatchSpeed", Mathf.Lerp(1f, hatchMaxSpeed, hatchTimer / hatchTime));
			if (hatchTimer >= hatchTime)
			{
				state = MonsterState.FollowTarget;
				mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Adult.texture);
				myPpt.unitCfg.currentHP = myPpt.unitCfg.currentHP + myPpt.unitCfg.maxHP * (1f - hatchHPRatio);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.unitCfg.currentHP = myPpt.unitCfg.maxHP * hatchHPRatio;
				componentData.ImmuneKnockbackUnregister();
				SetComponentData(componentData);
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.FollowTarget;
					base.Anima.SetTrigger("Crawl");
				}
				else
				{
					state = MonsterState.Idle;
					base.Anima.SetTrigger("Idle");
				}
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + hatchEFStr, base.transform.position, 2f);
				SEMgr.Inst.PlaySE(hatchSEStr);
			}
			break;
		case MonsterState.Idle:
			SetMove(Vector3.zero, isFlip: false);
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.FollowTarget;
					base.Anima.SetTrigger("Crawl");
				}
			}
			break;
		case MonsterState.FollowTarget:
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				base.Anima.SetTrigger("Idle");
				break;
			}
			GetNavInfo(base.TargetPoint);
			if (ToTargetDistanceSqr() > 0.040000003f)
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}
}
