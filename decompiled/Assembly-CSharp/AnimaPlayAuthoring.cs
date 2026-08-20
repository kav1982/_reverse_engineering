using Unity.Entities;
using UnityEngine;

public class AnimaPlayAuthoring : MonoBehaviour
{
	private class Baker : Baker<AnimaPlayAuthoring>
	{
		public override void Bake(AnimaPlayAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AnimaPlay component = new AnimaPlay
			{
				boolIndex = -1,
				animaSpeed = 1f
			};
			AddComponent(entity, in component);
		}
	}

	public void IntEvent(int eventIndex)
	{
	}
}
