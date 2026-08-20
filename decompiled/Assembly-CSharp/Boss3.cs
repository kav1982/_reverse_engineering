using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Boss3 : UnitBase
{
	private enum UnitState
	{
		BornIdle,
		Rotate,
		Stop,
		RotateAttack
	}

	private enum RotateDir
	{
		Left,
		Up,
		Right,
		Down
	}

	private enum AttackState
	{
		NoAttack,
		AttackBefore,
		Attacking
	}

	public Transform tsf_Model;

	public float rotateSpeed;

	public float rotateStopTime;

	public ShockParam rotateShock;

	[Header("Laser")]
	public GameObject pfb_Laser;

	public float laserOffset;

	public float laserHeight;

	public float laserMaxLength;

	public float laserWidth;

	public ShockParam laserShock;

	[Header("Attack")]
	public LayerMask attackLayer;

	public VariableFloat attackInterval;

	public float attackDuration;

	[Range(0f, 1f)]
	public float rotateAttackChance;

	public float rotateAttackRotateAngle;

	public float rotateAttackRotateSpeed;

	public float damageInterval;

	public int damage;

	public float summonDamageRatio;

	public LayerMask laserStopMask;

	[Header("Corpse")]
	public GameObject[] pfb_Corpses;

	public int corpseCount;

	[Header("Stage2")]
	public int stage2ID;

	[Header("Audio")]
	public AudioSource as_Roll;

	public AudioSource as_LaserStart;

	public AudioSource as_LaserLoop;

	public AudioSource as_LaserEnd;

	[Header("和谐模式")]
	public List<AnimationClip> harmonyAnimations = new List<AnimationClip>();

	private UnitState unitState;

	private RotateDir rotateDir;

	private AttackState attackState;

	private GameObject shadowGO;

	private GameObject invisibleRotateGO;

	private Boss3Laser laserL;

	private Boss3Laser laserR;

	private float rotateAngleCounter;

	private float rotateMoveSpeed;

	private float stopTimer;

	private float attackIntervalTimer;

	private float attackBeforeTime;

	private float attackBeforeTimer;

	private float attackDurationTimer;

	private float rotateAttackRotateCounter;

	private bool rotateAttackRotateLeft;

	private float damageIntervalTimer;

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
		as_Roll.volume = DataMgr.settingData.GetFinalSound();
		as_LaserStart.volume = DataMgr.settingData.GetFinalSound();
		as_LaserLoop.volume = DataMgr.settingData.GetFinalSound();
		as_LaserEnd.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		ObjPoolMgr.Inst.PreloadGO("Prefabs/Units/" + stage2ID, 1f, ObjPoolMgr.PreloadType.Unit);
		Shadow component = GetComponent<Shadow>();
		component.CreateShadow();
		shadowGO = component.ShadowGO;
		invisibleRotateGO = new GameObject();
		invisibleRotateGO.transform.position = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(0f, 1f, 0f);
		invisibleRotateGO.transform.SetParent(LevelMgr.Inst.CurrentRoomT);
		if (GameMgr.IsMobile_Static)
		{
			rotateSpeed *= 0.9f;
			rotateAttackRotateSpeed *= 0.8f;
		}
		rotateMoveSpeed = base.CC_Self.radius * 2f / (90f / rotateSpeed);
		attackBeforeTime = 90f / rotateSpeed * 2f + rotateStopTime * 2f;
		attackInterval.RandomResult();
		laserL = UnityEngine.Object.Instantiate(pfb_Laser, base.transform.parent).GetComponent<Boss3Laser>();
		laserR = UnityEngine.Object.Instantiate(pfb_Laser, base.transform.parent).GetComponent<Boss3Laser>();
		if (!GameMgr.IsHarmony_Static)
		{
			return;
		}
		AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(base.Anima.runtimeAnimatorController);
		base.Anima.runtimeAnimatorController = animatorOverrideController;
		for (int i = 0; i < harmonyAnimations.Count; i++)
		{
			string text = harmonyAnimations[i].name.Substring(0, harmonyAnimations[i].name.Length - 2);
			if (animatorOverrideController[text] != null)
			{
				animatorOverrideController[text] = harmonyAnimations[i];
			}
		}
	}

	public override void Update()
	{
		if (attackState == AttackState.Attacking)
		{
			Vector3 vector = base.transform.position - invisibleRotateGO.transform.right * laserOffset + new Vector3(0f, 0f, 0f - laserHeight);
			Vector3 vector2 = base.transform.position + invisibleRotateGO.transform.right * laserOffset + new Vector3(0f, 0f, 0f - laserHeight);
			Vector3 vector3 = vector - invisibleRotateGO.transform.right * laserMaxLength;
			if (UnitDotsSyncSystem.Raycast(vector, -invisibleRotateGO.transform.right, laserMaxLength, GameConst.Filter_Laser, out var result))
			{
				vector3 = result.point;
			}
			else if (UnitDotsSyncSystem.Raycast(vector, -invisibleRotateGO.transform.right, laserMaxLength, GameConst.Filter_Wall, out result))
			{
				vector3 = result.point;
			}
			Vector3 vector4 = vector2 + invisibleRotateGO.transform.right * laserMaxLength;
			if (UnitDotsSyncSystem.Raycast(vector2, invisibleRotateGO.transform.right, laserMaxLength, GameConst.Filter_Laser, out result))
			{
				vector4 = result.point;
			}
			else if (UnitDotsSyncSystem.Raycast(vector, -invisibleRotateGO.transform.right, laserMaxLength, GameConst.Filter_Wall, out result))
			{
				vector3 = result.point;
			}
			damageIntervalTimer += Time.deltaTime;
			if (damageIntervalTimer >= damageInterval)
			{
				damageIntervalTimer = 0f;
				CamController.Inst.SetShock(laserShock.radius, laserShock.speed, damageInterval);
				UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(vector, -invisibleRotateGO.transform.right, laserWidth, Vector3.Distance(vector, vector3), GameConst.Filter_MonsterAoeNoSpell);
				for (int i = 0; i < array.Length; i++)
				{
					if (EntityIsValid(array[i].entity))
					{
						UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(array[i].entity);
						TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
						info.damage = damage;
						if (componentData.unitCfg.unitType != 0)
						{
							info.damage *= summonDamageRatio;
						}
						UnitDotsSyncSystem.AddTakeDamageRequest(array[i].entity, info);
					}
				}
				array = UnitDotsSyncSystem.SphereCastAll(vector2, invisibleRotateGO.transform.right, laserWidth, Vector3.Distance(vector2, vector4), GameConst.Filter_MonsterAoeNoSpell);
				for (int j = 0; j < array.Length; j++)
				{
					if (EntityIsValid(array[j].entity))
					{
						UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>(array[j].entity);
						TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
						info2.damage = damage;
						if (componentData2.unitCfg.unitType != 0)
						{
							info2.damage *= summonDamageRatio;
						}
						UnitDotsSyncSystem.AddTakeDamageRequest(array[j].entity, info2);
					}
				}
			}
			laserL.SetLaser(vector, vector3);
			laserR.SetLaser(vector2, vector4);
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (unitState)
		{
		case UnitState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				DefineRotateDir();
			}
			break;
		case UnitState.Rotate:
		{
			float num2 = rotateSpeed * Time.deltaTime;
			rotateAngleCounter += num2;
			if (rotateAngleCounter >= 90f)
			{
				num2 = 90f - (rotateAngleCounter - num2);
				rotateAngleCounter = 0f;
				unitState = UnitState.Stop;
				CamController.Inst.SetShock(rotateShock);
				as_Roll.Play();
			}
			switch (rotateDir)
			{
			case RotateDir.Left:
				invisibleRotateGO.transform.Rotate(0f, num2, 0f, Space.World);
				base.transform.Translate((0f - rotateMoveSpeed) * Time.deltaTime, 0f, 0f, Space.World);
				break;
			case RotateDir.Up:
				invisibleRotateGO.transform.Rotate(num2, 0f, 0f, Space.World);
				base.transform.Translate(0f, rotateMoveSpeed * Time.deltaTime, 0f, Space.World);
				break;
			case RotateDir.Right:
				invisibleRotateGO.transform.Rotate(0f, 0f - num2, 0f, Space.World);
				base.transform.Translate(rotateMoveSpeed * Time.deltaTime, 0f, 0f, Space.World);
				break;
			case RotateDir.Down:
				invisibleRotateGO.transform.Rotate(0f - num2, 0f, 0f, Space.World);
				base.transform.Translate(0f, (0f - rotateMoveSpeed) * Time.deltaTime, 0f, Space.World);
				break;
			default:
				Debug.LogError(rotateDir);
				break;
			}
			SyncDotsPosition();
			ChangeRealRotate();
			if (unitState != UnitState.Stop || !(attackIntervalTimer >= attackInterval.result))
			{
				break;
			}
			Ray ray = new Ray(invisibleRotateGO.transform.position, -invisibleRotateGO.transform.right);
			Ray ray2 = new Ray(invisibleRotateGO.transform.position, invisibleRotateGO.transform.right);
			bool num3 = UnitDotsSyncSystem.Raycast(ray, 100f, GameConst.Filter_Wall);
			bool flag = UnitDotsSyncSystem.Raycast(ray2, 100f, GameConst.Filter_Wall);
			if (num3 || flag)
			{
				attackState = AttackState.AttackBefore;
				base.Anima.SetTrigger("Attack");
				attackIntervalTimer = 0f;
				attackInterval.RandomResult();
				as_LaserStart.Play();
				if (UnityEngine.Random.value <= rotateAttackChance)
				{
					unitState = UnitState.RotateAttack;
					rotateAttackRotateLeft = ((UnityEngine.Random.Range(0, 2) == 0) ? true : false);
				}
			}
			break;
		}
		case UnitState.Stop:
			stopTimer += Time.deltaTime;
			if (stopTimer >= rotateStopTime)
			{
				stopTimer = 0f;
				DefineRotateDir();
			}
			break;
		case UnitState.RotateAttack:
			if (attackState == AttackState.Attacking)
			{
				float num = rotateAttackRotateSpeed * Time.deltaTime;
				rotateAttackRotateCounter += num;
				if (rotateAttackRotateCounter >= rotateAttackRotateAngle)
				{
					num = rotateAttackRotateAngle - (rotateAttackRotateCounter - num);
					rotateAttackRotateCounter = 0f;
					DefineRotateDir();
					StopAttack();
				}
				if (rotateAttackRotateLeft)
				{
					num = 0f - num;
				}
				invisibleRotateGO.transform.Rotate(0f, 0f, num, Space.World);
				ChangeRealRotate();
				shadowGO.transform.Rotate(0f, 0f, num, Space.World);
			}
			break;
		default:
			Debug.LogError(unitState);
			break;
		}
		switch (attackState)
		{
		case AttackState.NoAttack:
			if (unitState != 0)
			{
				attackIntervalTimer += Time.deltaTime;
			}
			break;
		case AttackState.AttackBefore:
			attackBeforeTimer += Time.deltaTime;
			if (attackBeforeTimer >= attackBeforeTime)
			{
				attackBeforeTimer = 0f;
				attackState = AttackState.Attacking;
				as_LaserStart.Stop();
				as_LaserLoop.Play();
			}
			break;
		case AttackState.Attacking:
			if (unitState != UnitState.RotateAttack)
			{
				attackDurationTimer += Time.deltaTime;
				if (attackDurationTimer >= attackDuration)
				{
					attackDurationTimer = 0f;
					StopAttack();
				}
			}
			break;
		default:
			Debug.LogError(attackState);
			break;
		}
	}

	private void DefineRotateDir()
	{
		unitState = UnitState.Rotate;
		GetNearestTargetPlayerFirst();
		Vector3 vector;
		if (base.HaveTarget)
		{
			if (attackState == AttackState.NoAttack)
			{
				GetNavInfo(base.TargetPoint);
				vector = navInfo.ToGoPoint - Tool2D.IgnoreZPoint(base.transform);
			}
			else
			{
				vector = Tool2D.IgnoreZPoint(base.TargetPoint - base.transform.position);
			}
		}
		else
		{
			vector = Tool2D.GetDir();
		}
		Vector3 right = invisibleRotateGO.transform.right;
		right.x = Mathf.Round(right.x);
		right.y = Mathf.Round(right.y);
		right.z = Mathf.Round(right.z);
		if (vector.x > 0f)
		{
			if (vector.y > 0f)
			{
				if (attackState == AttackState.NoAttack)
				{
					if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
					{
						rotateDir = RotateDir.Right;
					}
					else
					{
						rotateDir = RotateDir.Up;
					}
				}
				else if (right == Vector3.right || right == -Vector3.right)
				{
					rotateDir = RotateDir.Up;
				}
				else if (right == Vector3.up || right == -Vector3.up)
				{
					rotateDir = RotateDir.Right;
				}
				else
				{
					Debug.LogError(right);
				}
			}
			else if (attackState == AttackState.NoAttack)
			{
				if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
				{
					rotateDir = RotateDir.Right;
				}
				else
				{
					rotateDir = RotateDir.Down;
				}
			}
			else if (right == Vector3.right || right == -Vector3.right)
			{
				rotateDir = RotateDir.Down;
			}
			else if (right == Vector3.up || right == -Vector3.up)
			{
				rotateDir = RotateDir.Right;
			}
			else
			{
				Debug.LogError(right);
			}
		}
		else if (vector.y > 0f)
		{
			if (attackState == AttackState.NoAttack)
			{
				if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
				{
					rotateDir = RotateDir.Left;
				}
				else
				{
					rotateDir = RotateDir.Up;
				}
			}
			else if (right == Vector3.right || right == -Vector3.right)
			{
				rotateDir = RotateDir.Up;
			}
			else if (right == Vector3.up || right == -Vector3.up)
			{
				rotateDir = RotateDir.Left;
			}
			else
			{
				Debug.LogError(right);
			}
		}
		else if (attackState == AttackState.NoAttack)
		{
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
			{
				rotateDir = RotateDir.Left;
			}
			else
			{
				rotateDir = RotateDir.Down;
			}
		}
		else if (right == Vector3.right || right == -Vector3.right)
		{
			rotateDir = RotateDir.Down;
		}
		else if (right == Vector3.up || right == -Vector3.up)
		{
			rotateDir = RotateDir.Left;
		}
		else
		{
			Debug.LogError(right);
		}
	}

	private void StopAttack()
	{
		attackState = AttackState.NoAttack;
		base.Anima.SetTrigger("Idle");
		as_LaserLoop.Stop();
		as_LaserEnd.Play();
		laserL.Stop();
		laserR.Stop();
	}

	private void ChangeRealRotate()
	{
		tsf_Model.rotation = invisibleRotateGO.transform.rotation;
		tsf_Model.Rotate(60f, 0f, 0f, Space.World);
	}

	protected override void BossDeadStay()
	{
		base.Anima.SetTrigger("Dead");
		base.enabled = false;
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

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		MusicMgr.Inst.ForcePlayMusic("");
		SEMgr.Inst.boss3Dead2.PlaySE();
		ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + stage2ID, base.transform.position);
		UnityEngine.Object.Destroy(laserL.gameObject);
		UnityEngine.Object.Destroy(laserR.gameObject);
		for (int i = 0; i < corpseCount; i++)
		{
			UnityEngine.Object.Instantiate(pfb_Corpses[UnityEngine.Random.Range(0, pfb_Corpses.Length)], base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Corpse>().Initialize(Vector3.zero);
		}
		ObjPoolMgr inst = ObjPoolMgr.Inst;
		FixedString128Bytes deadEF = myPpt.unitCfg.deadEF;
		inst.GetGO("Prefabs/EF/" + deadEF.ToString(), base.transform.position).transform.localScale = Vector3.one * 2f;
	}
}
