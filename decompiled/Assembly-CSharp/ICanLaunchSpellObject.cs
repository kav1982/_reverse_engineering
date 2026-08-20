using System.Collections.Generic;

public interface ICanLaunchSpellObject
{
	ShootSpellSpatialInfo GetLaunchSpellSpatialInfo();

	IEnumerable<SpellBase> Launch();

	SpellInitialParameter.Builder CreateLaunchParameterBuilder();
}
