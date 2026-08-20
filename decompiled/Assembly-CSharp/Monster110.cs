using UnityEngine;

public class Monster110 : UnitBase
{
	public enum MonsterState
	{
		Idle,
		Attack,
		Rebound,
		MoveBack,
		Sleep,
		Dead
	}

	private Vector3 attackDir;

	public float reboundTime;

	public GameObject[] zoomParents;

	public float checkInterval;

	public float checkIntervalTimer;

	public SpriteRenderer spriteRenderer;

	public float attackCdTime;

	public float attackCdTimer;

	public bool pattern2;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

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

	public override void EveryInitialCallback()
	{
		state = MonsterState.Idle;
		attackCdTimer = 0f;
		base.CC_Self.enabled = true;
		myPpt.CanTouch = true;
		if (pattern2)
		{
			myPpt.InvincibleRegister();
		}
	}

	public override void Update()
	{
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
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			checkIntervalTimer += Time.deltaTime;
			if (checkIntervalTimer >= checkInterval)
			{
				GetNearestTarget();
				checkIntervalTimer = 0f;
			}
			attackCdTimer += Time.deltaTime;
			if (base.HaveTarget && attackCdTimer > attackCdTime)
			{
				if (Mathf.Abs(base.TargetPoint.x - base.transform.position.x) < 0.6f)
				{
					if (base.TargetPoint.y > base.transform.position.y)
					{
						attackDir = new Vector3(0f, 1f, 0f) * base.MoveSpeed;
						spriteRenderer.transform.SetParent(zoomParents[0].transform);
					}
					else
					{
						attackDir = new Vector3(0f, -1f, 0f) * base.MoveSpeed;
						spriteRenderer.transform.SetParent(zoomParents[1].transform);
					}
					state = MonsterState.Attack;
				}
				else if (Mathf.Abs(base.TargetPoint.y - base.transform.position.y) < 0.6f)
				{
					if (base.TargetPoint.x > base.transform.position.x)
					{
						attackDir = new Vector3(1f, 0f, 0f) * base.MoveSpeed;
						spriteRenderer.transform.SetParent(zoomParents[3].transform);
					}
					else
					{
						attackDir = new Vector3(-1f, 0f, 0f) * base.MoveSpeed;
						spriteRenderer.transform.SetParent(zoomParents[2].transform);
					}
					state = MonsterState.Attack;
				}
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Attack");
			}
			SetMove(attackDir);
			break;
		case MonsterState.Rebound:
			if (changedState)
			{
				base.Anima.Play("Rebound");
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.MoveBack:
		{
			Vector3 vector = Vector3.Lerp(attackDir, Vector3.zero, Time.deltaTime * base.MoveSpeed);
			SetMove(-vector);
			if (stateExistTime > reboundTime)
			{
				attackCdTimer = 0f;
				state = MonsterState.Idle;
			}
			break;
		}
		case MonsterState.Dead:
			if (changedState)
			{
				base.Anima.Play("Dead");
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Sleep:
			if (changedState)
			{
				base.Anima.Play("Sleep");
				myPpt.InvincibleUnregister();
			}
			SetMove(Vector3.zero);
			break;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Cliff") || collision.gameObject.CompareTag("Abyss"))
		{
			state = MonsterState.Rebound;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "ReboundEnd":
			state = MonsterState.MoveBack;
			break;
		case "Dead":
			myPpt.AnnouncedDeath();
			break;
		}
	}
}
