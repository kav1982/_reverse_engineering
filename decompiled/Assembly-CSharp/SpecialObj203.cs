using System.Collections.Generic;
using UnityEngine;

public class SpecialObj203 : LayerCorrect, IRoomCtrller
{
	[Space(50f)]
	public GameObject pfb_Matrix;

	public Vector3[] coupleCenterPointOffsets;

	public bool[] coupleIsHorizontal;

	public Vector3 destinationOffset;

	public Vector3 originOffset;

	private RoomController belongCtrller;

	private List<SpecialObj203Matrix> buttonList = new List<SpecialObj203Matrix>();

	private void Start()
	{
		if (belongCtrller.roomCfg.isFlipped)
		{
			originOffset.x = 0f - originOffset.x;
			for (int i = 0; i < coupleCenterPointOffsets.Length; i++)
			{
				coupleCenterPointOffsets[i].x = 0f - coupleCenterPointOffsets[i].x;
			}
			destinationOffset.x = 0f - destinationOffset.x;
		}
		for (int j = 0; j < coupleCenterPointOffsets.Length; j++)
		{
			Vector3 vector = base.transform.position + coupleCenterPointOffsets[j];
			if (coupleIsHorizontal[j])
			{
				SpecialObj203Matrix component = Object.Instantiate(pfb_Matrix, vector + new Vector3(-1f, 0f, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj203Matrix>();
				SpecialObj203Matrix component2 = Object.Instantiate(pfb_Matrix, vector + new Vector3(1f, 0f, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj203Matrix>();
				if (Random.Range(0, 2) == 0)
				{
					component.SetWrong(isWrong: true, base.transform.position + originOffset);
				}
				else
				{
					component2.SetWrong(isWrong: true, base.transform.position + originOffset);
				}
				buttonList.Add(component);
				buttonList.Add(component2);
			}
			else
			{
				SpecialObj203Matrix component3 = Object.Instantiate(pfb_Matrix, vector + new Vector3(0f, -1f, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj203Matrix>();
				SpecialObj203Matrix component4 = Object.Instantiate(pfb_Matrix, vector + new Vector3(0f, 1f, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj203Matrix>();
				if (Random.Range(0, 2) == 0)
				{
					component3.SetWrong(isWrong: true, base.transform.position + originOffset);
				}
				else
				{
					component4.SetWrong(isWrong: true, base.transform.position + originOffset);
				}
				buttonList.Add(component3);
				buttonList.Add(component4);
			}
		}
		foreach (Transform item in belongCtrller.tsf_Thing.transform)
		{
			if (item.gameObject.GetComponent<SpecialObj205>() != null)
			{
				item.gameObject.GetComponent<SpecialObj205>().OnGameClear += GameEnd;
				break;
			}
		}
		base.transform.position += originOffset;
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}

	public void GameEnd()
	{
		foreach (SpecialObj203Matrix button in buttonList)
		{
			button.GameEnd();
		}
	}
}
