using Unity.Entities;
using Unity.Mathematics;

[ChunkSerializable]
public struct GlobalRandom : IComponentData, IQueryTypeParameter
{
	public Random random;

	public GlobalRandom(uint seed)
	{
		random = Random.CreateFromIndex(seed);
	}

	public Random NewRandom()
	{
		return new Random(random.NextUInt());
	}

	public float NextFloatByChunkIndex(int chunkIndex, float min = 0f, float max = 1f)
	{
		random.state = random.NextUInt((uint)chunkIndex, uint.MaxValue);
		return random.NextFloat(min, max);
	}

	public bool ChanceResult(int chunkIndex, float chance)
	{
		random.state = random.NextUInt((uint)chunkIndex, uint.MaxValue);
		return random.NextFloat(0f, 1f) < chance;
	}

	public float HalfChanceNPOne(int chunkIndex)
	{
		random.state = random.NextUInt((uint)chunkIndex, uint.MaxValue);
		return (random.NextFloat(0f, 2f) > 1f) ? 1 : (-1);
	}
}
