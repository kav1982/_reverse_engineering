using Unity.Entities;
using UnityEngine;

internal class SelfRotateEntityAuthoring : MonoBehaviour
{
	private class SelfRotateEntityAuthoringBaker : Baker<SelfRotateEntityAuthoring>
	{
		public override void Bake(SelfRotateEntityAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SelfRotateData component = new SelfRotateData
			{
				RotateSpeed = authoring.RotateSpeed
			};
			AddComponent(entity, in component);
		}
	}

	public float RotateSpeed;
}
