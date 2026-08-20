using DG.Tweening;
using UnityEngine;

public class Spell1024Effect : SpellEffectBase
{
	private Spell1024GiantBubble bubble;

	private Transform bubbleTrans;

	private Shadow shadowScript;

	private Transform bubbleRangeAura;

	public float bubbleGrowSpeed;

	public float bubbleMaxSizeRatio;

	public float bubbleCollapseSizeRatio;

	public float defaultShadowScale;

	protected override void Awake()
	{
		base.Awake();
		bubble = (Spell1024GiantBubble)base.Spell;
		shadowScript = GetComponent<Shadow>();
	}

	private void OnDisable()
	{
		bubbleTrans = null;
		bubbleRangeAura = null;
		shadowScript.shadowScale = defaultShadowScale;
	}

	protected override void Update()
	{
		base.Update();
		UpdateBubbleSizeState();
		UpdateBubbleRangeState();
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		switch (effect.Name)
		{
		case "Explosion":
			trans.localScale = Vector3.one * bubble.spellCfg.radius * 2f;
			break;
		case "ExplosionGround":
			trans.localScale = Vector3.one * bubble.spellCfg.radius * 2f;
			trans.position -= new Vector3(0f, effect.AttachTarget.localPosition.y, 0f);
			break;
		case "Spell":
			if (trans == null)
			{
				Debug.LogError("泡泡本体的transform去哪里了？");
			}
			bubbleTrans = trans;
			bubbleTrans.localScale = Vector3.one * Mathf.Pow(bubble.radiusRatio * bubble.finalRadiusRatio, 0.3333f);
			break;
		case "ChargeEnd":
			bubbleStartCollapse();
			break;
		case "EndRain":
		{
			ParticleSystem.ShapeModule shape3 = trans.Find("Rain1").GetComponent<ParticleSystem>().shape;
			shape3.radius = bubble.spellCfg.radius;
			break;
		}
		case "EndRainGround":
		{
			ParticleSystem.ShapeModule shape = trans.Find("Rain2").GetComponent<ParticleSystem>().shape;
			ParticleSystem.ShapeModule shape2 = trans.Find("Rain3").GetComponent<ParticleSystem>().shape;
			shape.radius = bubble.spellCfg.radius;
			shape2.radius = bubble.spellCfg.radius;
			break;
		}
		case "EffectRange":
			bubbleRangeAura = trans;
			break;
		}
	}

	private void bubbleStartCollapse()
	{
		if (!(bubbleTrans == null))
		{
			bubbleTrans.DOScale(bubbleTrans.localScale.x * bubbleCollapseSizeRatio, bubble.bubbleCollapseTime);
			shadowScript.ShadowGO.transform.DOScale(defaultShadowScale * bubbleTrans.localScale.x * bubbleCollapseSizeRatio, bubble.bubbleCollapseTime).SetEase(Ease.OutSine);
		}
	}

	private void UpdateBubbleSizeState()
	{
		if (!bubble.collapseEnd && (bool)bubbleTrans)
		{
			float num = Mathf.Min(bubbleMaxSizeRatio, bubbleTrans.localScale.x + bubbleGrowSpeed * bubble.radiusRatio * bubble.finalRadiusRatio * Time.deltaTime);
			bubbleTrans.localScale = Vector3.one * num;
			shadowScript.ShadowGO.transform.localScale = Vector3.one * defaultShadowScale * num;
		}
	}

	private void UpdateBubbleRangeState()
	{
		if ((bool)bubbleRangeAura)
		{
			bubbleRangeAura.position = bubble.transform.position + new Vector3(0f, 0f, 1.08f);
			bubbleRangeAura.localScale = Vector3.one * base.Spell.spellCfg.radius;
		}
	}
}
