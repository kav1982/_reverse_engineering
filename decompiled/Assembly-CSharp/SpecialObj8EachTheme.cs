using Unity.Entities;

public struct SpecialObj8EachTheme : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity pfb_801BoxCollider;

	public Entity ett_Layer;

	public Entity ett_CornerURLD;

	public Entity ett_CornerRD;

	public Entity ett_CornerUR;

	public Entity ett_Full;

	public Entity ett_LUR;

	public Entity ett_RD;

	public Entity ett_RDL;

	public Entity ett_UR;

	public Entity ett_URD;

	public float waitTimeForInitial;

	public int waitFrameForInitial;

	public int waitFrameForChangeCollider;

	public bool keepCornerURLD;

	public bool keepCornerUR;

	public bool keepCornerRD;

	public bool keepUR;

	public bool keepRD;

	public bool keepLUR;

	public bool keepURD;

	public bool keepRDL;

	public bool keepFull;

	public bool isFliped;
}
