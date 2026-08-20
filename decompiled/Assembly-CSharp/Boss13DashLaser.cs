using UnityEngine;

public class Boss13DashLaser : MonoBehaviour
{
	public Monster9Laser monster9Laser;

	public Vector3 currentDir;

	public Vector3 targetDir;

	public float rotateSpeed;

	public Vector3 startPoint;

	public Vector3 endPoint;

	public float prepareTime;

	public float prepareTimer;

	public float chargeTime;

	public float chargeTimer;

	public float duration;

	public float durationTimer;

	public bool singleDir;

	public Vector3 targetPos;

	public float moveSpeed;

	private void OnEnable()
	{
		chargeTimer = 0f;
		durationTimer = 0f;
		prepareTimer = 0f;
	}

	private void Update()
	{
		prepareTimer += Time.deltaTime;
		if (singleDir)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, targetPos, moveSpeed * Time.deltaTime);
		}
		if (prepareTimer > prepareTime)
		{
			currentDir = Vector3.Slerp(currentDir, targetDir, rotateSpeed * Time.deltaTime);
			chargeTimer += Time.deltaTime;
			SetWarning();
		}
		if (chargeTimer > chargeTime)
		{
			if (singleDir)
			{
				monster9Laser.SetLaser(base.transform.position, startPoint);
			}
			else
			{
				monster9Laser.SetLaser(startPoint, endPoint);
			}
			durationTimer += Time.deltaTime;
			if (durationTimer > duration)
			{
				monster9Laser.Stop();
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
	}

	public void SetWarning()
	{
		UnitDotsSyncSystem.Raycast(base.transform.position, currentDir, 999f, GameConst.Filter_Wall, out var result);
		UnitDotsSyncSystem.Raycast(base.transform.position, -currentDir, 999f, GameConst.Filter_Wall, out var result2);
		startPoint = result.point;
		endPoint = result2.point;
		if (singleDir)
		{
			monster9Laser.SetWarning(base.transform.position, startPoint);
		}
		else
		{
			monster9Laser.SetWarning(startPoint, endPoint);
		}
	}
}
