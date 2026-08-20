using System.Linq;
using UnityEngine;

public class Spell1026ChargeEffect : SpellChargeEffectBase
{
	public int MaxStage = 4;

	public ParticleSystem Charge;

	public ParticleSystem Shine;

	protected void OnEnable()
	{
		if (!IsSkipHolding)
		{
			Charge = CreateEffect("Charge_Charge").GetComponent<ParticleSystem>();
		}
		Shine = CreateEffect("Charge_Shine").GetComponent<ParticleSystem>();
		ResetAllEffect(Shine.transform);
	}

	protected override void OnFirstFrame()
	{
		if ((bool)Charge && (bool)Shine && (bool)AttachTarget)
		{
			Charge.transform.localScale = AttachTarget.lossyScale;
			Shine.transform.localScale = AttachTarget.lossyScale;
		}
	}

	public override void ChangeStage(int stage)
	{
		SetActiveOverlayStage(Shine.transform, stage, active: true);
		if (stage == MaxStage && (bool)Charge)
		{
			Charge.Stop(withChildren: true);
		}
		if (!IsSkipHolding)
		{
			GameObject gameObject = CreateEffect("Flash_" + stage);
			if ((bool)AttachTarget)
			{
				gameObject.transform.localScale = AttachTarget.lossyScale;
			}
		}
	}

	public override void Release()
	{
		string[] array = Effects.Keys.ToArray();
		foreach (string text in array)
		{
			if (text.StartsWith("Flash_"))
			{
				RemoveEffect(text, 2f);
			}
		}
		if ((bool)Charge)
		{
			Charge.Stop(withChildren: true);
		}
	}

	private void ResetAllEffect(Transform targetTrans)
	{
		for (int i = 1; i <= MaxStage; i++)
		{
			SetActiveByStage(targetTrans, i, active: false);
		}
	}

	private void SetActiveOverlayStage(Transform targetTrans, int stage, bool active)
	{
		for (int i = 0; i < stage; i++)
		{
			SetActiveByStage(targetTrans, i + 1, active);
		}
	}

	private void SetActiveByStage(Transform targetTrans, int stage, bool active)
	{
		targetTrans.Find("Stage" + stage).gameObject.SetActive(active);
	}
}
