using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class InteractiveObjAuthoring : MonoBehaviour
{
	private class Baker : Baker<InteractiveObjAuthoring>
	{
		public override void Bake(InteractiveObjAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			InteractiveObj_Dots component = new InteractiveObj_Dots
			{
				ett_Outline = GetEntity(authoring.ett_Outline, TransformUsageFlags.Dynamic),
				type = authoring.type,
				uiOffset = authoring.uiOffset
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Outline;

	public InteractiveObjType type;

	public float3 uiOffset;
}
