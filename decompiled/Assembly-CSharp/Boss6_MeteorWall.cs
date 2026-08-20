using System.Collections.Generic;
using UnityEngine;

public class Boss6_MeteorWall : MonoBehaviour
{
	public enum wallState
	{
		Rest,
		Left,
		Right,
		Both
	}

	[Header("单侧随机彗星")]
	public float singleMeteorInterval;

	[Header("双重彗星墙")]
	public float meteorWallSingleWidth;

	public int wallHoleCount;

	public int wallHoleWidth;

	public float meteorWallInterval;

	[Header("状态")]
	public wallState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private List<Boss6_Meteor> meteors = new List<Boss6_Meteor>();

	private float roomWidth;

	private float roomHeight;

	private Vector3 roomCenter;

	private List<wallState> attackStates = new List<wallState>
	{
		wallState.Left,
		wallState.Right,
		wallState.Both
	};

	public wallState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
			varMgr.Clear();
		}
	}

	private void Start()
	{
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y;
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
	}

	public void SetRandomWall()
	{
		int weightRandom = GeneralTool.GetWeightRandom(1f, 1f, 1f);
		state = attackStates[weightRandom];
	}

	public void SetWall(wallState state)
	{
		this.state = state;
	}

	private void ShootMeteorWall(bool isLeft)
	{
		int num = Mathf.CeilToInt(roomHeight / meteorWallSingleWidth);
		bool[] array = new bool[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = true;
		}
		int num2 = Mathf.CeilToInt((float)num / (float)wallHoleCount);
		for (int j = 0; j < wallHoleCount; j++)
		{
			int num3 = Mathf.Clamp(Random.Range(j * num2, num2 + j * num2), 0, num - 1);
			array[num3] = false;
		}
		for (int k = 0; k < num; k++)
		{
			float num4 = roomCenter.y - roomHeight / 2f + meteorWallSingleWidth / 2f;
			if (array[k])
			{
				Vector3 point = new Vector3(roomCenter.x + (float)((!isLeft) ? 1 : (-1)) * roomWidth / 2f, num4 + (float)k * meteorWallSingleWidth);
				Boss6_Meteor component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_Meteor", point).GetComponent<Boss6_Meteor>();
				component.Initialize(isLeft ? Vector3.right : Vector3.left, 4f);
				meteors.Add(component);
			}
		}
	}

	private void ShootMeteor()
	{
		if (state == wallState.Left)
		{
			Vector3 point = roomCenter + new Vector3((0f - roomWidth) / 2f, (Random.value - 0.5f) * roomHeight, 0f);
			Boss6_Meteor component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_Meteor", point).GetComponent<Boss6_Meteor>();
			component.Initialize(Vector3.right);
			meteors.Add(component);
		}
		else if (state == wallState.Right)
		{
			Vector3 point2 = roomCenter + new Vector3(roomWidth / 2f, (Random.value - 0.5f) * roomHeight, 0f);
			Boss6_Meteor component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_Meteor", point2).GetComponent<Boss6_Meteor>();
			component2.Initialize(Vector3.left);
			meteors.Add(component2);
		}
	}

	private void Update()
	{
		for (int num = meteors.Count - 1; num >= 0; num--)
		{
			if (meteors[num].state == Boss6_Meteor.meteorState.Fade)
			{
				meteors.RemoveAt(num);
			}
		}
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case wallState.Rest:
			if (changedState)
			{
				for (int i = 0; i < meteors.Count; i++)
				{
					meteors[i].Mute();
				}
			}
			break;
		case wallState.Left:
			if (stateExistTime > singleMeteorInterval)
			{
				ShootMeteor();
				stateExistTime = 0f;
			}
			break;
		case wallState.Right:
			if (stateExistTime > singleMeteorInterval)
			{
				ShootMeteor();
				stateExistTime = 0f;
			}
			break;
		case wallState.Both:
			if (changedState)
			{
				ShootMeteorWall(isLeft: true);
				ShootMeteorWall(isLeft: false);
			}
			if (stateExistTime > meteorWallInterval)
			{
				ShootMeteorWall(isLeft: true);
				ShootMeteorWall(isLeft: false);
				stateExistTime = 0f;
			}
			break;
		}
	}
}
