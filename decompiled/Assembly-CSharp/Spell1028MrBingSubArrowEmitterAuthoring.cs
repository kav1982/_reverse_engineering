using Unity.Entities;
using UnityEngine;

internal class Spell1028MrBingSubArrowEmitterAuthoring : MonoBehaviour
{
	private class Spell1028MrBingSubArrowEmitterAuthoringBaker : Baker<Spell1028MrBingSubArrowEmitterAuthoring>
	{
		public override void Bake(Spell1028MrBingSubArrowEmitterAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1028MrBingSubArrowEmitterData component = new Spell1028MrBingSubArrowEmitterData
			{
				subEmitTimer = 0f,
				remainSubArrowCount = 4
			};
			AddComponent(entity, in component);
		}
	}
}
