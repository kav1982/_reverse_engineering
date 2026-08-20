using UnityEngine;

public class Boss5_Rock : UnitBase
{
	public SpriteRenderer mainRenderer;

	public bool dying;

	public ParticleSystem explodeParticle;

	private Shadow selfShadow;

	public override void EveryInitialCallback()
	{
		selfShadow = GetComponent<Shadow>();
		selfShadow.enabled = true;
		base.Anima.Play("Boss5_RockIdle");
	}

	public void Die()
	{
		if (!dying)
		{
			base.Anima.Play("Boss5_RockDie");
			dying = true;
			myPpt.CanTouch = false;
			Invoke("DelayRecycle", 6f);
		}
	}

	private void DelayRecycle()
	{
		myPpt.AnnouncedDeath();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void BeforeTakeDamage(TakeDamageInfo info)
	{
		base.BeforeTakeDamage(info);
		if (dying)
		{
			info.immuneDamage = true;
			return;
		}
		if (info.damage >= myPpt.unitCfg.currentHP)
		{
			info.isTargetDead = true;
			info.immuneDamage = true;
			base.CC_Self.enabled = false;
			myPpt.CanTouch = false;
		}
		else
		{
			float num = info.damage * 2f;
			if (info.attackerPpt != null && info.attackerPpt.unitCfg.unitType == UnitType.Player && PlayerMgr.Inst.ItemCtrller.relicCfg_AddCriticalDamage.id != 0)
			{
				num = Mathf.Ceil(info.damage * (float)PlayerMgr.Inst.ItemCtrller.relicCfg_AddCriticalDamage.int1.result / 100f);
			}
			if (num >= myPpt.unitCfg.currentHP)
			{
				info.criticalChance = -999999f;
			}
		}
		if (info.isTargetDead)
		{
			Die();
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "RockBreak")
		{
			base.CC_Self.enabled = false;
			explodeParticle.Play();
			selfShadow.Hide();
			mainRenderer.enabled = false;
		}
	}
}
