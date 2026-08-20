using Unity.Entities;

public struct BackCampPortal : IComponentData, IQueryTypeParameter
{
	public bool isInitailized;

	public UnityObjectRef<BackCampPortalMono> portalMono;
}
