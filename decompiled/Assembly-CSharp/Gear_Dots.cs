using Unity.Entities;

public struct Gear_Dots : IComponentData, IQueryTypeParameter
{
	public bool initialized;

	public Entity normalGear;

	public Entity doubleGear;

	public bool playerPick;

	public bool stageFinishPick;

	public float pickupTimer;

	public int price;
}
