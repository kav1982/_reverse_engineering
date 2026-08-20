using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public struct UnitBase_Dots : IComponentData, IQueryTypeParameter
{
	public bool isAutoFlip;

	public float moveLerp;

	public float moveThreshold;

	public Entity ett_AnimaRoot;

	public Entity targetEtt;

	public float checkTargetIntervalTimer;

	public float3 currentMotion;

	public float3 targetMotion;

	public bool shouldChangeFlip;

	public bool currentFlip;

	public Entity ett_Flip;

	public bool baseIsJumping;

	public float baseJumpUpForce;

	public float baseJumpGravity;

	public bool onChapter3Reposition;

	public float3 repositionValue;

	public void SetMove(float3 motion, bool thisTimeShouldFlip = true)
	{
		targetMotion = Tool2D.IgnoreZPoint(motion);
		if (isAutoFlip && thisTimeShouldFlip)
		{
			SetFlip(motion.x < 0f);
		}
	}

	public void SetMoveWithVelocity(ref PhysicsVelocity velocity, ref SpellMovementComponentData move, float3 motion, bool thisTimeShouldFlip = true)
	{
		SetMove(motion * 0.001f, thisTimeShouldFlip);
		bool3 @bool = motion != float3.zero;
		if (@bool.x || @bool.y || @bool.z)
		{
			velocity.Linear = motion;
		}
		move.Direction = math.normalize(velocity.Linear);
	}

	public void SetFlip(bool isFlip)
	{
		shouldChangeFlip = true;
		currentFlip = isFlip;
	}

	public void JumpStart(float upForce, float gravity)
	{
		if (!baseIsJumping)
		{
			baseIsJumping = true;
			baseJumpUpForce = upForce;
			baseJumpGravity = gravity;
		}
	}

	public void JumpStop()
	{
		if (baseIsJumping)
		{
			baseIsJumping = false;
			baseJumpUpForce = 0f;
			baseJumpGravity = 0f;
		}
	}
}
