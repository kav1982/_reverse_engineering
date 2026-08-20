using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace PlayerLogger;

public record OutBattleGoldStatue
{
	[CompilerGenerated]
	protected virtual Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(OutBattleGoldStatue);
		}
	}

	public int gold_1001;

	public int gold_1002;

	public int gold_1003;

	public static string GetJson()
	{
		return JsonUtility.ToJson(new OutBattleGoldStatue
		{
			gold_1001 = DataMgr.selectedWorldData.magicCrystalCount,
			gold_1002 = DataMgr.selectedWorldData.ancientBloodCount,
			gold_1003 = DataMgr.selectedWorldData.chaosCoreCount
		});
	}

	[CompilerGenerated]
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("OutBattleGoldStatue");
		stringBuilder.Append(" { ");
		if (PrintMembers(stringBuilder))
		{
			stringBuilder.Append(' ');
		}
		stringBuilder.Append('}');
		return stringBuilder.ToString();
	}

	[CompilerGenerated]
	protected virtual bool PrintMembers(StringBuilder builder)
	{
		RuntimeHelpers.EnsureSufficientExecutionStack();
		builder.Append("gold_1001 = ");
		builder.Append(gold_1001.ToString());
		builder.Append(", gold_1002 = ");
		builder.Append(gold_1002.ToString());
		builder.Append(", gold_1003 = ");
		builder.Append(gold_1003.ToString());
		return true;
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return ((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(gold_1001)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(gold_1002)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(gold_1003);
	}

	[CompilerGenerated]
	public virtual bool Equals(OutBattleGoldStatue? other)
	{
		if ((object)this != other)
		{
			if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(gold_1001, other!.gold_1001) && EqualityComparer<int>.Default.Equals(gold_1002, other!.gold_1002))
			{
				return EqualityComparer<int>.Default.Equals(gold_1003, other!.gold_1003);
			}
			return false;
		}
		return true;
	}

	[CompilerGenerated]
	protected OutBattleGoldStatue(OutBattleGoldStatue original)
	{
		gold_1001 = original.gold_1001;
		gold_1002 = original.gold_1002;
		gold_1003 = original.gold_1003;
	}

	public OutBattleGoldStatue()
	{
	}
}
