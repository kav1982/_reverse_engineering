using System;
using UnityEngine;

public class Spell9008BulletSinSpeed : SpellBase
{
	[Space(50f)]
	private float sinTimer;

	private float frequency;

	private bool reversed;

	private float amplitude;

	private Vector3 rootSpeedDir;

	public override void InitializeCallback()
	{
		reversed = false;
		amplitude = 1f;
		sinTimer = 0f;
		frequency = 1f;
		Tool2D.GetDir(base.Direction, reversed ? 90 : (-90));
		rigid.linearVelocity = Vector3.zero;
		base.CurrentSpeed = (base.Direction * base.CurrentSpeed).magnitude;
		Update();
	}

	public override void Update()
	{
		base.Update();
		sinTimer += base.CurrentSpeed / base.spellCfg.speed * Time.deltaTime * frequency * 2f * MathF.PI;
		rootSpeedDir = Tool2D.GetDir(base.Direction, Mathf.Cos(sinTimer) * amplitude * (float)((!reversed) ? 1 : (-1)));
		rigid.linearVelocity = rootSpeedDir * base.CurrentSpeed;
		base.transform.up = rootSpeedDir;
		base.DurationTimer += Time.deltaTime;
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				rigid.linearVelocity = Vector3.zero;
				base.CurrentSpeed = 0f;
				base.CurrentSpeed = 0f;
			}
			tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
			if (tsf_Layer.localScale.x <= 0f)
			{
				PoolRecycle();
			}
		}
	}

	public override void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		if (GameMgr.IsHarmony_Static && IsSameCamp(UnitType.Monster))
		{
			EffectBase.CreateSpriteEffect("HitH", position, rotation);
		}
		else
		{
			EffectBase.CreateSpriteEffect("Hit", position, rotation);
		}
	}

	public void SetSin(float amplitude, float frequency = 1f, bool reversed = false)
	{
		this.amplitude = amplitude;
		this.frequency = frequency;
		this.reversed = reversed;
	}

	public override void ChangeTeamToPlayer()
	{
		base.ChangeTeamToPlayer();
		reversed = !reversed;
	}

	public override void ChangeTeamToMonster(UnitProperty monsterPpt)
	{
		base.ChangeTeamToMonster(monsterPpt);
		reversed = !reversed;
	}
}
