using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PlayerLogger;
using UnityEngine;

public class SpecialObj220 : LayerCorrect, IRoomCtrller
{
	private RoomController roomCtrller;

	public Vector2 CurrentEmptyPosition;

	public bool IsComplete;

	public Vector2 PositionOffset = new Vector2(-3f, 3f);

	public List<SpecialObj220Block> AllBlocks = new List<SpecialObj220Block>();

	public SpecialObj220Block emptyBlock;

	public SpecialObj220Block pfb_Block;

	public Transform tsfLayer;

	public int hight;

	public int width;

	public int sizex;

	public int sizey;

	public bool moving;

	private int blockCount = 6;

	public Sprite[] sprites;

	public GameObject backgroundObj;

	public void Start()
	{
		backgroundObj.transform.localPosition = -PositionOffset;
		base.transform.position = base.transform.position + (Vector3)PositionOffset;
		InitializePuzzle();
	}

	public int[] GetRandomStartPuzzel()
	{
		List<int[]> list = new List<int[]>();
		list.Add(new int[6] { 2, 6, 3, 5, 1, 4 });
		list.Add(new int[6] { 5, 3, 4, 1, 2, 6 });
		list.Add(new int[6] { 3, 2, 1, 6, 4, 5 });
		list.Add(new int[6] { 2, 4, 5, 3, 6, 1 });
		list.Add(new int[6] { 3, 6, 1, 2, 4, 5 });
		list.Add(new int[6] { 6, 2, 3, 4, 5, 1 });
		list.Add(new int[6] { 4, 3, 6, 5, 1, 2 });
		list.Add(new int[6] { 3, 4, 5, 6, 2, 1 });
		list.Add(new int[6] { 1, 3, 6, 4, 5, 2 });
		List<int[]> list2 = list;
		return list2[Random.Range(0, list2.Count)];
	}

	public void InitializePuzzle()
	{
		int[] randomStartPuzzel = GetRandomStartPuzzel();
		int x = 0;
		for (int i = 0; i < 300; i++)
		{
			x = Mix(randomStartPuzzel, x);
		}
		for (int j = 0; j < hight * width; j++)
		{
			if (randomStartPuzzel[j] == randomStartPuzzel.Length)
			{
				CurrentEmptyPosition = GetPositionByID(j);
				continue;
			}
			SpecialObj220Block specialObj220Block = Object.Instantiate(pfb_Block, tsfLayer);
			specialObj220Block.gameObject.transform.localPosition = GetPositionByID(j);
			specialObj220Block.gameObject.name = (randomStartPuzzel[j] - 1).ToString();
			specialObj220Block.id = randomStartPuzzel[j];
			specialObj220Block.BlockTargtPosition = GetPositionByID(specialObj220Block.id - 1);
			specialObj220Block.BlockPosition = GetPositionByID(j);
			specialObj220Block.spriterenderer.sprite = sprites[randomStartPuzzel[j] - 1];
			specialObj220Block.SpecialObj220 = this;
			AllBlocks.Add(specialObj220Block);
		}
	}

	public Vector2 GetPositionByID(int id)
	{
		int num = id % width;
		return new Vector2(y: ((float)(id / width) + 0.5f) * (float)(-sizey), x: ((float)num + 0.5f) * (float)sizex);
	}

	public bool MoveBolock(SpecialObj220Block block)
	{
		if (block.BlockPosition - CurrentEmptyPosition == new Vector2(sizex, 0f) || block.BlockPosition - CurrentEmptyPosition == new Vector2(-sizex, 0f) || block.BlockPosition - CurrentEmptyPosition == new Vector2(0f, sizey) || block.BlockPosition - CurrentEmptyPosition == new Vector2(0f, -sizey))
		{
			moving = true;
			Vector2 blockPosition = block.BlockPosition;
			block.gameObject.transform.DOMove((Vector3)CurrentEmptyPosition + base.transform.position, 0.15f).OnComplete(StopMoving);
			block.BlockPosition = CurrentEmptyPosition;
			SEMgr.Inst.so220Slide.PlaySE();
			CurrentEmptyPosition = blockPosition;
			return true;
		}
		return false;
	}

	public int Mix(int[] list, int x = 0)
	{
		int num = 0;
		if (x == 0)
		{
			for (int i = 0; i < list.Length; i++)
			{
				if (list[i] == list.Length)
				{
					num = i;
					break;
				}
			}
		}
		else
		{
			num = x;
		}
		if (list[num] == list.Length)
		{
			if (Random.value > 0.5f)
			{
				if (Random.value > 0.5f)
				{
					if (num / width > 0)
					{
						Swap(list, num, num - width);
					}
					else if (num / width < hight - 1)
					{
						Swap(list, num, num + width);
					}
				}
				else if (num / width < hight - 1)
				{
					Swap(list, num, num + width);
				}
				else if (num / width > 0)
				{
					Swap(list, num, num - width);
				}
			}
			else if (Random.value > 0.5f)
			{
				if ((num + 1) % width != 0)
				{
					Swap(list, num, num + 1);
				}
				else if ((num + 1) % width != 1)
				{
					Swap(list, num, num - 1);
				}
			}
			else if ((num + 1) % width != 1)
			{
				Swap(list, num, num - 1);
			}
			else if ((num + 1) % width != 0)
			{
				Swap(list, num, num + 1);
			}
			return num;
		}
		return 0;
	}

	public void StopMoving()
	{
		moving = false;
		if (CheckComplete())
		{
			IsComplete = true;
			SEMgr.Inst.puzzleSucceed.PlaySE();
			DropRward();
			StartCoroutine(ShowFinishBlock());
		}
	}

	private bool CheckComplete()
	{
		foreach (SpecialObj220Block allBlock in AllBlocks)
		{
			if (allBlock.BlockTargtPosition != allBlock.BlockPosition)
			{
				return false;
			}
		}
		return true;
	}

	public void DropRward()
	{
		int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
		LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Puzzle_Correct", PlayerMgr.Inst.PlayerCtrller.transform.position, 2f);
		ItemInfo itemInfo = default(ItemInfo);
		itemInfo.type = ItemType.Spell;
		itemInfo.id = specialRoomSpell;
		ItemInfo info = itemInfo;
		QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, info, base.transform.position);
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		this.roomCtrller = roomCtrller;
	}

	private static T[] Swap<T>(T[] list, int index1, int index2)
	{
		T val = list[index1];
		list[index1] = list[index2];
		list[index2] = val;
		return list;
	}

	public IEnumerator ShowFinishBlock()
	{
		emptyBlock = Object.Instantiate(pfb_Block, tsfLayer);
		emptyBlock.gameObject.transform.localPosition = CurrentEmptyPosition;
		emptyBlock.spriterenderer.sprite = sprites[blockCount - 1];
		emptyBlock.spriterenderer.color = Color.clear;
		emptyBlock.id = hight * width;
		float time = 0f;
		while (time < 0.5f)
		{
			yield return new WaitForFixedUpdate();
			time += Time.deltaTime;
			emptyBlock.spriterenderer.color = Color.Lerp(Color.clear, Color.white, time / 0.5f);
		}
	}
}
