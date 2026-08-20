using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct SpellMovementComponentData : IComponentData, IQueryTypeParameter
{
	public SpellSpecialMovementType Type;

	public float Speed;

	private float3 _direction;

	public int ReboundCount;

	public float ReboundAddTime;

	public Entity AroundTarget;

	public float3 AroundCenter;

	public float AroundAngle;

	public float AroundRadius;

	public Entity ChaseTarget;

	public float3 ChaseOwnerPosition;

	public float ChaseRotateSpeed;

	public float ChaseMouseLerpSpeed;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsFallSpell;

	public float Gravity;

	public float CurrentFallSpeed;

	public float FallingReboundForceRatio;

	public float3 FallTargetPosition;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsIgnoreWall;

	public float OriginalSpellHorizontalSpeed;

	[MarshalAs(UnmanagedType.U1)]
	public bool IsFallRebounded;

	public float3 Direction
	{
		get
		{
			return _direction;
		}
		set
		{
			if (!float.IsInfinity(value.x) && !float.IsNaN(value.x) && !float.IsInfinity(value.y) && !float.IsNaN(value.y) && (value.x != 0f || value.y != 0f))
			{
				_direction = value;
			}
		}
	}

	public float3 UpdateSelfChasePosition(ComponentLookup<LocalTransform> transformLookup, Entity shooter)
	{
		if (transformLookup.TryGetComponent(shooter, out var componentData, out var entityExists) && entityExists)
		{
			ChaseOwnerPosition = componentData.Position;
		}
		return ChaseOwnerPosition;
	}

	public float3 UpdateAroundFollowAndGetAroundPositionWhenAround(ComponentLookup<LocalTransform> transformLookup)
	{
		if (transformLookup.TryGetComponent(AroundTarget, out var componentData, out var entityExists) && entityExists)
		{
			return UpdateAroundFollowAndGetAroundPositionWhenAround(componentData.Position);
		}
		return UpdateAroundFollowAndGetAroundPositionWhenAround(AroundCenter);
	}

	public float3 UpdateAroundFollowAndGetAroundPositionWhenAround(float3 followTarget)
	{
		AroundCenter = followTarget;
		return AroundCenter + (float3)Tool2D.GetDir(AroundAngle) * AroundRadius;
	}

	public void ReboundFallSpeed()
	{
		CurrentFallSpeed = (0f - math.sqrt(Gravity)) * FallingReboundForceRatio * 1.8f;
	}
}
