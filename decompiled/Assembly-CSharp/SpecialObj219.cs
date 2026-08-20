using System;
using System.Collections.Generic;
using System.Linq;
using PlayerLogger;
using UnityEngine;

public class SpecialObj219 : LayerCorrect, IRoomCtrller
{
	[Serializable]
	public class Line
	{
		public bool complete;

		public List<Vector2Int> Points = new List<Vector2Int>();
	}

	public SpecialObj219GameMap.Map currentmap;

	public SpecialObj219Blocks pfb_Blocks;

	public Transform BlocksParent;

	public BoxCollider Edge;

	public int interactingNum;

	public Vector2Int currentStartPosition;

	public float blockSize;

	public float blockSizeInit = 1.425f;

	public List<Line> Lines = new List<Line>();

	public List<Sprite> IconsOriginal = new List<Sprite>();

	public List<Sprite> IconsH = new List<Sprite>();

	public List<Color> colors = new List<Color>();

	public bool IsComplete;

	public List<SpecialObj219Blocks> blocks = new List<SpecialObj219Blocks>();

	public int LoadIndex;

	private RoomController roomCtrller;

	public GameObject CompleteShow;

	public Sprite texture1;

	public Sprite texture2;

	public Sprite texture3;

	[ColorUsage(true, true)]
	public Color ColorNormal;

	[ColorUsage(true, true)]
	public Color ColorLight;

	private List<Sprite> Icons
	{
		get
		{
			if (GameMgr.IsHarmony_Static)
			{
				return IconsH;
			}
			return IconsOriginal;
		}
	}

	internal void MoveOUT()
	{
		if (interactingNum == 0)
		{
			return;
		}
		foreach (SpecialObj219Blocks block in blocks)
		{
			if (block.number == interactingNum)
			{
				block.spriterendererFrame.material.SetColor("_MainColor", ColorNormal);
			}
		}
		foreach (Vector2Int point in Lines[interactingNum - 1].Points)
		{
			GetBlock(point.x, point.y).spriterenderer.material.color = colors[0];
		}
		Lines[interactingNum - 1].Points.Clear();
		SEMgr.Inst.so219Cancle.PlaySE();
		interactingNum = 0;
	}

	private void Start()
	{
		currentmap = SpecialObj219GameMap.GetRandomMap();
		int num = UnityEngine.Random.Range(0, 4);
		for (int i = 0; i < num; i++)
		{
			currentmap = SpecialObj219GameMap.CreateSpinMap(currentmap);
		}
		Debug.Log("当前关卡id:" + currentmap.Id);
		SetUpBlocks(currentmap);
	}

	public bool CheckComplete()
	{
		foreach (Line line in Lines)
		{
			if (!line.complete)
			{
				return false;
			}
		}
		IsComplete = true;
		CompleteShow.SetActive(value: true);
		return true;
	}

