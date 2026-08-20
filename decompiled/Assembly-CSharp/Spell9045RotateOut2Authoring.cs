using Unity.Entities;
using UnityEngine;

public class Spell9045RotateOut2Authoring : MonoBehaviour
{
	private class Spell9045RotateOut2AuthoringBaker : Baker<Spell9045RotateOut2Authoring>
	{
		public override void Bake(Spell9045RotateOut2Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9045RotateOut2Data component = new Spell9045RotateOut2Data
			{
				Initialized = false
			};
			AddComponent(entity, in component);
		}
	}
}
