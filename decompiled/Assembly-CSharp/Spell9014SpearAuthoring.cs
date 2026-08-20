using Unity.Entities;
using UnityEngine;

public class Spell9014SpearAuthoring : MonoBehaviour
{
	private class Spell9014SpearAuthoringBaker : Baker<Spell9014SpearAuthoring>
	{
		public override void Bake(Spell9014SpearAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9014SpearData component = default(Spell9014SpearData);
			AddComponent(entity, in component);
		}
	}
}
