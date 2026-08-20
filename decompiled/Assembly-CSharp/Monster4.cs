using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster4 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Idle,
		Jump,
		Jumping,
		JumpOnGround,
		OnGroundRise,
		Spit,
		Spit2
	}

	public enum LiquidType
	{
		Mucus,
		Venom,
		Water,
		Fire
	}

	[Space(50f)]
	public Transform tsf_Rotate;

	public MeshRenderer mr;

	public Sprite sprite_FireIdle;

	public Sprite sprite_MucusIdle;

	public Sprite sprite_VenomIdle;

	public Sprite sprite_WaterIdle;

	public Sprite sprite_FireFly;

	public Sprite sprite_MucusFly;

	public Sprite sprite_VenomFly;

	public Sprite sprite_WaterFly;

	public Sprite sprite_FireOnGround;

	public Sprite sprite_MucusOnGround;

	public Sprite sprite_VenomOnGround;

	public Sprite sprite_WaterOnGround;

	public float venomDuration;

	public float rotateSpeed;

	public float onGroundTime;

	public VariableFloat idleTime;

	public float liquidDistance;

	public float landLiquidScale;

	public VariableFloat landSEPitch;

	public VariableFloat recoverSEPitch;

	[Range(0f, 1f)]
	[Header("Jump")]
	public float jumpToTargetChance;

	public VariableFloat jumpRadius;

	public float jumpUpForce;

	public float jumpGravity;

	public float jumpOnGrondTime;

	[Header("Pattern2")]
	public AIPattern pattern;

	public Sprite sprite_FireSpitBefore;

	public Sprite sprite_MucusSpitBefore;

	public Sprite sprite_VenomSpitBefore;

	public Sprite sprite_WaterSpitBefore;

	public Sprite sprite_FireSpit;

	public Sprite sprite_MucusSpit;

	public Sprite sprite_VenomSpit;

	public Sprite sprite_WaterSpit;

	[Range(0f, 1f)]
	public float spitChance;

	public float spitDistance;

	public int spitFireID;

	public int spitMucusID;

	public int spitVenomID;

	public int spitWaterID;

	public float spitHight;

	public float spitOffset;

	public float spitForce;

	public float spitUpForce;

	public float spitGravity;

	public int maxSpitChildCount;

	[Header("Fire")]
	public bool isFire;

	public MeshRenderer mr_Fire;

	public int fireDamage;

	public float fireDamageRadius;

	private float liquidRadius;

	private Sprite originalSprite;

	private Sprite nowSprite;

	private MonsterState state;

	public LiquidType liquidType;

	private float idleTimer;

	private Vector3 lastRecordPoint;

	private int maxSpitChildCounter;

	private float onGroundTimer;

	public override void SingleInitialCallback()
	{
		liquidRadius = base.transform.localScale.x * base.CC_Self.radius / 2f;
		switch (liquidType)
		{
		case LiquidType.Mucus:
			originalSprite = sprite_MucusIdle;
			break;
		case LiquidType.Venom:
			originalSprite = sprite_VenomIdle;
			break;
		case LiquidType.Water:
			originalSprite = sprite_WaterIdle;
			break;
		case LiquidType.Fire:
			originalSprite = sprite_FireIdle;
			myPpt.RemoveMRFromArray(mr_Fire);
			break;
		}
	}

	public override void EveryInitialCallback()
	{
		maxSpitChildCounter = 0;
		state = MonsterState.BornIdle;
		if (originalSprite == sprite_MucusIdle)
		{
			liquidType = LiquidType.Mucus;
		}
		else if (originalSprite == sprite_VenomIdle)
		{
			liquidType = LiquidType.Venom;
		}
		else if (originalSprite == sprite_WaterIdle)
		{
			liquidType = LiquidType.Water;
		}
		else if (originalSprite == sprite_FireIdle)
		{
			liquidType = LiquidType.Fire;
			mr_Fire.material.SetTexture(GameConstManaged.shaderTextureIndex, originalSprite.texture);
		}
		lastRecordPoint = base.transform.position;
		idleTime.RandomResult();
		base.Anima.SetTrigger("Idle");
		tsf_Rotate.rotation = Quaternion.identity;
		myPpt.correctType = LayerCorrectType.Coordinate;
		nowSprite = originalSprite;
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, originalSprite.texture);
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (!isFire && state != MonsterState.Jumping && (lastRecordPoint - base.transform.position).sqrMagnitude >= liquidDistance * liquidDistance)
		{
			switch (liquidType)
			{
			case LiquidType.Mucus:
				MucusSystem.CreateMucus(base.transform.position, lastRecordPoint, liquidRadius);
				break;
			case LiquidType.Venom:
				VenomSystem.CreateVenom(base.transform.position, lastRecordPoint, liquidRadius, venomDuration);
				break;
			case LiquidType.Water:
				WaterSystem.CreateWater(base.transform.position, lastRecordPoint, liquidRadius);
				break;
			default:
				Debug.LogError(liquidType);
				break;
			}
			lastRecordPoint = base.transform.position;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			idleTimer += Time.deltaTime;
			if (!(idleTimer >= idleTime.result))
			{
				break;
			}
			idleTimer = 0f;
			idleTime.RandomResult();
			if (pattern == AIPattern.Pattern2 && Random.value <= spitChance && maxSpitChildCounter < maxSpitChildCount)
			{
				GetNearestTarget();
				if (base.HaveTarget && ToTargetDistanceSqr() < spitDistance * spitDistance)
				{
					state = MonsterState.Spit;
					base.Anima.SetTrigger("Spit");
					CheckTypeChangeSprite();
					maxSpitChildCounter++;
				}
			}
			else
			{
				state = MonsterState.Jump;
				base.Anima.SetTrigger("Jump");
				if (!isFire)
				{
					CreateLiquidCricle();
				}
			}
			break;
		case MonsterState.Jumping:
		{
			tsf_Rotate.Rotate(0f, 0f, (float)((!(base.Rigid.linearVelocity.x > 0f)) ? 1 : (-1)) * rotateSpeed * Time.deltaTime);
			if (!(base.transform.position.z > 0f))
			{
				break;
			}
			base.transform.position = Tool2D.IgnoreZPoint(base.transform);
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			SetComponentData(componentData);
			JumpStop_Dots();
			myPpt.correctType = LayerCorrectType.SlimeOnGround;
			tsf_Rotate.localRotation = Quaternion.identity;
			base.Rigid.linearVelocity = Vector3.zero;
			PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>();
			componentData2.Linear = base.Rigid.linearVelocity;
			SetComponentData(componentData2);
			lastRecordPoint = base.transform.position;
			if (isFire)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster4LandFire", base.transform.position, Vector3.one * fireDamageRadius, 2f);
				List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
				UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, fireDamageRadius, GameConst.Filter_MonsterAoe, list);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
				info.damage = fireDamage;
				for (int i = 0; i < list.Count; i++)
				{
					Entity entity = list[i].entity;
					switch (UnitDotsSyncSystem.GetLayer(entity))
					{
					case 16777216u:
					{
						UnitDotsSyncSystem.ProcessHitSpell(entity, fireDamage, out var _);
						break;
					}
					case 512u:
					case 32768u:
					case 131072u:
					case 2097152u:
						UnitDotsSyncSystem.ProcessHitUnit(list[i].entity, info);
						break;
					}
				}
				SEMgr.Inst.monster4LandFire.PlaySE(base.transform.position).pitch = landSEPitch.RandomResult();
			}
			else
			{
				CollisionFilter collisionFilter = default(CollisionFilter);
				collisionFilter.GroupIndex = 0;
				collisionFilter.BelongsTo = 1073741824u;
				collisionFilter.CollidesWith = 32896u;
				CollisionFilter filter = collisionFilter;
				List<UnitDotsSyncSystem.DistanceHitResult> list2 = new List<UnitDotsSyncSystem.DistanceHitResult>();
				UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, liquidRadius, filter, list2);
				for (int j = 0; j < list2.Count; j++)
				{
					Entity entity2 = list2[j].entity;
					if (UnitDotsSyncSystem.HasComponent<MucusTag>(entity2))
					{
						liquidType = LiquidType.Mucus;
					}
					else if (UnitDotsSyncSystem.HasComponent<VenomTag>(entity2))
					{
						liquidType = LiquidType.Venom;
					}
					else if (UnitDotsSyncSystem.HasComponent<WaterTag>(entity2))
					{
						liquidType = LiquidType.Water;
					}
					else if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(entity2))
					{
						TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
						info2.damage = 10f;
						UnitDotsSyncSystem.AddTakeDamageRequest(entity2, info2);
					}
				}
				CreateLiquidCricle(landLiquidScale);
				SEMgr.Inst.monster4Land.PlaySE(base.transform.position).pitch = landSEPitch.RandomResult();
			}
			state = MonsterState.JumpOnGround;
			base.Rigid.isKinematic = true;
			SyncDotsRigidKindmatic();
			PhysicsVelocity componentData3 = GetComponentData<PhysicsVelocity>();
			componentData3.Linear = Vector3.zero;
			SetComponentData(componentData3);
			CheckTypeChangeSprite();
			break;
		}
		case MonsterState.JumpOnGround:
			onGroundTimer += Time.deltaTime;
			if (onGroundTimer >= onGroundTime)
			{
				onGroundTimer = 0f;
				base.Anima.SetTrigger("OnGroundRise");
				state = MonsterState.OnGroundRise;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case MonsterState.Jump:
		case MonsterState.OnGroundRise:
		case MonsterState.Spit:
		case MonsterState.Spit2:
			break;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (isFire || !(info.spell.Entity != Entity.Null))
		{
			return;
		}
		switch (info.spell.Config.ColorType)
		{
		case SpellColorType.Frozen:
			if (liquidType != LiquidType.Water)
			{
				liquidType = LiquidType.Water;
				CheckTypeChangeSprite();
			}
			break;
		case SpellColorType.Mucus:
			if (liquidType != 0)
			{
				liquidType = LiquidType.Mucus;
				CheckTypeChangeSprite();
			}
			break;
		case SpellColorType.Venom:
			if (liquidType != LiquidType.Venom)
			{
				liquidType = LiquidType.Venom;
				CheckTypeChangeSprite();
			}
			break;
		case SpellColorType.Monster:
			break;
		}
	}

	private void CheckTypeChangeSprite()
	{
		Sprite sprite = null;
		switch (state)
		{
		case MonsterState.BornIdle:
		case MonsterState.Idle:
		case MonsterState.Jump:
		case MonsterState.OnGroundRise:
			switch (liquidType)
			{
			case LiquidType.Mucus:
				sprite = sprite_MucusIdle;
				break;
			case LiquidType.Venom:
				sprite = sprite_VenomIdle;
				break;
			case LiquidType.Water:
				sprite = sprite_WaterIdle;
				break;
			case LiquidType.Fire:
				sprite = sprite_FireIdle;
				break;
			}
			break;
		case MonsterState.Jumping:
			switch (liquidType)
			{
			case LiquidType.Mucus:
				sprite = sprite_MucusFly;
				break;
			case LiquidType.Venom:
				sprite = sprite_VenomFly;
				break;
			case LiquidType.Water:
				sprite = sprite_WaterFly;
				break;
			case LiquidType.Fire:
				sprite = sprite_FireFly;
				break;
			}
			break;
		case MonsterState.JumpOnGround:
			switch (liquidType)
			{
			case LiquidType.Mucus:
				sprite = sprite_MucusOnGround;
				break;
			case LiquidType.Venom:
				sprite = sprite_VenomOnGround;
				break;
			case LiquidType.Water:
				sprite = sprite_WaterOnGround;
				break;
			case LiquidType.Fire:
				sprite = sprite_FireOnGround;
				break;
			default:
				Debug.LogError(liquidType);
				break;
			}
			break;
		case MonsterState.Spit:
			switch (liquidType)
			{
			case LiquidType.Mucus:
				sprite = sprite_MucusSpitBefore;
				break;
			case LiquidType.Venom:
				sprite = sprite_VenomSpitBefore;
				break;
			case LiquidType.Water:
				sprite = sprite_WaterSpitBefore;
				break;
			case LiquidType.Fire:
				sprite = sprite_FireSpitBefore;
				break;
			}
			break;
		case MonsterState.Spit2:
			switch (liquidType)
			{
			case LiquidType.Mucus:
				sprite = sprite_MucusSpit;
				break;
			case LiquidType.Venom:
				sprite = sprite_VenomSpit;
				break;
			case LiquidType.Water:
				sprite = sprite_WaterSpit;
				break;
			case LiquidType.Fire:
				sprite = sprite_FireSpit;
				break;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
		if (nowSprite != sprite && sprite != null)
		{
			nowSprite = sprite;
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, nowSprite.texture);
			if (liquidType == LiquidType.Fire)
			{
				mr_Fire.material.SetTexture(GameConstManaged.shaderTextureIndex, nowSprite.texture);
			}
		}
	}

	private void CreateLiquidCricle(float scale = 1f)
	{
		switch (liquidType)
		{
		case LiquidType.Mucus:
			MucusSystem.CreateMucus(Tool2D.IgnoreZPoint(base.transform), liquidRadius * scale);
			break;
		case LiquidType.Venom:
			VenomSystem.CreateVenom(Tool2D.IgnoreZPoint(base.transform), liquidRadius * scale, venomDuration);
			break;
		case LiquidType.Water:
			WaterSystem.CreateWater(Tool2D.IgnoreZPoint(base.transform), liquidRadius * scale);
			break;
		default:
			Debug.LogError(liquidType);
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "JumpAddForce":
		{
			state = MonsterState.Jumping;
			base.transform.position = Tool2D.IgnoreZPoint(base.transform);
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			SetComponentData(componentData);
			Vector3 zero2 = Vector3.zero;
			if (Random.value <= jumpToTargetChance)
			{
				GetNearestTarget();
				zero2 = ((!base.HaveTarget) ? Tool2D.GetNavMeshPointIngoreZ(base.transform.position, jumpRadius) : Tool2D.GetNavMeshPointIngoreZ(base.transform.position + ToTargetDir() * jumpRadius.RandomResult()));
			}
			else
			{
				zero2 = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, jumpRadius);
			}
			float num2 = GeneralTool.CannonSpeed(jumpUpForce, 0f, jumpGravity, Vector3.Distance(base.transform.position, zero2));
			base.Rigid.linearVelocity = ToPointDir(zero2) * num2;
			PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>();
			componentData2.Linear = base.Rigid.linearVelocity;
			SetComponentData(componentData2);
			JumpStart_Dots(jumpUpForce, jumpGravity);
			CheckTypeChangeSprite();
			break;
		}
		case "Rise":
			CheckTypeChangeSprite();
			SEMgr.Inst.monster4Recover.PlaySE(base.transform.position).pitch = recoverSEPitch.RandomResult();
			break;
		case "RiseFinish":
			state = MonsterState.Idle;
			base.Anima.SetTrigger("Idle");
			CheckTypeChangeSprite();
			base.Rigid.isKinematic = false;
			SyncDotsRigidKindmatic();
			myPpt.correctType = LayerCorrectType.Coordinate;
			break;
		case "SpitShoot":
		{
			int num = 0;
			switch (liquidType)
			{
			case LiquidType.Mucus:
				num = spitMucusID;
				break;
			case LiquidType.Venom:
				num = spitVenomID;
				break;
			case LiquidType.Water:
				num = spitWaterID;
				break;
			case LiquidType.Fire:
				num = spitFireID;
				break;
			default:
				Debug.LogError(liquidType);
				num = spitMucusID;
				break;
			}
			Vector3 position = base.transform.position;
			Vector3 zero = Vector3.zero;
			if (base.HaveTarget)
			{
				position += ToTargetDir() * spitOffset;
				zero = ToTargetDir() * spitForce;
			}
			else
			{
				position += Tool2D.GetDir() * spitOffset;
				zero = position.normalized * spitForce;
			}
			position.z = 0f - spitHight;
			ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + num, position).GetComponent<Monster4>().SpitByMother(zero, spitUpForce, spitGravity);
			state = MonsterState.Spit2;
			CheckTypeChangeSprite();
			SEMgr.Inst.monster4Split.PlaySE();
			break;
		}
		case "SpitFinish":
			state = MonsterState.Idle;
			base.Anima.SetTrigger("Idle");
			CheckTypeChangeSprite();
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public void SpitByMother(Vector3 spitForce, float upForce, float gravity)
	{
		state = MonsterState.Jumping;
		base.Rigid.linearVelocity = spitForce;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
		JumpStart_Dots(upForce, gravity);
		if (nowSprite == sprite_MucusIdle)
		{
			liquidType = LiquidType.Mucus;
		}
		else if (nowSprite == sprite_VenomIdle)
		{
			liquidType = LiquidType.Venom;
		}
		else if (nowSprite == sprite_WaterIdle)
		{
			liquidType = LiquidType.Water;
		}
		CheckTypeChangeSprite();
	}
}
