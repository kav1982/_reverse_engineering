using Unity.Entities;
using UnityEngine;

public class Spell3007LightningChainAuthoring : MonoBehaviour
{
	private class Spell3007LightningChainAuthoringBaker : Baker<Spell3007LightningChainAuthoring>
	{
		public override void Bake(Spell3007LightningChainAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell3007LightningChainEffect component = new Spell3007LightningChainEffect
			{
				SourceEntity = authoring.SourceEntity,
				TargetEntity = authoring.TargetEntity,
				Damage = authoring.Damage,
				PenetrateCount = authoring.PenetrateCount
			};
			AddComponent(entity, in component);
			AddBuffer<Spell3007DamageCoolDownBuffer>(entity);
		}
	}

	public Entity SourceEntity;

	public Entity TargetEntity;

	public float Damage;

	public int PenetrateCount;
}
