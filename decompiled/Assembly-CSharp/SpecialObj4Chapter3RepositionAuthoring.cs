using Unity.Entities;
using UnityEngine;

public class SpecialObj4Chapter3RepositionAuthoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj4Chapter3RepositionAuthoring>
	{
		public override void Bake(SpecialObj4Chapter3RepositionAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj4Chapter3Reposition component = default(SpecialObj4Chapter3Reposition);
			AddComponent(entity, in component);
		}
	}
}
