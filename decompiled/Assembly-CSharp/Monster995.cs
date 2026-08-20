using Unity.Entities;
using UnityEngine;

public class Monster995 : UnitBase
{
	private enum UnitState
	{
		Idle,
		Alert,
		FollowTarget,
		Jump,
		Idle2
	}

	[Space(50f)]
	public ChestType chestType;

	public float alertRadius;

	public float alertInterval;

	public float idleCheckTargetInterval;

	public float bornIdleTime;

	[Header("Sprite")]
	public MeshRenderer mr_Up;

	public MeshRenderer mr_Down;

	public Sprite sprite_DownNormal;

	public Sprite sprite_DownTransform;

	[Header("万圣节万圣节万圣节！")]
	public Sprite sprite_UpNormal_Holloween;

	public Sprite sprite_DownNormal_Holloween;

	public Sprite sprite_DownTransform_Holloween;

	[Header("圣诞节圣诞节圣诞节！")]
	public Sprite sprite_UpNormal_Christmas;

	public Sprite sprite_DownNormal_Christmas;

	public Sprite spirte_DownTranform_Christmas;

	[Header("Jump")]
	public float jumpForwardForce;

	private UnitState state;

	private float altarIntervalTimer;

	private float finalJumpForwardForce;

	public override void SingleInitialCallback()
	{
		if (GameMgr.CampSkinType == CampSkinType.Halloween)
		{
			mr_Up.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_UpNormal_Holloween.texture);
			sprite_DownNormal = sprite_DownNormal_Holloween;
			sprite_DownTransform = sprite_DownTransform_Holloween;
		}
		else if (GameMgr.CampSkinType == CampSkinType.Christmas)
		{
			mr_Up.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_UpNormal_Christmas.texture);
			sprite_DownNormal = sprite_DownNormal_Christmas;
			sprite_DownTransform = spirte_DownTranform_Christmas;
		}
	}

	public override void EveryInitialCallback()
	{
		mr_Down.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_DownNormal.texture);
		mr_Down.transform.localPosition = new Vector3(0f, 0.2f, -0.001f);
		base.Anima.Play("Idle", 0, 0f);
		altarIntervalTimer = 0f;
		state = UnitState.Idle;
		float num = myPpt.unitCfg.maxHP;
		float num2 = myPpt.unitCfg.knockbackRatio;
		finalJumpForwardForce = jumpForwardForce;
		if ((bool)BattleMgr.Inst)
		{
			int num3 = (BattleMgr.Inst.CurrentStage + 1) / 2;
			if (num3 > 5)
			{
				num3 = 5;
			}
			for (int i = 0; i < num3; i++)
			{
				num *= 2f;
				num2 /= 2f;
				finalJumpForwardForce *= 2f;
			}
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.maxHP = num;
			componentData.unitCfg.currentHP = num;
			componentData.unitCfg.knockbackRatio = num2;
			SetComponentData(componentData);
		}
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
		case UnitState.Idle:
			bornIdleTimer += Time.deltaTime;
			if (!(bornIdleTimer > bornIdleTime))
			{
				break;
			}
			SetMove(Vector3.zero, isFlip: false);
			altarIntervalTimer += Time.deltaTime;
			if (altarIntervalTimer >= 0f)
			{
				altarIntervalTimer = 0f;
				GetNearestTarget();
				if (targetEntity != Entity.Null && ToTargetDistanceSqr() <= alertRadius * alertRadius)
				{
					state = UnitState.Alert;
					base.Anima.Play("Alert");
					mr_Down.transform.localPosition = new Vector3(0f, 0.2f, 0.0001f);
					mr_Down.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_DownTransform.texture);
					UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
					componentData.unitCfg.unitType = UnitType.Monster;
					SetComponentData(componentData);
					LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Add(myPpt.myEntity);
					LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Add(myPpt.myEntity);
				}
			}
			break;
		case UnitState.Alert:
			SetMove(Vector3.zero, isFlip: false);
			break;
		case UnitState.FollowTarget:
			SetMove(Vector3.zero, isFlip: false);
			if (!base.HaveTarget)
			{
				GetNearestTarget();
				if (!base.HaveTarget)
				{
					base.Anima.Play("Idle2");
					state = UnitState.Idle2;
					break;
				}
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
			}
			break;
		case UnitState.Idle2:
			SetMove(Vector3.zero, isFlip: false);
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = UnitState.FollowTarget;
					base.Anima.Play("Jump");
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "AltarFinish":
			GetNearestTarget();
			if (base.HaveTarget)
			{
				state = UnitState.FollowTarget;
				base.Anima.Play("Jump");
			}
			else
			{
				state = UnitState.Idle2;
				base.Anima.Play("Idle2");
			}
			break;
		case "Jump":
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.IsVelocityDeclice = false;
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				CheckNavInfo();
				componentData.TakeKnockback(ToPointDir(navInfo.ToGoPoint) * finalJumpForwardForce);
			}
			SetComponentData(componentData);
			break;
		}
		case "JumpFinish":
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.IsVelocityDeclice = true;
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
		if (state == UnitState.Idle)
		{
			state = UnitState.Alert;
			base.Anima.Play("Alert");
			mr_Down.transform.localPosition = new Vector3(0f, 0.2f, 0.0001f);
			mr_Down.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_DownTransform.texture);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.unitType = UnitType.Monster;
			SetComponentData(componentData);
			LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Add(myPpt.myEntity);
			LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Add(myPpt.myEntity);
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, OutputMgr_Dots.GetSO4Chest(this.chestType), base.transform.position, 1f);
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_EndlessChest != null && Random.value <= (float)PlayerMgr.Inst.ItemCtrller.relicCfg_EndlessChest.int1.result / 100f)
		{
			ChestType chestType = (ChestType)Random.Range(0, 4);
			int id = 401;
			switch (chestType)
			{
			case ChestType.NoLock:
				id = 404;
				break;
			case ChestType.Lock:
				id = 401;
				break;
			case ChestType.Spike:
				id = 402;
				break;
			case ChestType.Curse:
				id = 403;
				break;
			default:
				Debug.LogError(chestType);
				break;
			}
			Entity entity = QuickCreateSystem.Inst.CreateSpecialObj(id, base.transform.position);
			Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, 1.5f);
			if (UnitDotsSyncSystem.entityMgr.HasComponent<SpecialObj4_Dots>(entity))
			{
				SpecialObj4_Dots componentData = GetComponentData<SpecialObj4_Dots>(entity);
				componentData.SetFly(navMeshPointIngoreZ);
				SetComponentData(componentData, entity);
			}
			else if (UnitDotsSyncSystem.entityMgr.HasComponent<SpecialObj4NoLock>(entity))
			{
				SpecialObj4NoLock componentData2 = GetComponentData<SpecialObj4NoLock>(entity);
				componentData2.SetFly(navMeshPointIngoreZ);
				SetComponentData(componentData2, entity);
			}
			IRoomCtrller_Dots componentData3 = GetComponentData<IRoomCtrller_Dots>(entity);
			componentData3.belongRoom.Value = LevelMgr.Inst.CurrentRoomCtrller;
			componentData3.onRoomEnter = true;
			SetComponentData(componentData3, entity);
		}
	}
}
