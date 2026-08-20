using System.Collections.Generic;
using UnityEngine;

public class Monster24_List : MonoBehaviour
{
	public List<Monster24> leftList;

	public List<Monster24> rightList;

	public List<Monster24> topList;

	public List<Monster24> bottomList;

	public float randomDelta;

	public float repositionDistance = 1.8f;

	public float repositionDistanceMobile = 3f;

	public float randomDeltaChangeInterval = 3f;

	private float randomDeltaTimer;

	private void Start()
	{
	}

	public void AskToInsert(List<Monster24> listToInsert, Monster24 monster)
	{
		int index = 0;
		for (int i = 0; i < listToInsert.Count; i++)
		{
			if (listToInsert == leftList || listToInsert == rightList)
			{
				if (!(listToInsert[i].transform.position.y < monster.transform.position.y))
				{
					break;
				}
				index = i + 1;
			}
			if (listToInsert == topList || listToInsert == bottomList)
			{
				if (!(listToInsert[i].transform.position.x < monster.transform.position.x))
				{
					break;
				}
				index = i + 1;
			}
		}
		listToInsert.Insert(index, monster);
	}

	private void Update()
	{
		randomDelta = 0f;
	}
}
