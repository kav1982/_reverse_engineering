using System;
using Unity.Entities;

public struct Spell1008HitTargetsData : IBufferElementData, IEquatable<Entity>
{
	public Entity SpellEntity;

	public bool Equals(Entity other)
	{
		return SpellEntity == other;
	}
}
