using Unity.Entities;

public struct Access_T1_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_Access;

	public Entity ett_AccessNotNeedKey;

	public float openFinalYOffset;

	public float openYOffsetSpeed;

	public bool isInitialized;

	public bool isOpening;

	public bool isClosing;

	public float openCurrentYOffset;
}
