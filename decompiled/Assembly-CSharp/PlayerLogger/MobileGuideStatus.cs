using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PlayerLogger;

public record MobileGuideStatus
{
	[CompilerGenerated]
	protected virtual Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(MobileGuideStatus);
		}
	}

	public int guide_id;

	public int guide_time;

	public string guide_name;

	public MobileGuideStatus(int guide_id, string guide_name, int guide_time)
	{
		this.guide_id = guide_id;
		this.guide_name = guide_name;
		this.guide_time = guide_time;
	}

	[CompilerGenerated]
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("MobileGuideStatus");
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
		builder.Append("guide_id = ");
		builder.Append(guide_id.ToString());
		builder.Append(", guide_time = ");
		builder.Append(guide_time.ToString());
		builder.Append(", guide_name = ");
		builder.Append((object)guide_name);
		return true;
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return ((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(guide_id)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(guide_time)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(guide_name);
	}

	[CompilerGenerated]
	public virtual bool Equals(MobileGuideStatus? other)
	{
		if ((object)this != other)
		{
			if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(guide_id, other!.guide_id) && EqualityComparer<int>.Default.Equals(guide_time, other!.guide_time))
			{
				return EqualityComparer<string>.Default.Equals(guide_name, other!.guide_name);
			}
			return false;
		}
		return true;
	}

	[CompilerGenerated]
	protected MobileGuideStatus(MobileGuideStatus original)
	{
		guide_id = original.guide_id;
		guide_time = original.guide_time;
		guide_name = original.guide_name;
	}
}
