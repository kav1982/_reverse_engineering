using Unity.Entities;

public struct SpellFromChargeModeStar : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public UnityObjectRef<Spell4004ChargeStars> Star;

	public Entity StarEntity;
}
