using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class SolidObj1 : UnitBase, IRoomObjExtraData, ITrap, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	private enum MoveDir
	{
		NoMotion,
		Up,
		Right,
		Down,
		Left
	}

	[Space(50f)]
	public float knockbackForce;

	public float attackInterval;

	[Header("Rotate")]
	public Transform tsf_Model;

	public float rotateSpeed;

	[Header("碰撞体")]
	public UnityEngine.BoxCollider thisCollider;

	private MoveDir moveDir;

	private MoveDir forceDir;

	private SpecialObj7 so7_GoTrack;

	private List<Entity> attackedCollider = new List<Entity>();

	private List<float> attackedIntervals = new List<float>();

	public Entity thisEntity { get; set; }

	public unsafe override void EveryInitialCallback()
	{
		attackedCollider.Clear();
		attackedIntervals.Clear();
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 131072u;
		collisionFilter.CollidesWith = DTool.GetCollidesWith(131072u) | 0x40000u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter collisionFilter2 = collisionFilter;
		PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
		componentData.ColliderPtr->SetCollisionFilter(collisionFilter2);
		SetComponentData(componentData);
	}

	public override void Frame1InitialCallback()
	{
		UnityEngine.Collider nearestColliderByTag = GeneralTool.GetNearestColliderByTag(base.transform.position, 1f, "SpikesTrack");
		if (nearestColliderByTag == null)
		{
			moveDir = MoveDir.NoMotion;
			return;
		}
		so7_GoTrack = nearestColliderByTag.GetComponent<SpecialObj7>();
		base.transform.position = so7_GoTrack.transform.position;
		SyncDotsPosition();
		if (!so7_GoTrack.IsInitialized)
		{
			so7_GoTrack.Initialize();
		}
		if (forceDir != 0)
		{
			moveDir = forceDir;
		}
		else if (so7_GoTrack.UpTrack != null)
		{
			moveDir = MoveDir.Up;
		}
		else if (so7_GoTrack.RightTrack != null)
		{
			moveDir = MoveDir.Right;
		}
		else if (so7_GoTrack.DownTrack != null)
		{
			moveDir = MoveDir.Down;
		}
		else if (so7_GoTrack.LeftTrack != null)
		{
			moveDir = MoveDir.Left;
		}
		else
		{
			moveDir = MoveDir.NoMotion;
		}
	}

	public override void Update()
	{
		base.Update();
		tsf_Model.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
		for (int num = attackedCollider.Count - 1; num >= 0; num--)
		{
			attackedIntervals[num] -= Time.deltaTime;
			if (attackedIntervals[num] <= 0f)
			{
				attackedIntervals.RemoveAt(num);
				attackedCollider.RemoveAt(num);
			}
		}
		if (moveDir == MoveDir.NoMotion)
		{
			return;
		}
		float num2 = base.MoveSpeed * Time.deltaTime;
		if ((base.transform.position - so7_GoTrack.transform.position).sqrMagnitude <= num2 * num2)
		{
			base.transform.position = so7_GoTrack.transform.position;
			switch (moveDir)
			{
			case MoveDir.Up:
				if (so7_GoTrack.UpTrack != null)
				{
					so7_GoTrack = so7_GoTrack.UpTrack;
				}
				else if (so7_GoTrack.RightTrack != null)
				{
					so7_GoTrack = so7_GoTrack.RightTrack;
					moveDir = MoveDir.Right;
				}
				else if (so7_GoTrack.LeftTrack != null)
				{
					so7_GoTrack = so7_GoTrack.LeftTrack;
					moveDir = MoveDir.Left;
				}
				else
				{
					so7_GoTrack = so7_GoTrack.DownTrack;
					moveDir = MoveDir.Down;
				}
				break;
			case MoveDir.Right:
				if (so7_GoTrack.RightTrack != null)
				{
					so7_GoTrack = so7_GoTrack.RightTrack;
				}
				else if (so7_GoTrack.DownTrack != null)
				{
					so7_GoTrack = so7_GoTrack.DownTrack;
					moveDir = MoveDir.Down;
				}
				else if (so7_GoTrack.UpTrack != null)
				{
					so7_GoTrack = so7_GoTrack.UpTrack;
					moveDir = MoveDir.Up;
				}
				else
				{
					so7_GoTrack = so7_GoTrack.LeftTrack;
					moveDir = MoveDir.Left;
				}
				break;
			case MoveDir.Down:
				if (so7_GoTrack.DownTrack != null)
				{
					so7_GoTrack = so7_GoTrack.DownTrack;
				}
				else if (so7_GoTrack.LeftTrack != null)
				{
					so7_GoTrack = so7_GoTrack.LeftTrack;
					moveDir = MoveDir.Left;
				}
				else if (so7_GoTrack.RightTrack != null)
				{
					so7_GoTrack = so7_GoTrack.RightTrack;
					moveDir = MoveDir.Right;
				}
				else
				{
					so7_GoTrack = so7_GoTrack.UpTrack;
					moveDir = MoveDir.Up;
				}
				break;
			case MoveDir.Left:
				if (so7_GoTrack.LeftTrack != null)
				{
					so7_GoTrack = so7_GoTrack.LeftTrack;
				}
				else if (so7_GoTrack.UpTrack != null)
				{
					so7_GoTrack = so7_GoTrack.UpTrack;
					moveDir = MoveDir.Up;
				}
				else if (so7_GoTrack.DownTrack != null)
				{
					so7_GoTrack = so7_GoTrack.DownTrack;
					moveDir = MoveDir.Down;
				}
				else
				{
					so7_GoTrack = so7_GoTrack.RightTrack;
					moveDir = MoveDir.Right;
				}
				break;
			default:
				Debug.LogError(moveDir);
				break;
			}
		}
		else
		{
			base.transform.position += (so7_GoTrack.transform.position - base.transform.position).normalized * num2;
		}
		SyncDotsPosition();
	}

	private void OnTriggerStay(UnityEngine.Collider other)
	{
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 <= 2f)
		{
			if (data1 != 1f)
			{
				if (data1 == 2f)
				{
					forceDir = MoveDir.Right;
				}
			}
			else
			{
				forceDir = MoveDir.Up;
			}
		}
		else if (data1 != 3f)
		{
			if (data1 == 4f)
			{
				forceDir = MoveDir.Left;
			}
		}
		else
		{
			forceDir = MoveDir.Down;
		}
	}

	public void SetTrapInvalid()
	{
		DotsAnnouncedDeath();
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
		if (attackedCollider.Contains(other) || !UnitDotsSyncSystem.EntityIsValid(other))
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
		switch (layer)
		{
		case 262144u:
		{
			attackedCollider.Add(other);
			attackedIntervals.Add(attackInterval);
			PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>(other);
			componentData2.Linear += (float3)ToPointDir(vector) * knockbackForce;
			SetComponentData(componentData2, other);
			break;
		}
		case 512u:
		case 2048u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
			info.damage = 10f;
			info.knockbackForce = (vector - base.transform.position).normalized * knockbackForce;
			info.isTrapDamage = true;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(other);
			if (!componentData.IsFly && !componentData.unitCfg.immuneSpike)
			{
				attackedCollider.Add(other);
				attackedIntervals.Add(attackInterval);
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
				if (!GameMgr.IsHarmony_Static)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DropBlood", vector, 1f);
				}
			}
			break;
		}
		case 8388608u:
		case 16777216u:
		{
			UnitDotsSyncSystem.ProcessHitSpell(other, 999f, out var _);
			break;
		}
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
