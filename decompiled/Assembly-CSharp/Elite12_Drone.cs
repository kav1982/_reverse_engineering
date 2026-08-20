using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Elite12_Drone : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public enum EyeState
	{
		Attack,
		Recycle
	}

	[Header("状态")]
	public EyeState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("攻击")]
	public Collider CCself;

	public VariableFloat attackInterval;

	public float aimAngle;

	public float startSpeed;

	public float flySpeed;

	public float accleration;

	public float flyTime;

	public int damage;

	public float knockBack;

	public float rotateSpeed;

	public float trackAngle;

	private Vector3 dashDirection;

	private Entity targetEntity;

	private LocalTransform targetTsf;

	[Header("表现")]
	public SpriteRenderer thisRenderer;

	public Transform tsf_Body;

	public Elite12_2 master;

	private float thisHeight;

	public ParticleSystem launchParticle;

	public ParticleSystem trailParticle;

	public ParticleSystem explodeParticle;

	public AnimaEvent animaEvent;

	public Animator anima;

	public Shadow shadow;

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

	public Entity thisEntity { get; set; }

	private void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			rotateSpeed *= 0.8f;
			flySpeed *= 0.8f;
		}
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Initialize(Elite12_2 master, Entity targetEntity, Vector3 startDir)
	{
		this.master = master;
		state = EyeState.Attack;
		thisHeight = master.droneHeight;
		Launch(targetEntity);
		dashDirection = startDir;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_Laser, CCself);
	}

	private void Update()
	{
		if (master.myPpt.AlreadyDead)
		{
			Elite12_1.MiniPool.RecycleGO(base.gameObject);
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
		tsf_Body.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * thisHeight, LayerCorrectType.Coordinate);
		bool flag = master.EntityIsValid(targetEntity);
		if (master.EntityIsValid(targetEntity))
		{
			targetTsf = UnitDotsSyncSystem.GetComponentData<LocalTransform>(targetEntity);
		}
		switch (state)
		{
		case EyeState.Attack:
		{
			ref bool reference = ref varMgr.RegBool(0);
			ref float reference2 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				trailParticle.Play();
				thisRenderer.enabled = true;
				shadow.Show();
				CCself.enabled = true;
				SEMgr.Inst.elite12DroneCast.PlaySE();
				launchParticle.Play();
				if (flag)
				{
					dashDirection = Tool2D.GetDir(((Vector3)targetTsf.Position - base.transform.position).normalized, aimAngle * (UnityEngine.Random.value - 0.5f));
				}
				reference2 = startSpeed;
			}
			if (reference2 < flySpeed)
			{
				reference2 += Time.deltaTime * accleration;
			}
			if (!flag)
			{
				reference = true;
			}
			if (!reference)
			{
				Vector3 normalized = ((Vector3)targetTsf.Position - base.transform.position).normalized;
				dashDirection = Tool2D.IgnoreZPoint(Vector3.RotateTowards(dashDirection, normalized, reference2 * rotateSpeed * (MathF.PI / 180f) * Time.deltaTime, 0f)).normalized;
				if (Tool2D.IgnoreZAngle(normalized, dashDirection) > trackAngle)
				{
					reference = true;
				}
			}
			thisRenderer.flipX = dashDirection.x > 0f;
			base.transform.position += dashDirection.normalized * reference2 * Time.deltaTime;
			break;
		}
		case EyeState.Recycle:
			if (changedState)
			{
				SEMgr.Inst.elite12DroneHit.PlaySE(SEPlayMode.Replay, 3, 0.2f);
				thisRenderer.enabled = false;
				trailParticle.Stop();
				explodeParticle.Play();
				shadow.Hide();
			}
			if (stateExistTime > 2f)
			{
				Elite12_1.MiniPool.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	public void teleportTogether(Vector3 offset)
	{
		base.transform.position += offset;
	}

	public void Launch(Entity targetPpt)
	{
		state = EyeState.Attack;
		targetEntity = targetPpt;
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (state == EyeState.Recycle)
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		switch (layer)
		{
		case 256u:
		{
			for (int i = 0; i < Elite12_1.Inst.rocks.Count; i++)
			{
				if (Elite12_1.Inst.rocks[i].thisEntity == other)
				{
					return;
				}
			}
			state = EyeState.Recycle;
			break;
		}
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite12_2.Inst.myPpt.myEntity);
				info.damage = damage;
				info.knockbackForce = dashDirection * knockBack;
				info.teammateTakeDamageRatio = 4f;
				if (result.unitCfg.unitType == UnitType.NotAttack)
				{
					info.damage = 99999f;
					info.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
				if (layer != 32768)
				{
					state = EyeState.Recycle;
				}
			}
			break;
		}
		}
	}

	public void AnimaAction(string animaName)
	{
		if (!(animaName == "Attack"))
		{
			_ = animaName == "AttackFinish";
		}
	}
}
