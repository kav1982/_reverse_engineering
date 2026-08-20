using System;
using System.Linq;
using UnityEngine;

public static class SlotDataArrayExtend
{
	public static string Bag_ToDebugString(this SlotData[] Self)
	{
		return string.Join("|", Self.Select(delegate(SlotData e)
		{
			if (e == null)
			{
				return "_";
			}
			return e.isSealSlot ? "×" : e.id.ToString();
		}));
	}

	public static string Bag_ToDebugString(this bool[] locks)
	{
		return string.Join("|", locks.Select((bool e) => (!e) ? "_" : "×"));
	}

	public static SlotData Bag_PopLast(this SlotData[] Self, bool[] locks)
	{
		int index = Self.Length;
		int num;
		SlotData result;
		while (true)
		{
			num = Self.Bag_GetPreviousSpell(index);
			if (num < 0)
			{
				return null;
			}
			result = Self[num];
			if (!locks[num])
			{
				break;
			}
			index = num;
		}
		Self.Bag_RemoveSlot(num);
		return result;
	}

	public static bool Bag_CanSetSpell(this SlotData[] Self, bool[] locks, SlotData data, int index)
	{
		if (Self[index] != null)
		{
			return false;
		}
		int finalSlotCost = data.GetFinalSlotCost();
		return Self.Bag_GetRightSpace(locks, index) + 1 >= finalSlotCost;
	}

	public static void Bag_SetSlot(this SlotData[] Self, bool[] locks, SlotData data, int index)
	{
		if (!Self.Bag_CanSetSpell(locks, data, index))
		{
			throw new Exception($"不能在 {index} 设置格子，ID: {data.id}");
		}
		Self[index] = data;
		int finalSlotCost = data.GetFinalSlotCost();
		for (int i = index + 1; i < index + finalSlotCost; i++)
		{
			Self[i] = Bag_CreateSealedSlotData(data);
		}
	}

	public static int Bag_GetSlotSize(this SlotData[] Self, int index)
	{
		int num = 1;
		for (int i = index + 1; i < Self.Length; i++)
		{
			SlotData slotData = Self[i];
			if (slotData == null)
			{
				return num;
			}
			if (!slotData.isSealSlot)
			{
				return num;
			}
			num++;
		}
		return num;
	}

	public static void Bag_ClearSeal(this SlotData[] Self, int index)
	{
		for (int i = index + 1; i < Self.Length; i++)
		{
			SlotData slotData = Self[i];
			if (slotData == null || !slotData.isSealSlot)
			{
				break;
			}
			Self[i] = null;
		}
	}

	public static void Bag_RemoveSlot(this SlotData[] Self, int index)
	{
		if (!Self.Bag_IndexCheck(index))
		{
			throw new Exception("不能移除法术");
		}
		if ((Self[index] ?? throw new Exception("不能移除空的格子")).isSealSlot)
		{
			throw new Exception("不能移除被禁用的格子");
		}
		Self[index].slotSpellExtraLevel = 0;
		SpellAbilityType abilityType = SpellConfig.dic[Self[index].id].abilityType;
		if (abilityType == SpellAbilityType.ManaTendril || abilityType == SpellAbilityType.Mimic)
		{
			Self[index].specialInt = 0;
		}
		Self[index] = null;
		Self.Bag_ClearSeal(index);
	}

	public static bool Bag_IndexCheck(this SlotData[] Self, int index)
	{
		if (index >= 0)
		{
			return index < Self.Length;
		}
		return false;
	}

	public static bool Bag_CanPushToRight(this SlotData[] Self, bool[] locks, int index)
	{
		Self.Bag_BagDebugLog($"Bag_CanPushToRight({locks.Bag_ToDebugString()}, {index})");
		if (!Self.Bag_IndexCheck(index))
		{
			return false;
		}
		if (locks[index])
		{
			return false;
		}
		if (Self[index] == null)
		{
			return false;
		}
		if (Self[index].isSealSlot)
		{
			return false;
		}
		return Self.Bag_GetRightSpaceThroughSpell(locks, index) >= 1;
	}

	public static void Bag_PushToRight(this SlotData[] Self, bool[] locks, int index)
	{
		if (!Self.Bag_CanPushToRight(locks, index))
		{
			throw new Exception("不能向右边推进");
		}
		SlotData slotData = Self[index];
		int finalSlotCost = slotData.GetFinalSlotCost();
		int num = index + finalSlotCost;
		if (!Self.Bag_IndexCheck(num))
		{
			throw new Exception("意外错误，在右推物品的时候");
		}
		if (Self[num] != null)
		{
			Self.Bag_PushToRight(locks, num);
		}
		Self.Bag_RemoveSlot(index);
		Self.Bag_SetSlot(locks, slotData, index + 1);
	}

