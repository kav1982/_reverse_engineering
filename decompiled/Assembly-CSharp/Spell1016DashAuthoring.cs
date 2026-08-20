using Unity.Entities;
using UnityEngine;

public class Spell1016DashAuthoring : MonoBehaviour
{
	private class Spell1016DashBaker : Baker<Spell1016DashAuthoring>
	{
		public override void Bake(Spell1016DashAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1016DashData component = new Spell1016DashData
			{
				HitSpeedCoolDownDuration = authoring.HitSpeedCoolDownDuration,
				RemainingTime = authoring.HitSpeedCoolDownDuration,
				PauseMouseEffect = false,
				AcceessTheme6StopTrail = false
			};
			AddComponent(entity, in component);
			AddComponent<Spell1016InitTag>(entity);
			SetComponentEnabled<Spell1016InitTag>(entity, enabled: true);
		}
	}

	public float HitSpeedCoolDownDuration = 0.05f;
}
