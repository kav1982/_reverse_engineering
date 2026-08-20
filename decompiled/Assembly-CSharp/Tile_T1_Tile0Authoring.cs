using Unity.Entities;
using UnityEngine;

public class Tile_T1_Tile0Authoring : MonoBehaviour
{
	private class Baker : Baker<Tile_T1_Tile0Authoring>
	{
		public override void Bake(Tile_T1_Tile0Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Tile_T1_Tile0_Dots component = default(Tile_T1_Tile0_Dots);
			component.variationChance = authoring.variationChance;
			component.ett_Tile0Base = GetEntity(authoring.ett_Tile0Base, TransformUsageFlags.Dynamic);
			DynamicBuffer<EntityBED1> dynamicBuffer = AddBuffer<EntityBED1>(entity);
			for (int i = 0; i < authoring.etts_Tile0.Length; i++)
			{
				dynamicBuffer.Add(new EntityBED1
				{
					ett = GetEntity(authoring.etts_Tile0[i], TransformUsageFlags.Dynamic)
				});
			}
			component.ett_Tile1 = GetEntity(authoring.ett_Tile1, TransformUsageFlags.Dynamic);
			component.tile1Chance = authoring.tile1Chance;
			component.tile1CellWidth = authoring.tile1CellWidth;
			AddComponent(entity, in component);
		}
	}

	[Range(0f, 1f)]
	public float variationChance;

	public GameObject ett_Tile0Base;

	public GameObject[] etts_Tile0;

	[Range(0f, 1f)]
	[Header("Tile1")]
	public float tile1Chance;

	public GameObject ett_Tile1;

	public int tile1CellWidth;
}
