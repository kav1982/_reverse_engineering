using System;
using System.Collections.Generic;
using System.Linq;

public class SpellGroupParser
{
	private readonly (SlotData spell, SlotData[] enhansList)[] _flatSpells;

	private readonly int _baseMaxShootCount;

	private int _index;

	private (SlotData spell, SlotData[] enhansList)? cur
	{
		get
		{
			if (!isEnd)
			{
				return _flatSpells[_index];
			}
			return null;
		}
	}

	private bool isEnd => _index >= _flatSpells.Length;

	private SpellGroupParser(IEnumerable<SlotData> spells, int baseMaxShootCount)
	{
		_baseMaxShootCount = baseMaxShootCount;
		List<(SlotData, SlotData[])> list = new List<(SlotData, SlotData[])>();
		List<SlotData> list2 = new List<SlotData>();
		foreach (SlotData item in spells.Where((SlotData e) => e != null && !e.isSealSlot))
		{
			SpellConfig finalConfig = item.GetFinalConfig();
			if (!item.IsTrigger())
			{
				SpellType useType = finalConfig.useType;
				if (useType != SpellType.Summon && useType != 0)
				{
					if (finalConfig.useType == SpellType.Enhance)
					{
						list2.Add(item);
					}
					continue;
				}
			}
			list.Add((item, list2.ToArray()));
		}
		_flatSpells = list.ToArray();
	}

	private IEnumerable<SpellShootGroup> Parse()
	{
		_index = 0;
		List<SpellShootGroup> list = new List<SpellShootGroup>();
		while (true)
		{
			SpellShootGroup spellShootGroup = NextGroup(Array.Empty<SlotData>());
			if (spellShootGroup == null)
			{
				break;
			}
			list.Add(spellShootGroup);
		}
		return list.ToArray();
	}

	private SpellShootGroup NextGroup(IEnumerable<SlotData> hideEnhances, SpellShootData ownerShootData = null)
	{
		List<SpellShootData> list = new List<SpellShootData>();
		SpellShootGroup spellShootGroup = new SpellShootGroup
		{
			OwnerShootData = ownerShootData
		};
		SlotData[] hideEnhances2 = hideEnhances.ToArray();
		while (list.Count == 0 || list.Count < list.Max((SpellShootData e) => e.GetMaxMeantimeShootCount(_baseMaxShootCount)))
		{
			SpellShootData spellShootData = NextShoot(spellShootGroup, hideEnhances2);
			if (spellShootData == null)
			{
				break;
			}
			list.Add(spellShootData);
		}
		if (list.Count == 0)
		{
			return null;
		}
		spellShootGroup.Shoots = list.ToArray();
		return spellShootGroup;
	}

	private SpellShootData NextShoot(SpellShootGroup shootGroup, IEnumerable<SlotData> hideEnhances)
	{
		if (isEnd || !cur.HasValue)
		{
			return null;
		}
		while (cur.Value.spell.IsTrigger())
		{
			_index++;
			if (isEnd)
			{
				return null;
			}
		}
		(SlotData, SlotData[])? tuple = cur;
		_index++;
		SlotData[] source = hideEnhances.ToArray();
		List<SlotData> enhancesList = tuple.Value.Item2.ToList();
		foreach (SlotData item in source.Where((SlotData e) => enhancesList.Contains(e)))
		{
			enhancesList.Remove(item);
		}
		SpellShootData spellShootData = new SpellShootData(tuple.Value.Item1, shootGroup)
		{
			EnhanceList = enhancesList.ToArray()
		};
		List<SlotData> list = new List<SlotData>();
		if ((cur.HasValue && cur.Value.spell.IsTrigger()) || tuple.Value.Item1.GetFinalConfig().isParentTypeSpell)
		{
			while (cur.HasValue && cur.Value.spell.IsTrigger())
			{
				list.Add(cur.Value.spell);
				_index++;
			}
			List<SlotData> list2 = source.ToList();
			list2.AddRange(spellShootData.EnhanceList.Where((SlotData e) => e.HindInSubGroup()));
			spellShootData.SubGroup = NextGroup(list2, spellShootData);
		}
		spellShootData.Triggers = list.ToArray();
		return spellShootData;
	}

	public static IEnumerable<SpellShootGroup> Parse(IEnumerable<SlotData> spells, int baseShootCount)
	{
		return new SpellGroupParser(spells, baseShootCount).Parse();
	}
}
