using UnityEngine;

public class Spell4013Effect : SpellEffectBase
{
	private Spell4013ArcaneBlade hammer;

	private Transform targetHammer;

	private Transform targetHammerShadow;

	private SpriteRenderer hammerSprite;

	private SpriteRenderer hammerStickSprite;

	public float SizeMaxTransparencyDecreaseRatio;

	public float CountMaxTransparencyDecreaseRatio;

	public float MinTransparency;

	public int TrailOffStartThreshold;

	protected override void Awake()
	{
		base.Awake();
		hammer = (Spell4013ArcaneBlade)base.Spell;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		targetHammer = null;
		hammerSprite = null;
		hammerStickSprite = null;
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		switch (effect.Name)
		{
		case "Spell":
		{
			targetHammer = trans;
			bool flag = hammer.spellCfg.isSplitSpell || hammer.SplitHammerList.Count <= 0;
			float num3 = Mathf.Max(hammer.hammerMinWidthRatio, hammer.radiusRatio * hammer.finalRadiusRatio);
			float y2 = num3 * 1.5f * hammer.hammerBaseSpriteBaseWidthAndLength;
			hammerSprite = trans.Find("Hammer").GetComponent<SpriteRenderer>();
			hammerSprite.size = new Vector2(hammer.hammerBaseSpriteBaseWidthAndLength, y2);
			SpriteRenderer component5 = trans.Find("HammerBase").GetComponent<SpriteRenderer>();
			component5.size = new Vector2(hammer.hammerBaseSpriteBaseWidthAndLength, y2);
			hammerStickSprite = trans.Find("HammerStick").GetComponent<SpriteRenderer>();
			hammerStickSprite.size = new Vector2(hammer.hammerBaseSpriteBaseWidthAndLength, y2);
			SpriteRenderer component6 = trans.Find("HammerStickBase").GetComponent<SpriteRenderer>();
			component6.size = new Vector2(hammer.hammerBaseSpriteBaseWidthAndLength, y2);
			trans.GetComponent<Spell4013HammerTransparencyController>().SetHammerBonusTransparencyRatio(GetHammerBonusTransaprencyRadio());
			hammerSprite.enabled = false;
			component5.enabled = false;
			hammerStickSprite.enabled = false;
			component6.enabled = false;
			if (flag)
			{
				hammerSprite.enabled = true;
				component5.enabled = true;
			}
			else
			{
				hammerStickSprite.enabled = true;
				component6.enabled = true;
			}
			float num4 = num3 - 1f;
			Vector3 localPosition = new Vector3(1.5f + num4 * hammer.triailXposShiftConvertRatio, 0f, 0f);
			int num5 = hammer.SIP.multiShootCount * (hammer.SplitCount + 1);
			bool active = flag;
			if (flag && num5 > TrailOffStartThreshold && hammer.SIP.inMultiShootIndex % 2 == 0)
			{
				active = false;
			}
			trans.Find("Trail").gameObject.SetActive(active);
			trans.Find("Trail").transform.localPosition = localPosition;
			Transform obj = trans.Find("CenterEmber");
			obj.gameObject.SetActive(flag);
			obj.localPosition = localPosition;
			hammer.DistanceCalculateTransform.localPosition = localPosition;
			foreach (Transform item in obj)
			{
				ParticleSystem component7 = item.GetComponent<ParticleSystem>();
				if ((bool)component7)
				{
					component7.Play();
				}
			}
			ParticleSystem.ShapeModule shape = trans.Find("Ember").GetComponent<ParticleSystem>().shape;
			shape.radius = hammer.emberBaseRadiu + hammer.emberShiftConvertRatio * num4;
			shape.position = new Vector3(hammer.emberShiftConvertRatio * num4, 0f, 0f);
			break;
		}
		case "FallSpell":
		{
			targetHammer = trans;
			bool num2 = hammer.spellCfg.isSplitSpell || hammer.SplitHammerList.Count <= 0;
			hammerSprite = trans.Find("Hammer").GetComponent<SpriteRenderer>();
			SpriteRenderer component3 = trans.Find("HammerBase").GetComponent<SpriteRenderer>();
			hammerStickSprite = trans.Find("HammerStick").GetComponent<SpriteRenderer>();
			SpriteRenderer component4 = trans.Find("HammerStickBase").GetComponent<SpriteRenderer>();
			trans.GetComponent<Spell4013HammerTransparencyController>().SetHammerBonusTransparencyRatio(GetHammerBonusTransaprencyRadio());
			hammerSprite.enabled = false;
			component3.enabled = false;
			hammerStickSprite.enabled = false;
			component4.enabled = false;
			if (num2)
			{
				hammerSprite.enabled = true;
				component3.enabled = true;
			}
			else
			{
				hammerStickSprite.enabled = true;
				component4.enabled = true;
			}
			break;
		}
		case "Shadow":
		{
			targetHammerShadow = trans;
			bool num = hammer.spellCfg.isSplitSpell || hammer.SplitHammerList.Count <= 0;
			float y = Mathf.Max(hammer.hammerMinWidthRatio, hammer.radiusRatio * hammer.finalRadiusRatio) * 1.5f * hammer.hammerBaseSpriteBaseWidthAndLength;
			SpriteRenderer component = trans.Find("HammerBase").GetComponent<SpriteRenderer>();
			component.size = new Vector2(hammer.hammerBaseSpriteBaseWidthAndLength, y);
			SpriteRenderer component2 = trans.Find("StickHammerBase").GetComponent<SpriteRenderer>();
			component2.size = new Vector2(hammer.hammerBaseSpriteBaseWidthAndLength, y);
			component.enabled = false;
			component2.enabled = false;
			if (num)
			{
				component.enabled = true;
			}
			else
			{
				component2.enabled = true;
			}
			break;
		}
		case "FallShadow":
			targetHammerShadow = trans;
			break;
		}
	}

