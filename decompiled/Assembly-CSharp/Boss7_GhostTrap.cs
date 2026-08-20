using UnityEngine;

public class Boss7_GhostTrap : MonoBehaviour
{
	public enum TrapState
	{
		Show,
		Wait,
		Charge,
		Dash,
		Fade
	}

	[Header("判定")]
	public float showTime;

	public float dashChargeTime;

	public float maxWaitTime;

	public float speed;

	private Vector3 direction;

	public BoxCollider checkTrigger;

	public CapsuleCollider damageTrigger;

	public float maxDashDistance;

	[Header("排序")]
	public int index;

	[Header("伤害")]
	public float damage;

	public float knockBack;

	[Header("表现")]
	public Transform tsf_Layer;

	public SpriteRenderer sprite;

	public Animator Anima;

	public ParticleSystem fadeParticle;

	public Shadow thisShadow;

	[Header("状态")]
	public TrapState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	protected bool changedState;

	protected float stateExistTime;

	public TrapState state
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

	public void Initialize(int index, Vector3 direction, Vector3 triggerCenter, Vector3 TriggerScale, float distance)
	{
		this.index = index;
		this.direction = direction.normalized;
		checkTrigger.center = triggerCenter - base.transform.position;
		checkTrigger.size = TriggerScale;
		sprite.flipX = !(direction.x >= 0f);
		maxDashDistance = distance;
		state = TrapState.Show;
	}

	private void Update()
	{
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform.position);
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
		case TrapState.Show:
			if (changedState)
			{
				checkTrigger.enabled = false;
				damageTrigger.enabled = false;
				sprite.enabled = true;
				fadeParticle.Play();
				thisShadow.Show();
				Anima.Play("Show");
			}
			if (stateExistTime > showTime)
			{
				state = TrapState.Wait;
			}
			break;
		case TrapState.Wait:
			if (changedState)
			{
				checkTrigger.enabled = true;
				damageTrigger.enabled = false;
			}
			if (stateExistTime > maxWaitTime)
			{
				state = TrapState.Charge;
			}
			break;
		case TrapState.Charge:
			if (changedState)
			{
				checkTrigger.enabled = false;
				Anima.Play("Charge");
			}
			if (stateExistTime > dashChargeTime)
			{
				state = TrapState.Dash;
			}
			break;
		case TrapState.Dash:
		{
			ref float reference = ref varMgr.RegFloat(0);
			if (changedState)
			{
				damageTrigger.enabled = true;
				Anima.Play("Dash");
			}
			base.transform.position += direction * Time.deltaTime * speed;
			reference += Time.deltaTime * speed;
			if (reference > maxDashDistance)
			{
				state = TrapState.Fade;
			}
			break;
		}
		case TrapState.Fade:
			if (changedState)
			{
				checkTrigger.enabled = false;
				damageTrigger.enabled = false;
				fadeParticle.Play();
				sprite.enabled = false;
				thisShadow.Hide();
				Boss7.Inst.TrapFadeReport(index, direction.x != 0f);
			}
			if (stateExistTime > 3f)
			{
				Boss7.MiniPool.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!(other.tag == "Player") && !(other.tag == "Teammate"))
		{
			return;
		}
		if (state == TrapState.Wait)
		{
			state = TrapState.Charge;
			checkTrigger.enabled = false;
		}
		else if (state == TrapState.Dash)
		{
			switch (other.tag)
			{
			case "Player":
			case "Teammate":
			case "Brittleness":
			case "Destructible":
			{
				UnitProperty component = other.GetComponent<UnitProperty>();
				TakeDamageInfo info = new TakeDamageInfo();
				float num = damage;
				component.TakeDamage(num, Boss7.Inst.myPpt, info);
				break;
			}
			case "RollBall":
				((Spell1002RollBall)other.GetComponentInParent<SpellBase>()).TakeDamage(damage);
				break;
			case "ButterFly":
				((Spell1003Butterfly)other.GetComponentInParent<SpellBase>()).HitEFAndRecycle();
				break;
			case "Wall":
				state = TrapState.Fade;
				break;
			case "Cliff":
				state = TrapState.Fade;
				break;
			}
		}
	}
}
