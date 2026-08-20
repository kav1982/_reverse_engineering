using Unity.Entities;
using UnityEngine;

internal class RandomRotateAuthoring : MonoBehaviour
{
	private class RandomRotateAuthoringBaker : Baker<RandomRotateAuthoring>
	{
		public override void Bake(RandomRotateAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<RandomRotateInitTag>(entity);
			SetComponentEnabled<RandomRotateInitTag>(entity, enabled: true);
		}
	}

	public float minRotateAngle;

	public float maxRotateAngle = 360f;
}
