using UnityEngine;

public class NavInfo
{
	public Vector3[] corners;

	public int currentCornerIndex;

	public float moveThreshold;

	public bool allCornerArrived;

	public Vector3 ToGoPoint
	{
		get
		{
			if (currentCornerIndex >= corners.Length)
			{
				return Vector3.zero;
			}
			return corners[currentCornerIndex];
		}
	}
}
