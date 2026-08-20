using Unity.Entities;

public struct AnimaPlay : IComponentData, IQueryTypeParameter
{
	public int boolIndex;

	public bool needPlay;

	public float animaSpeed;

	public void Play(int boolIndex)
	{
		needPlay = true;
		this.boolIndex = boolIndex;
	}

	public void SetLockMotion(bool locked)
	{
		if (locked)
		{
			animaSpeed = 0f;
		}
		else
		{
			animaSpeed = 1f;
		}
	}
}
