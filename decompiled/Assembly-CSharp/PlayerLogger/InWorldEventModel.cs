using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace PlayerLogger;

public abstract class InWorldEventModel : EventModel
{
	public record WorldData
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(WorldData);
			}
		}

		public int play_time;

		public long created_time;

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("WorldData");
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
			builder.Append("play_time = ");
			builder.Append(play_time.ToString());
			builder.Append(", created_time = ");
			builder.Append(created_time.ToString());
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(play_time)) * -1521134295 + EqualityComparer<long>.Default.GetHashCode(created_time);
		}

		[CompilerGenerated]
		public virtual bool Equals(WorldData? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(play_time, other!.play_time))
				{
					return EqualityComparer<long>.Default.Equals(created_time, other!.created_time);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected WorldData(WorldData original)
		{
			play_time = original.play_time;
			created_time = original.created_time;
		}

		public WorldData()
		{
		}
	}

	public WorldData world_data { get; private set; }

	protected InWorldEventModel()
	{
		world_data = new WorldData
		{
			play_time = Mathf.RoundToInt(DataMgr.selectedWorldData.playTime),
			created_time = DataMgr.selectedWorldData.timeStampOnStartUsing
		};
	}
}
