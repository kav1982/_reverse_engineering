using Unity.Entities;
using UnityEngine;

public class Spell1025DragonBreathFireLinePointsAuthoring : MonoBehaviour
{
	public class Spell1025Baker : Baker<Spell1025DragonBreathFireLinePointsAuthoring>
	{
		public override void Bake(Spell1025DragonBreathFireLinePointsAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell1025DragonBreathFireLinePointData>(entity);
		}
	}
}
