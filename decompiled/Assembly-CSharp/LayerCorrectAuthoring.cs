using Unity.Entities;
using UnityEngine;

public class LayerCorrectAuthoring : MonoBehaviour
{
	private class Baker : Baker<LayerCorrectAuthoring>
	{
		public override void Bake(LayerCorrectAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			LayerCorrect_Dots component = new LayerCorrect_Dots
			{
				ett_Layer = GetEntity(authoring.ett_Layer, TransformUsageFlags.Dynamic),
				type = authoring.type,
				updateEveryFrame = authoring.updateEveryFrame,
				inChild = authoring.inChild
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Layer;

	public LayerCorrectType type;

	public bool updateEveryFrame;

	public bool inChild;
}
