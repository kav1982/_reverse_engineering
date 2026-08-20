using Unity.Collections;
using Unity.Entities;

public struct Spell3007CreateRequest : IBufferElementData
{
	public Entity Source;

	public int Penetrate;

	public FixedString32Bytes ColorName;

	public float Damage;

	public bool MarkAsFired;
}
