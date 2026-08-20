using UnityEngine;

public class Monster10_Leg : MonoBehaviour
{
	private enum LegDir
	{
		None,
		Left,
		Right
	}

	public Transform[] tsf_Legs;

	public float widthScale;

	public float heightScale;

	public float rotateSpeed;

	private LegDir dir;

	private float currentAngle;

	private void Update()
	{
		switch (dir)
		{
		case LegDir.Left:
			currentAngle += rotateSpeed * Time.deltaTime;
			break;
		case LegDir.Right:
			currentAngle -= rotateSpeed * Time.deltaTime;
			break;
		default:
			Debug.LogError(dir);
			break;
		case LegDir.None:
			break;
		}
		if (dir != 0)
		{
			for (int i = 0; i < tsf_Legs.Length; i++)
			{
				Vector3 rootPoint = Tool2D.GetDir((float)(360 / tsf_Legs.Length * i) + currentAngle);
				rootPoint = Tool2D.GetLayerPoint(rootPoint);
				rootPoint.x *= widthScale;
				rootPoint.y *= heightScale;
				rootPoint.z *= 0.01f;
				tsf_Legs[i].localPosition = rootPoint;
			}
		}
	}

	public void SetDir(float x)
	{
		if (x == 0f && dir != 0)
		{
			dir = LegDir.None;
		}
		else if (x > 0f && dir != LegDir.Right)
		{
			dir = LegDir.Right;
		}
		else if (x < 0f && dir != LegDir.Left)
		{
			dir = LegDir.Left;
		}
	}
}
