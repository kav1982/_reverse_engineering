using Unity.Entities;
using UnityEngine;

public class Spell1017DeathAdderTraceAuthoring : MonoBehaviour
{
	private class Spell1017DeathAdderTraceAuthoringBaker : Baker<Spell1017DeathAdderTraceAuthoring>
	{
		public override void Bake(Spell1017DeathAdderTraceAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell1017DeathAdderTraceProgress>(entity);
			AddComponent<Spell1017DeathAdderTraceTransparency>(entity);
		}
	}
}
