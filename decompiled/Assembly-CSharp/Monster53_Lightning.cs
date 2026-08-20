using System.Collections.Generic;
using UnityEngine;

public class Monster53_Lightning : MonoBehaviour
{
	public enum EffectState
	{
		Chase,
		Bottom,
		Charge,
		Attack,
		Fade
	}

	[Header("表现")]
	public Animator Anima;

	public ParticleSystem damageParticle;

	public LineRenderer beamRenderer;

	[Header("状态机")]
	private StateVariableMgr varMgr = new StateVariableMgr();

	private EffectState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("追踪索敌")]
	private UnitProperty targetPpt;

	public float speed;

	public float rotateSpeed;

	public float chaseTime;

	public float followTime;

	public float chargeTime;

	public float attackTime;

	public float fadeTime;

	[Header("攻击")]
	public int damage;

	public float knockback;

	public float damageInterval;

	public float attackInterval;

	public float attackRadius;

	private List<UnitProperty> attackedPpt = new List<UnitProperty>();

	public EffectState state
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

	private bool HaveTarget
	{
		get
		{
			if (targetPpt != null)
			{
				return targetPpt.AlreadyDead;
			}
			return false;
		}
	}

	private Vector3 targetDir
	{
		get
		{
			if (targetPpt != null)
			{
				return Tool2D.IgnoreZPoint(targetPpt.transform.position - base.transform.position).normalized;
			}
			Debug.Log("没有目标！");
			return Vector3.zero;
		}
	}

	public void Initialize(EffectState state)
	{
		attackedPpt.Clear();
		Anima.GetComponent<AnimaEvent>().DoAction = AnimaAction;
		this.state = state;
	}

	public void Mute()
	{
		state = EffectState.Fade;
	}

	private void Update()
	{
		beamRenderer.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position));
		beamRenderer.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, -40f)));
		stateExistTime += Time.deltaTime;
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		switch (state)
		{
		case EffectState.Chase:
		{
			ref float reference = ref varMgr.RegFloat(0);
			ref Vector3 reference2 = ref varMgr.RegV3(0);
			if (changedState)
			{
				targetPpt = PlayerMgr.Inst.GetNearestPptPlayerFirst(base.transform.position);
				Anima.Play("Monster53_L_Follow");
			}
			if (stateExistTime > chaseTime)
			{
				state = EffectState.Charge;
				break;
			}
			if (HaveTarget)
			{
				reference2 = Tool2D.DirMoveTowards(reference2, targetDir, rotateSpeed * Time.deltaTime);
			}
			else
			{
				reference2 = Vector3.zero;
				reference += Time.deltaTime;
				if (reference >= 1f)
				{
					reference = 0f;
					targetPpt = PlayerMgr.Inst.GetNearestPptPlayerFirst(base.transform.position);
				}
			}
			base.transform.position += reference2 * speed;
			break;
		}
		case EffectState.Bottom:
			if (HaveTarget)
			{
				Anima.Play("Monster53_L_Follow");
				base.transform.position = targetPpt.transform.position;
			}
			if (stateExistTime > followTime)
			{
				state = EffectState.Charge;
			}
			break;
		case EffectState.Charge:
			if (changedState)
			{
				Anima.Play("Monster53_L_Show");
			}
			break;
		case EffectState.Attack:
		{
			ref float reference3 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				Anima.Play("Monster53_L_Shock");
			}
			if (reference3 > attackInterval)
			{
				attackInterval = 0f;
				Damage();
			}
			reference3 += Time.deltaTime;
			if (stateExistTime > attackTime)
			{
				state = EffectState.Fade;
			}
			break;
		}
		case EffectState.Fade:
			if (changedState)
			{
				Anima.Play("Monster53_L_Fade");
			}
			if (stateExistTime > fadeTime)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	public void AnimaAction(string animaName)
	{
		if (animaName == "ChargeFinish")
		{
			state = EffectState.Attack;
		}
	}

	public void Damage()
	{
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, attackRadius, "Destructible", "Spell", "RollBall", "Butterfly", "Brittleness", "Player", "Teammate");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			UnitProperty component = collidersByTag[i].GetComponent<UnitProperty>();
			if (attackedPpt.Contains(component))
			{
				break;
			}
			attackedPpt.Add(component);
			if (collidersByTag[i].tag == "Spell" || collidersByTag[i].tag == "RollBall" || collidersByTag[i].tag == "Butterfly")
			{
				if (!collidersByTag[i].gameObject.activeInHierarchy)
				{
					continue;
				}
				SpellBase componentInParent = collidersByTag[i].GetComponentInParent<SpellBase>();
				if (componentInParent.spellCfg.abilityType != SpellAbilityType.FireBall)
				{
					if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
					{
						((Spell1002RollBall)componentInParent).TakeDamage(damage);
					}
					else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
					{
						((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
					}
				}
			}
			else
			{
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
				takeDamageInfo.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(collidersByTag[i].transform.position, base.transform.position) * knockback;
				collidersByTag[i].GetComponent<UnitProperty>().TakeDamage(damage, null, takeDamageInfo);
			}
		}
	}
}
