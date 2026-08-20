using DG.Tweening;
using UnityEngine;

public class Spell1021Effect : SpellEffectBase
{
	private static int progressId = Shader.PropertyToID("_Process");

	private Spell1021MagicBreaker bladeScript;

	private ParticleSystem trailParticle;

	private ParticleSystem trailEmberParticle;

	private SpriteRenderer bladeSprite;

	private SpriteRenderer bladeShadowSprite;

	public Transform bladeCenterSprite;

	private SpriteRenderer fallBladeSprite;

	public VariableFloat fallBladeUnderGroundPercentInterval;

	private float fallBladeUnderGroundRatio;

	public float bladeSpriteEdgeLength;

	public AnimationCurve fadeInCurve;

	public AnimationCurve fadeOutCurve;

	public float fadeInTime;

	public float fadeOutTime;

	private static readonly int EnableHiddenUnderGround = Shader.PropertyToID("_EnableHiddenUnderGround");

	private static readonly int GroundHiddenHeight = Shader.PropertyToID("_GroundHiddenHeight");

	private Transform fallBladeTransform;

	private Transform fallTraceTransform;

	private Transform shadowTrans;

	protected override void Awake()
	{
		base.Awake();
		bladeScript = (Spell1021MagicBreaker)base.Spell;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		shadowTrans = null;
	}

	private void OnDisable()
	{
		trailParticle = null;
		trailEmberParticle = null;
		bladeSprite = null;
		fallBladeSprite = null;
		fallBladeUnderGroundRatio = 0f;
		fallBladeTransform = null;
	}

	protected override void Update()
	{
		base.Update();
		UpdateFallHiddenHeight();
		UpdateFallBladeRotation();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		UpdateShadowPosition();
	}

	private void UpdateShadowPosition()
	{
		if ((bool)shadowTrans)
		{
			shadowTrans.position = base.transform.position + shadowTrans.right * 0.3f * base.transform.localScale.x;
		}
	}

	private void UpdateFallBladeRotation()
	{
		if ((bool)fallBladeSprite && !bladeScript.isFlyFinish && (bool)fallBladeTransform)
		{
			float z = Vector2.SignedAngle(to: new Vector2(base.Spell.Direction.x * base.Spell.CurrentSpeed, base.Spell.CurrentUpSpeed + base.Spell.Direction.y * base.Spell.CurrentSpeed), from: Vector2.right);
			Quaternion rotation = Quaternion.Euler(0f, 0f, z);
			fallBladeTransform.rotation = rotation;
		}
	}

