using Unity.Entities;

public struct UnitFrameAnima : IComponentData, IQueryTypeParameter
{
	public bool isPlaying;

	public float currentTime;

	public int animaIndex;

	public Entity indexEntity;

	public BlobAssetReference<UnitFrameAnimaData> data;

	public void Play(int index, float time = 0f)
	{
		animaIndex = index;
		currentTime = time;
	}
}