	public static bool Bag_CanPushToLeft(this SlotData[] Self, bool[] locks, int index)
	{
		if (!Self.Bag_IndexCheck(index))
		{
			return false;
		}
		if (locks[index])
		{
			return false;
		}
		if (Self[index] == null)
		{
			return false;
		}
		if (Self[index].isSealSlot)
		{
			return false;
		}
		return Self.Bag_GetLeftSpaceThroughSpell(locks, index) >= 1;
	}

	public static void Bag_PushToLeft(this SlotData[] Self, bool[] locks, int index)
	{
		if (!Self.Bag_CanPushToLeft(locks, index))
		{
			throw new Exception("不能向左边推进");
		}
		SlotData data = Self[index];
		int num = index - 1;
		if (!Self.Bag_IndexCheck(num))
		{
			throw new Exception("意外错误，在左推物品的时候");
		}
		SlotData slotData = Self[num];
		if (slotData != null)
		{
			int index2 = Array.IndexOf(Self, slotData.sealSlotOwner);
			if (slotData.isSealSlot)
			{
				Self.Bag_PushToLeft(locks, index2);
			}
			else
			{
				Self.Bag_PushToLeft(locks, num);
			}
		}
		Self.Bag_RemoveSlot(index);
		Self.Bag_SetSlot(locks, data, index - 1);
	}

	public static int Bag_SpaceCount(this SlotData[] Self, bool[] locks = null)
	{
		if (locks == null)
		{
			return Self.Count((SlotData e) => e == null);
		}
		return Self.Zip(locks).Count(((SlotData Left, bool Right) e) => e.Left == null && !e.Right);
	}

	public static int Bag_SpaceCountOfSideThroughSpell(this SlotData[] Self, bool[] locks, int index)
	{
		if (locks[index])
		{
			return 0;
		}
		if (Self[index] != null)
		{
			return 0;
		}
		return Self.Bag_GetLeftSpaceThroughSpell(locks, index) + Self.Bag_GetRightSpaceThroughSpell(locks, index) + 1;
	}

	public static int Bag_GetRightSpaceThroughSpell(this SlotData[] Self, bool[] locks, int index)
	{
		int num = 0;
		for (int i = index + 1; i < Self.Length && !locks[i]; i++)
		{
			if (Self[i] == null)
			{
				num++;
			}
		}
		return num;
	}

	public static int Bag_GetLeftSpaceThroughSpell(this SlotData[] Self, bool[] locks, int index)
	{
		int num = 0;
		int num2 = index - 1;
		while (num2 >= 0 && !locks[num2])
		{
			if (Self[num2] == null)
			{
				num++;
			}
			num2--;
		}
		return num;
	}

	public static int Bag_GetRightSpace(this SlotData[] Self, bool[] locks, int index)
	{
		int num = 0;
		for (int i = index + 1; i < Self.Length && !locks[i] && Self[i] == null; i++)
		{
			num++;
		}
		return num;
	}

	public static int Bag_GetLeftSpace(this SlotData[] Self, bool[] locks, int index)
	{
		int num = 0;
		int num2 = index - 1;
		while (num2 >= 0 && !locks[num2] && Self[num2] == null)
		{
			num++;
			num2--;
		}
		return num;
	}

	public static int Bag_GetNextSpell(this SlotData[] Self, int index)
	{
		for (int i = index + 1; i < Self.Length; i++)
		{
			SlotData slotData = Self[i];
			if (slotData != null && !slotData.isSealSlot)
			{
				return i;
			}
		}
		return -1;
	}

	public static int Bag_GetPreviousSpell(this SlotData[] Self, int index)
	{
		for (int num = index - 1; num >= 0; num--)
		{
			SlotData slotData = Self[num];
			if (slotData != null && !slotData.isSealSlot)
			{
				return num;
			}
		}
		return -1;
	}

	public static int Bag_GetFirstCanSetWithPushSlotIndex(this SlotData[] Self, bool[] locks, SlotData data)
	{
		for (int i = 0; i < Self.Length; i++)
		{
			if (Self.Bag_CanSetSlotWithPush(locks, data, i))
			{
				return i;
			}
		}
		return -1;
	}

	public static int Bag_GetFirstNullSlotIndex(this SlotData[] Self)
	{
		for (int i = 0; i < Self.Length; i++)
		{
			if (Self[i] == null)
			{
				return i;
			}
		}
		return -1;
	}

	public static bool Bag_CanSetSlotWithPush(this SlotData[] Self, bool[] locks, SlotData data, int index)
	{
		if (index >= Self.Length)
		{
			return false;
		}
		if (Self[index] != null || locks[index])
		{
			return false;
		}
		int finalSlotCost = data.GetFinalSlotCost();
		return Self.Bag_SpaceCountOfSideThroughSpell(locks, index) >= finalSlotCost;
	}

