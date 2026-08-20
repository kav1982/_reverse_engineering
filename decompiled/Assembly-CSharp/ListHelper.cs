using System.Collections.Generic;
using UnityEngine;

public static class ListHelper
{
	public static List<T> Copy<T>(this List<T> list)
	{
		List<T> list2 = new List<T>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i]);
		}
		return list2;
	}

	public static T[] Copy<T>(this T[] array)
	{
		T[] array2 = new T[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = array[i];
		}
		return array2;
	}

	public static void Upset<T>(this List<T> list)
	{
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			T item = list[Random.Range(0, list.Count - i)];
			list.Remove(item);
			list.Add(item);
		}
	}

	public static T GetRandom<T>(this List<T> list, int minIndex = 0)
	{
		return list[Random.Range(minIndex, list.Count)];
	}

	public static T GetRandom<T>(this T[] array, int minIndex = 0)
	{
		return array[Random.Range(minIndex, array.Length)];
	}
}
