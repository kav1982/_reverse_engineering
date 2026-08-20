using Unity.Entities;

public struct Spell1012TrickMineData : IComponentData, IQueryTypeParameter
{
	public float ChainExplosionImmuteTimer;

	public bool IsInitialize;

	public bool EndingFlashEnable;

	public bool IsDenoteByOtherTrickMine;

	public float ExplosionCooldown;
}
