using System;
using UnityEngine;

public class Monster34 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Stop,
		Random,
		Wait,
		Forward,
		Detonate
	}

	public float chargeEffectSize = 2f;

	public float forwardTurnRate = 1f;

	public ParticleSystem alert;

	public ParticleSystem fire;

	public Transform bone9Transform;

	public float forwardTime = 1f;

	public float forwardSpeedMultiply = 7f;

	public float waitTime = 1f;

	private float waitTimer;

	private float forwardTimer;

	private Vector3 curDir;

	public float bombTime = 0.5f;

	private float bombTimer;

	public float idleInterval = 4f;

	private float idleTimer;

	public float turnDirInterval = 4f;

	private float turnDirTimer;

	public float backTime = 0.2f;

	public float backSpeed = 4f;

	private bool startShine;

	private float shineIntervalTime = 0.2f;

	private float shineTimer;

	public SpriteRenderer spriteRenderer;

	public AudioSource audioSource;

	public GameObject model;

	private Vector3 originModelScale;

	private Vector3 originModelLocalPosition;

	private Vector2 berlinSeed;

	[Header("Pattern2")]
	public AIPattern pattern;

	public int fragmentAmount;

	public VariableFloat fragmentDistance;

	private MonsterState state = MonsterState.Random;

	private MonsterState preState;

	private MonsterState tempState;

	[Header("和谐")]
	public SpriteRenderer sr;

	public Sprite sprite_H;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundChange));
		SoundChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundChange));
	}

	private void SoundChange()
	{
		audioSource.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		originModelScale = model.transform.localScale;
		originModelLocalPosition = model.transform.localPosition;
		if (pattern == AIPattern.Pattern2 && GameMgr.IsHarmony_Static)
		{
			sr.sprite = sprite_H;
		}
	}

	public override void EveryInitialCallback()
	{
		waitTimer = 0f;
		startShine = false;
		curDir = Tool2D.GetDir();
		state = MonsterState.BornIdle;
		base.Anima.Play("Monster34_idle");
		model.transform.localPosition = originModelLocalPosition;
		model.transform.localScale = originModelScale;
		berlinSeed = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
		spriteRenderer.material.SetColor("_ShineColor", new Color(0f, 0f, 0f, 0f));
		bornIdleTimer = 0f;
	}

	public override void Update()
	{
		Vector3 vector = bone9Transform.position - base.transform.position;
		if (base.IsFlipped)
		{
			fire.transform.localPosition = new Vector3(0f - vector.x, vector.y, 0f);
		}
		else
		{
			fire.transform.position = bone9Transform.position;
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		bool flag = false;
		preState = tempState;
		tempState = state;
		if (preState != state)
		{
			flag = true;
		}
		if (startShine)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
			if (shineTimer >= shineIntervalTime)
			{
				shineTimer = 0f;
			}
			if (shineTimer == 0f)
			{
				spriteRenderer.material.SetColor("_ShineColor", new Color(1f, 1f, 1f, 1f));
			}
			if (shineTimer > shineIntervalTime / 2f)
			{
				spriteRenderer.material.SetColor("_ShineColor", new Color(0f, 0f, 0f, 0f));
			}
			shineTimer += Time.deltaTime;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			if (flag)
			{
				base.Anima.Play("Monster34_idle");
				bornIdleTimer = 0f;
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.Random;
			}
			break;
		case MonsterState.Stop:
			if (flag)
			{
				base.Anima.Play("Monster34_idle");
				idleTimer = 0f;
			}
			SetMove(Vector3.zero);
			idleTimer += Time.deltaTime;
			if (idleTimer > idleInterval)
			{
				idleTimer = 0f;
				if (UnityEngine.Random.Range(0, 10) < 5)
				{
					state = MonsterState.Random;
				}
			}
			break;
		case MonsterState.Random:
			if (flag)
			{
				base.Anima.Play("Monster34_walk");
				curDir = Tool2D.GetDir();
				turnDirTimer = 0f;
			}
			turnDirTimer += Time.deltaTime;
			if (turnDirTimer > turnDirInterval)
			{
				turnDirTimer = 0f;
				if (UnityEngine.Random.Range(0, 10) >= 5)
				{
					state = MonsterState.Stop;
					break;
				}
				curDir = Tool2D.GetDir();
			}
			GetNavInfo(curDir * 4f);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			break;
		case MonsterState.Wait:
			if (flag)
			{
				alert.Play();
				base.Anima.Play("Monster34_jump");
				waitTimer = 0f;
			}
			waitTimer += Time.deltaTime;
			SetMove(Vector3.zero);
			if (waitTimer > waitTime)
			{
				state = MonsterState.Forward;
			}
			break;
		case MonsterState.Forward:
			if (flag)
			{
				fire.Play();
				base.Anima.Play("Monster34_wait");
				forwardTimer = 0f;
			}
			startShine = true;
			forwardTimer += Time.deltaTime;
			if (forwardTimer > forwardTime)
			{
				state = MonsterState.Detonate;
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				SetMove(Vector3.zero);
				break;
			}
			if (targetPpt == PlayerMgr.Inst.PlayerPpt && !PlayerMgr.Inst.PlayerCtrller.IsVisible)
			{
				SetMove(Vector3.zero);
				break;
			}
			GetNavInfo(base.TargetPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * forwardSpeedMultiply);
			if ((base.transform.position - PlayerMgr.Inst.PlayerPoint).sqrMagnitude < 1f)
			{
				state = MonsterState.Detonate;
			}
			break;
		case MonsterState.Detonate:
		{
			if (flag)
			{
				bombTimer = 0f;
				base.Anima.Play("Monster34_wait");
			}
			startShine = true;
			bombTimer += Time.deltaTime;
			SetMove(Vector3.zero);
			if (bombTimer > bombTime)
			{
				DotsAnnouncedDeath();
				break;
			}
			Vector2 vector2 = berlinSeed * bombTimer * 16f;
			float x = Mathf.PerlinNoise(vector2.x, vector2.y) - 0.5f;
			float y = Mathf.PerlinNoise(vector2.y, vector2.x) - 0.5f;
			model.transform.localPosition = originModelLocalPosition + new Vector3(x, y, 0f) * 2f * bombTimer / bombTime;
			model.transform.localScale = originModelScale * Mathf.Lerp(1f, 1.3f, bombTimer / bombTime);
			break;
		}
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		targetEntity = info.attackerEntity;
		if (state != MonsterState.Wait && state != MonsterState.Detonate && state != MonsterState.Forward)
		{
			state = MonsterState.Wait;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (GetComponentData<UnitProperty_Dots>().unitCfg.currentHP <= 0f)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Fragment", Tool2D.IgnoreZPoint(base.transform)).GetComponent<Monster34_Fragment>().Iniaitlize(base.transform.position + Tool2D.GetDir() * fragmentDistance.RandomResult(), 0, explodeNow: true);
		}
		else if (pattern == AIPattern.Pattern2)
		{
			for (int i = 0; i < fragmentAmount; i++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Fragment", Tool2D.IgnoreZPoint(base.transform)).GetComponent<Monster34_Fragment>().Iniaitlize(base.transform.position + Tool2D.GetDir(45 + 90 * i) * fragmentDistance.RandomResult(), GameMgr.IsMobile_Static ? 2 : 3);
			}
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Fragment", Tool2D.IgnoreZPoint(base.transform)).GetComponent<Monster34_Fragment>().Iniaitlize(base.transform.position + Tool2D.GetDir() * fragmentDistance.RandomResult(), 0, explodeNow: true);
		}
		else
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Explosion", Tool2D.IgnoreZPoint(base.transform), 20f);
		}
	}
}
