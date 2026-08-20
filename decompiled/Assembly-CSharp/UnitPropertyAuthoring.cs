using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class UnitPropertyAuthoring : MonoBehaviour
{
	private class Baker : Baker<UnitPropertyAuthoring>
	{
		public override void Bake(UnitPropertyAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			UnitProperty_Dots component = default(UnitProperty_Dots);
			int.TryParse(authoring.gameObject.name, out component.id);
			if (component.id != 800001)
			{
				Transform transform = authoring.transform.Find("Layer/BeHit");
				if (!(transform == null))
				{
					component.ett_BeHit = GetEntity(transform.gameObject, TransformUsageFlags.Dynamic);
				}
				Transform transform2 = authoring.transform.Find("Layer/BeHit/Model/Motion");
				if (!(transform2 == null))
				{
					component.ett_Motion = GetEntity(transform2.gameObject, TransformUsageFlags.Dynamic);
				}
				DynamicBuffer<UnitMREttBED> dynamicBuffer = AddBuffer<UnitMREttBED>(entity);
				if (transform != null)
				{
					MeshRenderer[] componentsInChildren = transform.GetComponentsInChildren<MeshRenderer>();
					if (componentsInChildren.Length != 0)
					{
						for (int i = 0; i < componentsInChildren.Length; i++)
						{
							dynamicBuffer.Add(new UnitMREttBED
							{
								ett = GetEntity(componentsInChildren[i].gameObject, TransformUsageFlags.Dynamic)
							});
						}
					}
				}
			}
			else
			{
				AddBuffer<UnitMREttBED>(entity);
			}
			AddComponent(entity, in component);
			AddBuffer<TakeDamageInfo_Dots>(entity);
			UnitDead component2 = default(UnitDead);
			AddComponent(entity, in component2);
		}
	}
}
