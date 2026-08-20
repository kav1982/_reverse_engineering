using UnityEngine;

public class Spell9024RotateOut : SpellBase
{
	private float rotateRight;

	private float rotateSpeed;

	private Vector3 fromStartDirVertical;

	private Vector3 startPoint;

	private Vector3 fromStartDir;

	public bool isChapter3Fade;

	public float fadeDistance;

	private float roomHeight;

	private float roomWidth;

	private bool isChapter3;

	private Vector3 roomCenter;

	public void InitializeRotate(float rotateSpeed, float rotateRight)
	{
		if (isChapter3Fade)
		{
			isChapter3 = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1;
			if (isChapter3)
			{
				roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
				roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
				roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			}
		}
		startPoint = base.transform.position;
		this.rotateSpeed = rotateSpeed;
		this.rotateRight = rotateRight;
		base.transform.position = startPoint + base.Direction * 0.01f;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		fromStartDir = (base.transform.position - startPoint).normalized;
		fromStartDirVertical = Tool2D.GetDir(fromStartDir, -90f);
		rigid.linearVelocity = fromStartDir * base.CurrentSpeed + fromStartDirVertical * rotateSpeed * rotateRight;
		base.Direction = rigid.linearVelocity.normalized;
	}

	public override void ChangeTeamToPlayer()
	{
		base.ChangeTeamToPlayer();
		startPoint = base.transform.position + Tool2D.GetDir(startPoint - base.transform.position, 180f);
	}

	public override void InitializeCallback()
	{
	}

	public override void Update()
	{
		base.Update();
		if (isChapter3Fade && isChapter3 && (Mathf.Abs(roomCenter.x - base.transform.position.x) > roomWidth / 2f + fadeDistance || Mathf.Abs(roomCenter.y - base.transform.position.y) > roomHeight / 2f + fadeDistance))
		{
			PoolRecycle();
		}
		if (!base.SIP.spellIsFall)
		{
			base.DurationTimer += Time.deltaTime;
		}
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
		base.transform.localScale = Vector3.one * (base.transform.localScale.x - 5f * Time.deltaTime);
		if (base.transform.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	protected override TakeDamageInfo MakeDamageToUnit(UnitProperty unit)
	{
		TakeDamageInfo result = base.MakeDamageToUnit(unit);
		if (base.spellCfg.float1 > 0f)
		{
			unit.BonusTakeDamageRatioRegister(base.spellCfg.float1, base.spellCfg.float2);
		}
		return result;
	}

	public override void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		if (!base.SIP.spellIsFall)
		{
			base.CreateHitEffect(position, rotation);
		}
	}
}
