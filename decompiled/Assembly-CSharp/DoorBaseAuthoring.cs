using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class DoorBaseAuthoring : MonoBehaviour
{
	private class Baker : Baker<DoorBaseAuthoring>
	{
		public override void Bake(DoorBaseAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			DoorBase_Dots component = new DoorBase_Dots
			{
				refreshEFOffset = authoring.refreshEFPos,
				ett_Reward0 = GetEntity(authoring.ett_Reward0, TransformUsageFlags.Dynamic),
				ett_Reward1 = GetEntity(authoring.ett_Reward1, TransformUsageFlags.Dynamic),
				ett_Reward2 = GetEntity(authoring.ett_Reward2, TransformUsageFlags.Dynamic),
				ett_Reward3 = GetEntity(authoring.ett_Reward3, TransformUsageFlags.Dynamic),
				ett_Reward4 = GetEntity(authoring.ett_Reward4, TransformUsageFlags.Dynamic),
				ett_Reward5 = GetEntity(authoring.ett_Reward5, TransformUsageFlags.Dynamic),
				ett_Reward6 = GetEntity(authoring.ett_Reward6, TransformUsageFlags.Dynamic),
				ett_Reward7 = GetEntity(authoring.ett_Reward7, TransformUsageFlags.Dynamic),
				ett_Reward100 = GetEntity(authoring.ett_Reward100, TransformUsageFlags.Dynamic),
				ett_Reward101 = GetEntity(authoring.ett_Reward101, TransformUsageFlags.Dynamic),
				ett_Reward131 = GetEntity(authoring.ett_Reward131, TransformUsageFlags.Dynamic),
				ett_Reward200 = GetEntity(authoring.ett_Reward200, TransformUsageFlags.Dynamic),
				ett_Portal = GetEntity(authoring.ett_Portal, TransformUsageFlags.Dynamic),
				ett_Door = GetEntity(authoring.ett_Door, TransformUsageFlags.Dynamic),
				ett_DoorRuined = GetEntity(authoring.ett_DoorRuined, TransformUsageFlags.Dynamic),
				ett_Door2 = GetEntity(authoring.ett_Door2, TransformUsageFlags.Dynamic),
				openDoorSpeed = authoring.openDoorSpeed,
				openDoorFinalOffsetY = authoring.openDoorFinalOffsetY
			};
			AddComponent(entity, in component);
		}
	}

	public float3 refreshEFPos;

	[Header("Reward")]
	public GameObject ett_Reward0;

	public GameObject ett_Reward1;

	public GameObject ett_Reward2;

	public GameObject ett_Reward3;

	public GameObject ett_Reward4;

	public GameObject ett_Reward5;

	public GameObject ett_Reward6;

	public GameObject ett_Reward7;

	public GameObject ett_Reward100;

	public GameObject ett_Reward101;

	public GameObject ett_Reward131;

	public GameObject ett_Reward200;

	[Header("Door")]
	public GameObject ett_Portal;

	public GameObject ett_Door;

	public GameObject ett_DoorRuined;

	[Header("OpenDoor")]
	public GameObject ett_Door2;

	public float openDoorSpeed;

	public float openDoorFinalOffsetY;
}
