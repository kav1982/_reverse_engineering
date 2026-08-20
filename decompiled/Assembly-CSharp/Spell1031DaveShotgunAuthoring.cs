using Unity.Entities;
using UnityEngine;

public class Spell1031DaveShotgunAuthoring : MonoBehaviour
{
	public class Spell1031DaveShotgunBaker : Baker<Spell1031DaveShotgunAuthoring>
	{
		public override void Bake(Spell1031DaveShotgunAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1031DaveShotgunData component = new Spell1031DaveShotgunData
			{
				IsInitialized = false,
				CreateDestroyEffected = false
			};
			AddComponent(entity, in component);
		}
	}
}
