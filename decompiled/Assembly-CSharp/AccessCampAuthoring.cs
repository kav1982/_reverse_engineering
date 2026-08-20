using Unity.Entities;
using UnityEngine;

public class AccessCampAuthoring : MonoBehaviour
{
	private class Baker : Baker<AccessCampAuthoring>
	{
		public override void Bake(AccessCampAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AccessCamp component = new AccessCamp
			{
				dir = authoring.dir
			};
			AddComponent(entity, in component);
		}
	}

	public FourDir dir;
}
