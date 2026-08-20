using System;
using System.Collections.Generic;
using System.Linq;

public class LauncherSpellBase : SpellBase, ICanLaunchSpellObject
{
	public virtual ShootSpellSpatialInfo GetLaunchSpellSpatialInfo()
	{
		return ShootSpellSpatialInfo.ToStartPointButWithDirection(base.transform.position.IgnoreZ(), base.Direction);
	}

	public IEnumerable<SpellBase> Launch()
	{
		if (base.ShootData?.SubGroup == null)
		{
			return Array.Empty<SpellBase>();
		}
		return CreateLaunchSpells().ToArray();
	}

	protected virtual IEnumerable<SpellBase> CreateLaunchSpells()
	{
		CreateLaunchParameterBuilder();
		GetLaunchSpellSpatialInfo();
		return Array.Empty<SpellBase>();
	}

	public virtual SpellInitialParameter.Builder CreateLaunchParameterBuilder()
	{
		SpellInitialParameter.Builder builder = new SpellInitialParameter.Builder();
		if (!(ownerPpt == PlayerMgr.Inst.PlayerPpt))
		{
			_ = ownerPpt.UnitBas.SummonerSpellBase;
		}
		if ((object)base.shooterWand != null)
		{
			Wand wand = PlayerMgr.Inst.Wands.FirstOrDefault((Wand e) => e.WandCfg == base.InitialParameter.shooterWandCfg);
			if ((bool)wand)
			{
				builder.ApplyWandEffect(wand, base.wandChargeData);
			}
		}
		builder.ApplyOwnerSpellEffect(this);
		return builder;
	}
}