	protected override Vector3 GetFallingExplosionLayerPoint(Vector3 worldPosition)
	{
		return Tool2D.GetLayerPoint(worldPosition, LayerCorrectType.GroundEffect);
	}

	protected override void Update()
	{
		base.Update();
		UpdateHammerShadowPositon();
	}

	private float GetHammerBonusTransaprencyRadio()
	{
		float num = 1f;
		num -= Mathf.Min((base.transform.localScale.x - 1f) * 0.1f, SizeMaxTransparencyDecreaseRatio);
		num -= Mathf.Min((float)(hammer.SIP.multiShootCount * (hammer.SplitCount + 1) - 1) * 0.02f, CountMaxTransparencyDecreaseRatio);
		return Mathf.Max(MinTransparency, num);
	}

	private void UpdateHammerTransparency()
	{
		float num = 1f;
		num -= Mathf.Min((base.transform.localScale.x - 1f) * 0.1f, SizeMaxTransparencyDecreaseRatio);
		num -= Mathf.Min((float)(hammer.SIP.multiShootCount * (hammer.SplitCount + 1) - 1) * 0.02f, CountMaxTransparencyDecreaseRatio);
		num = Mathf.Max(MinTransparency, num);
		hammerSprite.color = new Color(1f, 1f, 1f, num);
		hammerStickSprite.color = new Color(1f, 1f, 1f, num);
	}

	public void HideHammer()
	{
		if ((bool)targetHammer)
		{
			RecycleEffect(targetHammer);
			RecycleEffect(targetHammerShadow);
			targetHammer = null;
			targetHammerShadow = null;
		}
	}

	public void SpawnHammer()
	{
		if (!targetHammer)
		{
			ManualCreateEffect("Spell");
			ManualCreateEffect("Shadow");
		}
	}

	public void SpawnFallHammer()
	{
		if (!targetHammer)
		{
			ManualCreateEffect("FallSpell");
			ManualCreateEffect("FallShadow");
		}
	}

	private void UpdateHammerShadowPositon()
	{
		if ((bool)targetHammerShadow)
		{
			if (hammer.SIP.spellIsFall)
			{
				targetHammerShadow.transform.position = hammer.transform.position - new Vector3(0f, hammer.FallHammerHeight * hammer.GetFallCurveProgress(), 0f);
				Transform obj = targetHammerShadow.Find("Shadow");
				obj.localScale = Vector3.one * (0.6f + hammer.GetFallCurveProgress() * 0.4f);
				obj.localPosition = new Vector3(0f, 0f, 1.05f);
			}
			else
			{
				targetHammerShadow.Find("HammerBase").localPosition = new Vector3(0f, 0f, 1.05f);
			}
		}
	}

	private void OnDisable()
	{
		RecycleEffect(targetHammer);
		RecycleEffect(targetHammerShadow);
	}
}
