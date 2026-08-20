using Unity.Entities;
using UnityEngine;

internal class Spell3110LifeLineAuthoring : MonoBehaviour
{
	private class Spell3110LifeLineAuthoringBaker : Baker<Spell3110LifeLineAuthoring>
	{
		public override void Bake(Spell3110LifeLineAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.NonUniformScale);
			Spell3110LifeLineComponent component = new Spell3110LifeLineComponent
			{
				distancePocess = 1f
			};
			AddComponent(entity, in component);
		}
	}
}
