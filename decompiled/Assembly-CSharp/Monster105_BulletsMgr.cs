using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster105_BulletsMgr : MonoBehaviour
{
	private struct RotatedBullet
	{
		public GameObject bullet;

		public bool isAlive;

		public Vector3 direction;
	}

	public Vector3 bulletDir;

	public float bulletSpeed = 4f;

	public float rotationSpeed = 5f;

	public float expandSpeed;

	public float expandCounter;

	public float radius = 1.4f;

	public float bulletSpacingSquare = 0.2f;

	public float bulletSpacingCross = 0.3f;

	public float bulletDisTriangle = 1.2f;

	public int type;

	private List<RotatedBullet> rotatedBullets = new List<RotatedBullet>();

	private float currentAngle;

	public void Init(Vector3 dir, int type, List<GameObject> bullets, Vector3 center)
	{
		currentAngle = 0f;
		bulletDir = dir;
		rotatedBullets.Clear();
		expandCounter = 0f;
		this.type = type;
		switch (type)
		{
		case 0:
		{
			float num2 = 360f / (float)bullets.Count;
			for (int l = 0; l < bullets.Count; l++)
			{
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[l],
					isAlive = true,
					direction = Tool2D.GetDir(Vector3.up, (float)l * num2) * radius
				});
			}
			break;
		}
		case 1:
		{
			rotatedBullets.Add(new RotatedBullet
			{
				bullet = bullets[0],
				isAlive = true,
				direction = Tool2D.IgnoreZPoint(center - base.transform.position)
			});
			for (int m = 1; m < (bullets.Count - 1) / 4 + 1; m++)
			{
				Vector3 vector7 = center + new Vector3(0f, (float)m * bulletSpacingSquare, 0f);
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[m],
					isAlive = true,
					direction = Tool2D.IgnoreZPoint(vector7 - base.transform.position)
				});
			}
			for (int n = 1; n < (bullets.Count - 1) / 4 + 1; n++)
			{
				Vector3 vector8 = center - new Vector3(0f, (float)n * bulletSpacingSquare, 0f);
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[n + (bullets.Count - 1) / 4],
					isAlive = true,
					direction = Tool2D.IgnoreZPoint(vector8 - base.transform.position)
				});
			}
			for (int num3 = 1; num3 < (bullets.Count - 1) / 4 + 1; num3++)
			{
				Vector3 vector9 = center + new Vector3((float)num3 * bulletSpacingSquare, 0f, 0f);
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[num3 + (bullets.Count - 1) / 2],
					isAlive = true,
					direction = Tool2D.IgnoreZPoint(vector9 - base.transform.position)
				});
			}
			for (int num4 = 1; num4 < (bullets.Count - 1) / 4 + 1; num4++)
			{
				Vector3 vector10 = center - new Vector3((float)num4 * bulletSpacingSquare, 0f, 0f);
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[num4 + (bullets.Count - 1) / 4 * 3],
					isAlive = true,
					direction = Tool2D.IgnoreZPoint(vector10 - base.transform.position)
				});
			}
			break;
		}
		case 2:
		{
			float num5 = bulletSpacingSquare * (float)(bullets.Count / 4);
			for (int num6 = 0; num6 < bullets.Count / 4; num6++)
			{
				Vector3 vector11 = center + new Vector3((float)num6 * bulletSpacingSquare - num5 / 2f, num5 / 2f, 0f);
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[num6],
					isAlive = true,
					direction = Tool2D.IgnoreZPoint(vector11 - base.transform.position)
				});
			}
			for (int num7 = 0; num7 < bullets.Count / 4; num7++)
			{
				Vector3 vector12 = center + new Vector3(0f - ((float)num7 * bulletSpacingSquare - num5 / 2f), (0f - num5) / 2f, 0f);
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[num7 + bullets.Count / 4],
					isAlive = true,
					direction = Tool2D.IgnoreZPoint(vector12 - base.transform.position)
				});
			}
			for (int num8 = 0; num8 < bullets.Count / 4; num8++)
			{
				Vector3 vector13 = center + new Vector3((0f - num5) / 2f, (float)num8 * bulletSpacingSquare - num5 / 2f, 0f);
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[num8 + bullets.Count / 2],
					isAlive = true,
					direction = Tool2D.IgnoreZPoint(vector13 - base.transform.position)
				});
			}
			for (int num9 = 0; num9 < bullets.Count / 4; num9++)
			{
				Vector3 vector14 = center + new Vector3(num5 / 2f, 0f - ((float)num9 * bulletSpacingSquare - num5 / 2f), 0f);
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[num9 + bullets.Count / 4 * 3],
					isAlive = true,
					direction = Tool2D.IgnoreZPoint(vector14 - base.transform.position)
				});
			}
			break;
		}
		case 3:
		{
			Vector3 vector = center + Vector3.up * bulletDisTriangle;
			Vector3 vector2 = center + Tool2D.GetDir(Vector3.up, 120f) * bulletDisTriangle;
			Vector3 vector3 = center + Tool2D.GetDir(Vector3.up, 240f) * bulletDisTriangle;
			float num = Mathf.Sqrt(2f * bulletDisTriangle * bulletDisTriangle * (1f - Mathf.Cos(MathF.PI * 2f / 3f))) / (float)bullets.Count * 3f;
			for (int i = 0; i < bullets.Count / 3; i++)
			{
				Vector3 vector4 = vector + Tool2D.GetDir(Vector3.left, 60f) * num * i;
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[i],
					isAlive = true,
					direction = Tool2D.IgnoreZPoint(vector4 - base.transform.position)
				});
			}
			for (int j = 0; j < bullets.Count / 3; j++)
			{
				Vector3 vector5 = vector3 + Tool2D.GetDir(Vector3.up, 30f) * num * j;
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[j + bullets.Count / 3],
					isAlive = true,
					direction = Tool2D.IgnoreZPoint(vector5 - base.transform.position)
				});
			}
			for (int k = 0; k < bullets.Count / 3; k++)
			{
				Vector3 vector6 = vector2 + new Vector3(num * (float)k, 0f, 0f);
				rotatedBullets.Add(new RotatedBullet
				{
					bullet = bullets[k + bullets.Count / 3 * 2],
					isAlive = true,
					direction = Tool2D.IgnoreZPoint(vector6 - base.transform.position)
				});
			}
			break;
		}
		}
	}

	private void Update()
	{
		RotateBullets();
		if (expandCounter >= 1f)
		{
			base.transform.Translate(bulletDir * bulletSpeed * Time.deltaTime);
		}
	}

	private void RotateBullets()
	{
		bool flag = true;
		currentAngle += rotationSpeed * Time.deltaTime;
		if (currentAngle >= 360f)
		{
			currentAngle -= 360f;
		}
		for (int i = 0; i < rotatedBullets.Count; i++)
		{
			if (!rotatedBullets[i].bullet.activeSelf)
			{
				RotatedBullet value = rotatedBullets[i];
				value.isAlive = false;
				rotatedBullets[i] = value;
			}
			if (rotatedBullets[i].isAlive)
			{
				flag = false;
			}
			if (expandCounter < 1f)
			{
				expandCounter += Time.deltaTime * expandSpeed;
			}
			Vector3 dir = Tool2D.GetDir(rotatedBullets[i].direction, currentAngle);
			Vector3 position = base.transform.position + dir * expandCounter;
			rotatedBullets[i].bullet.transform.position = position;
		}
		if (flag)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}
}
