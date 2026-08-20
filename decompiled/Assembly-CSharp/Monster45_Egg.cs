using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster45_Egg : UnitBase
{
	public enum MonsterState
	{
		flying,
		sliding,
		landed
	}

	public float flyUpForce;

	public float flyGravity;

	public float bounceVelocityReduce;

	public int bounceTime;

	private Vector3 flySpeed;

	public float FogRadius;

	public float lifetime;

	public GameObject Sprite;

	public Vector3 landPoint;

	public Vector3 StartPosition;

	public float poisonInterval;

	private List<Entity> AttackUnit = new List<Entity>();

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private ParticleSystem particleTrail;

	private ParticleSystem particleExplosion;

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

	private void Rotate()
	{
		if (StartPosition.x < landPoint.x)
		{
			Sprite.transform.Rotate(base.transform.rotation.eulerAngles + new Vector3(0f, 0f, -10f + Time.deltaTime));
		}
		else
		{
			Sprite.transform.Rotate(base.transform.root.eulerAngles + new Vector3(0f, 0f, 10f + Time.deltaTime));
		}
	}

	private void FogCheckAttack()
	{
		AttackUnit.Clear();
		AttackUnit.Clear();
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, FogRadius, GameConst.Filter_Friendly, list);
		foreach (UnitDotsSyncSystem.DistanceHitResult item in list)
		{
			AttackUnit.Add(item.entity);
		}
		foreach (Entity item2 in AttackUnit)
		{
			UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(item2);
			if (item2 == PlayerMgr.Inst.PlayerEtt)
			{
				componentData.SetVenom(3f, 5f);
			}
			else
			{
				componentData.SetVenom(3f, 20f);
			}
			UnitDotsSyncSystem.SetComponentData(componentData, item2);
		}
	}

	public override void EveryInitialCallback()
	{
		particleExplosion = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster45_PoisonEffect", base.transform.position).GetComponent<ParticleSystem>();
		particleTrail = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster45_PoisonTrail", base.transform.position).GetComponent<ParticleSystem>();
		state = MonsterState.flying;
	}

	public override void Frame1InitialCallback()
	{
		Debug.Log(base.transform.position);
		LocalTransform componentData = GetComponentData<LocalTransform>();
		componentData.Position = base.transform.position;
		SetComponentData(componentData);
		JumpStart_Dots(flyUpForce, flyGravity);
		flySpeed = Tool2D.IgnoreZV2ToV1Normal(landPoint, base.transform.position) * GeneralTool.CannonSpeed(flyUpForce, 0f - base.transform.position.z, flyGravity, Tool2D.IgnoreZDistance(base.transform.position, landPoint));
		PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>();
		componentData2.Linear = flySpeed;
		SetComponentData(componentData2);
	}

	public override void Update()
	{
		base.Update();
		particleTrail.transform.position = base.transform.position;
		particleExplosion.transform.position = base.transform.position;
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
		case MonsterState.flying:
		{
			ref int reference2 = ref varMgr.RegInt(0);
			Rotate();
			if (!(base.transform.position.z > 0f) || !base.isFalling)
			{
				break;
			}
			base.Anima.Play("Land", 0, 0f);
			base.transform.position = Tool2D.IgnoreZPoint(base.transform.position);
			SEMgr.Inst.PlaySE("SE_Monster463_Bounce");
			if (reference2 == 0)
			{
				particleExplosion.Play();
			}
			reference2++;
			if (reference2 >= bounceTime)
			{
				state = MonsterState.landed;
				break;
			}
			JumpRebounce(bounceVelocityReduce);
			if ((Tool2D.GetNavMeshPointIngoreZ(base.transform.position) - Tool2D.IgnoreZPoint(base.transform.position)).sqrMagnitude > 0.04f)
			{
				Die();
			}
			break;
		}
		case MonsterState.landed:
		{
			if (changedState)
			{
				particleTrail.Stop();
				JumpStop_Dots();
				PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
				componentData.Linear = flySpeed;
				SetComponentData(componentData);
			}
			ref float reference = ref varMgr.RegFloat(0);
			reference += Time.deltaTime;
			if (reference > poisonInterval)
			{
				reference = 0f;
				FogCheckAttack();
			}
			if (stateExistTime >= lifetime)
			{
				Die();
			}
			break;
		}
		}
	}

	public void Die()
	{
		DotsAnnouncedDeath();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		particleExplosion.Stop();
		ObjPoolMgr.Inst.RecycleGO(particleTrail.gameObject, 2f);
		ObjPoolMgr.Inst.RecycleGO(particleExplosion.gameObject, 2f);
	}
}
