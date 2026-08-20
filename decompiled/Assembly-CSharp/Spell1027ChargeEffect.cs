using UnityEngine;

public class Spell1027ChargeEffect : SpellChargeEffectBase
{
	public ParticleSystem Charge;

	protected void OnEnable()
	{
		if (!IsSkipHolding)
		{
			Charge = CreateEffect("Charge").GetComponent<ParticleSystem>();
		}
	}

	protected override void OnFirstFrame()
	{
		if ((bool)Charge && (bool)AttachTarget)
		{
			Charge.transform.localScale = AttachTarget.lossyScale;
		}
	}

	public override void ChangeStage(int stage)
	{
	}

	public override void Release()
	{
		if ((bool)Charge)
		{
			Charge.Stop(withChildren: true);
		}
	}
}
