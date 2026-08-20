using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster996 : UnitBase
{
	public enum UnitState
	{
		BornIdle,
		WalkToTarget,
		GetTargetIdle,
		NoTargetIdle,
		WalkToBoundary
	}

	[Space(50f)]
	public float getTargetIdleTime;

	public float noTargetIdleTime;

	public Vector3 getItemOffset;

	public float t6EntadDistance;

	[Header("MoveRatio")]
	public float moveRatioWand;

	public float moveRatioSpell;

	public float moveRatioRelic;

	public float moveRatioPotion;

	public float moveRatioResource_Coin1;

	public float moveRatioResource_Coin5;

	public float moveRatioResource_Coin50;

	public float moveRatioResource_Key1;

	public float moveRatioResource_Key2;

	public float moveRatioResource_HP1;

	public float moveRatioResource_HP2;

	public float moveRatioResource_HP3;

	public float moveRatioResource_Shield1;

	public float moveRatioResource_Shield2;

	public float moveRatioResource_Shield3;

	public float moveRatioResource_MagicCrystal;

	public float moveRatioResource_MagicCrystal2;

	public float moveRatioResource_MagicCrystal3;

	public float moveRatioResource_AncientBlood;

	public float moveRatioResource_AncientBlood2;

	public float moveRatioResource_AncientBlood3;

	public float moveRatioResource_ChaosCore;

	private float MoveRatio;

	private Monster996Born monster996Born;

	private ItemInfo targetItemInfo;

	private Entity targetItemEntity;

	public UnitState state;

	private float getTargetIdleTimer;

	private float noTargetIdleTimer;

	private bool getItem;

	private bool HaveTargetItem => EntityIsValid(targetItemEntity);

	private Vector3 TargetItemPoint => GetComponentData<LocalTransform>(targetItemEntity).Position;

	private float CaculateRatio()
	{
		if (getItem && HaveTargetItem)
		{
			targetItemInfo = GetComponentData<Item>(targetItemEntity).info;
			switch (targetItemInfo.type)
			{
			case ItemType.Wand:
				return moveRatioWand;
			case ItemType.Spell:
				return moveRatioSpell;
			case ItemType.Relic:
				return moveRatioRelic;
			case ItemType.Potion:
				return moveRatioPotion;
			case ItemType.Resource:
				switch (targetItemInfo.id)
				{
				case 11:
					return moveRatioResource_Coin1;
				case 12:
					return moveRatioResource_Coin5;
				case 13:
					return moveRatioResource_Coin50;
				case 21:
					return moveRatioResource_Key1;
				case 22:
					return moveRatioResource_Key2;
				case 31:
					return moveRatioResource_HP1;
				case 32:
					return moveRatioResource_HP2;
				case 33:
					return moveRatioResource_HP3;
				case 41:
					return moveRatioResource_Shield1;
				case 42:
					return moveRatioResource_Shield2;
				case 43:
					return moveRatioResource_Shield3;
				case 101:
					return moveRatioResource_MagicCrystal;
				case 102:
					return moveRatioResource_MagicCrystal2;
				case 103:
					return moveRatioResource_MagicCrystal3;
				case 111:
					return moveRatioResource_AncientBlood;
				case 112:
					return moveRatioResource_AncientBlood2;
				case 113:
					return moveRatioResource_AncientBlood3;
				case 121:
					return moveRatioResource_ChaosCore;
				default:
					Debug.LogError(targetItemInfo.id);
					return 1f;
				}
			default:
				Debug.LogError(targetItemInfo.id);
				return 1f;
			}
		}
		return 1f;
	}

	public override void EveryInitialCallback()
	{
		state = UnitState.BornIdle;
		getTargetIdleTimer = 0f;
		noTargetIdleTimer = 0f;
		getItem = false;
		base.Anima.Play("Idle", 0, 0f);
		targetItemEntity = Entity.Null;
		float num = myPpt.unitCfg.maxHP;
		float num2 = myPpt.unitCfg.knockbackRatio;
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
			}
			myPpt.unitCfg.maxHP = num;
			myPpt.unitCfg.currentHP = num;
			myPpt.unitCfg.knockbackRatio = num2;
		}
		MoveRatio = 1f;
	}

	public unsafe override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case UnitState.BornIdle:
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				base.Anima.Play("Walk", 0, 0f);
				if (!HaveTargetItem)
				{
					state = UnitState.WalkToBoundary;
					GetNavInfo(base.transform.position + Tool2D.GetDir() * 100f);
				}
				else
				{
					state = UnitState.WalkToTarget;
					GetNavInfo(GetComponentData<LocalTransform>(targetItemEntity).Position);
				}
			}
			break;
		case UnitState.WalkToTarget:
			if (!HaveTargetItem)
			{
				state = UnitState.NoTargetIdle;
				base.Anima.Play("Idle", 0, 0f);
			}
			else if (navInfo.allCornerArrived)
			{
				state = UnitState.GetTargetIdle;
				PhysicsCollider componentData5 = GetComponentData<PhysicsCollider>(targetItemEntity);
				componentData5.ColliderPtr->SetCollisionFilter(GameConst.Filter_None);
				SetComponentData(componentData5, targetItemEntity);
				getItem = true;
			}
			else
			{
				GetNavInfo(TargetItemPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			break;
		case UnitState.GetTargetIdle:
			SetMove(Vector3.zero);
			if (HaveTargetItem)
			{
				LocalTransform componentData4 = GetComponentData<LocalTransform>(targetItemEntity);
				componentData4.Position = base.transform.position + getItemOffset;
				SetComponentData(componentData4, targetItemEntity);
			}
			getTargetIdleTimer += Time.deltaTime;
			if (getTargetIdleTimer >= getTargetIdleTime)
			{
				getTargetIdleTimer = 0f;
				MoveRatio = CaculateRatio();
				state = UnitState.WalkToBoundary;
				GetNavInfo(base.transform.position + Tool2D.GetDir() * 100f);
			}
			break;
		case UnitState.NoTargetIdle:
			SetMove(Vector3.zero);
			noTargetIdleTimer += Time.deltaTime;
			if (noTargetIdleTimer >= noTargetIdleTime)
			{
				noTargetIdleTimer = 0f;
				base.Anima.Play("Walk", 0, 0f);
				targetItemEntity = monster996Born.CurseStealthy.GetCanStealItem();
				if (!HaveTargetItem)
				{
					state = UnitState.WalkToBoundary;
					GetNavInfo(base.transform.position + Tool2D.GetDir() * 100f);
					break;
				}
				state = UnitState.WalkToTarget;
				Item componentData6 = GetComponentData<Item>(targetItemEntity);
				componentData6.isCurseStealthyTarget = true;
				SetComponentData(componentData6, targetItemEntity);
			}
			break;
		case UnitState.WalkToBoundary:
			if (HaveTargetItem)
			{
				LocalTransform componentData = GetComponentData<LocalTransform>(targetItemEntity);
				componentData.Position = base.transform.position + getItemOffset;
				SetComponentData(componentData, targetItemEntity);
			}
			if (navInfo.allCornerArrived)
			{
				if (HaveTargetItem)
				{
					Item componentData2 = GetComponentData<Item>(targetItemEntity);
					componentData2.BackPool();
					componentData2.Pickup(playSE: false);
					SetComponentData(componentData2, targetItemEntity);
				}
				DotsAnnouncedDeath();
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * MoveRatio);
			CheckNavInfo();
			if ((LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1) && (base.transform.position.x > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x + (float)(LevelMgr.Inst.CurrentRoomCfg.theme6Width / 2) - t6EntadDistance || base.transform.position.x < LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x - (float)(LevelMgr.Inst.CurrentRoomCfg.theme6Width / 2) + t6EntadDistance || base.transform.position.y > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + (float)(LevelMgr.Inst.CurrentRoomCfg.theme6Height / 2) - t6EntadDistance || base.transform.position.y < LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y - (float)(LevelMgr.Inst.CurrentRoomCfg.theme6Height / 2) + t6EntadDistance))
			{
				if (HaveTargetItem)
				{
					Item componentData3 = GetComponentData<Item>(targetItemEntity);
					componentData3.BackPool();
					componentData3.Pickup(playSE: false);
					SetComponentData(componentData3, targetItemEntity);
				}
				myPpt.AnnouncedDeath_Dots();
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		monster996Born.CurseStealthy.MonsterUnregister(this);
		monster996Born.CurseStealthy.MiniPool.RecycleGO(monster996Born.gameObject);
		if (HaveTargetItem)
		{
			PhysicsCollider pc = GetComponentData<PhysicsCollider>(targetItemEntity);
			DTool.SetCollider(in pc, 262144u);
			SetComponentData(pc, targetItemEntity);
			Item componentData = GetComponentData<Item>(targetItemEntity);
			componentData.isCurseStealthyTarget = false;
			SetComponentData(componentData, targetItemEntity);
			LocalTransform componentData2 = GetComponentData<LocalTransform>(targetItemEntity);
			componentData2.Position = Tool2D.IgnoreZPoint(componentData2.Position);
			SetComponentData(componentData2, targetItemEntity);
		}
	}

	public void Setting(Monster996Born monster996Born, Entity targetItemEntity)
	{
		this.targetItemEntity = targetItemEntity;
		this.monster996Born = monster996Born;
	}
}
