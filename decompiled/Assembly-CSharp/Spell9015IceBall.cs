using UnityEngine;

public class Spell9015IceBall : SpellBase
{
	[Space(50f)]
	public Transform tsf_LayerShadow;

	public float playerDamageRatio;

	public float teammateDamageRatio;

	public float frozenTime;

	public override void InitializeCallback()
	{
		ChangeTeamToMonster(ownerPpt);
		base.transform.up = base.Direction;
		spellFrozenTime = frozenTime;
		if (base.spellAroundOwnerRadius != 0f)
		{
			rigid.linearVelocity = Vector3.zero;
			Update();
		}
		else
		{
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
	}

	public override void Update()
	{
		base.Update();
		tsf_LayerShadow.position = Tool2D.IgnoreZPoint(base.transform, 1.05f);
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

	protected override TakeDamageInfo CreateDefaultTakeDamageInfo(UnitProperty unit)
	{
		TakeDamageInfo takeDamageInfo = base.CreateDefaultTakeDamageInfo(unit);
		takeDamageInfo.playerTakeDamageRatio = playerDamageRatio;
		takeDamageInfo.teammateTakeDamageRatio = teammateDamageRatio;
		return takeDamageInfo;
	}
}
