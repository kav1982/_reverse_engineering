using Unity.Entities;

public struct LayerCorrect_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_Layer;

	public LayerCorrectType type;

	public bool updateEveryFrame;

	public bool inChild;

	public int waitFrameToUnenable;
}
