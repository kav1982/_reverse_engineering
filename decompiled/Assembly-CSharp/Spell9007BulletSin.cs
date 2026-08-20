using System;
using UnityEngine;

public class Spell9007BulletSin : SpellBase
{
	[Space(50f)]
	public GameObject Model;

	private float sinTimer;

	private float frequency;

	private bool reversed;

	private float amplitude;

	private Vector3 horizontalDiration;

	private Vector3 lastFrameDiration;

	private Vector3 rootPosition;

	private float fakeSpeed;

	public override void InitializeCallback()
	{
		reversed = false;
		amplitude = 1f;
		sinTimer = 0f;
		frequency = 1f;
		horizontalDiration = Tool2D.GetDir(base.Direction, reversed ? 90 : (-90));
		rootPosition = base.transform.position;
		rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		fakeSpeed = rigid.linearVelocity.magnitude;
		rigid.linearVelocity = Vector3.zero;
		Update();
	}

	public override void Update()
	{
		base.Update();
		if (lastFrameDiration != base.Direction)
		{
			lastFrameDiration = base.Direction;
			horizontalDiration = Tool2D.GetDir(base.Direction, reversed ? 90 : (-90));
		}
		sinTimer += fakeSpeed / base.spellCfg.speed * Time.deltaTime * frequency * 2f * MathF.PI;
		rootPosition += Time.deltaTime * fakeSpeed * base.Direction;
		base.transform.position = rootPosition + horizontalDiration * Mathf.Sin(sinTimer) * amplitude;
		base.DurationTimer += Time.deltaTime;
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				rigid.linearVelocity = Vector3.zero;
				fakeSpeed = 0f;
				base.CurrentSpeed = 0f;
			}
			tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
			if (tsf_Layer.localScale.x <= 0f)
			{
				PoolRecycle();
			}
		}
	}

	public void SetSin(float amplitude, float frequency = 1f, bool reversed = false)
	{
		this.amplitude = amplitude;
		this.frequency = frequency;
		this.reversed = reversed;
	}
}
