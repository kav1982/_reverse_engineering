using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace PlayerLogger;

public record Suit
{
	[CompilerGenerated]
	protected virtual Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(Suit);
		}
	}

	public int id;

	public int level;

	public static List<Suit> CreateAuto()
	{
		return DataMgr.selectedWorldData.setUnlockedSets.Select((KeyValuePair<int, int> item) => new Suit
		{
			id = item.Key,
			level = item.Value
		}).ToList();
	}

	public static string GetJson()
	{
		return JsonConvert.SerializeObject(CreateAuto());
	}

	[CompilerGenerated]
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Suit");
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
		builder.Append("id = ");
		builder.Append(id.ToString());
		builder.Append(", level = ");
		builder.Append(level.ToString());
		return true;
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(id)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(level);
	}

	[CompilerGenerated]
	public virtual bool Equals(Suit? other)
	{
		if ((object)this != other)
		{
			if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(id, other!.id))
			{
				return EqualityComparer<int>.Default.Equals(level, other!.level);
			}
			return false;
		}
		return true;
	}

	[CompilerGenerated]
	protected Suit(Suit original)
	{
		id = original.id;
		level = original.level;
	}

	public Suit()
	{
	}
}
