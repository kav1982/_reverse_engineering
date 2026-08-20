using UnityEngine;

public class UIPotion_Psychedelic : MonoBehaviour
{
	public float timeScale;

	private PotionConfig potionCfg;

	private float durationTimer;

	private float fadeTime = 0.5f;

	private void Update()
	{
		durationTimer += Time.unscaledDeltaTime;
		if (durationTimer >= potionCfg.float1 + fadeTime * 2f)
		{
			DestroySelf();
		}
	}

	public void Initialize(PotionConfig potionCfg)
	{
		this.potionCfg = potionCfg;
		TimeScaleMgr.Inst.AddNewTimeScaleModifyRequest(timeScale, potionCfg.float1, fadeTime);
		PlayerMgr.Inst.IsAffectedTimeScale = false;
		durationTimer = 0f;
	}

	public void DestroySelf()
	{
		PlayerMgr.Inst.IsAffectedTimeScale = true;
		TimeScaleMgr.Inst.AddNewTimeScaleModifyRequest(1f, 0f, 100f);
		Object.Destroy(base.gameObject);
	}
}
