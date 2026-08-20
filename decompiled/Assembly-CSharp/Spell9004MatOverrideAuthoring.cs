using Unity.Entities;
using UnityEngine;

public class Spell9004MatOverrideAuthoring : MonoBehaviour
{
	private class Spell9004MatOverrideAuthoringBaker : Baker<Spell9004MatOverrideAuthoring>
	{
		public override void Bake(Spell9004MatOverrideAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9004MatRingWidthData component = new Spell9004MatRingWidthData
			{
				Value = 0.06f
			};
			AddComponent(entity, in component);
			Spell9004MatOutlineWidth1Data component2 = new Spell9004MatOutlineWidth1Data
			{
				Value = 0.12f
			};
			AddComponent(entity, in component2);
			Spell9004MatOutlineWidth2Data component3 = new Spell9004MatOutlineWidth2Data
			{
				Value = 0.24f
			};
			AddComponent(entity, in component3);
		}
	}
}
