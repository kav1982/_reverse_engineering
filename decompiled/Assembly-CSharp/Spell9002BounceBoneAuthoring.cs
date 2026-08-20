using Unity.Entities;
using UnityEngine;

public class Spell9002BounceBoneAuthoring : MonoBehaviour
{
	private class Spell9002BounceBoneAuthoringBaker : Baker<Spell9002BounceBoneAuthoring>
	{
		public override void Bake(Spell9002BounceBoneAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9002BounceBoneData component = new Spell9002BounceBoneData
			{
				RotationSpeed = 500f,
				InitOver = false
			};
			AddComponent(entity, in component);
			Spell9002FallToAbyssTag component2 = default(Spell9002FallToAbyssTag);
			AddComponent(entity, in component2);
			SetComponentEnabled<Spell9002FallToAbyssTag>(entity, enabled: false);
		}
	}
}
