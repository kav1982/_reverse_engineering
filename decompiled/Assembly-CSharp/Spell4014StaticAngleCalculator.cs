using UnityEngine;

public static class Spell4014StaticAngleCalculator
{
	public static float OveralAnlge = 0f;

	public static float RotateSpeed = 180f;

	public static bool UpdatedAngleInThisFrame = false;

	public static void UpdateOveralAngle()
	{
		if (!UpdatedAngleInThisFrame)
		{
			OveralAnlge += RotateSpeed * Time.deltaTime;
			UpdatedAngleInThisFrame = true;
		}
	}
}
