using Unity.Entities;
using UnityEngine;

public class AudioSourceInDotsAuthoring : MonoBehaviour
{
	private class Baker : Baker<AudioSourceInDotsAuthoring>
	{
		public override void Bake(AudioSourceInDotsAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<AudioSourceInDots>(entity);
		}
	}
}
