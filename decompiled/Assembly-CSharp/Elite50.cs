using UnityEngine;

public class Elite50 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		RandomMove,
		Aim,
		Attack
	}

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("表现")]
	public ParticleSystem fireParticle;

	public ShockParam shock;

	[Header("行动")]
	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public VariableFloat attackCD;

	private float attackCDTimer;

	[Header("攻击")]
	public float CannonTime;

	public VariableFloat ScatterCannonTime;

	public int ScatterCannonCount;

	public VariableFloat CannonOffset;

	public VariableFloat ScatterCannonOffset;

	private UIEndlessEliteHpBar hpBar;

	private bool useScatterCannon;

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

	public override void SingleInitialCallback()
	{
		hpBar = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIEndlessEliteHpBar"), myPpt.tsf_Layer.position + new Vector3(0f, myPpt.unitCfg.relicShowHPUIHight - 0.2f, 0f) * myPpt.tsf_Layer.lossyScale.y, Quaternion.identity, myPpt.tsf_Layer).GetComponent<UIEndlessEliteHpBar>();
		hpBar.Initialize(this);
	}

	public override void EveryInitialCallback()
	{
		hpBar.gameObject.SetActive(value: true);
		useScatterCannon = false;
	}

	private void TryAttack()
	{
		attackCDTimer += Time.deltaTime;
		if (attackCDTimer > attackCD.result)
		{
			attackCD.RandomResult();
			attackCDTimer = 0f;
			state = MonsterState.Aim;
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
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.RandomMove;
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.RandomMove:
			if (changedState)
			{
				base.Anima.Play("Move");
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			TryAttack();
			if (navInfo.allCornerArrived || stateExistTime > randomMoveTime.result)
			{
				stateExistTime = 0f;
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				GetNavInfo(base.transform.position + Tool2D.GetDir() * randomMoveRadius.result);
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			break;
		case MonsterState.Aim:
			if (changedState)
			{
				base.Anima.Play("Aim");
			}
			SetMove(Vector3.zero, isFlip: false);
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Attack");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "AimFinish":
			state = MonsterState.Attack;
			break;
		case "AttackFinish":
			state = MonsterState.RandomMove;
			break;
		case "Attack":
		{
			fireParticle.Play();
			CamController.Inst.SetShock(shock);
			SEMgr.Inst.monster309_Cannon.PlaySE();
			Vector3 vector = new Vector3(fireParticle.transform.position.x, base.transform.position.y, 0f - fireParticle.transform.position.y + base.transform.position.y);
			Vector3 startPoint = PlayerMgr.Inst.PlayerPoint;
			GetNearestTarget();
			if (base.HaveTarget)
			{
				startPoint = base.TargetPoint;
			}
			if (targetEntity == PlayerMgr.Inst.PlayerEtt)
			{
				startPoint += 0.5f * PlayerMgr.Inst.PlayerCtrller.CurrentMotion;
			}
			if (!useScatterCannon)
			{
				startPoint = Tool2D.GetNavMeshPointIngoreZ(startPoint, CannonOffset);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite50_Cannon_Big", vector).GetComponent<Monster309_Cannon>().InitializeCannon(vector, startPoint, CannonTime, myPpt.myEntity, buffed: false);
			}
			else
			{
				if (targetEntity == PlayerMgr.Inst.PlayerEtt)
				{
					startPoint += ScatterCannonTime.RandomResult() * 0.6f * PlayerMgr.Inst.PlayerCtrller.CurrentMotion;
				}
				for (int i = 0; i <= ScatterCannonCount; i++)
				{
					Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(startPoint, ScatterCannonOffset);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite50_Cannon", vector).GetComponent<Monster309_Cannon>().InitializeCannon(vector, navMeshPointIngoreZ, ScatterCannonTime.RandomResult(), myPpt.myEntity, buffed: false);
				}
			}
			useScatterCannon = !useScatterCannon;
			break;
		}
		}
	}
}
