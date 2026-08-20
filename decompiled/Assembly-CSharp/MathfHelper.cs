using UnityEngine;

public static class MathfHelper
{
	public static float MoveTowardsAngleCounterClockWise(this Mathf mf, float current, float target, float maxDelta)
	{
		float num = Mathf.Abs(Mathf.DeltaAngle(current, target));
		if (0f - maxDelta < num && num < maxDelta)
		{
			return target;
		}
		target = current + num;
		return Mathf.MoveTowards(current, target, maxDelta);
	}

	public static (float a, float b) MoveTowardsAngleCounterClockWiseReTurn2Angle(this Mathf mf, float current, float target, float maxDelta)
	{
		float num = Mathf.Abs(Mathf.DeltaAngle(current, target));
		if (0f - maxDelta < num && num < maxDelta)
		{
			return (current, target);
		}
		target = current + num;
		return (current, target);
	}

	public static float MoveTowardsAngleClockWise(this Mathf mf, float current, float target, float maxDelta)
	{
		float num = 0f - Mathf.Abs(Mathf.DeltaAngle(current, target));
		if (0f - maxDelta < num && num < maxDelta)
		{
			return target;
		}
		target = current + num;
		return Mathf.MoveTowards(current, target, maxDelta);
	}

	public static (float a, float b) MoveTowardsAngleClockWiseReTurn2Angle(this Mathf mf, float current, float target, float maxDelta)
	{
		float num = 0f - Mathf.Abs(Mathf.DeltaAngle(current, target));
		if (0f - maxDelta < num && num < maxDelta)
		{
			return (current, target);
		}
		target = current + num;
		return (current, target);
	}

	public static Vector3 RotateZ(this Vector3 dir, float angle)
	{
		return Quaternion.Euler(0f, 0f, angle) * dir;
	}
}
