using System.Collections.Generic;
using UnityEngine;

public class SpecialObj206 : SpecialObj205
{
	public GameObject pfb_Portal;

	public Vector3[] gridPoints;

	public Vector3[] portalOffsetPints;

	public Vector3[] portalOffsetPintsGrid0;

	public Vector3[] finshportal;

	private List<SpecialObj206Portal> portals = new List<SpecialObj206Portal>();

	private new void Start()
	{
		base.Start();
		if (belongRoom.roomCfg.isFlipped)
		{
			for (int i = 0; i < gridPoints.Length; i++)
			{
				gridPoints[i].x = 0f - gridPoints[i].x;
			}
			for (int j = 0; j < portalOffsetPintsGrid0.Length; j++)
			{
				portalOffsetPintsGrid0[j].x = 0f - portalOffsetPintsGrid0[j].x;
			}
			for (int k = 0; k < finshportal.Length; k++)
			{
				finshportal[k].x = 0f - finshportal[k].x;
			}
		}
		Vector3 vector = belongRoom.CenterPoint + gridPoints[0];
		List<int> list = new List<int> { 1, 2, 3, 4 };
		list.Upset();
		list.Insert(0, 0);
		list.Add(5);
		int num = Random.Range(0, 2);
		for (int l = 0; l < portalOffsetPintsGrid0.Length; l++)
		{
			Vector3 next = vector;
			if (l == num)
			{
				next = belongRoom.CenterPoint + gridPoints[list[1]];
			}
			SpecialObj206Portal component = Object.Instantiate(pfb_Portal, vector + portalOffsetPintsGrid0[l], Quaternion.identity, base.transform.parent).GetComponent<SpecialObj206Portal>();
			component.Initialize(next);
			portals.Add(component);
		}
		for (int m = 1; m < 5; m++)
		{
			num = Random.Range(0, 4);
			for (int n = 0; n < portalOffsetPints.Length; n++)
			{
				Vector3 next2 = vector;
				if (n == num)
				{
					next2 = belongRoom.CenterPoint + gridPoints[list[m + 1]];
				}
				SpecialObj206Portal component2 = Object.Instantiate(pfb_Portal, belongRoom.CenterPoint + gridPoints[list[m]] + portalOffsetPints[n], Quaternion.identity, base.transform.parent).GetComponent<SpecialObj206Portal>();
				component2.Initialize(next2);
				portals.Add(component2);
			}
		}
	}

	public override void SO205PlayerEntered()
	{
		base.SO205PlayerEntered();
		for (int i = 0; i < portals.Count; i++)
		{
			portals[i].gameObject.SetActive(value: false);
		}
		Object.Instantiate(pfb_Portal, belongRoom.CenterPoint + gridPoints[5] + finshportal[0], Quaternion.identity, base.transform.parent).GetComponent<SpecialObj206Portal>().Initialize(belongRoom.CenterPoint + gridPoints[0]);
		Object.Instantiate(pfb_Portal, belongRoom.CenterPoint + gridPoints[0] + finshportal[1], Quaternion.identity, base.transform.parent).GetComponent<SpecialObj206Portal>().Initialize(belongRoom.CenterPoint + gridPoints[5]);
	}
}
