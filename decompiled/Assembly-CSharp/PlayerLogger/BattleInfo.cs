using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace PlayerLogger;

public record BattleInfo
{
	[CompilerGenerated]
	protected virtual Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(BattleInfo);
		}
	}

	public int maxDifficult;

	public int deadCount;

	public int finishGameCount;

	public int totalBattleCount;

	public static string GetJson()
	{
		return JsonUtility.ToJson(new BattleInfo
		{
			maxDifficult = ((DataMgr.selectedWorldData.finishedDifficulty.Count == 0) ? (-1) : DataMgr.selectedWorldData.finishedDifficulty.Max((DifficultyType difficulty) => (int)difficulty)),
			deadCount = DataMgr.selectedWorldData.deadCount,
			finishGameCount = DataMgr.finishGameBuilds.finishGameBuilds.Count,
			totalBattleCount = DataMgr.selectedWorldData.enterBattleTime
		});
	}

	[CompilerGenerated]
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("BattleInfo");
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
		builder.Append("maxDifficult = ");
		builder.Append(maxDifficult.ToString());
		builder.Append(", deadCount = ");
		builder.Append(deadCount.ToString());
		builder.Append(", finishGameCount = ");
		builder.Append(finishGameCount.ToString());
		builder.Append(", totalBattleCount = ");
		builder.Append(totalBattleCount.ToString());
		return true;
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return (((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(maxDifficult)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(deadCount)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(finishGameCount)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(totalBattleCount);
	}

	[CompilerGenerated]
	public virtual bool Equals(BattleInfo? other)
	{
		if ((object)this != other)
		{
			if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(maxDifficult, other!.maxDifficult) && EqualityComparer<int>.Default.Equals(deadCount, other!.deadCount) && EqualityComparer<int>.Default.Equals(finishGameCount, other!.finishGameCount))
			{
				return EqualityComparer<int>.Default.Equals(totalBattleCount, other!.totalBattleCount);
			}
			return false;
		}
		return true;
	}

	[CompilerGenerated]
	protected BattleInfo(BattleInfo original)
	{
		maxDifficult = original.maxDifficult;
		deadCount = original.deadCount;
		finishGameCount = original.finishGameCount;
		totalBattleCount = original.totalBattleCount;
	}

	public BattleInfo()
	{
	}
}
