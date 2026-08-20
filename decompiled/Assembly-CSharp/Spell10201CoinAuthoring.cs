using Unity.Entities;
using UnityEngine;

public class Spell10201CoinAuthoring : MonoBehaviour
{
	private class Baker : Baker<Spell10201CoinAuthoring>
	{
		public override void Bake(Spell10201CoinAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell10201Coin component = new Spell10201Coin
			{
				ett_MR5 = GetEntity(authoring.ett_MR5, TransformUsageFlags.Dynamic),
				ett_MR20 = GetEntity(authoring.ett_MR20, TransformUsageFlags.Dynamic),
				ett_MR50 = GetEntity(authoring.ett_MR50, TransformUsageFlags.Dynamic),
				ett_MR100 = GetEntity(authoring.ett_MR100, TransformUsageFlags.Dynamic),
				ett_Anima = GetEntity(authoring.ett_Anima, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_MR5;

	public GameObject ett_MR20;

	public GameObject ett_MR50;

	public GameObject ett_MR100;

	public GameObject ett_Anima;
}
