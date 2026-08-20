using Unity.Entities;

public static class HammerAttackRangeMethods
{
	public static bool TryGetWandHammerAttackRange(this ref HammerAttackRangeInWandSingleton comp, UnityObjectRef<Wand> wand, out float range)
	{
		return comp.WandsFirstHammerAttackRange.TryGetValue(wand, out range);
	}

	public static void ClearHammerAttackRange(this ref HammerAttackRangeInWandSingleton comp, UnityObjectRef<Wand> wand)
	{
		comp.WandsFirstHammerAttackRange.Remove(wand);
	}

	public static void SetWandAttackRange(this ref HammerAttackRangeInWandSingleton comp, UnityObjectRef<Wand> wand, float range)
	{
		if (comp.WandsFirstHammerAttackRange.ContainsKey(wand))
		{
			comp.WandsFirstHammerAttackRange[wand] = range;
		}
		else
		{
			comp.WandsFirstHammerAttackRange.Add(wand, range);
		}
	}
}
