using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class Spell1017DeathAdderAuthoring : MonoBehaviour
{
	private class Baker : Baker<Spell1017DeathAdderAuthoring>
	{
		public override void Bake(Spell1017DeathAdderAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1017DeathAdderData component = new Spell1017DeathAdderData
			{
				InitOver = false,
				EffectEntity = GetEntity(authoring.chainEffect, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	[FormerlySerializedAs("ChainEffect")]
	public GameObject chainEffect;
}
