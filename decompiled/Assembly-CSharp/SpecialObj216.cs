using System.Collections.Generic;
using UnityEngine;

public class SpecialObj216 : LayerCorrect, IRoomCtrller
{
	[Space(50f)]
	public GameObject pfb_Matrix01;

	public GameObject pfb_Matrix02;

	public Vector2[] securityZoneSize;

	public Vector3[] securityZoneOffset;

	public int[] forward;

	public Vector3 originOffset;

	private float distance;

	private RoomController belongCtrller;

	private List<SpecialObj9> so9s = new List<SpecialObj9>();

	private void Start()
	{
		if (belongCtrller.roomCfg.isFlipped)
		{
			originOffset.x = 0f - originOffset.x;
			for (int i = 0; i < securityZoneOffset.Length; i++)
			{
				securityZoneOffset[i].x = 0f - securityZoneOffset[i].x;
				if (i % 2 == 0)
				{
					forward[i] = -forward[i];
				}
			}
		}
		Vector3 vector = base.transform.position + securityZoneOffset[0];
		while (distance <= Mathf.Abs(securityZoneOffset[0].x - securityZoneOffset[1].x) - securityZoneSize[1].x / 2f)
		{
			SpecialObj216Matrix01 component = Object.Instantiate(pfb_Matrix01, vector + new Vector3((float)forward[0] * distance, (0f - (securityZoneSize[0].y + 1f)) / 2f, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj216Matrix01>();
			SpecialObj216Matrix01 component2 = Object.Instantiate(pfb_Matrix01, vector + new Vector3((float)forward[0] * distance, (securityZoneSize[0].y + 1f) / 2f, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj216Matrix01>();
			component.SetWrong(isWrong: true, base.transform.position + originOffset);
			component2.SetWrong(isWrong: true, base.transform.position + originOffset);
			for (float num = (0f - (securityZoneSize[0].y + 1f)) / 2f + 1f; num <= (securityZoneSize[0].y + 1f) / 2f - 1f; num += 1f)
			{
				if (Random.Range(0, 30) == 0)
				{
					Object.Instantiate(pfb_Matrix01, vector + new Vector3((float)forward[0] * distance, num, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj216Matrix01>().SetWrong(isWrong: true, base.transform.position + originOffset);
				}
			}
			distance += 1f;
		}
		Vector3 position = base.transform.position + securityZoneOffset[securityZoneOffset.Length - 1] / 2f;
		Object.Instantiate(pfb_Matrix02, position, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj216Matrix02>();
		base.transform.position += originOffset;
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}
}
