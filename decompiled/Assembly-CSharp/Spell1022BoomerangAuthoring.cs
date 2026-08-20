using Unity.Entities;
using UnityEngine;

internal class Spell1022BoomerangAuthoring : MonoBehaviour
{
	private class Spell1022BoomerangAuthoringBaker : Baker<Spell1022BoomerangAuthoring>
	{
		public override void Bake(Spell1022BoomerangAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1022BoomerangData component = new Spell1022BoomerangData
			{
				IsInitialize = false,
				IgnoreRecycleDurationTimer = 0f,
				extraLerpSpeed = 0f
			};
			AddComponent(entity, in component);
		}
	}
}
