using Unity.Entities;
using UnityEngine;

public class Access_T6Authoring : MonoBehaviour
{
	private class Baker : Baker<Access_T6Authoring>
	{
		public override void Bake(Access_T6Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Access_T6_Dots component = new Access_T6_Dots
			{
				themeType = authoring.themeType,
				ett_AccessTriggerT6 = GetEntity(authoring.ett_AccessTriggerT6, TransformUsageFlags.Dynamic),
				ett_PortalNormal = GetEntity(authoring.ett_PortalNormal, TransformUsageFlags.Dynamic),
				ett_PortalBoss = GetEntity(authoring.ett_PortalBoss, TransformUsageFlags.Dynamic),
				ett_Layer = GetEntity(authoring.ett_Layer, TransformUsageFlags.Dynamic),
				openAnimaTime = authoring.openAnimaTime
			};
			AddComponent(entity, in component);
		}
	}

	public RoomThemeType themeType;

	public GameObject ett_AccessTriggerT6;

	public GameObject ett_PortalNormal;

	public GameObject ett_PortalBoss;

	public GameObject ett_Layer;

	public float openAnimaTime;
}
