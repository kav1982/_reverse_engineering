using UnityEngine;

public class Boss51_BurstFire : MonoBehaviour
{
	private Boss51 master;

	private Vector3 startPoint;

	private Vector3 endPoint;

	private Vector3 direction;

	public float distanceInterval;

	public float timeInterval;

	public float maxDistance;

	private float distance;

	private float nowTime;

	private float nowDistance;

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
				Vector3 v = startPoint + direction * nowDistance;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss51_SingleBurstFire", Tool2D.IgnoreZPoint(v), 5f);
			}
		}
		if (stopSummon)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}
}
