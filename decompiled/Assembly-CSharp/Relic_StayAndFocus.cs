using UnityEngine;

public class Relic_StayAndFocus : LayerCorrect
{
	[Space(50f)]
	public SpriteRenderer sr;

	public float focusSpeed;

	public float disappearSpeed;

	public float maxSize;

	public float minSize;

	public RelicConfig Cfg;

	private void Update()
	{
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
		if (PlayerMgr.Inst.PlayerCtrller.CurrentMoveDir == Vector3.zero)
		{
			Cfg.floatTimer += focusSpeed * PlayerMgr.Inst.PlayerDeltaTime;
			if (Cfg.floatTimer > (float)Cfg.int1.result / 100f)
			{
				Cfg.floatTimer = (float)Cfg.int1.result / 100f;
			}
		}
		else
		{
			Cfg.floatTimer -= disappearSpeed * PlayerMgr.Inst.PlayerDeltaTime;
			if (Cfg.floatTimer < 0f)
			{
				Cfg.floatTimer = 0f;
			}
		}
		float t = Cfg.floatTimer * 100f / (float)Cfg.int1.result;
		sr.transform.localScale = Vector3.one * Mathf.Lerp(maxSize, minSize, t);
		float a = Mathf.Lerp(0f, 1f, t);
		sr.color = new Color(1f, 1f, 1f, a);
	}

	public void Initialize(RelicConfig blessingCfg)
	{
		Cfg = blessingCfg;
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}
}
