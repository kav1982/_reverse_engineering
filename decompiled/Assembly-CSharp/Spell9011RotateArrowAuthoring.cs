using Unity.Entities;
using UnityEngine;

public class Spell9011RotateArrowAuthoring : MonoBehaviour
{
	private class Spell9011RotateArrowAuthoringBaker : Baker<Spell9011RotateArrowAuthoring>
	{
		public override void Bake(Spell9011RotateArrowAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9011RotateArrowData component = new Spell9011RotateArrowData
			{
				InitOver = false,
				Aligned = false
			};
			AddComponent(entity, in component);
		}
	}
}
