using Unity.Entities;
using UnityEngine;

public class LevelRewardAuthoring : MonoBehaviour
{
	private class Baker : Baker<LevelRewardAuthoring>
	{
		public override void Bake(LevelRewardAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			LevelReward component = new LevelReward
			{
				ett_Hover = GetEntity(authoring.ett_Hover, TransformUsageFlags.Dynamic),
				hoverSpeed = authoring.hoverSpeed,
				hoverRange = authoring.hoverRange,
				ett_Outline_Reward0 = GetEntity(authoring.ett_Outline_Reward0, TransformUsageFlags.Dynamic),
				ett_Outline_Reward1 = GetEntity(authoring.ett_Outline_Reward1, TransformUsageFlags.Dynamic),
				ett_Outline_Reward2 = GetEntity(authoring.ett_Outline_Reward2, TransformUsageFlags.Dynamic),
				ett_Outline_Reward3 = GetEntity(authoring.ett_Outline_Reward3, TransformUsageFlags.Dynamic),
				ett_Outline_Reward4 = GetEntity(authoring.ett_Outline_Reward4, TransformUsageFlags.Dynamic),
				ett_Outline_Reward5 = GetEntity(authoring.ett_Outline_Reward5, TransformUsageFlags.Dynamic),
				ett_Outline_Reward6 = GetEntity(authoring.ett_Outline_Reward6, TransformUsageFlags.Dynamic),
				ett_Outline_Reward7 = GetEntity(authoring.ett_Outline_Reward7, TransformUsageFlags.Dynamic),
				ett_Outline_Reward8 = GetEntity(authoring.ett_Outline_Reward8, TransformUsageFlags.Dynamic),
				ett_Outline_Reward100 = GetEntity(authoring.ett_Outline_Reward100, TransformUsageFlags.Dynamic),
				ett_Outline_Reward101 = GetEntity(authoring.ett_Outline_Reward101, TransformUsageFlags.Dynamic),
				ett_Outline_Reward131 = GetEntity(authoring.ett_Outline_Reward131, TransformUsageFlags.Dynamic),
				ett_Outline_Reward200 = GetEntity(authoring.ett_Outline_Reward200, TransformUsageFlags.Dynamic),
				ett_Icon_Reward0 = GetEntity(authoring.ett_Icon_Reward0, TransformUsageFlags.Dynamic),
				ett_Icon_Reward1 = GetEntity(authoring.ett_Icon_Reward1, TransformUsageFlags.Dynamic),
				ett_Icon_Reward2 = GetEntity(authoring.ett_Icon_Reward2, TransformUsageFlags.Dynamic),
				ett_Icon_Reward3 = GetEntity(authoring.ett_Icon_Reward3, TransformUsageFlags.Dynamic),
				ett_Icon_Reward4 = GetEntity(authoring.ett_Icon_Reward4, TransformUsageFlags.Dynamic),
				ett_Icon_Reward5 = GetEntity(authoring.ett_Icon_Reward5, TransformUsageFlags.Dynamic),
				ett_Icon_Reward6 = GetEntity(authoring.ett_Icon_Reward6, TransformUsageFlags.Dynamic),
				ett_Icon_Reward7 = GetEntity(authoring.ett_Icon_Reward7, TransformUsageFlags.Dynamic),
				ett_Icon_Reward8 = GetEntity(authoring.ett_Icon_Reward8, TransformUsageFlags.Dynamic),
				ett_Icon_Reward100 = GetEntity(authoring.ett_Icon_Reward100, TransformUsageFlags.Dynamic),
				ett_Icon_Reward101 = GetEntity(authoring.ett_Icon_Reward101, TransformUsageFlags.Dynamic),
				ett_Icon_Reward131 = GetEntity(authoring.ett_Icon_Reward131, TransformUsageFlags.Dynamic),
				ett_Icon_Reward200 = GetEntity(authoring.ett_Icon_Reward200, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
			AddBuffer<LevelRewardInfoBED>(entity);
		}
	}

	public GameObject ett_Hover;

	public float hoverSpeed;

	public float hoverRange;

	[Header("Outline")]
	public GameObject ett_Outline_Reward0;

	public GameObject ett_Outline_Reward1;

	public GameObject ett_Outline_Reward2;

	public GameObject ett_Outline_Reward3;

	public GameObject ett_Outline_Reward4;

	public GameObject ett_Outline_Reward5;

	public GameObject ett_Outline_Reward6;

	public GameObject ett_Outline_Reward7;

	public GameObject ett_Outline_Reward8;

	public GameObject ett_Outline_Reward100;

	public GameObject ett_Outline_Reward101;

	public GameObject ett_Outline_Reward131;

	public GameObject ett_Outline_Reward200;

	[Header("Icon")]
	public GameObject ett_Icon_Reward0;

	public GameObject ett_Icon_Reward1;

	public GameObject ett_Icon_Reward2;

	public GameObject ett_Icon_Reward3;

	public GameObject ett_Icon_Reward4;

	public GameObject ett_Icon_Reward5;

	public GameObject ett_Icon_Reward6;

	public GameObject ett_Icon_Reward7;

	public GameObject ett_Icon_Reward8;

	public GameObject ett_Icon_Reward100;

	public GameObject ett_Icon_Reward101;

	public GameObject ett_Icon_Reward131;

	public GameObject ett_Icon_Reward200;
}
