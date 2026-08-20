using UnityEngine;

public class Relic_FollowObj : LayerCorrect
{
	[Space(50f)]
	public Transform tsf_Hover;

	public float hoverSpeed;

	public float hoverHeight;

	private float hoverTimer;

	public RelicConfig RelicCfg { get; private set; }

	private void Update()
	{
		hoverTimer += hoverSpeed * Time.deltaTime;
		tsf_Hover.localPosition = new Vector3(0f, Mathf.Sin(hoverTimer) * hoverHeight, 0f);
	}

	public virtual void Initialize(RelicConfig reliCfg)
	{
		RelicCfg = reliCfg;
		PlayerMgr.Inst.PlayerCtrller.FollowObjRegister(base.gameObject);
	}

	public virtual void DestroySelf()
	{
		PlayerMgr.Inst.PlayerCtrller.FollowObjUnregister(base.gameObject);
		Object.Destroy(base.gameObject);
	}
}
