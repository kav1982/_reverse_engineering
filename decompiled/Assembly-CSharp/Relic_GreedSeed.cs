using UnityEngine;

public class Relic_GreedSeed : LayerCorrect
{
	[Space(50f)]
	public Shadow shadow;

	public float rotateSpeed;

	public float followDistance;

	public float followLerp;

	public RelicConfig RelicCfg;

	private float angleTimer;

	private void Update()
	{
		if (tsf_Layer.gameObject.activeSelf)
		{
			angleTimer += rotateSpeed * Time.deltaTime;
			Vector3 b = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir(angleTimer) * followDistance;
			base.transform.position = Vector3.Lerp(base.transform.position, b, followLerp * Time.deltaTime);
			if (RelicCfg.intTimer >= RelicCfg.int1.result)
			{
				HideSelf();
			}
		}
	}

	public void Initialize(RelicConfig relicCfg)
	{
		RelicCfg = relicCfg;
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
	}

	public void PointerToPlayer()
	{
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
	}

	public void HideSelf()
	{
		tsf_Layer.gameObject.SetActive(value: false);
		shadow.ShadowGO.SetActive(value: false);
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}
}
