using UnityEngine;

public class BezierCurvePointData
{
	public Vector3 currentPosition = Vector3.zero;

	public Vector3 pointShiftDir = Vector3.zero;

	public float pointShiftSpeed;

	public Vector3 targetLerpPoint = Vector3.zero;

	public float moveLerpSpeed;

	public float percentInRange;

	public void Type1PointShift()
	{
		currentPosition += pointShiftDir * pointShiftSpeed * Time.deltaTime;
	}

	public void Type2PointLerp()
	{
		currentPosition = Vector3.Lerp(currentPosition, targetLerpPoint, moveLerpSpeed * Time.deltaTime);
	}

	public void Type2SetAndLerpToTargetPoint(Vector3 startPoint, Vector3 endPoint)
	{
		targetLerpPoint = startPoint + (endPoint - startPoint) * percentInRange;
		currentPosition = Vector3.Lerp(currentPosition, targetLerpPoint, moveLerpSpeed * Time.deltaTime);
	}
}
