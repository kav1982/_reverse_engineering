using UnityEngine;

public class Spell9006TrickLongTrail : SpellBase
{
	[Space(50f)]
	public GameObject Model;

	public float rotateSpeed;

	public override void InitializeCallback()
	{
		if (base.spellAroundOwnerRadius != 0f)
		{
			rigid.linearVelocity = Vector3.zero;
			Update();
		}
		else
		{
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
		ChangeTeamToMonster(ownerPpt);
	}

	public override void Update()
	{
		base.Update();
		Model.transform.localEulerAngles += new Vector3(0f, 0f, Time.deltaTime * rotateSpeed);
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
}
