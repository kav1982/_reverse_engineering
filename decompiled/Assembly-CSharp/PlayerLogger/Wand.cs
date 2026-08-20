using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PlayerLogger;

public record Wand
{
	[CompilerGenerated]
	protected virtual Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(Wand);
		}
	}

	public int id;

	public List<int> normal_slot_spells;

	public List<int> post_slot_spells;

	public static List<Wand> CreateAuto()
	{
		return (from e in PlayerMgr.Inst.BaData.wandCfgs
			where e != null && e.id > 0
			select e into wandCfg
			select new Wand
			{
				id = wandCfg.id,
				normal_slot_spells = (from e in wandCfg.normalSlots
					where e != null && !e.isSealSlot && e.id > 0
					select e.id).ToList(),
				post_slot_spells = (from e in wandCfg.postSlots
					where e != null && !e.isSealSlot && e.id > 0
					select e.id).ToList()
			}).ToList();
	}

	[CompilerGenerated]
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Wand");
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
		builder.Append(", normal_slot_spells = ");
		builder.Append(normal_slot_spells);
		builder.Append(", post_slot_spells = ");
		builder.Append(post_slot_spells);
		return true;
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return ((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(id)) * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(normal_slot_spells)) * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(post_slot_spells);
	}

	[CompilerGenerated]
	public virtual bool Equals(Wand? other)
	{
		if ((object)this != other)
		{
			if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(id, other!.id) && EqualityComparer<List<int>>.Default.Equals(normal_slot_spells, other!.normal_slot_spells))
			{
				return EqualityComparer<List<int>>.Default.Equals(post_slot_spells, other!.post_slot_spells);
			}
			return false;
		}
		return true;
	}

	[CompilerGenerated]
	protected Wand(Wand original)
	{
		id = original.id;
		normal_slot_spells = original.normal_slot_spells;
		post_slot_spells = original.post_slot_spells;
	}

	public Wand()
	{
	}
}
