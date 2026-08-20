using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Relic_ShowUnitHPAuthoring : MonoBehaviour
{
	private class Baker : Baker<Relic_ShowUnitHPAuthoring>
	{
		public override void Bake(Relic_ShowUnitHPAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Relic_ShowUnitHP component = new Relic_ShowUnitHP
			{
				ett_Hight = GetEntity(authoring.ett_Hight, TransformUsageFlags.Dynamic),
				ett_HPBarRoot = GetEntity(authoring.ett_HPBarRoot, TransformUsageFlags.NonUniformScale),
				ett_HPBar_Monster = GetEntity(authoring.ett_HPBar_Monster, TransformUsageFlags.Dynamic),
				ett_HPBar_Teammate = GetEntity(authoring.ett_HPBar_Teammate, TransformUsageFlags.Dynamic),
				ett_TextRoot = GetEntity(authoring.ett_TextRoot, TransformUsageFlags.Dynamic),
				ett_CurrentHP = GetEntity(authoring.ett_CurrentHP, TransformUsageFlags.NonUniformScale),
				ett_MaxHP = GetEntity(authoring.ett_MaxHP, TransformUsageFlags.NonUniformScale),
				oneNumberScale = authoring.oneNumberScale,
				hpOffset = authoring.hpOffset
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Hight;

	public GameObject ett_HPBarRoot;

	public GameObject ett_HPBar_Monster;

	public GameObject ett_HPBar_Teammate;

	[Header("Text")]
	public GameObject ett_TextRoot;

	public GameObject ett_CurrentHP;

	public GameObject ett_MaxHP;

	public float2 oneNumberScale;

	public float hpOffset;
}
