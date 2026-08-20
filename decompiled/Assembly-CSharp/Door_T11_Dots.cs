using Unity.Entities;

public struct Door_T11_Dots : IComponentData, IQueryTypeParameter
{
	public float openDoorTime;

	public bool isInitialized;

	public UnityObjectRef<Door_SpineMono> doorSpineMono;

	public float openDoorTimer;
}
