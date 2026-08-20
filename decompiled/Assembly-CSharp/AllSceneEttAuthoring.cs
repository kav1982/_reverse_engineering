using Unity.Entities;
using UnityEngine;

public class AllSceneEttAuthoring : MonoBehaviour
{
	private class Baker : Baker<AllSceneEttAuthoring>
	{
		public override void Bake(AllSceneEttAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AllSceneEtt component = new AllSceneEtt
			{
				ett_OuterBoundary = GetEntity(authoring.ett_OuterBoundary, TransformUsageFlags.Dynamic),
				ett_T8CliffCollider = GetEntity(authoring.ett_T8CliffCollider, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
			DynamicBuffer<SceneEttBED> dynamicBuffer = AddBuffer<SceneEttBED>(entity);
			for (int i = 0; i < authoring.sceneEttSOs.Length; i++)
			{
				SceneEttBED elem = default(SceneEttBED);
				elem.ett_AccessD = GetEntity(authoring.sceneEttSOs[i].ett_AccessD, TransformUsageFlags.Dynamic);
				elem.ett_AccessL = GetEntity(authoring.sceneEttSOs[i].ett_AccessL, TransformUsageFlags.Dynamic);
				elem.ett_AccessR = GetEntity(authoring.sceneEttSOs[i].ett_AccessR, TransformUsageFlags.Dynamic);
				elem.ett_AccessU = GetEntity(authoring.sceneEttSOs[i].ett_AccessU, TransformUsageFlags.Dynamic);
				elem.ett_Boundary = GetEntity(authoring.sceneEttSOs[i].ett_Boundary, TransformUsageFlags.Dynamic);
				elem.ett_Boundary2 = GetEntity(authoring.sceneEttSOs[i].ett_Boundary2, TransformUsageFlags.Dynamic);
				elem.ett_Door = GetEntity(authoring.sceneEttSOs[i].ett_Door, TransformUsageFlags.Dynamic);
				elem.ett_Tile0 = GetEntity(authoring.sceneEttSOs[i].ett_Tile0, TransformUsageFlags.Dynamic);
				elem.ett_Tile1 = GetEntity(authoring.sceneEttSOs[i].ett_Tile1, TransformUsageFlags.Dynamic);
				elem.ett_Tile2 = GetEntity(authoring.sceneEttSOs[i].ett_Tile2, TransformUsageFlags.Dynamic);
				elem.ett_Tile3 = GetEntity(authoring.sceneEttSOs[i].ett_Tile3, TransformUsageFlags.Dynamic);
				elem.ett_Tile4 = GetEntity(authoring.sceneEttSOs[i].ett_Tile4, TransformUsageFlags.Dynamic);
				elem.ett_Tile5 = GetEntity(authoring.sceneEttSOs[i].ett_Tile5, TransformUsageFlags.Dynamic);
				elem.ett_Tile6 = GetEntity(authoring.sceneEttSOs[i].ett_Tile6, TransformUsageFlags.Dynamic);
				elem.ett_Tile7 = GetEntity(authoring.sceneEttSOs[i].ett_Tile7, TransformUsageFlags.Dynamic);
				elem.ett_Tile8 = GetEntity(authoring.sceneEttSOs[i].ett_Tile8, TransformUsageFlags.Dynamic);
				elem.ett_Tile9 = GetEntity(authoring.sceneEttSOs[i].ett_Tile9, TransformUsageFlags.Dynamic);
				dynamicBuffer.Add(elem);
			}
		}
	}

	public SceneEttSO[] sceneEttSOs;

	public GameObject ett_OuterBoundary;

	public GameObject ett_T8CliffCollider;
}
