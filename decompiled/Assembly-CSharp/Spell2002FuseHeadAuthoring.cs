using Unity.Entities;
using UnityEngine;

public class Spell2002FuseHeadAuthoring : MonoBehaviour
{
	private class Spell2002FuseHeadBaker : Baker<Spell2002FuseHeadAuthoring>
	{
		public override void Bake(Spell2002FuseHeadAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			FuseHeadData component = new FuseHeadData
			{
				RootEntity = GetEntity(authoring.LegsRoot, TransformUsageFlags.Dynamic),
				FireEffectEntity = GetEntity(authoring.FireEffect, TransformUsageFlags.Dynamic),
				SafeFireEffectEntity = GetEntity(authoring.SafeFireEffect, TransformUsageFlags.Dynamic),
				HeadEntity = GetEntity(authoring.HeadEntity, TransformUsageFlags.Dynamic),
				SafeHeadEntity = GetEntity(authoring.SafeHeadEntity, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject LegsRoot;

	public GameObject FireEffect;

	public GameObject SafeFireEffect;

	public GameObject HeadEntity;

	public GameObject SafeHeadEntity;
}
