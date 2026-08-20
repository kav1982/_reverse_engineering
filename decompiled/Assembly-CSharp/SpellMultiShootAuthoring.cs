using Unity.Entities;
using UnityEngine;

public class SpellMultiShootAuthoring : MonoBehaviour
{
	private class SpellMultiShootAuthoringBaker : Baker<SpellMultiShootAuthoring>
	{
		public override void Bake(SpellMultiShootAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			MultiShootData component = new MultiShootData
			{
				Count = 0
			};
			AddComponent(entity, in component);
		}
	}
}
