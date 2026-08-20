using Unity.Entities;
using UnityEngine;

public class CampSkinChangerAuthoring : MonoBehaviour
{
	private class Baker : Baker<CampSkinChangerAuthoring>
	{
		public override void Bake(CampSkinChangerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			CampSkinChanger component = default(CampSkinChanger);
			AddComponent(entity, in component);
		}
	}
}
