using UnityEngine;

public class Spell9013BladeWaveVertical : SpellBase
{
	[Space(50f)]
	public SpriteRenderer thisRenderer;

	public SpriteRenderer bottomRender;

	public Transform bottom;

	public Sprite sprite1;

	public Sprite sprite2;

	public Sprite sprite3;

	public Sprite sprite4;

	public float changeTime;

	private float changeTimer;

	private bool isSprite1;

	public override void InitializeCallback()
	{
		tsf_Layer.gameObject.SetActive(value: false);
		base.penetrateTime = 100;
		if (base.spellAroundOwnerRadius != 0f)
		{
			rigid.linearVelocity = Vector3.zero;
		}
		else
		{
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		tsf_Layer.gameObject.SetActive(value: true);
	}

	public override void Update()
	{
		base.Update();
		if (rigid.linearVelocity != Vector3.zero)
		{
			thisRenderer.material.SetFloat("_RotateAngle", Tool2D.IgnoreZAngleWithSign(Vector3.right, rigid.linearVelocity));
			bottom.right = rigid.linearVelocity;
		}
		changeTimer += Time.deltaTime;
		if (changeTimer > changeTime)
		{
			changeTimer = 0f;
			isSprite1 = !isSprite1;
			if (isSprite1)
			{
				thisRenderer.sprite = sprite1;
				bottomRender.sprite = sprite3;
			}
			else
			{
				thisRenderer.sprite = sprite2;
				bottomRender.sprite = sprite4;
			}
		}
		base.DurationTimer += Time.deltaTime;
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				rigid.linearVelocity = Vector3.zero;
				base.CurrentSpeed = 0f;
			}
			tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
			if (tsf_Layer.localScale.x <= 0f)
			{
				PoolRecycle();
			}
		}
	}

	public override TakeDamageInfo OutputDamage(UnitProperty unitPpt, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		SEMgr.Inst.spell9009Hit.PlaySE();
		return base.OutputDamage(unitPpt, info);
	}
}
