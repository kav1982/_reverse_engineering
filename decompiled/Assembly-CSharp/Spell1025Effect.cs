using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class Spell1025Effect : SpellEffectBase
{
	private Spell1025DragonBreath flameScript;

	private readonly List<Transform> fallGroundEffects = new List<Transform>();

	private VisualEffectLineController VELC;

	private Transform VELCRotate;

	private ParticleSystem fireParticle;

	private ParticleSystem smokeParticle;

	private ParticleSystem emberParticle;

	private ParticleSystem voidFireParticle;

	public VariableFloat fireSpeedRatio;

	public float smokeSpeedRatio;

	public float emberSpeedRatio;

	public float AngleToFireEmitRatio;

	public float AnlgeToSmokeEmitRatio;

	public float angleToEmberEmitRatio;

	public float rangeToFireEmitRatio;

	public float rangeToSmokeEmitRatio;

	public float rangeToEmberEmitRatio;

	public VariableFloat fireEmitRange;

	public VariableFloat smokeEmitRange;

	public VariableFloat emberEmitRange;

	public float fallFireEmitPerRadiuRatio;

	public float fallFireEmitNumRatio;

	private Transform fallFireTrans;

	private Transform startTrans;

	protected override void Awake()
	{
		base.Awake();
		flameScript = (Spell1025DragonBreath)base.Spell;
	}

	protected override void OnEnable()
	{
		fallFireTrans = null;
		startTrans = null;
		fallGroundEffects.Clear();
	}

	protected override void Update()
	{
		base.Update();
		UpdateFireState();
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		switch (effect.Name)
		{
		case "Start":
			startTrans = trans.Find("Center");
			startTrans.localPosition = Vector3.zero;
			break;
		case "Spell":
		{
			fireParticle = trans.Find("Fire").GetComponent<ParticleSystem>();
			smokeParticle = trans.Find("Smoke").GetComponent<ParticleSystem>();
			emberParticle = trans.Find("Ember").GetComponent<ParticleSystem>();
			ParticleSystem.MainModule main5 = fireParticle.main;
			ParticleSystem.ShapeModule shape5 = fireParticle.shape;
			ParticleSystem.EmissionModule emission5 = fireParticle.emission;
			ParticleSystem.MainModule main6 = smokeParticle.main;
			ParticleSystem.ShapeModule shape6 = smokeParticle.shape;
			ParticleSystem.EmissionModule emission6 = smokeParticle.emission;
			ParticleSystem.MainModule main7 = emberParticle.main;
			ParticleSystem.ShapeModule shape7 = emberParticle.shape;
			ParticleSystem.EmissionModule emission7 = emberParticle.emission;
			float num2 = flameScript.currentAttackDistance / flameScript.minAttackDistanceRatio;
			main5.startSpeed = new ParticleSystem.MinMaxCurve(num2 * fireSpeedRatio.value1, num2 * fireSpeedRatio.value2);
			main6.startSpeed = new ParticleSystem.MinMaxCurve(num2 * smokeSpeedRatio);
			main7.startSpeed = new ParticleSystem.MinMaxCurve(num2 * emberSpeedRatio);
			shape5.arc = flameScript.wandShootAngle - 30f;
			shape6.arc = flameScript.wandShootAngle - 30f;
			shape7.arc = flameScript.wandShootAngle + 40f;
			float finalSpellTransparent2 = DataMgr.settingData.FinalSpellTransparent;
			finalSpellTransparent2 = Mathf.Pow(finalSpellTransparent2, 2f * (finalSpellTransparent2 + 0.3f));
			emission5.rateOverTime = Mathf.Clamp(Mathf.CeilToInt(shape5.arc * AngleToFireEmitRatio * (flameScript.maxAttackDistance * rangeToFireEmitRatio / flameScript.spellCfg.float1)), fireEmitRange.value1, fireEmitRange.value2) * finalSpellTransparent2;
			emission6.rateOverTime = Mathf.Clamp(Mathf.CeilToInt(shape6.arc * AnlgeToSmokeEmitRatio * (flameScript.maxAttackDistance * rangeToSmokeEmitRatio / flameScript.spellCfg.float1)), smokeEmitRange.value1, smokeEmitRange.value2) * finalSpellTransparent2;
			emission7.rateOverTime = Mathf.Clamp(Mathf.CeilToInt(shape7.arc * angleToEmberEmitRatio * (flameScript.maxAttackDistance * rangeToEmberEmitRatio / flameScript.spellCfg.float1)), emberEmitRange.value1, emberEmitRange.value2) * finalSpellTransparent2;
			fireParticle.transform.right = Tool2D.GetDir(flameScript.Direction, (0f - shape5.arc) / 2f);
			smokeParticle.transform.right = Tool2D.GetDir(flameScript.Direction, (0f - shape6.arc) / 2f);
			emberParticle.transform.right = Tool2D.GetDir(flameScript.Direction, (0f - shape7.arc) / 2f);
			if (flameScript.ColorType == SpellColorType.Void)
			{
				voidFireParticle = trans.Find("FireVoid").GetComponent<ParticleSystem>();
				ParticleSystem.MainModule main8 = voidFireParticle.main;
				ParticleSystem.EmissionModule emission8 = voidFireParticle.emission;
				ParticleSystem.ShapeModule shape8 = voidFireParticle.shape;
				main8.startSpeed = new ParticleSystem.MinMaxCurve(num2 * fireSpeedRatio.value1, num2 * fireSpeedRatio.value2);
				shape8.arc = flameScript.wandShootAngle - 30f;
				emission8.rateOverTime = Mathf.Clamp(Mathf.CeilToInt(shape5.arc * AngleToFireEmitRatio * (flameScript.maxAttackDistance * rangeToFireEmitRatio / flameScript.spellCfg.float1)), fireEmitRange.value1, fireEmitRange.value2) * finalSpellTransparent2;
				voidFireParticle.transform.right = Tool2D.GetDir(flameScript.Direction, (0f - shape5.arc) / 2f);
			}
			break;
		}
		case "FallSpellGround":
		{
			fireParticle = trans.Find("Fire").GetComponent<ParticleSystem>();
			smokeParticle = trans.Find("Smoke").GetComponent<ParticleSystem>();
			emberParticle = trans.Find("Ember").GetComponent<ParticleSystem>();
			ParticleSystem.MainModule main9 = fireParticle.main;
			ParticleSystem.EmissionModule emission9 = fireParticle.emission;
			ParticleSystem.MainModule main10 = smokeParticle.main;
			ParticleSystem.EmissionModule emission10 = smokeParticle.emission;
			ParticleSystem.MainModule main11 = emberParticle.main;
			ParticleSystem.EmissionModule emission11 = emberParticle.emission;
			float num3 = fallFireEmitPerRadiuRatio * flameScript.GetFallDamageRange() * fallFireEmitNumRatio;
			float constant = flameScript.GetFallDamageRange() / 0.5f;
			main9.startSpeed = new ParticleSystem.MinMaxCurve(constant);
			main10.startSpeed = new ParticleSystem.MinMaxCurve(constant);
			main11.startSpeed = new ParticleSystem.MinMaxCurve(constant);
			float finalSpellTransparent3 = DataMgr.settingData.FinalSpellTransparent;
			finalSpellTransparent3 = Mathf.Pow(finalSpellTransparent3, 2f * (finalSpellTransparent3 + 0.3f));
			emission9.rateOverTime = num3 * finalSpellTransparent3;
			emission10.rateOverTime = num3 * finalSpellTransparent3;
			emission11.rateOverTime = num3 * finalSpellTransparent3;
			break;
		}
		case "RotationSpell":
		{
			fireParticle = trans.Find("Fire").GetComponent<ParticleSystem>();
			smokeParticle = trans.Find("Smoke").GetComponent<ParticleSystem>();
			emberParticle = trans.Find("Ember").GetComponent<ParticleSystem>();
			ParticleSystem.EmissionModule emission = fireParticle.emission;
			ParticleSystem.EmissionModule emission2 = smokeParticle.emission;
			ParticleSystem.EmissionModule emission3 = emberParticle.emission;
			ParticleSystem.ShapeModule shape = fireParticle.shape;
			ParticleSystem.ShapeModule shape2 = smokeParticle.shape;
			ParticleSystem.ShapeModule shape3 = emberParticle.shape;
			ParticleSystem.MainModule main = fireParticle.main;
			ParticleSystem.MainModule main2 = smokeParticle.main;
			ParticleSystem.MainModule main3 = emberParticle.main;
			float finalSpellTransparent = DataMgr.settingData.FinalSpellTransparent;
			finalSpellTransparent = Mathf.Pow(finalSpellTransparent, 2f * (finalSpellTransparent + 0.3f));
			float spellAroundOwnerRadius = flameScript.spellAroundOwnerRadius;
			float num = flameScript.radiusRatio * flameScript.finalRadiusRatio;
			main.startSize = 2f * num;
			main2.startSize = 2f * num;
			main3.startSize = 0.1f * num;
			shape.radius = spellAroundOwnerRadius;
			shape2.radius = spellAroundOwnerRadius;
			shape3.radius = spellAroundOwnerRadius;
			emission.rateOverTime = 200f * spellAroundOwnerRadius * finalSpellTransparent;
			emission2.rateOverTime = 20f * spellAroundOwnerRadius * finalSpellTransparent;
			emission3.rateOverTime = 70f * spellAroundOwnerRadius * finalSpellTransparent;
			if (flameScript.ColorType == SpellColorType.Void)
			{
				voidFireParticle = trans.Find("FireVoid").GetComponent<ParticleSystem>();
				ParticleSystem.MainModule main4 = voidFireParticle.main;
				ParticleSystem.EmissionModule emission4 = voidFireParticle.emission;
				ParticleSystem.ShapeModule shape4 = voidFireParticle.shape;
				main4.startSize = 2f * num;
				shape4.radius = spellAroundOwnerRadius;
				emission4.rateOverTime = 200f * spellAroundOwnerRadius * finalSpellTransparent;
			}
			break;
		}
		case "FallingSpell":
		{
			fallFireTrans = trans.Find("RotateCenter");
			Vector3 localPosition = new Vector3(0f, 7f, -0.5f) - flameScript.Direction * (7f / GameConst.spellFallAngleTan);
			fallFireTrans.localPosition = localPosition;
			fallFireTrans.transform.right = Tool2D.GetDir(new Vector3(0f, -1f, 0f), (flameScript.Direction.x > 0f) ? 15f : (-15f));
			if ((bool)startTrans)
			{
				startTrans.localPosition = localPosition;
			}
			break;
		}
		case "FireLine":
		{
			VELC = trans.GetComponent<VisualEffectLineController>();
			VisualEffect[] effects = VELC.Effects;
			for (int i = 0; i < effects.Length; i++)
			{
				effects[i].SetFloat("SizeMul", 1f);
			}
			VELCRotate = trans.Find("Rotate");
			break;
		}
		case "FireGround":
			fallGroundEffects.Add(trans);
			break;
		}
	}

	public void UpdateFireState()
	{
		if (!(fireParticle == null) && !(smokeParticle == null) && !flameScript.SIP.spellIsFall && flameScript.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			ParticleSystem.MainModule main = fireParticle.main;
			main.startSpeed = new ParticleSystem.MinMaxCurve(flameScript.currentAttackDistance * fireSpeedRatio.value1, flameScript.currentAttackDistance * fireSpeedRatio.value2);
			if (flameScript.ColorType == SpellColorType.Void)
			{
				ParticleSystem.MainModule main2 = voidFireParticle.main;
				main2.startSpeed = new ParticleSystem.MinMaxCurve(flameScript.currentAttackDistance * fireSpeedRatio.value1, flameScript.currentAttackDistance * fireSpeedRatio.value2);
			}
			ParticleSystem.MainModule main3 = smokeParticle.main;
			main3.startSpeed = new ParticleSystem.MinMaxCurve(flameScript.currentAttackDistance * smokeSpeedRatio);
		}
	}

	public void UpdateFallFireLine(Vector3[] points)
	{
		if ((bool)VELC)
		{
			VELC.SetPositions(points, 24f, 0.2f);
		}
		if ((bool)VELCRotate)
		{
			Vector3 right = points[1] - points[0];
			right.z = 0f;
			VELCRotate.right = right;
		}
	}

	public void UpdateFallGround(Vector3[] groundPoints)
	{
		if (SpellEffectBase.FullTransparency)
		{
			return;
		}
		SpellEffectSettings effects = Effects.First((SpellEffectSettings e) => e.Name == "FallGround");
		int i = 0;
		float fallDamageRange = flameScript.GetFallDamageRange();
		for (; i < groundPoints.Length; i++)
		{
			Vector3 layerPoint = Tool2D.GetLayerPoint(groundPoints[i]);
			if (i >= fallGroundEffects.Count)
			{
				fallGroundEffects.Add(GetEffectGoFromPool(effects).transform);
			}
			fallGroundEffects[i].position = layerPoint;
			fallGroundEffects[i].localScale = Vector3.one * fallDamageRange;
		}
		while (i < fallGroundEffects.Count)
		{
			ObjPoolMgr inst = ObjPoolMgr.Inst;
			List<Transform> list = fallGroundEffects;
			inst.RecycleGO(list[list.Count - 1].gameObject);
			fallGroundEffects.RemoveAt(fallGroundEffects.Count - 1);
		}
	}

	private void OnDisable()
	{
		if ((bool)VELC)
		{
			VisualEffect[] effects = VELC.Effects;
			for (int i = 0; i < effects.Length; i++)
			{
				effects[i].DoFloat("SizeMul", 0f, 0.25f);
			}
			VELC = null;
		}
		foreach (Transform fallGroundEffect in fallGroundEffects)
		{
			ParticleSystem[] componentsInChildren = fallGroundEffect.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Stop();
			}
			ObjPoolMgr.Inst.RecycleGO(fallGroundEffect.gameObject, 2f);
		}
		fallGroundEffects.Clear();
	}
}
