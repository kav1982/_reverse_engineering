using Unity.Entities;
using UnityEngine;

public class RankingListAuthoring : MonoBehaviour
{
	private class Baker : Baker<RankingListAuthoring>
	{
		public override void Bake(RankingListAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			RankingList_Dots component = default(RankingList_Dots);
			AddComponent(entity, in component);
		}
	}
}
