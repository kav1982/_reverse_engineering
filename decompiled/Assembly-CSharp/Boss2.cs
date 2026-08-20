using System.Collections;
using UnityEngine;

public class Boss2 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Breath,
		VenomPool,
		GetUp,
		GetDown,
		Vomit,
		VenomRain,
		VenomRainAfter,
		Die
	}

	public VariableFloat breathTime;

	public VariableFloat landPlayerRadius;

	[Header("VenomPool")]
	[Range(0f, 1f)]
	public float venomPoolChance;

	public Transform tsf_VenomPoolPreant;

	public Boss2_VenomPool pfb_VenomPool;

	public Boss2_VenomPool pfb_VenomPoolMobile;

	public Vector3 venomPoolOffset;

	public ParticleSystem ps_VenomFall;

	public float venomPoolTime;

	public float venomPoolDelay;

	[Range(0f, 1f)]
	[Header("Spell Bullet")]
	public float venomRainChance;

	public Transform tsf_BulletPoint;

	public VariableFloat venomRainTime;

	public VariableFloat venomRainBulletInterval;

	public float venomRainRadius;

	public float venomRainAfterTime;

	public float bulletForwardSpeed;

	public float bulletUpSpeed;

	public float bulletGravity;

	public float bulletDuration;

	[Header("Spit Ball")]
	[Range(0f, 1f)]
	public float spitBallChance;

	public float rollBallOffset;

	public float rollBallAngle;

	[Range(0f, 1f)]
	public float spitTwoBallHPRatio;

	[Header("Spell Rollball")]
	public bool isPlayerDerate;

	public float rollBallSpeed;

	public float rollBallDuration;

	public int rollBallDamage;

	public float rollBallDamageRatio;

	public float rollBallFollowRotateSpeed;

	[Header("影子控制")]
	public float venomRainShadowScale;

	public float maxShadowHeight;

	public Transform tsf_Motion;

	public Shadow thisShadow;

	private float originShadowScale;

	[Header("死亡爆绳子")]
	public int ropeExplosionCount;

	public float ropeExplotionInterval;

	public Transform tsf_ropeExplodeRoot;

	private MonsterState state;

	private float breathTimer;

	private float pourTimer;

	private Boss2_VenomPool venomPool;

	private float originalEmissionRate;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	private SpellInitialParameter sipRollBall = new SpellInitialParameter();

	private SpellSpawnParams ssp_Bullet;

	private SpellSpawnParams ssp_RollBall;

	private bool canEnterVenomRain;

	private bool isVenomRain;

	private float venomRainTimer;

	private float venomRainBulletIntervalTimer;

	private float venomRainAfterTimer;

	public override void SingleInitialCallback()
	{
		originalEmissionRate = ps_VenomFall.emission.rateOverTime.constantMax;
		PauseParticle();
		breathTime.RandomResult();
		venomRainTime.RandomResult();
		venomRainBulletInterval.RandomResult();
		tsf_VenomPoolPreant.SetParent(base.transform.parent);
		ssp_Bullet = UnitDotsSyncSystem.GetSpellPrototype(90011);
		ssp_Bullet.MovementComponentData.Speed = bulletForwardSpeed;
		ssp_Bullet.MovementComponentData.CurrentFallSpeed = 0f - bulletUpSpeed;
		ssp_Bullet.MovementComponentData.Gravity = 0f - bulletGravity;
		ssp_Bullet.ConfigComponentData.Duration = new AttributeValue(bulletDuration);
		ssp_Bullet.SetShooter(myPpt.myEntity, myPpt.myEntity);
		ssp_Bullet.ConfigComponentData.ColorType = SpellColorType.Venom;
		ssp_Bullet.ElementComponentData.VenomApplyCount = 2f;
		ssp_Bullet.ElementComponentData.VenomDuration = 4f;
		ssp_RollBall = UnitDotsSyncSystem.GetSpellPrototype(10021);
		ssp_RollBall.MovementComponentData.Speed = rollBallSpeed;
		ssp_RollBall.MovementComponentData.Type = SpellSpecialMovementType.ChaseEnemy;
		ssp_RollBall.MovementComponentData.ChaseRotateSpeed = rollBallFollowRotateSpeed;
		ssp_RollBall.ConfigComponentData.Duration = new AttributeValue(rollBallDuration);
		ssp_RollBall.ConfigComponentData.Damage = new AttributeValue((float)rollBallDamage * rollBallDamageRatio);
		ssp_RollBall.ConfigComponentData.CriticalChance = -999999f;
		ssp_RollBall.ConfigComponentData.ColorType = SpellColorType.Venom;
		ssp_RollBall.ElementComponentData.VenomApplyCount = 2f;
		ssp_RollBall.ElementComponentData.VenomDuration = 4f;
		ssp_RollBall.SetShooter(myPpt.myEntity, myPpt.myEntity);
		ssp_RollBall.SpellExtraSizeRatio = 0.6f;
		originShadowScale = thisShadow.shadowScale;
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (state == MonsterState.GetDown || state == MonsterState.GetUp || state == MonsterState.VenomRain || state == MonsterState.VenomRainAfter)
		{
			float num = Mathf.Clamp01(1f - tsf_Motion.localPosition.y / maxShadowHeight);
			if (isVenomRain)
			{
				num = Mathf.Lerp(1f, venomRainShadowScale, tsf_Motion.localPosition.y / maxShadowHeight);
			}
			thisShadow.SetScale(num * originShadowScale);
		}
		else
		{
			float num2 = Mathf.Clamp01(1f - tsf_Motion.localPosition.y / maxShadowHeight);
			thisShadow.SetScale(num2 * originShadowScale);
		}
		if (base.deadStayed)
		{
			return;
		}
		SetMove(Vector3.zero);
		switch (state)
		{
		case MonsterState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.Breath;
			}
			break;
		case MonsterState.Breath:
			breathTimer += Time.deltaTime;
			if (breathTimer >= breathTime.result)
			{
				breathTimer = 0f;
				breathTime.RandomResult();
				GetUp();
			}
			break;
		case MonsterState.VenomPool:
			pourTimer += Time.deltaTime;
			if (pourTimer >= venomPoolTime)
			{
				pourTimer = 0f;
				GetUp();
			}
			break;
		case MonsterState.GetUp:
			if (isVenomRain)
			{
				venomRainBulletIntervalTimer += Time.deltaTime;
				if (venomRainBulletIntervalTimer >= venomRainBulletInterval.result)
				{
					venomRainBulletIntervalTimer = 0f;
					venomRainBulletInterval.RandomResult();
					Vector3 vector2 = base.transform.position + new Vector3(0f, 0f, 0f - tsf_BulletPoint.position.y);
					ssp_Bullet.SpawnPosition = vector2;
					ssp_Bullet.MovementComponentData.Direction = Tool2D.GetDir();
					ShootSpell(ssp_Bullet);
				}
				venomRainTimer += Time.deltaTime;
				if (venomRainTimer >= venomRainTime.result)
				{
					venomRainTimer = 0f;
					venomRainTime.RandomResult();
					state = MonsterState.VenomRainAfter;
				}
			}
			break;
		case MonsterState.VenomRain:
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				if (ToTargetDistanceSqr() > 0.040000003f)
				{
					base.transform.position += ToTargetDir() * base.MoveSpeed * Time.deltaTime;
					SyncDotsPosition();
				}
				else
				{
					SetMove(Vector3.zero, isFlip: false);
				}
			}
			venomRainBulletIntervalTimer += Time.deltaTime;
			if (venomRainBulletIntervalTimer >= venomRainBulletInterval.result)
			{
				venomRainBulletIntervalTimer = 0f;
				venomRainBulletInterval.RandomResult();
				float num3 = tsf_BulletPoint.position.y - base.transform.position.y;
				Vector3 vector = base.transform.position + new Vector3(0f, 0f, 0f - num3);
				ssp_Bullet.SpawnPosition = vector;
				ssp_Bullet.MovementComponentData.Direction = Tool2D.GetDir();
				ShootSpell(ssp_Bullet);
			}
			venomRainTimer += Time.deltaTime;
			if (venomRainTimer >= venomRainTime.result)
			{
				venomRainTimer = 0f;
				venomRainTime.RandomResult();
				state = MonsterState.VenomRainAfter;
			}
			break;
		case MonsterState.VenomRainAfter:
			venomRainAfterTimer += Time.deltaTime;
			if (venomRainAfterTimer >= venomRainAfterTime)
			{
				venomRainAfterTimer = 0f;
				state = MonsterState.GetDown;
				base.Anima.SetTrigger("GetDown");
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case MonsterState.GetDown:
		case MonsterState.Vomit:
		case MonsterState.Die:
			break;
		}
	}

	private void GetUp()
	{
		base.Anima.SetTrigger("GetUp");
		state = MonsterState.GetUp;
		if (venomPool != null)
		{
			venomPool.StopAndMinify();
			venomPool = null;
		}
		PauseParticle();
	}

	private void StartParticle()
	{
		ParticleSystem.EmissionModule emission = ps_VenomFall.emission;
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(originalEmissionRate);
	}

	private void PauseParticle()
	{
		ParticleSystem.EmissionModule emission = ps_VenomFall.emission;
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
	}

	private IEnumerator CreateVenomPool()
	{
		yield return new WaitForSeconds(venomPoolDelay);
		if (tsf_VenomPoolPreant != null && state == MonsterState.VenomPool)
		{
			if (GameMgr.IsMobile_Static)
			{
				venomPool = Object.Instantiate(pfb_VenomPoolMobile, Tool2D.IgnoreZPoint(base.transform.position + venomPoolOffset), Tool2D.GetRotation(), tsf_VenomPoolPreant);
			}
			else
			{
				venomPool = Object.Instantiate(pfb_VenomPool, Tool2D.IgnoreZPoint(base.transform.position + venomPoolOffset), Tool2D.GetRotation(), tsf_VenomPoolPreant);
			}
		}
	}

	protected override void BossDeadStay()
	{
		if (venomPool != null)
		{
			venomPool.StopAndMinify();
			venomPool = null;
		}
		PauseParticle();
		base.deadStayed = true;
		state = MonsterState.Die;
		base.Anima.SetTrigger("Die");
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (animaName)
		{
		case "GetUpSE":
			SEMgr.Inst.boss2_GetUp.PlaySE();
			break;
		case "GetDownSE":
			SEMgr.Inst.boss2_GetDown.PlaySE();
			break;
		case "Land":
			SEMgr.Inst.monster15Land.PlaySE();
			break;
		case "RopeExplode":
		{
			for (int i = 0; i < ropeExplosionCount; i++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Bone", base.transform.position - new Vector3(0f, 0f, tsf_ropeExplodeRoot.position.y - base.transform.position.y + (float)i * ropeExplotionInterval), 2f);
			}
			SEMgr.Inst.boss2_Fall.PlaySE();
			break;
		}
		case "GetUpFinish":
			if (isVenomRain)
			{
				state = MonsterState.VenomRain;
				break;
			}
			base.transform.position = Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPoint, landPlayerRadius);
			SyncDotsPosition();
			base.Anima.SetTrigger("GetDown");
			break;
		case "GetDownFinish":
			if (Random.value <= venomPoolChance)
			{
				state = MonsterState.VenomPool;
				base.Anima.SetTrigger("Pour");
				StartParticle();
				StartCoroutine(CreateVenomPool());
				if (Random.value <= venomRainChance)
				{
					canEnterVenomRain = true;
				}
				else
				{
					canEnterVenomRain = false;
				}
			}
			else
			{
				state = MonsterState.Breath;
				base.Anima.SetTrigger("Breath");
			}
			break;
		case "Vomit":
			GetNearestTargetPlayerFirst();
			if (base.CurrentHPRatio <= spitTwoBallHPRatio)
			{
				Vector3 vector;
				Vector3 vector2;
				Vector3 vector3;
				Vector3 vector4;
				if (base.HaveTarget)
				{
					vector = ToTargetDir(0f - rollBallAngle);
					vector2 = ToTargetDir(rollBallAngle);
					vector3 = base.transform.position + vector;
					vector4 = base.transform.position + vector2;
				}
				else
				{
					vector = Tool2D.GetDir();
					vector2 = Tool2D.GetDir();
					vector3 = base.transform.position + vector * rollBallOffset;
					vector4 = base.transform.position + vector2 * rollBallOffset;
				}
				ssp_RollBall.MovementComponentData.Direction = vector;
				ssp_RollBall.MovementComponentData.ChaseTarget = LevelMgr.Inst.CurrentRoomCtrller.GetMinimalAngleTargetableEntity(base.transform.position, vector);
				ssp_RollBall.SpawnPosition = vector3;
				ShootSpell(ssp_RollBall);
				ssp_RollBall.MovementComponentData.Direction = vector2;
				ssp_RollBall.MovementComponentData.ChaseTarget = LevelMgr.Inst.CurrentRoomCtrller.GetMinimalAngleTargetableEntity(base.transform.position, vector2);
				ssp_RollBall.SpawnPosition = vector4;
				ShootSpell(ssp_RollBall);
			}
			else
			{
				Vector3 vector5 = ((!base.HaveTarget) ? Tool2D.GetDir() : ToTargetDir((Random.Range(0, 2) == 0) ? (0f - rollBallAngle) : rollBallAngle));
				Vector3 vector6 = base.transform.position + vector5 * rollBallOffset;
				ssp_RollBall.MovementComponentData.Direction = vector5;
				ssp_RollBall.MovementComponentData.ChaseTarget = LevelMgr.Inst.CurrentRoomCtrller.GetMinimalAngleTargetableEntity(base.transform.position, vector5);
				ssp_RollBall.SpawnPosition = vector6;
				ShootSpell(ssp_RollBall);
			}
			break;
		case "VomitFinish":
			GetUp();
			break;
		case "TouchFalse":
		{
			base.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = false;
			SetComponentData(componentData);
			break;
		}
		case "TouchTrue":
		{
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			SetComponentData(componentData);
			break;
		}
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (state == MonsterState.Breath)
		{
			breathTimer = 0f;
			breathTime.RandomResult();
			if (Random.value <= spitBallChance)
			{
				state = MonsterState.Vomit;
				base.Anima.SetTrigger("Vomit");
			}
			else
			{
				isVenomRain = false;
				GetUp();
			}
		}
		else if (state == MonsterState.VenomPool && pourTimer > venomPoolDelay && canEnterVenomRain)
		{
			pourTimer = 0f;
			isVenomRain = true;
			GetUp();
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		Object.Destroy(tsf_VenomPoolPreant.gameObject);
	}
}
