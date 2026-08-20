using UnityEngine;

public class Potion_Invisible : LayerCorrect
{
	private PotionConfig potionCfg;

	private float durationTimer;

	private void Update()
	{
		durationTimer += Time.deltaTime;
		if (durationTimer >= potionCfg.float1)
		{
			PlayerMgr.Inst.ItemCtrller.potion_Invisible = null;
			PlayerMgr.Inst.PlayerCtrller.SetVisiable();
			Object.Destroy(base.gameObject);
		}
	}

	public void Initialize(PotionConfig potionCfg)
	{
		this.potionCfg = potionCfg;
		durationTimer = 0f;
		PlayerMgr.Inst.PlayerCtrller.SetInvisiable();
	}
}
