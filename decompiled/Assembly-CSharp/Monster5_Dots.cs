using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public struct Monster5_Dots : IComponentData, IQueryTypeParameter
{
	private Entity targetEntity;

	public float3 noAroundDir;

	private float aroundCheckIntervalTimer;

	private float attackIntervalTimer;

	public float aroundAdjustmentDistance;

	public float aroundPlayerRadius;

	public float aroundPointDistance;

	public bool isAroundPlayer;

	public float sprintInterval;

	public float sprintTime;

	public float sprintSpeedRatio;

	public float noAroundRotateSpeed;

	public float sprintIntervalTimer;

	public float sprintTimer;

	public bool isSprint;

	public float3 sprintDir;

	public bool isInitialized;

	public Entity aroundEntity;

	public float checkTargetIntervalTimer;

	public Monster5State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public Monster5State state
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
		}
	}

	public Vector3 GetMotion(Vector3 targetPosition, Vector3 position, float aroundDistance, float moveSpeed, PhysicsWorldSingleton pws, ref PathFinding pathFinding)
	{
		Vector3 vector = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(targetPosition, position), 90f) * moveSpeed;
		float num = Vector3.Distance(position, targetPosition);
		bool flag = false;
		if (Mathf.Abs(num - aroundDistance) > aroundAdjustmentDistance)
		{
			flag = true;
			if (num < aroundDistance)
			{
				vector += -Tool2D.IgnoreZV2ToV1Normal(targetPosition, position) * moveSpeed;
			}
			else
			{
				vector += Tool2D.IgnoreZV2ToV1Normal(targetPosition, position) * moveSpeed;
			}
		}
		float num2 = 360f * moveSpeed / (MathF.PI * 2f * aroundDistance);
		if (flag)
		{
			num2 *= 1.414f;
		}
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1u;
		collisionFilter.CollidesWith = 256u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		RaycastInput raycastInput = default(RaycastInput);
		raycastInput.Start = targetPosition;
		raycastInput.End = targetPosition + Mathf.Max(aroundDistance, num) * Tool2D.GetDir(position - targetPosition, (0f - num2) * ((num < aroundDistance) ? 0.5f : 0.1f)).normalized;
		raycastInput.Filter = filter;
		RaycastInput input = raycastInput;
		if (pws.CastRay(input, out var closestHit))
		{
			pathFinding.UpdatePath(position, closestHit.Position, 32, needUpdateNow: true);
			Debug.DrawLine(position, targetPosition, Color.red);
			Debug.DrawLine(position, pathFinding.walkToPoint, Color.red);
			return Tool2D.IgnoreZV2ToV1Normal(pathFinding.walkToPoint, position) * moveSpeed * (flag ? 1.414f : 1f);
		}
		pathFinding.UpdatePath(position, position + vector * 0.1f, 32, needUpdateNow: true);
		Debug.DrawLine(position, targetPosition);
		Debug.DrawLine(position, pathFinding.walkToPoint);
		return Tool2D.IgnoreZV2ToV1Normal(pathFinding.walkToPoint, position) * moveSpeed * (flag ? 1.414f : 1f);
	}

	public void SprintStart(Vector3 sprintDir)
	{
		sprintIntervalTimer = 0f;
		isSprint = true;
		this.sprintDir = sprintDir;
	}

	public void SprintStop()
	{
		sprintTimer = 0f;
		isSprint = false;
	}
}
