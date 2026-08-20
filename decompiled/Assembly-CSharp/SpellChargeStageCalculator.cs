using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SpellChargeStageCalculator
{
	public class Param
	{
		[CanBeNull]
		public readonly SpellShootData ShootData;

		[CanBeNull]
		public readonly Wand Wand;

		public readonly float Time;

		public Param([NotNull] SpellShootData shootData, float time, [CanBeNull] Wand wand)
		{
			ShootData = shootData;
			Wand = wand;
			Time = time;
		}
	}

	private static SpellChargeStageCalculator inst;

	private readonly Dictionary<int, Func<Param, int>> funcMap = new Dictionary<int, Func<Param, int>>();

	public static SpellChargeStageCalculator Inst
	{
		get
		{
			if (inst == null)
			{
				inst = new SpellChargeStageCalculator();
			}
			return inst;
		}
	}

	public SpellChargeStageCalculator()
	{
		funcMap.Add(1026, Spell1026);
		funcMap.Add(1027, Spell1027);
	}

	public static int Calculate(int spellId, Param param)
	{
		if (!Inst.funcMap.ContainsKey(spellId))
		{
			Debug.LogError($"法术 ID {spellId} 不存在计算蓄力阶段的方法。");
			return 0;
		}
		return Inst.funcMap[spellId](param);
	}

	public static int Spell1026(Param param)
	{
		if (param.ShootData == null)
		{
			Debug.LogError("没有 ShootData 不能计算 1026 的蓄力阶段");
			return 0;
		}
		float num = 0f;
		num = ((!param.Wand) ? param.ShootData.GetSpellCriticalChance().Result : param.ShootData.GetSpellCriticalChance_FinalPlayerValue(param.Wand).Result);
		float num2 = num + param.Time * param.ShootData.Spell.GetFinalConfig().float1 / 100f;
		if (num2 > 0.33f)
		{
			if (!(num2 >= 1f))
			{
				if (num2 > 0.66f)
				{
					return 3;
				}
				return 2;
			}
			return 4;
		}
		if (num2 >= 0f)
		{
			return 1;
		}
		return 0;
	}

	public static int Spell1027(Param param)
	{
		if (param.ShootData == null)
		{
			Debug.LogError("没有 ShootData 不能计算 1027蓄力时间");
			return 0;
		}
		return 0;
	}
}
