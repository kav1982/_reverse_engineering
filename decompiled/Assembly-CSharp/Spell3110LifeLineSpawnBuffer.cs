using Unity.Entities;

public struct Spell3110LifeLineSpawnBuffer : IBufferElementData
{
	public SpellComponentData data;

	public SpellConfigComponentData config;

	public SpellElementEffectComponentData element;

	public SpellColorType lifeLineColorType;

	public Entity linkTarget1;

	public Entity linkTarget2;
}
