using Unity.Entities;

public struct FrameAnima : IComponentData, IQueryTypeParameter
{
	public int xCount;

	public int yCount;

	public float duration;

	public Entity ett_Root;

	public bool IsRandomDir;

	public bool IsLoopAnima;

	public bool isInitialized;

	public float timer;

	public int currentIndex;
}
