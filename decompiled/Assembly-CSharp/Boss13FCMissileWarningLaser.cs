using UnityEngine;

public class Boss13FCMissileWarningLaser : MonoBehaviour
{
	public Monster9Laser monster9Laser;

	public LineRenderer laserRenderer;

	public float startWidth;

	public float offset;

	public float speed;

	private void OnEnable()
	{
		offset = 0f;
		float num3 = (laserRenderer.startWidth = (laserRenderer.endWidth = startWidth));
	}

	private void Update()
	{
		if (laserRenderer.startWidth > 0f)
		{
			offset += speed * Time.deltaTime;
			float num3 = (laserRenderer.startWidth = (laserRenderer.endWidth = startWidth + offset));
		}
		else
		{
			monster9Laser.Stop();
		}
	}
}
