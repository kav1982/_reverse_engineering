using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class UIDamageRecordRank : MonoBehaviour
{
	public float positionLerp = 0.15f;

	public RectTransform rankLayout;

	public UIDamageRecordBar templateBar;

	private readonly Dictionary<int, UIDamageRecordBar> bars = new Dictionary<int, UIDamageRecordBar>();

	public void SetData(DamageRecorder recorde, int maxCount)
	{
		KeyValuePair<int, BigInteger>[] array = recorde.DamagePreSpell.OrderBy((KeyValuePair<int, BigInteger> e) => -e.Value).ToArray();
		HashSet<int> hashSet = bars.Keys.ToHashSet();
		for (int i = 0; i < maxCount && i < array.Length; i++)
		{
			int key = array[i].Key;
			if (!bars.TryGetValue(key, out var value))
			{
				GameObject obj = Object.Instantiate(templateBar.gameObject, rankLayout);
				obj.SetActive(value: true);
				value = obj.GetComponent<UIDamageRecordBar>();
				bars.Add(key, value);
			}
			else
			{
				hashSet.Remove(key);
			}
			value.Initialize(array[i].Key, i, array[i].Value, recorde.TotalDamage);
			value.positionLerp = positionLerp;
		}
		foreach (int item in hashSet)
		{
			Object.Destroy(bars[item].gameObject);
			bars.Remove(item);
		}
	}
}
