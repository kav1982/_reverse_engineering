using Unity.Entities;
using UnityEngine;

public class Spell4024DaveHarpoonAuthoring : MonoBehaviour
{
	public class Spell4024DaveHarpoonBaker : Baker<Spell4024DaveHarpoonAuthoring>
	{
		public override void Bake(Spell4024DaveHarpoonAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell4024DaveHarpoonData>(entity);
			AddBuffer<HarpoonChainData>(entity);
			AddBuffer<HighspeedHarpoonSphereCastPos>(entity);
		}
	}
}
