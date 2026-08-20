using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PlayerLogger;

public record Relic
{
	[CompilerGenerated]
	protected virtual Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(Relic);
		}
	}

	public int id;

	public int level;

	public static List<Relic> CreateAuto()
	{
		return PlayerMgr.Inst.BaData.relicCfgs.Select((RelicConfig e) => new Relic
		{
			id = e.id,
			level = e.level
		}).ToList();
	}

	[CompilerGenerated]
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Relic");
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
	public virtual bool Equals(Relic? other)
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
	protected Relic(Relic original)
	{
		id = original.id;
		level = original.level;
	}

	public Relic()
	{
	}
}
