using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObj37 : MonoBehaviour, IRoomCtrller, IRoomObjExtraData
{
	public LineRenderer lr;

	public Material mat_T8;

	public Material mat_T9;

	public float nodeInterval;

	public float width;

	public float height;

	[Header("SolidObj3")]
	public int solidObj3ID;

	public int solidObj3Count;

	public float solidObjMoveSpeed;

	private List<Vector3> nodes = new List<Vector3>();

	private float roundCorner;

	private RoomController belongRoom;

	private SolidObj3[] solidObj3s;

	private int[] moveToNodeIndex;

	private bool moveReverse;

	private void Start()
	{
		Reshape();
		solidObj3s = new SolidObj3[solidObj3Count];
		moveToNodeIndex = new int[solidObj3Count];
		moveReverse = UnityEngine.Random.Range(0, 2) == 0;
		for (int i = 0; i < solidObj3Count; i++)
		{
			int num = nodes.Count * i / solidObj3Count;
			if (moveReverse)
			{
				moveToNodeIndex[i] = num - 1;
				if (moveToNodeIndex[i] == -1)
				{
					moveToNodeIndex[i] = nodes.Count - 1;
				}
			}
			else
			{
				moveToNodeIndex[i] = num + 1;
				if (moveToNodeIndex[i] == nodes.Count)
				{
					moveToNodeIndex[i] = 0;
				}
			}
			solidObj3s[i] = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + solidObj3ID, nodes[num]).GetComponent<SolidObj3>();
			belongRoom.TrapRegister(solidObj3s[i]);
		}
		if (belongRoom.roomCfg.themeType == RoomThemeType.Theme8_Chapter4)
		{
			lr.material = mat_T8;
		}
		else if (belongRoom.roomCfg.themeType == RoomThemeType.Theme9_Chapter4_2)
		{
			lr.material = mat_T9;
		}
		else
		{
			Debug.LogError(belongRoom.roomCfg.themeType);
		}
	}

	private void Update()
	{
		for (int i = 0; i < solidObj3s.Length; i++)
		{
			if (!(solidObj3s[i] != null) || !solidObj3s[i].gameObject.activeSelf)
			{
				continue;
			}
			solidObj3s[i].transform.position = Vector3.MoveTowards(solidObj3s[i].transform.position, nodes[moveToNodeIndex[i]], solidObjMoveSpeed * Time.deltaTime);
			solidObj3s[i].SyncDotsPositionSafe();
			if (!(solidObj3s[i].transform.position == nodes[moveToNodeIndex[i]]))
			{
				continue;
			}
			if (moveReverse)
			{
				moveToNodeIndex[i]--;
				if (moveToNodeIndex[i] == -1)
				{
					moveToNodeIndex[i] = nodes.Count - 1;
				}
			}
			else
			{
				moveToNodeIndex[i]++;
				if (moveToNodeIndex[i] == nodes.Count)
				{
					moveToNodeIndex[i] = 0;
				}
			}
		}
	}

	private void Reshape()
	{
		width = Mathf.Max(width, 1f);
		height = Mathf.Max(height, 1f);
		roundCorner = Mathf.Min(width / 2f, height / 2f, roundCorner);
		nodes.Clear();
		if (roundCorner == 0f)
		{
			nodes.Add(base.transform.position + new Vector3(width / 2f, height / 2f, 0f));
			nodes.Add(base.transform.position + new Vector3(width / 2f, (0f - height) / 2f, 0f));
			nodes.Add(base.transform.position + new Vector3((0f - width) / 2f, (0f - height) / 2f, 0f));
			nodes.Add(base.transform.position + new Vector3((0f - width) / 2f, height / 2f, 0f));
		}
		else if (width == height && height == roundCorner)
		{
			int num = (int)(2f * roundCorner * MathF.PI / nodeInterval);
			for (int i = 0; i < num; i++)
			{
				nodes.Add(base.transform.position + Tool2D.GetDir(360f / (float)num * (float)i) * roundCorner);
			}
		}
		else if (roundCorner == width / 2f)
		{
			int num2 = (int)(2f * roundCorner * MathF.PI / 2f / nodeInterval);
			for (int j = 0; j < num2; j++)
			{
				nodes.Add(base.transform.position + new Vector3(0f, height / 2f - roundCorner, 0f) + Tool2D.GetDir(90f - 180f / (float)num2 * (float)j) * roundCorner);
			}
			nodes.Add(base.transform.position + new Vector3(width / 2f, height / 2f - roundCorner, 0f));
			for (int k = 0; k < num2; k++)
			{
				nodes.Add(base.transform.position + new Vector3(0f, (0f - height) / 2f + roundCorner, 0f) + Tool2D.GetDir(-90f - 180f / (float)num2 * (float)k) * roundCorner);
			}
			nodes.Add(base.transform.position + new Vector3((0f - width) / 2f, (0f - height) / 2f + roundCorner, 0f));
		}
		else if (roundCorner == height / 2f)
		{
			int num3 = (int)(2f * roundCorner * MathF.PI / 2f / nodeInterval);
			for (int l = 0; l < num3; l++)
			{
				nodes.Add(base.transform.position + new Vector3(width / 2f - roundCorner, 0f, 0f) + Tool2D.GetDir(-180f / (float)num3 * (float)l) * roundCorner);
			}
			nodes.Add(base.transform.position + new Vector3(width / 2f - roundCorner, (0f - height) / 2f, 0f));
			for (int m = 0; m < num3; m++)
			{
				nodes.Add(base.transform.position + new Vector3((0f - width) / 2f + roundCorner, 0f, 0f) + Tool2D.GetDir(-180f - 180f / (float)num3 * (float)m) * roundCorner);
			}
			nodes.Add(base.transform.position + new Vector3((0f - width) / 2f + roundCorner, height / 2f, 0f));
		}
		else
		{
			int num4 = (int)(2f * roundCorner * MathF.PI / 4f / nodeInterval);
			for (int n = 0; n < num4; n++)
			{
				nodes.Add(base.transform.position + new Vector3(width / 2f - roundCorner, height / 2f - roundCorner, 0f) + Tool2D.GetDir(-90f / (float)num4 * (float)n) * roundCorner);
			}
			nodes.Add(base.transform.position + new Vector3(width / 2f, height / 2f - roundCorner, 0f));
			for (int num5 = 0; num5 < num4; num5++)
			{
				nodes.Add(base.transform.position + new Vector3(width / 2f - roundCorner, (0f - height) / 2f + roundCorner, 0f) + Tool2D.GetDir(-90f - 90f / (float)num4 * (float)num5) * roundCorner);
			}
			nodes.Add(base.transform.position + new Vector3(width / 2f - roundCorner, (0f - height) / 2f, 0f));
			for (int num6 = 0; num6 < num4; num6++)
			{
				nodes.Add(base.transform.position + new Vector3((0f - width) / 2f + roundCorner, (0f - height) / 2f + roundCorner, 0f) + Tool2D.GetDir(-180f - 90f / (float)num4 * (float)num6) * roundCorner);
			}
			nodes.Add(base.transform.position + new Vector3((0f - width) / 2f, (0f - height) / 2f + roundCorner, 0f));
			for (int num7 = 0; num7 < num4; num7++)
			{
				nodes.Add(base.transform.position + new Vector3((0f - width) / 2f + roundCorner, height / 2f - roundCorner, 0f) + Tool2D.GetDir(-270f - 90f / (float)num4 * (float)num7) * roundCorner);
			}
			nodes.Add(base.transform.position + new Vector3((0f - width) / 2f + roundCorner, height / 2f, 0f));
		}
		lr.positionCount = nodes.Count + 1;
		for (int num8 = 0; num8 < nodes.Count; num8++)
		{
			lr.SetPosition(num8, Tool2D.IgnoreZPoint(nodes[num8], 1.21f));
		}
		lr.SetPosition(nodes.Count, Tool2D.IgnoreZPoint(nodes[0], 1.21f));
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongRoom = roomCtrller;
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 > 0f)
		{
			width = data1;
		}
		if (data2 > 0f)
		{
			height = data2;
		}
		if (data3 > 0f)
		{
			roundCorner = data3;
		}
	}
}
