using Unity.Entities;
using UnityEngine;

public class Spell1005PreFireworkAuthoring : MonoBehaviour
{
	private class Spell1005PreFireworkAuthoringBaker : Baker<Spell1005PreFireworkAuthoring>
	{
		public override void Bake(Spell1005PreFireworkAuthoring authoring)
		{
			AddComponent<Spell1005PreFirework_Tag>(GetEntity(TransformUsageFlags.Dynamic));
		}
	}
}
