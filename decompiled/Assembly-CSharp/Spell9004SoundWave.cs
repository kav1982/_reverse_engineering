using System.Collections.Generic;
using UnityEngine;

public class Spell9004SoundWave : SpellBase
{
	[Space(50f)]
	public Transform tsf_Motion;

	public float maxScale;

	private List<Collider> attackedColliders = new List<Collider>();

	public override void InitializeCallback()
	{
		rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		base.penetrateTime = 100;
		attackedColliders.Clear();
		tsf_Motion.localScale = Vector3.one;
		triggerIn.transform.localScale = Vector3.one;
		ChangeTeamToMonster(ownerPpt);
	}

	public override void Update()
	{
		base.Update();
		if (!base.isFlyFinish)
		{
			float t = base.DurationTimer / base.spellCfg.duration;
			tsf_Motion.localScale = Vector3.one * Mathf.Lerp(1f, maxScale, t);
			triggerIn.transform.localScale = tsf_Motion.localScale;
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
			if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
			{
				base.SpellHoverTimer += Time.deltaTime;
			}
			else if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
			{
				PoolRecycle();
			}
			else
			{
				PoolRecycle();
			}
		}
	}

	public override void ChangeTeamToPlayer()
	{
		base.ChangeTeamToPlayer();
		attackedColliders.Clear();
	}

	public override void ChangeTeamToMonster(UnitProperty monsterPpt)
	{
		base.ChangeTeamToMonster(monsterPpt);
		attackedColliders.Clear();
	}
}
