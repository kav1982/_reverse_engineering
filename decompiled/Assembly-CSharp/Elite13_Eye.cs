using UnityEngine;

public class Elite13_Eye : MonoBehaviour
{
	public enum EyeState
	{
		Follow,
		MoveToTarget,
		Attack,
		MoveBack,
		BouunceBullet
	}

	public Elite13 master;

	public Transform tsf_Body;

	public Shadow thisShadow;

	public SpriteRenderer thisRenderer;

	private float thisHeight;

	public ParticleSystem ballParticle;

	public ParticleSystem trailParticle;

	public AnimaEvent animaEvent;

	public Animator anima;

	public AnimationCurve moveCurve;

	[Header("状态")]
	public EyeState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("假装攻击")]
	public float moveTime;

	public float moveBackTime;

	public float attackHeight;

	private Vector3 targetPoint;

	private bool hover;

	public EyeState state
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

	private void LerpMove()
	{
		base.transform.position = Vector3.Lerp(base.transform.position, master.GetEyePos(this), Time.deltaTime * 10f);
	}

	public void SetAttack(Vector3 targetPoint)
	{
		this.targetPoint = targetPoint;
		state = EyeState.MoveToTarget;
	}

	public void PlayParticle()
	{
		ballParticle.Play();
	}

	public void Initialize(Elite13 master)
	{
		this.master = master;
		animaEvent.DoAction = AnimaAction;
		state = EyeState.Follow;
	}

	private void Update()
	{
		if (master.state == Elite13.MonsterState.ThunderStorm && !trailParticle.isPlaying)
		{
			trailParticle.Play();
		}
		if (master.myPpt.AlreadyDead)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			return;
		}
		thisRenderer.flipX = master.myPpt.SR_Models[0].flipX;
		thisRenderer.color = master.myPpt.BaseColor;
		if (master.state == Elite13.MonsterState.LightningDash && tsf_Body.gameObject.activeSelf)
		{
			tsf_Body.gameObject.SetActive(value: false);
			thisShadow.Hide();
			trailParticle.Stop();
		}
		if (master.state != Elite13.MonsterState.LightningDash && !tsf_Body.gameObject.activeSelf)
		{
			tsf_Body.gameObject.SetActive(value: true);
			thisShadow.Show();
			trailParticle.Play();
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
		case EyeState.Follow:
			if (changedState)
			{
				anima.Play("Idle");
			}
			LerpMove();
			thisHeight = Mathf.Lerp(thisHeight, master.eyeHeight, Time.deltaTime * 10f);
			tsf_Body.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * thisHeight, LayerCorrectType.Coordinate);
			if (master.state == Elite13.MonsterState.BounceBullet)
			{
				state = EyeState.BouunceBullet;
			}
			break;
		case EyeState.BouunceBullet:
			_ = changedState;
			base.transform.position = Vector3.Lerp(base.transform.position, master.GetBounceBulletEyePos(this), Time.deltaTime * 10f);
			thisHeight = Mathf.Lerp(thisHeight, master.eyeHeight, Time.deltaTime * 10f);
			tsf_Body.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * thisHeight, LayerCorrectType.Coordinate);
			if (master.state != Elite13.MonsterState.BounceBullet)
			{
				state = EyeState.Follow;
			}
			break;
		case EyeState.MoveToTarget:
		{
			if (changedState)
			{
				anima.Play("MoveOut");
			}
			ref Vector3 reference3 = ref varMgr.RegV3(0);
			ref float reference4 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				reference3 = base.transform.position;
				reference4 = thisHeight;
			}
			thisHeight = Mathf.Lerp(reference4, attackHeight, moveCurve.Evaluate(stateExistTime / moveTime));
			base.transform.position = Vector3.Lerp(reference3, targetPoint, moveCurve.Evaluate(stateExistTime / moveTime));
			tsf_Body.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * thisHeight, LayerCorrectType.Coordinate);
			if (stateExistTime > moveTime)
			{
				state = EyeState.Attack;
			}
			break;
		}
		case EyeState.Attack:
			if (changedState)
			{
				anima.Play("TeleportAttack");
			}
			break;
		case EyeState.MoveBack:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			ref float reference2 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				reference = base.transform.position;
				reference2 = thisHeight;
			}
			thisHeight = Mathf.Lerp(reference2, master.eyeHeight, moveCurve.Evaluate(stateExistTime / moveBackTime));
			base.transform.position = Vector3.Lerp(reference, master.GetEyePos(this), moveCurve.Evaluate(stateExistTime / moveBackTime));
			tsf_Body.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * thisHeight, LayerCorrectType.Coordinate);
			if (stateExistTime > moveTime)
			{
				state = EyeState.Follow;
			}
			break;
		}
		}
	}

	public void AnimaAction(string animaName)
	{
		if (!(animaName == "Attack"))
		{
			if (animaName == "AttackFinish")
			{
				state = EyeState.MoveBack;
			}
		}
		else
		{
			ballParticle.Play();
		}
	}
}
