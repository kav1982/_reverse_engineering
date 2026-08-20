using Unity.Entities;
using UnityEngine;

public class Spell4012MagicShieldDataAuthoring : MonoBehaviour
{
	public class Spell4012MagicShieldBaker : Baker<Spell4012MagicShieldDataAuthoring>
	{
		public override void Bake(Spell4012MagicShieldDataAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell4012MagicShieldData>(entity);
			AddBuffer<MagicShieldDmgEvent>(entity);
		}
	}
}
