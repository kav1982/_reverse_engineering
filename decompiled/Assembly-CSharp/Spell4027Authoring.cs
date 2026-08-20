using Unity.Entities;
using UnityEngine;

internal class Spell4027Authoring : MonoBehaviour
{
	private class Spell4027AuthoringBaker : Baker<Spell4027Authoring>
	{
		public override void Bake(Spell4027Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell4027BlueRuneData component = new Spell4027BlueRuneData
			{
				NeedResetChaseTargetAngleSpeed = true,
				CurrentRotationRadius = 0.1f
			};
			AddComponent(entity, in component);
		}
	}
}
