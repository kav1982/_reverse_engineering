using Unity.Entities;

public struct Spell2007SuicideBugNestData : IComponentData, IQueryTypeParameter
{
	public enum Spell2007AnimType
	{
		Idle,
		Landing,
		Attack
	}

	public Spell2007AnimType AnimType;

	public float CurrentAnimTimer;

	public float SpawnTimer;

	public float positionZ;

	public bool ForbiddenSpawnSuicideBugWhenDestroy;
}
