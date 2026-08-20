using System;
using DG.Tweening;
using UnityEngine;

public class Spell1023Effect : SpellEffectBase
{
	[Serializable]
	public class constData
	{
		[Tooltip("剑身渐入时的材质变化的时间")]
		public float bladeStartDissolveTime;

		[Tooltip("剑身淡出时的材质变化的时间")]
		public float bladeEndDissolveTime;

		[Tooltip("剑身淡出时额外移动的距离")]
		public VariableFloat bladEndExtraMoveDistance;

		[Tooltip("剑身淡出时额外移动的时间")]
		public float bladEndExtraMoveTime;

		[Tooltip("剑身影子物体")]
		public GameObject bladeShadowObject;

		[Tooltip("剑身影子贴图")]
		public SpriteRenderer bladeShadowSprite;

		[Tooltip("剑身脚本")]
		public Spell1023JudgementBlade bladeScript;
	}

	private static int progressId = Shader.PropertyToID("_Progress");

	private static int shadowTransparencyId = Shader.PropertyToID("_Transparency");

	private static int shadowProgressId = Shader.PropertyToID("_ShadowSize");

	private static int shadowWidthRatioId = Shader.PropertyToID("_WidthRatio");

	public constData param = new constData();

	private Transform bladeTrans;

	private Transform bladeTransCenter;

	private Transform bladeTrailTrans;

	private static readonly int EnableHiddenUnderGround = Shader.PropertyToID("_EnableHiddenUnderGround");

	private static readonly int GroundHiddenHeight = Shader.PropertyToID("_GroundHiddenHeight");

	private SpriteRenderer fallBladeSprite;

	protected override void OnEnable()
	{
		base.OnEnable();
		param.bladeShadowSprite.material.DOFloat(0.4f, shadowTransparencyId, param.bladeStartDissolveTime);
		param.bladeShadowSprite.material.SetFloat(shadowProgressId, 0.13f);
		param.bladeShadowObject.transform.localPosition = new Vector3(0f, 0f, Tool2D.GetLayerPoint(param.bladeShadowObject.transform, LayerCorrectType.Shadow).z);
		bladeTrans = null;
		bladeTransCenter = null;
		bladeTrailTrans = null;
		fallBladeSprite = null;
	}

	private void OnDisable()
	{
		param.bladeShadowSprite.material.DOFloat(0f, shadowTransparencyId, param.bladeStartDissolveTime);
	}

	public void ShadowStartChange(float time = 0f)
	{
		if (time == 0f)
		{
			time = param.bladeStartDissolveTime;
		}
		param.bladeShadowSprite.material.DOFloat(1f / param.bladeShadowSprite.material.GetFloat(shadowWidthRatioId), shadowProgressId, time);
	}

	public void ShadowRotate(Vector3 dir)
	{
		param.bladeShadowObject.transform.right = dir;
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		string text = effect.Name;
		if (!(text == "Spell"))
		{
			if (text == "Trail")
			{
				bladeTrailTrans = trans;
			}
			return;
		}
		bladeTrans = trans;
		bladeTransCenter = trans.Find("BladeCenter");
		SpriteRenderer component = trans.GetComponent<SpriteRenderer>();
		component.material.SetFloat(progressId, 0f);
		component.material.DOFloat(1f, progressId, param.bladeStartDissolveTime);
		fallBladeSprite = component;
		if (param.bladeScript.SIP.spellIsFall && param.bladeScript.rebounceTime <= 0)
		{
			fallBladeSprite.material.SetFloat(EnableHiddenUnderGround, 1f);
			fallBladeSprite.material.SetFloat(GroundHiddenHeight, param.bladeScript.SIP.finalShootSpatialInfo.Target.Value.y);
		}
		else
		{
			fallBladeSprite.material.SetFloat(EnableHiddenUnderGround, 0f);
		}
	}

	protected override void OnWillRecycleEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnWillRecycleEffect(effect, trans);
		if (!(effect.Name == "Spell"))
		{
			return;
		}
		SpriteRenderer component = trans.GetComponent<SpriteRenderer>();
		Spell1023JudgementBlade spell1023JudgementBlade = (Spell1023JudgementBlade)base.Spell;
		if (!spell1023JudgementBlade.SIP.spellIsFall)
		{
			if (spell1023JudgementBlade.currentState == Spell1023JudgementBlade.BladeState.AfterShoot || spell1023JudgementBlade.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				component.transform.DOMove(component.transform.position + component.transform.right * param.bladEndExtraMoveDistance.RandomResult() * 0.75f, param.bladEndExtraMoveTime / 2f);
			}
			else
			{
				component.transform.DOMove(component.transform.position - component.transform.right * param.bladEndExtraMoveDistance.RandomResult(), param.bladEndExtraMoveTime);
			}
		}
		trans.GetComponent<ParticleSystem>().Play();
		component.material.DOFloat(2f, progressId, param.bladeEndDissolveTime);
	}

	protected override void Update()
	{
		base.Update();
		UpdateFallHiddenHeight();
		UpdateTrailPosition();
	}

	private void UpdateFallHiddenHeight()
	{
		if ((bool)fallBladeSprite && param.bladeScript.isFlyFinish)
		{
			fallBladeSprite.material.SetFloat(EnableHiddenUnderGround, 1f);
			fallBladeSprite.material.SetFloat(GroundHiddenHeight, param.bladeScript.transform.position.y);
			param.bladeShadowSprite.material.SetFloat(shadowProgressId, 0.13f);
		}
	}

	private void UpdateTrailPosition()
	{
		if ((bool)bladeTrailTrans && (bool)bladeTransCenter)
		{
			bladeTrailTrans.position = bladeTransCenter.position;
		}
	}

	private void UpdateFallBladeRotation()
	{
		switch (param.bladeScript.currentState)
		{
		case Spell1023JudgementBlade.BladeState.Spawn:
		case Spell1023JudgementBlade.BladeState.DetectingTarget:
			bladeTrans.eulerAngles = new Vector3(0f, 0f, -90f);
			break;
		case Spell1023JudgementBlade.BladeState.LockingTarget:
			bladeTrans.right = param.bladeScript.GetFallingBladeToTargetDirection();
			break;
		case Spell1023JudgementBlade.BladeState.AfterShoot:
		{
			float z = Vector2.SignedAngle(to: new Vector2(base.Spell.Direction.x * base.Spell.CurrentSpeed, base.Spell.CurrentUpSpeed + base.Spell.Direction.y * base.Spell.CurrentSpeed), from: Vector2.right);
			Quaternion rotation = Quaternion.Euler(0f, 0f, z);
			bladeTrans.rotation = rotation;
			break;
		}
		}
	}

	protected override void UpdateRotation(Transform trans, SpellEffectSettings effect)
	{
		if (trans != bladeTrans || !param.bladeScript.SIP.spellIsFall)
		{
			base.UpdateRotation(trans, effect);
		}
		else
		{
			UpdateFallBladeRotation();
		}
	}
}
