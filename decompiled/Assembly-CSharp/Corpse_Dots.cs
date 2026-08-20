using Unity.Entities;

public struct Corpse_Dots : IComponentData, IQueryTypeParameter
{
	public enum CorpseState
	{
		Fly,
		Dark,
		Stay
	}

	public int imageCount;

	public int harmonyImageCount;

	public Entity corpsePhysicsEntity;

	public Entity corpseImageEntity;

	public Entity shadowImageEntity;

	public CorpseType type;

	public CorpseState state;

	public int bounceTime;

	public int bounceTimer;

	public float bounceRatio;

	public float currentUpSpeed;

	public float gravity;

	public float rotateSpeed;

	public float nowAngle;

	public float currentAlpha;

	public float reduceAlphaSpeed;

	public float minAlpha;

	public float duration;

	public float durationTimer;
}
