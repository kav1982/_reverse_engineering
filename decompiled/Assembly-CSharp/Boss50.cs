using System;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

public class Boss50 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Move,
		ChargeBefore,
		Charge,
		ChargeAfter,
		Dead
	}

	[Serializable]
	public class Boss50Turret
	{
		public enum TurretState
		{
			Stop,
			Mine,
			Reloading,
			Shooting,
			Aim,
			StopAim,
			Shoot,
			MissileReloading,
			MissileAim,
			MissileStopAim,
			MissileShoot,
			MissileWait
		}

		[Header("表现")]
		public Transform tsf_Turret;

		public Transform tsf_Muzzle;

		public Transform tsf_TurretCenter;

		public float turretRotateSpeed;

		public Vector3 aimDir;

		[Header("状态")]
		public TurretState _state;

		public StateVariableMgr varMgr = new StateVariableMgr();

		private bool stateQuit;

		private bool changedState;

		private float stateExistTime;

		[Header("技能")]
		public float cannonChance;

		public float gunChance;

		public float missileChance;

		public float mineChance;

		public VariableFloat reloadTime;

		[Header("火炮")]
		public ParticleSystem cannonParticle;

		public float aimingTime;

		public float stopAimingTime;

		public float shootTime;

		public float warningRadius;

		public SpriteRenderer SR_CannonAim;

		private WarningArea warningArea;

		[Header("炮射导弹")]
		public ParticleSystem PS_AimEffect;

		public SpriteRenderer SR_AimEffect;

		public SpriteRenderer SR_AimEffectScaler;

		public Boss50Cannon cannonMissile;

		public float missileAimingTime;

		public float missileStopAimingTime;

		public float missileShootTime;

		[Header("同轴机枪")]
		public VariableFloat machineGunShootTime;

		public Transform tsf_MachineGun1;

		public Transform tsf_MachineGun2;

		public ParticleSystem ps_MachineGun1;

		public ParticleSystem ps_MachineGun2;

		public float machineGunScatterAngle;

		public float machineGunShootInterval;

		public float machineGunHeight;

		public float bulletSpeed;

		public float bulletLifeTime;

		public float bulletDamage;

		public bool attacking;

		public Boss50 body;

		private SpellSpawnParams ssp;

		private UnitSpellModifier usm;

		private TurretState lastState;

		public TurretState state
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

		public void Initialize()
		{
			ssp = UnitDotsSyncSystem.GetSpellPrototype(90461);
			usm = UnitBase.GetSSPModifier(in ssp);
			usm.Duration = bulletLifeTime;
			usm.Speed = bulletSpeed;
			usm.Damage = bulletDamage * GameConstManaged.endlessMonsterDamageRatio;
			usm.Shooter = Inst.myPpt.myEntity;
			usm.ApplyToSSP(ref ssp);
			ssp.DisableResize = true;
			state = TurretState.Stop;
			warningArea = null;
			SR_CannonAim.enabled = false;
			SR_AimEffect.enabled = false;
			SR_AimEffectScaler.enabled = false;
		}

		public void OnDead()
		{
			SR_CannonAim.enabled = false;
			SR_AimEffect.enabled = false;
			SR_AimEffectScaler.enabled = false;
			if (warningArea != null && warningArea.gameObject.activeSelf)
			{
				ObjPoolMgr.Inst.RecycleGO(warningArea.gameObject);
			}
		}

		public void MachineGunShoot(bool shootRight)
		{
			if (shootRight)
			{
				ps_MachineGun1.Play();
			}
			else
			{
				ps_MachineGun2.Play();
			}
			Vector3 v = (shootRight ? tsf_MachineGun1.position : tsf_MachineGun2.position);
			usm = UnitBase.GetSSPModifier(in ssp);
			usm.SpawnPosition = body.transform.position + Tool2D.IgnoreZV2ToV1(v, tsf_TurretCenter.position) - Vector3.forward * machineGunHeight;
			usm.Direction = Tool2D.GetDir(aimDir, UnityEngine.Random.Range(-0.5f, 0.5f) * machineGunScatterAngle);
			usm.ApplyToSSP(ref ssp);
			UnitDotsSyncSystem.ShootSpell(ssp);
			SEMgr.Inst.monster306_Shoot.PlaySE();
		}

		public void ChooseSkill()
		{
			bool flag = false;
			TurretState turretState = TurretState.Stop;
			bool isSecondStage = body.isSecondStage;
			while (!flag)
			{
				flag = true;
				int weightRandom = GeneralTool.GetWeightRandom(cannonChance, gunChance, 0f);
				if (isSecondStage)
				{
					weightRandom = GeneralTool.GetWeightRandom(cannonChance, gunChance, missileChance);
				}
				switch (weightRandom)
				{
				case 0:
					if (lastState != state)
					{
						turretState = TurretState.Aim;
					}
					else
					{
						flag = false;
					}
					break;
				case 1:
					turretState = TurretState.Shooting;
					break;
				case 2:
					turretState = TurretState.MissileReloading;
					break;
				}
				if (isSecondStage && turretState == lastState)
				{
					flag = false;
				}
			}
			state = turretState;
			lastState = state;
		}

		public void Update()
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
			TurretState turretState = state;
			if ((turretState == TurretState.Reloading || turretState == TurretState.Shooting || turretState == TurretState.Aim || turretState == TurretState.Stop || turretState == TurretState.MissileReloading || turretState == TurretState.MissileAim || turretState == TurretState.MissileWait) && body.HaveTarget)
			{
				aimDir = Tool2D.RotateTowardsAroundZAxis(aimDir, body.ToTargetDir(), turretRotateSpeed * Time.deltaTime).normalized;
			}
			tsf_Turret.localEulerAngles = Tool2D.GetEulerAngleByDir(aimDir) + new Vector3(0f, 0f, 90f);
			switch (state)
			{
			case TurretState.Stop:
				if (attacking)
				{
					state = TurretState.Reloading;
				}
				break;
			case TurretState.Reloading:
				if (changedState)
				{
					reloadTime.RandomResult();
				}
				if (!attacking)
				{
					state = TurretState.Stop;
				}
				else if (stateExistTime > reloadTime.result)
				{
					ChooseSkill();
				}
				break;
			case TurretState.Shooting:
			{
				ref float reference3 = ref varMgr.RegFloat(0);
				ref bool reference4 = ref varMgr.RegBool(0);
				if (changedState)
				{
					machineGunShootTime.RandomResult();
				}
				reference3 += Time.deltaTime;
				if (reference3 > machineGunShootInterval)
				{
					reference3 = 0f;
					reference4 = !reference4;
					MachineGunShoot(reference4);
				}
				if (stateExistTime > machineGunShootTime.result)
				{
					state = TurretState.Aim;
					if (body.isSecondStage && GeneralTool.ChanceResult(0.5f))
					{
						state = TurretState.MissileReloading;
					}
				}
				break;
			}
			case TurretState.Aim:
			{
				ref float reference2 = ref varMgr.RegFloat(0);
				if (changedState)
				{
					reference2 = 7f;
					if (body.HaveTarget)
					{
						reference2 = Mathf.Max(5f, body.ToTargetDistance());
					}
					warningArea = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle", body.transform.position + aimDir * reference2).GetComponent<WarningArea>();
					warningArea.Initialize(warningRadius, 10f, zoomDirect: false);
					SEMgr.Inst.boss50CannonReload.PlaySE();
					SR_CannonAim.enabled = true;
				}
				warningArea.transform.position = body.transform.position + reference2 * aimDir;
				warningArea.tsf_Fill.localScale = Vector3.one * stateExistTime / (aimingTime + stopAimingTime) * warningRadius * 2f;
				if (body.HaveTarget)
				{
					reference2 = Mathf.Max(5f, body.ToTargetDistance());
				}
				if (stateExistTime > aimingTime)
				{
					state = TurretState.StopAim;
				}
				break;
			}
			case TurretState.StopAim:
				_ = changedState;
				aimDir = (warningArea.transform.position - body.transform.position).normalized;
				warningArea.tsf_Fill.localScale = Vector3.one * (stateExistTime + aimingTime) / (aimingTime + stopAimingTime) * warningRadius * 2f;
				if (stateExistTime > stopAimingTime)
				{
					state = TurretState.Shoot;
				}
				break;
			case TurretState.Shoot:
			{
				ref bool reference5 = ref varMgr.RegBool(0);
				if (changedState)
				{
					SEMgr.Inst.boss50CannonShoot.PlaySE();
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss50Cannon", body.transform.position + Tool2D.IgnoreZV2ToV1(tsf_Muzzle.position, tsf_TurretCenter.position)).GetComponent<Boss50Cannon>().Initialize(warningArea.transform.position, tsf_TurretCenter.position.y - body.transform.position.y);
					ObjPoolMgr.Inst.RecycleGO(warningArea.gameObject);
					SR_CannonAim.enabled = false;
				}
				if (stateExistTime > 0.1f && !reference5)
				{
					reference5 = true;
					cannonParticle.Play();
				}
				if (stateExistTime > shootTime)
				{
					state = TurretState.Reloading;
				}
				break;
			}
			case TurretState.MissileReloading:
				if (changedState)
				{
					SEMgr.Inst.boss50CannonReload.PlaySE();
				}
				if (body.station.state == Boss50WeaponStation.WeaponState.Stop || body.station.state == Boss50WeaponStation.WeaponState.Reloading)
				{
					body.station.state = Boss50WeaponStation.WeaponState.CannonMissileWait;
				}
				if (body.station.state == Boss50WeaponStation.WeaponState.CannonMissileWait)
				{
					state = TurretState.MissileAim;
				}
				break;
			case TurretState.MissileAim:
				if (changedState)
				{
					body.AS_MissileChasing.Play();
					PS_AimEffect.Play();
					SR_AimEffect.enabled = true;
					SR_AimEffectScaler.enabled = true;
				}
				if (stateExistTime > missileAimingTime)
				{
					state = TurretState.MissileStopAim;
				}
				break;
			case TurretState.MissileStopAim:
				_ = changedState;
				if (stateExistTime > missileStopAimingTime)
				{
					state = TurretState.MissileShoot;
				}
				break;
			case TurretState.MissileShoot:
			{
				ref bool reference = ref varMgr.RegBool(0);
				if (changedState)
				{
					SEMgr.Inst.boss50CannonShoot.PlaySE();
					cannonMissile = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss50Missile", body.transform.position + Tool2D.IgnoreZV2ToV1(tsf_Muzzle.position, tsf_TurretCenter.position)).GetComponent<Boss50Cannon>();
					Entity targetEntity = Entity.Null;
					if (body.HaveTarget)
					{
						targetEntity = body.targetEntity;
					}
					cannonMissile.InitializeMissile(aimDir, tsf_TurretCenter.position.y - body.transform.position.y, targetEntity);
				}
				if (stateExistTime > 0.1f && !reference)
				{
					reference = true;
					cannonParticle.Play();
				}
				if (stateExistTime > missileShootTime)
				{
					state = TurretState.MissileWait;
				}
				break;
			}
			case TurretState.MissileWait:
				if (cannonMissile.exploded)
				{
					SR_AimEffect.enabled = false;
					SR_AimEffectScaler.enabled = false;
					PS_AimEffect.Stop();
					body.AS_MissileChasing.Stop();
					state = TurretState.Reloading;
					body.station.state = Boss50WeaponStation.WeaponState.Reloading;
					body.station.ForceReload();
				}
				break;
			case TurretState.Mine:
				break;
			}
		}

		public void LateUpdate()
		{
			if (SR_AimEffect.enabled)
			{
				if (body.HaveTarget)
				{
					Vector3 rootPoint = ((body.targetEntity == PlayerMgr.Inst.PlayerEtt) ? PlayerMgr.Inst.PlayerPoint : body.TargetPoint);
					SR_AimEffect.transform.position = Tool2D.GetLayerPoint(rootPoint) + new Vector3(0f, 0.6f, -0.01f);
				}
				else
				{
					SR_AimEffect.enabled = false;
					SR_AimEffectScaler.enabled = false;
				}
				float x = SR_AimEffectScaler.transform.localScale.x;
				x -= Time.deltaTime;
				if (x < 1f)
				{
					x = 1.4f;
				}
				SR_AimEffectScaler.transform.localScale = Vector3.one * x;
			}
			if (SR_CannonAim.enabled)
			{
				if (warningArea != null)
				{
					SR_CannonAim.transform.position = Tool2D.GetLayerPoint(warningArea.transform.position, LayerCorrectType.GroundEffect);
				}
				else
				{
					SR_CannonAim.enabled = false;
				}
			}
		}
	}

	[Serializable]
	public class Boss50WeaponStation
	{
		public enum WeaponState
		{
			Stop,
			Reloading,
			RapidShootAim,
			RapidShoot,
			StrafeShoot,
			HiveMissileShoot,
			Mine,
			CannonMissileWait
		}

		[Header("状态")]
		public WeaponState _state;

		public StateVariableMgr varMgr = new StateVariableMgr();

		private bool stateQuit;

		private bool changedState;

		private float stateExistTime;

		[Header("表现")]
		public Transform tsf_MachineGun;

		public Transform tsf_RotateCenter;

		public Transform tsf_Muzzle;

		public ParticleSystem ps_MachineGun;

		public Transform tsf_MissileHive1;

		public Transform tsf_MissileHive2;

		public float turretRotateSpeed;

		public Vector3 aimDir;

		[Header("攻击间隔和技能选择")]
		public VariableFloat ReloadTime;

		public float strafeChance;

		public float rapidChance;

		public float missileChance;

		public float mineChance;

		[Header("子弹")]
		public float bulletSpeed;

		public float bulletLifeTime;

		public float bulletDamage;

		[Header("扫射机枪")]
		public float strafeTime;

		public float strafeAngle;

		public float starfeRotateSpeed;

		public float strafeInterval;

		public float machineGunHeight;

		[Header("连射机枪")]
		public VariableFloat rapidShootCount;

		public float rapidShootBulletSpeed;

		public float rapidShootBulletSpeedFix;

		public float rapidShootInterval;

		public float rapidShootBulletInterval;

		public int rapidShootBulletCount;

		public float rapidShootAngleRange;

		public float rapidShootMinAngle;

		private int rapidShootCounter;

		private float lastRapidShootAngle;

		[Header("导弹巢")]
		public float missileShootInterval;

		public float missileDuration;

		public float missileHight;

		public VariableFloat missileShootCount;

		public VariableFloat missileScatterRadius;

		[Header("地雷")]
		public Transform tsf_MinePlacer;

		public int minePlaceCount;

		public float minePlaceInterval;

		public VariableFloat minePlaceRadius;

		public bool attacking;

		public Boss50 body;

		private SpellSpawnParams ssp;

		private UnitSpellModifier usm;

		private WeaponState lastState;

		public WeaponState state
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

		public void Initialize()
		{
			ssp = UnitDotsSyncSystem.GetSpellPrototype(90461);
			usm = UnitBase.GetSSPModifier(in ssp);
			usm.Duration = bulletLifeTime;
			usm.Speed = bulletSpeed;
			usm.Damage = bulletDamage * GameConstManaged.endlessMonsterDamageRatio;
			usm.Shooter = Inst.myPpt.myEntity;
			usm.ApplyToSSP(ref ssp);
			ssp.DisableResize = true;
			state = WeaponState.Stop;
		}

		public Vector3 ToTargetDir()
		{
			return body.TargetPointIgnoreZ - body.transform.position + (tsf_MachineGun.position - tsf_RotateCenter.position);
		}

		public void MachineGunShoot()
		{
			SEMgr.Inst.boss50StrafeShoot.PlaySE();
			ps_MachineGun.Play();
			Vector3 position = tsf_Muzzle.position;
			usm = UnitBase.GetSSPModifier(in ssp);
			usm.SpawnPosition = body.transform.position + Tool2D.IgnoreZV2ToV1(position, tsf_RotateCenter.position) - Vector3.forward * machineGunHeight;
			usm.Direction = aimDir;
			usm.Speed = bulletSpeed;
			usm.ApplyToSSP(ref ssp);
			UnitDotsSyncSystem.ShootSpell(ssp);
		}

		public void MachineGunRapidShoot()
		{
			SEMgr.Inst.boss50RapidShoot.PlaySE();
			ps_MachineGun.Play();
			Vector3 position = tsf_Muzzle.position;
			usm = UnitBase.GetSSPModifier(in ssp);
			usm.SpawnPosition = body.transform.position + Tool2D.IgnoreZV2ToV1(position, tsf_RotateCenter.position) - Vector3.forward * machineGunHeight;
			usm.Direction = aimDir;
			for (int i = 0; i < rapidShootBulletCount; i++)
			{
				usm.Speed = rapidShootBulletSpeed + rapidShootBulletSpeedFix * (float)i;
				usm.ApplyToSSP(ref ssp);
				UnitDotsSyncSystem.ShootSpell(ssp);
			}
		}

		public void ShootMissile(bool shootFromLeft)
		{
			Vector3 vector = (shootFromLeft ? tsf_MissileHive1.position : tsf_MissileHive2.position);
			SEMgr.Inst.boss50HiveMissile.PlaySE();
			Vector3 startPoint = body.transform.position + body.turret.aimDir * 7f;
			if (body.HaveTarget)
			{
				Vector3 vector2 = ((!(body.targetEntity == PlayerMgr.Inst.PlayerEtt)) ? ((Vector3)body.GetComponentData<UnitBase_Dots>(body.targetEntity).currentMotion) : PlayerMgr.Inst.PlayerCtrller.CurrentMotion);
				startPoint = vector2 * missileDuration * 0.5f + body.TargetPoint;
			}
			startPoint += Tool2D.GetDir() * missileScatterRadius.RandomResult();
			startPoint = Tool2D.GetNavMeshPointIngoreZ(startPoint);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss50HiveMissile", body.transform.position + vector - tsf_RotateCenter.position + Vector3.back * missileHight).GetComponent<Boss50HiveMissile>().Initialize(missileDuration, body.turret.aimDir, startPoint);
		}

		public void LaunchMine()
		{
			SEMgr.Inst.boss50Mine.PlaySE();
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss50_Mine", body.transform.position + Tool2D.IgnoreZV2ToV1(tsf_MinePlacer.position, tsf_RotateCenter.position) + Vector3.back * missileHight).GetComponent<Boss50Mine>().Initialize(body.transform.position + minePlaceRadius.RandomResult() * Tool2D.GetDir());
		}

		public void ForceReload()
		{
			stateExistTime = ReloadTime.RandomResult() / 2f;
		}

		public void ChooseSkill()
		{
			bool flag = false;
			WeaponState weaponState = WeaponState.Stop;
			bool isSecondStage = body.isSecondStage;
			while (!flag)
			{
				flag = true;
				int weightRandom = GeneralTool.GetWeightRandom(rapidChance, strafeChance, 0f, mineChance);
				if (isSecondStage)
				{
					weightRandom = GeneralTool.GetWeightRandom(rapidChance, strafeChance, missileChance, mineChance);
				}
				switch (weightRandom)
				{
				case 0:
					weaponState = WeaponState.RapidShootAim;
					break;
				case 1:
					weaponState = WeaponState.StrafeShoot;
					break;
				case 2:
					weaponState = WeaponState.HiveMissileShoot;
					break;
				case 3:
					weaponState = WeaponState.Mine;
					break;
				}
				if (lastState == weaponState)
				{
					flag = false;
				}
			}
			state = weaponState;
			lastState = state;
		}

		public void RotateTowardsTarget()
		{
			if (body.HaveTarget)
			{
				aimDir = Tool2D.RotateTowardsAroundZAxis(aimDir, ToTargetDir(), turretRotateSpeed * Time.deltaTime).normalized;
			}
		}

		public void Update()
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
			tsf_MachineGun.eulerAngles = Tool2D.GetEulerAngleByDir(aimDir) + new Vector3(0f, 0f, 90f);
			switch (state)
			{
			case WeaponState.Stop:
				if (attacking)
				{
					state = WeaponState.Reloading;
				}
				RotateTowardsTarget();
				break;
			case WeaponState.Reloading:
				if (changedState)
				{
					ReloadTime.RandomResult();
				}
				RotateTowardsTarget();
				if (!attacking)
				{
					state = WeaponState.Stop;
				}
				else if (stateExistTime > ReloadTime.result)
				{
					ChooseSkill();
				}
				break;
			case WeaponState.Mine:
			{
				ref float reference10 = ref varMgr.RegFloat(0);
				ref int reference11 = ref varMgr.RegInt(0);
				reference10 += Time.deltaTime;
				if (reference10 > minePlaceInterval)
				{
					reference10 -= minePlaceInterval;
					LaunchMine();
					reference11++;
					if (reference11 > minePlaceCount)
					{
						state = WeaponState.Reloading;
					}
				}
				break;
			}
			case WeaponState.StrafeShoot:
			{
				ref Vector3 reference6 = ref varMgr.RegV3(0);
				ref float reference7 = ref varMgr.RegFloat(0);
				ref float reference8 = ref varMgr.RegFloat(1);
				ref float reference9 = ref varMgr.RegFloat(2);
				if (changedState)
				{
					reference6 = aimDir;
					reference8 = GeneralTool.HalfChanceNPOne();
				}
				reference7 += Time.deltaTime * starfeRotateSpeed * reference8;
				if (Mathf.Abs(reference7) > strafeAngle / 2f)
				{
					reference8 *= -1f;
				}
				if (body.HaveTarget)
				{
					reference6 = Tool2D.RotateTowardsAroundZAxis(reference6, ToTargetDir(), turretRotateSpeed * Time.deltaTime).normalized;
				}
				aimDir = Tool2D.GetDir(reference6, reference7);
				reference9 += Time.deltaTime;
				if (reference9 > strafeInterval)
				{
					reference9 = 0f;
					MachineGunShoot();
				}
				if (stateExistTime > strafeTime)
				{
					state = WeaponState.Reloading;
				}
				break;
			}
			case WeaponState.RapidShootAim:
			{
				ref float reference4 = ref varMgr.RegFloat(0);
				ref Vector3 reference5 = ref varMgr.RegV3(0);
				if (changedState)
				{
					if (rapidShootCounter == 0)
					{
						rapidShootCount.RandomResult();
					}
					rapidShootCounter++;
					reference5 = aimDir;
					reference4 = (UnityEngine.Random.value - 0.5f) * rapidShootAngleRange;
					float f = reference4 - lastRapidShootAngle;
					if (Mathf.Abs(f) < rapidShootMinAngle)
					{
						if (Mathf.Abs(lastRapidShootAngle + Mathf.Sign(f) * rapidShootMinAngle) < rapidShootAngleRange / 2f)
						{
							reference4 = lastRapidShootAngle + Mathf.Sign(f) * rapidShootMinAngle;
						}
						else
						{
							reference4 = lastRapidShootAngle - Mathf.Sign(f) * rapidShootMinAngle;
						}
					}
					lastRapidShootAngle = reference4;
				}
				if (body.HaveTarget)
				{
					Vector3 vector = ((!(body.targetEntity == PlayerMgr.Inst.PlayerEtt)) ? ((Vector3)body.GetComponentData<UnitBase_Dots>(body.targetEntity).currentMotion) : PlayerMgr.Inst.PlayerCtrller.CurrentMotion);
					float a = Tool2D.IgnoreZDistance(body.TargetPoint, body.transform.position) / (rapidShootBulletSpeed + rapidShootBulletSpeedFix * (float)rapidShootBulletCount) * 0.8f;
					a = Mathf.Max(a, 0f);
					Vector3 vector2 = vector * a + body.TargetPoint;
					reference5 = Tool2D.RotateTowardsAroundZAxis(reference5, (vector2 - body.transform.position).normalized, turretRotateSpeed * Time.deltaTime).normalized;
				}
				aimDir = Tool2D.GetDir(reference5, reference4 * stateExistTime / rapidShootInterval);
				if (stateExistTime > rapidShootInterval)
				{
					state = WeaponState.RapidShoot;
				}
				break;
			}
			case WeaponState.RapidShoot:
			{
				ref int reference12 = ref varMgr.RegInt(0);
				ref float reference13 = ref varMgr.RegFloat(0);
				if (changedState)
				{
					MachineGunRapidShoot();
				}
				if (stateExistTime > 0.4f)
				{
					if ((float)rapidShootCounter > rapidShootCount.result)
					{
						rapidShootCounter = 0;
						state = WeaponState.Reloading;
					}
					else
					{
						state = WeaponState.RapidShootAim;
					}
					break;
				}
				reference13 += Time.deltaTime;
				if (reference13 > rapidShootBulletInterval && reference12 < rapidShootBulletCount - 1)
				{
					ps_MachineGun.Play();
					reference12++;
					reference13 = 0f;
				}
				break;
			}
			case WeaponState.HiveMissileShoot:
			{
				ref float reference = ref varMgr.RegFloat(0);
				ref int reference2 = ref varMgr.RegInt(0);
				ref bool reference3 = ref varMgr.RegBool(0);
				if (changedState)
				{
					missileShootCount.RandomResult();
					reference3 = GeneralTool.ChanceResult(0.5f);
				}
				reference += Time.deltaTime;
				if (reference > missileShootInterval)
				{
					reference = 0f;
					reference2++;
					if ((float)reference2 > missileShootCount.result)
					{
						state = WeaponState.Reloading;
						break;
					}
					reference3 = !reference3;
					ShootMissile(reference3);
				}
				break;
			}
			case WeaponState.CannonMissileWait:
				RotateTowardsTarget();
				break;
			}
		}
	}

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("表现")]
	public Transform tsf_Bottom;

	public Transform tsf_Shadow;

	[Header("行动")]
	public float rotateSpeed;

	public VariableFloat idleTime;

	public VariableFloat movetime;

	[Header("炮塔")]
	public Boss50Turret turret;

	[Header("机枪")]
	public Boss50WeaponStation station;

	[Header("创人")]
	public VariableInt ChargeBeforeCount;

	private int chargeBeforeCounter;

	public float chargeRotateSpeed;

	public float chargeSpeed;

	public float chargeBeforeTime;

	public float chargeTime;

	public float chargeAfterTime;

	public Boss50AttackZone attackZone;

	public LineRenderer warningLine;

	public ParticleSystem chargeParticle;

	[Header("二阶段")]
	public bool secondStageOverride;

	[Header("伤害传递")]
	private Boss50Collider boss50Collider;

	[Header("死亡")]
	public float shakeFrequency;

	public float shakeAmplitude;

	[Header("音效")]
	public AudioSource AS_Move;

	public AudioSource AS_MissileChasing;

	public static Boss50 Inst;

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

	public Vector3 moveDir { get; protected set; }

	public bool isSecondStage
	{
		get
		{
			UnitConfig unitCfg = GetComponentData<UnitProperty_Dots>(myPpt.myEntity).unitCfg;
			if (!secondStageOverride)
			{
				return unitCfg.currentHP / unitCfg.maxHP < 0.5f;
			}
			return true;
		}
	}

	public Entity thisEntity { get; set; }

	public override void SingleInitialCallback()
	{
		warningLine.positionCount = 10;
	}

	public override void Frame1InitialCallback()
	{
		Boss50Collider boss50Collider = (this.boss50Collider = ObjPoolMgr.Inst.GetGO("Prefabs/Units/505021", base.transform.position).GetComponent<Boss50Collider>());
		boss50Collider.Init(this, GetComponentData<UnitProperty_Dots>().unitCfg.maxHP);
		attackZone = boss50Collider.attackZone;
	}

	public unsafe override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
		CollisionFilter collisionFilter = componentData.ColliderPtr->GetCollisionFilter();
		collisionFilter.BelongsTo = 2048u;
		collisionFilter.CollidesWith = 65536u;
		componentData.ColliderPtr->SetCollisionFilter(collisionFilter);
		myPpt.RemoveSRFromArray(tsf_Shadow.GetComponent<SpriteRenderer>());
		myPpt.RemoveSRFromArray(turret.SR_AimEffect);
		myPpt.RemoveSRFromArray(turret.SR_AimEffectScaler);
		myPpt.RemoveSRFromArray(turret.SR_CannonAim);
		moveDir = Vector3.right;
		turret.aimDir = moveDir;
		Inst = this;
		turret.Initialize();
		station.Initialize();
		ChargeBeforeCount.RandomResult();
		warningLine.enabled = false;
		state = MonsterState.BornIdle;
	}

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		AS_Move.volume = DataMgr.settingData.GetFinalSound();
		AS_MissileChasing.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		tsf_Bottom.localEulerAngles = Tool2D.GetEulerAngleByDir(moveDir) + new Vector3(0f, 0f, 90f);
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
		if (changedState)
		{
			if (state == MonsterState.Move || state == MonsterState.Charge)
			{
				if (!AS_Move.isPlaying)
				{
					AS_Move.Play();
				}
			}
			else
			{
				AS_Move.Stop();
			}
		}
		if (!base.deadStayed)
		{
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			tsf_Shadow.localEulerAngles = tsf_Bottom.localEulerAngles;
			turret.Update();
			station.Update();
			boss50Collider.SyncPosition(base.transform.position, tsf_Bottom.localEulerAngles);
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			_ = changedState;
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.Idle;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Idle:
		{
			ref bool reference5 = ref varMgr.RegBool(0);
			if (changedState)
			{
				turret.attacking = true;
				station.attacking = true;
				idleTime.RandomResult();
			}
			if (base.HaveTarget)
			{
				float num = Tool2D.IgnoreZAngle(moveDir, ToTargetDir());
				if (!reference5 && num < 0.1f)
				{
					reference5 = true;
				}
				else if (reference5 && num > 20f)
				{
					reference5 = false;
				}
				if (!reference5)
				{
					moveDir = Tool2D.RotateTowardsAroundZAxis(moveDir, ToTargetDir(), rotateSpeed * Time.deltaTime).normalized;
				}
			}
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.Move;
			}
			else
			{
				SetMove(Vector3.zero);
			}
			break;
		}
		case MonsterState.Move:
			if (changedState)
			{
				movetime.RandomResult();
				chargeBeforeCounter++;
				if (chargeBeforeCounter >= ChargeBeforeCount.result)
				{
					chargeBeforeCounter = 0;
					ChargeBeforeCount.RandomResult();
					state = MonsterState.ChargeBefore;
					break;
				}
			}
			if (stateExistTime < movetime.result)
			{
				SetMove(moveDir.normalized * base.MoveSpeed);
			}
			else
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.ChargeBefore:
		{
			ref bool reference6 = ref varMgr.RegBool(0);
			if (changedState)
			{
				turret.attacking = false;
				station.attacking = false;
				attackZone.attackedEtt.Clear();
			}
			for (int i = 0; i < warningLine.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(base.transform.position, base.transform.position + moveDir * chargeSpeed * chargeTime, (float)i / (float)(warningLine.positionCount - 1));
				warningLine.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
			if (base.HaveTarget)
			{
				Tool2D.IgnoreZAngle(moveDir, ToTargetDir());
				moveDir = Tool2D.RotateTowardsAroundZAxis(moveDir, ToTargetDir(), chargeRotateSpeed * Time.deltaTime).normalized;
			}
			SetMove(Vector3.zero);
			if (turret.state == Boss50Turret.TurretState.Stop && station.state == Boss50WeaponStation.WeaponState.Stop)
			{
				if (!reference6)
				{
					warningLine.enabled = true;
					SEMgr.Inst.boss50ChargePrepare.PlaySE();
					reference6 = true;
					chargeParticle.Play();
				}
				if (stateExistTime > chargeBeforeTime)
				{
					state = MonsterState.Charge;
				}
			}
			else
			{
				stateExistTime = 0f;
			}
			break;
		}
		case MonsterState.Charge:
		{
			if (changedState)
			{
				warningLine.enabled = false;
			}
			Vector3 vector2 = base.transform.position + moveDir * 2f;
			GetNavInfo(vector2);
			if ((navInfo.ToGoPoint - vector2).sqrMagnitude < 0.01f && stateExistTime < chargeTime)
			{
				SetMove(moveDir.normalized * chargeSpeed);
			}
			else
			{
				state = MonsterState.ChargeAfter;
			}
			break;
		}
		case MonsterState.ChargeAfter:
			_ = changedState;
			if (stateExistTime > chargeAfterTime)
			{
				chargeParticle.Stop();
				state = MonsterState.Idle;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Dead:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			ref Vector3 reference2 = ref varMgr.RegV3(1);
			ref float reference3 = ref varMgr.RegFloat(0);
			ref Vector3 reference4 = ref varMgr.RegV3(2);
			if (changedState)
			{
				reference4 = base.transform.position;
				reference = Tool2D.GetDir();
				reference2 = Tool2D.GetDir(reference, 90f);
			}
			SetMove(Vector3.zero);
			reference3 += Time.deltaTime * shakeFrequency;
			float x = Mathf.PerlinNoise(reference.x * reference3, reference.y * reference3) - 0.5f;
			float y = Mathf.PerlinNoise(reference2.x * reference3, reference2.y * reference3) - 0.5f;
			Vector3 vector = new Vector3(x, y, 0f) * 2f * shakeAmplitude;
			base.transform.position = reference4 + vector;
			break;
		}
		case MonsterState.RandomMove:
			break;
		}
	}

	public void LateUpdate()
	{
		turret.LateUpdate();
	}

	public void OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		if (((state == MonsterState.Charge && state == MonsterState.Move) || !(stateExistTime < 0.2f)) && UnitDotsSyncSystem.GetLayer(collision.GetOtherEntity(myPpt.myEntity)) == 256)
		{
			if (state == MonsterState.Charge)
			{
				state = MonsterState.ChargeAfter;
			}
			else if (state == MonsterState.Move)
			{
				state = MonsterState.Idle;
			}
		}
	}

	public void OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
		((IDotsCollisionReceiver)this).OnCollisionEnter_Dots(collision);
	}

	public void OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}

	protected override void BossDeadStay()
	{
		state = MonsterState.Dead;
		turret.OnDead();
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		boss50Collider.SetCanBeTarget(value: false);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		boss50Collider.DotsAnnouncedDeath();
		base.AfterDead(ref info);
	}
}
