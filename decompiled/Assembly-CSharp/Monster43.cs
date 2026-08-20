using System;
using System.Collections.Generic;
using Spine;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster43 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Chase,
		BeforeDrain,
		Drain,
		DrainFinish,
		ContinueAttack,
		Attack,
		AttackFinish
	}

	public enum DrainTargetType
	{
		Book,
		Wand,
		Player
	}

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("吸取魔力")]
	public List<Entity> targetsInsight = new List<Entity>();

	public List<Vector3> targetsInsightPos = new List<Vector3>();

	public List<Entity> targetsToDrain = new List<Entity>();

	public List<DrainTargetType> targetsType = new List<DrainTargetType>();

	public List<DrainTargetType> targetsToDrainType = new List<DrainTargetType>();

	public List<Monster43_Drain> drainRays = new List<Monster43_Drain>();

	public LayerMask drainMask;

	public float checkInSightTime;

	public float closeRange;

	public float drainRange;

	public float drainAmount;

	public float drainTime;

	public float minDrainTime;

	public float wholeDrainTime;

	public float drainRemainTime;

	public int maxDrainTargets;

	public ParticleSystem drainParticles;

	public float healthConvert;

	public float drainMoveAngle;

	public Vector3 drainMovePos;

	public float drainSpeedFix;

	public VariableFloat actCDTime;

	private float actCDTimer;

	public float drainableTime;

	private float lifeTime;

	[Header("其他行动")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public float randomMoveRadius;

	public float blinkInterval;

	public float blinkDistance;

	public float blinkMinDistance;

	public float chaseRadius;

	[Header("spine 动作时间")]
	public float beforeDrainTime;

	public float drainFinishTime;

	public float attackTime;

	public float startShootTime;

	public float attackFinishTime;

	[Header("光翼展开")]
	public float attackChance;

	public float attackInterval;

	private SpellSpawnParams ssp;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public float spellHeight;

	public VariableFloat spellFinalSpeed;

	public VariableFloat spellFollowRotateSpeed;

	public VariableFloat launchAngle;

	public VariableFloat attackPointOffset;

	private Vector3 attackPoint;

	public float shootInterval;

	public float bulletTimes;

	public float bulletCount;

	public AudioSource as_Drain;

	[Header("二模式")]
	public AIPattern pattern;

	public float continueAttackSpeed;

	public float continueAttackTime;

	public float continueAttackAngle;

	public CollisionFilter Filter_BlockSight = new CollisionFilter
	{
		GroupIndex = 0,
		BelongsTo = 1073741824u,
		CollidesWith = 131328u
	};

	private EntityQuery bookQuery;

	private EntityQuery wandQuery;

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
		if (GameMgr.IsMobile_Static)
		{
			wholeDrainTime *= 0.8f;
			continueAttackTime *= 0.8f;
		}
	}

	public override void EveryInitialCallback()
	{
		bookQuery = UnitDotsSyncSystem.entityMgr.CreateEntityQuery(typeof(Spell2005GrimoireData));
		wandQuery = UnitDotsSyncSystem.entityMgr.CreateEntityQuery(typeof(Spell4005WandSpiritData));
		base.SAnima.timeScale = 1f;
		base.SAnima.AnimationState.Data.DefaultMix = 0f;
		base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
		base.SAnima.Update(1f);
		base.SAnima.skeleton.UpdateWorldTransform(Skeleton.Physics.None);
		base.SAnima.LateUpdate();
		for (int i = 0; i < drainRays.Count; i++)
		{
			drainRays[i].master = this;
			drainRays[i].targetEntity = Entity.Null;
		}
		targetsToDrain.Clear();
		targetsInsight.Clear();
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90121);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		state = MonsterState.BornIdle;
		drainParticles.Clear();
		drainParticles.Stop();
		actCDTimer = UnityEngine.Random.Range(0f, attackInterval);
		lifeTime = 0f;
	}

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundChange));
		SoundChange();
	}

	private void OnDisable()
	{
		bookQuery.Dispose();
		wandQuery.Dispose();
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundChange));
	}

	private void SoundChange()
	{
		as_Drain.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			CloseDrainRay();
			drainParticles.Stop();
			as_Drain.Stop();
			return;
		}
		if (state == MonsterState.Drain)
		{
			drainParticles.Play();
			as_Drain.Play();
		}
		lifeTime += Time.deltaTime;
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
				bornIdleTimer = 0f;
				base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
				base.Anima.Play("Idle");
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.Chase;
			}
			break;
		case MonsterState.RandomMove:
		{
			ref float reference = ref varMgr.RegFloat(0);
			if (changedState)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, randomMoveRadius));
				randomMoveTime.RandomResult();
				reference = 0f;
				base.SAnima.AnimationState.Data.DefaultMix = 0.2f;
				base.SAnima.AnimationState.SetAnimation(0, "Move", loop: true);
				base.Anima.Play("Move");
			}
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, randomMoveRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestMagicTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.Chase;
					break;
				}
			}
			reference += Time.deltaTime;
			if (reference > randomMoveTime.result)
			{
				state = MonsterState.Idle;
			}
			break;
		}
		case MonsterState.Attack:
		{
			ref float reference2 = ref varMgr.RegFloat(0);
			ref float reference3 = ref varMgr.RegFloat(1);
			ref bool reference4 = ref varMgr.RegBool(0);
			if (changedState)
			{
				SetMove(Vector3.zero);
				GetNearestTarget();
				if (!base.HaveTarget)
				{
					state = MonsterState.Chase;
					break;
				}
				base.SAnima.AnimationState.SetAnimation(0, "Attack", loop: false);
				base.Anima.Play("Attack");
			}
			if (stateExistTime > attackTime)
			{
				state = MonsterState.AttackFinish;
				break;
			}
			reference2 += Time.deltaTime;
			if (reference2 > startShootTime && !reference4)
			{
				reference4 = true;
			}
			if (reference4)
			{
				reference3 += Time.deltaTime;
			}
			if (reference3 > shootInterval)
			{
				reference3 -= shootInterval;
				TryShootBullet();
			}
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * 0.5f);
			}
			else
			{
				GetNearestTargetPlayerFirst();
				SetMove(Vector3.zero, isFlip: false);
			}
			break;
		}
		case MonsterState.ContinueAttack:
		{
			ref float reference11 = ref varMgr.RegFloat(0);
			ref float reference12 = ref varMgr.RegFloat(1);
			ref bool reference13 = ref varMgr.RegBool(0);
			ref bool reference14 = ref varMgr.RegBool(1);
			ref Vector3 reference15 = ref varMgr.RegV3(0);
			if (changedState)
			{
				SetMove(Vector3.zero);
				GetNearestTarget();
				if (!base.HaveTarget)
				{
					state = MonsterState.Chase;
					break;
				}
				base.SAnima.AnimationState.SetAnimation(0, "Attack", loop: true);
				base.Anima.Play("Attack");
				reference15 = Tool2D.GetDir();
				reference14 = GeneralTool.ChanceResult(0.5f);
			}
			if (stateExistTime > continueAttackTime)
			{
				state = MonsterState.Chase;
				break;
			}
			reference11 += Time.deltaTime;
			if (reference11 > 0f && !reference13)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Run", loop: true);
				reference13 = true;
			}
			if (reference13)
			{
				reference12 += Time.deltaTime;
			}
			if (reference12 > shootInterval)
			{
				reference12 -= shootInterval;
				TryShootBullet();
			}
			if (base.HaveTarget)
			{
				reference15 = Tool2D.GetDir(ToTargetDir(), continueAttackAngle * (float)((!reference14) ? 1 : (-1)));
				GetNavInfo(base.transform.position + reference15 * 3f);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * (reference13 ? continueAttackSpeed : 0f));
			}
			else
			{
				GetNearestTargetPlayerFirst();
				GetNavInfo(reference15 * 3f);
				SetMove(reference15 * base.MoveSpeed);
			}
			break;
		}
		case MonsterState.AttackFinish:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "AttackFinish", loop: false);
			}
			if (stateExistTime > attackFinishTime)
			{
				state = MonsterState.Chase;
			}
			else if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * 0.5f);
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			break;
		case MonsterState.Idle:
		{
			ref float reference10 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.SAnima.timeScale = 1f;
				SetMove(Vector3.zero);
				idleTime.RandomResult();
				base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
				base.Anima.Play("Idle");
				base.SAnima.timeScale = 0.8f;
			}
			SetMove(Vector3.zero);
			checkTargetIntervalTimer += Time.deltaTime * base.SAnima.timeScale;
			reference10 += Time.deltaTime;
			if (reference10 > idleTime.result && checkTargetIntervalTimer > 1f)
			{
				GetNearestMagicTarget();
				if (base.HaveTarget)
				{
					base.SAnima.timeScale = 1f;
					state = MonsterState.Chase;
					break;
				}
			}
			if (reference10 > idleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			break;
		}
		case MonsterState.BeforeDrain:
		{
			ref float reference5 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "BeforeDrain", loop: false);
				base.Anima.Play("DrainPrepare");
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				if (navInfo.allCornerArrived)
				{
					GetNavInfo(base.TargetPoint);
				}
				else
				{
					CheckNavInfo();
					SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * drainSpeedFix);
				}
			}
			else
			{
				SetMove(Vector3.zero);
				SetFlip(ToTargetDir().x);
			}
			reference5 += Time.deltaTime;
			if (reference5 > beforeDrainTime)
			{
				state = MonsterState.Drain;
			}
			break;
		}
		case MonsterState.Drain:
		{
			ref float reference6 = ref varMgr.RegFloat(0);
			ref float reference7 = ref varMgr.RegFloat(1);
			ref float reference8 = ref varMgr.RegFloat(2);
			ref float reference9 = ref varMgr.RegFloat(3);
			if (changedState)
			{
				Drain();
				drainParticles.Play();
				as_Drain.Play();
				checkTargetIntervalTimer = 0f;
				base.SAnima.AnimationState.SetAnimation(0, "Drain", loop: true);
				base.Anima.Play("Drain");
				drainMovePos = base.transform.position;
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				if (navInfo.allCornerArrived)
				{
					GetNavInfo(base.TargetPoint);
				}
				else
				{
					CheckNavInfo();
					SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * drainSpeedFix, isFlip: false);
					SetFlip(ToTargetDir().x);
				}
			}
			else
			{
				SetMove(Vector3.zero);
			}
			reference7 += Time.deltaTime;
			if (reference7 > checkInSightTime)
			{
				reference7 = 0f;
				CheckInSight();
			}
			SetDrainRay();
			reference9 += Time.deltaTime;
			if (reference9 > drainTime)
			{
				reference9 = 0f;
				Drain();
			}
			reference8 += Time.deltaTime;
			if (reference8 > wholeDrainTime)
			{
				CloseDrainRay();
				drainParticles.Stop();
				as_Drain.Stop();
				state = MonsterState.DrainFinish;
				break;
			}
			if (reference8 > minDrainTime)
			{
				if (targetsToDrain.Count == 0)
				{
					reference6 += Time.deltaTime;
				}
				else
				{
					reference6 = 0f;
				}
			}
			if (reference6 > drainRemainTime)
			{
				CloseDrainRay();
				drainParticles.Stop();
				as_Drain.Stop();
				state = MonsterState.DrainFinish;
			}
			break;
		}
		case MonsterState.DrainFinish:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "DrainFinish", loop: false);
				base.Anima.Play("DrainFinish");
			}
			SetMove(Vector3.zero);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			if (stateExistTime > drainFinishTime)
			{
				state = MonsterState.Chase;
			}
			break;
		case MonsterState.Chase:
			if (changedState)
			{
				GetNearestTargetPlayerFirst();
				if (!base.HaveTarget)
				{
					state = MonsterState.Idle;
					break;
				}
				base.SAnima.timeScale = 0.8f;
				base.SAnima.AnimationState.SetAnimation(0, "Move", loop: true);
				base.Anima.Play("Move");
				actCDTime.RandomResult();
			}
			GetNearestTargetPlayerFirst();
			if (!base.HaveTarget)
			{
				base.SAnima.timeScale = 1f;
				state = MonsterState.Idle;
				break;
			}
			GetNavInfo(base.TargetPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			actCDTimer += Time.deltaTime;
			if (!(actCDTimer > actCDTime.result) || !(ToTargetDistanceSqr() < closeRange * closeRange))
			{
				break;
			}
			actCDTimer = 0f;
			base.SAnima.timeScale = 1f;
			if (UnityEngine.Random.Range(0f, 1f) < attackChance || lifeTime > drainableTime)
			{
				if (pattern == AIPattern.Pattern2)
				{
					state = MonsterState.ContinueAttack;
				}
				else
				{
					state = MonsterState.Attack;
				}
			}
			else
			{
				state = MonsterState.BeforeDrain;
			}
			break;
		}
	}

	private void SetDrainRay()
	{
		for (int i = 0; i < maxDrainTargets; i++)
		{
			drainRays[i].targetEntity = Entity.Null;
		}
		for (int j = 0; j < targetsToDrain.Count; j++)
		{
			drainRays[j].targetEntity = targetsToDrain[j];
		}
	}

	private void CloseDrainRay()
	{
		for (int i = 0; i < maxDrainTargets; i++)
		{
			drainRays[i].targetEntity = Entity.Null;
		}
	}

	public void GetNearestMagicTarget()
	{
		targetEntity = Entity.Null;
		float num = 100000000f;
		if (PlayerMgr.Inst.PlayerCtrller.IsVisible)
		{
			Vector3 position = PlayerMgr.Inst.PlayerCtrller.transform.position;
			num = Tool2D.IgnoreZDistance(position, base.transform.position);
			if (!UnitDotsSyncSystem.Raycast(base.transform.position, position - base.transform.position, num, Filter_BlockSight, out var _))
			{
				targetEntity = PlayerMgr.Inst.PlayerEtt;
			}
		}
		NativeArray<Entity> nativeArray = wandQuery.ToEntityArray(Allocator.Temp);
		for (int i = 0; i < nativeArray.Length; i++)
		{
			Vector3 vector = GetComponentData<LocalTransform>(nativeArray[i]).Position;
			float num2 = Tool2D.IgnoreZDistance(vector, base.transform.position);
			if ((targetEntity == Entity.Null || num2 < num) && !UnitDotsSyncSystem.Raycast(base.transform.position, vector - base.transform.position, num2, Filter_BlockSight, out var _))
			{
				num = num2;
				targetEntity = nativeArray[i];
			}
		}
		nativeArray.Dispose();
		NativeArray<Entity> nativeArray2 = wandQuery.ToEntityArray(Allocator.Temp);
		for (int j = 0; j < nativeArray2.Length; j++)
		{
			Vector3 vector2 = GetComponentData<LocalTransform>(nativeArray2[j]).Position;
			float num3 = Tool2D.IgnoreZDistance(vector2, base.transform.position);
			if ((targetEntity == Entity.Null || num3 < num) && !UnitDotsSyncSystem.Raycast(base.transform.position, vector2 - base.transform.position, num3, Filter_BlockSight, out var _))
			{
				num = num3;
				targetEntity = nativeArray2[j];
			}
		}
		nativeArray2.Dispose();
	}

	public void CheckInSight()
	{
		for (int num = targetsToDrain.Count - 1; num >= 0; num--)
		{
			Entity entity = targetsToDrain[num];
			if (!EntityIsValid(entity))
			{
				targetsToDrain.RemoveAt(num);
			}
			else
			{
				Vector3 vector = GetComponentData<LocalTransform>(entity).Position;
				if (UnitDotsSyncSystem.Raycast(base.transform.position, vector - base.transform.position, Vector3.Distance(vector, base.transform.position), Filter_BlockSight, out var _))
				{
					targetsToDrain.RemoveAt(num);
				}
				else if (targetsToDrain[num] == PlayerMgr.Inst.PlayerEtt && !PlayerMgr.Inst.PlayerCtrller.IsVisible)
				{
					targetsToDrain.RemoveAt(num);
				}
			}
		}
	}

	public void Drain()
	{
		targetsInsight.Clear();
		targetsInsightPos.Clear();
		targetsType.Clear();
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, drainRange, GameConst.Filter_Friendly, list);
		for (int i = 0; i < list.Count; i++)
		{
			Entity entity = list[i].entity;
			float distance = list[i].distance;
			Vector3 point = list[i].point;
			Spell4005WandSpiritData result3;
			if (UnitDotsSyncSystem.TryGetComponent<Spell2005GrimoireData>(entity, out var _))
			{
				if (!UnitDotsSyncSystem.Raycast(base.transform.position, point - base.transform.position, distance, Filter_BlockSight, out var _))
				{
					targetsInsight.Add(entity);
					targetsInsightPos.Add(point);
					targetsType.Add(DrainTargetType.Book);
				}
			}
			else if (UnitDotsSyncSystem.TryGetComponent<Spell4005WandSpiritData>(entity, out result3))
			{
				if (!UnitDotsSyncSystem.Raycast(base.transform.position, point - base.transform.position, distance, Filter_BlockSight, out var _))
				{
					targetsInsight.Add(entity);
					targetsInsightPos.Add(point);
					targetsType.Add(DrainTargetType.Wand);
				}
			}
			else
			{
				if (!(list[i].entity == PlayerMgr.Inst.PlayerEtt))
				{
					continue;
				}
				bool flag = false;
				foreach (Wand key in PlayerMgr.Inst.autoWandList.Keys)
				{
					if (PlayerMgr.Inst.SelectedWand == key)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Vector3 position = PlayerMgr.Inst.PlayerCtrller.transform.position;
					if (!UnitDotsSyncSystem.Raycast(base.transform.position, position - base.transform.position, Vector3.Distance(position, base.transform.position), Filter_BlockSight, out var _) && PlayerMgr.Inst.PlayerCtrller.IsVisible)
					{
						targetsInsight.Add(PlayerMgr.Inst.PlayerEtt);
						targetsInsightPos.Add(PlayerMgr.Inst.PlayerPoint);
						targetsType.Add(DrainTargetType.Player);
					}
				}
			}
		}
		targetsToDrain.Clear();
		targetsToDrainType.Clear();
		for (int j = 0; j < maxDrainTargets; j++)
		{
			Entity entity2 = Entity.Null;
			for (int k = 0; k < targetsInsight.Count; k++)
			{
				if (!targetsToDrain.Contains(targetsInsight[k]))
				{
					if (entity2 == Entity.Null)
					{
						entity2 = targetsInsight[k];
					}
					else if (Vector3.SqrMagnitude(targetsInsightPos[k] - base.transform.position) < Vector3.SqrMagnitude(targetsInsightPos[targetsInsight.IndexOf(entity2)] - base.transform.position))
					{
						entity2 = targetsInsight[k];
					}
				}
			}
			if (entity2 != Entity.Null)
			{
				targetsToDrain.Add(entity2);
				targetsToDrainType.Add(targetsType[targetsInsight.IndexOf(entity2)]);
			}
		}
		for (int l = 0; l < targetsToDrain.Count; l++)
		{
			Entity entity3 = targetsToDrain[l];
			switch (targetsToDrainType[l])
			{
			case DrainTargetType.Book:
			{
				Spell2005GrimoireData componentData = GetComponentData<Spell2005GrimoireData>(entity3);
				componentData.CurrentMp -= drainAmount;
				if (componentData.CurrentMp < 0f)
				{
					componentData.CurrentMp = 0f;
				}
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize((0f - drainAmount).ToString(), UITextFloatType.DropMP, GetComponentData<LocalTransform>(entity3).Position);
				SetComponentData(componentData, entity3);
				UnitDotsSyncSystem.UnitRecoveryHP(myPpt.myEntity, drainAmount * healthConvert, UnitDotsSyncSystem.entityMgr, needTextFloat: true, needCreateEF: false);
				break;
			}
			case DrainTargetType.Wand:
			{
				Spell4005WandSpiritData componentData2 = GetComponentData<Spell4005WandSpiritData>(entity3);
				componentData2.Wand.Value.GainMP(0f - drainAmount);
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize((0f - drainAmount).ToString(), UITextFloatType.DropMP, componentData2.Wand.Value.transform.position);
				UnitDotsSyncSystem.UnitRecoveryHP(myPpt.myEntity, drainAmount * healthConvert, UnitDotsSyncSystem.entityMgr, needTextFloat: true, needCreateEF: false);
				break;
			}
			case DrainTargetType.Player:
			{
				bool flag2 = false;
				for (int m = 0; m < PlayerMgr.Inst.Wands.Count; m++)
				{
					if (!PlayerMgr.Inst.Wands[m].passiveAutoWand && PlayerMgr.Inst.Wands[m].WandCfg != null && PlayerMgr.Inst.Wands[m] == PlayerMgr.Inst.SelectedWand)
					{
						flag2 = true;
						ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize((0f - drainAmount).ToString(), UITextFloatType.DropMP, PlayerMgr.Inst.PlayerPoint);
						PlayerMgr.Inst.Wands[m].GainMP(0f - drainAmount);
						break;
					}
				}
				if (flag2)
				{
					UnitDotsSyncSystem.UnitRecoveryHP(myPpt.myEntity, drainAmount * healthConvert, UnitDotsSyncSystem.entityMgr, needTextFloat: true, needCreateEF: false);
				}
				break;
			}
			}
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		StopAllCoroutines();
	}

	private void TryShootBullet()
	{
		GetNearestTarget();
		if (base.HaveTarget)
		{
			attackPoint = base.TargetPointIgnoreZ;
		}
		else
		{
			attackPoint = base.transform.position + Tool2D.GetDir() * 5f;
		}
		SEMgr.Inst.monster43Attack.PlaySE();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		for (int i = 0; (float)i < bulletCount; i++)
		{
			launchAngle.RandomResult();
			sSPModifier.Direction = Tool2D.GetDir(base.transform.position - attackPoint, launchAngle.result).normalized;
			spellFinalSpeed.RandomResult();
			sSPModifier.Float1 = 5f;
			sSPModifier.Float2 = spellFinalSpeed.result / ssp.MovementComponentData.Speed;
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight) + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0f, 0f);
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
	}

	public override void AnimaAction(string animaName)
	{
	}
}
