using Unity.Entities;
using UnityEngine;

internal class Spell3110LivingTieAuthoring : MonoBehaviour
{
	private class Spell3110LivingTieAuthoringBaker : Baker<Spell3110LivingTieAuthoring>
	{
		public override void Bake(Spell3110LivingTieAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell3110LivingTieComponent component = new Spell3110LivingTieComponent
			{
				tie1 = GetEntity(authoring.tie1Obj, TransformUsageFlags.Dynamic),
				tie2 = GetEntity(authoring.tie2Obj, TransformUsageFlags.Dynamic),
				tieFire = GetEntity(authoring.tieFire1, TransformUsageFlags.Dynamic),
				tieFire2 = GetEntity(authoring.tieFire2, TransformUsageFlags.Dynamic),
				starting = false
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject tie1Obj;

	public GameObject tie2Obj;

	public GameObject tieFire1;

	public GameObject tieFire2;
}
