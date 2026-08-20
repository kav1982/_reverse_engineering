using UnityEngine;

public class Boss5_MainAttack : MonoBehaviour
{
	public enum HeadState
	{
		Rest,
		Charge,
		Attack,
		AfterAttack,
		OpenMouth,
		CloseMouth
	}

	[Header("状态机")]
	public HeadState _state;

	private bool stateQuit;

	private bool changedState;

	public float stateExistTime;

	public bool freeAttack;

	public Boss5 master;

	[Header("水球表现和伤害")]
	public float spreadAngle;

	public VariableFloat aimAngle;

	public float bulletInterval;

	public int attackTimes;

	private int attackCount;

	public int bulletCount;

	public VariableInt bubbleFreeCount;

	[Header("粒子系统")]
	public ParticleSystem chargeParticle;

	public ParticleSystem attackParticle;

	public Transform bubbleParticleRoot;

	public float bubbleParticleMaxScale;

	public float knockBack;

	[Header("状态时间")]
	public float bubbleChargeTime;

	public float bubbleAttackTime;

	public float bubbleAfterAttackTime;

	public Transform mouthTransform;

	[Header("杂项")]
	public Animator anima;

	public HeadState state
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
		}
	}

	private void Start()
	{
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
		case HeadState.OpenMouth:
			if (changedState)
			{
				anima.Play("Boss5_OpenMouth");
			}
			break;
		case HeadState.Charge:
			if (changedState)
			{
				master.GetNearestTarget();
				chargeParticle.Play();
			}
			bubbleParticleRoot.transform.localScale = Vector3.one * Mathf.Lerp(0f, bubbleParticleMaxScale, stateExistTime / bubbleChargeTime);
			if (stateExistTime > bubbleChargeTime)
			{
				chargeParticle.Stop();
				state = HeadState.Attack;
			}
			break;
		case HeadState.Attack:
			if (changedState)
			{
				Attack();
			}
			if (stateExistTime > bubbleAttackTime)
			{
				state = HeadState.AfterAttack;
			}
			break;
		case HeadState.AfterAttack:
			_ = changedState;
			if (stateExistTime > bubbleAfterAttackTime)
			{
				if (freeAttack)
				{
					state = HeadState.Charge;
				}
				else
				{
					state = HeadState.CloseMouth;
				}
			}
			break;
		case HeadState.CloseMouth:
			if (changedState)
			{
				anima.Play("Boss5_CloseMouth");
			}
			break;
		case HeadState.Rest:
			if (freeAttack)
			{
				state = HeadState.OpenMouth;
			}
			break;
		}
	}

	private void Attack()
	{
		SEMgr.Inst.boss5_BubbleShoot.PlaySE();
		attackParticle.Play();
		for (int i = 0; i < bulletCount; i++)
		{
			aimAngle.RandomResult();
			Boss5_Bubble component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_BubbleQuick", Tool2D.IgnoreZPoint(mouthTransform.position)).GetComponent<Boss5_Bubble>();
			component.Initialize(base.transform.position + Tool2D.GetDir(master.targetDir, aimAngle.result).normalized * 15f);
			master.allBubbles.Add(component);
		}
	}

	public void AnimaAction(string animaName)
	{
		if (!(animaName == "MouthOpen"))
		{
			if (animaName == "MouthClose")
			{
				state = HeadState.Rest;
				anima.Play("Boss5_Idle");
			}
		}
		else
		{
			state = HeadState.Charge;
			anima.Play("Boss5_MouthOpen");
		}
	}
}
