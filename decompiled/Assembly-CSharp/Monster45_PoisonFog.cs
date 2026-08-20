using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster45_PoisonFog : MonoBehaviour
{
	public enum FogState
	{
		Damage,
		Fade
	}

	[Header("伤害和判定")]
	public float attackTime;

	public float attackRadius;

	public float attackInterval;

	public List<Entity> attackEntities = new List<Entity>();

	public StateVariableMgr varMgr = new StateVariableMgr();

	public FogState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public FogState state
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

	private void OnEnable()
	{
		state = FogState.Damage;
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
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case FogState.Damage:
		{
			ref float reference = ref varMgr.RegFloat(0);
			_ = changedState;
			if (stateExistTime > attackTime)
			{
				state = FogState.Fade;
			}
			reference += Time.deltaTime;
			if (reference > attackInterval)
			{
				reference = 0f;
				Attack();
			}
			break;
		}
		case FogState.Fade:
			if (stateExistTime > 2f)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	private void Attack()
	{
		attackEntities.Clear();
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position + new Vector3(0f, 0f, 0f), attackRadius, GameConst.Filter_Friendly, list);
		foreach (UnitDotsSyncSystem.DistanceHitResult item in list)
		{
			attackEntities.Add(item.entity);
		}
		foreach (Entity attackEntity in attackEntities)
		{
			UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(attackEntity);
			if (attackEntity == PlayerMgr.Inst.PlayerEtt)
			{
				componentData.SetVenom(3f, 5f);
			}
			else
			{
				componentData.SetVenom(3f, 20f);
			}
			UnitDotsSyncSystem.SetComponentData(componentData, attackEntity);
		}
	}
}
