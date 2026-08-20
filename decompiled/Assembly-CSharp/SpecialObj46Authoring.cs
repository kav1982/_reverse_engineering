using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpecialObj46Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj46Authoring>
	{
		public override void Bake(SpecialObj46Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj46 component = new SpecialObj46
			{
				ett_Appearance_Chapter1 = GetEntity(authoring.ett_Appearance_Chapter1, TransformUsageFlags.Dynamic),
				ett_Appearance_Chapter2 = GetEntity(authoring.ett_Appearance_Chapter2, TransformUsageFlags.Dynamic),
				ett_Appearance_Chapter3 = GetEntity(authoring.ett_Appearance_Chapter3, TransformUsageFlags.Dynamic),
				ett_Appearance_Chapter4 = GetEntity(authoring.ett_Appearance_Chapter4, TransformUsageFlags.Dynamic),
				ett_Appearance_Chapter5 = GetEntity(authoring.ett_Appearance_Chapter5, TransformUsageFlags.Dynamic),
				npc8Offset = authoring.npc8Offset,
				ett_Sushi = GetEntity(authoring.ett_Sushi, TransformUsageFlags.Dynamic),
				ett_SushiOutline = GetEntity(authoring.ett_SushiOutline, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Appearance_Chapter1;

	public GameObject ett_Appearance_Chapter2;

	public GameObject ett_Appearance_Chapter3;

	public GameObject ett_Appearance_Chapter4;

	public GameObject ett_Appearance_Chapter5;

	public float3 npc8Offset;

	[Header("Susui")]
	public GameObject ett_Sushi;

	public GameObject ett_SushiOutline;
}
