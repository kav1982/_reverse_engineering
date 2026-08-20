using Unity.Entities;
using UnityEngine;

public class Spell1027SuperNovaAuthoring : MonoBehaviour
{
	private class Spell1027SuperNovaBaker : Baker<Spell1027SuperNovaAuthoring>
	{
		public override void Bake(Spell1027SuperNovaAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1027SuperNovaData component = new Spell1027SuperNovaData
			{
				InitOver = false,
				BoomOver = false,
				CreateFallStarTrailEffected = false
			};
			AddComponent(entity, in component);
		}
	}
}
