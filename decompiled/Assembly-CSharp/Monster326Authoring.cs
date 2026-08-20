using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Monster326Authoring : MonoBehaviour
{
	private class Monster326AuthoringBaker : Baker<Monster326Authoring>
	{
		public override void Bake(Monster326Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Monster326_Dots component = new Monster326_Dots
			{
				tailSwiggleAngle = math.radians(authoring.tailSwiggleAngle),
				tailSwiggleSpeed = authoring.tailSwiggleSpeed,
				tailSpacing = authoring.tailSpacing,
				IsInitialized = false
			};
			AddComponent(entity, in component);
			EndlessMonsterTag component2 = default(EndlessMonsterTag);
			AddComponent(entity, in component2);
			DynamicBuffer<TailSegment_Dots> dynamicBuffer = AddBuffer<TailSegment_Dots>(entity);
			if (authoring.tailTransforms != null)
			{
				for (int i = 0; i < authoring.tailTransforms.Length; i++)
				{
					Entity entity2 = GetEntity(authoring.tailTransforms[i], TransformUsageFlags.Dynamic | TransformUsageFlags.WorldSpace);
					Entity entity3 = GetEntity(authoring.shadowTransforms[i], TransformUsageFlags.Dynamic | TransformUsageFlags.WorldSpace);
					dynamicBuffer.Add(new TailSegment_Dots
					{
						Entity = entity2,
						ShadowEntity = entity3
					});
				}
			}
		}
	}

	[Range(0f, 180f)]
	[Header("Movement (2D)")]
	public float tailSwiggleAngle = 60f;

	public float tailSwiggleSpeed = 10f;

	[Header("Tail Settings")]
	public Transform[] tailTransforms;

	public Transform[] shadowTransforms;

	public float tailSpacing = 0.5f;
}
