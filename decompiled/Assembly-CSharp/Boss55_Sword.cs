using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss55_Sword : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public enum SwordState
	{
		Idle,
		Aim,
		Attack,
		BackToIdle,
		DashTeleport,
		DashAim,
		Dash,
		DashAfter,
		DashAfterBack,
		LaserAim,
		RotateSmallLaserAim,
		RotateSmallLaser,
		FallGround,
		FreeMove,
		FreeMoveAim,
		FreeMoveAttackAfter,
		RotateSlashAim,
		RotateSlash,
		RotateSlashAfter,
		CurveSlashAim,
		CurveSlash,
		SideSlashPrepare,
		SideSlash,
		SideSlashAfter,
		Hide,
		Show
	}

	private StateVariableMgr varMgr = new StateVariableMgr();

	public SwordState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("射击")]
	public ParticleSystem shootParticle;

	public float bulletSpeed;

	public VariableFloat crossBulletAngleRange;

	[Header("创人")]
	public float fadeTime;

	public float dashAimTime;

	public float stopAimTime;

	public float dashTime;

	public float startDashSpeed;

	public float maxDashSpeed;

	public float dashSpeedAccleration;

	public float dashDamage;

	public float knockBack;

	public float dashPredictTime;

	public VariableFloat dashAimOffset;

	public UnityEngine.BoxCollider bc;

	private Vector3 dashPoint;

	private Vector3 dashDir;

	private bool dashAim;

	private float nowDashSpeed;

	public bool dashAutoBack;

	public LineRenderer lr_Warning;

	public LineRenderer lr_FinalWarning;

	public AnimationCurve finalWarningTransparentCurve;

	public LineRenderer lr_WarningShadow;

	public ParticleSystem dashParticle;

	public ParticleSystem dashShadowParticle;

	public ParticleSystem dashChargeParticle;

	public float dashBulletSpeed;

	public float dashBulletInterval;

	[Header("激光")]
	public Boss55_Laser laser;

	[Header("自由行动")]
	public float freeSmoothTime;

	public float freeMoveSpeed;

	public float freeMoveAimTime;

	public float freeMoveAttackAfterTime;

	public VariableFloat freeMoveKeepAngleRange;

	public VariableFloat freeMoveKeepDistance;

	[Header("曲线斩")]
	public float curveSmoothTime;

	public AnimationCurve curvePosCurve;

	public float curveDuration;

	public float curveExtraDuration;

	public SpriteRenderer sr_Warning;

	private Vector3 startPoint;

	private Vector3 middlePoint;

	private Vector3 endPoint;

	[Header("侧向斩")]
	public LineRenderer lr_SlashWarning;

	public ParticleSystem sideSlashParticle;

	public ParticleSystem sideSlashShadowParticle;

	public AnimationCurve sideSlashCurve;

	public float sideSlashWidth;

	public float sideSlashAngle;

	public float sideSlashTime;

	public float sideSlashAfterTime;

	public float sideSlashWarningTime;

	private Vector3 sideSlashCenterPoint;

	private Vector3 sideSlashStartDir;

	private float sideSlashDistance;

	private float sideSlashRotateDir;

	private float sideSlashLastAngle;

	private List<UnitDotsSyncSystem.DistanceHitResult> sideSlashHitTargets = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private HashSet<Entity> sideSlashDamagedEntities = new HashSet<Entity>();

	[Header("旋转剑阵")]
	public ParticleSystem rotateSlashParticle;

	public ParticleSystem rotateSlashShadowParticle;

	public float rotateSlashWarningPreviewTime;

	private Vector3 rotateSlashCenterPoint;

	private float rotateSlashFromCenterDistance;

	private float rotateSlashAngleSpeed;

	public float rotateSlashAfterTime;

	[Header("旋转小激光")]
	public float smallLaserRotateSpeed;

	public float smallLaserRotateWarningTime;

	public float smallLaserRotateDuration;

	private Vector3 smallLaserRotateStartPoint;

	private Vector3 smallLaserRotateTargetOffset;

	private float smallLaserRotateDir;

	private float smallLaserRotateTimer;

	[Header("表现")]
	public float smoothTime;

	public Transform tsf_Layer;

	public Transform tsf_shadowRotateRoot;

	public Transform tsf_rotateRoot;

	public SpriteRenderer sr_Sword;

	public SpriteRenderer sr_Shadow;

	public float battleHeight;

	public float rotateSpeed;

	private Transform tsf_IdlePoint;

	private Vector3 smoothSpeed;

	private float heightLerp;

	private float shadowLerp;

	private Vector3 nowDir;

	private Vector3 battleTargetPos;

	private Vector3 battleTargetDir;

	private bool isRight;

	[Header("隐藏")]
	public float showTime;

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

	public Entity thisEntity { get; set; }

	public bool isAutoLaser { get; set; }

	private Vector3 idlePosition => new Vector3(tsf_IdlePoint.position.x, Boss55.Inst.transform.position.y, 0f);

	private Vector3 idleDir => -tsf_IdlePoint.up;

	private float idleHeight => tsf_IdlePoint.position.y - Boss55.Inst.transform.position.y;

	private float nowHeight => Mathf.Lerp(idleHeight, battleHeight, heightLerp);

	public void SetFlip(bool flipX)
	{
		SwordState swordState = state;
		if (swordState == SwordState.Idle || swordState == SwordState.BackToIdle || swordState == SwordState.DashAfterBack || swordState == SwordState.Hide)
		{
			if (isRight)
			{
				tsf_rotateRoot.localScale = new Vector3(flipX ? 1 : (-1), 1f, 1f);
			}
			else
			{
				tsf_rotateRoot.localScale = new Vector3((!flipX) ? 1 : (-1), 1f, 1f);
			}
			if (state == SwordState.Idle)
			{
				base.transform.position = idlePosition;
			}
		}
	}

	public void SetBattlePose(Vector3 postion, Vector3 aimDir)
	{
		battleTargetPos = postion;
		battleTargetDir = aimDir;
	}

	public void ForceSetBattlePose(Vector3 postion, Vector3 aimDir)
	{
		battleTargetPos = postion;
		battleTargetDir = aimDir;
		base.transform.position = battleTargetPos;
		nowDir = battleTargetDir;
	}

	public void SetDash(Vector3 dashPoint)
	{
		state = SwordState.DashTeleport;
		this.dashPoint = dashPoint;
		dashAim = true;
	}

	public void SetDash(Vector3 dashPoint, Vector3 dashDir)
	{
		state = SwordState.DashTeleport;
		this.dashPoint = dashPoint;
		dashAim = false;
		this.dashDir = dashDir;
	}

	public void SetCurveSlash(Vector3 startPoint, Vector3 middlePoint, Vector3 endPoint, float time)
	{
		state = SwordState.CurveSlashAim;
		this.startPoint = startPoint;
		this.middlePoint = middlePoint;
		this.endPoint = endPoint;
		curveDuration = time;
	}

	public void SetRotateSmallLaser(Vector3 startPoint, Vector3 targetOffset, float rotateDir)
	{
		smallLaserRotateStartPoint = startPoint;
		smallLaserRotateTargetOffset = targetOffset;
		smallLaserRotateDir = rotateDir;
		smallLaserRotateTimer = 0f;
		isAutoLaser = false;
		state = SwordState.RotateSmallLaserAim;
	}

	public void SetSideSlash(Vector3 startPoint, Vector3 centerPoint, float rotateDir)
	{
		sideSlashCenterPoint = centerPoint;
		sideSlashStartDir = Tool2D.IgnoreZV2ToV1Normal(startPoint, centerPoint);
		sideSlashDistance = Tool2D.IgnoreZDistance(startPoint, centerPoint);
		sideSlashRotateDir = rotateDir;
		state = SwordState.SideSlashPrepare;
	}

	public void SetRotateSlash(Vector3 startPoint, Vector3 centerPoint, float angleSpeed)
	{
		rotateSlashCenterPoint = centerPoint;
		rotateSlashFromCenterDistance = Tool2D.IgnoreZDistance(startPoint, centerPoint);
		rotateSlashAngleSpeed = angleSpeed;
		SetBattlePose(startPoint, Tool2D.IgnoreZV2ToV1Normal(startPoint, centerPoint));
		state = SwordState.RotateSlashAim;
	}

	public void SetRotateSlashPose(Vector3 centerPoint, Vector3 dir)
	{
		rotateSlashCenterPoint = centerPoint;
		SetBattlePose(centerPoint + dir * rotateSlashFromCenterDistance, dir);
	}

	public void Initialize(Transform idlePoint)
	{
		tsf_IdlePoint = idlePoint;
		state = SwordState.Idle;
		heightLerp = 0f;
		isRight = idlePoint.position.x > Boss55.Inst.transform.position.x;
		if (!isRight)
		{
			tsf_rotateRoot.localScale = new Vector3(1f, 1f, 1f);
		}
		lr_Warning.enabled = false;
		lr_WarningShadow.enabled = false;
		lr_FinalWarning.enabled = false;
		lr_SlashWarning.enabled = false;
		shadowLerp = 1f;
		sr_Warning.enabled = false;
	}

	public void SyncColor(Color color)
	{
		if (state != 0)
		{
			color = Color.white;
		}
		sr_Sword.material.SetColor(GameConstManaged.shaderSpriteColorIndex, color);
	}

	public void CrossShoot()
	{
		SEMgr.Inst.boss55BulletShoot.PlaySE();
		shootParticle.Play();
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Bullet", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<Boss55_Bullet>().Initialize(Tool2D.GetDir(nowDir, crossBulletAngleRange.RandomResult()), bulletSpeed, Boss55.Inst.myPpt.myEntity);
	}

	public void DashShoot(Vector3 shootPoint)
	{
		if (Tool2D.PointOnNavMesh(shootPoint))
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_BulletQuick", Tool2D.IgnoreZPoint(shootPoint)).GetComponent<Boss55_Bullet>().Initialize(nowDir, dashBulletSpeed, Boss55.Inst.myPpt.myEntity);
		}
	}

	public void FreeShoot()
	{
		shootParticle.Play();
		for (int i = 0; i < 3; i++)
		{
			shootParticle.Play();
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Bullet", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<Boss55_Bullet>().Initialize(Tool2D.GetDir(nowDir, -30f + (float)i * 30f), bulletSpeed, Boss55.Inst.myPpt.myEntity);
		}
	}

	public void DashSpinShoot()
	{
		SEMgr.Inst.boss55BulletShoot.PlaySE();
		shootParticle.Play();
		float num = (Boss55.Inst.inSecondStage ? 20 : 30);
		for (int i = 0; i < 3; i++)
		{
			shootParticle.Play();
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Bullet", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<Boss55_Bullet>().Initialize(Tool2D.GetDir(nowDir, 0f - num + (float)i * num + crossBulletAngleRange.RandomResult()), bulletSpeed, Boss55.Inst.myPpt.myEntity);
		}
	}

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228736u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, bc);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
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
		bc.transform.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(nowDir));
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case SwordState.Idle:
			_ = changedState;
			base.transform.position = idlePosition;
			nowDir = idleDir;
			break;
		case SwordState.Aim:
			if (changedState)
			{
				smoothSpeed = Vector3.zero;
			}
			heightLerp = Mathf.MoveTowards(heightLerp, 1f, 1f / smoothTime * Time.deltaTime);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, battleTargetPos, ref smoothSpeed, smoothTime);
			nowDir = Tool2D.RotateTowardsAroundZAxisSmooth(nowDir, battleTargetDir, rotateSpeed * Time.deltaTime, 5f);
			break;
		case SwordState.BackToIdle:
			if (changedState)
			{
				laser = null;
				smoothSpeed = Vector3.zero;
			}
			heightLerp = Mathf.MoveTowards(heightLerp, 0f, 1f / smoothTime * Time.deltaTime);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, idlePosition, ref smoothSpeed, Mathf.Min(smoothTime, Mathf.Max(0.001f, smoothTime * 2f - stateExistTime)));
			nowDir = Tool2D.RotateTowardsAroundZAxisSmooth(nowDir, idleDir, rotateSpeed * Time.deltaTime, 5f);
			if (stateExistTime > smoothTime * 2f)
			{
				state = SwordState.Idle;
			}
			break;
		case SwordState.FreeMove:
		{
			ref Vector3 reference2 = ref varMgr.RegV3(0);
			ref Vector3 reference3 = ref varMgr.RegV3(1);
			if (changedState)
			{
				reference2 = -Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position), GeneralTool.HalfChanceNPOne() * freeMoveKeepAngleRange.RandomResult()) * freeMoveKeepDistance.RandomResult();
				smoothSpeed = Vector3.zero;
				reference3 = PlayerMgr.Inst.PlayerPoint;
			}
			heightLerp = Mathf.MoveTowards(heightLerp, 1f, 1f / freeSmoothTime * Time.deltaTime);
			Vector3 target = reference3 + reference2;
			base.transform.position = Vector3.SmoothDamp(base.transform.position, target, ref smoothSpeed, freeSmoothTime);
			nowDir = Tool2D.RotateTowardsAroundZAxisSmooth(nowDir, Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position), rotateSpeed * Time.deltaTime, 5f);
			if (stateExistTime > freeSmoothTime * 2f)
			{
				state = SwordState.FreeMoveAim;
			}
			break;
		}
		case SwordState.FreeMoveAim:
			_ = changedState;
			if (stateExistTime > freeMoveAimTime)
			{
				state = SwordState.FreeMoveAttackAfter;
			}
			break;
		case SwordState.FreeMoveAttackAfter:
			if (changedState)
			{
				FreeShoot();
			}
			if (stateExistTime > freeMoveAttackAfterTime)
			{
				state = SwordState.FreeMove;
			}
			break;
		case SwordState.DashTeleport:
			_ = changedState;
			TeleportFade(fadeOut: true);
			if (stateExistTime > fadeTime)
			{
				state = SwordState.DashAim;
			}
			break;
		case SwordState.DashAim:
			if (changedState)
			{
				base.transform.position = dashPoint;
				heightLerp = 1f;
				lr_Warning.enabled = true;
				lr_WarningShadow.enabled = true;
				lr_FinalWarning.enabled = true;
				dashChargeParticle.Play();
				if (dashAim)
				{
					Vector3 v3 = PlayerMgr.Inst.PlayerPointIgnoreZ + PlayerMgr.Inst.PlayerCtrller.CurrentMotion * dashPredictTime + Tool2D.GetDir() * dashAimOffset.RandomResult();
					nowDir = Tool2D.IgnoreZV2ToV1Normal(v3, base.transform.position);
				}
				else
				{
					nowDir = dashDir;
				}
				SEMgr.Inst.boss55SwordDashWarning.PlaySE();
			}
			if (dashAim)
			{
				lr_FinalWarning.material.SetFloat(GameConstManaged.shaderTransparencyIndex, finalWarningTransparentCurve.Evaluate(stateExistTime / dashAimTime));
			}
			TeleportFade(fadeOut: false);
			SetWarning();
			if (stateExistTime > dashAimTime)
			{
				state = SwordState.Dash;
			}
			break;
		case SwordState.Dash:
		{
			ref Vector3 reference5 = ref varMgr.RegV3(0);
			if (changedState)
			{
				lr_FinalWarning.enabled = false;
				reference5 = base.transform.position;
				nowDashSpeed = startDashSpeed;
				dashChargeParticle.Stop();
				dashParticle.Clear();
				dashParticle.Play();
				dashShadowParticle.Clear();
				dashShadowParticle.Play();
				SEMgr.Inst.boss55SwordDash.PlaySE();
			}
			SetWarning();
			float num3 = Tool2D.IgnoreZDistance(base.transform.position, reference5);
			while (num3 > dashBulletInterval)
			{
				num3 -= dashBulletInterval;
				reference5 = base.transform.position - nowDir * num3;
				DashShoot(reference5);
			}
			nowDashSpeed += Time.deltaTime * dashSpeedAccleration;
			nowDashSpeed = Mathf.Min(nowDashSpeed, maxDashSpeed);
			base.transform.position += Time.deltaTime * nowDashSpeed * nowDir;
			if (stateExistTime > dashTime)
			{
				state = SwordState.DashAfter;
			}
			break;
		}
		case SwordState.DashAfter:
			if (changedState)
			{
				dashParticle.Stop();
				dashShadowParticle.Stop();
				lr_Warning.enabled = false;
				lr_WarningShadow.enabled = false;
				dashChargeParticle.Play();
			}
			base.transform.position += Time.deltaTime * nowDashSpeed * nowDir;
			TeleportFade(fadeOut: true);
			if (stateExistTime > fadeTime && dashAutoBack)
			{
				dashChargeParticle.Stop();
				state = SwordState.DashAfterBack;
			}
			break;
		case SwordState.DashAfterBack:
			if (changedState)
			{
				SEMgr.Inst.boss55SwordDashWarning.PlaySE();
				heightLerp = 0f;
			}
			TeleportFade(fadeOut: false);
			base.transform.position = idlePosition;
			nowDir = idleDir;
			if (stateExistTime > fadeTime)
			{
				state = SwordState.Idle;
			}
			break;
		case SwordState.LaserAim:
			_ = changedState;
			heightLerp = Mathf.MoveTowards(heightLerp, 1f, 1f / smoothTime * Time.deltaTime);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, battleTargetPos, ref smoothSpeed, smoothTime);
			nowDir = Tool2D.RotateTowardsAroundZAxisSmooth(nowDir, battleTargetDir, rotateSpeed * Time.deltaTime, 5f);
			if (heightLerp == 1f)
			{
				if (laser == null)
				{
					laser = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Laser", base.transform.position).GetComponent<Boss55_Laser>();
					laser.Initialize(nowDir, isAutoLaser);
				}
				else
				{
					laser.SetStartAndDir(base.transform.position, nowDir);
				}
			}
			break;
		case SwordState.RotateSmallLaserAim:
		{
			if (changedState)
			{
				smoothSpeed = Vector3.zero;
			}
			Vector3 v2 = smallLaserRotateStartPoint + smallLaserRotateTargetOffset;
			heightLerp = Mathf.MoveTowards(heightLerp, 1f, 1f / smoothTime * Time.deltaTime);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, smallLaserRotateStartPoint, ref smoothSpeed, smoothTime);
			nowDir = Tool2D.RotateTowardsAroundZAxisSmooth(nowDir, Tool2D.IgnoreZV2ToV1Normal(v2, base.transform.position), rotateSpeed * Time.deltaTime, 5f);
			if (heightLerp == 1f)
			{
				if (laser == null)
				{
					laser = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss55_Laser", base.transform.position).GetComponent<Boss55_Laser>();
					laser.Initialize(nowDir, isAutoLaser: false);
				}
				else
				{
					laser.SetStartAndDir(base.transform.position, nowDir);
				}
				if (stateExistTime > smallLaserRotateWarningTime)
				{
					state = SwordState.RotateSmallLaser;
				}
			}
			break;
		}
		case SwordState.RotateSmallLaser:
		{
			if (changedState)
			{
				smoothSpeed = Vector3.zero;
				smallLaserRotateTimer = 0f;
				laser.state = Boss55_Laser.LaserState.Attack;
			}
			smallLaserRotateTargetOffset = Tool2D.GetDir(smallLaserRotateTargetOffset, Time.deltaTime * smallLaserRotateSpeed * smallLaserRotateDir);
			Vector3 v = smallLaserRotateStartPoint + smallLaserRotateTargetOffset;
			nowDir = Tool2D.IgnoreZV2ToV1Normal(v, base.transform.position);
			if (heightLerp == 1f)
			{
				laser.SetStartAndDir(base.transform.position, nowDir);
				smallLaserRotateTimer += Time.deltaTime;
				if (smallLaserRotateTimer > smallLaserRotateDuration)
				{
					laser.state = Boss55_Laser.LaserState.Fade;
					state = SwordState.BackToIdle;
				}
			}
			break;
		}
		case SwordState.RotateSlashAim:
			if (changedState)
			{
				smoothSpeed = Vector3.zero;
				lr_SlashWarning.positionCount = 12;
				lr_SlashWarning.enabled = true;
				dashChargeParticle.Play();
			}
			heightLerp = Mathf.MoveTowards(heightLerp, 1f, 1f / smoothTime * Time.deltaTime);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, battleTargetPos, ref smoothSpeed, smoothTime);
			nowDir = Tool2D.RotateTowardsAroundZAxisSmooth(nowDir, battleTargetDir, rotateSpeed * Time.deltaTime, 5f);
			SetRotateSlashWarning();
			break;
		case SwordState.RotateSlash:
			if (changedState)
			{
				smoothSpeed = Vector3.zero;
				lr_SlashWarning.enabled = true;
				rotateSlashParticle.Play();
				rotateSlashShadowParticle.Play();
				dashChargeParticle.Stop();
			}
			heightLerp = Mathf.MoveTowards(heightLerp, 1f, 1f / smoothTime * Time.deltaTime);
			base.transform.position = battleTargetPos;
			nowDir = battleTargetDir;
			SetRotateSlashWarning();
			break;
		case SwordState.RotateSlashAfter:
			if (changedState)
			{
				lr_SlashWarning.enabled = false;
				rotateSlashParticle.Stop();
				rotateSlashShadowParticle.Stop();
			}
			if (stateExistTime > sideSlashAfterTime)
			{
				state = SwordState.BackToIdle;
			}
			break;
		case SwordState.SideSlashPrepare:
			if (changedState)
			{
				smoothSpeed = Vector3.zero;
				lr_SlashWarning.positionCount = 12;
				lr_SlashWarning.enabled = true;
				dashChargeParticle.Play();
				tsf_rotateRoot.localScale = new Vector3((sideSlashRotateDir > 0f) ? 1 : (-1), 1f, 1f);
			}
			heightLerp = Mathf.MoveTowards(heightLerp, 1f, 1f / smoothTime * Time.deltaTime);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, sideSlashCenterPoint + sideSlashStartDir * sideSlashDistance, ref smoothSpeed, smoothTime);
			nowDir = Tool2D.RotateTowardsAroundZAxisSmooth(nowDir, sideSlashStartDir, rotateSpeed * Time.deltaTime, 5f);
			SetSideSlashWarning();
			if (stateExistTime > sideSlashWarningTime)
			{
				state = SwordState.SideSlash;
			}
			break;
		case SwordState.SideSlash:
		{
			if (changedState)
			{
				SEMgr.Inst.boss55SideSlash.PlaySE();
				lr_SlashWarning.enabled = false;
				sideSlashParticle.Play();
				sideSlashShadowParticle.Play();
				dashChargeParticle.Stop();
				sideSlashLastAngle = 0f;
				sideSlashDamagedEntities.Clear();
			}
			float num = sideSlashCurve.Evaluate(stateExistTime / sideSlashTime);
			float num2 = sideSlashAngle * num * sideSlashRotateDir;
			Vector3 dir = Tool2D.GetDir(sideSlashStartDir, num2);
			base.transform.position = sideSlashCenterPoint + dir * sideSlashDistance;
			nowDir = dir;
			if (num > 0f && num < 1f)
			{
				DealSideSlashDamage(sideSlashLastAngle, num2);
			}
			sideSlashLastAngle = num2;
			if (stateExistTime > sideSlashTime)
			{
				state = SwordState.SideSlashAfter;
			}
			break;
		}
		case SwordState.SideSlashAfter:
			if (changedState)
			{
				sideSlashParticle.Stop();
				sideSlashShadowParticle.Stop();
			}
			if (stateExistTime > sideSlashAfterTime)
			{
				state = SwordState.BackToIdle;
			}
			break;
		case SwordState.CurveSlashAim:
			if (changedState)
			{
				smoothSpeed = Vector3.zero;
			}
			heightLerp = Mathf.MoveTowards(heightLerp, 1f, 1f / curveSmoothTime * Time.deltaTime);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, startPoint, ref smoothSpeed, curveSmoothTime);
			nowDir = Tool2D.RotateTowardsAroundZAxisSmooth(nowDir, nowDir = Tool2D.IgnoreZV2ToV1Normal(middlePoint, startPoint), rotateSpeed * Time.deltaTime, 5f);
			if (stateExistTime > curveSmoothTime * 2f)
			{
				state = SwordState.CurveSlash;
			}
			break;
		case SwordState.CurveSlash:
		{
			ref Vector3 reference4 = ref varMgr.RegV3(0);
			if (changedState)
			{
				nowDir = Tool2D.IgnoreZV2ToV1Normal(middlePoint, startPoint);
				nowDashSpeed = 0f;
				dashParticle.Clear();
				dashParticle.Play();
				dashShadowParticle.Clear();
				dashShadowParticle.Play();
				sr_Warning.enabled = true;
			}
			if (stateExistTime < curveDuration)
			{
				reference4 = base.transform.position;
				base.transform.position = GeneralTool.FreeBezierCurve(curvePosCurve.Evaluate(stateExistTime / curveDuration), startPoint, middlePoint, endPoint);
				if (!changedState)
				{
					nowDir = Tool2D.IgnoreZV2ToV1Normal(base.transform.position, reference4);
				}
			}
			else if (stateExistTime < curveDuration + curveExtraDuration)
			{
				if (nowDashSpeed == 0f)
				{
					nowDashSpeed = (base.transform.position - reference4).magnitude / Time.deltaTime;
					break;
				}
				reference4 = base.transform.position;
				base.transform.position += nowDir * nowDashSpeed * Time.deltaTime;
			}
			else
			{
				sr_Warning.enabled = false;
				dashAutoBack = true;
				state = SwordState.DashAfter;
			}
			break;
		}
		case SwordState.Hide:
			if (changedState)
			{
				sr_Sword.material.SetFloat(GameConstManaged.shaderTransparencyIndex, 0f);
				shadowLerp = 0f;
			}
			base.transform.position = idlePosition;
			nowDir = idleDir;
			break;
		case SwordState.Show:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			if (changedState)
			{
				SEMgr.Inst.boss55SummonSword.PlaySE();
				dashChargeParticle.Play();
				reference = idlePosition + Vector3.up * 2f;
			}
			base.transform.position = Vector3.Lerp(idlePosition, reference, EaseInOutQuad(Mathf.Clamp01(2f - stateExistTime * 2f / showTime)));
			nowDir = idleDir;
			shadowLerp = Mathf.Clamp01(stateExistTime * 2f / showTime);
			sr_Sword.material.SetFloat(GameConstManaged.shaderBlendIndex, Mathf.Clamp01(2f - stateExistTime * 2f / showTime));
			sr_Sword.material.SetFloat(GameConstManaged.shaderTransparencyIndex, Mathf.Clamp01(stateExistTime * 2f / showTime));
			if (stateExistTime > showTime)
			{
				state = SwordState.Idle;
				dashChargeParticle.Stop();
			}
			break;
		}
		case SwordState.Attack:
		case SwordState.FallGround:
			break;
		}
	}

	private float EaseInOutQuad(float t)
	{
		if (!(t < 0.5f))
		{
			return 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
		}
		return 2f * t * t;
	}

	private void SetWarning()
	{
		lr_Warning.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position) + Vector3.up * nowHeight);
		lr_Warning.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + nowDir * 50f) + Vector3.up * nowHeight);
		lr_WarningShadow.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow));
		lr_WarningShadow.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + nowDir * 50f, LayerCorrectType.Shadow));
		lr_FinalWarning.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position) + Vector3.up * nowHeight);
		lr_FinalWarning.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + nowDir * 50f) + Vector3.up * nowHeight);
	}

	private void SetSideSlashWarning()
	{
		for (int i = 0; i < lr_SlashWarning.positionCount; i++)
		{
			float num = (float)i / ((float)lr_SlashWarning.positionCount - 1f);
			Vector3 dir = Tool2D.GetDir(sideSlashStartDir, sideSlashAngle * num * sideSlashRotateDir);
			lr_SlashWarning.SetPosition(i, Tool2D.GetLayerPoint(sideSlashCenterPoint + dir * (sideSlashDistance + lr_SlashWarning.widthMultiplier / 2f), LayerCorrectType.GroundEffect));
		}
	}

	private void SetRotateSlashWarning()
	{
		for (int i = 0; i < lr_SlashWarning.positionCount; i++)
		{
			float num = (float)i / ((float)lr_SlashWarning.positionCount - 1f);
			Vector3 dir = Tool2D.GetDir(battleTargetDir, rotateSlashAngleSpeed * rotateSlashWarningPreviewTime * num);
			lr_SlashWarning.SetPosition(i, Tool2D.GetLayerPoint(rotateSlashCenterPoint + dir * (rotateSlashFromCenterDistance + lr_SlashWarning.widthMultiplier / 2f), LayerCorrectType.Shadow));
		}
	}

	private void DealSideSlashDamage(float startAngle, float endAngle)
	{
		Debug.DrawLine(sideSlashCenterPoint + nowDir * (sideSlashDistance + sideSlashWidth), sideSlashCenterPoint + nowDir * sideSlashDistance);
		UnitDotsSyncSystem.GetCollidersInRange(sideSlashCenterPoint, sideSlashDistance + sideSlashWidth, GameConst.Filter_MonsterAoeNoSpell, sideSlashHitTargets);
		for (int i = 0; i < sideSlashHitTargets.Count; i++)
		{
			Entity entity = sideSlashHitTargets[i].entity;
			if (!sideSlashDamagedEntities.Contains(entity))
			{
				Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(sideSlashHitTargets[i].entity).Position;
				Vector3 vector2 = Tool2D.IgnoreZPoint(vector - sideSlashCenterPoint);
				if (!(Mathf.Abs(vector2.magnitude - sideSlashDistance - sideSlashWidth * 0.5f) > sideSlashWidth * 0.5f) && TargetInSideSlashSweep(vector2.normalized, startAngle, endAngle))
				{
					DealSideSlashDamageToEntity(entity, vector);
				}
			}
		}
	}

	private bool TargetInSideSlashSweep(Vector3 targetDir, float startAngle, float endAngle)
	{
		float num = Tool2D.IgnoreZAngleWithSign(sideSlashStartDir, targetDir);
		float num2 = Mathf.Min(startAngle, endAngle);
		float num3 = Mathf.Max(startAngle, endAngle);
		if (num >= num2)
		{
			return num <= num3;
		}
		return false;
	}

	private void DealSideSlashDamageToEntity(Entity entity, Vector3 hitPoint)
	{
		switch (UnitDotsSyncSystem.GetLayer(entity))
		{
		case 131072u:
		{
			TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(Boss55.Inst.myPpt.myEntity);
			info2.damage = dashDamage * 10f;
			UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info2);
			sideSlashDamagedEntities.Add(entity);
			break;
		}
		case 512u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss55.Inst.myPpt.myEntity);
			info.damage = dashDamage;
			Vector3 normalized = (nowDir + Tool2D.IgnoreZV2ToV1Normal(hitPoint, sideSlashCenterPoint)).normalized;
			info.knockbackForce = normalized * knockBack;
			UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite14_BladeHit", Tool2D.GetLayerPoint(hitPoint), Quaternion.Euler(new Vector3(0f, 0f, Tool2D.IgnoreZAngle(Vector3.up, normalized) - 90f)), Vector3.one, 3f);
			SEMgr.Inst.boss55SwordHit.PlaySE();
			sideSlashDamagedEntities.Add(entity);
			break;
		}
		}
	}

	private void TeleportFade(bool fadeOut)
	{
		if (fadeOut)
		{
			shadowLerp = Mathf.Clamp01(2f - stateExistTime * 2f / fadeTime);
			sr_Sword.material.SetFloat(GameConstManaged.shaderBlendIndex, Mathf.Clamp01(stateExistTime * 2f / fadeTime));
			sr_Sword.material.SetFloat(GameConstManaged.shaderTransparencyIndex, Mathf.Clamp01(2f - stateExistTime * 2f / fadeTime));
		}
		else
		{
			shadowLerp = Mathf.Clamp01(stateExistTime * 2f / fadeTime);
			sr_Sword.material.SetFloat(GameConstManaged.shaderBlendIndex, Mathf.Clamp01(2f - stateExistTime * 2f / fadeTime));
			sr_Sword.material.SetFloat(GameConstManaged.shaderTransparencyIndex, Mathf.Clamp01(stateExistTime * 2f / fadeTime));
		}
	}

	private void LateUpdate()
	{
		if (state == SwordState.DashAim && stateExistTime < stopAimTime)
		{
			if (dashAim)
			{
				nowDir = Tool2D.RotateTowardsAroundZAxisSmooth(nowDir, Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPointIgnoreZ, base.transform.position), rotateSpeed, 5f);
			}
			SetWarning();
		}
		tsf_rotateRoot.transform.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(nowDir));
		tsf_shadowRotateRoot.transform.eulerAngles = tsf_rotateRoot.transform.eulerAngles;
		tsf_shadowRotateRoot.transform.localScale = tsf_rotateRoot.transform.localScale;
		sr_Shadow.color = new Color(1f, 1f, 1f, heightLerp * shadowLerp);
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform.position) + Vector3.up * nowHeight;
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		if ((state == SwordState.CurveSlash || state == SwordState.RotateSlash || (state == SwordState.Dash && !(nowDashSpeed < 0f - startDashSpeed))) && !Boss55.Inst.deadStayed)
		{
			switch (UnitDotsSyncSystem.GetLayer(other))
			{
			case 131072u:
			{
				TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(Boss55.Inst.myPpt.myEntity);
				info2.damage = dashDamage * 10f;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info2);
				break;
			}
			case 512u:
			case 2097152u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss55.Inst.myPpt.myEntity);
				info.damage = dashDamage;
				Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
				Vector3 normalized = (nowDir + Tool2D.IgnoreZV2ToV1Normal(vector, base.transform.position)).normalized;
				info.knockbackForce = normalized * knockBack;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite14_BladeHit", Tool2D.GetLayerPoint(vector), Quaternion.Euler(new Vector3(0f, 0f, Tool2D.IgnoreZAngle(Vector3.up, normalized) - 90f)), Vector3.one, 3f);
				SEMgr.Inst.boss55SwordHit.PlaySE();
				break;
			}
			}
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
