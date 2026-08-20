using UnityEngine;

public class ChainTentacleSpawnData
{
	public Vector3 startPoint;

	public Vector3 endPoint;

	public Vector3 currentPoint;

	public Vector3 moveDir;

	public Vector3 velocity;

	public float remainTime;

	public float currentAngle;

	public ChainTentacleSpawnData(Vector3 sp, Vector3 ep, float speed, float time)
	{
		startPoint = sp;
		currentPoint = sp;
		endPoint = ep;
		moveDir = Tool2D.IgnoreZPoint(ep - sp).normalized;
		velocity = moveDir * speed;
		currentAngle = Random.Range(0f, 360f);
		remainTime = time;
	}
}
