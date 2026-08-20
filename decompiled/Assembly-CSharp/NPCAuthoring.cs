using Unity.Entities;
using UnityEngine;

public class NPCAuthoring : MonoBehaviour
{
	private class Baker : Baker<NPCAuthoring>
	{
		public override void Bake(NPCAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			NPC_Dots component = default(NPC_Dots);
			AddComponent(entity, in component);
			NPCBaseComponent component2 = default(NPCBaseComponent);
			AddComponent(entity, in component2);
		}
	}
}
