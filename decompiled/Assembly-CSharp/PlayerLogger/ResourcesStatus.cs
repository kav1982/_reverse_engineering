using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PlayerLogger;

public record ResourcesStatus
{
	[CompilerGenerated]
	protected virtual Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(ResourcesStatus);
		}
	}

	public int coin;

	public int key;

	public int crystal;

	public int ancient_blood;

	public int chaos_core;

	public int gear;

	public float max_hp;

	public float hp;

	public float shield;

	public float temp_shield;

	public static ResourcesStatus CreateAuto()
	{
		ResourcesStatus resourcesStatus = new ResourcesStatus();
		resourcesStatus.coin = PlayerMgr.Inst.CoinCount;
		resourcesStatus.key = PlayerMgr.Inst.KeyCount;
		resourcesStatus.crystal = DataMgr.selectedWorldData.magicCrystalCount;
		resourcesStatus.ancient_blood = DataMgr.selectedWorldData.ancientBloodCount;
		resourcesStatus.chaos_core = DataMgr.selectedWorldData.chaosCoreCount;
		resourcesStatus.gear = DataMgr.selectedWorldData.GearCount;
		if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
		{
			resourcesStatus.max_hp = playerPpt.unitCfg.maxHP;
			resourcesStatus.hp = playerPpt.unitCfg.currentHP;
			resourcesStatus.shield = playerPpt.unitCfg.shield;
			resourcesStatus.temp_shield = playerPpt.unitCfg.shieldTemp;
		}
		return resourcesStatus;
	}

	public static ResourcesStatus CreateFlow(ResourcesStatus before, ResourcesStatus after)
	{
		return new ResourcesStatus
		{
			coin = after.coin - before.coin,
			key = after.key - before.key,
			crystal = after.crystal - before.crystal,
			ancient_blood = after.ancient_blood - before.ancient_blood,
			chaos_core = after.chaos_core - before.chaos_core,
			gear = after.gear - before.gear,
			max_hp = after.max_hp - before.max_hp,
			hp = after.hp - before.hp,
			shield = after.shield - before.shield,
			temp_shield = after.temp_shield - before.temp_shield
		};
	}

	[CompilerGenerated]
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("ResourcesStatus");
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
		builder.Append("coin = ");
		builder.Append(coin.ToString());
		builder.Append(", key = ");
		builder.Append(key.ToString());
		builder.Append(", crystal = ");
		builder.Append(crystal.ToString());
		builder.Append(", ancient_blood = ");
		builder.Append(ancient_blood.ToString());
		builder.Append(", chaos_core = ");
		builder.Append(chaos_core.ToString());
		builder.Append(", gear = ");
		builder.Append(gear.ToString());
		builder.Append(", max_hp = ");
		builder.Append(max_hp.ToString());
		builder.Append(", hp = ");
		builder.Append(hp.ToString());
		builder.Append(", shield = ");
		builder.Append(shield.ToString());
		builder.Append(", temp_shield = ");
		builder.Append(temp_shield.ToString());
		return true;
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return (((((((((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(coin)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(key)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(crystal)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(ancient_blood)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(chaos_core)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(gear)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(max_hp)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(hp)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(shield)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(temp_shield);
	}

	[CompilerGenerated]
	public virtual bool Equals(ResourcesStatus? other)
	{
		if ((object)this != other)
		{
			if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<int>.Default.Equals(coin, other!.coin) && EqualityComparer<int>.Default.Equals(key, other!.key) && EqualityComparer<int>.Default.Equals(crystal, other!.crystal) && EqualityComparer<int>.Default.Equals(ancient_blood, other!.ancient_blood) && EqualityComparer<int>.Default.Equals(chaos_core, other!.chaos_core) && EqualityComparer<int>.Default.Equals(gear, other!.gear) && EqualityComparer<float>.Default.Equals(max_hp, other!.max_hp) && EqualityComparer<float>.Default.Equals(hp, other!.hp) && EqualityComparer<float>.Default.Equals(shield, other!.shield))
			{
				return EqualityComparer<float>.Default.Equals(temp_shield, other!.temp_shield);
			}
			return false;
		}
		return true;
	}

	[CompilerGenerated]
	protected ResourcesStatus(ResourcesStatus original)
	{
		coin = original.coin;
		key = original.key;
		crystal = original.crystal;
		ancient_blood = original.ancient_blood;
		chaos_core = original.chaos_core;
		gear = original.gear;
		max_hp = original.max_hp;
		hp = original.hp;
		shield = original.shield;
		temp_shield = original.temp_shield;
	}

	public ResourcesStatus()
	{
	}
}
