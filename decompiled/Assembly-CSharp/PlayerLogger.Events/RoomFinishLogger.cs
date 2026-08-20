using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PlayerLogger.Events;

public class RoomFinishLogger : InBattleEventModel
{
	public record CursedChestInfo
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(CursedChestInfo);
			}
		}

		public int spawn_count;

		public List<int> spawn_curse = new List<int>();

		public int open_count;

		public List<int> open_curse = new List<int>();

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CursedChestInfo");
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
			builder.Append("spawn_count = ");
			builder.Append(spawn_count.ToString());
			builder.Append(", spawn_curse = ");
			builder.Append(spawn_curse);
			builder.Append(", open_count = ");
			builder.Append(open_count.ToString());
			builder.Append(", open_curse = ");
			builder.Append(open_curse);
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(spawn_count)) * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(spawn_curse)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(open_count)) * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(open_curse);
		}

		[CompilerGenerated]
		public virtual bool Equals(CursedChestInfo? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(spawn_count, other!.spawn_count) && EqualityComparer<List<int>>.Default.Equals(spawn_curse, other!.spawn_curse) && EqualityComparer<int>.Default.Equals(open_count, other!.open_count))
				{
					return EqualityComparer<List<int>>.Default.Equals(open_curse, other!.open_curse);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected CursedChestInfo(CursedChestInfo original)
		{
			spawn_count = original.spawn_count;
			spawn_curse = original.spawn_curse;
			open_count = original.open_count;
			open_curse = original.open_curse;
		}

		public CursedChestInfo()
		{
		}
	}

	public record LockedChestInfo
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(LockedChestInfo);
			}
		}

		public int spawn_count;

		public int open_count;

		public int cost_keys;

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("LockedChestInfo");
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
			builder.Append("spawn_count = ");
			builder.Append(spawn_count.ToString());
			builder.Append(", open_count = ");
			builder.Append(open_count.ToString());
			builder.Append(", cost_keys = ");
			builder.Append(cost_keys.ToString());
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(spawn_count)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(open_count)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(cost_keys);
		}

		[CompilerGenerated]
		public virtual bool Equals(LockedChestInfo? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(spawn_count, other!.spawn_count) && EqualityComparer<int>.Default.Equals(open_count, other!.open_count))
				{
					return EqualityComparer<int>.Default.Equals(cost_keys, other!.cost_keys);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected LockedChestInfo(LockedChestInfo original)
		{
			spawn_count = original.spawn_count;
			open_count = original.open_count;
			cost_keys = original.cost_keys;
		}

		public LockedChestInfo()
		{
		}
	}

	public record SideRoomInfo
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(SideRoomInfo);
			}
		}

		public int id;

		public RoomType type;

		public FourDir dir;

		public bool unlocked;

		public List<Item> reward;

		public int spend_seconds;

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SideRoomInfo");
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
			builder.Append(", type = ");
			builder.Append(type.ToString());
			builder.Append(", dir = ");
			builder.Append(dir.ToString());
			builder.Append(", unlocked = ");
			builder.Append(unlocked.ToString());
			builder.Append(", reward = ");
			builder.Append(reward);
			builder.Append(", spend_seconds = ");
			builder.Append(spend_seconds.ToString());
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(id)) * -1521134295 + EqualityComparer<RoomType>.Default.GetHashCode(type)) * -1521134295 + EqualityComparer<FourDir>.Default.GetHashCode(dir)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(unlocked)) * -1521134295 + EqualityComparer<List<Item>>.Default.GetHashCode(reward)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(spend_seconds);
		}

		[CompilerGenerated]
		public virtual bool Equals(SideRoomInfo? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(id, other!.id) && EqualityComparer<RoomType>.Default.Equals(type, other!.type) && EqualityComparer<FourDir>.Default.Equals(dir, other!.dir) && EqualityComparer<bool>.Default.Equals(unlocked, other!.unlocked) && EqualityComparer<List<Item>>.Default.Equals(reward, other!.reward))
				{
					return EqualityComparer<int>.Default.Equals(spend_seconds, other!.spend_seconds);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected SideRoomInfo(SideRoomInfo original)
		{
			id = original.id;
			type = original.type;
			dir = original.dir;
			unlocked = original.unlocked;
			reward = original.reward;
			spend_seconds = original.spend_seconds;
		}

		public SideRoomInfo()
		{
		}
	}

	public record Reward
	{
		public record Coin : Reward
		{
			[CompilerGenerated]
			protected override Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(Coin);
				}
			}

			public int number;

			public Coin()
				: base("Coins")
			{
			}

			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Coin");
				stringBuilder.Append(" { ");
				if (PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			[CompilerGenerated]
			protected override bool PrintMembers(StringBuilder builder)
			{
				if (base.PrintMembers(builder))
				{
					builder.Append(", ");
				}
				builder.Append("number = ");
				builder.Append(number.ToString());
				return true;
			}

			[CompilerGenerated]
			public override int GetHashCode()
			{
				return base.GetHashCode() * -1521134295 + EqualityComparer<int>.Default.GetHashCode(number);
			}

			[CompilerGenerated]
			public virtual bool Equals(Coin? other)
			{
				if ((object)this != other)
				{
					if (base.Equals(other))
					{
						return EqualityComparer<int>.Default.Equals(number, other!.number);
					}
					return false;
				}
				return true;
			}

			[CompilerGenerated]
			protected Coin(Coin original)
				: base(original)
			{
				number = original.number;
			}
		}

		public record MaxHp : Reward
		{
			[CompilerGenerated]
			protected override Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(MaxHp);
				}
			}

			public int number;

			public MaxHp()
				: base("MaxHp")
			{
			}

			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("MaxHp");
				stringBuilder.Append(" { ");
				if (PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			[CompilerGenerated]
			protected override bool PrintMembers(StringBuilder builder)
			{
				if (base.PrintMembers(builder))
				{
					builder.Append(", ");
				}
				builder.Append("number = ");
				builder.Append(number.ToString());
				return true;
			}

			[CompilerGenerated]
			public override int GetHashCode()
			{
				return base.GetHashCode() * -1521134295 + EqualityComparer<int>.Default.GetHashCode(number);
			}

			[CompilerGenerated]
			public virtual bool Equals(MaxHp? other)
			{
				if ((object)this != other)
				{
					if (base.Equals(other))
					{
						return EqualityComparer<int>.Default.Equals(number, other!.number);
					}
					return false;
				}
				return true;
			}

			[CompilerGenerated]
			protected MaxHp(MaxHp original)
				: base(original)
			{
				number = original.number;
			}
		}

		public record Spell : Reward
		{
			[CompilerGenerated]
			protected override Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(Spell);
				}
			}

			public List<int> options;

			public List<int> selected;

			public int rerollTime;

			public int remainRerollTime;

			public Spell()
				: base("Spells")
			{
			}

			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Spell");
				stringBuilder.Append(" { ");
				if (PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			[CompilerGenerated]
			protected override bool PrintMembers(StringBuilder builder)
			{
				if (base.PrintMembers(builder))
				{
					builder.Append(", ");
				}
				builder.Append("options = ");
				builder.Append(options);
				builder.Append(", selected = ");
				builder.Append(selected);
				builder.Append(", rerollTime = ");
				builder.Append(rerollTime.ToString());
				builder.Append(", remainRerollTime = ");
				builder.Append(remainRerollTime.ToString());
				return true;
			}

			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((base.GetHashCode() * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(options)) * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(selected)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(rerollTime)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(remainRerollTime);
			}

			[CompilerGenerated]
			public virtual bool Equals(Spell? other)
			{
				if ((object)this != other)
				{
					if (base.Equals(other) && EqualityComparer<List<int>>.Default.Equals(options, other!.options) && EqualityComparer<List<int>>.Default.Equals(selected, other!.selected) && EqualityComparer<int>.Default.Equals(rerollTime, other!.rerollTime))
					{
						return EqualityComparer<int>.Default.Equals(remainRerollTime, other!.remainRerollTime);
					}
					return false;
				}
				return true;
			}

			[CompilerGenerated]
			protected Spell(Spell original)
				: base(original)
			{
				options = original.options;
				selected = original.selected;
				rerollTime = original.rerollTime;
				remainRerollTime = original.remainRerollTime;
			}
		}

		public record Relic : Reward
		{
			[CompilerGenerated]
			protected override Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(Relic);
				}
			}

			public List<int> options;

			public List<int> selected;

			public int rerollTime;

			public int remainRerollTime;

			public Relic()
				: base("Relics")
			{
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
			protected override bool PrintMembers(StringBuilder builder)
			{
				if (base.PrintMembers(builder))
				{
					builder.Append(", ");
				}
				builder.Append("options = ");
				builder.Append(options);
				builder.Append(", selected = ");
				builder.Append(selected);
				builder.Append(", rerollTime = ");
				builder.Append(rerollTime.ToString());
				builder.Append(", remainRerollTime = ");
				builder.Append(remainRerollTime.ToString());
				return true;
			}

			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((base.GetHashCode() * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(options)) * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(selected)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(rerollTime)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(remainRerollTime);
			}

			[CompilerGenerated]
			public virtual bool Equals(Relic? other)
			{
				if ((object)this != other)
				{
					if (base.Equals(other) && EqualityComparer<List<int>>.Default.Equals(options, other!.options) && EqualityComparer<List<int>>.Default.Equals(selected, other!.selected) && EqualityComparer<int>.Default.Equals(rerollTime, other!.rerollTime))
					{
						return EqualityComparer<int>.Default.Equals(remainRerollTime, other!.remainRerollTime);
					}
					return false;
				}
				return true;
			}

			[CompilerGenerated]
			protected Relic(Relic original)
				: base(original)
			{
				options = original.options;
				selected = original.selected;
				rerollTime = original.rerollTime;
				remainRerollTime = original.remainRerollTime;
			}
		}

		public record Wand : Reward
		{
			[CompilerGenerated]
			protected override Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(Wand);
				}
			}

			public List<int> options;

			public int selected;

			public Wand()
				: base("Wands")
			{
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
			protected override bool PrintMembers(StringBuilder builder)
			{
				if (base.PrintMembers(builder))
				{
					builder.Append(", ");
				}
				builder.Append("options = ");
				builder.Append(options);
				builder.Append(", selected = ");
				builder.Append(selected.ToString());
				return true;
			}

			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (base.GetHashCode() * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(options)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(selected);
			}

			[CompilerGenerated]
			public virtual bool Equals(Wand? other)
			{
				if ((object)this != other)
				{
					if (base.Equals(other) && EqualityComparer<List<int>>.Default.Equals(options, other!.options))
					{
						return EqualityComparer<int>.Default.Equals(selected, other!.selected);
					}
					return false;
				}
				return true;
			}

			[CompilerGenerated]
			protected Wand(Wand original)
				: base(original)
			{
				options = original.options;
				selected = original.selected;
			}
		}

		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(Reward);
			}
		}

		public string type { get; private set; }

		private Reward(string type)
		{
			this.type = type;
		}

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Reward");
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
			builder.Append((object)type);
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(type);
		}

		[CompilerGenerated]
		public virtual bool Equals(Reward? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract)
				{
					return EqualityComparer<string>.Default.Equals(type, other!.type);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected Reward(Reward original)
		{
			type = original.type;
		}
	}

	public string current_room;

	public int spend_seconds;

	public List<Reward> rewards;

	public LevelRewardType next_room_selected;

	public List<LevelRewardType> next_room_options;

	public PlayerEquips entry_equips;

	public PlayerEquips finish_equips;

	public ResourcesStatus entry_resources;

	public ResourcesStatus finish_resources;

	public ResourcesStatus flow_resources;

	public CursedChestInfo cursed_chest;

	public LockedChestInfo locked_chest;

	public List<SideRoomInfo> side_room;

	public override string GetEventName()
	{
		return "room_finish";
	}

	public void AddCurrentSideRoomReward(Item item)
	{
		side_room.FirstOrDefault((SideRoomInfo e) => e.id == LevelMgr.Inst.CurrentRoomCfg.id)?.reward.Add(item);
	}
}
