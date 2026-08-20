using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Elite11 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		AfterAttack,
		BlockAfterAttack,
		BeforeSummon,
		Summon,
		AfterSummon,
		Cannon,
		ChaseLaserBefore,
		ChaseLaser,
		BeforeLaser,
		AfterLaser,
		Laser,
		WaveBullet,
		BlockBullet,
		RotateBullet,
		CrossBullet,
		CrossSpike,
		Bullet,
		RingBullet
	}

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("召唤物")]
	public static Elite11 Inst;

	public Elite11_Spawner spawner;

	public ParticleSystem summonParticle;

	public ShockParam summonShock;

	public List<Elite11_Child> children = new List<Elite11_Child>();

	public float summonTime;

	[Header("召唤子弹")]
	public float trackBulletSpeed;

	public float trackBulletRotateSpeed;

	public int trackSpellDamage;

	public VariableInt trackSpellCount;

	public float trackSpellLifeTime;

	private SpellInitialParameter sipBullet1 = new SpellInitialParameter();

	[Header("基础设定")]
	public float startAttackDistance;

	public float startLaserDistance;

	[Header("二阶段")]
	public float secondStageHealthRatio;

	public bool inSecondStage;

	[Header("休息")]
	public VariableFloat idleTime;

	private float afterAttackTime;

	[Header("选技能")]
	public float bulletChance;

	public float laserChance;

	public float cannonChance;

	public float chaseLaserChance;

	public float waveBulletChance;

	public float blockBulletChance;

	public float corssSpikeChance;

	public float rotateBulletChance;

	public float crossBulletChance;

	public float summonChance;

	public float noSummonLimit;

	[Header("双重交错旋转子弹")]
	public float crossAfterTime;

	public float crossBulletDirInterval;

	public float crossBulletShootDirations;

	public float crossBulletShootRounds;

	public float crossBulletRoundsInterval;

	public float crossBulletRotateSpeed;

	public float crossBulletLifeTime;

	public float crossBulletSpeed;

	[Header("交错旋转子弹")]
	public float waveAfterTime;

	public float waveBulletDirInterval;

	public float waveBulletShootDirations;

	public float waveBulletShootRounds;

	public float waveBulletRoundsInterval;

	public float waveBulletRotateSpeed;

	public float waveBulletLifeTime;

	public float waveBulletSpeed;

	[Header("改版触手封锁")]
	public List<Elite11_Tentacle> tentacles = new List<Elite11_Tentacle>();

	[Header("子弹封锁")]
	public float blockAfterTime;

	public float blockRandomBulletInterval;

	public VariableFloat blockRandomBulletSpeed;

	public int blockBulletDirations;

	public float blockBulletMaxAngle;

	public float blockBulletRotateFrequency;

	public AnimationCurve blockBulletRotateCurve;

	public float blockBulletInterval;

	public float blockBulletWaves;

	public float blockBulletSpeed;

	[Header("侧向旋转排子弹，快")]
	public float rotateAfterTime;

	public float rotateBulletCount;

	public float rotateBulletInterval;

	public float rotateBulletSpeed;

	public float rotateBulletDirations;

	public float rotateBulletRounds;

	[Header("子弹阵")]
	public float bulletShootInterval;

	public int bulletCount;

	public float bulletAngleInterval;

	public float bulletShootDirations;

	public float bulletShootRounds;

	public float bulletRoundsInterval;

	public float bulletLifeTime;

	public float bulletSpeed;

	private bool bulletRotateClockWise;

	[Header("激光干扰子弹")]
	public float laserSpellHeight;

	public VariableInt laserBulletDamage;

	public float laserBulletCount;

	public VariableFloat laserBulletLifeTime;

	public VariableFloat laserBulletDistance;

	public VariableFloat laserBulletSlowDownTime;

	private SpellSpawnParams ssp1;

	private SpellSpawnParams ssp2;

	[Header("大激光")]
	public float chaseLaserAcclerateTime;

	public AnimationCurve chaseLaserAcclerateCurve;

	public float chaseLaserSpeed;

	public float chaseLaserPrepareAngle;

	public float chaseLaserWaitTime;

	public float chaseLaserTime;

	private Elite11_Laser largeLaser;

	public ParticleSystem bigLaserChargeParticle;

	public ParticleSystem bigLaserShootParticle;

	public ParticleSystem bigLaserChargeParticle_H;

	public ParticleSystem bigLaserShootParticle_H;

	[Header("激光扫射")]
	public ShockParam laserShock;

	public ParticleSystem laserShootParticle;

	public ParticleSystem laserShootParticle_H;

	public float laserInterval;

	public int laserRounds;

	private List<Elite11_Laser> lasers = new List<Elite11_Laser>();

	private List<Elite11_Laser> lasers1 = new List<Elite11_Laser>();

	private bool laserGroup1;

	public float laserCount;

	public float laserRange;

	public VariableFloat laserRangeOffset;

	private int laserCounter;

	[Header("前后旋转尖刺")]
	public float crossSpikeDirations;

	public float crossSpikeInterval;

	public float crossSpikeSpeed;

	public float crossSpikeTime;

	[Header("爆弹抛射")]
	public int cannonCount;

	public int cannonShootRounds;

	public float cannonShootInterval;

	public float cannonInitialHeight;

	public float cannonAfterTime;

	public float cannonRandomChance;

	public VariableFloat cannonAngleRange;

	public VariableFloat cannonDistanceRange;

	private int cannonCounter;

	private bool cannonFinished;

	[Header("spine相关")]
	public SkeletonAnimation SAnimaTop;

	public SkeletonAnimation SAnimaMiddle;

	public SkeletonAnimation SAnimaBottom;

	public Transform tsf_MiddleCenter;

	public Transform tsf_mask;

	public Transform tsf_TopCenter;

	public MeshRenderer SMesh;

	[Header("对象池")]
	public static MiniObjPool MiniPool;

	[Header("音效")]
	public AudioSource as_Tentacle;

	public AudioSource as_TentacleGround;

	private MonsterState lastSkill;

	private MonsterState lastSkillQuick;

	private bool isQuick = true;

	private List<MonsterState> skills = new List<MonsterState>
	{
		MonsterState.WaveBullet,
		MonsterState.BlockBullet,
		MonsterState.BeforeLaser,
		MonsterState.RotateBullet,
		MonsterState.CrossBullet
	};

	private float extraCannonCount;

	public MonsterState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
			varMgr.Clear();
		}
	}

	public static Vector3 elite11Position => Inst.transform.position;

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
		as_Tentacle.volume = DataMgr.settingData.GetFinalSound();
		as_TentacleGround.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		ssp1 = UnitDotsSyncSystem.GetSpellPrototype(90441);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp1);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Duration = bulletLifeTime;
		sSPModifier.ApplyToSSP(ref ssp1);
		ssp2 = UnitDotsSyncSystem.GetSpellPrototype(90221);
		sSPModifier = UnitBase.GetSSPModifier(in ssp2);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Duration = bulletLifeTime;
		sSPModifier.ApplyToSSP(ref ssp2);
		if (GameMgr.IsChAge14_Static)
		{
			bigLaserShootParticle = bigLaserShootParticle_H;
			bigLaserChargeParticle = bigLaserChargeParticle_H;
			laserShootParticle = laserShootParticle_H;
		}
		if (GameMgr.IsMobile_Static)
		{
			chaseLaserSpeed *= 0.85f;
			laserBulletCount *= 0.8f;
			blockRandomBulletInterval *= 1.25f;
			crossBulletShootDirations -= 2f;
			waveBulletShootDirations -= 2f;
			rotateBulletDirations -= 2f;
			laserRounds--;
			laserCount -= 1f;
		}
	}

	public override void EveryInitialCallback()
	{
		if (MiniPool == null)
		{
			MiniPool = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.CurrentRoomT).GetComponent<MiniObjPool>();
		}
		base.transform.position = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		Inst = this;
		for (int i = 0; (float)i < laserCount; i++)
		{
			lasers.Add(MiniPool.GetGO("Prefabs/EF/EF_Elite11_Laser", base.transform.position).GetComponent<Elite11_Laser>());
			lasers1.Add(MiniPool.GetGO("Prefabs/EF/EF_Elite11_Laser", base.transform.position).GetComponent<Elite11_Laser>());
		}
		largeLaser = MiniPool.GetGO("Prefabs/EF/EF_Elite11_LaserBig", base.transform.position).GetComponent<Elite11_Laser>();
		tentacles.Clear();
		for (int j = 0; j < blockBulletDirations; j++)
		{
			Elite11_Tentacle component = MiniPool.GetGO("Prefabs/EF/EF_Elite11_Tentacle").GetComponent<Elite11_Tentacle>();
			tentacles.Add(component);
			component.Initialize();
		}
		inSecondStage = false;
		idleTime.RandomResult();
		MiniPool.GetGO("Prefabs/EF/EF_Elite11_Bottom", base.transform.position);
	}

	public override void Frame1InitialCallback()
	{
	}

	private void LateUpdate()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		myPpt.MR_Models[0].GetPropertyBlock(materialPropertyBlock);
		if (materialPropertyBlock.GetColor("_Color") != myPpt.BaseColor)
		{
			materialPropertyBlock.SetColor("_Color", myPpt.BaseColor);
			for (int i = 0; i < myPpt.MR_Models.Length; i++)
			{
				myPpt.MR_Models[i].SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	public override void Update()
	{
		if (SAnimaBottom.timeScale != base.SAnima.timeScale || SAnimaTop.timeScale != base.SAnima.timeScale)
		{
			SAnimaBottom.timeScale = base.SAnima.timeScale;
			SAnimaMiddle.timeScale = base.SAnima.timeScale;
			SAnimaTop.timeScale = base.SAnima.timeScale;
		}
		SAnimaMiddle.transform.localPosition = new Vector3(0f, 0f, Mathf.Min(0.1f, 0f - (tsf_MiddleCenter.position.y - base.transform.position.y) + 0.1f) * 0.01f);
		SAnimaTop.transform.localPosition = new Vector3(0f, 0f, Mathf.Min(-0.2f, 0f - (tsf_TopCenter.position.y - base.transform.position.y)) * 0.01f);
		SAnimaBottom.transform.localPosition = new Vector3(0f, 0f, Mathf.Min(0.11f, SAnimaMiddle.transform.localPosition.z + 0.1f) * 0.01f);
		tsf_mask.localPosition = new Vector3(tsf_mask.localPosition.x, tsf_mask.localPosition.y, SAnimaMiddle.transform.localPosition.z + -0.002f);
		for (int num = children.Count - 1; num >= 0; num--)
		{
			if (children[num].myPpt.AlreadyDead)
			{
				children.RemoveAt(num);
			}
		}
		if (tentacles[0].state == Elite11_Tentacle.TentacleState.WaveMute && as_Tentacle.isPlaying)
		{
			as_Tentacle.Stop();
			as_TentacleGround.Stop();
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				SAnimaTop.AnimationState.SetAnimation(0, "Idle", loop: true);
				SAnimaMiddle.AnimationState.SetAnimation(0, "Idle", loop: true);
				SAnimaBottom.AnimationState.SetAnimation(0, "Idle_H", loop: true);
				base.Anima.Play("Idle");
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 1f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				SAnimaTop.AnimationState.SetAnimation(0, "Idle", loop: true);
				SAnimaMiddle.AnimationState.SetAnimation(0, "Idle", loop: true);
				SAnimaBottom.AnimationState.SetAnimation(0, "Idle_H", loop: true);
				base.Anima.Play("Idle");
			}
			if (base.CurrentHPRatio < secondStageHealthRatio && !inSecondStage)
			{
				state = MonsterState.BeforeSummon;
			}
			else if (stateExistTime > idleTime.result)
			{
				idleTime.RandomResult();
				ChooseSkill();
			}
			break;
		case MonsterState.AfterAttack:
			if (changedState)
			{
				SAnimaTop.AnimationState.SetAnimation(0, "ContinueAttackAfter", loop: false);
				SAnimaMiddle.AnimationState.SetAnimation(0, "ContinueAttackAfter", loop: false);
				base.Anima.Play("ContinueAttackAfter");
			}
			if (stateExistTime > afterAttackTime)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.ChaseLaserBefore:
			if (changedState)
			{
				base.Anima.Play("LerpBullet");
				SAnimaTop.AnimationState.SetAnimation(0, "LerpBullet", loop: false);
				SAnimaMiddle.AnimationState.SetAnimation(0, "LerpBullet", loop: false);
				SEMgr.Inst.elite11Grow.PlaySE();
			}
			break;
		case MonsterState.ChaseLaser:
		{
			ref float reference15 = ref varMgr.RegFloat(0);
			_ = ref varMgr.RegFloat(1);
			ref bool reference16 = ref varMgr.RegBool(0);
			if (changedState)
			{
				reference16 = GeneralTool.ChanceResult(0.5f);
				base.Anima.Play("BigLaserCharge");
				SAnimaTop.AnimationState.SetAnimation(0, "BigLaserCharge", loop: true);
				SAnimaMiddle.AnimationState.SetAnimation(0, "BigLaserCharge", loop: true);
				GetNearestTargetPlayerFirst();
				Vector3 to = Tool2D.GetDir();
				if (base.HaveTarget)
				{
					to = ToTargetDir();
				}
				reference15 = Tool2D.IgnoreZAngleWithSign(Vector3.up, to);
				reference15 += (reference16 ? (0f - chaseLaserPrepareAngle) : chaseLaserPrepareAngle);
				largeLaser.InitializeLarge(reference15, chaseLaserWaitTime, chaseLaserTime - chaseLaserWaitTime, startLaserDistance);
				bigLaserChargeParticle.Play();
				SEMgr.Inst.elite11BigLaserCharge.PlaySE();
			}
			if (stateExistTime < chaseLaserTime - 1f && stateExistTime > chaseLaserWaitTime)
			{
				reference15 += chaseLaserSpeed * chaseLaserAcclerateCurve.Evaluate(stateExistTime / chaseLaserAcclerateTime) * Time.deltaTime * (float)(reference16 ? 1 : (-1));
			}
			else if (stateExistTime > chaseLaserTime - 1f)
			{
				reference15 += chaseLaserSpeed * chaseLaserAcclerateCurve.Evaluate((chaseLaserTime - stateExistTime) / chaseLaserAcclerateTime) * Time.deltaTime * (float)(reference16 ? 1 : (-1));
			}
			largeLaser.SetAngle(reference15);
			if (stateExistTime > chaseLaserTime)
			{
				state = MonsterState.AfterLaser;
			}
			break;
		}
		case MonsterState.BeforeLaser:
			if (changedState)
			{
				SEMgr.Inst.elite11Grow.PlaySE();
				base.Anima.Play("BeforeLaser");
				SAnimaTop.AnimationState.SetAnimation(0, "BeforeLaser", loop: false);
				SAnimaMiddle.AnimationState.SetAnimation(0, "BeforeLaser", loop: false);
				SAnimaBottom.AnimationState.SetAnimation(0, "Close_H", loop: false);
			}
			break;
		case MonsterState.AfterLaser:
			if (changedState)
			{
				SEMgr.Inst.elite11Grow.PlaySE();
				base.Anima.Play("AfterLaser");
				SAnimaTop.AnimationState.SetAnimation(0, "AfterLaser", loop: false);
				SAnimaMiddle.AnimationState.SetAnimation(0, "AfterLaser", loop: false);
				SAnimaBottom.AnimationState.SetAnimation(0, "Open_H", loop: false);
			}
			break;
		case MonsterState.Laser:
		{
			ref bool reference14 = ref varMgr.RegBool(0);
			if (changedState)
			{
				laserGroup1 = true;
				laserCounter = 0;
				Vector3 targetDir = Tool2D.GetDir();
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					targetDir = ToTargetDir();
				}
				StartLaser(targetDir);
			}
			if (stateExistTime > 0.24f && !reference14)
			{
				reference14 = true;
				base.Anima.Play("Laser", 0, 0f);
				SAnimaTop.AnimationState.SetAnimation(0, "Laser", loop: false);
				SAnimaMiddle.AnimationState.SetAnimation(0, "Laser", loop: false);
			}
			break;
		}
		case MonsterState.Bullet:
		{
			ref int reference13 = ref varMgr.RegInt(0);
			if (changedState)
			{
				bulletRotateClockWise = GeneralTool.ChanceResult(0.5f);
				StartCoroutine(ShootBullet());
				reference13 = 1;
			}
			if (stateExistTime > bulletRoundsInterval)
			{
				if ((float)reference13 >= bulletShootRounds)
				{
					state = MonsterState.Idle;
					break;
				}
				reference13++;
				StartCoroutine(ShootBullet());
				stateExistTime = 0f;
			}
			break;
		}
		case MonsterState.CrossBullet:
		{
			ref float reference21 = ref varMgr.RegFloat(0);
			ref float reference22 = ref varMgr.RegFloat(1);
			ref int reference23 = ref varMgr.RegInt(0);
			if (changedState)
			{
				base.Anima.Play("ContinueAttack");
				SAnimaTop.AnimationState.SetAnimation(0, "ContinueAttack", loop: true);
				SAnimaMiddle.AnimationState.SetAnimation(0, "ContinueAttack", loop: true);
				SAnimaBottom.AnimationState.SetAnimation(0, "Attack_H", loop: true);
				reference22 = 1f;
				ShootCrossBullet(reference21);
				reference21 += reference22 * crossBulletDirInterval;
			}
			if (stateExistTime > crossBulletRoundsInterval)
			{
				if ((float)reference23 >= crossBulletShootRounds)
				{
					SetAfterAttack(crossAfterTime);
					break;
				}
				reference23++;
				ShootCrossBullet(reference21);
				reference21 += reference22 * crossBulletDirInterval;
				stateExistTime = 0f;
			}
			break;
		}
		case MonsterState.WaveBullet:
		{
			ref int reference9 = ref varMgr.RegInt(0);
			ref Vector3 reference10 = ref varMgr.RegV3(0);
			ref bool reference11 = ref varMgr.RegBool(0);
			ref float reference12 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.Anima.Play("ContinueAttack");
				SAnimaTop.AnimationState.SetAnimation(0, "ContinueAttack", loop: true);
				SAnimaMiddle.AnimationState.SetAnimation(0, "ContinueAttack", loop: true);
				SAnimaBottom.AnimationState.SetAnimation(0, "Attack_H", loop: true);
				reference10 = Tool2D.GetDir();
				bulletRotateClockWise = GeneralTool.ChanceResult(0.5f);
				reference11 = GeneralTool.ChanceResult(0.5f);
				reference12 = ((!reference11) ? 1 : (-1));
				ShootWaveBullet(reference10, reference11);
				reference10 = Tool2D.GetDir(reference10, waveBulletDirInterval * reference12);
				reference11 = !reference11;
				reference9 = 1;
			}
			if (stateExistTime > waveBulletRoundsInterval)
			{
				if ((float)reference9 >= waveBulletShootRounds)
				{
					SetAfterAttack(waveAfterTime);
					break;
				}
				reference9++;
				ShootWaveBullet(reference10, reference11);
				reference10 = Tool2D.GetDir(reference10, waveBulletDirInterval * reference12);
				reference11 = !reference11;
				stateExistTime = 0f;
			}
			break;
		}
		case MonsterState.BlockAfterAttack:
		{
			ref bool reference17 = ref varMgr.RegBool(0);
			if (changedState)
			{
				SAnimaTop.AnimationState.SetAnimation(0, "ContinueAttackAfter", loop: false);
				SAnimaMiddle.AnimationState.SetAnimation(0, "ContinueAttackAfter", loop: false);
				base.Anima.Play("ContinueAttackAfter");
			}
			if (stateExistTime > 1f && !reference17)
			{
				reference17 = true;
				SAnimaBottom.AnimationState.SetAnimation(0, "Open_H", loop: false);
			}
			if (stateExistTime > afterAttackTime)
			{
				state = MonsterState.Idle;
			}
			break;
		}
		case MonsterState.BlockBullet:
		{
			ref float reference2 = ref varMgr.RegFloat(0);
			ref float reference3 = ref varMgr.RegFloat(1);
			ref float reference4 = ref varMgr.RegFloat(2);
			ref float reference5 = ref varMgr.RegFloat(3);
			ref bool reference6 = ref varMgr.RegBool(0);
			ref bool reference7 = ref varMgr.RegBool(1);
			ref int reference8 = ref varMgr.RegInt(0);
			if (changedState)
			{
				base.Anima.Play("ContinueAttack");
				SAnimaTop.AnimationState.SetAnimation(0, "ContinueAttack", loop: true);
				SAnimaMiddle.AnimationState.SetAnimation(0, "ContinueAttack", loop: true);
				SAnimaBottom.AnimationState.SetAnimation(0, "Close_H", loop: false);
				reference6 = GeneralTool.ChanceResult(0.5f);
				reference4 = UnityEngine.Random.Range(-180, 180);
			}
			if (stateExistTime > 0.5f && !reference7)
			{
				reference7 = true;
				SEMgr.Inst.monster41Stretch.PlaySE();
				as_Tentacle.Play();
				as_TentacleGround.Play();
				for (int i = 0; i < blockBulletDirations; i++)
				{
					tentacles[i].LaunchWave(reference6, reference4 + (float)(360 / blockBulletDirations * i));
				}
			}
			if (stateExistTime > 0.5f)
			{
				reference5 += Time.deltaTime;
				if (reference5 > blockBulletRotateFrequency)
				{
					reference5 -= blockBulletRotateFrequency;
					reference6 = !reference6;
					reference8++;
				}
			}
			reference3 += Time.deltaTime;
			if (reference3 > blockRandomBulletInterval)
			{
				reference3 -= blockRandomBulletInterval;
				ShootSingleBullet(Vector3.up, blockBulletDirations);
			}
			reference2 += Time.deltaTime;
			if (!(reference2 > blockBulletInterval))
			{
				break;
			}
			if ((float)reference8 >= blockBulletWaves)
			{
				for (int j = 0; j < blockBulletDirations; j++)
				{
					tentacles[j].StopWave();
				}
				SetBlockAfterAttack(blockAfterTime);
			}
			else
			{
				reference2 -= blockBulletInterval;
			}
			break;
		}
		case MonsterState.RotateBullet:
		{
			ref float reference18 = ref varMgr.RegFloat(0);
			ref int reference19 = ref varMgr.RegInt(0);
			ref bool reference20 = ref varMgr.RegBool(0);
			if (changedState)
			{
				base.Anima.Play("ContinueAttack");
				SAnimaTop.AnimationState.SetAnimation(0, "ContinueAttack", loop: true);
				SAnimaMiddle.AnimationState.SetAnimation(0, "ContinueAttack", loop: true);
				SAnimaBottom.AnimationState.SetAnimation(0, "Attack_H", loop: true);
				reference20 = GeneralTool.ChanceResult(0.5f);
				StartCoroutine(ShootRotateGroup(Tool2D.GetDir(), reference20));
				reference19++;
				reference20 = !reference20;
			}
			reference18 += Time.deltaTime;
			if (reference18 > rotateBulletInterval)
			{
				if ((float)reference19 < rotateBulletRounds)
				{
					StartCoroutine(ShootRotateGroup(Tool2D.GetDir(), reference20));
					reference20 = !reference20;
					reference18 -= rotateBulletInterval;
					reference19++;
				}
				else
				{
					SetAfterAttack(rotateAfterTime);
				}
			}
			break;
		}
		case MonsterState.BeforeSummon:
			if (changedState)
			{
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanBeTarget = false;
				componentData2.CanTouch = false;
				SetComponentData(componentData2);
				base.Anima.Play("BeforeSummon");
				SAnimaTop.AnimationState.SetAnimation(0, "BeforeSummon", loop: false);
				SAnimaMiddle.AnimationState.SetAnimation(0, "BeforeSummon", loop: false);
				SAnimaBottom.AnimationState.SetAnimation(0, "Attack_H", loop: true);
			}
			break;
		case MonsterState.Summon:
			if (changedState)
			{
				SEMgr.Inst.elite11Grow.PlaySE();
				base.Anima.Play("Summon");
				SAnimaTop.AnimationState.SetAnimation(0, "Summon", loop: true);
				SAnimaMiddle.AnimationState.SetAnimation(0, "Summon", loop: true);
			}
			if (spawner.children.Count <= 0)
			{
				state = MonsterState.AfterSummon;
			}
			break;
		case MonsterState.AfterSummon:
			if (changedState)
			{
				SEMgr.Inst.elite11Grow.PlaySE();
				base.Anima.Play("AfterSummon");
				SAnimaTop.AnimationState.SetAnimation(0, "AfterSummon", loop: false);
				SAnimaMiddle.AnimationState.SetAnimation(0, "AfterSummon", loop: false);
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanBeTarget = true;
				componentData.CanTouch = true;
				SetComponentData(componentData);
				inSecondStage = true;
			}
			break;
		case MonsterState.Cannon:
		{
			_ = ref varMgr.RegV3(0);
			ref float reference = ref varMgr.RegFloat(1);
			if (changedState)
			{
				base.Anima.Play("Cannon", 0, 0f);
				SAnimaTop.AnimationState.SetAnimation(0, "Cannon", loop: false);
				SAnimaMiddle.AnimationState.SetAnimation(0, "Cannon", loop: false);
				SAnimaBottom.AnimationState.SetAnimation(0, "Attack_H", loop: true);
				cannonCounter = 0;
				cannonFinished = false;
				extraCannonCount = 0f;
			}
			if (cannonFinished)
			{
				reference += Time.deltaTime;
				if (reference > cannonAfterTime)
				{
					state = MonsterState.Idle;
				}
			}
			break;
		}
		case MonsterState.CrossSpike:
			break;
		}
	}

	private void ChooseSkill()
	{
		if (base.CurrentHPRatio < secondStageHealthRatio && !inSecondStage)
		{
			state = MonsterState.BeforeSummon;
			return;
		}
		if (isQuick)
		{
			int weightRandom = GeneralTool.GetWeightRandom(0f, cannonChance, 0f);
			if (inSecondStage)
			{
				weightRandom = GeneralTool.GetWeightRandom(chaseLaserChance, cannonChance);
			}
			if (weightRandom == 0 && lastSkillQuick != MonsterState.ChaseLaserBefore)
			{
				state = MonsterState.ChaseLaserBefore;
				lastSkillQuick = state;
			}
			else
			{
				if (weightRandom != 1 || (inSecondStage && lastSkillQuick == MonsterState.Cannon))
				{
					return;
				}
				state = MonsterState.Cannon;
				lastSkillQuick = state;
			}
			SEMgr.Inst.elite11BeforeAttack.PlaySE();
			isQuick = !isQuick;
			return;
		}
		int weightRandom2 = GeneralTool.GetWeightRandom(waveBulletChance, 0f, 0f, rotateBulletChance, crossBulletChance);
		if (inSecondStage)
		{
			weightRandom2 = GeneralTool.GetWeightRandom(0f, blockBulletChance, laserChance, rotateBulletChance, 0f);
		}
		if (lastSkill != skills[weightRandom2])
		{
			state = skills[weightRandom2];
			lastSkill = state;
			SEMgr.Inst.elite11BeforeAttack.PlaySE();
			isQuick = !isQuick;
		}
	}

	private void SetAfterAttack(float AfterAttackTime = 1f)
	{
		state = MonsterState.AfterAttack;
		afterAttackTime = AfterAttackTime;
	}

	private void SetBlockAfterAttack(float AfterAttackTime = 1f)
	{
		state = MonsterState.BlockAfterAttack;
		afterAttackTime = AfterAttackTime;
	}

	private void StartLaser(Vector3 targetDir)
	{
		laserCounter++;
		float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, targetDir);
		bool flag = false;
		float num2 = 0f;
		GetNearestTargetPlayerFirst();
		if (base.HaveTarget)
		{
			flag = true;
			num2 = Tool2D.IgnoreZAngleWithSign(Vector3.up, ToTargetDir());
		}
		laserGroup1 = !laserGroup1;
		for (int i = 0; i < lasers.Count; i++)
		{
			float num3 = num + laserRangeOffset.RandomResult() + (float)i * (laserRange / (float)lasers.Count - 1f) - laserRange / 2f;
			if (flag && Mathf.Abs(num3 - num2) < laserRangeOffset.value2)
			{
				num3 = num2;
				flag = false;
			}
			if (laserGroup1)
			{
				lasers1[i].Initialize(Tool2D.GetDir(Vector3.up, num3), startLaserDistance);
			}
			else
			{
				lasers[i].Initialize(Tool2D.GetDir(Vector3.up, num3), startLaserDistance);
			}
		}
	}

	private void ShootCannon(Vector3 cannonDir)
	{
		SEMgr.Inst.elite11Split.PlaySE();
		if (UnityEngine.Random.value < cannonRandomChance && extraCannonCount < (float)cannonShootRounds * cannonRandomChance)
		{
			Vector3 dir = Tool2D.GetDir();
			MiniPool.GetGO("Prefabs/EF/EF_Elite11_Cannon", base.transform.position + new Vector3(0f, 0f, 0f - cannonInitialHeight)).GetComponent<Elite11_Cannon>().SetTarget(base.transform.position + dir * cannonDistanceRange.RandomResult(), cannonInitialHeight);
			extraCannonCount += 1f;
		}
		for (int i = 0; i < cannonCount; i++)
		{
			Vector3 dir2 = Tool2D.GetDir(cannonDir, cannonAngleRange.RandomResult());
			MiniPool.GetGO("Prefabs/EF/EF_Elite11_Cannon", base.transform.position + new Vector3(0f, 0f, 0f - cannonInitialHeight)).GetComponent<Elite11_Cannon>().SetTarget(base.transform.position + dir2 * cannonDistanceRange.RandomResult(), cannonInitialHeight);
		}
	}

	private IEnumerator ShootRotateGroup(Vector3 randomDir, bool rotateRight)
	{
		for (int i = 0; (float)i < rotateBulletCount; i++)
		{
			ShootRotateBullet(randomDir, rotateRight);
			yield return new WaitForSeconds(0.15f);
		}
	}

	public void ShootLerpBullet()
	{
		SEMgr.Inst.elite11Shoot.PlaySE();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp1);
		for (int i = 0; (float)i < laserBulletCount; i++)
		{
			Vector3 vector = (sSPModifier.Direction = Tool2D.GetDir());
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - laserSpellHeight) + vector * startAttackDistance;
			sSPModifier.Damage = laserBulletDamage.RandomResult();
			sSPModifier.Duration = laserBulletLifeTime.RandomResult();
			sSPModifier.Float1 = laserBulletSlowDownTime.RandomResult();
			sSPModifier.Speed = laserBulletDistance.RandomResult() / sSPModifier.Float1 * 2f;
			sSPModifier.Float2 = sSPModifier.Speed;
			sSPModifier.ApplyToSSP(ref ssp1);
			ShootSpell(ssp1);
		}
	}

	public void SummonSingleChild()
	{
		Elite11_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/301121", base.transform.position + Tool2D.GetDir() * cannonDistanceRange.RandomResult()).GetComponent<Elite11_Child>();
		children.Add(component);
	}

	public void ShootRotateBullet(Vector3 randomDir, bool rotateRight)
	{
		SEMgr.Inst.elite11Shoot.PlaySE();
		for (int i = 0; (float)i < rotateBulletDirations; i++)
		{
			Vector3 dir = Tool2D.GetDir(randomDir, (float)(i * 360) / rotateBulletDirations);
			MiniPool.GetGO("Prefabs/EF/EF_Elite11_BigBullet", base.transform.position + dir * startAttackDistance).GetComponent<Elite11_Bullet>().InitializeCenter(Elite11_Bullet.BulletMode.RotateCenter, base.transform.position + dir * 6f, rotateBulletSpeed, 5f, rotateRight);
		}
	}

	public void ShootCrossBullet(float angle)
	{
		SEMgr.Inst.elite11Shoot.PlaySE();
		for (int i = 0; (float)i < crossBulletShootDirations; i++)
		{
			Vector3 dir = Tool2D.GetDir(Vector3.up, (float)(i * 360) / crossBulletShootDirations + angle);
			MiniPool.GetGO("Prefabs/EF/EF_Elite11_BigBullet", base.transform.position + dir * startAttackDistance).GetComponent<Elite11_Bullet>().Initialize(Elite11_Bullet.BulletMode.Rotate, dir, crossBulletSpeed, crossBulletLifeTime, crossBulletRotateSpeed);
			dir.x = 0f - dir.x;
			MiniPool.GetGO("Prefabs/EF/EF_Elite11_BigBullet", base.transform.position + dir * startAttackDistance).GetComponent<Elite11_Bullet>().Initialize(Elite11_Bullet.BulletMode.Rotate, dir, crossBulletSpeed, crossBulletLifeTime, crossBulletRotateSpeed, rotateRight: true);
		}
	}

	public void ShootSingleBullet(Vector3 randomDir, int dirations)
	{
		SEMgr.Inst.elite11ShootLow.PlaySE();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp2);
		for (int i = 0; i < dirations; i++)
		{
			Vector3 dir = Tool2D.GetDir(randomDir, i * 360 / dirations + UnityEngine.Random.Range(0, 360 / dirations));
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - laserSpellHeight) + dir * startAttackDistance;
			sSPModifier.Direction = dir;
			sSPModifier.Damage = laserBulletDamage.RandomResult();
			sSPModifier.Speed = blockRandomBulletSpeed.RandomResult();
			sSPModifier.ApplyToSSP(ref ssp2);
			ShootSpell(ssp2);
		}
	}

	public void ShootWaveBullet(Vector3 randomDir, bool rotateRight)
	{
		SEMgr.Inst.elite11Shoot.PlaySE();
		for (int i = 0; (float)i < waveBulletShootDirations; i++)
		{
			MiniPool.GetGO("Prefabs/EF/EF_Elite11_BigBullet", base.transform.position + Tool2D.GetDir(randomDir, (float)(i * 360) / waveBulletShootDirations) * startAttackDistance).GetComponent<Elite11_Bullet>().Initialize(Elite11_Bullet.BulletMode.Rotate, Tool2D.GetDir(randomDir, (float)(i * 360) / waveBulletShootDirations), waveBulletSpeed, waveBulletLifeTime, waveBulletRotateSpeed, rotateRight);
		}
	}

	public void ShootDirationBullet(float angle, float dirations)
	{
		Vector3 dir = Tool2D.GetDir(Vector3.up, angle);
		SEMgr.Inst.elite11Shoot.PlaySE();
		for (int i = 0; (float)i < dirations; i++)
		{
			MiniPool.GetGO("Prefabs/EF/EF_Elite11_BigBullet", base.transform.position + Tool2D.GetDir(dir, (float)(i * 360) / dirations) * startAttackDistance).GetComponent<Elite11_Bullet>().Initialize(Elite11_Bullet.BulletMode.Straight, Tool2D.GetDir(dir, (float)(i * 360) / dirations), blockBulletSpeed, bulletLifeTime);
		}
	}

	private void ShootTrackBullet()
	{
		trackSpellCount.RandomResult();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp2);
		for (int i = 0; i < trackSpellCount.result; i++)
		{
			Vector3 dir = Tool2D.GetDir();
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - laserSpellHeight) + dir * startAttackDistance;
			sSPModifier.Direction = dir;
			sSPModifier.Damage = laserBulletDamage.RandomResult();
			sSPModifier.Speed = blockRandomBulletSpeed.RandomResult();
			sSPModifier.ApplyToSSP(ref ssp2);
			ShootSpell(ssp2);
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (state == MonsterState.BeforeSummon || state == MonsterState.Summon || state == MonsterState.AfterSummon)
		{
			info.immuneDamage = true;
		}
	}

	protected override void BossDeadStay()
	{
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		myPpt.ChangeColor(componentData.baseColor);
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		myPpt.MR_Models[0].GetPropertyBlock(materialPropertyBlock);
		if (materialPropertyBlock.GetColor("_Color") != myPpt.BaseColor)
		{
			materialPropertyBlock.SetColor("_Color", myPpt.BaseColor);
			for (int i = 0; i < myPpt.MR_Models.Length; i++)
			{
				myPpt.MR_Models[i].SetPropertyBlock(materialPropertyBlock);
			}
		}
		bigLaserChargeParticle.Stop();
		bigLaserShootParticle.Stop();
		laserShootParticle.Stop();
		base.Anima.Play("Die");
		SetSpineStop(SAnimaBottom);
		SetSpineStop(SAnimaMiddle);
		SetSpineStop(SAnimaTop);
		SEMgr.Inst.elite11Dead.PlaySE();
		for (int num = lasers.Count - 1; num >= 0; num--)
		{
			MiniPool.RecycleGO(lasers[num].gameObject);
			MiniPool.RecycleGO(lasers1[num].gameObject);
		}
		MiniPool.RecycleGO(largeLaser.gameObject);
		for (int num2 = tentacles.Count - 1; num2 >= 0; num2--)
		{
			tentacles[num2].enabled = false;
		}
	}

	private void SetSpineStop(SkeletonAnimation sAnima)
	{
		TrackEntry current = sAnima.AnimationState.GetCurrent(0);
		float num2 = (current.AnimationEnd = (current.AnimationStart = current.AnimationTime));
		current.Loop = false;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		for (int num = tentacles.Count - 1; num >= 0; num--)
		{
			tentacles[num].DieExplode();
			MiniPool.RecycleGO(tentacles[num].gameObject);
		}
		base.AfterDead(ref info);
	}

	private IEnumerator ShootBullet()
	{
		Vector3 originDir = Tool2D.GetDir();
		float angleOffset = 0f;
		for (int i = 0; i < bulletCount; i++)
		{
			SEMgr.Inst.elite11Shoot.PlaySE();
			for (int j = 0; (float)j < bulletShootDirations; j++)
			{
				MiniPool.GetGO("Prefabs/EF/EF_Elite11_BigBullet", base.transform.position + Tool2D.GetDir(originDir, angleOffset + (float)(j * 360) / bulletShootDirations + angleOffset) * 3f).GetComponent<Elite11_Bullet>().Initialize(Elite11_Bullet.BulletMode.Straight, Tool2D.GetDir(originDir, angleOffset + (float)(j * 360) / bulletShootDirations + angleOffset), bulletSpeed, bulletLifeTime);
			}
			angleOffset += bulletAngleInterval * (float)(bulletRotateClockWise ? 1 : (-1));
			yield return new WaitForSeconds(bulletShootInterval);
		}
		yield return null;
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (animaName)
		{
		case "SpineBottomClose":
			SAnimaBottom.AnimationState.SetAnimation(0, "Close_H", loop: false);
			break;
		case "SpineBottomOpen":
			SAnimaBottom.AnimationState.SetAnimation(0, "Open_H", loop: false);
			break;
		case "BeforeSummonFinish":
			state = MonsterState.Summon;
			break;
		case "AfterSummonFinish":
			state = MonsterState.Idle;
			break;
		case "ShootTrackBullet":
			ShootTrackBullet();
			break;
		case "BeforeLaserFinish":
			state = MonsterState.Laser;
			break;
		case "BigLaserChargeFinish":
			base.Anima.Play("BigLaserAttack");
			SAnimaTop.AnimationState.SetAnimation(0, "BigLaserAfter", loop: true);
			SAnimaMiddle.AnimationState.SetAnimation(0, "BigLaserAfter", loop: true);
			bigLaserChargeParticle.Stop();
			bigLaserShootParticle.Play();
			break;
		case "LaserStartAim":
		{
			if (laserCounter >= laserRounds)
			{
				laserCounter++;
				break;
			}
			Vector3 targetDir = Tool2D.GetDir();
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				targetDir = ToTargetDir();
			}
			StartLaser(targetDir);
			break;
		}
		case "LaserShoot":
			laserShootParticle.Play();
			CamController.Inst.SetShock(laserShock);
			break;
		case "AfterLaser":
			state = MonsterState.Idle;
			break;
		case "LaserFinish":
			if (laserCounter >= laserRounds + 1)
			{
				state = MonsterState.AfterLaser;
				break;
			}
			base.Anima.Play("Laser", 0, 0f);
			SAnimaTop.AnimationState.SetAnimation(0, "Laser", loop: true);
			SAnimaMiddle.AnimationState.SetAnimation(0, "Laser", loop: true);
			break;
		case "AttackAfterFinish":
			base.Anima.Play("Idle");
			SAnimaTop.AnimationState.SetAnimation(0, "Idle", loop: true);
			SAnimaMiddle.AnimationState.SetAnimation(0, "Idle", loop: true);
			break;
		case "CannonShoot":
		{
			cannonCounter++;
			GetNearestTargetPlayerFirst();
			Vector3 cannonDir = Tool2D.GetDir();
			if (base.HaveTarget)
			{
				cannonDir = ToTargetDir();
			}
			ShootCannon(cannonDir);
			break;
		}
		case "CannonFinish":
			if (cannonCounter >= cannonShootRounds)
			{
				base.Anima.Play("Idle");
				SAnimaTop.AnimationState.SetAnimation(0, "Idle", loop: true);
				SAnimaMiddle.AnimationState.SetAnimation(0, "Idle", loop: true);
				SAnimaBottom.AnimationState.SetAnimation(0, "Idle_H", loop: true);
				cannonFinished = true;
			}
			else
			{
				base.Anima.Play("Cannon", 0, 0f);
				SAnimaTop.AnimationState.SetAnimation(0, "Cannon", loop: false);
				SAnimaMiddle.AnimationState.SetAnimation(0, "Cannon", loop: false);
			}
			break;
		case "ShootLerpBullet":
			ShootLerpBullet();
			break;
		case "LerpBulletFinish":
			state = MonsterState.ChaseLaser;
			break;
		case "SummonShout":
			SEMgr.Inst.elite11Summon.PlaySE();
			spawner.SummonChild();
			summonParticle.Play();
			CamController.Inst.SetShock(summonShock);
			break;
		}
	}
}
