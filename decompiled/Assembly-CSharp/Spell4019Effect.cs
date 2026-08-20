using UnityEngine;

public class Spell4019Effect : SpellEffectBase
{
	private Spell4019BiAnLethalBlade biAnBlade;

	private Transform bladeTrans;

	private Transform bladeFrontTrans;

	private Transform bladeBackTrans;

	private SpriteRenderer bladeSprite;

	private Transform bladeTrailTrans;

	private Transform bladeTrailTrans2;

	private ParticleSystem bladeTrailParticle;

	private static readonly int EnableHiddenUnderGround = Shader.PropertyToID("_EnableHiddenUnderGround");

	private static readonly int EnableBodyOverlayColor = Shader.PropertyToID("_EnableBodyOverlayColor");

	private ParticleSystem voidTrailParticle;

	public float LightSaberLerpSpeed;

	private float targetLightSaberProgress;

	protected override void Awake()
	{
		base.Awake();
		biAnBlade = (Spell4019BiAnLethalBlade)base.Spell;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		bladeTrans = null;
		bladeFrontTrans = null;
		bladeSprite = null;
		bladeBackTrans = null;
		bladeTrailTrans = null;
		bladeTrailParticle = null;
		voidTrailParticle = null;
		targetLightSaberProgress = 0f;
	}

	protected override void Update()
	{
		base.Update();
		UpdateLightSaberProgress();
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		string text = effect.Name;
		if (!(text == "Spell"))
		{
			if (text == "Hit")
			{
				trans.right = biAnBlade.Direction;
				trans.position = biAnBlade.tsf_Layer.position + new Vector3(0f, biAnBlade.BladeHeightTrans.localPosition.y - biAnBlade.tsf_Layer.localPosition.y, 0f);
			}
			return;
		}
		bladeTrans = trans;
		bladeFrontTrans = trans.Find("BladeFront");
		bladeTrailTrans = bladeFrontTrans.Find("Trail");
		bladeTrailTrans2 = bladeTrailTrans.Find("Sparks");
		bladeBackTrans = bladeFrontTrans.Find("BladeBack");
		bladeTrailParticle = bladeFrontTrans.Find("TrailParticle").GetComponent<ParticleSystem>();
		if (biAnBlade.ColorType == SpellColorType.Void)
		{
			voidTrailParticle = bladeFrontTrans.Find("VoidParticle").GetComponent<ParticleSystem>();
		}
		bladeSprite = bladeBackTrans.Find("BladeSprite").GetComponent<SpriteRenderer>();
		bladeBackTrans.localEulerAngles = Vector3.zero;
		bladeFrontTrans.localEulerAngles = Vector3.zero;
		bladeSprite.material.SetFloat(EnableHiddenUnderGround, 0f);
		bladeSprite.material.SetFloat(EnableBodyOverlayColor, 0f);
		LightSaberSwitch(toggle: false, instanceChange: true);
		TrailSwitch(toggle: false);
		TrailParticleToggle(toggle: false);
	}

	public Transform GetBladeFrontTrasnform()
	{
		return bladeFrontTrans;
	}

	public Transform GetBladeBackTrasnform()
	{
		return bladeBackTrans;
	}

	public void SetBladeHiddenData(bool toggle, float hiddentHeight = 0f)
	{
		if ((bool)bladeSprite)
		{
			bladeSprite.material.SetFloat(EnableHiddenUnderGround, toggle ? 1 : 0);
			bladeSprite.material.SetFloat("_GroundHiddenHeight", (hiddentHeight == 0f) ? base.transform.position.y : hiddentHeight);
		}
	}

	public void TrailSwitch(bool toggle)
	{
		if ((bool)bladeTrailTrans)
		{
			bladeTrailTrans.GetComponent<TrailRenderer>().Clear();
			bladeTrailTrans2.GetComponent<TrailRenderer>().Clear();
			bladeTrailTrans.gameObject.SetActive(toggle);
		}
	}

	public void LightSaberSwitch(bool toggle, bool instanceChange = false)
	{
		if ((bool)bladeSprite)
		{
			if (instanceChange)
			{
				bladeSprite.material.SetFloat(EnableBodyOverlayColor, toggle ? 1 : 0);
			}
			targetLightSaberProgress = (toggle ? 1 : 0);
		}
	}

	public void ClearTrail()
	{
		if ((bool)bladeTrans)
		{
			SpellEffectBase.StopAllTrailRender(bladeTrans.gameObject);
		}
	}

	public void TrailParticleToggle(bool toggle)
	{
		if (!bladeTrailParticle)
		{
			return;
		}
		if (toggle)
		{
			bladeTrailParticle.Play();
			if ((bool)voidTrailParticle && !voidTrailParticle.isPlaying)
			{
				voidTrailParticle.Play();
			}
		}
		else
		{
			bladeTrailParticle.Stop();
			if ((bool)voidTrailParticle)
			{
				voidTrailParticle.Stop();
			}
		}
	}

	private void UpdateLightSaberProgress()
	{
		if ((bool)bladeSprite)
		{
			bladeSprite.material.SetFloat(EnableBodyOverlayColor, Mathf.Lerp(bladeSprite.material.GetFloat(EnableBodyOverlayColor), targetLightSaberProgress, LightSaberLerpSpeed * Time.deltaTime));
		}
	}
}
