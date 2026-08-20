using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Elite9_Mine : MonoBehaviour
{
	[Header("发射")]
	public bool dropped;

	private float horizontalSpeed;

	public VariableFloat upSpeed;

	private float nowUpSpeed;

	public float gravity;

	private Vector3 diration;

	[Header("感应")]
	public float range;

	public int damage;

	public bool startExplode;

	public bool exploded;

	public float knockback;

	public bool muted;

	public GameObject warningRing;

	public GameObject warningCircle;

	public SpriteRenderer bombSprite;

	public SpriteRenderer coreSprite;

	public VariableFloat coreRotateSpeed;

	public Transform warningScaleRoot;

	public float checkInterval;

	private float checkTimer;

	public float explodeDelay;

	private float explodeTimer;

	public ParticleSystem trailParticle;

	public ParticleSystem explodeParticle;

	private Shadow shadow;

	public Animator anima;

	public Transform modelTransform;

	private Vector2 berlinSeed;

	public float shakeFrequency;

	public float shakeAmplitude;

	private Vector3 originModelLocalPosition;

	public Transform spriteRoot;

	public float rotateSpeed;

	private float rotateRight;

	public Elite9 master;

	public ShockParam shockParam;

	public AudioSource as_explode;

	public float muteTime;

	[Header("发光")]
	public Light2D glow;

	public float originGlowRadius;

	public float originGlowStrength;

	public float glowRadiusHeightFix;

	public float glowStrengthHeightFix;

	public float explodeGlowRadius;

	public float explodeGlowStrength;

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void OnEnable()
	{
		exploded = false;
		dropped = false;
		startExplode = false;
		muted = false;
		checkTimer = 0f;
		explodeTimer = 0f;
		warningRing.SetActive(value: false);
		warningCircle.SetActive(value: false);
		warningCircle.transform.localScale = Vector3.zero;
		trailParticle.Play();
		warningScaleRoot.transform.localScale = Vector3.one * range;
		bombSprite.enabled = true;
		coreSprite.enabled = true;
		upSpeed.RandomResult();
		coreRotateSpeed.RandomResult();
		nowUpSpeed = upSpeed.result;
		shadow = GetComponent<Shadow>();
		if (shadow.ShadowGO != null)
		{
			shadow.Show();
		}
		anima.Play("Elite9_MineFly");
		originModelLocalPosition = modelTransform.localPosition;
		berlinSeed = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
		rotateRight = ((!(UnityEngine.Random.Range(0f, 1f) > 0.5f)) ? 1 : (-1));
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
		glow.enabled = true;
		glow.pointLightOuterRadius = 0f;
		glow.intensity = 0f;
	}

	private void OnDisable()
	{
		modelTransform.localPosition = originModelLocalPosition;
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_explode.volume = DataMgr.settingData.GetFinalSound();
	}

	public void SetTarget(Vector3 targetPoint, float initialHeight)
	{
		horizontalSpeed = GeneralTool.CannonSpeed(nowUpSpeed, initialHeight, gravity, Tool2D.IgnoreZPoint(base.transform.position - targetPoint).magnitude);
		diration = (targetPoint - base.transform.position).normalized;
	}

	public void ManualMute()
	{
		if (!exploded)
		{
			muted = true;
			anima.Play("Elite9_MineMute");
			warningRing.SetActive(value: false);
			warningCircle.SetActive(value: false);
			trailParticle.Stop();
			shadow.Hide();
			explodeTimer = 0f;
		}
	}

	private void Update()
	{
		glow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.GroundEffect);
		if (muted)
		{
			glow.pointLightOuterRadius = Mathf.Lerp(originGlowRadius, 0f, explodeTimer / muteTime);
			glow.intensity = Mathf.Lerp(originGlowStrength, 0f, explodeTimer / muteTime);
			explodeTimer += Time.deltaTime;
			if (explodeTimer > 3f)
			{
				Elite9.MiniPool.RecycleGO(base.gameObject);
			}
			return;
		}
		if (!dropped)
		{
			glow.pointLightOuterRadius = Mathf.Max(0f, originGlowRadius - Mathf.Abs(base.transform.position.z) * glowRadiusHeightFix);
			glow.intensity = Mathf.Max(0f, originGlowStrength - Mathf.Abs(base.transform.position.z) * glowStrengthHeightFix);
			spriteRoot.transform.localEulerAngles += new Vector3(0f, 0f, rotateRight * rotateSpeed * Time.deltaTime);
			nowUpSpeed += Time.deltaTime * gravity;
			base.transform.position += new Vector3(0f, 0f, (0f - nowUpSpeed) * Time.deltaTime) + diration * horizontalSpeed * Time.deltaTime;
			if (base.transform.position.z > 0f)
			{
				SEMgr.Inst.itemDropBase.PlaySE();
				base.transform.position = Tool2D.IgnoreZPoint(base.transform.position);
				dropped = true;
				anima.Play("Elite9_MineIdle");
			}
			return;
		}
		coreSprite.transform.right = Tool2D.GetDir(coreSprite.transform.right, coreRotateSpeed.result * Time.deltaTime);
		if (!startExplode)
		{
			checkTimer += Time.deltaTime;
			if (checkTimer > checkInterval)
			{
				checkTimer = 0f;
				Checktarget();
			}
		}
		else if (!exploded)
		{
			glow.pointLightOuterRadius = Mathf.Lerp(originGlowRadius, explodeGlowRadius, explodeTimer / explodeDelay);
			glow.intensity = Mathf.Lerp(originGlowStrength, explodeGlowStrength, explodeTimer / explodeDelay);
			explodeTimer += Time.deltaTime;
			warningCircle.transform.localScale = Vector3.one * explodeTimer / explodeDelay;
			Vector2 vector = berlinSeed * explodeTimer * shakeFrequency;
			float x = Mathf.PerlinNoise(vector.x, vector.y) - 0.5f;
			float y = Mathf.PerlinNoise(vector.y, vector.x) - 0.5f;
			modelTransform.localPosition = originModelLocalPosition + new Vector3(x, y, 0f) * shakeAmplitude * explodeTimer / explodeDelay;
			if (explodeTimer > explodeDelay)
			{
				as_explode.Stop();
				warningRing.SetActive(value: false);
				warningCircle.SetActive(value: false);
				bombSprite.enabled = false;
				coreSprite.enabled = false;
				exploded = true;
				trailParticle.Stop();
				Explode();
				shadow.Hide();
				glow.enabled = false;
			}
		}
		else
		{
			explodeTimer += Time.deltaTime;
			if (explodeTimer > explodeDelay + 3f)
			{
				Elite9.MiniPool.RecycleGO(base.gameObject);
			}
		}
	}

	private void Checktarget()
	{
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, range, GameConst.Filter_Friendly, targetsInRange);
		if (targetsInRange.Count > 0)
		{
			as_explode.Play();
			anima.Play("Elite9_MineExplode");
			warningRing.SetActive(value: true);
			warningCircle.SetActive(value: true);
			startExplode = true;
		}
	}

	private void Explode()
	{
		SEMgr.Inst.monster34Explosion.PlaySE();
		CamController.Inst.SetShock(shockParam);
		Elite9.MiniPool.GetGO("Prefabs/EF/EF_Monster34_Trace", base.transform.position, 10f);
		explodeParticle.Play();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, range, GameConst.Filter_MonsterAoe, targetsInRange);
		for (int i = 0; i < targetsInRange.Count; i++)
		{
			Entity entity = targetsInRange[i].entity;
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = targetsInRange[i];
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite9.Inst.myPpt.myEntity);
				info.damage = damage;
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * knockback;
				info.teammateTakeDamageRatio = 3f;
				UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				break;
			}
			}
		}
	}
}
