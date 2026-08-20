using System;
using UnityEngine;

public class Spell9011RotateArrow : SpellBase
{
	[Space(50f)]
	public GameObject Model;

	public GameObject Shadow;

	private Vector3 targetPoint;

	public float rotateSpeed;

	private bool aligned;

	public float allowAngle;

	public override void InitializeCallback()
	{
		tsf_Layer.gameObject.SetActive(value: false);
		if (base.spellAroundOwnerRadius != 0f)
		{
			rigid.linearVelocity = Vector3.zero;
		}
		else
		{
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
		aligned = false;
		base.rebounceTime = 0;
	}

	public void Initialize(Vector3 targetPoint, float rotateSpeed)
	{
		this.rotateSpeed = rotateSpeed;
		this.targetPoint = targetPoint;
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		tsf_Layer.gameObject.SetActive(value: true);
	}

	public override void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		EffectBase.ManualCreateEffect("Hit");
	}

	public override void Update()
	{
		base.Update();
		if (rigid.linearVelocity != Vector3.zero)
		{
			Shadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
			Model.transform.up = Tool2D.IgnoreZPoint(rigid.linearVelocity);
			Shadow.transform.up = Tool2D.IgnoreZPoint(rigid.linearVelocity);
			Vector3 vector = Tool2D.IgnoreZPoint(targetPoint - base.transform.position);
			if (!aligned)
			{
				base.Direction = Tool2D.IgnoreZPoint(Vector3.RotateTowards(base.Direction, vector, rotateSpeed * Time.deltaTime * (MathF.PI / 180f), 0f).normalized);
				if (Vector3.Angle(base.Direction, vector) < allowAngle)
				{
					aligned = true;
				}
			}
		}
		rigid.linearVelocity = base.Direction * base.CurrentSpeed;
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

	public override TakeDamageInfo OutputDamage(GameObject targetGO, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		if (info == null)
		{
			info = new TakeDamageInfo();
		}
		switch (targetGO.tag)
		{
		case "Teammate":
			info.teammateTakeDamageRatio = 3f;
			break;
		}
		return OutputDamage(targetGO.GetComponent<UnitProperty>(), info);
	}

	public override TakeDamageInfo OutputDamage(UnitProperty unitPpt, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		SEMgr.Inst.spell9011Hit.PlaySE();
		return base.OutputDamage(unitPpt, info);
	}
}
