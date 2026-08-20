using UnityEngine;

public class Spell9002BounceBone : SpellBase
{
	[Space(50f)]
	public VariableFloat rotateSpeed;

	public float rebounceRemainRatio;

	public float stopThreshold;

	public float playLandSEUpSpeedThreshould;

	public float fallInCliffDisntace;

	private Vector3 affect_AbyssPoint;

	private bool affect_InAbyss;

	private int bounceTime;

	private int bounceTimer;

	protected override void OnFallingGround()
	{
	}

	public override void InitializeCallback()
	{
		rotateSpeed.RandomResult();
		rotateSpeed.result *= ((Random.Range(0, 2) == 0) ? 1 : (-1));
		base.rebounceTime = 100;
		affect_InAbyss = false;
		bounceTime = 0;
		bounceTimer = 0;
		if (base.spellAroundOwnerRadius != 0f)
		{
			rigid.linearVelocity = Vector3.zero;
			base.spellCfg.upSpeed = 0f;
			base.spellCfg.gravity = 0f;
			base.CurrentUpSpeed = 0f;
			base.spellCfg.duration = SpellConfig.dic[10011].duration + base.spellCfg.duration - SpellConfig.dic[base.spellCfg.id].duration;
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
		if (affect_InAbyss)
		{
			if (base.transform.position != affect_AbyssPoint)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, affect_AbyssPoint, 4f * Time.deltaTime);
			}
			float num = base.transform.localScale.x - Time.deltaTime;
			if (num < 0f)
			{
				PoolRecycle();
			}
			else
			{
				base.transform.localScale = Vector3.one * num;
			}
		}
		if (!base.isFlyFinish)
		{
			((Spell9002Effect)EffectBase).Rotate(rotateSpeed.result * Time.deltaTime);
		}
		if (base.transform.position.z >= 0f)
		{
			if (!affect_InAbyss)
			{
				Collider collider = GeneralTool.HaveCollider(Tool2D.IgnoreZPoint(base.transform), base.ColliderRadius * base.transform.localScale.x, "Abyss", "Abyss");
				if (collider != null)
				{
					affect_InAbyss = true;
					affect_AbyssPoint = Tool2D.IgnoreZPoint(collider.transform.position);
					rigid.linearVelocity = Vector3.zero;
				}
			}
			if ((Tool2D.GetNavMeshPointIngoreZ(Tool2D.IgnoreZPoint(base.transform), 8) - base.transform.position).sqrMagnitude > fallInCliffDisntace * fallInCliffDisntace)
			{
				affect_InAbyss = true;
				affect_AbyssPoint = Tool2D.IgnoreZPoint(base.transform.position);
				rigid.linearVelocity = Vector3.zero;
			}
			if (base.CurrentSpeed <= stopThreshold)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				ChangeVelocityZ(0f);
				ChangeGravity(0f);
				base.CurrentSpeed = 0f;
				base.Height = 0.01f;
				rigid.linearVelocity = Vector3.zero;
				base.DurationTimer = base.spellCfg.duration;
			}
			else
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				ChangeVelocityZ((0f - base.CurrentUpSpeed) * rebounceRemainRatio);
				base.Height = 0.01f;
				rotateSpeed.result *= rebounceRemainRatio;
				base.CurrentSpeed *= rebounceRemainRatio;
				rigid.linearVelocity = rigid.linearVelocity.normalized * base.CurrentSpeed;
				if (base.CurrentUpSpeed > playLandSEUpSpeedThreshould)
				{
					SEMgr.Inst.spell9002Land.PlaySE();
				}
				if (bounceTime > 0)
				{
					bounceTimer++;
					if (bounceTimer >= bounceTime)
					{
						base.transform.position = Tool2D.IgnoreZPoint(base.transform);
						ChangeVelocityZ(0f);
						ChangeGravity(0f);
						base.CurrentSpeed = 0f;
						rigid.linearVelocity = Vector3.zero;
						base.DurationTimer = base.spellCfg.duration;
					}
				}
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
			base.transform.localScale = Vector3.one * (base.transform.localScale.x - 5f * Time.deltaTime);
			if (base.transform.localScale.x <= 0f)
			{
				PoolRecycle();
			}
		}
	}

	public void SetBounceTime(int bounceTime)
	{
		this.bounceTime = bounceTime;
	}
}
