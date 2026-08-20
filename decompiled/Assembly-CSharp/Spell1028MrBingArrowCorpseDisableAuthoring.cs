using Unity.Entities;
using UnityEngine;

internal class Spell1028MrBingArrowCorpseDisableAuthoring : MonoBehaviour
{
	private class Spell1028MrBingArrowDisableAuthoringBaker : Baker<Spell1028MrBingArrowCorpseDisableAuthoring>
	{
		public override void Bake(Spell1028MrBingArrowCorpseDisableAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1028MrBingArrowCorpseDisableTag component = default(Spell1028MrBingArrowCorpseDisableTag);
			AddComponent(entity, in component);
			SetComponentEnabled<Spell1028MrBingArrowCorpseDisableTag>(entity, enabled: true);
			DynamicBuffer<Spell1028ArrowCorpseEntity> dynamicBuffer = AddBuffer<Spell1028ArrowCorpseEntity>(entity);
			GameObject[] arrowCorpseObjs = authoring.arrowCorpseObjs;
			foreach (GameObject authoring2 in arrowCorpseObjs)
			{
				dynamicBuffer.Add(new Spell1028ArrowCorpseEntity
				{
					entity = GetEntity(authoring2, TransformUsageFlags.Dynamic)
				});
			}
		}
	}

	public GameObject[] arrowCorpseObjs;
}
