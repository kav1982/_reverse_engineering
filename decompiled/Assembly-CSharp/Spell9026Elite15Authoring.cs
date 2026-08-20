using Unity.Entities;
using UnityEngine;

public class Spell9026Elite15Authoring : MonoBehaviour
{
	private class Spell9026Elite15AuthoringBaker : Baker<Spell9026Elite15Authoring>
	{
		public override void Bake(Spell9026Elite15Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell9026Elite15Data component = default(Spell9026Elite15Data);
			AddComponent(entity, in component);
		}
	}
}
