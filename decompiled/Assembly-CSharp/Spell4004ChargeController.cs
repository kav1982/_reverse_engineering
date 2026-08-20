using UnityEngine;

public class Spell4004ChargeController : EffectController
{
	public GameObject normalCharge;

	public GameObject fullCharge;

	public override void OnEnable()
	{
		base.OnEnable();
		normalCharge.SetActive(value: true);
		fullCharge.SetActive(value: false);
	}

	public void WandFullCharge()
	{
		normalCharge.SetActive(value: false);
		fullCharge.SetActive(value: true);
	}
}
