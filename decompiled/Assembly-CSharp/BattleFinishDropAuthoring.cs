using Unity.Entities;
using UnityEngine;

public class BattleFinishDropAuthoring : MonoBehaviour
{
	private class Baker : Baker<BattleFinishDropAuthoring>
	{
		public override void Bake(BattleFinishDropAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			BattleFinishDrop component = default(BattleFinishDrop);
			AddComponent(entity, in component);
		}
	}
}
