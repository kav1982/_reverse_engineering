using Unity.Entities;

public struct RandomFlipData : IComponentData, IQueryTypeParameter
{
	public bool HorizontalFlip;

	public bool VerticalFlip;

	public bool IsInitialized;
}
