using UnityEngine;

public class Spell1027Effect : SpellEffectBase
{
	private Spell1027SuperNova novaScript;

	private Transform ChargeTransform;

	private int effectLevel = 1;

	protected override void Awake()
	{
		base.Awake();
		novaScript = (Spell1027SuperNova)base.Spell;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		ChargeTransform = null;
		effectLevel = 1;
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		switch (effect.Name)
		{
		case "Explosion":
		{
			int num2 = effectLevel;
			num2 = ((!GeneralTool.IsLowFpsOptimizeActive(100f) || !(GameMgr.Inst.GetFps() / 30f <= Random.Range(0.33f, 1f))) ? num2 : 0);
			int num3 = effectLevel;
			num3 = ((!GeneralTool.IsLowFpsOptimizeActive(10f)) ? num3 : 0);
			Transform transform3 = trans.Find("NormalLevel");
			Transform transform4 = trans.Find("GroundLevel");
			for (int j = 1; j < novaScript.effectStageThreshold.Count + 1; j++)
			{
				transform3.Find("Stage" + j).gameObject.SetActive(num2 >= j);
				transform4.Find("Stage" + j).gameObject.SetActive(num3 >= j);
			}
			break;
		}
		case "Charge":
			ChargeTransform = trans;
			break;
		case "Hit":
		{
			int num = effectLevel;
			num = ((!GeneralTool.IsLowFpsOptimizeActive(100f) || !(GameMgr.Inst.GetFps() / 30f <= Random.Range(0f, 1f))) ? num : 0);
			Transform transform = trans.Find("NormalLevel");
			Transform transform2 = trans.Find("GroundLevel");
			for (int i = 1; i < novaScript.effectStageThreshold.Count + 1; i++)
			{
				transform.Find("Stage" + i).gameObject.SetActive(num >= i);
				transform2.Find("Stage" + i).gameObject.SetActive(num >= i);
			}
			trans.localScale = Vector3.one * (1f + (float)(num - 1) * 0.15f);
			break;
		}
		}
	}

	protected override void Update()
	{
		base.Update();
		UpdateChargeCenterPoint();
	}

	private void UpdateChargeCenterPoint()
	{
		if ((bool)ChargeTransform && novaScript.ownerPpt.unitCfg.unitType == UnitType.Player)
		{
			ChargeTransform.transform.position += new Vector3(0f, -0.2f, 0f);
		}
	}

	public void SetEffectLevel(int level)
	{
		effectLevel = level;
	}
}
