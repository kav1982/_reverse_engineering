using Unity.Entities;
using UnityEngine;

public class Spell4019BiAnBladeAuthoring : MonoBehaviour
{
	public class Spell4019BiAnBladeBaker : Baker<Spell4019BiAnBladeAuthoring>
	{
		public override void Bake(Spell4019BiAnBladeAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent<Spell4019BiAnBladeData>(entity);
		}
	}
}
