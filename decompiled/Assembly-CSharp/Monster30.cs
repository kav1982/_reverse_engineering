using Unity.Transforms;
using UnityEngine;

public class Monster30 : UnitBase
{
	public enum MonsterState
	{
		Idle,
		MirrorBefore,
		Mirror,
		RunToTarget,
		RandomMove
	}

	[Space(50f)]
	public VariableFloat mirrorInterval;

	public int mirrorCount;

	public float mirrorBeforeTime;

	public float mirrorTime;

	public float mirrorSpeed;

	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public float idleTime;

	private Vector3 randomMovePoint;

	private bool isMainBody = true;

	private float mirrorIntervalTimer;

	private float mirrorAngleOffset;

	private float mirrorBeforeTimer;

	private float mirrorTimer;

	private Monster30[] mirrors;

	private Vector3 startPoint;

	private Vector3 endPoint;

	public AIPattern pattern;

	private LocalTransform localTsf;

	private UnitBase unk;

	private bool needRecycleMirror;

	[Header("状态机")]
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
			mirrorSpeed *= 0.8f;
		}
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.Idle;
		isMainBody = true;
		mirrorIntervalTimer = 9999f;
		mirrorAngleOffset = 0f;
		mirrorBeforeTimer = 0f;
		mirrorTimer = 0f;
		mirrors = null;
		if (GameMgr.IsHarmony_Static)
		{
			base.SAnima.initialSkinName += "_HX";
		}
		base.SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
		base.SAnima.Update(1f);
		base.SAnima.LateUpdate();
		mirrorInterval.RandomResult();
		base.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		needRecycleMirror = false;
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (needRecycleMirror)
		{
			needRecycleMirror = false;
			state = MonsterState.MirrorBefore;
			if (mirrors != null)
			{
				for (int i = 1; i < mirrors.Length; i++)
				{
					if (mirrors[i].gameObject.activeSelf)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster30_Disappear" + (GameMgr.IsHarmony_Static ? " H" : ""), mirrors[i].transform.position, 2f);
						LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(mirrors[i].myPpt.myEntity);
						mirrors[i].gameObject.SetActive(value: false);
					}
				}
			}
			mirrors = null;
		}
		if (!isMainBody)
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
		mirrorIntervalTimer += Time.deltaTime;
		switch (state)
		{
		case MonsterState.Idle:
			if (changedState)
			{
				AnimaTogether("Idle", loop: true);
			}
			if (stateExistTime > idleTime)
			{
				state = MonsterState.RandomMove;
				break;
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.RunToTarget;
				}
				else
				{
					state = MonsterState.RandomMove;
				}
			}
			MoveTogether(Vector3.zero);
			break;
		case MonsterState.MirrorBefore:
		{
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "MirrorBefore", loop: true);
			}
			MoveTogether(Vector3.zero);
			mirrorBeforeTimer += Time.deltaTime;
			if (!(mirrorBeforeTimer >= mirrorBeforeTime))
			{
				break;
			}
			SEMgr.Inst.monster30Attack.PlaySE();
			mirrorBeforeTimer = 0f;
			state = MonsterState.Mirror;
			mirrorAngleOffset = Random.Range(0, 360);
			SetMirrorStart(0, mirrorAngleOffset);
			if (mirrors != null)
			{
				for (int l = 1; l < mirrors.Length; l++)
				{
					mirrors[l].transform.position = base.transform.position;
					mirrors[l].gameObject.SetActive(value: true);
					mirrors[l].SetMirrorStart(l, mirrorAngleOffset);
					if (pattern == AIPattern.Pattern2)
					{
						for (int m = l; m < mirrors.Length; m++)
						{
							ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster30_Chain" + (GameMgr.IsHarmony_Static ? " H" : "")).GetComponent<Monster30Chain>().Iniatialize(myPpt, mirrors[l - 1].transform, mirrors[m].transform);
						}
						continue;
					}
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster30_Chain" + (GameMgr.IsHarmony_Static ? " H" : "")).GetComponent<Monster30Chain>().Iniatialize(myPpt, mirrors[l - 1].transform, mirrors[l].transform);
					if (l == mirrorCount - 1 && mirrorCount > 2)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster30_Chain" + (GameMgr.IsHarmony_Static ? " H" : "")).GetComponent<Monster30Chain>().Iniatialize(myPpt, mirrors[l].transform, mirrors[0].transform);
					}
				}
				break;
			}
			mirrors = new Monster30[mirrorCount];
			mirrors[0] = this;
			for (int n = 1; n < mirrorCount; n++)
			{
				Monster30 component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + myPpt.unitCfg.id, base.transform.position).GetComponent<Monster30>();
				component.SetMirrorStart(n, mirrorAngleOffset);
				component.MaskAsMirror();
				mirrors[n] = component;
				if (pattern != AIPattern.Pattern2)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster30_Chain" + (GameMgr.IsHarmony_Static ? " H" : "")).GetComponent<Monster30Chain>().Iniatialize(myPpt, mirrors[n - 1].transform, mirrors[n].transform);
					if (n == mirrorCount - 1 && mirrorCount > 2)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster30_Chain" + (GameMgr.IsHarmony_Static ? " H" : "")).GetComponent<Monster30Chain>().Iniatialize(myPpt, mirrors[n].transform, mirrors[0].transform);
					}
				}
			}
			if (pattern != AIPattern.Pattern2)
			{
				break;
			}
			for (int num = 1; num < mirrorCount; num++)
			{
				for (int num2 = num; num2 < mirrors.Length; num2++)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster30_Chain" + (GameMgr.IsHarmony_Static ? " H" : "")).GetComponent<Monster30Chain>().Iniatialize(myPpt, mirrors[num - 1].transform, mirrors[num2].transform);
				}
			}
			break;
		}
		case MonsterState.Mirror:
		{
			for (int j = 0; j < mirrors.Length; j++)
			{
				mirrors[j].transform.position = Vector3.Lerp(mirrors[j].startPoint, mirrors[j].endPoint, mirrorTimer / mirrorTime);
				LocalTransform componentData = mirrors[j].GetComponentData<LocalTransform>();
				componentData.Position = mirrors[j].transform.position;
				mirrors[j].SetComponentData(componentData);
			}
			mirrorTimer += Time.deltaTime;
			if (mirrorTimer >= mirrorTime)
			{
				mirrorTimer = 0f;
				for (int k = 0; k < mirrors.Length; k++)
				{
					mirrors[k].SetMirrorFinish();
				}
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.RunToTarget;
				}
				else
				{
					state = MonsterState.RandomMove;
				}
			}
			break;
		}
		case MonsterState.RunToTarget:
		{
			if (changedState)
			{
				AnimaTogether("Walk", loop: true);
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RandomMove;
				break;
			}
			GetNavInfo(base.TargetPoint);
			Vector3 motion2 = ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed;
			if (ToTargetDistanceSqr() > 0.040000003f)
			{
				MoveTogether(motion2);
			}
			else
			{
				MoveTogether(Vector3.zero);
			}
			break;
		}
		case MonsterState.RandomMove:
		{
			if (changedState)
			{
				AnimaTogether("Walk", loop: true);
				GetRandomMovePoint();
			}
			GetNavInfo(randomMovePoint);
			Vector3 motion = ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed;
			MoveTogether(motion);
			if ((double)(base.transform.position - randomMovePoint).sqrMagnitude < 0.25)
			{
				state = MonsterState.Idle;
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTarget();
				if (base.HaveTarget)
				{
					state = MonsterState.RunToTarget;
				}
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void GetRandomMovePoint()
	{
		Vector3 vector = LevelMgr.Inst.CurrentRoomCtrller.RoomScale;
		randomMovePoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(Random.Range(0f - vector.x, vector.x) / 2f, Random.Range(0f - vector.y, vector.y) / 2f, 0f);
	}

	private void MoveTogether(Vector3 motion)
	{
		SetMove(motion);
		if (mirrors != null)
		{
			for (int i = 1; i < mirrors.Length; i++)
			{
				mirrors[i].SetMove(motion);
			}
		}
	}

	private void AnimaTogether(string animaName, bool loop)
	{
		base.SAnima.AnimationState.SetAnimation(0, animaName, loop);
		if (mirrors != null)
		{
			for (int i = 1; i < mirrors.Length; i++)
			{
				mirrors[i].SAnima.AnimationState.SetAnimation(0, animaName, loop);
			}
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (!isMainBody)
		{
			info.immuneDamage = true;
			info.knockbackForce = Vector3.zero;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (!isMainBody)
		{
			return;
		}
		if (mirrorIntervalTimer >= mirrorInterval.result)
		{
			mirrorIntervalTimer = 0f;
			mirrorInterval.RandomResult();
			needRecycleMirror = true;
		}
		if (mirrors != null)
		{
			for (int i = 1; i < mirrors.Length; i++)
			{
				mirrors[i].myPpt.TakeKnockback(info.knockbackForce);
			}
		}
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeAnnouncedDeath_Dots(ref info);
		if (!isMainBody)
		{
			info.stopAnnouncedDeath = true;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (!isMainBody || mirrors == null)
		{
			return;
		}
		for (int i = 1; i < mirrors.Length; i++)
		{
			if (mirrors[i].gameObject.activeSelf)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster30_Disappear" + (GameMgr.IsHarmony_Static ? " H" : ""), mirrors[i].transform.position, 2f);
				LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(mirrors[i].myPpt.myEntity);
				mirrors[i].gameObject.SetActive(value: false);
			}
		}
	}

	public void MaskAsMirror()
	{
		isMainBody = false;
	}

	public void SetMirrorStart(int index, float offset)
	{
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
		base.SAnima.AnimationState.SetAnimation(0, "Mirror", loop: false);
		startPoint = base.transform.position;
		endPoint = base.transform.position + mirrorSpeed * mirrorTime * Tool2D.GetDir(offset + (float)(360 / mirrorCount * index));
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme6_Chapter3 && LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			endPoint = Tool2D.GetNavMeshPointIngoreZ(endPoint);
		}
	}

	public void SetMirrorFinish()
	{
		base.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = true;
		SetComponentData(componentData);
	}
}