	public static void Bag_SetSlotWithPush(this SlotData[] Self, bool[] locks, SlotData data, int index)
	{
		Self.Bag_BagDebugLog($"Bag_SetSlotWithPush({locks.Bag_ToDebugString()}, {data.id}, {index});");
		if (!Self.Bag_CanSetSlotWithPush(locks, data, index))
		{
			Debug.Log(data.id + " " + index);
			throw new Exception("空间不足放新法术");
		}
		int finalSlotCost = data.GetFinalSlotCost();
		while (true)
		{
			if (Self.Bag_GetRightSpace(locks, index) + 1 >= finalSlotCost)
			{
				Self.Bag_BagDebugLog(string.Join("|", Self.Select((SlotData e) => (e != null) ? 1 : 0)));
				Self.Bag_BagDebugLog($"推右侧空间足够了  {index}  {Self.Bag_GetRightSpace(locks, index)}  {finalSlotCost}");
				break;
			}
			if (!Self.Bag_CanPushToRight(locks, Self.Bag_GetNextSpell(index)))
			{
				Self.Bag_BagDebugLog("推右侧不能继续退了");
				break;
			}
			Self.Bag_BagDebugLog($"Right Push Once {Self.Bag_GetNextSpell(index)}");
			Self.Bag_PushToRight(locks, Self.Bag_GetNextSpell(index));
		}
		int num = Self.Bag_GetRightSpace(locks, index);
		int num2 = 0;
		if (num + 1 < finalSlotCost)
		{
			Self.Bag_BagDebugLog("需要向左推");
			num2 = -(finalSlotCost - num - 1);
			while (true)
			{
				if (Self.Bag_GetRightSpace(locks, index) + Self.Bag_GetLeftSpace(locks, index) + 1 >= finalSlotCost)
				{
					Self.Bag_BagDebugLog(string.Join("|", Self.Select((SlotData e) => (e != null) ? 1 : 0)));
					Self.Bag_BagDebugLog($"推左侧空间足够了  {index}  {Self.Bag_GetLeftSpace(locks, index)}  {finalSlotCost}");
					break;
				}
				if (!Self.Bag_CanPushToLeft(locks, Self.Bag_GetPreviousSpell(index)))
				{
					Self.Bag_BagDebugLog("推左侧不能继续推了");
					break;
				}
				Self.Bag_BagDebugLog($"Left Push Once {Self.Bag_GetPreviousSpell(index)}");
				Self.Bag_PushToLeft(locks, Self.Bag_GetPreviousSpell(index));
			}
		}
		Self.Bag_SetSlot(locks, data, index + num2);
	}

	public static void Bag_SwapSlot(this SlotData[] Self, bool[] locks, int a, int b)
	{
		Self.Bag_BagDebugLog($"Bag_SwapSlot({a},{b});");
		SlotData data = Self[a];
		SlotData data2 = Self[b];
		Self.Bag_RemoveSlot(a);
		Self.Bag_RemoveSlot(b);
		Self.Bag_SetSlotWithPush(locks, data, b);
		int num = a;
		while (!Self.Bag_CanSetSlotWithPush(locks, data2, num) || !Self.Bag_CanSetSpell(locks, data2, num))
		{
			num++;
			if (num == a)
			{
				Debug.LogError("没找到合适的放置位置");
				break;
			}
			if (num >= Self.Length)
			{
				num = 0;
			}
		}
		Self.Bag_SetSlotWithPush(locks, data2, num);
		UIPlayerDataMgr.Inst.UpdateBag();
	}

	public static void Bag_BagDebugLog(this SlotData[] Self, string message)
	{
	}

	public static SlotData[] Bag_DeepCopy(this SlotData[] Self)
	{
		return (from e in Self.ToArray()
			select e?.Copy()).ToArray();
	}

	public static SlotData Bag_CreateSealedSlotData(SlotData owner)
	{
		if (owner == null)
		{
			return null;
		}
		return new SlotData
		{
			sealSlotOwner = owner
		};
	}

	public static int Bag_GetOwnerSlotIndex(this SlotData[] Self, int index)
	{
		int num = index;
		do
		{
			if (Self[num] == null)
			{
				return num;
			}
			if (!Self[num].isSealSlot)
			{
				return num;
			}
			num--;
		}
		while (num >= 0);
		return -1;
	}

	public static int BagLocks_GetLeftCanMoveSpace(this bool[] Self, int index)
	{
		int num = 0;
		index--;
		while (index >= 0 && !Self[index])
		{
			num++;
			index--;
		}
		return num;
	}

	public static int BagLocks_GetRightCanMoveSpace(this bool[] Self, int index)
	{
		int num = 0;
		index++;
		while (index < Self.Length && !Self[index])
		{
			num++;
			index++;
		}
		return num;
	}

	public static int BagLocks_GetCanMoveSpace(this bool[] Self, int index)
	{
		if (Self[index])
		{
			return 0;
		}
		return Self.BagLocks_GetLeftCanMoveSpace(index) + Self.BagLocks_GetRightCanMoveSpace(index) + 1;
	}
}
