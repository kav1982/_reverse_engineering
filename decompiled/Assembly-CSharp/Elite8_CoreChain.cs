using System.Collections.Generic;
using UnityEngine;

public class Elite8_CoreChain : MonoBehaviour
{
	public GameObject chainPrefab;

	public int childId;

	public List<Elite8_SingleChain> chains;

	public Elite8_Child Core;

	public List<Vector3> ChildrenDirations;

	public List<int> childrenConnection;

	public float radius;

	public Vector3 rotateDiraction;

	public Vector3 diration;

	public float speed;

	public VariableFloat rotateSpeedRange;

	public float rotateSpeed;

	public float radiusSpreadSpeed;

	public float radiusNow;

	public float maxExistTime;

	public float existTimer;

	public bool workOnce;

	public bool isLine;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public void ChildReportDeath()
	{
		Core = null;
	}

	public void Recycle()
	{
		if (Core != null)
		{
			Core.DieWithinBorder();
			Core = null;
		}
	}

	private void Start()
	{
		rotateDiraction = Tool2D.GetDir();
		rotateSpeed = rotateSpeedRange.RandomResult();
		Core = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + childId, base.transform.position + new Vector3(0f, 200f, 0f)).GetComponent<Elite8_Child>();
		Core.core = this;
		Core.group = null;
		if (isLine)
		{
			for (int i = 0; i < 2; i++)
			{
				Elite8_SingleChain component = Object.Instantiate(chainPrefab, base.transform.position, Quaternion.identity, base.transform).GetComponent<Elite8_SingleChain>();
				component.dummy = true;
				if (workOnce)
				{
					component.workOnce = true;
				}
				component.point1 = Core.transform;
				chains.Add(component);
				if (i == 0)
				{
					component.point2.position = base.transform.position + Tool2D.GetDir(diration, 90f).normalized * 30f;
				}
				if (i == 1)
				{
					component.point2.position = base.transform.position - Tool2D.GetDir(diration, 90f).normalized * 30f;
				}
			}
		}
		else
		{
			for (int j = 0; j < 4; j++)
			{
				Elite8_SingleChain component2 = Object.Instantiate(chainPrefab, base.transform.position, Quaternion.identity, base.transform).GetComponent<Elite8_SingleChain>();
				component2.dummy = true;
				if (workOnce)
				{
					component2.workOnce = true;
				}
				component2.point1.position = base.transform.position;
				chains.Add(component2);
				if (j == 0)
				{
					component2.point2.position = base.transform.position + new Vector3(30f, 0f, 0f);
				}
				if (j == 1)
				{
					component2.point2.position = base.transform.position - new Vector3(30f, 0f, 0f);
				}
				if (j == 2)
				{
					component2.point2.position = base.transform.position + new Vector3(0f, 30f, 0f);
				}
				if (j == 3)
				{
					component2.point2.position = base.transform.position - new Vector3(0f, 30f, 0f);
				}
			}
		}
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
	}

	private void Update()
	{
		base.transform.position += diration.normalized * speed * Time.deltaTime;
		if (Core != null)
		{
			Core.transform.position = base.transform.position;
		}
		bool flag = true;
		bool flag2 = true;
		for (int i = 0; i < chains.Count; i++)
		{
			if (chains[i].gameObject.activeSelf)
			{
				flag = false;
				flag2 = false;
			}
			if (Core == null)
			{
				chains[i].StopChain();
			}
			if (chains[i].reportHurt && !chains[i].hasStopped)
			{
				chains[i].StopChain();
			}
		}
		existTimer += Time.deltaTime;
		if (existTimer > maxExistTime && Core != null)
		{
			Core.DieWithinBorder();
			Core = null;
		}
		if (Core != null)
		{
			flag = false;
		}
		if (!isLine && Core != null)
		{
			if (Core.transform.position.x > roomWidth / 2f + roomCenterPoint.x)
			{
				Core.DieWithinBorder();
				Core = null;
			}
			else if (Core.transform.position.x < (0f - roomWidth) / 2f + roomCenterPoint.x)
			{
				Core.DieWithinBorder();
				Core = null;
			}
			else if (Core.transform.position.y > roomHeight / 2f + roomCenterPoint.y)
			{
				Core.DieWithinBorder();
				Core = null;
			}
			else if (Core.transform.position.y < (0f - roomHeight) / 2f + roomCenterPoint.y)
			{
				Core.DieWithinBorder();
				Core = null;
			}
		}
		if (flag2 && Core != null)
		{
			Core.DieWithinBorder();
			Core = null;
		}
		if (flag)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
