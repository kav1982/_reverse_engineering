using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
internal class EntityCampSkinKeepAuthoring : MonoBehaviour
{
	private class EntityCampSkinKeepAuthoringBaker : Baker<EntityCampSkinKeepAuthoring>
	{
		public override void Bake(EntityCampSkinKeepAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			EntityCampSkinKeep component = new EntityCampSkinKeep
			{
				ett_Default = GetEntity(authoring.ett_Default, TransformUsageFlags.Dynamic),
				ett_Halloween = (authoring.ett_Halloween ? GetEntity(authoring.ett_Halloween, TransformUsageFlags.Dynamic) : Entity.Null),
				ett_Spring = (authoring.ett_Spring ? GetEntity(authoring.ett_Spring, TransformUsageFlags.Dynamic) : Entity.Null),
				ett_Summer = (authoring.ett_Summer ? GetEntity(authoring.ett_Summer, TransformUsageFlags.Dynamic) : Entity.Null),
				ett_Christmas = (authoring.ett_Christmas ? GetEntity(authoring.ett_Christmas, TransformUsageFlags.Dynamic) : Entity.Null)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Default;

	public GameObject ett_Halloween;

	public GameObject ett_Spring;

	public GameObject ett_Summer;

	public GameObject ett_Christmas;
}
