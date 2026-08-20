using Unity.Entities;
using UnityEngine;

public class Boundary2_T10Authoring : MonoBehaviour
{
	private class Baker : Baker<Boundary2_T10Authoring>
	{
		public override void Bake(Boundary2_T10Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Boundary2_T10 component = default(Boundary2_T10);
			component.detailChancePotion = authoring.detailChancePotion;
			component.intervalPotion = authoring.intervalPotion;
			component.offsetPotion = authoring.offsetPotion;
			component.ett_DetailPotion = GetEntity(authoring.ett_DetailPotion, TransformUsageFlags.Dynamic);
			component.detailChanceStore = authoring.detailChanceStore;
			component.intervalStore = authoring.intervalStore;
			component.offsetStore = authoring.offsetStore;
			component.ett_DetailStore = GetEntity(authoring.ett_DetailStore, TransformUsageFlags.Dynamic);
			AddComponent(entity, in component);
		}
	}

	[Range(0f, 1f)]
	[Header("Potion")]
	public float detailChancePotion;

	public int intervalPotion;

	public float offsetPotion;

	public GameObject ett_DetailPotion;

	[Range(0f, 1f)]
	[Header("Store")]
	public float detailChanceStore;

	public int intervalStore;

	public float offsetStore;

	public GameObject ett_DetailStore;
}
