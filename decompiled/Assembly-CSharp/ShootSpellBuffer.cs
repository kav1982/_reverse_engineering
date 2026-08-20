using System;
using System.Collections.Generic;
using Unity.Entities;

internal class ShootSpellBuffer : IDisposable
{
	private readonly List<(SpellShootGroup group, ShootSpellSpatialInfo spatial, SpellInitialParameter.Builder sipBuilder, float reverseCopyShootRate)> _buffer = new List<(SpellShootGroup, ShootSpellSpatialInfo, SpellInitialParameter.Builder, float)>();

	public void Shoot(SpellShootGroup group, ShootSpellSpatialInfo spatial, SpellInitialParameter.Builder sipBuilder, float reverseCopyShootRate = 0f)
	{
		_buffer.Add((group, spatial, sipBuilder, reverseCopyShootRate));
	}

	public void ShootByTrigger(Entity shooterSpell, SpellComponentData shooterData, SpellShootGroup group, ShootSpellSpatialInfo spatial, SpellInitialParameter.Builder sipBuilder = null)
	{
		if (sipBuilder == null)
		{
			sipBuilder = new SpellInitialParameter.Builder();
		}
		sipBuilder.ApplyShooterEntity(shooterSpell, shooterData.OwnerEntity);
		if ((bool)shooterData.Wand && shooterData.Wand.Value.WandCfg != null)
		{
			sipBuilder.ApplyWandEffect(shooterData.Wand, shooterData.Wand.Value.BuildPostSlotChargeData());
		}
		Shoot(group, spatial, sipBuilder);
	}

	public void Playback()
	{
		foreach (var (group, spatialInfo, parameterBuilder, reverseCopyShootRate) in _buffer)
		{
			ShootSpellUtils.ShootSpellGroup(group, spatialInfo, parameterBuilder, reverseCopyShootRate);
		}
	}

	public void Dispose()
	{
		_buffer.Clear();
	}
}
