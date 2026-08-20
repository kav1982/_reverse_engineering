using Unity.Entities;

public struct Curse_RandomBomb_Dots : IComponentData, IQueryTypeParameter
{
	public float explosionDelay;

	public float explosionRadius;

	public float explosionKnockback;

	public float explosionDamage;

	public bool isInitialized;

	public UnityObjectRef<Curse_RandomBombMono> bombMono;

	public float timer;
}
