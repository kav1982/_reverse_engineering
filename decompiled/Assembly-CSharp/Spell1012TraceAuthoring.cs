using Unity.Entities;
using UnityEngine;

internal class Spell1012TraceAuthoring : MonoBehaviour
{
	private class Spell1012TraceAuthoringBaker : Baker<Spell1012TraceAuthoring>
	{
		public override void Bake(Spell1012TraceAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1012TraceSingleton component = default(Spell1012TraceSingleton);
			component.duration = authoring.ExistDuration;
			component.timer = 0f;
			component.recordRootEntity = authoring.ett_Root != null;
			if (authoring.ett_Root != null)
			{
				component.ett_Root = GetEntity(authoring.ett_Root, TransformUsageFlags.Dynamic);
			}
			AddComponent(entity, in component);
		}
	}

	public float ExistDuration;

	public GameObject ett_Root;
}
