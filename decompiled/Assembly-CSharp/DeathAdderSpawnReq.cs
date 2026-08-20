using Unity.Collections;
using Unity.Entities;

public struct DeathAdderSpawnReq
{
	public Entity Prefab;

	public Spell1017DeathAdderEffectData Data;

	public FixedString32Bytes Color;
}
