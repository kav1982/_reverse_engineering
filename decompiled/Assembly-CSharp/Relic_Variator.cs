using UnityEngine;

public class Relic_Variator : LayerCorrect
{
	[Space(50f)]
	public ParticleSystem ps;

	public float minEmission;

	public float maxEmission;

	private ParticleSystem.EmissionModule emission;

	private RelicConfig relicCfg;

	public float MoveRate => relicCfg.floatTimer / 100f;

	private void Update()
	{
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
		relicCfg.floatTimer += (float)(relicCfg.int2.result - relicCfg.int1.result) / relicCfg.float1.result * PlayerMgr.Inst.PlayerDeltaTime;
		if (relicCfg.floatTimer > (float)relicCfg.int2.result)
		{
			relicCfg.floatTimer = relicCfg.int2.result;
		}
		float t = (relicCfg.floatTimer - relicCfg.float1.result) / ((float)relicCfg.int2.result - relicCfg.float1.result);
		float constant = Mathf.Lerp(minEmission, maxEmission, t);
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(constant);
	}

	public void Initialize(RelicConfig blessingCfg)
	{
		relicCfg = blessingCfg;
		emission = ps.emission;
		blessingCfg.floatTimer = blessingCfg.int1.result;
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
	}

	public void BeHit()
	{
		relicCfg.floatTimer = relicCfg.int1.result;
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}
}
