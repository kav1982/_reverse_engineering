using Unity.Entities;
using UnityEngine;

public class Access_T1Authoring : MonoBehaviour
{
	private class Baker : Baker<Access_T1Authoring>
	{
		public override void Bake(Access_T1Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Access_T1_Dots component = new Access_T1_Dots
			{
				ett_Access = GetEntity(authoring.ett_Access, TransformUsageFlags.Dynamic),
				ett_AccessNotNeedKey = GetEntity(authoring.ett_AccessNotNeedKey, TransformUsageFlags.Dynamic),
				openFinalYOffset = authoring.openFinalYOffset,
				openYOffsetSpeed = authoring.openYOffsetSpeed
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Access;

	public GameObject ett_AccessNotNeedKey;

	public float openFinalYOffset;

	public float openYOffsetSpeed;
}
