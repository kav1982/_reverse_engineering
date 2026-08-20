using Unity.Entities;

public struct EndlessSpawnEffect : IComponentData, IQueryTypeParameter
{
	public Entity effectEntity;

	public Entity scaleRoot;

	public int MonsterID;

	public float scale;

	public float showTime;

	public float stayTime;

	public float fadeTime;

	public float lifeTimer;

	public bool summoned;

	public void Initialize(int ID, float scale, float delay = 0f)
	{
		MonsterID = ID;
		this.scale = scale;
		lifeTimer = 0f - delay;
	}
}
