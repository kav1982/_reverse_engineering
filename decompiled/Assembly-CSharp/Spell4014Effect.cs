using UnityEngine;

public class Spell4014Effect : SpellEffectBase
{
	private Spell4014LaserCrystal crystal;

	public float BodyDustEmitPowerUpRatio;

	public float BodyDustMaxSpeed;

	public float BodyAuraTimeToScaleRatio;

	public float BodyAuraMaxScale;

	public float NormalCrystalTrailLife;

	public float SplitCrystalTrailLife;

	public float NormalCrystalTrailWidth;

	public float SplitCrystalTrailWidth;

	private SpriteRenderer crystalBodySprite;

	private ParticleSystem bodyDustPowerUpParticle;

	private ParticleSystem.MainModule bodyDustPowerUpParticleMain;

	private Transform powerUpAuraTrans;

	private Transform afterTwoSecondsTrans;

	private Transform afterFiveSecondsTrans;

	private Transform afterTenSecondsTrans;

	private Transform spellEffectTrans;

	private LineRenderer laserLine;

	private LineRenderer shadowLine;

	private TrailRenderer crystalTrail;

	private static readonly int LaserSwitch = Shader.PropertyToID("_LaserSwitch");

	private Transform startNodeTrans;

	private Transform endNodeTrans;

	public Vector3 EffectHeightshift;

	public float lerpRatio;

	public Transform shadowTrans;

	private static readonly int Transparency = Shader.PropertyToID("_Transparency");

	protected override void Awake()
	{
		base.Awake();
		crystal = (Spell4014LaserCrystal)base.Spell;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		spellEffectTrans = null;
		bodyDustPowerUpParticle = null;
		bodyDustPowerUpParticleMain = default(ParticleSystem.MainModule);
		powerUpAuraTrans = null;
		afterTwoSecondsTrans = null;
		afterFiveSecondsTrans = null;
		afterTenSecondsTrans = null;
		laserLine = null;
		shadowLine = null;
		startNodeTrans = null;
		crystalTrail = null;
		endNodeTrans = null;
		shadowTrans = null;
		crystalBodySprite = null;
	}

	protected override void Update()
	{
		base.Update();
		UpdatePowerUpParticleState();
		UpdatePowerUpLaserState();
		UpdateStartNode();
		UpdateEndNode();
		UpdateSpellPosition();
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		switch (effect.Name)
		{
		case "Spell":
			spellEffectTrans = trans;
			crystalBodySprite = trans.Find("Crystal").GetComponent<SpriteRenderer>();
			bodyDustPowerUpParticle = trans.Find("PowerUpDust").GetComponent<ParticleSystem>();
			powerUpAuraTrans = trans.Find("PowerUpAura");
			bodyDustPowerUpParticleMain = bodyDustPowerUpParticle.main;
			afterTwoSecondsTrans = trans.Find("After2Sec");
			afterFiveSecondsTrans = trans.Find("After5Sec");
			afterTenSecondsTrans = trans.Find("After10Sec");
			afterTwoSecondsTrans.gameObject.SetActive(value: false);
			afterFiveSecondsTrans.gameObject.SetActive(value: false);
			afterTenSecondsTrans.gameObject.SetActive(value: false);
			crystalTrail = trans.Find("Trail").GetComponent<TrailRenderer>();
			crystalTrail.time = (crystal.spellCfg.isSplitSpell ? SplitCrystalTrailLife : NormalCrystalTrailLife);
			crystalTrail.startWidth = (crystal.spellCfg.isSplitSpell ? SplitCrystalTrailWidth : NormalCrystalTrailWidth);
			break;
		case "Laser":
			laserLine = trans.GetComponent<LineRenderer>();
			shadowLine = trans.Find("Shadow").GetComponent<LineRenderer>();
			shadowLine.enabled = !crystal.SIP.spellIsFall || crystal.currentSpellMovement != SpellSpecialMovementType.Rotation;
			break;
		case "Start":
			startNodeTrans = trans;
			break;
		case "End":
			endNodeTrans = trans;
			break;
		case "Hit":
			trans.localScale = Vector3.one * crystal.GetCurrentLaserWidth();
			trans.position += EffectHeightshift;
			break;
		case "Shadow":
			shadowTrans = trans;
			shadowTrans.localScale = (crystal.spellCfg.isSplitSpell ? (Vector3.one * 0.6f) : Vector3.one);
			break;
		}
	}

	private void UpdateSpellPosition()
	{
		if ((bool)spellEffectTrans)
		{
			spellEffectTrans.position = crystal.CenterTransform.position + EffectHeightshift;
			shadowTrans.position = (crystal.SIP.spellIsFall ? (crystal.CenterTransform.position + new Vector3(0f, 0f - crystal.fallBaseHeight, crystal.fallBaseHeight)) : crystal.CenterTransform.position);
			if (crystal.SIP.spellIsFall && crystal.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				shadowTrans.position += new Vector3(0f, -0.3f, 0f);
			}
			shadowTrans.position = Tool2D.IgnoreZPoint(shadowTrans.position, 1.05f);
			UpdataSpellFaceDirection();
		}
	}

