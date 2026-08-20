using Unity.Entities;

public struct SpellNeedResize : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public bool ResizeByDamage;

	public float ExtraSizeRatio;
}
