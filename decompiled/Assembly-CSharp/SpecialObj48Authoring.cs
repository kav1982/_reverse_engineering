using Unity.Entities;
using UnityEngine;

public class SpecialObj48Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj48Authoring>
	{
		public override void Bake(SpecialObj48Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj48_Dots component = new SpecialObj48_Dots
			{
				ett_AccessTriggerLR = GetEntity(authoring.ett_AccessTriggerLR, TransformUsageFlags.Dynamic),
				ett_LeftAccess = GetEntity(authoring.ett_LeftBridge, TransformUsageFlags.Dynamic),
				ett_LeftCollider = GetEntity(authoring.ett_LeftCollider, TransformUsageFlags.Dynamic),
				accessTriggerPosL = authoring.accessTriggerPosL.transform.position,
				ett_RightAccess = GetEntity(authoring.ett_RightAccess, TransformUsageFlags.Dynamic),
				ett_RightCollider = GetEntity(authoring.ett_RightCollider, TransformUsageFlags.Dynamic),
				accessTriggerPosR = authoring.accessTriggerPosR.transform.position
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_AccessTriggerLR;

	public GameObject ett_LeftBridge;

	public GameObject ett_LeftCollider;

	public GameObject accessTriggerPosL;

	public GameObject ett_RightAccess;

	public GameObject ett_RightCollider;

	public GameObject accessTriggerPosR;
}
