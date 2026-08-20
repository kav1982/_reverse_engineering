using Unity.Entities;
using UnityEngine;

internal class Spell1020ManaCoinAuthoring : MonoBehaviour
{
	private class Spell1020ManaCoinAuthoringBaker : Baker<Spell1020ManaCoinAuthoring>
	{
		public override void Bake(Spell1020ManaCoinAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1020ManaCoinData component = new Spell1020ManaCoinData
			{
				IsInitialized = false,
				CoinUseCount = 0,
				BuffRatio = 0f
			};
			AddComponent(entity, in component);
			AddBuffer<GroundCoinsBuffer>(entity);
		}
	}
}
