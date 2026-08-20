using UnityEngine;

public class Boss6_KnockGround : MonoBehaviour
{
	public enum handState
	{
		Prepare,
		Attack,
		Fade
	}

	[Header("状态")]
	public handState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("表现")]
	public Animator anima;

	public Transform tsf_Portal;

	public Transform tsf_Hand;

	public AnimaEvent animaEvent;

	[Header("数值")]
	public Collider thisCollider;

	public int damage;

	public float knockBack;

	public ShockParam shock;

	public handState state
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

	private void Start()
	{
		animaEvent.DoAction = AnimaAction;
	}

	public void Initialize()
	{
		state = handState.Prepare;
	}

	private void Update()
	{
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
		case handState.Prepare:
			if (changedState)
			{
				anima.Play("Prepare");
			}
			break;
		case handState.Attack:
			if (changedState)
			{
				anima.Play("Attack");
			}
			break;
		case handState.Fade:
			if (changedState)
			{
				anima.Play("Fade");
			}
			break;
		}
	}

	public void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "PrepareFinish":
			state = handState.Attack;
			break;
		case "Attack":
			CamController.Inst.SetShock(shock);
			thisCollider.enabled = true;
			break;
		case "AttackFinish":
			thisCollider.enabled = false;
			state = handState.Fade;
			break;
		case "FadeFinish":
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		UnitProperty component = other.GetComponent<UnitProperty>();
		TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
		takeDamageInfo.damage = damage;
		if (component == null)
		{
			return;
		}
		takeDamageInfo.knockbackForce = (base.transform.position - component.transform.position).normalized * knockBack;
		takeDamageInfo.teammateTakeDamageRatio = 4f;
		string text = "EF_MonsterPunch_Large";
		switch (other.tag)
		{
		case "Player":
			if (other.IsPlayerTrigger())
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, component.transform.position, 3f);
				component.TakeDamage(damage, AttackerType.NothingSpecial, takeDamageInfo);
			}
			break;
		case "Teammate":
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, component.transform.position, 3f);
			component.TakeDamage(damage, AttackerType.NothingSpecial, takeDamageInfo);
			break;
		case "Brittleness":
			component.TakeDamage(damage, AttackerType.NothingSpecial, takeDamageInfo);
			break;
		case "Destructible":
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, component.transform.position, 3f);
			component.TakeDamage(999f, AttackerType.NothingSpecial, takeDamageInfo);
			break;
		case "Wall":
			break;
		}
	}
}
