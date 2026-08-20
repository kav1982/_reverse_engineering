using Unity.Entities;

public struct Spell4013RuneHammerData : IComponentData, IQueryTypeParameter
{
	public int currentIndex;

	public int maxHammerCount;

	public float HammerLength;

	public bool HasSplitSpell;

	public bool IsRotateAroundWandSpirit;

	public bool IsInitialized;

	public Entity EmberEntity;

	public float radiusDecrease;
}
