using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;

public class SpecialObj208 : SpecialObj205
{
	public GameObject pfb_Matrix;

	public int widthCount;

	public int heightCount;

	private new void Start()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
		int[] array = new int[widthCount];
		for (int i = 0; i < array.Length; i++)
		{
			if (i % 2 == 0)
			{
				array[i] = Random.Range(0, heightCount);
			}
		}
		for (int j = 0; j < widthCount; j++)
		{
			List<int> list = new List<int>();
			if (j % 2 == 1)
			{
				if (array[j - 1] < array[j + 1])
				{
					for (int k = array[j - 1]; k <= array[j + 1]; k++)
					{
						list.Add(k);
					}
				}
				else
				{
					for (int l = array[j + 1]; l <= array[j - 1]; l++)
					{
						list.Add(l);
					}
				}
			}
			for (int m = 0; m < heightCount; m++)
			{
				SpecialObj208Matrix component = Object.Instantiate(pfb_Matrix, belongRoom.CenterPoint + new Vector3((float)(-widthCount) / 2f + 0.5f, (float)(-heightCount) / 2f + 0.5f) + new Vector3(j, m, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj208Matrix>();
				belongRoom.TrapRegister(component);
				if (j % 2 == 0)
				{
					if (m == array[j])
					{
						component.SetRight();
					}
				}
				else if (list.Contains(m))
				{
					component.SetRight();
				}
			}
		}
	}
}
