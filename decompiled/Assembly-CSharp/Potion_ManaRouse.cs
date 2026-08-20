using UnityEngine;

public class Potion_ManaRouse : LayerCorrect
{
	private PotionConfig potionCfg;

	private float durationTimer;

	public void Initialize(PotionConfig potionCfg)
	{
		this.potionCfg = potionCfg;
	}

	private void Update()
	{
		if (PlayerMgr.Inst.SelectedWandCfg != null)
		{
			ToRecoverMp(PlayerMgr.Inst.SelectedWand);
		}
		durationTimer += PlayerMgr.Inst.PlayerDeltaTime;
		if (durationTimer >= potionCfg.float1)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void ToRecoverMp(Wand wand)
	{
		if (!(wand == null) && wand.WandCfg != null)
		{
			float value = (float)wand.WandCfg.mpRecovery * PlayerMgr.Inst.PlayerDeltaTime * (float)potionCfg.int1 / 100f;
			value = Mathf.Clamp(value, 0f, wand.MaxMP - wand.CurrentMP);
			wand.CurrentMP += value;
		}
	}
}
