using Unity.Entities;
using UnityEngine;

public class CampMirrorAuthoring : MonoBehaviour
{
	private class Baker : Baker<CampMirrorAuthoring>
	{
		public override void Bake(CampMirrorAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			CampMirror_Dots component = default(CampMirror_Dots);
			AddComponent(entity, in component);
		}
	}
}
