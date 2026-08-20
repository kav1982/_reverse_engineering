using Unity.Entities;
using UnityEngine;

internal class EndlessRankingListAuthoring : MonoBehaviour
{
	private class EndlessRankingListAuthoringBaker : Baker<EndlessRankingListAuthoring>
	{
		public override void Bake(EndlessRankingListAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			EndlessRankinglist component = default(EndlessRankinglist);
			AddComponent(entity, in component);
		}
	}
}