	private void UpdataSpellFaceDirection()
	{
		if ((bool)crystalBodySprite)
		{
			if (crystal.SIP.spellIsFall && crystal.currentSpellMovement != SpellSpecialMovementType.Rotation && crystal.currentSpellMovement != SpellSpecialMovementType.ChaseOwner)
			{
				crystalBodySprite.flipX = crystal.endPos.x < crystal.CenterTransform.position.x;
			}
			else
			{
				crystalBodySprite.flipX = crystal.Direction.x < 0f;
			}
		}
	}

	public void ClearCrystalTrail()
	{
		if ((bool)crystalTrail)
		{
			crystalTrail.Clear();
		}
	}

	private void UpdatePowerUpParticleState()
	{
		float attackTotalTime = crystal.attackTotalTime;
		if (!(afterTwoSecondsTrans == null) && !(afterFiveSecondsTrans == null) && !(afterTenSecondsTrans == null))
		{
			afterTwoSecondsTrans.gameObject.SetActive(attackTotalTime >= 2f);
			afterFiveSecondsTrans.gameObject.SetActive(attackTotalTime >= 5f);
			afterTenSecondsTrans.gameObject.SetActive(attackTotalTime >= 10f);
			bodyDustPowerUpParticleMain.startSpeed = Mathf.Min(attackTotalTime * BodyDustEmitPowerUpRatio, BodyDustMaxSpeed);
			powerUpAuraTrans.localScale = Vector3.one * Mathf.Min(attackTotalTime * BodyAuraTimeToScaleRatio, BodyAuraMaxScale);
		}
	}

	private void UpdateStartNode()
	{
		if ((bool)startNodeTrans)
		{
			Vector3 vector = (crystal.SIP.spellIsFall ? new Vector3(0f, -1f, 0f) : crystal.Direction);
			startNodeTrans.position = crystal.CenterTransform.position + EffectHeightshift + vector * crystal.LaserNodeAroundCenterDistance;
			float num = Mathf.Lerp(endNodeTrans.localScale.x, crystal.attackedInThisFrame ? crystal.GetCurrentLaserWidth() : crystal.LaserAttackBaseWidth, lerpRatio);
			startNodeTrans.localScale = Vector3.one * num;
		}
	}

	private void UpdateEndNode()
	{
		if (crystal.rayNodes.Count > 0 && (bool)endNodeTrans)
		{
			endNodeTrans.position = crystal.rayNodes[crystal.rayNodes.Count - 1];
			if (!crystal.SIP.spellIsFall)
			{
				endNodeTrans.position += EffectHeightshift;
			}
			float num = Mathf.Lerp(endNodeTrans.localScale.x, crystal.attackedInThisFrame ? crystal.GetCurrentLaserWidth() : crystal.LaserAttackBaseWidth, lerpRatio * 3f);
			endNodeTrans.localScale = Vector3.one * num;
		}
	}

	private void UpdatePowerUpLaserState()
	{
		if (!laserLine)
		{
			return;
		}
		int num = ((!crystal.attackedInThisFrame) ? 1 : 0);
		laserLine.material.SetFloat(LaserSwitch, num);
		shadowLine.material.SetFloat(LaserSwitch, num);
		laserLine.material.SetFloat(Transparency, DataMgr.settingData.FinalSpellTransparent);
		shadowLine.material.SetFloat(Transparency, DataMgr.settingData.FinalSpellTransparent);
		float num2 = Mathf.Lerp(laserLine.startWidth, crystal.attackedInThisFrame ? crystal.GetCurrentLaserWidth() : crystal.LaserAttackBaseWidth, lerpRatio);
		num2 = crystal.GetCurrentLaserWidth();
		if (crystal.spellCfg.isSplitSpell)
		{
			num2 *= 0.35f;
		}
		laserLine.startWidth = num2;
		laserLine.endWidth = num2;
		shadowLine.startWidth = num2;
		shadowLine.endWidth = num2;
		if (crystal.CrystalIsAttacking)
		{
			laserLine.positionCount = crystal.rayNodes.Count;
			shadowLine.positionCount = crystal.rayNodes.Count;
			int num3 = 0;
			{
				foreach (Vector3 rayNode in crystal.rayNodes)
				{
					Vector3 position = rayNode + new Vector3(0f, EffectHeightshift.y, crystal.tsf_Layer.position.z + crystal.CenterTransform.localPosition.z + 1f);
					if (crystal.SIP.spellIsFall && crystal.rayNodes.IndexOf(rayNode) == crystal.rayNodes.Count - 1)
					{
						position = rayNode.IgnoreZ();
					}
					laserLine.SetPosition(num3, position);
					if (crystal.SIP.spellIsFall)
					{
						int num4 = crystal.rayNodes.IndexOf(rayNode);
						if (num4 >= 0)
						{
							shadowLine.SetPosition(num3, Tool2D.IgnoreZPoint(crystal.fallShadowRayNodes[num4], 1.05f));
						}
					}
					else
					{
						shadowLine.SetPosition(num3, Tool2D.IgnoreZPoint(rayNode, 1.05f));
					}
					num3++;
				}
				return;
			}
		}
		laserLine.positionCount = 0;
	}
}
