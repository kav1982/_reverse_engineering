using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace PlayerLogger;

public record InBattleGoldStatue
{
	[CompilerGenerated]
	protected virtual Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(InBattleGoldStatue);
		}
	}

	public int coin;

	public int key;

	public static string GetJson()
	{
		return JsonUtility.ToJson(new InBattleGoldStatue
		{
			coin = PlayerMgr.Inst.CoinCount,
			key = PlayerMgr.Inst.KeyCount
		});
	}

	[CompilerGenerated]
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("InBattleGoldStatue");
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
		builder.Append("coin = ");
		builder.Append(coin.ToString());
		builder.Append(", key = ");
		builder.Append(key.ToString());
		return true;
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(coin)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(key);
	}

	[CompilerGenerated]
	public virtual bool Equals(InBattleGoldStatue? other)
	{
		if ((object)this != other)
		{
			if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(coin, other!.coin))
			{
				return EqualityComparer<int>.Default.Equals(key, other!.key);
			}
			return false;
		}
		return true;
	}

	[CompilerGenerated]
	protected InBattleGoldStatue(InBattleGoldStatue original)
	{
		coin = original.coin;
		key = original.key;
	}

	public InBattleGoldStatue()
	{
	}
}