	public bool CheckContinue(Vector2Int newvector)
	{
		if (Lines[interactingNum - 1].Points.Count == 0)
		{
			Vector2Int vector2Int = currentStartPosition;
			if (vector2Int == newvector + new Vector2Int(0, 1) || vector2Int == newvector + new Vector2Int(0, -1) || vector2Int == newvector + new Vector2Int(1, 0) || vector2Int == newvector + new Vector2Int(-1, 0))
			{
				return true;
			}
		}
		else
		{
			Vector2Int vector2Int2 = Lines[interactingNum - 1].Points[Lines[interactingNum - 1].Points.Count - 1];
			if (vector2Int2 == newvector + new Vector2Int(0, 1) || vector2Int2 == newvector + new Vector2Int(0, -1) || vector2Int2 == newvector + new Vector2Int(1, 0) || vector2Int2 == newvector + new Vector2Int(-1, 0))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckSamePoint(Vector2Int newvector)
	{
		if (Lines[interactingNum - 1].Points.Count == 0)
		{
			if (currentStartPosition == newvector)
			{
				return true;
			}
		}
		else if (Lines[interactingNum - 1].Points[Lines[interactingNum - 1].Points.Count - 1] == newvector)
		{
			return true;
		}
		return false;
	}

	private void SetUpBlocks(SpecialObj219GameMap.Map currentmap)
	{
		Edge.size = new Vector3((float)currentmap.Size * blockSize - 0.55f, (float)currentmap.Size * blockSize - 0.55f, 1f);
		for (int i = 0; i < currentmap.Points.Length / 2; i++)
		{
			Lines.Add(new Line());
		}
		Vector3 vector = new Vector3(((float)currentmap.Size / 2f + 0.5f) * (0f - blockSize), ((float)currentmap.Size / 2f + 0.5f) * (0f - blockSize), 0f);
		for (int j = 0; j < currentmap.Size * currentmap.Size; j++)
		{
			int num = Mathf.FloorToInt((float)j / (float)currentmap.Size);
			int num2 = j % currentmap.Size;
			SpecialObj219Blocks specialObj219Blocks = UnityEngine.Object.Instantiate(pfb_Blocks, BlocksParent.transform.position + vector + new Vector3((float)(num2 + 1) * blockSize, (float)(num + 1) * blockSize, 0f), Quaternion.identity, BlocksParent);
			blocks.Add(specialObj219Blocks);
			specialObj219Blocks.transform.localScale = new Vector3(blockSizeInit, blockSizeInit, 1f);
			specialObj219Blocks.SpecialObj219 = this;
			specialObj219Blocks.Position = new Vector2Int(num2, num);
			specialObj219Blocks.spriterenderer.sprite = GetRandomTexture();
			specialObj219Blocks.spriterenderer.material.color = specialObj219Blocks.spriterenderer.color;
			SpecialObj219GameMap.Point[] points = currentmap.Points;
			foreach (SpecialObj219GameMap.Point point in points)
			{
				if ((float)num2 == point.Position.x && (float)num == point.Position.y)
				{
					specialObj219Blocks.isNumberedBlock = true;
					specialObj219Blocks.number = point.Number;
					specialObj219Blocks.spriterenderer.material.color = colors[point.Number];
					specialObj219Blocks.colorBlindnessFriendly.enabled = true;
					specialObj219Blocks.colorBlindnessFriendly.sprite = Icons[point.Number];
					specialObj219Blocks.spriterendererFrame.enabled = true;
				}
			}
		}
	}

	private Sprite GetRandomTexture()
	{
		float value = UnityEngine.Random.value;
		if (value < 0.33f)
		{
			return texture1;
		}
		if (value > 0.66f)
		{
			return texture2;
		}
		return texture3;
	}

	public void Add(int x, int y, SpecialObj219Blocks block, int num = 0)
	{
		Vector2Int vector2Int = new Vector2Int(x, y);
		if (interactingNum == 0 && num != 0)
		{
			StopLine(num);
			StartLine(num, vector2Int);
			SEMgr.Inst.so219Start.PlaySE();
		}
		else if (interactingNum != 0 && num == 0)
		{
			if (Lines[interactingNum - 1].Points.Contains(vector2Int))
			{
				if (Lines[interactingNum - 1].Points.Count > 1)
				{
					List<Vector2Int> points = Lines[interactingNum - 1].Points;
					if (points[points.Count - 2] == vector2Int)
					{
						List<Vector2Int> points2 = Lines[interactingNum - 1].Points;
						Vector2Int vector2Int2 = points2[points2.Count - 1];
						GetBlock(vector2Int2.x, vector2Int2.y).spriterenderer.material.color = colors[0];
						GetBlock(vector2Int2.x, vector2Int2.y).spriterendererFrameSmall.SetActive(value: false);
						Lines[interactingNum - 1].Points.RemoveAt(Lines[interactingNum - 1].Points.Count - 1);
						SEMgr.Inst.so219Click.PlaySE();
					}
					else if (!CheckSamePoint(vector2Int))
					{
						StopLine(CheckPointBeenUse(vector2Int));
						SEMgr.Inst.so219Cancle.PlaySE();
					}
				}
			}
			else if (CheckContinue(vector2Int))
			{
				if (!CheckSamePoint(vector2Int))
				{
					if (CheckPointBeenUse(vector2Int) != -1)
					{
						SEMgr.Inst.so219Cancle.PlaySE();
						StopLine(CheckPointBeenUse(vector2Int), Stopinteracting: false);
					}
					else
					{
						SEMgr.Inst.so219Click.PlaySE();
					}
				}
				Lines[interactingNum - 1].Points.Add(vector2Int);
				GetBlock(x, y).spriterenderer.material.color = colors[interactingNum];
				block.spriterendererFrameSmall.SetActive(value: true);
			}
			else
			{
				StopLine(interactingNum);
				SEMgr.Inst.so219Cancle.PlaySE();
			}
		}
		else
		{
			if (num == 0)
			{
				return;
			}
			if (num != interactingNum)
			{
				SEMgr.Inst.so219Start.PlaySE();
				StopCurrentLine();
				StopLine(num);
				interactingNum = num;
				currentStartPosition = vector2Int;
				{
					foreach (SpecialObj219Blocks block2 in blocks)
					{
						if (block2.Position == vector2Int)
						{
							block2.spriterendererFrame.material.SetColor("_MainColor", ColorLight);
						}
					}
					return;
				}
			}
			if (currentStartPosition == vector2Int)
			{
				SEMgr.Inst.so219Cancle.PlaySE();
				StopCurrentLine();
			}
			else if (CheckContinue(vector2Int))
			{
				Lines[interactingNum - 1].complete = true;
				interactingNum = 0;
				foreach (SpecialObj219Blocks block3 in blocks)
				{
					if (block3.Position == vector2Int)
					{
						block3.spriterendererFrame.material.SetColor("_MainColor", ColorLight);
					}
				}
				if (CheckComplete())
				{
					SEMgr.Inst.puzzleSucceed.PlaySE();
					DropRward();
				}
				else
				{
					SEMgr.Inst.so219FinishLIne.PlaySE();
				}
			}
			else
			{
				SEMgr.Inst.so219Cancle.PlaySE();
				StopCurrentLine();
			}
		}
	}

	public void StopCurrentLine()
	{
		foreach (SpecialObj219Blocks block in blocks)
		{
			if (block.number == interactingNum)
			{
				block.spriterendererFrame.material.SetColor("_MainColor", ColorNormal);
			}
		}
		foreach (Vector2Int point in Lines[interactingNum - 1].Points)
		{
			GetBlock(point.x, point.y).spriterenderer.material.color = colors[0];
			GetBlock(point.x, point.y).spriterendererFrameSmall.SetActive(value: false);
		}
		Lines[interactingNum - 1].Points.Clear();
		interactingNum = 0;
	}

	private void StopLine(int num, bool Stopinteracting = true)
	{
		if (num < 0)
		{
			return;
		}
		foreach (SpecialObj219Blocks item in blocks.Where((SpecialObj219Blocks a) => a.isNumberedBlock && a.number == num))
		{
			item.spriterendererFrame.material.SetColor("_MainColor", ColorNormal);
		}
		foreach (Vector2Int point in Lines[num - 1].Points)
		{
			GetBlock(point.x, point.y).spriterenderer.material.color = colors[0];
			GetBlock(point.x, point.y).spriterendererFrameSmall.SetActive(value: false);
		}
		Lines[num - 1].Points.Clear();
		Lines[num - 1].complete = false;
		if (Stopinteracting)
		{
			interactingNum = 0;
		}
	}

	private void StartLine(int num, Vector2Int newvector)
	{
		interactingNum = num;
		currentStartPosition = newvector;
		foreach (SpecialObj219Blocks item in blocks.Where((SpecialObj219Blocks a) => a.Position == newvector))
		{
			item.spriterendererFrame.material.SetColor("_MainColor", ColorLight);
		}
	}

	private SpecialObj219Blocks GetBlock(int x, int y)
	{
		return blocks[x + y * currentmap.Size];
	}

	private int CheckPointBeenUse(Vector2Int newPosition)
	{
		for (int i = 0; i < Lines.Count; i++)
		{
			if (Lines[i].Points.Any((Vector2Int t) => t == newPosition))
			{
				return i + 1;
			}
		}
		return -1;
	}

	private void DropRward()
	{
		int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
		LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
		ItemInfo itemInfo = default(ItemInfo);
		itemInfo.type = ItemType.Spell;
		itemInfo.id = specialRoomSpell;
		ItemInfo info = itemInfo;
		QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, info, base.transform.position);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Puzzle_Correct", PlayerMgr.Inst.PlayerCtrller.transform.position, 2f);
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		this.roomCtrller = roomCtrller;
	}
}
