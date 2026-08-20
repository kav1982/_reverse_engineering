using System.Collections.Generic;
using UnityEngine;

public class Boss51_LineFire : MonoBehaviour
{
	private Boss51 master;

	private Vector3 startPoint;

	private Vector3 endPoint;

	private Vector3 direction;

	public float distanceInterval;

	public float timeInterval;

	public float maxDistance;

	public float pointOffset;

	public float warningRadius;

	public float createDelayTime;

	private float distance;

	private float nowTime;

	private float nowDistance;

	private List<float> delayTimes = new List<float>();

	private List<Vector3> delayPoints = new List<Vector3>();

	private bool stopSummon;

	private bool hitCliff;

	public void Initialize(Vector3 startPoint, Vector3 direction)
	{
		master = Boss51.Inst;
		this.direction = direction.normalized;
		this.startPoint = Tool2D.IgnoreZPoint(startPoint);
		endPoint = startPoint + direction.normalized * maxDistance;
		if (UnitDotsSyncSystem.Raycast(startPoint, direction, maxDistance, GameConst.Filter_Border, out var result))
		{
			endPoint = result.point;
		}
		endPoint = Tool2D.IgnoreZPoint(endPoint);
		distance = (endPoint - this.startPoint).magnitude;
		nowTime = timeInterval;
		nowDistance = 0f;
		stopSummon = false;
		hitCliff = false;
	}

	private void Update()
	{
		Debug.DrawLine(startPoint, endPoint);
		nowTime += Time.deltaTime;
		if (nowTime > timeInterval)
		{
			nowTime -= timeInterval;
			nowDistance += distanceInterval;
			if (hitCliff || master.myPpt.AlreadyDead)
			{
				stopSummon = true;
			}
			else
			{
				if (nowDistance > distance)
				{
					hitCliff = true;
				}
				delayTimes.Add(createDelayTime);
				Vector3 vector = startPoint + direction * nowDistance + Tool2D.GetDir() * Random.Range(0f, pointOffset);
				delayPoints.Add(vector);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss51_LineFireWarning", Tool2D.IgnoreZPoint(vector), 5f);
			}
		}
		if (stopSummon && delayPoints.Count == 0)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		for (int num = delayPoints.Count - 1; num >= 0; num--)
		{
			delayTimes[num] -= Time.deltaTime;
			if (delayTimes[num] < 0f)
			{
				master.CreateGroundFire(delayPoints[num], isLineFire: true);
				delayTimes.RemoveAt(num);
				delayPoints.RemoveAt(num);
			}
		}
	}
}
