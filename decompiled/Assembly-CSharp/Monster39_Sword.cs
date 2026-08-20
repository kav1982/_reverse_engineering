using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster39_Sword : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public enum SwordState
	{
		Invisible,
		BeforeSlash,
		BeforeRotate,
		Slash,
		StartSpin,
		Spin,
		SpinSlowDown,
		StopSpin,
		Recycle,
		SpinRecycle,
		Hide
	}

	public Transform tsf_DamageZoneRoot;

	public Transform tsf_BladeRoot;

	public Transform tsf_BladeShadowRoot;

	public Transform tsf_SwordSprite;

	public SpriteRenderer sr_MagicBlade;

	public SpriteRenderer sr_Hand;

	public SpriteRenderer sr_Sword;

	public UnityEngine.BoxCollider trigger;

	public TriggerIn triggerIn;

	public ParticleSystem bladeParticle;

	[Header("斩击")]
	public float showTime;

	public AnimationCurve bladeSpeedCurve;

	public float baseRotateSpeedAdd;

	public float baseRotateSpeedMult;

	private float nowRotateSpeed;

	public float nowAngle;

	public float slashAngle;

	private Vector3 originDir;

	public float bladeHeight;

	public bool isClockWise;

	[Header("旋转")]
	public float spinShowTime;

	public float SpinAcclerateTime;

	public AnimationCurve spinAcclerateCurve;

	public float SpinSpeed;

	[Header("回正")]
	public float recycleTime;

	public AnimationCurve recycleCurve;

	[Header("数值")]
	public float knockback;

	public int damage;

	public float environmentExtraDamage;

	private StateVariableMgr varMgr = new StateVariableMgr();

	public Monster39 master;

	public SwordState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private int shaderFlipIndex;

	private List<SpellAbilityType> allowRebounceType = new List<SpellAbilityType>
	{
		SpellAbilityType.Bullet,
		SpellAbilityType.Rollball,
		SpellAbilityType.Butterfly,
		SpellAbilityType.Laser,
		SpellAbilityType.PreFirework,
		SpellAbilityType.HoverTorch,
		SpellAbilityType.BackMP,
		SpellAbilityType.SnakeWalk,
		SpellAbilityType.Rainbow,
		SpellAbilityType.ArcaneNova,
		SpellAbilityType.Dash,
		SpellAbilityType.ManaCoin,
		SpellAbilityType.Boomerang,
		SpellAbilityType.ShiningStar,
		SpellAbilityType.MrBingArrow,
		SpellAbilityType.DimensionTraveller,
		SpellAbilityType.BulletParabola
	};

	public Vector3 towards => tsf_DamageZoneRoot.right;

	public SwordState state
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

	public Entity thisEntity { get; set; }

	public void Initialize(Monster39 master)
	{
		this.master = master;
		state = SwordState.Invisible;
		sr_MagicBlade.material.SetFloat("_Process", 0f);
	}

	public void Start()
	{
		shaderFlipIndex = Shader.PropertyToID("_FlipY");
	}

	public void OnEnable()
	{
		CollisionFilter filter_MonsterAoe = GameConst.Filter_MonsterAoe;
		filter_MonsterAoe.CollidesWith |= 16777216u;
		filter_MonsterAoe.BelongsTo |= 2048u;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter_MonsterAoe, trigger, ignoredBySpell: true);
	}

	public void Update()
	{
		if (master.myPpt.BaseColor != sr_Hand.material.color)
		{
			sr_Hand.material.color = master.myPpt.BaseColor;
			sr_Sword.material.color = master.myPpt.BaseColor;
		}
		tsf_BladeShadowRoot.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		float num = ((!(tsf_BladeRoot.transform.right.y > 0.1f)) ? (-0.1f) : 0.1f);
		float b = master.tsf_ChestPoint.position.x - master.transform.position.x;
		b = Mathf.Lerp(master.tsf_ChestPoint1.position.x - master.transform.position.x, b, Tool2D.IgnoreZAngle(Vector3.up, tsf_DamageZoneRoot.right) * 2f / 180f);
		base.transform.position = master.transform.position + new Vector3(b, 0f, 0f);
		float num2 = master.tsf_ChestPoint.position.y - master.transform.position.y;
		tsf_BladeRoot.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - num2), LayerCorrectType.Coordinate) + new Vector3(0f, 0f, num2 + num) * 0.01f;
		sr_Sword.material.SetFloat(shaderFlipIndex, master.fakeFlipped ? 1 : (-1));
		sr_Hand.material.SetFloat(shaderFlipIndex, (!master.fakeFlipped) ? 1 : (-1));
		if (master.swordLocked)
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
		case SwordState.Invisible:
			if (changedState)
			{
				trigger.enabled = false;
				sr_MagicBlade.material.SetFloat("_Process", 0f);
				bladeParticle.Stop();
				bladeParticle.Clear();
				tsf_BladeShadowRoot.gameObject.SetActive(value: false);
			}
			if (master.fakeFlipped)
			{
				SetSwordDiration(Vector3.left);
			}
			else
			{
				SetSwordDiration(Vector3.right);
			}
			tsf_SwordSprite.eulerAngles = new Vector3(0f, 0f, 90f);
			break;
		case SwordState.BeforeSlash:
			if (changedState)
			{
				SEMgr.Inst.monster39BladeShow.PlaySE();
				tsf_SwordSprite.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, tsf_BladeRoot.transform.up));
				nowAngle = 0f;
				trigger.enabled = false;
				SetSwordDiration(Tool2D.GetDir(originDir, (float)((!isClockWise) ? 1 : (-1)) * nowAngle));
				tsf_BladeShadowRoot.gameObject.SetActive(value: true);
			}
			sr_MagicBlade.material.SetFloat("_Process", stateExistTime / showTime);
			if (stateExistTime >= showTime)
			{
				state = SwordState.Slash;
			}
			break;
		case SwordState.Slash:
			if (changedState)
			{
				bladeParticle.Play();
				trigger.enabled = true;
				nowAngle = 0f;
			}
			nowRotateSpeed = baseRotateSpeedAdd + baseRotateSpeedMult * bladeSpeedCurve.Evaluate(nowAngle / slashAngle);
			nowAngle += nowRotateSpeed * Time.deltaTime;
			SetSwordDiration(Tool2D.GetDir(originDir, (float)((!isClockWise) ? 1 : (-1)) * nowAngle));
			if (nowAngle >= slashAngle)
			{
				state = SwordState.Hide;
			}
			break;
		case SwordState.StartSpin:
			if (changedState)
			{
				tsf_SwordSprite.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, tsf_BladeRoot.transform.up));
				nowAngle = 0f;
				trigger.enabled = false;
				SetSwordDiration(Tool2D.GetDir(originDir, (float)((!isClockWise) ? 1 : (-1)) * nowAngle));
				tsf_BladeShadowRoot.gameObject.SetActive(value: true);
			}
			sr_MagicBlade.material.SetFloat("_Process", stateExistTime / spinShowTime);
			break;
		case SwordState.Spin:
			if (changedState)
			{
				bladeParticle.Play();
				trigger.enabled = true;
				nowAngle = 0f;
				SEMgr.Inst.monster39Slash.PlaySE();
			}
			nowRotateSpeed = SpinSpeed * spinAcclerateCurve.Evaluate(stateExistTime / SpinAcclerateTime);
			nowAngle += nowRotateSpeed * Time.deltaTime;
			SetSwordDiration(Tool2D.GetDir(originDir, (float)((!isClockWise) ? 1 : (-1)) * nowAngle));
			if (nowAngle >= 360f)
			{
				nowAngle = 0f;
				SEMgr.Inst.monster39Slash.PlaySE();
			}
			break;
		case SwordState.SpinSlowDown:
			nowRotateSpeed = SpinSpeed * spinAcclerateCurve.Evaluate(1f - stateExistTime / SpinAcclerateTime);
			nowAngle += nowRotateSpeed * Time.deltaTime;
			SetSwordDiration(Tool2D.GetDir(originDir, (float)((!isClockWise) ? 1 : (-1)) * nowAngle));
			if (nowRotateSpeed <= 0f)
			{
				state = SwordState.StopSpin;
			}
			break;
		case SwordState.StopSpin:
			if (changedState)
			{
				bladeParticle.Stop();
				trigger.enabled = false;
				tsf_BladeShadowRoot.gameObject.SetActive(value: false);
			}
			sr_MagicBlade.material.SetFloat("_Process", Mathf.Max(0f, 1f - stateExistTime / spinShowTime));
			break;
		case SwordState.Recycle:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			ref float reference2 = ref varMgr.RegFloat(0);
			ref float reference3 = ref varMgr.RegFloat(1);
			if (changedState)
			{
				if (master.fakeFlipped)
				{
					reference = new Vector3(-1f, 0f, 0f);
					reference3 = -90f;
				}
				else
				{
					reference3 = 90f;
					reference = new Vector3(1f, 0f, 0f);
				}
				reference2 = Tool2D.IgnoreZAngleWithSign(reference, tsf_DamageZoneRoot.right);
				if (master.fakeFlipped && reference2 < 0f)
				{
					reference2 += 360f;
				}
				if (!master.fakeFlipped && reference2 > 0f)
				{
					reference2 -= 360f;
				}
			}
			if (stateExistTime > recycleTime)
			{
				state = SwordState.Invisible;
				break;
			}
			tsf_SwordSprite.localEulerAngles = new Vector3(0f, 0f, reference3 * recycleCurve.Evaluate(stateExistTime / recycleTime));
			SetSwordDiration(Tool2D.GetDir(reference, reference2 * recycleCurve.Evaluate(1f - stateExistTime / recycleTime)));
			break;
		}
		case SwordState.SpinRecycle:
		{
			ref Vector3 reference4 = ref varMgr.RegV3(0);
			ref float reference5 = ref varMgr.RegFloat(0);
			ref float reference6 = ref varMgr.RegFloat(1);
			if (changedState)
			{
				if (master.fakeFlipped)
				{
					reference4 = new Vector3(-1f, 0f, 0f);
					reference6 = -90f;
				}
				else
				{
					reference6 = 90f;
					reference4 = new Vector3(1f, 0f, 0f);
				}
				reference5 = Tool2D.IgnoreZAngleWithSign(reference4, tsf_DamageZoneRoot.right);
			}
			if (stateExistTime > recycleTime)
			{
				state = SwordState.Invisible;
				break;
			}
			tsf_SwordSprite.localEulerAngles = new Vector3(0f, 0f, reference6 * recycleCurve.Evaluate(stateExistTime / recycleTime));
			SetSwordDiration(Tool2D.GetDir(reference4, reference5 * recycleCurve.Evaluate(1f - stateExistTime / recycleTime)));
			break;
		}
		case SwordState.Hide:
			if (changedState)
			{
				tsf_BladeShadowRoot.gameObject.SetActive(value: false);
				bladeParticle.Stop();
				trigger.enabled = false;
			}
			sr_MagicBlade.material.SetFloat("_Process", 1f - stateExistTime / showTime);
			if (stateExistTime > showTime)
			{
				state = SwordState.Recycle;
			}
			break;
		case SwordState.BeforeRotate:
			break;
		}
	}

	public void SetSwordDiration(Vector3 dir)
	{
		float z = Tool2D.IgnoreZAngleWithSign(Vector3.right, dir);
		tsf_DamageZoneRoot.localEulerAngles = new Vector3(0f, 0f, z);
		tsf_BladeRoot.localEulerAngles = new Vector3(0f, 0f, z);
		tsf_BladeShadowRoot.localEulerAngles = new Vector3(0f, 0f, z);
	}

	public void SpinPrepare(Vector3 Diration)
	{
		if (state == SwordState.Invisible)
		{
			if (Diration.x > 0f)
			{
				isClockWise = true;
			}
			else
			{
				isClockWise = false;
			}
			originDir = Tool2D.GetDir(Diration, isClockWise ? (slashAngle / 2f) : ((0f - slashAngle) / 2f));
			state = SwordState.StartSpin;
		}
	}

	public void SpinStart()
	{
		state = SwordState.Spin;
	}

	public void SpinStop()
	{
		state = SwordState.SpinSlowDown;
	}

	public void SwordRecycle()
	{
		state = SwordState.SpinRecycle;
	}

	public void SlashAt(Vector3 Diration)
	{
		if (state == SwordState.Invisible)
		{
			if (Diration.x > 0f)
			{
				isClockWise = true;
			}
			else
			{
				isClockWise = false;
			}
			originDir = Tool2D.GetDir(Diration, isClockWise ? (slashAngle / 2f) : ((0f - slashAngle) / 2f));
			state = SwordState.BeforeSlash;
		}
	}

	private void tryRecycleSpell(SpellBase _spellBase)
	{
		if (!_spellBase.IsSameCamp(UnitType.Monster) && _spellBase.spellCfg.abilityType != SpellAbilityType.Dash)
		{
			_spellBase.PoolRecycle();
		}
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		SpellConfigComponentData result2;
		if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
		{
			LocalTransform componentData = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other);
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = ((Vector3)componentData.Position - base.transform.position).normalized * knockback;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			_ = result.unitCfg.unitType;
			_ = 8;
		}
		else if (UnitDotsSyncSystem.TryGetComponent<SpellConfigComponentData>(other, out result2) && allowRebounceType.Contains(result2.AbilityType))
		{
			UnitDotsSyncSystem.entityMgr.SetComponentEnabled<SpellDestroyTag>(other, value: true);
			LocalTransform componentData2 = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster39_Hit", componentData2.Position, 2f);
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}
}
