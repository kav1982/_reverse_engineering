using System.Collections.Generic;
using UnityEngine;

public class Boss8_BulletsMgr : MonoBehaviour
{
	public class RightAngleBullets
	{
		public SpellBase bullet;

		public float turnHeight;

		public bool isAlive;

		public bool turning;

		public float turnIntervalTimer;
	}

	public List<RightAngleBullets> rightAngleBullets = new List<RightAngleBullets>();

	public VariableFloat turnHeight;

	public VariableFloat xOffset;

	public float bulletSpeed;

	public bool needRecycle;

	public float turnInterval;

	public void Initialized(List<GameObject> bullets, Transform generateXLeft, Transform generateXRight)
	{
		rightAngleBullets.Clear();
		foreach (GameObject bullet in bullets)
		{
			rightAngleBullets.Add(new RightAngleBullets
			{
				bullet = bullet.GetComponent<SpellBase>(),
				turnHeight = turnHeight.RandomResult(),
				isAlive = true,
				turning = false,
				turnIntervalTimer = 1f
			});
		}
		for (int i = 0; i < rightAngleBullets.Count / 2; i++)
		{
			rightAngleBullets[i].bullet.transform.position = new Vector3(generateXLeft.position.x + xOffset.RandomResult(), generateXLeft.position.y + Random.Range(5f, 25f), rightAngleBullets[i].bullet.transform.position.z);
		}
		for (int j = rightAngleBullets.Count / 2; j < rightAngleBullets.Count; j++)
		{
			rightAngleBullets[j].bullet.transform.position = new Vector3(generateXRight.position.x - xOffset.RandomResult(), generateXRight.position.y + Random.Range(5f, 25f), rightAngleBullets[j].bullet.transform.position.z);
		}
	}

	private void Update()
	{
		needRecycle = true;
		for (int i = 0; i < rightAngleBullets.Count / 2; i++)
		{
			if (rightAngleBullets[i].bullet.transform.position.y < rightAngleBullets[i].turnHeight)
			{
				rightAngleBullets[i].turning = true;
			}
			if (rightAngleBullets[i].turning)
			{
				rightAngleBullets[i].turnIntervalTimer += Time.deltaTime;
				if (rightAngleBullets[i].turnIntervalTimer > turnInterval)
				{
					rightAngleBullets[i].turnIntervalTimer = 0f;
					rightAngleBullets[i].bullet.Direction = Tool2D.GetDir(rightAngleBullets[i].bullet.Direction, 15f);
					rightAngleBullets[i].bullet.ApplySpeedToVelocity();
					if (Tool2D.IgnoreZAngleWithSign(Vector3.up, rightAngleBullets[i].bullet.Direction) > -91f)
					{
						rightAngleBullets[i].bullet.Direction = Vector3.right;
						rightAngleBullets[i].bullet.ApplySpeedToVelocity();
						rightAngleBullets[i].bullet.isThroughWall = false;
						rightAngleBullets[i].turning = false;
					}
				}
			}
			if (!rightAngleBullets[i].bullet.gameObject.activeSelf)
			{
				rightAngleBullets[i].isAlive = false;
			}
			if (rightAngleBullets[i].isAlive)
			{
				needRecycle = false;
			}
		}
		for (int j = rightAngleBullets.Count / 2; j < rightAngleBullets.Count; j++)
		{
			if (rightAngleBullets[j].bullet.transform.position.y < rightAngleBullets[j].turnHeight)
			{
				rightAngleBullets[j].turning = true;
			}
			if (rightAngleBullets[j].turning)
			{
				rightAngleBullets[j].turnIntervalTimer += Time.deltaTime;
				if (rightAngleBullets[j].turnIntervalTimer > turnInterval)
				{
					rightAngleBullets[j].turnIntervalTimer = 0f;
					rightAngleBullets[j].bullet.Direction = Tool2D.GetDir(rightAngleBullets[j].bullet.Direction, -15f);
					rightAngleBullets[j].bullet.ApplySpeedToVelocity();
					if (Tool2D.IgnoreZAngleWithSign(Vector3.up, rightAngleBullets[j].bullet.Direction) < 91f)
					{
						rightAngleBullets[j].bullet.Direction = Vector3.left;
						rightAngleBullets[j].bullet.ApplySpeedToVelocity();
						rightAngleBullets[j].bullet.isThroughWall = false;
						rightAngleBullets[j].turning = false;
					}
				}
			}
			if (!rightAngleBullets[j].bullet.gameObject.activeSelf)
			{
				rightAngleBullets[j].isAlive = false;
			}
			if (rightAngleBullets[j].isAlive)
			{
				needRecycle = false;
			}
		}
		if (needRecycle)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}
}
