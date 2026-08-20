using System.Collections.Generic;
using UnityEngine;

public static class SpecialObj219GameMap
{
	public class Map
	{
		public readonly string Id;

		public readonly int Size;

		public readonly Point[] Points;

		public Map(string id, int size, Point[] points)
		{
			Id = id;
			Size = size;
			Points = points;
		}
	}

	public class Point
	{
		public readonly Vector2 Position;

		public readonly int Number;

		public Point(Vector2 position, int number)
		{
			Position = position;
			Number = number;
		}
	}

	private static readonly Map[] Maps = new Map[8]
	{
		new Map("2024-02-27:N2KE9DSV5H0JWRYZ", 5, new Point[8]
		{
			new Point(new Vector2(0f, 0f), 1),
			new Point(new Vector2(2f, 2f), 1),
			new Point(new Vector2(0f, 1f), 2),
			new Point(new Vector2(2f, 4f), 2),
			new Point(new Vector2(1f, 1f), 3),
			new Point(new Vector2(3f, 0f), 3),
			new Point(new Vector2(4f, 0f), 4),
			new Point(new Vector2(3f, 4f), 4)
		}),
		new Map("2024-02-27:N2KE9DSV5H0JWRYZ", 5, new Point[8]
		{
			new Point(new Vector2(0f, 3f), 1),
			new Point(new Vector2(2f, 3f), 1),
			new Point(new Vector2(0f, 0f), 2),
			new Point(new Vector2(4f, 2f), 2),
			new Point(new Vector2(0f, 4f), 3),
			new Point(new Vector2(2f, 2f), 3),
			new Point(new Vector2(2f, 4f), 4),
			new Point(new Vector2(4f, 4f), 4)
		}),
		new Map("2024-02-27:N2KE9DSV5H0JWRYZ", 5, new Point[8]
		{
			new Point(new Vector2(1f, 1f), 1),
			new Point(new Vector2(3f, 1f), 1),
			new Point(new Vector2(0f, 1f), 2),
			new Point(new Vector2(2f, 2f), 2),
			new Point(new Vector2(0f, 4f), 3),
			new Point(new Vector2(0f, 2f), 3),
			new Point(new Vector2(3f, 0f), 4),
			new Point(new Vector2(1f, 4f), 4)
		}),
		new Map("2024-02-27:N2KE9DSV5H0JWRYZ", 5, new Point[6]
		{
			new Point(new Vector2(0f, 3f), 1),
			new Point(new Vector2(3f, 3f), 1),
			new Point(new Vector2(0f, 4f), 2),
			new Point(new Vector2(3f, 1f), 2),
			new Point(new Vector2(3f, 2f), 3),
			new Point(new Vector2(4f, 4f), 3)
		}),
		new Map("2024-02-27:N2KE9DSV5H0JWRYZ", 5, new Point[6]
		{
			new Point(new Vector2(0f, 4f), 1),
			new Point(new Vector2(3f, 1f), 1),
			new Point(new Vector2(1f, 4f), 2),
			new Point(new Vector2(3f, 3f), 2),
			new Point(new Vector2(2f, 0f), 3),
			new Point(new Vector2(2f, 3f), 3)
		}),
		new Map("2024-02-27:N2KE9DSV5H0JWRYZ", 5, new Point[6]
		{
			new Point(new Vector2(0f, 4f), 1),
			new Point(new Vector2(3f, 1f), 1),
			new Point(new Vector2(1f, 4f), 2),
			new Point(new Vector2(3f, 3f), 2),
			new Point(new Vector2(2f, 0f), 3),
			new Point(new Vector2(2f, 3f), 3)
		}),
		new Map("2024-02-27:N2KE9DSV5H0JWRYZ", 5, new Point[8]
		{
			new Point(new Vector2(0f, 0f), 1),
			new Point(new Vector2(4f, 3f), 1),
			new Point(new Vector2(1f, 2f), 2),
			new Point(new Vector2(4f, 0f), 2),
			new Point(new Vector2(0f, 4f), 3),
			new Point(new Vector2(2f, 4f), 3),
			new Point(new Vector2(3f, 2f), 4),
			new Point(new Vector2(4f, 4f), 4)
		}),
		new Map("2024-02-27:N2KE9DSV5H0JWRYZ", 5, new Point[6]
		{
			new Point(new Vector2(0f, 3f), 1),
			new Point(new Vector2(2f, 3f), 1),
			new Point(new Vector2(2f, 4f), 2),
			new Point(new Vector2(0f, 0f), 2),
			new Point(new Vector2(0f, 4f), 3),
			new Point(new Vector2(2f, 2f), 3)
		})
	};

	public static Map GetRandomMap()
	{
		return Maps[Random.Range(0, Maps.Length)];
	}

	public static Map GetMapByIndex(int index)
	{
		return Maps[index];
	}

	public static Map GetMapByID(string id)
	{
		Map[] maps = Maps;
		foreach (Map map in maps)
		{
			if (map.Id == id)
			{
				return map;
			}
		}
		Debug.Log("没有指定的关卡");
		return GetRandomMap();
	}

	public static Map CreateSpinMap(Map map)
	{
		List<Point> list = new List<Point>();
		for (int i = 0; i < map.Points.Length; i++)
		{
			list.Add(new Point(new Vector2((float)map.Size - map.Points[i].Position.y - 1f, map.Points[i].Position.x), map.Points[i].Number));
		}
		return new Map(map.Id + "x", map.Size, list.ToArray());
	}
}