	private void UpdateFallHiddenHeight()
	{
		if ((bool)fallBladeSprite && bladeScript.isFlyFinish)
		{
			fallBladeSprite.material.SetFloat(EnableHiddenUnderGround, 1f);
			fallBladeSprite.material.SetFloat(GroundHiddenHeight, bladeScript.transform.position.y);
		}
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		switch (effect.Name)
		{
		case "NormalShadow":
		{
			shadowTrans = trans;
			bladeShadowSprite = trans.Find("Shadow").GetComponent<SpriteRenderer>();
			bladeShadowSprite.flipY = bladeScript.counterclockMovement > 0;
			float num3 = (1f + bladeScript.InitialParameter.extraSizeRatio) * bladeScript.InitialParameter.finalSizeRatio;
			bladeShadowSprite.size = new Vector2(bladeSpriteEdgeLength * num3, bladeSpriteEdgeLength);
			shadowTrans.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, 1f);
			float num4 = bladeScript.spellAroundOwnerRadius * bladeScript.radiusRatio * bladeScript.finalRadiusRatio;
			trans.Find("Shadow").gameObject.transform.localPosition = new Vector3(num4 - 0.3f, 0f, 900f);
			break;
		}
		case "Spell":
		{
			bladeSprite = trans.GetComponent<SpriteRenderer>();
			trailParticle = trans.Find("Trail").GetComponent<ParticleSystem>();
			trailEmberParticle = trans.Find("Ember").GetComponent<ParticleSystem>();
			bladeSprite.flipY = bladeScript.counterclockMovement > 0;
			bladeSprite.material.SetFloat(0, progressId);
			bladeSprite.material.DOFloat(1f, progressId, fadeInTime).SetEase(fadeInCurve);
			ParticleSystem.ShapeModule shape = trailParticle.shape;
			ParticleSystem.ShapeModule shape2 = trailEmberParticle.shape;
			float num2 = Mathf.Max(bladeScript.minEffectRadiuRatio, (1f + bladeScript.InitialParameter.extraSizeRatio) * bladeScript.InitialParameter.finalSizeRatio);
			float x = bladeSpriteEdgeLength / 2f * Mathf.Max(bladeScript.minEffectRadiuRatio, num2);
			trailParticle.gameObject.transform.localPosition = new Vector3(x, 0f, trailParticle.gameObject.transform.localPosition.z);
			trailEmberParticle.gameObject.transform.localPosition = new Vector3(x, 0f, trailEmberParticle.gameObject.transform.localPosition.z);
			shape.radius = num2;
			shape2.radius = num2;
			shape2.rotation = new Vector3(0f, 0f, (bladeScript.counterclockMovement < 0) ? 180 : 0);
			if (bladeScript.ColorType == SpellColorType.Thunder)
			{
				ParticleSystem component = trans.Find("ThunderTrail").GetComponent<ParticleSystem>();
				ParticleSystem component2 = trans.Find("ThunderEmber").GetComponent<ParticleSystem>();
				shape = component.shape;
				shape2 = component2.shape;
				component.gameObject.transform.localPosition = new Vector3(x, 0f, component.gameObject.transform.localPosition.z);
				component2.gameObject.transform.localPosition = new Vector3(x, 0f, component2.gameObject.transform.localPosition.z);
				shape.radius = num2;
				shape2.radius = num2;
			}
			bladeSprite.size = new Vector2(bladeSpriteEdgeLength * num2, bladeSpriteEdgeLength);
			break;
		}
		case "SpellFall":
		{
			fallBladeUnderGroundRatio = fallBladeUnderGroundPercentInterval.RandomResult();
			fallBladeTransform = trans;
			fallBladeSprite = trans.Find("Blade").GetComponent<SpriteRenderer>();
			fallBladeSprite.flipY = bladeScript.counterclockMovement > 0;
			fallBladeSprite.material.SetFloat(0, progressId);
			fallBladeSprite.material.DOFloat(1f, progressId, fadeInTime).SetEase(fadeInCurve);
			if (bladeScript.rebounceTime <= 0)
			{
				fallBladeSprite.material.SetFloat(EnableHiddenUnderGround, 1f);
				fallBladeSprite.material.SetFloat(GroundHiddenHeight, bladeScript.SIP.finalShootSpatialInfo.Target.Value.y);
			}
			else
			{
				fallBladeSprite.material.SetFloat(EnableHiddenUnderGround, 0f);
			}
			float num = (1f + bladeScript.InitialParameter.extraSizeRatio) * bladeScript.InitialParameter.finalSizeRatio;
			fallBladeSprite.size = new Vector2(bladeSpriteEdgeLength * num, bladeSpriteEdgeLength);
			fallBladeSprite.transform.localPosition = new Vector3((0f - fallBladeSprite.size.x) * fallBladeUnderGroundRatio, 0f, 0f);
			break;
		}
		case "Shadow":
			trans.position = base.gameObject.transform.position;
			trans.localScale = new Vector3(bladeScript.transform.localScale.x, bladeScript.transform.localScale.y, 1f);
			break;
		case "GroundTrace":
			fallTraceTransform = trans;
			fallTraceTransform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = true;
			fallTraceTransform.localScale = new Vector3(bladeScript.transform.localScale.x, bladeScript.transform.localScale.y, 1f);
			break;
		}
	}

	protected override void OnWillRecycleEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnWillRecycleEffect(effect, trans);
		switch (effect.Name)
		{
		case "Spell":
			bladeSprite.material.DOFloat(0f, progressId, fadeOutTime).SetEase(fadeOutCurve);
			trailParticle.Stop();
			trailEmberParticle.Stop();
			break;
		case "SpellFall":
			fallBladeSprite.material.DOFloat(0f, progressId, fadeOutTime * 2f).SetEase(fadeOutCurve);
			break;
		case "GroundTrace":
			fallTraceTransform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = false;
			fallTraceTransform.Find("Particle").GetComponent<ParticleSystem>().Play();
			break;
		}
	}
}
