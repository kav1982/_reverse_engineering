using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PlayerLogger;

public record Item
{
	public enum Type
	{
		Unknown,
		Spell,
		Relic,
		Curse,
		Wand,
		Potion,
		Coin,
		Key,
		Crystal,
		Ancient_blood,
		Chaos_core,
		Hp,
		Shield
	}

	[CompilerGenerated]
	protected virtual System.Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(Item);
		}
	}

	public Type type;

	public int id;

	public int number;

	public static List<Item> Create(List<ItemInfo> output)
	{
		Dictionary<(int, ItemType), Item> dictionary = new Dictionary<(int, ItemType), Item>();
		foreach (ItemInfo item in output)
		{
			(int, ItemType) key = (item.id, item.type);
			if (dictionary.ContainsKey(key))
			{
				dictionary[key].number++;
				continue;
			}
			dictionary[key] = new Item
			{
				number = 1,
				id = key.Item1,
				type = ItemTypeConvert(item)
			};
		}
		return dictionary.Values.ToList();
	}

	public static Type ItemTypeConvert(ItemInfo itemInfo)
	{
		return itemInfo.type switch
		{
			ItemType.Curse => Type.Curse, 
			ItemType.Potion => Type.Potion, 
			ItemType.Relic => Type.Relic, 
			ItemType.Wand => Type.Wand, 
			ItemType.Spell => Type.Spell, 
			ItemType.Resource => itemInfo.id switch
			{
				11 => Type.Coin, 
				32 => Type.Hp, 
				42 => Type.Shield, 
				_ => Type.Unknown, 
			}, 
			_ => Type.Unknown, 
		};
	}

	public static Item CreateSpell(int spellId)
	{
		return new Item
		{
			id = spellId,
			type = Type.Spell,
			number = 1
		};
	}

	[CompilerGenerated]
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Item");
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
		builder.Append("type = ");
		builder.Append(type.ToString());
		builder.Append(", id = ");
		builder.Append(id.ToString());
		builder.Append(", number = ");
		builder.Append(number.ToString());
		return true;
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return ((EqualityComparer<System.Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(type)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(id)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(number);
	}

	[CompilerGenerated]
	public virtual bool Equals(Item? other)
	{
		if ((object)this != other)
		{
			if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<Type>.Default.Equals(type, other!.type) && EqualityComparer<int>.Default.Equals(id, other!.id))
			{
				return EqualityComparer<int>.Default.Equals(number, other!.number);
			}
			return false;
		}
		return true;
	}

	[CompilerGenerated]
	protected Item(Item original)
	{
		type = original.type;
		id = original.id;
		number = original.number;
	}

	public Item()
	{
	}
}
