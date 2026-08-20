using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss5_Wave : MonoBehaviour
{
	public float moveSpeed;

	public float maxSpeed;

	private float nowSpeed;

	public float acceleration;

	public float attackRadius;

	public float attackInterval;

	private float attackTimer;

	public int damage;

	public float knockback;

	private List<Entity> attackedEntities = new List<Entity>();

	public ParticleSystem moveParticle;

	public bool fade;

	private bool growing;

	public float scaleSpeed;

	private float nowSacle;

	public SpriteRenderer mainSprite;

	public Sprite HotizonWave1;

	public Sprite HotizonWave2;

	public Sprite UpWave1;

	public Sprite UpWave2;

	public Sprite DownWave1;

	public Sprite DownWave2;

	public float waveSwitchInterval;

	private float waveSwitchTimer;

	private int spriteIndex;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private bool isFrame1;

	private Vector3 lastRecordPoint;

	private FourDir moveDir;

	public AudioSource waveLoop;

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		waveLoop.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			maxSpeed *= 0.8f;
			acceleration *= 0.8f;
			moveSpeed *= 0.8f;
		}
	}

	public void Initialize(FourDir diration)
	{
		SEMgr.Inst.monster42Land.PlaySE();
		SEMgr.Inst.boss5_Wave.PlaySE();
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		moveDir = diration;
		attackedEntities.Clear();
		mainSprite.enabled = true;
		fade = false;
		moveParticle.Play();
		nowSpeed = moveSpeed;
		if (moveDir == FourDir.Right)
		{
			mainSprite.flipX = false;
		}
		else if (moveDir == FourDir.Left)
		{
			mainSprite.flipX = true;
		}
		isFrame1 = true;
		mainSprite.transform.localScale = Vector3.zero;
		growing = true;
	}

	private void Update()
	{
		if (isFrame1)
		{
			isFrame1 = false;
			lastRecordPoint = base.transform.position;
		}
		else
		{
			WaterSystem.CreateWater(base.transform.position, lastRecordPoint, 0.5f);
			lastRecordPoint = base.transform.position;
		}
		if (growing && !fade)
		{
			nowSacle += Time.deltaTime * scaleSpeed;
			if (nowSacle > 1f)
			{
				growing = false;
			}
			mainSprite.transform.localScale = nowSacle * Vector3.one;
		}
		if (!fade)
		{
			if (nowSpeed < maxSpeed)
			{
				nowSpeed += Time.deltaTime * acceleration;
			}
			base.transform.position += Tool2D.GetDirByFourDir(moveDir) * nowSpeed * Time.deltaTime;
			attackTimer += Time.deltaTime;
		}
		else
		{
			nowSacle -= Time.deltaTime * scaleSpeed;
			if (nowSacle < 0f)
			{
				nowSacle = 0f;
				waveLoop.Stop();
			}
			mainSprite.transform.localScale = nowSacle * Vector3.one;
		}
		waveSwitchTimer += Time.deltaTime;
		if (waveSwitchTimer > waveSwitchInterval)
		{
			waveSwitchTimer = 0f;
			if (spriteIndex == 0)
			{
				spriteIndex = 1;
				if (moveDir == FourDir.Left || moveDir == FourDir.Right)
				{
					mainSprite.sprite = HotizonWave1;
				}
				else if (moveDir == FourDir.Down)
				{
					mainSprite.sprite = DownWave1;
				}
				else if (moveDir == FourDir.Up)
				{
					mainSprite.sprite = UpWave1;
				}
			}
			else
			{
				spriteIndex = 0;
				if (moveDir == FourDir.Left || moveDir == FourDir.Right)
				{
					mainSprite.sprite = HotizonWave2;
				}
				else if (moveDir == FourDir.Down)
				{
					mainSprite.sprite = DownWave2;
				}
				else if (moveDir == FourDir.Up)
				{
					mainSprite.sprite = UpWave2;
				}
			}
		}
		if (attackTimer > attackInterval)
		{
			attackTimer = 0f;
			AttackOnce();
		}
		Vector3 vector = base.transform.position - roomCenterPoint;
		if ((Mathf.Abs(vector.x) > roomWidth / 2f + 0.5f || Mathf.Abs(vector.y) > roomHeight / 2f + 0.5f) && !fade)
		{
			fade = true;
			moveParticle.Stop();
		}
	}

	private void AttackOnce()
	{
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, attackRadius, GameConst.Filter_MonsterAoe, results);
		for (int i = 0; i < results.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = results[i];
			Entity entity = distanceHitResult.entity;
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
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss5.Inst.myPpt.myEntity);
					info.damage = damage;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * knockback;
					info.teammateTakeDamageRatio = 3f;
					if (!attackedEntities.Contains(distanceHitResult.entity))
					{
						attackedEntities.Add(distanceHitResult.entity);
						UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
						SEMgr.Inst.boss5_WaveHit.PlaySE();
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_Hit", distanceHitResult.point, 3f);
					}
				}
				break;
			}
		}
	}
}
