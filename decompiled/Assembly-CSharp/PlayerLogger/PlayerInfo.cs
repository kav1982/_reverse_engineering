using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Steamworks;

namespace PlayerLogger;

public record PlayerInfo
{
	[CompilerGenerated]
	protected virtual Type EqualityContract
	{
		[CompilerGenerated]
		get
		{
			return typeof(PlayerInfo);
		}
	}

	public string role_name = "";

	public string level = "";

	public string b_account_id = "";

	public string b_role_id = "";

	public string b_zone_id = "";

	public string b_sdk_uid = "";

	public PlayerInfo()
	{
		if (SteamManager.Initialized)
		{
			role_name = SteamFriends.GetPersonaName();
			b_role_id = SteamUser.GetSteamID().ToString();
		}
		else
		{
			role_name = "Unknown steam user name";
			b_role_id = "Unknown steam id";
		}
		b_zone_id = VersionSO.Inst.Branch;
	}

	[CompilerGenerated]
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("PlayerInfo");
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
		builder.Append("role_name = ");
		builder.Append((object)role_name);
		builder.Append(", level = ");
		builder.Append((object)level);
		builder.Append(", b_account_id = ");
		builder.Append((object)b_account_id);
		builder.Append(", b_role_id = ");
		builder.Append((object)b_role_id);
		builder.Append(", b_zone_id = ");
		builder.Append((object)b_zone_id);
		builder.Append(", b_sdk_uid = ");
		builder.Append((object)b_sdk_uid);
		return true;
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return (((((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(role_name)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(level)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(b_account_id)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(b_role_id)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(b_zone_id)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(b_sdk_uid);
	}

	[CompilerGenerated]
	public virtual bool Equals(PlayerInfo? other)
	{
		if ((object)this != other)
		{
			if ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<string>.Default.Equals(role_name, other!.role_name) && EqualityComparer<string>.Default.Equals(level, other!.level) && EqualityComparer<string>.Default.Equals(b_account_id, other!.b_account_id) && EqualityComparer<string>.Default.Equals(b_role_id, other!.b_role_id) && EqualityComparer<string>.Default.Equals(b_zone_id, other!.b_zone_id))
			{
				return EqualityComparer<string>.Default.Equals(b_sdk_uid, other!.b_sdk_uid);
			}
			return false;
		}
		return true;
	}

	[CompilerGenerated]
	protected PlayerInfo(PlayerInfo original)
	{
		role_name = original.role_name;
		level = original.level;
		b_account_id = original.b_account_id;
		b_role_id = original.b_role_id;
		b_zone_id = original.b_zone_id;
		b_sdk_uid = original.b_sdk_uid;
	}
}
