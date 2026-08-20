using UnityEngine;

public class Boss5_LargeTentacle : UnitBase
{
	public enum MonsterState
	{
		Show,
		Idle,
		Hide,
		Attack,
		Invisible
	}

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	[Header("效果")]
	public ParticleSystem bornParticle;

	public ParticleSystem chargeParticle;

	public ParticleSystem attackParticle;

	[Header("攻击")]
	public float bulletCount;

	public VariableFloat aimAngle;

	[Header("死亡")]
	public float height;

	public int effectCount;

	public float offset;

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
			varMgr.Clear();
		}
	}

	public override void SingleInitialCallback()
	{
		if (GameMgr.IsMobile_Static)
		{
			bulletCount -= 1f;
		}
	}

	public override void EveryInitialCallback()
	{
		myPpt.CC_Self.enabled = false;
		myPpt.CanTouch = false;
		myPpt.CanBeTarget = false;
		state = MonsterState.Show;
		if (base.transform.position.x > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x)
		{
			SetFlip(-1f);
		}
		else
		{
			SetFlip(1f);
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
		switch (state)
		{
		case MonsterState.Show:
			if (changedState)
			{
				SEMgr.Inst.boss5_GroundTentacle.PlaySE();
				base.SAnima.AnimationState.SetAnimation(0, "beforeAttack2", loop: false);
				base.Anima.Play("Show", 0, 0f);
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "idle3", loop: true);
				base.Anima.Play("Idle");
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "attack4", loop: false);
				chargeParticle.Play();
				base.Anima.Play("Attack", 0, 0f);
			}
			break;
		case MonsterState.Hide:
			if (changedState)
			{
				base.Anima.Play("Hide");
			}
			break;
		case MonsterState.Invisible:
			if (changedState)
			{
				base.Anima.Play("Invisible");
			}
			break;
		}
	}

	private void LateUpdate()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		myPpt.MR_Models[0].GetPropertyBlock(materialPropertyBlock);
		if (materialPropertyBlock.GetColor("_Color") != myPpt.BaseColor)
		{
			materialPropertyBlock.SetColor("_Color", myPpt.BaseColor);
			for (int i = 0; i < myPpt.MR_Models.Length; i++)
			{
				myPpt.MR_Models[i].SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	public void Attack()
	{
		state = MonsterState.Attack;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		if (!GameMgr.IsHarmony_Static)
		{
			for (int i = 0; i < effectCount; i++)
			{
				Vector3 point = base.transform.position + new Vector3(0f, 0f, 0f - Mathf.Lerp(0f, height, (float)i / (float)effectCount)) + Tool2D.GetDir() * Random.Range(0f, offset);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Blood", point, 2f);
			}
		}
		SEMgr.Inst.boss5_WaveHit.PlaySE();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Show":
			bornParticle.Play();
			break;
		case "ShowDone":
			myPpt.CanBeTarget = true;
			myPpt.CC_Self.enabled = true;
			state = MonsterState.Idle;
			break;
		case "AttackFinish":
			state = MonsterState.Idle;
			break;
		case "PlaySE":
			SEMgr.Inst.boss5_Portal.PlaySE();
			break;
		case "Attack":
		{
			chargeParticle.Stop();
			SEMgr.Inst.boss5_BubbleShoot.PlaySE();
			Vector3 oldDir = ((base.transform.position.x > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x) ? Vector3.left : Vector3.right);
			attackParticle.Play();
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget)
			{
				oldDir = ToTargetDir();
			}
			for (int i = 0; (float)i < bulletCount; i++)
			{
				aimAngle.RandomResult();
				Boss5_Bubble component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_BubbleHigh", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<Boss5_Bubble>();
				component.Initialize(base.transform.position + Tool2D.GetDir(oldDir, aimAngle.result).normalized * 25f);
				Boss5.Inst.allBubbles.Add(component);
			}
			break;
		}
		case "AttackDone":
			base.SAnima.AnimationState.SetAnimation(0, "idle3", loop: true);
			break;
		case "HideDone":
			myPpt.CanBeTarget = false;
			myPpt.CC_Self.enabled = false;
			break;
		}
	}
}
