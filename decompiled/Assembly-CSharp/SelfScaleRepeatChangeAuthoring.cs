using Unity.Entities;
using UnityEngine;

internal class SelfScaleRepeatChangeAuthoring : MonoBehaviour
{
	private class SelfScaleRepeatChangeAuthoringBaker : Baker<SelfScaleRepeatChangeAuthoring>
	{
		public override void Bake(SelfScaleRepeatChangeAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			ScaleRepeatChangeData component = new ScaleRepeatChangeData
			{
				BaseScale = authoring.BaseScale,
				TargetScale = authoring.TargetScale,
				ChangePeriod = authoring.ChangePeriod,
				TimeOffset = authoring.TimeOffset
			};
			AddComponent(entity, in component);
		}
	}

	public float BaseScale;

	public float TargetScale;

	public float ChangePeriod;

	public float TimeOffset;
}
