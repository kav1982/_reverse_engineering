using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Elite8 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		FlyRandom,
		FlyAroundTarget,
		Jail,
		Cross,
		Triangle,
		Straight,
		Fury,
		Furying,
		FuryDone
	}

	public VariableFloat keepDistanceWithPlayer;

	public VariableFloat actionInterval;

	[Header("红色锁链阵")]
	public int childId;

	public GameObject chainChildPrefab;

	public GameObject coreChainPrefab;

	public GameObject chainChildPrefabStable;

	public float stableChance;

	public List<Elite8_ChildChain> childChains = new List<Elite8_ChildChain>();

	public List<Elite8_ChildChain> stableCoreChains = new List<Elite8_ChildChain>();

	public List<Elite8_CoreChain> coreChains = new List<Elite8_CoreChain>();

	public List<Elite8_CoreChain> borderChains = new List<Elite8_CoreChain>();

	public ParticleSystem attackParticle_H;

	public ParticleSystem attackParticle;

	public ParticleSystem attackParticle2;

	private bool triangleEnsure = true;

	[Header("锁链阵同时使用小子弹")]
	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public float shootInterval;

	[Header("紫色锁链阵")]
	public GameObject chainsManagerPrefab;

	private Elite8_Chains chainsManager;

	public Elite8_SingleChain straightChainPrafab;

	public List<Elite8_SingleChain> straightChains = new List<Elite8_SingleChain>();

	public float chainModeInterval;

	[Header("愤怒模式有关特效设置")]
	public GameObject headRoot;

	public SpriteRenderer Head1;

	public SpriteRenderer Head2;

	public ParticleSystem HeadSmoke;

	public Light2D HeadLight;

	public SpriteRenderer Back;

	public Elite8_BackShine backEffectManager;

	public List<SpriteRenderer> backStars = new List<SpriteRenderer>();

	private bool starFlipped;

	private float backRotateSpeed;

	public GameObject cloth;

	public List<ParticleSystem> ShoutParticles = new List<ParticleSystem>();

	private float originKnockback;

	private Vector3 idleMoveDir;

	private Vector3 originHeadLocalPosition;

	private Vector3 originClothLocalPositon;

	private Vector3 originBackLocalPosition;

	private Vector2 berlinSeed;

	private float furyingTimer;

	private float furyAverage;

	[Header("愤怒模式数值")]
	public float furyChance;

	public float forceFuryInterval;

	private float forceFuryTimer;

	[Header("Idle")]
	public float idleChangeDirTime;

	private float idleChangeDirTimer;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private bool secondStage;

	[Header("状态")]
	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private Vector3 keepOffset;

	private float actionIntervalTimer;

	[Header("和谐模式")]
	public List<AnimationClip> harmonyAnimations = new List<AnimationClip>();

	public SpriteRenderer sr_Head;

	public Sprite sprite_HeadH;

	public static Elite8 Inst;

	public static MiniObjPool MiniPool;

	private SpellSpawnParams ssp;

	private float FuryShockTimer;

	public MonsterState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateQuit = true;
			_state = value;
		}
	}

	public override void EveryInitialCallback()
	{
		Inst = this;
		originKnockback = myPpt.unitCfg.knockbackRatio;
		state = MonsterState.BornIdle;
		furyAverage = Random.Range(0, 3);
		childChains.Clear();
		stableCoreChains.Clear();
		coreChains.Clear();
		borderChains.Clear();
		straightChains.Clear();
		secondStage = false;
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		actionInterval.RandomResult();
		chainsManager = Object.Instantiate(chainsManagerPrefab, roomCenterPoint, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_Chains>();
		berlinSeed = new Vector2(0.832f, 0.443f);
		originHeadLocalPosition = headRoot.transform.localPosition;
		originClothLocalPositon = cloth.transform.localPosition;
		originBackLocalPosition = Back.transform.localPosition;
		MiniPool = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool"), LevelMgr.Inst.CurrentRoomT).GetComponent<MiniObjPool>();
		if (GameMgr.IsHarmony_Static)
		{
			AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(base.Anima.runtimeAnimatorController);
			base.Anima.runtimeAnimatorController = animatorOverrideController;
			for (int i = 0; i < harmonyAnimations.Count; i++)
			{
				string text = harmonyAnimations[i].name.Substring(0, harmonyAnimations[i].name.Length - 2);
				if (animatorOverrideController[text] != null)
				{
					animatorOverrideController[text] = harmonyAnimations[i];
				}
			}
			sr_Head.sprite = sprite_HeadH;
			ParticleSystem.MainModule main = HeadSmoke.main;
			main.startColor = new Color(0f, 0.2f, 0.2f, 1f);
		}
		base.Anima.Play("Elite8_Idle");
	}

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90171);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.ApplyToSSP(ref ssp);
		if (GameMgr.IsMobile_Static)
		{
			shootInterval *= 2f;
		}
	}

	private IEnumerator ShootBullet()
	{
		int _shootTime = 300;
		for (int i = 0; i < _shootTime; i++)
		{
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
			sSPModifier.Direction = Tool2D.GetDir();
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
			yield return new WaitForSeconds(shootInterval);
		}
	}

	public override void Update()
	{
		for (int num = childChains.Count - 1; num >= 0; num--)
		{
			if (childChains[num] == null)
			{
				childChains.Remove(childChains[num]);
			}
		}
		for (int num2 = coreChains.Count - 1; num2 >= 0; num2--)
		{
			if (coreChains[num2] == null)
			{
				coreChains.Remove(coreChains[num2]);
			}
		}
		for (int num3 = borderChains.Count - 1; num3 >= 0; num3--)
		{
			if (borderChains[num3] == null)
			{
				borderChains.Remove(borderChains[num3]);
			}
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (!secondStage && base.CurrentHPRatio < 0.5f)
		{
			secondStage = true;
		}
		bool flag;
		if (stateQuit)
		{
			stateQuit = false;
			flag = true;
		}
		else
		{
			flag = false;
		}
		Back.transform.localEulerAngles += new Vector3(0f, 0f, backRotateSpeed) * Time.deltaTime;
		for (int i = 0; i < backStars.Count; i++)
		{
			if (starFlipped != Back.flipX)
			{
				backStars[i].transform.localPosition = new Vector3(0f - backStars[i].transform.localPosition.x, backStars[i].transform.localPosition.y, backStars[i].transform.localPosition.z);
			}
			backStars[i].transform.eulerAngles = Vector3.zero;
		}
		if (starFlipped != Back.flipX)
		{
			starFlipped = Back.flipX;
		}
		if (chainsManager.state == Elite8_Chains.ChainMode.Rest)
		{
			forceFuryTimer += Time.deltaTime;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			if (flag)
			{
				Head1.enabled = true;
				Head2.enabled = false;
				HeadLight.intensity = 1f;
				HeadLight.pointLightOuterRadius = 3f;
				ParticleSystem.MainModule main3 = HeadSmoke.main;
				if (GameMgr.IsHarmony_Static)
				{
					main3.startColor = new Color(0f, 0.2f, 0.2f, 1f);
					HeadLight.color = Color.cyan;
				}
				else
				{
					main3.startColor = new Color(0.4f, 0f, 0f, 1f);
					HeadLight.color = Color.red;
				}
				backEffectManager.isFury = false;
				headRoot.transform.localPosition = originHeadLocalPosition;
				Back.transform.localPosition = originBackLocalPosition;
				for (int k = 0; k < ShoutParticles.Count; k++)
				{
					ShoutParticles[k].Stop();
					ShoutParticles[k].Clear();
				}
				myPpt.unitCfg.knockbackRatio = originKnockback;
				myPpt.unitCfg.beHitRatio = 1f;
				myPpt.IsVelocityDeclice = true;
				base.Anima.Play("Elite8_Idle");
				base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
				GetNavInfo(Tool2D.IgnoreZPoint(base.transform.position));
			}
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					state = MonsterState.FlyAroundTarget;
				}
				else
				{
					state = MonsterState.FlyRandom;
				}
			}
			break;
		case MonsterState.FlyRandom:
			if (flag)
			{
				base.Anima.Play("Elite8_Idle");
				idleMoveDir = Tool2D.GetDir();
				idleChangeDirTimer = 0f;
			}
			idleChangeDirTimer += Time.deltaTime;
			if (idleChangeDirTimer > idleChangeDirTime)
			{
				idleChangeDirTimer = 0f;
				idleMoveDir = Tool2D.GetDir();
			}
			SetMove(idleMoveDir * base.MoveSpeed);
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					state = MonsterState.FlyAroundTarget;
				}
			}
			RandomAct();
			break;
		case MonsterState.FlyAroundTarget:
			if (flag)
			{
				Head1.enabled = true;
				Head2.enabled = false;
				HeadLight.intensity = 1f;
				HeadLight.pointLightOuterRadius = 3f;
				ParticleSystem.MainModule main4 = HeadSmoke.main;
				if (GameMgr.IsHarmony_Static)
				{
					main4.startColor = new Color(0f, 0.2f, 0.2f, 1f);
					HeadLight.color = Color.cyan;
				}
				else
				{
					main4.startColor = new Color(0.4f, 0f, 0f, 1f);
					HeadLight.color = Color.red;
				}
				backEffectManager.isFury = false;
				headRoot.transform.localPosition = originHeadLocalPosition;
				Back.transform.localPosition = originBackLocalPosition;
				for (int l = 0; l < ShoutParticles.Count; l++)
				{
					ShoutParticles[l].Stop();
				}
				myPpt.unitCfg.knockbackRatio = originKnockback;
				myPpt.unitCfg.beHitRatio = 1f;
				myPpt.IsVelocityDeclice = true;
				base.Anima.Play("Elite8_Idle");
				keepOffset = Tool2D.GetDir() * keepDistanceWithPlayer.RandomResult();
				if (base.HaveTarget)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint + keepOffset));
				}
			}
			if (Mathf.Abs(Back.transform.localEulerAngles.z) < 5f)
			{
				backRotateSpeed = 0f;
				Back.transform.localEulerAngles = Vector3.zero;
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.FlyRandom;
				break;
			}
			if (navInfo.allCornerArrived)
			{
				keepOffset = Tool2D.GetDir() * keepDistanceWithPlayer.RandomResult();
				GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint + keepOffset));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			SetFlip(ToTargetDir().x);
			RandomAct();
			break;
		case MonsterState.Cross:
			if (flag)
			{
				base.Anima.Play("Elite8_Cross");
				SetMove(Vector3.zero, isFlip: false);
				if (base.HaveTarget)
				{
					SetFlip(ToTargetDir().x);
				}
			}
			break;
		case MonsterState.Triangle:
			if (flag)
			{
				base.Anima.Play("Elite8_Triangle");
				SetMove(Vector3.zero, isFlip: false);
				if (base.HaveTarget)
				{
					SetFlip(ToTargetDir().x);
				}
				triangleEnsure = Random.Range(0, 2) < 1;
			}
			break;
		case MonsterState.Straight:
			if (flag)
			{
				if (secondStage)
				{
					Head1.enabled = false;
					Head2.enabled = true;
					HeadLight.color = new Color(1f, 0f, 1f, 1f);
					backEffectManager.isFury = true;
					ParticleSystem.MainModule main5 = HeadSmoke.main;
					main5.startColor = new Color(0.3f, 0f, 0.5f, 1f);
					base.Anima.Play("Elite8_Straight2");
				}
				else
				{
					base.Anima.Play("Elite8_Straight");
				}
				SetMove(Vector3.zero, isFlip: false);
				if (base.HaveTarget)
				{
					SetFlip(ToTargetDir().x);
				}
			}
			break;
		case MonsterState.Jail:
			if (flag)
			{
				base.Anima.Play("Elite8_Jail");
				SetMove(Vector3.zero, isFlip: false);
				if (base.HaveTarget)
				{
					SetFlip(ToTargetDir().x);
				}
			}
			break;
		case MonsterState.Fury:
			if (flag)
			{
				base.Anima.Play("Elite8_Fury");
				MuteAllChild();
				myPpt.unitCfg.knockbackRatio = 0f;
				Head1.enabled = false;
				Head2.enabled = true;
				HeadLight.color = new Color(1f, 0f, 1f, 1f);
				HeadLight.intensity = 5f;
				HeadLight.pointLightOuterRadius = 5f;
				backEffectManager.isFury = true;
				ParticleSystem.MainModule main2 = HeadSmoke.main;
				main2.startColor = new Color(0.3f, 0f, 0.5f, 1f);
				backRotateSpeed = 540f;
			}
			SetMove((roomCenterPoint - base.transform.position) * 2f, isFlip: false);
			break;
		case MonsterState.Furying:
		{
			if (flag)
			{
				base.SAnima.AnimationState.SetAnimation(0, "FuryOut", loop: true);
				furyingTimer = 0f;
			}
			if (chainsManager.state == Elite8_Chains.ChainMode.Rest && chainsManager.hasUsed)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
				chainsManager.hasUsed = false;
				state = MonsterState.FuryDone;
				StopAllCoroutines();
			}
			SetMove((roomCenterPoint - base.transform.position) * 3f, isFlip: false);
			furyingTimer += Time.deltaTime;
			Vector2 vector = berlinSeed * furyingTimer * 32f;
			float num4 = Mathf.PerlinNoise(vector.x, vector.y) - 0.5f;
			float num5 = Mathf.PerlinNoise(vector.y, vector.x) - 0.5f;
			cloth.transform.localPosition = originClothLocalPositon + new Vector3(num5, num4, 0f) * 0.1f;
			headRoot.transform.localPosition = originHeadLocalPosition + new Vector3(num4, num5, 0f) * 0.5f;
			Back.transform.localPosition = originBackLocalPosition + new Vector3(num5, num4, 0f) * 0.1f;
			FuryShockTimer += Time.deltaTime;
			if ((double)FuryShockTimer > 0.2)
			{
				FuryShockTimer = 0f;
				CamController.Inst.SetShock(0.02f, 2.5f, 0.2f);
			}
			break;
		}
		case MonsterState.FuryDone:
			if (flag)
			{
				base.Anima.Play("Elite8_FuryDone");
				HeadLight.intensity = 1f;
				HeadLight.pointLightOuterRadius = 3f;
				ParticleSystem.MainModule main = HeadSmoke.main;
				if (GameMgr.IsHarmony_Static)
				{
					main.startColor = new Color(0f, 0.2f, 0.2f, 1f);
					HeadLight.color = Color.cyan;
				}
				else
				{
					main.startColor = new Color(0.4f, 0f, 0f, 1f);
					HeadLight.color = Color.red;
				}
				for (int j = 0; j < ShoutParticles.Count; j++)
				{
					ShoutParticles[j].Stop();
				}
				headRoot.transform.localPosition = originHeadLocalPosition;
				Back.transform.localPosition = originBackLocalPosition;
				myPpt.unitCfg.knockbackRatio = originKnockback;
				myPpt.unitCfg.beHitRatio = 1f;
				myPpt.IsVelocityDeclice = true;
				keepOffset = Tool2D.GetDir() * keepDistanceWithPlayer.RandomResult();
			}
			if (Mathf.Abs(Back.transform.localEulerAngles.z) < 5f)
			{
				backRotateSpeed = 0f;
				Back.transform.localEulerAngles = Vector3.zero;
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint + keepOffset));
				if (navInfo.allCornerArrived)
				{
					keepOffset = Tool2D.GetDir() * keepDistanceWithPlayer.RandomResult();
				}
				else
				{
					SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				}
				SetFlip(ToTargetDir().x);
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private Vector3 GetRandomPointWithinBorder()
	{
		return roomCenterPoint + new Vector3(Random.Range((0f - roomWidth) / 2f, roomHeight / 2f), Random.Range((0f - roomHeight) / 2f, roomHeight / 2f), 0f);
	}

	private void CastTriangleChain()
	{
		bool flag = false;
		float num = (GameMgr.IsMobile_Static ? 0.8f : 1f);
		Vector3 vector = ((!base.HaveTarget) ? GetRandomPointWithinBorder() : base.TargetPoint);
		if (Random.Range(0f, 1f) < stableChance)
		{
			flag = true;
			Elite8_ChildChain component = Object.Instantiate(chainChildPrefabStable, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_ChildChain>();
			component.diration = Tool2D.GetDir(vector - base.transform.position, Random.Range(-45f, 45f));
			childChains.Add(component);
			component.speed = 3f * num;
			component.Elite8ppt = myPpt;
			component.stableDestory = true;
		}
		else
		{
			Elite8_ChildChain component2 = Object.Instantiate(chainChildPrefab, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_ChildChain>();
			component2.diration = Tool2D.GetDir(vector - base.transform.position, Random.Range(-45f, 45f));
			childChains.Add(component2);
			component2.workOnce = true;
			component2.speed = 3f * num;
			component2.Elite8ppt = myPpt;
		}
		if (Random.Range(0f, 1f) < stableChance && !flag)
		{
			flag = true;
			Elite8_ChildChain component3 = Object.Instantiate(chainChildPrefabStable, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_ChildChain>();
			component3.diration = Tool2D.GetDir();
			component3.wideRadius = 2.5f;
			component3.radius = 2f;
			component3.speed = 1.5f;
			component3.maxExistTime = 6f;
			component3.stableDestory = true;
			childChains.Add(component3);
			component3.Elite8ppt = myPpt;
		}
		else
		{
			Elite8_ChildChain component4 = Object.Instantiate(chainChildPrefab, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_ChildChain>();
			component4.diration = Tool2D.GetDir();
			component4.wideRadius = 2.5f;
			component4.radius = 2f;
			component4.speed = 1.5f;
			component4.maxExistTime = 6f;
			childChains.Add(component4);
			component4.workOnce = true;
			component4.Elite8ppt = myPpt;
		}
		if (!GameMgr.IsMobile_Static)
		{
			if ((Random.Range(0f, 1f) < stableChance || triangleEnsure) && !flag)
			{
				Elite8_ChildChain component5 = Object.Instantiate(chainChildPrefabStable, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_ChildChain>();
				component5.diration = Tool2D.GetDir(vector - base.transform.position, Random.Range(-30f, 30f));
				childChains.Add(component5);
				component5.speed = 4f;
				component5.Elite8ppt = myPpt;
				component5.stableDestory = true;
			}
			else
			{
				Elite8_ChildChain component6 = Object.Instantiate(chainChildPrefab, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_ChildChain>();
				component6.diration = Tool2D.GetDir(vector - base.transform.position, Random.Range(-30f, 30f));
				childChains.Add(component6);
				component6.workOnce = true;
				component6.speed = 4f;
				component6.Elite8ppt = myPpt;
			}
		}
		if (!flag)
		{
			triangleEnsure = true;
		}
		else
		{
			triangleEnsure = false;
		}
	}

	private void CastStraitChain()
	{
		Vector3 vector = ((!base.HaveTarget) ? GetRandomPointWithinBorder() : base.TargetPoint);
		if (secondStage)
		{
			bool flag = false;
			Elite8_SingleChain elite8_SingleChain = null;
			Vector3 vector2 = -(vector - myPpt.transform.position).normalized * 5f;
			for (int i = 0; i < straightChains.Count; i++)
			{
				if (!straightChains[i].gameObject.activeSelf)
				{
					flag = true;
					elite8_SingleChain = straightChains[i];
					elite8_SingleChain.transform.position = base.transform.position + vector2;
					break;
				}
			}
			if (!flag)
			{
				elite8_SingleChain = Object.Instantiate(straightChainPrafab, base.transform.position + vector2, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_SingleChain>();
				straightChains.Add(elite8_SingleChain);
			}
			if (elite8_SingleChain != null)
			{
				elite8_SingleChain.diration = (vector - elite8_SingleChain.transform.position).normalized;
				elite8_SingleChain.speed = 3f;
				elite8_SingleChain.existTime = 8f;
				elite8_SingleChain.single = true;
				elite8_SingleChain.gameObject.SetActive(value: true);
			}
		}
		else
		{
			Vector3 vector3 = -(vector - myPpt.transform.position).normalized * 5f;
			Elite8_CoreChain component = Object.Instantiate(coreChainPrefab, base.transform.position + vector3, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_CoreChain>();
			component.workOnce = true;
			component.isLine = true;
			component.transform.position = base.transform.position + vector3;
			component.diration = vector - component.transform.position;
			component.speed = 3f;
			component.maxExistTime = 8f;
			coreChains.Add(component);
		}
	}

	private void CastJail()
	{
		Elite8_ChildChain component = Object.Instantiate(chainChildPrefab, PlayerMgr.Inst.PlayerCtrller.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_ChildChain>();
		component.speed = 0f;
		component.workOnce = true;
		component.isJail = true;
		component.edges = 7;
		component.radius = (GameMgr.IsMobile_Static ? 7 : 6);
		component.radiusSpreadSpeed = 1f;
		component.useRandomRotate = false;
		component.maxExistTime = 6f;
		component.rotateSpeed = ((Random.Range(0, 2) != 0) ? 1 : (-1)) * 45;
		component.Elite8ppt = myPpt;
		childChains.Add(component);
	}

	private void CastCrossChain()
	{
		Elite8_CoreChain component = Object.Instantiate(coreChainPrefab, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_CoreChain>();
		component.workOnce = true;
		component.transform.position = roomCenterPoint + new Vector3((roomWidth - 0.5f) / 2f * (float)((Random.Range(0, 2) != 0) ? 1 : (-1)), (roomHeight - 0.5f) / 2f * (float)((Random.Range(0, 2) != 0) ? 1 : (-1)), 0f);
		component.diration = roomCenterPoint - component.transform.position;
		coreChains.Add(component);
	}

	private void CastBorderChain()
	{
		Elite8_CoreChain component = Object.Instantiate(coreChainPrefab, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_CoreChain>();
		component.workOnce = true;
		component.transform.position = roomCenterPoint + new Vector3((roomWidth - 2f) / 2f * (float)((Random.Range(0, 2) != 0) ? 1 : (-1)), (roomHeight - 2f) / 2f * (float)((Random.Range(0, 2) != 0) ? 1 : (-1)), 0f);
		component.diration = roomCenterPoint - component.transform.position;
		component.speed = 0f;
		component.maxExistTime = 5f;
		coreChains.Add(component);
		borderChains.Add(component);
		Elite8_CoreChain component2 = Object.Instantiate(coreChainPrefab, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Elite8_CoreChain>();
		component2.workOnce = true;
		component2.transform.position = roomCenterPoint - (component.transform.position - roomCenterPoint);
		component2.diration = roomCenterPoint - component2.transform.position;
		component2.speed = 0f;
		component2.maxExistTime = 5f;
		coreChains.Add(component2);
		borderChains.Add(component2);
	}

	private void MuteAllChild()
	{
		for (int num = childChains.Count - 1; num >= 0; num--)
		{
			if (childChains[num] != null)
			{
				childChains[num].Recycle();
			}
		}
		for (int num2 = coreChains.Count - 1; num2 >= 0; num2--)
		{
			if (coreChains[num2] != null)
			{
				coreChains[num2].Recycle();
			}
		}
		for (int num3 = straightChains.Count - 1; num3 >= 0; num3--)
		{
			if (straightChains[num3] != null)
			{
				straightChains[num3].StopChain();
			}
		}
	}

	private void RandomAct()
	{
		actionIntervalTimer += Time.deltaTime;
		if (!(actionIntervalTimer >= actionInterval.result))
		{
			return;
		}
		actionIntervalTimer = 0f;
		actionInterval.RandomResult();
		if ((Random.Range(0f, 1f) < furyChance || forceFuryTimer > forceFuryInterval) && chainsManager.restTimer > chainModeInterval && chainsManager.state == Elite8_Chains.ChainMode.Rest)
		{
			state = MonsterState.Fury;
			forceFuryTimer = 0f;
			return;
		}
		float num = Random.Range(0f, 1f);
		if ((double)num < 0.45)
		{
			backRotateSpeed = 360f;
			state = MonsterState.Triangle;
		}
		else if ((double)num < 0.6)
		{
			if (borderChains.Count > 0)
			{
				for (int i = 0; i < straightChains.Count; i++)
				{
					if (straightChains[i].gameObject.activeSelf)
					{
						actionIntervalTimer = actionInterval.result;
						return;
					}
				}
			}
			for (int j = 0; j < coreChains.Count; j++)
			{
				if (!coreChains[j].isLine)
				{
					actionIntervalTimer = actionInterval.result;
					return;
				}
			}
			backRotateSpeed = 360f;
			state = MonsterState.Cross;
		}
		else if ((double)num < 0.75)
		{
			if (borderChains.Count > 0 && secondStage)
			{
				actionIntervalTimer = actionInterval.result;
				return;
			}
			backRotateSpeed = 360f;
			state = MonsterState.Straight;
		}
		else if (!base.HaveTarget)
		{
			actionIntervalTimer = actionInterval.result;
		}
		else
		{
			backRotateSpeed = 360f;
			state = MonsterState.Jail;
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
		Head1.enabled = true;
		Head2.enabled = false;
		HeadLight.intensity = 1f;
		HeadLight.pointLightOuterRadius = 3f;
		ParticleSystem.MainModule main = HeadSmoke.main;
		if (GameMgr.IsHarmony_Static)
		{
			main.startColor = new Color(0f, 0.2f, 0.2f, 1f);
			HeadLight.color = Color.cyan;
		}
		else
		{
			main.startColor = new Color(0.4f, 0f, 0f, 1f);
			HeadLight.color = Color.red;
		}
		backEffectManager.isFury = false;
		headRoot.transform.localPosition = originHeadLocalPosition;
		Back.transform.localPosition = originBackLocalPosition;
		for (int i = 0; i < ShoutParticles.Count; i++)
		{
			ShoutParticles[i].Stop();
		}
		base.Anima.Play("Elite8_Die");
		base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
		base.SAnima.Update(1f);
		base.SAnima.timeScale = 0f;
		backRotateSpeed = 0f;
		Back.transform.localEulerAngles = Vector3.zero;
		for (int j = 0; j < backStars.Count; j++)
		{
			backStars[j].transform.eulerAngles = Vector3.zero;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		StopAllCoroutines();
		MuteAllChild();
		chainsManager.state = Elite8_Chains.ChainMode.Rest;
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Jail":
			if (GameMgr.IsHarmony_Static)
			{
				attackParticle_H.Play();
			}
			else
			{
				attackParticle.Play();
			}
			SEMgr.Inst.elite8Attack.PlaySE();
			CastJail();
			break;
		case "AttackFinish":
			base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
			state = MonsterState.FlyAroundTarget;
			break;
		case "Cross":
			if (GameMgr.IsHarmony_Static)
			{
				attackParticle_H.Play();
			}
			else
			{
				attackParticle.Play();
			}
			if (secondStage && borderChains.Count == 0 && Random.Range(0f, 1f) < 0.5f)
			{
				CastBorderChain();
			}
			else
			{
				CastCrossChain();
			}
			SEMgr.Inst.elite8Attack.PlaySE();
			break;
		case "Triangle":
			if (GameMgr.IsHarmony_Static)
			{
				attackParticle_H.Play();
			}
			else
			{
				attackParticle.Play();
			}
			CastTriangleChain();
			SEMgr.Inst.elite8Attack.PlaySE();
			break;
		case "Straight":
			if (secondStage)
			{
				attackParticle2.Play();
			}
			else if (GameMgr.IsHarmony_Static)
			{
				attackParticle_H.Play();
			}
			else
			{
				attackParticle.Play();
			}
			CastStraitChain();
			SEMgr.Inst.elite8Attack.PlaySE();
			break;
		case "PreAttack":
			base.SAnima.AnimationState.SetAnimation(0, "Attack", loop: false);
			break;
		case "Fury":
		{
			state = MonsterState.Furying;
			for (int i = 0; i < ShoutParticles.Count; i++)
			{
				ShoutParticles[i].Play();
			}
			StartCoroutine(ShootBullet());
			chainsManager.hasUsed = true;
			furyAverage += 1f;
			if (furyAverage > 2f)
			{
				furyAverage = 0f;
			}
			if (GameMgr.IsMobile_Static && furyAverage > 1f)
			{
				furyAverage = 0f;
			}
			if (!secondStage)
			{
				if (furyAverage == 0f)
				{
					chainsManager.state = Elite8_Chains.ChainMode.Cross;
				}
				else if (furyAverage == 1f)
				{
					chainsManager.state = Elite8_Chains.ChainMode.Wave;
				}
				else
				{
					chainsManager.state = Elite8_Chains.ChainMode.Triangle;
				}
			}
			else if (furyAverage == 0f)
			{
				chainsManager.state = Elite8_Chains.ChainMode.Triangle;
			}
			else if (furyAverage == 1f)
			{
				chainsManager.state = Elite8_Chains.ChainMode.Lines;
			}
			else
			{
				chainsManager.state = Elite8_Chains.ChainMode.Maze;
			}
			break;
		}
		case "FuryDone":
			Head1.enabled = true;
			Head2.enabled = false;
			backEffectManager.isFury = false;
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
