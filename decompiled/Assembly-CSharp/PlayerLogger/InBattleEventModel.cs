using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace PlayerLogger;

public abstract class InBattleEventModel : InWorldEventModel
{
	public record BattleData
	{
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(BattleData);
			}
		}

		public int room_id;

		public RoomType room_type;

		public int stage;

		public DifficultyType difficulty;

		public int battle_time;

		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("BattleData");
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
			builder.Append("room_id = ");
			builder.Append(room_id.ToString());
			builder.Append(", room_type = ");
			builder.Append(room_type.ToString());
			builder.Append(", stage = ");
			builder.Append(stage.ToString());
			builder.Append(", difficulty = ");
			builder.Append(difficulty.ToString());
			builder.Append(", battle_time = ");
			builder.Append(battle_time.ToString());
			return true;
		}

		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(room_id)) * -1521134295 + EqualityComparer<RoomType>.Default.GetHashCode(room_type)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(stage)) * -1521134295 + EqualityComparer<DifficultyType>.Default.GetHashCode(difficulty)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(battle_time);
		}

		[CompilerGenerated]
		public virtual bool Equals(BattleData? other)
		{
			if ((object)this != other)
			{
				if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(room_id, other!.room_id) && EqualityComparer<RoomType>.Default.Equals(room_type, other!.room_type) && EqualityComparer<int>.Default.Equals(stage, other!.stage) && EqualityComparer<DifficultyType>.Default.Equals(difficulty, other!.difficulty))
				{
					return EqualityComparer<int>.Default.Equals(battle_time, other!.battle_time);
				}
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		protected BattleData(BattleData original)
		{
			room_id = original.room_id;
			room_type = original.room_type;
			stage = original.stage;
			difficulty = original.difficulty;
			battle_time = original.battle_time;
		}

		public BattleData()
		{
		}
	}

	private bool _inBattle = true;

	public BattleData battle_data { get; private set; }

	protected InBattleEventModel()
	{
		if (LevelMgr.Inst == null || BattleMgr.Inst == null)
		{
			_inBattle = false;
			return;
		}
		battle_data = new BattleData
		{
			room_id = LevelMgr.Inst.CurrentRoomCfg.id,
			room_type = LevelMgr.Inst.CurrentRoomCfg.type,
			stage = BattleMgr.Inst.CurrentStage,
			difficulty = DataMgr.selectedWorldData.selectedDifficulty,
			battle_time = Mathf.RoundToInt(DataMgr.selectedWorldData.timeuse)
		};
	}

	public override bool CanReport()
	{
		if (_inBattle)
		{
			return base.CanReport();
		}
		return false;
	}
}
