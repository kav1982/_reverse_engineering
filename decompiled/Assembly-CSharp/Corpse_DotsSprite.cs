using Unity.Entities;

public struct Corpse_DotsSprite : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<CorpseSprite> sprite;
}
