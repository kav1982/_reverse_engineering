using Unity.Entities;
using UnityEngine;

public class UnitBaseAuthoring : MonoBehaviour
{
	private class Baker : Baker<UnitBaseAuthoring>
	{
		public override void Bake(UnitBaseAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			UnitBase_Dots component = default(UnitBase_Dots);
			component.isAutoFlip = authoring.autoFlip;
			component.moveThreshold = authoring.moveThreshold;
			component.moveLerp = authoring.moveLerp;
			component.ett_AnimaRoot = GetEntity(authoring.animaRoot, TransformUsageFlags.NonUniformScale);
			Transform transform = authoring.transform.Find("Layer/BeHit");
			if (transform != null)
			{
				component.ett_Flip = GetEntity(transform.gameObject, TransformUsageFlags.NonUniformScale);
			}
			AddComponent(entity, in component);
		}
	}

	public bool autoFlip;

	public float moveLerp = 10f;

	public float moveThreshold = 0.1f;

	public GameObject animaRoot;
}
