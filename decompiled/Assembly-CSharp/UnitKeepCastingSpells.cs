using Unity.Entities;

public struct UnitKeepCastingSpells : IBufferElementData
{
	public Entity Spell;

	public bool ReduceMoveSpeed;
}
