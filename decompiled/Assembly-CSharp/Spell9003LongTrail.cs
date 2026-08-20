using UnityEngine;

public class Spell9003LongTrail : SpellBase
{
	[Header("\ufffd\ufffdβ\ufffdӵ\ufffd")]
	public float bulletDistance;

	public float bulletLifeTime;

	public int bulletDamage;

	private float movedDistance;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public override void InitializeCallback()
	{
		if (ownerPpt.unitCfg.IsSameCamp(UnitType.Monster))
		{
			ChangeTeamToMonster(ownerPpt);
		}
		base.transform.up = base.Direction;
		if (base.spellAroundOwnerRadius != 0f)
		{
			rigid.linearVelocity = Vector3.zero;
			Update();
		}
		else
		{
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
		sipBullet.spelldataConfig = SpellConfig.GetConfigCopy(90031);
		sipBullet.spelldataConfig.speed = 0f;
		sipBullet.spelldataConfig.duration = bulletLifeTime;
		sipBullet.spelldataConfig.damage = bulletDamage;
		sipBullet.spelldataConfig.playShootSE = false;
		sipBullet.ownerPpt = ownerPpt;
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		CorrectLayerOnce();
	}

	public override void FixedUpdate()
	{
		if (base.spellCfg.level == 2 && Mathf.Abs(base.transform.position.x - roomCenterPoint.x) < roomWidth / 2f + 2f && Mathf.Abs(base.transform.position.y - roomCenterPoint.y) < roomHeight / 2f + 2f)
		{
			movedDistance += Time.fixedDeltaTime * base.CurrentSpeed;
			if (sipBullet.ownerPpt != ownerPpt)
			{
				sipBullet.ownerPpt = ownerPpt;
			}
			if (movedDistance > bulletDistance)
			{
				movedDistance -= bulletDistance;
				sipBullet.shootDirection = base.Direction;
				ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + sipBullet.spelldataConfig.prefab, base.transform.position).GetComponent<SpellBase>().Initialize(sipBullet);
			}
		}
		base.FixedUpdate();
	}

	public override void Update()
	{
		base.Update();
		base.transform.up = base.Direction;
		base.DurationTimer += Time.deltaTime;
		if (!(base.DurationTimer > base.spellCfg.duration))
		{
			return;
		}
		if (!base.isFlyFinish)
		{
			base.isFlyFinish = true;
			rigid.linearVelocity = Vector3.zero;
			base.CurrentSpeed = 0f;
		}
		if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
		{
			base.SpellHoverTimer += Time.deltaTime;
			return;
		}
		if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
		{
			PoolRecycle();
			return;
		}
		tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
		if (tsf_Layer.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}
}
