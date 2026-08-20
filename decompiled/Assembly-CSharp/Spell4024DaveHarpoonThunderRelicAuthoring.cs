using Unity.Entities;
using UnityEngine;

public class Spell4024DaveHarpoonThunderRelicAuthoring : MonoBehaviour
{
	public class Spell4024DaveHarpoonBaker : Baker<Spell4024DaveHarpoonThunderRelicAuthoring>
	{
		public override void Bake(Spell4024DaveHarpoonThunderRelicAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell4024DaveHarpoonThunderRelicData>(entity);
		}
	}
}
