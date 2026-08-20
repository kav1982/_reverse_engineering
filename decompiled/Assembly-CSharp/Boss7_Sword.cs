using UnityEngine;

public class Boss7_Sword : MonoBehaviour
{
	public enum SwordState
	{
		Invisible,
		BeforeSlash,
		BeforeRotate,
		Slash,
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

	public BoxCollider trigger;

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

	[Header("回正")]
	public float recycleTime;

	public AnimationCurve recycleCurve;

	[Header("数值")]
	public float knockback;

	public int damage;

	public float environmentExtraDamage;

	private StateVariableMgr varMgr = new StateVariableMgr();

	public Boss7 master;

	public SwordState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

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

	public void Initialize(Boss7 master)
	{
		this.master = master;
		state = SwordState.Invisible;
		triggerIn.Initialize(TriggerIn);
		sr_MagicBlade.material.SetFloat("_Process", 0f);
	}

	public void Update()
	{
		if (master.myPpt.BaseColor != sr_Hand.color)
		{
			sr_Hand.color = master.myPpt.BaseColor;
			sr_Sword.color = master.myPpt.BaseColor;
		}
		tsf_BladeShadowRoot.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		float num = ((!(tsf_BladeRoot.transform.right.y > 0.1f)) ? (-0.1f) : 0.1f);
		float b = master.tsf_ChestPoint.position.x - master.transform.position.x;
		b = Mathf.Lerp(master.tsf_ChestPoint1.position.x - master.transform.position.x, b, Tool2D.IgnoreZAngle(Vector3.up, tsf_DamageZoneRoot.right) * 2f / 180f);
		base.transform.position = master.transform.position + new Vector3(b, 0f, 0f);
		float num2 = master.tsf_ChestPoint.position.y - master.transform.position.y;
		tsf_BladeRoot.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - num2), LayerCorrectType.Coordinate) + new Vector3(0f, 0f, num2 + num);
		sr_Sword.flipY = !master.fakeFlipped;
		sr_Hand.flipY = master.fakeFlipped;
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
				tsf_SwordSprite.right = tsf_BladeRoot.transform.right;
				nowAngle = 0f;
				trigger.enabled = false;
				SetSwordDiration(Tool2D.GetDir(originDir, (float)((!isClockWise) ? 1 : (-1)) * nowAngle));
				tsf_BladeShadowRoot.gameObject.SetActive(value: true);
			}
			SetSwordDiration(Tool2D.GetDir(originDir, (float)((!isClockWise) ? 1 : (-1)) * nowAngle));
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

	public void SwordRecycle()
	{
		state = SwordState.SpinRecycle;
	}

	public void SlashAt(Vector3 Diration, bool reversed = false)
	{
		if (state == SwordState.Invisible)
		{
			if (Diration.x > 0f == !reversed)
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

	public void SlashAim(Vector3 nowDir)
	{
		originDir = Tool2D.GetDir(nowDir, isClockWise ? (slashAngle / 2f) : ((0f - slashAngle) / 2f));
	}

	private void tryRecycleSpell(SpellBase _spellBase)
	{
		if (!_spellBase.IsSameCamp(UnitType.Monster) && _spellBase.spellCfg.abilityType != SpellAbilityType.Dash)
		{
			_spellBase.PoolRecycle();
		}
	}

	public void TriggerIn(Collider other)
	{
		UnitProperty component = other.GetComponent<UnitProperty>();
		TakeDamageInfo info = new TakeDamageInfo();
		switch (other.tag)
		{
		case "Wall":
			break;
		case "SolidObj":
			break;
		case "Spell":
		{
			SpellBase componentInParent2 = other.GetComponentInParent<SpellBase>();
			if (!componentInParent2.IsSameCamp(UnitType.Monster) && !(componentInParent2 is Spell1021MagicBreaker) && !(componentInParent2 is Spell4013ArcaneBlade) && !(componentInParent2 is Spell4019BiAnLethalBlade))
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster39_Hit", componentInParent2.transform.position, 2f);
				tryRecycleSpell(componentInParent2);
				SEMgr.Inst.monster39Hit.PlaySE();
			}
			break;
		}
		case "Player":
		case "Teammate":
			component.TakeKnockback((component.transform.position - base.transform.position).normalized * knockback);
			component.TakeDamage(damage, master.myPpt, info);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster39_Hit", component.transform.position, 2f);
			SEMgr.Inst.monster39Hit.PlaySE();
			break;
		case "Destructible":
			component.TakeDamage((int)((float)damage * environmentExtraDamage), master.myPpt, info);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster39_Hit", component.transform.position, 2f);
			SEMgr.Inst.monster39Hit.PlaySE();
			break;
		case "RollBall":
		{
			Spell1002RollBall componentInParent = other.GetComponentInParent<Spell1002RollBall>();
			if (!componentInParent.IsSameCamp(UnitType.Monster))
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster39_Hit", componentInParent.transform.position, 2f);
				componentInParent.TakeDamage(damage);
				SEMgr.Inst.monster39Hit.PlaySE();
			}
			break;
		}
		case "Brittleness":
			component.TakeDamage(damage, master.myPpt, info);
			break;
		}
	}
}
