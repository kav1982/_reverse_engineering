using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace PlayerLogger;

public record TalentStatus
{
	[CompilerGenerated]
	protected virtual Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(TalentStatus);
		}
	}

	public int levelOfWandLimit;

	public int levelOfBagLimit;

	public int levelOfEnterDoorRecovery;

	public bool isTalentUnlock1;

	public int levelOfMaxHP;

	public int levelOfHPRoom;

	public bool isTalentUnlock2;

	public int levelOfInitialCoin;

	public int levelOfCoinRoom;

	public bool isTalentUnlock3;

	public int levelOfSpellRoom;

	public int levelOfRelicRoom;

	public bool isTalentUnlock4;

	public int levelOfMaxMP;

	public int levelOfMPRecover;

	public static TalentStatus CreateAuto()
	{
		return new TalentStatus
		{
			levelOfWandLimit = DataMgr.selectedWorldData.levelOfWandLimit,
			levelOfBagLimit = DataMgr.selectedWorldData.levelOfBagLimit,
			levelOfEnterDoorRecovery = DataMgr.selectedWorldData.levelOfEnterDoorRecovery,
			isTalentUnlock1 = DataMgr.selectedWorldData.isTalentUnlock1,
			levelOfMaxHP = DataMgr.selectedWorldData.levelOfMaxHP,
			levelOfHPRoom = DataMgr.selectedWorldData.levelOfHPRoom,
			isTalentUnlock2 = DataMgr.selectedWorldData.isTalentUnlock2,
			levelOfInitialCoin = DataMgr.selectedWorldData.levelOfInitialCoin,
			levelOfCoinRoom = DataMgr.selectedWorldData.levelOfCoinRoom,
			isTalentUnlock3 = DataMgr.selectedWorldData.isTalentUnlock3,
			levelOfSpellRoom = DataMgr.selectedWorldData.levelOfSpellRoom,
			levelOfRelicRoom = DataMgr.selectedWorldData.levelOfRelicRoom,
			isTalentUnlock4 = DataMgr.selectedWorldData.isTalentUnlock4,
			levelOfMaxMP = DataMgr.selectedWorldData.levelOfMaxMP,
			levelOfMPRecover = DataMgr.selectedWorldData.levelOfMPRecover
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
		stringBuilder.Append("TalentStatus");
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
		builder.Append("levelOfWandLimit = ");
		builder.Append(levelOfWandLimit.ToString());
		builder.Append(", levelOfBagLimit = ");
		builder.Append(levelOfBagLimit.ToString());
		builder.Append(", levelOfEnterDoorRecovery = ");
		builder.Append(levelOfEnterDoorRecovery.ToString());
		builder.Append(", isTalentUnlock1 = ");
		builder.Append(isTalentUnlock1.ToString());
		builder.Append(", levelOfMaxHP = ");
		builder.Append(levelOfMaxHP.ToString());
		builder.Append(", levelOfHPRoom = ");
		builder.Append(levelOfHPRoom.ToString());
		builder.Append(", isTalentUnlock2 = ");
		builder.Append(isTalentUnlock2.ToString());
		builder.Append(", levelOfInitialCoin = ");
		builder.Append(levelOfInitialCoin.ToString());
		builder.Append(", levelOfCoinRoom = ");
		builder.Append(levelOfCoinRoom.ToString());
		builder.Append(", isTalentUnlock3 = ");
		builder.Append(isTalentUnlock3.ToString());
		builder.Append(", levelOfSpellRoom = ");
		builder.Append(levelOfSpellRoom.ToString());
		builder.Append(", levelOfRelicRoom = ");
		builder.Append(levelOfRelicRoom.ToString());
		builder.Append(", isTalentUnlock4 = ");
		builder.Append(isTalentUnlock4.ToString());
		builder.Append(", levelOfMaxMP = ");
		builder.Append(levelOfMaxMP.ToString());
		builder.Append(", levelOfMPRecover = ");
		builder.Append(levelOfMPRecover.ToString());
		return true;
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return ((((((((((((((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(levelOfWandLimit)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(levelOfBagLimit)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(levelOfEnterDoorRecovery)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(isTalentUnlock1)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(levelOfMaxHP)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(levelOfHPRoom)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(isTalentUnlock2)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(levelOfInitialCoin)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(levelOfCoinRoom)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(isTalentUnlock3)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(levelOfSpellRoom)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(levelOfRelicRoom)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(isTalentUnlock4)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(levelOfMaxMP)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(levelOfMPRecover);
	}

	[CompilerGenerated]
	public virtual bool Equals(TalentStatus? other)
	{
		if ((object)this != other)
		{
			if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(levelOfWandLimit, other!.levelOfWandLimit) && EqualityComparer<int>.Default.Equals(levelOfBagLimit, other!.levelOfBagLimit) && EqualityComparer<int>.Default.Equals(levelOfEnterDoorRecovery, other!.levelOfEnterDoorRecovery) && EqualityComparer<bool>.Default.Equals(isTalentUnlock1, other!.isTalentUnlock1) && EqualityComparer<int>.Default.Equals(levelOfMaxHP, other!.levelOfMaxHP) && EqualityComparer<int>.Default.Equals(levelOfHPRoom, other!.levelOfHPRoom) && EqualityComparer<bool>.Default.Equals(isTalentUnlock2, other!.isTalentUnlock2) && EqualityComparer<int>.Default.Equals(levelOfInitialCoin, other!.levelOfInitialCoin) && EqualityComparer<int>.Default.Equals(levelOfCoinRoom, other!.levelOfCoinRoom) && EqualityComparer<bool>.Default.Equals(isTalentUnlock3, other!.isTalentUnlock3) && EqualityComparer<int>.Default.Equals(levelOfSpellRoom, other!.levelOfSpellRoom) && EqualityComparer<int>.Default.Equals(levelOfRelicRoom, other!.levelOfRelicRoom) && EqualityComparer<bool>.Default.Equals(isTalentUnlock4, other!.isTalentUnlock4) && EqualityComparer<int>.Default.Equals(levelOfMaxMP, other!.levelOfMaxMP))
			{
				return EqualityComparer<int>.Default.Equals(levelOfMPRecover, other!.levelOfMPRecover);
			}
			return false;
		}
		return true;
	}

	[CompilerGenerated]
	protected TalentStatus(TalentStatus original)
	{
		levelOfWandLimit = original.levelOfWandLimit;
		levelOfBagLimit = original.levelOfBagLimit;
		levelOfEnterDoorRecovery = original.levelOfEnterDoorRecovery;
		isTalentUnlock1 = original.isTalentUnlock1;
		levelOfMaxHP = original.levelOfMaxHP;
		levelOfHPRoom = original.levelOfHPRoom;
		isTalentUnlock2 = original.isTalentUnlock2;
		levelOfInitialCoin = original.levelOfInitialCoin;
		levelOfCoinRoom = original.levelOfCoinRoom;
		isTalentUnlock3 = original.isTalentUnlock3;
		levelOfSpellRoom = original.levelOfSpellRoom;
		levelOfRelicRoom = original.levelOfRelicRoom;
		isTalentUnlock4 = original.isTalentUnlock4;
		levelOfMaxMP = original.levelOfMaxMP;
		levelOfMPRecover = original.levelOfMPRecover;
	}

	public TalentStatus()
	{
	}
}
