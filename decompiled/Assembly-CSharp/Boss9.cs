using System;
using System.Collections;
using System.Linq;
using Spine.Unity;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Boss9 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		NormalAttack,
		Dead
	}

	private float downTentacleOffste = 0.1f;

	public static Boss9 Inst;

	private bool dialogueActive;

	[Header("主播包伤害调整")]
	public float waterBulletFactor;

	public float stage1Damage;

	public float stage2Damage;

	public float fishBulletFactor;

	public float fishSpellDamage;

	[Header("Spine")]
	public Animator bodySpine;

	public SkeletonAnimation upTentacleSpine;

	public SkeletonAnimation downTentacleSpine;

	[Header("通用属性")]
	public VariableFloat generalAttackCD;

	public float generalAttackCDTimer;

	public bool canAttack;

	public int attackType;

	public int debuffType;

	public bool lastShortAttack;

	private int bulletAttackCount;

	private int tentacleAttackCount;

	private int waterCount;

	private int inkCount;

	public Transform decorationPivot;

	public SpriteRenderer water2;

	public Material waterMaterial1;

	public Material waterMaterial2;

	public VariableFloat rotateEyeCDTime;

	public float rotateEyeCDTimer;

	public VariableFloat blinkEyeCDTime;

	public float blinkEyeCDTimer;

	private MaterialPropertyBlock mpb;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("触手刺击")]
	public VariableFloat tentacleAttackCD;

	public float tentacleAttackCDTimer;

	public float tentacleAttackLength;

	public float tentacleAttackLengthTimer;

	public Transform leftUpPivot;

	public Transform rightDownPivot;

	[Header("水流强制位移")]
	public float force;

	public float summonReduceFactor;

	public bool isForceActive;

	public ParticleSystem inhaleEffect;

	public ParticleSystem breathEffect;

	public float breathTime;

	private float breathTimer;

	private float roomHeight;

	private float roomWidth;

	private Vector3 roomCenter;

	[Header("触手砸出子弹")]
	public Animator tentacleUpAnimator;

	public float waterSpellHeight;

	public float waterSpellSpeed;

	public float waterSpellDuration;

	public Transform bulletPivot;

	public int bulletCount;

	[Header("鱿鱼弹幕")]
	public float fishSpellHeight;

	public float fishSpellSpeed;

	public float fishSpellDuration;

	public VariableFloat fishCD;

	public float fishCDTimer;

	public float fishLength;

	public float fishLengthTimer;

	public int fishCount;

	public float spellAmplitude;

	public float spellFrequency;

	public VariableFloat fishOffset;

	[Header("无敌")]
	public float invincibleTime;

	private float invincibleTimer;

	public float invincibleCD;

	private float invincibleCDTimer;

	private bool changeToINvincible;

	private bool canChangeToInvincible;

	private bool isInvincible;

	public bool isTentalceIdle;

	[Header("夜盲症")]
	public bool hasDarkView;

	private float originLightIntensity;

	public bool blinded;

	public float blindSpeed;

	public float currentBlindProgress;

	public float blindDuration;

	public float blindTimmer;

	[Header("音效")]
	public AudioSource waterFlow;

	private SpellSpawnParams waterSsp;

	private SpellSpawnParams fishSsp;

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
		waterFlow.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		if (ScriptableObjMgr.Inst.testCtrller.isBW)
		{
			stage1Damage *= waterBulletFactor;
			stage2Damage *= waterBulletFactor;
			fishSpellDamage *= fishBulletFactor;
		}
		if (GameMgr.IsMobile_Static)
		{
			fishSpellDuration *= 1.3f;
			fishSpellSpeed *= 0.8f;
			spellFrequency *= 0.8f;
			waterSpellSpeed *= 0.9f;
		}
		waterSsp = UnitDotsSyncSystem.GetSpellPrototype(90381);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in waterSsp);
		sSPModifier.Speed = waterSpellSpeed;
		sSPModifier.Duration = waterSpellDuration;
		sSPModifier.Damage = stage1Damage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref waterSsp);
		fishSsp = UnitDotsSyncSystem.GetSpellPrototype(90391);
		sSPModifier.Speed = fishSpellSpeed;
		sSPModifier.Duration = fishSpellDuration;
		sSPModifier.Damage = fishSpellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref fishSsp);
		myPpt.RemoveSRFromArray(water2);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
		mpb = new MaterialPropertyBlock();
		if (GameMgr.IsMobile_Static)
		{
			force *= 0.8f;
		}
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width;
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Decoration", decorationPivot.position).transform.position = decorationPivot.transform.position;
		lastShortAttack = false;
		canAttack = true;
		canChangeToInvincible = false;
		isInvincible = false;
		isTentalceIdle = true;
		bodySpine.Play("Body_Idle", 0);
		bodySpine.Play("Eye_Blink", 2);
		upTentacleSpine.AnimationState.SetAnimation(0, "DecTentacle_Idle", loop: true);
		downTentacleSpine.AnimationState.SetAnimation(0, "DecTentacle_Idle", loop: true);
		upTentacleSpine.timeScale = 1f;
		downTentacleSpine.timeScale = 1f;
		rotateEyeCDTime.RandomResult();
		waterMaterial1.SetFloat("_Strength", 0.01f);
		waterMaterial2.SetFloat("_Strength", 0.01f);
		Inst = this;
		hasDarkView = false;
		MusicMgr.Inst.UpdateThemeMusic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.InvincibleRegister();
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		SetComponentData(componentData);
		dialogueActive = false;
		blinded = false;
		currentBlindProgress = 0f;
		originLightIntensity = LevelMgr.Inst.globalLight.intensity;
	}

	public void LateUpdate()
	{
		if (!isForceActive)
		{
			return;
		}
		LocalTransform componentData = GetComponentData<LocalTransform>(PlayerMgr.Inst.PlayerEtt);
		componentData.Position += (float3)Vector3.left * force * Time.deltaTime;
		SetComponentData(componentData, PlayerMgr.Inst.PlayerEtt);
		foreach (Entity teammateEtt in LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList)
		{
			LocalTransform componentData2 = UnitDotsSyncSystem.GetComponentData<LocalTransform>(teammateEtt);
			componentData2.Position += (float3)(Vector3.left * force / summonReduceFactor * Time.deltaTime);
			SetComponentData(componentData2, teammateEtt);
		}
		breathTimer += Time.deltaTime;
	}

	public override void Update()
	{
		base.Update();
		if (blinded)
		{
			if (currentBlindProgress < 1f)
			{
				currentBlindProgress += blindSpeed * Time.deltaTime;
				LevelMgr.Inst.globalLight.intensity = Mathf.Lerp(1f, 0f, currentBlindProgress) * originLightIntensity;
			}
			blindTimmer += Time.deltaTime;
			if (blindTimmer > blindDuration)
			{
				blinded = false;
				hasDarkView = false;
			}
		}
		else if (currentBlindProgress > 0f)
		{
			currentBlindProgress -= blindSpeed * Time.deltaTime;
			LevelMgr.Inst.globalLight.intensity = Mathf.Lerp(1f, 0f, currentBlindProgress) * originLightIntensity;
		}
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
		rotateEyeCDTimer += Time.deltaTime;
		blinkEyeCDTimer += Time.deltaTime;
		if (rotateEyeCDTimer > rotateEyeCDTime.result && !base.deadStayed && !isInvincible && !changeToINvincible)
		{
			rotateEyeCDTimer = 0f;
			rotateEyeCDTime.RandomResult();
			bodySpine.Play("Eye_Roate", 1);
		}
		if (blinkEyeCDTimer > blinkEyeCDTime.result && !base.deadStayed && !isInvincible && !changeToINvincible)
		{
			blinkEyeCDTimer = 0f;
			blinkEyeCDTime.RandomResult();
			bodySpine.Play("Eye_Blink", 2);
		}
		if (canChangeToInvincible)
		{
			invincibleCDTimer += Time.deltaTime;
			if (invincibleCDTimer > invincibleCD && !changeToINvincible && isTentalceIdle)
			{
				tentacleUpAnimator.Play("CoverEye");
				bodySpine.Play("Eye_Close", 2);
				changeToINvincible = true;
				isTentalceIdle = false;
			}
		}
		if (isInvincible)
		{
			invincibleTimer += Time.deltaTime;
			if (invincibleTimer > invincibleTime && changeToINvincible)
			{
				tentacleUpAnimator.Play("ShowEye");
				bodySpine.Play("Eye_Open", 2);
				changeToINvincible = false;
			}
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			if (DataMgr.selectedWorldData.daveFirstMeetBoss9)
			{
				if (Tool2D.IgnoreZDistanceSqr(base.transform.position, PlayerMgr.Inst.PlayerPoint) < 49f && !dialogueActive)
				{
					dialogueActive = true;
					GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(304, (Action)delegate
					{
						DataMgr.selectedWorldData.daveFirstMeetBoss9 = false;
						base.CC_Self.enabled = true;
						SetDotsCCEnable(isOpen: true);
						UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
						componentData2.InvincibleUnregister();
						componentData2.CanBeTarget = true;
						componentData2.CanTouch = true;
						SetComponentData(componentData2);
						state = MonsterState.NormalAttack;
						GameUISingletonMono<UIBossHP>.ShowInit(myPpt.myEntity);
						GameUISingletonMono<UIBossShow>.ShowInit(myPpt.myEntity);
						generalAttackCDTimer = -1f;
					});
				}
			}
			else
			{
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.InvincibleUnregister();
				componentData.CanBeTarget = true;
				componentData.CanTouch = true;
				SetComponentData(componentData);
				state = MonsterState.NormalAttack;
				GameUISingletonMono<UIBossHP>.ShowInit(myPpt.myEntity);
				GameUISingletonMono<UIBossShow>.ShowInit(myPpt.myEntity);
				generalAttackCDTimer = -1f;
			}
			break;
		case MonsterState.NormalAttack:
			_ = changedState;
			if (!canAttack || !isTentalceIdle)
			{
				break;
			}
			generalAttackCDTimer += Time.deltaTime;
			if (!(generalAttackCDTimer > generalAttackCD.result))
			{
				break;
			}
			generalAttackCDTimer = 0f;
			generalAttackCD.RandomResult();
			canAttack = false;
			attackType = UnityEngine.Random.Range(0, 4);
			debuffType = UnityEngine.Random.Range(0, 2);
			if (lastShortAttack)
			{
				if (bulletAttackCount >= 2)
				{
					attackType = UnityEngine.Random.Range(1, 4);
					bulletAttackCount = 0;
					tentacleAttackCount = 0;
				}
				if (tentacleAttackCount >= 2)
				{
					attackType = 0;
					bulletAttackCount = 0;
					tentacleAttackCount = 0;
				}
				if (waterCount >= 2)
				{
					debuffType = 1;
					waterCount = 0;
					inkCount = 0;
				}
				if (inkCount >= 2)
				{
					debuffType = 0;
					waterCount = 0;
					inkCount = 0;
				}
				switch (debuffType)
				{
				case 0:
					inhaleEffect.Play();
					breathTimer = 0f;
					waterFlow.Play();
					bodySpine.Play("Eye_Charge_1", 2);
					bodySpine.Play("Body_Charge", 0);
					StartCoroutine(DelayAttack());
					waterCount++;
					break;
				case 1:
					bodySpine.Play("Eye_Charge_1", 2);
					bodySpine.Play("Body_Charge", 0);
					StartCoroutine(DelayAttack());
					inkCount++;
					break;
				}
				lastShortAttack = false;
			}
			else
			{
				int num = attackType;
				if ((uint)num <= 3u)
				{
					StartCoroutine(ShortAttack());
				}
				lastShortAttack = true;
			}
			break;
		case MonsterState.Dead:
			break;
		}
	}

	private IEnumerator TentacleAttack()
	{
		while (tentacleAttackLengthTimer < tentacleAttackLength)
		{
			tentacleAttackCDTimer += 0.01f;
			tentacleAttackLengthTimer += 0.01f;
			if (tentacleAttackCDTimer > tentacleAttackCD.result)
			{
				tentacleAttackCDTimer = 0f;
				tentacleAttackCD.RandomResult();
				float[] tentaclePos = new float[5];
				float x = PlayerMgr.Inst.PlayerPoint.x;
				int num = 1;
				for (int j = 1; j < 3; j++)
				{
					if (x + (float)j * 3.5f < rightDownPivot.position.x)
					{
						tentaclePos[j] = x + (float)j * 3.5f;
						continue;
					}
					tentaclePos[j] = x - (float)(num + 2) * 3.5f;
					num++;
				}
				tentaclePos[0] = x;
				num = 1;
				for (int k = 1; k < 3; k++)
				{
					if (x - (float)k * 3.5f > roomCenter.x - roomWidth / 2f)
					{
						tentaclePos[k + 2] = x - (float)k * 3.5f;
						continue;
					}
					tentaclePos[k + 2] = x + (float)(num + 2) * 3.5f;
					num++;
				}
				Array.Sort(tentaclePos);
				for (int i = 4; i >= 0; i--)
				{
					if (UnityEngine.Random.Range(0, 2) == 0)
					{
						Boss9_Tentacle component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Tentacle", new Vector3(tentaclePos[i], roomCenter.y + roomHeight / 2f, base.transform.position.z)).GetComponent<Boss9_Tentacle>();
						component.rotateObj.transform.eulerAngles = new Vector3(0f, 0f, 180f);
						component.holeSpriteRenderer.gameObject.SetActive(value: true);
						component.SetReadyEffectPosition(isUp: true, isTilt: false);
					}
					else
					{
						Boss9_Tentacle component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Tentacle", new Vector3(tentaclePos[i], roomCenter.y - roomHeight / 2f - downTentacleOffste, base.transform.position.z)).GetComponent<Boss9_Tentacle>();
						component2.rotateObj.transform.eulerAngles = new Vector3(0f, 0f, 0f);
						component2.holeSpriteRenderer.gameObject.SetActive(value: false);
						component2.SetReadyEffectPosition(isUp: false, isTilt: false);
					}
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield return new WaitForSeconds(0.01f);
		}
		tentacleAttackLengthTimer = 0f;
		generalAttackCDTimer = 0f;
		canAttack = true;
		yield return new WaitForSeconds(1.5f);
		isForceActive = false;
		breathEffect.Stop();
		waterFlow.Stop();
	}

	private IEnumerator TentacleAttackCross()
	{
		while (tentacleAttackLengthTimer < tentacleAttackLength)
		{
			tentacleAttackCDTimer += 0.01f;
			tentacleAttackLengthTimer += 0.02f;
			if (tentacleAttackCDTimer > tentacleAttackCD.result)
			{
				tentacleAttackCDTimer = 0f;
				tentacleAttackCD.RandomResult();
				Vector3 playerPoint = PlayerMgr.Inst.PlayerPoint;
				float y;
				bool isUp;
				if (playerPoint.y > roomCenter.y)
				{
					y = roomCenter.y + roomHeight / 2f;
					isUp = true;
				}
				else
				{
					y = roomCenter.y - roomHeight / 2f - 0.4f - downTentacleOffste;
					isUp = false;
				}
				Vector3 aimPoint = new Vector3(playerPoint.x, roomCenter.y, playerPoint.z);
				float posX3 = Mathf.Clamp(playerPoint.x - 3f, roomCenter.x - roomWidth / 2f, rightDownPivot.position.x);
				float posX5 = Mathf.Clamp(playerPoint.x + 3f, roomCenter.x - roomWidth / 2f, rightDownPivot.position.x);
				Boss9_Tentacle component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Tentacle", new Vector3(playerPoint.x, y, base.transform.position.z)).GetComponent<Boss9_Tentacle>();
				component.rotateObj.transform.eulerAngles = (isUp ? new Vector3(0f, 0f, 180f) : new Vector3(0f, 0f, 0f));
				if (isUp)
				{
					component.holeSpriteRenderer.gameObject.SetActive(value: true);
					component.SetReadyEffectPosition(isUp: true, isTilt: false);
				}
				else
				{
					component.holeSpriteRenderer.gameObject.SetActive(value: false);
					component.SetReadyEffectPosition(isUp: false, isTilt: false);
				}
				yield return new WaitForSeconds(0.15f);
				float tentacle2Y = (isUp ? (roomCenter.y - roomHeight / 2f - 0.4f - downTentacleOffste) : (roomCenter.y + roomHeight / 2f));
				Boss9_Tentacle component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Tentacle", new Vector3(posX3, tentacle2Y, base.transform.position.z)).GetComponent<Boss9_Tentacle>();
				component2.rotateObj.transform.eulerAngles = Tool2D.GetEulerAngleByDir(Tool2D.IgnoreZV2ToV1Normal(aimPoint, component2.transform.position));
				if (isUp)
				{
					component2.SetReadyEffectPosition(isUp: false, isTilt: true);
					component2.holeSpriteRenderer.gameObject.SetActive(value: false);
				}
				else
				{
					component2.holeSpriteRenderer.gameObject.SetActive(value: true);
					component2.SetReadyEffectPosition(isUp: true, isTilt: true);
				}
				yield return new WaitForSeconds(0.15f);
				Boss9_Tentacle component3 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Tentacle", new Vector3(posX5, tentacle2Y, base.transform.position.z)).GetComponent<Boss9_Tentacle>();
				component3.rotateObj.transform.eulerAngles = Tool2D.GetEulerAngleByDir(Tool2D.IgnoreZV2ToV1Normal(aimPoint, component3.transform.position));
				if (isUp)
				{
					component3.SetReadyEffectPosition(isUp: false, isTilt: true);
					component3.holeSpriteRenderer.gameObject.SetActive(value: false);
				}
				else
				{
					component3.SetReadyEffectPosition(isUp: true, isTilt: true);
					component3.readyEffect.transform.position = component3.readyEffect.transform.position;
				}
				yield return new WaitForSeconds(2.5f);
				playerPoint = PlayerMgr.Inst.PlayerPoint;
				aimPoint = new Vector3(playerPoint.x, roomCenter.y, playerPoint.z);
				posX3 = Mathf.Clamp(playerPoint.x - 3f, roomCenter.x - roomWidth / 2f, rightDownPivot.position.x);
				posX5 = Mathf.Clamp(playerPoint.x + 3f, roomCenter.x - roomWidth / 2f, rightDownPivot.position.x);
				float tentacle5Y = (isUp ? (roomCenter.y + roomHeight / 2f) : (roomCenter.y - roomHeight / 2f - 0.4f - downTentacleOffste));
				Boss9_Tentacle component4 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Tentacle", new Vector3(posX3, tentacle5Y, base.transform.position.z)).GetComponent<Boss9_Tentacle>();
				component4.rotateObj.transform.eulerAngles = Tool2D.GetEulerAngleByDir(Tool2D.IgnoreZV2ToV1Normal(aimPoint, component4.transform.position));
				if (isUp)
				{
					component4.holeSpriteRenderer.gameObject.SetActive(value: true);
					component4.SetReadyEffectPosition(isUp: true, isTilt: true);
				}
				else
				{
					component4.SetReadyEffectPosition(isUp: false, isTilt: true);
					component4.holeSpriteRenderer.gameObject.SetActive(value: false);
				}
				yield return new WaitForSeconds(0.15f);
				Boss9_Tentacle component5 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Tentacle", new Vector3(posX5, tentacle5Y, base.transform.position.z)).GetComponent<Boss9_Tentacle>();
				component5.rotateObj.transform.eulerAngles = Tool2D.GetEulerAngleByDir(Tool2D.IgnoreZV2ToV1Normal(aimPoint, component5.transform.position));
				if (isUp)
				{
					component5.SetReadyEffectPosition(isUp: true, isTilt: true);
					component5.holeSpriteRenderer.gameObject.SetActive(value: true);
				}
				else
				{
					component5.SetReadyEffectPosition(isUp: false, isTilt: true);
					component5.holeSpriteRenderer.gameObject.SetActive(value: false);
				}
				yield return new WaitForSeconds(0.15f);
				float y2 = (isUp ? (roomCenter.y - roomHeight / 2f - 0.4f - downTentacleOffste) : (roomCenter.y + roomHeight / 2f));
				Boss9_Tentacle component6 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Tentacle", new Vector3(playerPoint.x, y2, base.transform.position.z)).GetComponent<Boss9_Tentacle>();
				component6.rotateObj.transform.eulerAngles = (isUp ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 0f, 180f));
				if (isUp)
				{
					component6.SetReadyEffectPosition(isUp: false, isTilt: false);
					component6.holeSpriteRenderer.gameObject.SetActive(value: false);
				}
				else
				{
					component6.holeSpriteRenderer.gameObject.SetActive(value: true);
					component6.SetReadyEffectPosition(isUp: true, isTilt: false);
				}
			}
			yield return new WaitForSeconds(0.01f);
		}
		tentacleAttackLengthTimer = 0f;
		generalAttackCDTimer = 0f;
		canAttack = true;
		yield return new WaitForSeconds(1.5f);
		isForceActive = false;
		breathEffect.Stop();
		waterFlow.Stop();
	}

	private IEnumerator TentacleAttackFollow()
	{
		while (tentacleAttackLengthTimer < tentacleAttackLength)
		{
			tentacleAttackCDTimer += 0.01f;
			tentacleAttackLengthTimer += 0.01f;
			if (tentacleAttackCDTimer > tentacleAttackCD.result)
			{
				tentacleAttackCDTimer = 0f;
				tentacleAttackCD.RandomResult();
				float[] tentaclePos = new float[5];
				float x = PlayerMgr.Inst.PlayerPoint.x;
				int num = 1;
				for (int j = 1; j < 3; j++)
				{
					if (x + (float)j * 3.5f < rightDownPivot.position.x)
					{
						tentaclePos[j] = x + (float)j * 3.5f;
						continue;
					}
					tentaclePos[j] = x - (float)(num + 2) * 3.5f;
					num++;
				}
				tentaclePos[0] = x;
				num = 1;
				for (int k = 1; k < 3; k++)
				{
					if (x - (float)k * 3.5f > roomCenter.x - roomWidth / 2f)
					{
						tentaclePos[k + 2] = x - (float)k * 3.5f;
						continue;
					}
					tentaclePos[k + 2] = x + (float)(num + 2) * 3.5f;
					num++;
				}
				Array.Sort(tentaclePos);
				for (int i = 4; i >= 0; i--)
				{
					Vector3 playerPoint = PlayerMgr.Inst.PlayerPoint;
					if (UnityEngine.Random.Range(0, 2) == 0)
					{
						Boss9_Tentacle component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Tentacle", new Vector3(tentaclePos[i], roomCenter.y + roomHeight / 2f, base.transform.position.z)).GetComponent<Boss9_Tentacle>();
						component.rotateObj.transform.eulerAngles = Tool2D.GetEulerAngleByDir(Tool2D.IgnoreZV2ToV1Normal(new Vector3(Mathf.Clamp(playerPoint.x, component.transform.position.x - 3f, component.transform.position.x + 3f), roomCenter.y, base.transform.position.z), component.transform.position));
						component.holeSpriteRenderer.gameObject.SetActive(value: true);
						component.SetReadyEffectPosition(isUp: true, isTilt: true);
					}
					else
					{
						Boss9_Tentacle component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Tentacle", new Vector3(tentaclePos[i], roomCenter.y - roomHeight / 2f - 0.4f - downTentacleOffste, base.transform.position.z)).GetComponent<Boss9_Tentacle>();
						component2.rotateObj.transform.eulerAngles = Tool2D.GetEulerAngleByDir(Tool2D.IgnoreZV2ToV1Normal(new Vector3(Mathf.Clamp(playerPoint.x, component2.transform.position.x - 3f, component2.transform.position.x + 3f), roomCenter.y, base.transform.position.z), component2.transform.position));
						component2.holeSpriteRenderer.gameObject.SetActive(value: false);
						component2.SetReadyEffectPosition(isUp: false, isTilt: true);
					}
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield return new WaitForSeconds(0.01f);
		}
		tentacleAttackLengthTimer = 0f;
		generalAttackCDTimer = 0f;
		canAttack = true;
		yield return new WaitForSeconds(1.5f);
		isForceActive = false;
		breathEffect.Stop();
		waterFlow.Stop();
	}

	private IEnumerator ShortAttack()
	{
		tentacleUpAnimator.Play("HighBeat");
		yield return new WaitForSeconds(0.5f);
	}

	private IEnumerator GenerateFish()
	{
		UnitSpellModifier usm = UnitBase.GetSSPModifier(in fishSsp);
		usm.Direction = Vector3.left;
		usm.Speed = fishSpellSpeed;
		while (fishLengthTimer < fishLength - 3f)
		{
			fishLengthTimer += 0.1f;
			fishCDTimer += 0.1f;
			if (fishCDTimer > fishCD.result)
			{
				fishCD.RandomResult();
				fishCDTimer = 0f;
				int[] array = new int[fishCount];
				for (int i = 0; i < fishCount; i++)
				{
					int num = UnityEngine.Random.Range(0, 8);
					float y = roomCenter.y + roomHeight / 2f - roomHeight / 8f * (float)num;
					if (!array.Contains(num))
					{
						array[i] = num;
						usm.SpawnPosition = new Vector3(0f, 0f, 0f - fishSpellHeight) + new Vector3(roomCenter.x + roomWidth / 2f + fishOffset.RandomResult(), y, base.transform.position.z);
						usm.Float1 = spellAmplitude;
						usm.Float2 = spellFrequency;
						usm.ApplyToSSP(ref fishSsp);
						fishSsp.MovementComponentData.IsIgnoreWall = true;
						ShootSpell(fishSsp);
					}
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(2f);
		fishLengthTimer = 0f;
		generalAttackCDTimer = 0f;
		canAttack = true;
		isForceActive = false;
		breathEffect.Stop();
		waterFlow.Stop();
	}

	public void BodyBeatAnim()
	{
		bodySpine.Play("Body_Beat", 0);
		bodySpine.Play("Eye_Blink", 2);
	}

	public void BulletAttack(Vector3 bulletPivot)
	{
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in waterSsp);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9Trace", bulletPivot, 5f).transform.localScale = new Vector3(3f, 3f, 3f);
		float num = UnityEngine.Random.Range(0, 360);
		int num2 = bulletCount;
		if (canChangeToInvincible)
		{
			num2 += 5;
			sSPModifier.Damage = stage2Damage;
		}
		else
		{
			sSPModifier.Damage = stage1Damage;
		}
		for (int i = 0; i < num2; i++)
		{
			sSPModifier.Direction = Tool2D.GetDir(Tool2D.GetDir(Vector3.up, num), (float)i * 360f / (float)num2);
			sSPModifier.Speed = waterSpellSpeed;
			sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - waterSpellHeight) + bulletPivot + sSPModifier.Direction * 0.8f;
			sSPModifier.ApplyToSSP(ref waterSsp);
			ShootSpell(waterSsp);
		}
		for (int j = 0; j < num2; j++)
		{
			sSPModifier.Direction = Tool2D.GetDir(Tool2D.GetDir(Vector3.up, num + 180f / (float)num2), (float)j * 360f / (float)num2);
			sSPModifier.Speed = waterSpellSpeed - 1f;
			sSPModifier.SpawnPosition = new Vector3(0f, 0f, 0f - waterSpellHeight) + bulletPivot;
			sSPModifier.ApplyToSSP(ref waterSsp);
			ShootSpell(waterSsp);
		}
	}

	private IEnumerator DelayAttack()
	{
		yield return new WaitForSeconds(1.5f);
		switch (attackType)
		{
		case 0:
			bulletAttackCount++;
			StartCoroutine(GenerateFish());
			break;
		case 1:
			tentacleAttackCount++;
			StartCoroutine(TentacleAttack());
			break;
		case 2:
			tentacleAttackCount++;
			StartCoroutine(TentacleAttackCross());
			break;
		case 3:
			tentacleAttackCount++;
			StartCoroutine(TentacleAttackFollow());
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (animaName)
		{
		case "Attack":
		{
			if (debuffType == 0)
			{
				breathEffect.Play();
				isForceActive = true;
				inhaleEffect.Stop();
				break;
			}
			int num = 3;
			if (canChangeToInvincible)
			{
				num = 5;
			}
			for (int i = 0; i < num; i++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Ink", base.transform.position).GetComponent<Boss9_Ink>().dir = Tool2D.GetDir(Tool2D.GetDir(Tool2D.GetDir(60f), 60f / (float)(num - 1) * (float)i), UnityEngine.Random.Range(-8f, 8f));
			}
			break;
		}
		case "BreathIn":
			SEMgr.Inst.boss9_BreathIn.PlaySE();
			Debug.Log("BreathIn");
			break;
		case "BreathOut":
			SEMgr.Inst.boss9_BreathOut.PlaySE();
			Debug.Log("BreathOut");
			break;
		}
	}

	public void SetInvincible()
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.InvincibleRegister();
		SetComponentData(componentData);
		isInvincible = true;
	}

	public void SetUnInvincible()
	{
		invincibleTimer = 0f;
		invincibleCDTimer = 0f;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.InvincibleUnregister();
		SetComponentData(componentData);
		isInvincible = false;
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		if (componentData.unitCfg.currentHP < componentData.unitCfg.maxHP / 2f && !canChangeToInvincible)
		{
			canChangeToInvincible = true;
			invincibleCDTimer = 99f;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_Corpse", base.transform.position);
		MusicMgr.Inst.UpdateThemeMusic();
		base.AfterDead(ref info);
		waterMaterial1.SetFloat("_Strength", 0f);
		waterMaterial2.SetFloat("_Strength", 0f);
		base.transform.position = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		SyncDotsPosition();
		DataMgr.selectedWorldData.daveKilledBoss1 = true;
	}

	protected override void BossDeadStay()
	{
		StopAllCoroutines();
		tentacleUpAnimator.enabled = false;
		blinded = false;
		hasDarkView = false;
		waterFlow.Stop();
		state = MonsterState.Dead;
		base.Anima.Play("Eye_Idle", 1);
		upTentacleSpine.AnimationState.SetAnimation(1, "Dead", loop: false);
		downTentacleSpine.AnimationState.SetAnimation(1, "Dead", loop: false);
		base.Anima.Play("Dead");
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		base.deadStayed = true;
	}

	public void AddBlinded()
	{
		if (!base.deadStayed)
		{
			blinded = true;
			blindTimmer = 0f;
		}
	}
}
