using Unity.Entities;
using UnityEngine;

internal class Spell2002SummonAuthoring : MonoBehaviour
{
	private class Spell2002SummonAuthoringBaker : Baker<Spell2002SummonAuthoring>
	{
		public override void Bake(Spell2002SummonAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell2002Data component = new Spell2002Data
			{
				State = Spell2002State.Initialize,
				IsLegInvisible = true
			};
			AddComponent(entity, in component);
			AddComponent<Spell2002InitTag>(entity);
			SetComponentEnabled<Spell2002InitTag>(entity, enabled: true);
			AddComponent<Spell2002StartFuseTag>(entity);
			SetComponentEnabled<Spell2002StartFuseTag>(entity, enabled: false);
			AddComponent<Spell2002StartGhostTag>(entity);
			SetComponentEnabled<Spell2002StartGhostTag>(entity, enabled: false);
			AddBuffer<LegsData>(entity);
			AddBuffer<EssenceLegsData>(entity);
			AddBuffer<LegsTarget>(entity);
			AddBuffer<LegsAttackData>(entity);
			AddBuffer<EssenceLegAttackedEntity>(entity);
			AddBuffer<FuseHeadEntity>(entity);
		}
	}
}
