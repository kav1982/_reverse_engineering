using Unity.Entities;
using UnityEngine;

internal class Spell3015WormAuthoring : MonoBehaviour
{
	private class Spell3015WormAuthoringBaker : Baker<Spell3015WormAuthoring>
	{
		public override void Bake(Spell3015WormAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Entity entity2 = GetEntity(authoring.mesh, TransformUsageFlags.Dynamic);
			Spell3015WormComponent component = new Spell3015WormComponent
			{
				meshEntity = entity2
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject mesh;
}
