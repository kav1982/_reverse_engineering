using Unity.Entities;
using UnityEngine;

public class FreeScaleAuthoring : MonoBehaviour
{
	private class Baker : Baker<FreeScaleAuthoring>
	{
		public override void Bake(FreeScaleAuthoring authoring)
		{
			GetEntity(authoring.gameObject, TransformUsageFlags.NonUniformScale);
		}
	}
}
