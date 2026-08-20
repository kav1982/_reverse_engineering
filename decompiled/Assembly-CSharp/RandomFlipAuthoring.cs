using Unity.Entities;
using UnityEngine;

internal class RandomFlipAuthoring : MonoBehaviour
{
	private class RandomFlipAuthoringBaker : Baker<RandomFlipAuthoring>
	{
		public override void Bake(RandomFlipAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			RandomFlipData component = new RandomFlipData
			{
				HorizontalFlip = authoring.HorizontalFlip,
				VerticalFlip = authoring.VerticalFlip,
				IsInitialized = false
			};
			AddComponent(entity, in component);
		}
	}

	public bool HorizontalFlip;

	public bool VerticalFlip;
}
