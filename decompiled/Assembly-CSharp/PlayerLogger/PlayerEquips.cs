using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace PlayerLogger;

public record PlayerEquips
{
	[CompilerGenerated]
	protected virtual Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(PlayerEquips);
		}
	}

	public List<int> in_bag_spells;

	public List<Wand> wands;

	public List<int> points;

	public List<Relic> relics;

	public List<Curse> curses;

	public static PlayerEquips CreateAuto()
	{
		return new PlayerEquips
		{
			in_bag_spells = (from e in PlayerMgr.Inst.BaData.bagSpellDatas
				where e != null && !e.isSealSlot && e.id > 0
				select e.id).ToList(),
			points = PlayerMgr.Inst.BaData.potionIDs.ToList(),
			wands = Wand.CreateAuto(),
			relics = Relic.CreateAuto(),
			curses = Curse.CreateAuto()
		};
	}

	public static string GetJson()
	{
		return JsonConvert.SerializeObject(CreateAuto());
	}

	[CompilerGenerated]
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("PlayerEquips");
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
		builder.Append("in_bag_spells = ");
		builder.Append(in_bag_spells);
		builder.Append(", wands = ");
		builder.Append(wands);
		builder.Append(", points = ");
		builder.Append(points);
		builder.Append(", relics = ");
		builder.Append(relics);
		builder.Append(", curses = ");
		builder.Append(curses);
		return true;
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return ((((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(in_bag_spells)) * -1521134295 + EqualityComparer<List<Wand>>.Default.GetHashCode(wands)) * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(points)) * -1521134295 + EqualityComparer<List<Relic>>.Default.GetHashCode(relics)) * -1521134295 + EqualityComparer<List<Curse>>.Default.GetHashCode(curses);
	}

	[CompilerGenerated]
	public virtual bool Equals(PlayerEquips? other)
	{
		if ((object)this != other)
		{
			if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<List<int>>.Default.Equals(in_bag_spells, other!.in_bag_spells) && EqualityComparer<List<Wand>>.Default.Equals(wands, other!.wands) && EqualityComparer<List<int>>.Default.Equals(points, other!.points) && EqualityComparer<List<Relic>>.Default.Equals(relics, other!.relics))
			{
				return EqualityComparer<List<Curse>>.Default.Equals(curses, other!.curses);
			}
			return false;
		}
		return true;
	}

	[CompilerGenerated]
	protected PlayerEquips(PlayerEquips original)
	{
		in_bag_spells = original.in_bag_spells;
		wands = original.wands;
		points = original.points;
		relics = original.relics;
		curses = original.curses;
	}

	public PlayerEquips()
	{
	}
}
