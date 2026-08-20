using UnityEngine;

public class Elite12_LineMeteor : MonoBehaviour
{
	private Elite12_2 master;

	private Vector3 startPoint;

	private Vector3 endPoint;

	private Vector3 direction;

	public float distanceInterval;

	public float timeInterval;

	public float maxDistance;

	public float pointOffset;

	public LayerMask attackMask;

	private float distance;

	private float nowTime;

	private float nowDistance;

	public void Initialize(Vector3 startPoint, Vector3 direction)
	{
		master = Elite12_2.Inst;
		this.direction = direction.normalized;
		this.startPoint = Tool2D.IgnoreZPoint(startPoint);
		UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.RayCastAll(startPoint, direction, maxDistance, GameConst.Filter_Wall);
		endPoint = startPoint + direction.normalized * maxDistance;
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = true;
			for (int j = 0; j < Elite12_1.Inst.rocks.Count; j++)
			{
				if (Elite12_1.Inst.rocks[i].thisEntity == array[i].entity)
				{
					flag = false;
				}
			}
			if (flag)
			{
				endPoint = array[i].point;
				break;
			}
		}
		endPoint = Tool2D.IgnoreZPoint(endPoint);
		distance = (endPoint - this.startPoint).magnitude;
		nowTime = 0f;
		nowDistance = 0f;
	}

	private void Update()
	{
		Debug.DrawLine(startPoint, endPoint);
		nowTime += Time.deltaTime;
		if (nowTime > timeInterval)
		{
			nowTime -= timeInterval;
			nowDistance += distanceInterval;
			if (nowDistance > distance || master.myPpt.AlreadyDead)
			{
				Elite12_1.MiniPool.RecycleGO(base.gameObject);
			}
			else
			{
				master.ShootSingleMeteor(startPoint + direction * nowDistance + new Vector3(0f, 0f, 0f - master.meteoriteHeight) + Tool2D.GetDir() * pointOffset);
			}
		}
	}
}
