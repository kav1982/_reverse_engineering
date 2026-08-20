using Unity.Entities;
using Unity.Mathematics;

public readonly struct SpellSpawnParamsStorage
{
	private readonly SpellSpawnParams Params;

	public SpellSpawnParamsStorage(SpellSpawnParams spellParams)
	{
		Params = spellParams;
	}

	private static void ClearTriggerInfoAndSubGroup(ref SpellSpawnParams ssp)
	{
		ssp.SubGroupEntity = Entity.Null;
		ssp.OverSplitTriggerBufferEntity = Entity.Null;
		ssp.HitTriggerComponentData = default(SpellHitTriggerComponentData);
		ssp.MoveTriggerComponentData = default(SpellMoveTriggerComponentData);
		ssp.OverTriggerComponentData = default(SpellOverTriggerComponentData);
		ssp.TwineTriggerComponentData = default(SpellTwineTriggerComponentData);
	}

	public SpellSpawnParams BuildMagicBreakerRefractBullet(UnitType targetType, float3 spawnPosition, float3 direction)
	{
		SpellSpawnParams @params = Params;
		@params.DisableShootSound = true;
		@params.MovementComponentData.Type = SpellSpecialMovementType.Normal;
		@params.ConfigComponentData.ShooterType = targetType;
		@params.ConfigComponentData.ColorType = ((targetType != 0) ? SpellColorType.Monster : SpellColorType.Player);
		@params.MovementComponentData.Direction = direction;
		@params.MovementComponentData.Speed *= 1.5f;
		@params.SpawnPosition = spawnPosition;
		return @params;
	}

	public SpellSpawnParams ToSplit(Entity sourceSpell, SpellSplitComponentData splitData)
	{
		SpellSpawnParams ssp = Params;
		ClearTriggerInfoAndSubGroup(ref ssp);
		ssp.SetShooter(sourceSpell, ssp.OwnerUnit);
		ssp.MovementComponentData.AroundTarget = sourceSpell;
		ssp.IsSplitSpell = true;
		ssp.SplitComponentData.Count = 0;
		ssp.ConfigComponentData.Damage.MulRatio *= splitData.DamageRatio;
		ssp.ConfigComponentData.Radius.Base *= 0.8f;
		ssp.ConfigComponentData.Radius.Extra *= 0.8f;
		ssp.ConfigComponentData.Knockback *= 0.5f;
		if (ssp.MovementComponentData.IsFallSpell)
		{
			ssp.MovementComponentData.FallingReboundForceRatio *= 0.7f;
			ssp.MovementComponentData.IsFallRebounded = true;
			ssp.MovementComponentData.ReboundFallSpeed();
		}
		else
		{
			ssp.MovementComponentData.Speed *= 0.5f;
		}
		return ssp;
	}

	public SpellSpawnParams BuildMiniMeteor(Entity sourceSpell, int index, float3 spawnPosition, float3 direction, float aroundAngle)
	{
		SpellSpawnParams ssp = Params;
		ClearTriggerInfoAndSubGroup(ref ssp);
		float3 @float = spawnPosition - ssp.SpawnPosition;
		ssp.SetShooter(sourceSpell, ssp.OwnerUnit);
		ssp.MovementComponentData.AroundTarget = sourceSpell;
		ssp.SpawnPosition = spawnPosition;
		ssp.MovementComponentData.FallTargetPosition += @float;
		ssp.MovementComponentData.Direction = direction;
		ssp.MovementComponentData.AroundAngle = aroundAngle;
		ssp.ConfigComponentData.Int3 = index;
		return ssp;
	}

	public float GetManaCost()
	{
		return Params.ManaCost;
	}

	public SpellSpawnParams BuildHighPressureWaterPoint(Entity aroundTarget, float3 aroundCenter, Entity shooterEntity, Entity ownerEntity, float duration, float3 spawnPosition, float3 direction)
	{
		SpellSpawnParams ssp = Params;
		ClearTriggerInfoAndSubGroup(ref ssp);
		ssp.SplitComponentData.Count = 0;
		ssp.SpawnPosition = spawnPosition;
		ssp.MovementComponentData.AroundCenter = aroundCenter;
		ssp.MovementComponentData.Direction = direction;
		ssp.MovementComponentData.AroundTarget = aroundTarget;
		ssp.ConfigComponentData.HoverDuration = 0f;
		ssp.ConfigComponentData.Duration.Base = 0f;
		ssp.ConfigComponentData.Duration.Extra = duration + 0.25f;
		ssp.ConfigComponentData.Int3 = 1;
		ssp.IgnoreSpawnLightningChain = true;
		ssp.SetShooter(shooterEntity, ownerEntity);
		return ssp;
	}

	public SpellSpawnParams BuildGhostFire(float duration, float3 spawnPosition, float3 direction, float aroundAngle, Entity shooter, Entity ownerUnit, int reboundCount, int remainHalfLifeTeleportCount)
	{
		SpellSpawnParams ssp = Params;
		ClearTriggerInfoAndSubGroup(ref ssp);
		ssp.SetShooter(shooter, ownerUnit);
		ssp.MovementComponentData.AroundTarget = shooter;
		ssp.SpawnPosition = spawnPosition;
		ssp.MovementComponentData.FallTargetPosition = spawnPosition + direction;
		ssp.ConfigComponentData.Duration = new AttributeValue(duration);
		ssp.MovementComponentData.Direction = direction;
		ssp.MovementComponentData.AroundAngle = aroundAngle;
		ssp.MovementComponentData.ReboundCount = reboundCount;
		ssp.HalfLifeTeleportCount = remainHalfLifeTeleportCount;
		return ssp;
	}

	public SpellSpawnParams BuildFuseTeammate(Entity sourceSpell, float3 spawnPosition, TeammateData MainFuseTeammate, TeammateData MainSubTeammate)
	{
		SpellSpawnParams ssp = Params;
		ClearTriggerInfoAndSubGroup(ref ssp);
		ssp.TeammateComponentData.TeammateMaxFuseLevel = math.max(MainFuseTeammate.TeammateMaxFuseLevel, MainSubTeammate.TeammateMaxFuseLevel);
		ssp.TeammateComponentData.TeammateCurrentFuseLevel = math.min(MainFuseTeammate.TeammateCurrentFuseLevel + MainSubTeammate.TeammateCurrentFuseLevel + 1, ssp.TeammateComponentData.TeammateMaxFuseLevel);
		ssp.SetShooter(sourceSpell, ssp.OwnerUnit);
		ssp.MovementComponentData.AroundTarget = sourceSpell;
		ssp.SpawnPosition = spawnPosition;
		return ssp;
	}

	public SpellSpawnParams BuildBoboBullet(Entity sourceSpell, float3 spawnPosition, float3 targetPosition, float3 direction, float damage, float speed, float bonusDuration)
	{
		SpellSpawnParams ssp = Params;
		ClearTriggerInfoAndSubGroup(ref ssp);
		ssp.PrefabId = 9001;
		ssp.ConfigComponentData.Id = 90011;
		ssp.ConfigComponentData.AbilityType = SpellAbilityType.Summon1;
		ssp.ConfigComponentData.Damage.Base = damage;
		ssp.ConfigComponentData.Knockback = 3f;
		ssp.SetShooter(sourceSpell, ssp.OwnerUnit);
		ssp.MovementComponentData.AroundTarget = sourceSpell;
		if (!ssp.MovementComponentData.IsFallSpell)
		{
			ssp.SpawnPosition = spawnPosition;
		}
		ssp.MovementComponentData.FallTargetPosition = targetPosition;
		ssp.MovementComponentData.Speed = speed;
		ssp.MovementComponentData.OriginalSpellHorizontalSpeed = speed;
		ssp.MovementComponentData.Direction = direction;
		if (ssp.MovementComponentData.Type == SpellSpecialMovementType.Rotation)
		{
			ssp.ConfigComponentData.Duration.Base = 3f;
			ssp.ConfigComponentData.Duration.Extra += bonusDuration;
			ssp.MovementComponentData.Speed *= 2f;
		}
		if (ssp.MovementComponentData.IsFallSpell)
		{
			SpellTools.SpellInitialFallData(ref ssp, targetPosition, speed);
		}
		if (ssp.MovementComponentData.IsFallSpell || ssp.MovementComponentData.Type == SpellSpecialMovementType.Rotation)
		{
			return ssp;
		}
		ssp.MovementComponentData.Gravity = 13f;
		ssp.MovementComponentData.CurrentFallSpeed = -4f;
		return ssp;
	}

	public SpellSpawnParams BuildBoboBomb(Entity sourceSpell, float3 spawnPosition, float3 targetPosition, float3 direction, float damage, float damageRatio, float resizeBaseDamage, float speed, float gravity, float upSpeed)
	{
		SpellSpawnParams ssp = Params;
		ClearTriggerInfoAndSubGroup(ref ssp);
		ssp.PrefabId = 90012;
		ssp.ConfigComponentData.Id = 90012;
		ssp.ConfigComponentData.AbilityType = SpellAbilityType.Summon1;
		ssp.ConfigComponentData.Damage.Base = resizeBaseDamage;
		ssp.ConfigComponentData.Float1 = damageRatio;
		ssp.ConfigComponentData.Float2 = damage;
		ssp.ConfigComponentData.Knockback = 7f;
		ssp.SetShooter(sourceSpell, ssp.OwnerUnit);
		ssp.MovementComponentData.AroundTarget = sourceSpell;
		if (!ssp.MovementComponentData.IsFallSpell)
		{
			ssp.SpawnPosition = spawnPosition;
		}
		ssp.MovementComponentData.FallTargetPosition = targetPosition;
		ssp.MovementComponentData.Speed = speed;
		ssp.MovementComponentData.Gravity = gravity;
		ssp.MovementComponentData.CurrentFallSpeed = upSpeed;
		ssp.MovementComponentData.Direction = direction;
		if (ssp.MovementComponentData.Type == SpellSpecialMovementType.Rotation)
		{
			ssp.ConfigComponentData.Duration.Base = 3f;
			ssp.MovementComponentData.Speed *= 2f;
		}
		if (ssp.MovementComponentData.IsFallSpell)
		{
			SpellTools.SpellInitialFallData(ref ssp, targetPosition, speed);
		}
		if (!ssp.MovementComponentData.IsFallSpell)
		{
			_ = ssp.MovementComponentData.Type;
			_ = 3;
		}
		return ssp;
	}

	public SpellSpawnParams BuildMrBingArrow(float3 spawnPosition, float3 direction)
	{
		SpellSpawnParams ssp = Params;
		ClearTriggerInfoAndSubGroup(ref ssp);
		ssp.SpawnPosition = spawnPosition;
		ssp.MovementComponentData.Direction = direction;
		ssp.ConfigComponentData.Int3 = 1;
		return ssp;
	}

	public SpellSpawnParams BuildRedRuneSlash(float3 spawnPosition, float splitPower)
	{
		SpellSpawnParams ssp = Params;
		ClearTriggerInfoAndSubGroup(ref ssp);
		ssp.SpawnPosition = spawnPosition;
		ssp.ConfigComponentData.Damage.MulRatio *= 0.33f * splitPower;
		ssp.IsSplitSpell = true;
		ssp.SplitComponentData.Count = 0;
		return ssp;
	}

	public SpellSpawnParams BuildGreenRuneExplosion(float3 spawnPosition)
	{
		SpellSpawnParams ssp = Params;
		ClearTriggerInfoAndSubGroup(ref ssp);
		ssp.SpawnPosition = spawnPosition;
		ssp.ConfigComponentData.Int3 = 1;
		ssp.MovementComponentData.Type = SpellSpecialMovementType.Normal;
		return ssp;
	}

	public SpellSpawnParams BuildArcaneExplosion(float3 spawnPosition, float randomAngle)
	{
		SpellSpawnParams ssp = Params;
		ClearTriggerInfoAndSubGroup(ref ssp);
		ssp.SpawnPosition = spawnPosition;
		ssp.MovementComponentData.AroundAngle = randomAngle;
		ssp.MovementComponentData.FallTargetPosition = spawnPosition;
		return ssp;
	}
}
