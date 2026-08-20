using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomNavTool
{
	private enum NavDir
	{
		Left,
		Right,
		Up,
		Down
	}

	public static void Nav<T>(Vector2 dir, ref int currentID, ref int currentParentID, List<T> Contains, int widthCount) where T : Component
	{
		if (currentID == -1)
		{
			currentID = 0;
		}
		else if (dir == Vector2.left)
		{
			Nav(NavDir.Left, ref currentID, ref currentParentID, Contains, widthCount);
		}
		else if (dir == Vector2.right)
		{
			Nav(NavDir.Right, ref currentID, ref currentParentID, Contains, widthCount);
		}
		else if (dir == Vector2.up)
		{
			Nav(NavDir.Up, ref currentID, ref currentParentID, Contains, widthCount);
		}
		else if (dir == Vector2.down)
		{
			Nav(NavDir.Down, ref currentID, ref currentParentID, Contains, widthCount);
		}
	}

	private static void Nav<T>(NavDir dir, ref int currentID, ref int currentParentID, List<T> Contains, int widthCount) where T : Component
	{
		switch (dir)
		{
		case NavDir.Left:
			if (currentID > 0)
			{
				currentID--;
			}
			break;
		case NavDir.Right:
			if (currentID < Contains[currentParentID].transform.childCount - 1)
			{
				currentID++;
			}
			break;
		case NavDir.Up:
			if (currentID - widthCount >= 0)
			{
				currentID -= widthCount;
			}
			else if (currentParentID > 0)
			{
				currentParentID--;
				if (GetIndexHorizon(currentID) > GetIndexHorizon(Contains[currentParentID].transform.childCount - 1))
				{
					currentID = Contains[currentParentID].transform.childCount - 1;
					Debug.Log(currentID);
				}
				else if (Contains[currentParentID].transform.childCount % widthCount != 0)
				{
					currentID = widthCount * Mathf.FloorToInt((float)Contains[currentParentID].transform.childCount / (float)widthCount) + currentID % widthCount;
				}
				else
				{
					currentID = widthCount * Mathf.FloorToInt((float)Contains[currentParentID].transform.childCount / (float)widthCount - 1f) + currentID % widthCount;
				}
			}
			break;
		case NavDir.Down:
			if (currentID < Contains[currentParentID].transform.childCount - widthCount)
			{
				currentID += widthCount;
			}
			else if (GetIndexVertical(currentID) < GetIndexVertical(Contains[currentParentID].transform.childCount - 1))
			{
				currentID = Contains[currentParentID].transform.childCount - 1;
			}
			else if (currentParentID < Contains.Count - 1)
			{
				currentParentID++;
				if (Contains[currentParentID].transform.childCount < currentID % widthCount + 1)
				{
					currentID = Contains[currentParentID].transform.childCount - 1;
				}
				else
				{
					currentID %= widthCount;
				}
			}
			break;
		}
		int GetIndexHorizon(int x)
		{
			if ((x + 1) % widthCount == 0)
			{
				return widthCount;
			}
			return (x + 1) % widthCount;
		}
		int GetIndexVertical(int x)
		{
			if ((x + 1) % widthCount == 0)
			{
				return Mathf.FloorToInt(x / widthCount) - 1;
			}
			return Mathf.FloorToInt(x / widthCount);
		}
	}

	public static bool Nav(Vector2 dir, ref int currentID, GridLayoutGroup gridLayout, Func<bool> CheckAvailable)
	{
		if (gridLayout.constraint == GridLayoutGroup.Constraint.FixedRowCount)
		{
			int num = 0;
			for (int i = 0; i < gridLayout.transform.childCount; i++)
			{
				if (gridLayout.transform.GetChild(i).gameObject.activeInHierarchy)
				{
					num++;
				}
			}
			int constraintCount = gridLayout.constraintCount;
			if (dir == Vector2.up)
			{
				if (currentID == 0)
				{
					currentID = constraintCount - 1;
				}
				else if (currentID == 8)
				{
					currentID = num - 1;
				}
				else
				{
					currentID--;
				}
			}
			else if (dir == Vector2.down)
			{
				if (currentID == constraintCount - 1)
				{
					currentID = 0;
				}
				else if (currentID >= num - 1)
				{
					currentID = constraintCount;
				}
				else
				{
					currentID++;
				}
			}
			else if (dir == Vector2.left)
			{
				if (currentID >= constraintCount)
				{
					currentID -= constraintCount;
				}
			}
			else if (dir == Vector2.right && currentID <= constraintCount - 1)
			{
				currentID += constraintCount;
				if (currentID > num - 1)
				{
					currentID = num - 1;
				}
			}
		}
		if (!gridLayout.transform.GetChild(currentID).gameObject.activeSelf || !CheckAvailable())
		{
			return true;
		}
		return false;
	}
}
