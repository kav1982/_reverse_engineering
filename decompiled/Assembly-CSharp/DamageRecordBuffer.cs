using Unity.Entities;

public struct DamageRecordBuffer : IBufferElementData
{
	public int SpellOrRelicId;

	public float Damage;

	public int HitUnitId;
}
