using Unity.Entities;
using UnityEngine;

internal class SelfPeriodRandomRotateAuthoring : MonoBehaviour
{
	private class SelfPeriodRandomRotateAuthoringBaker : Baker<SelfPeriodRandomRotateAuthoring>
	{
		public override void Bake(SelfPeriodRandomRotateAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SelfPeriodRandomRotateData component = new SelfPeriodRandomRotateData
			{
				ChangePeriod = authoring.ChangePeriod,
				Timer = authoring.TimeOffset
			};
			AddComponent(entity, in component);
		}
	}

	public float TimeOffset;

	public float ChangePeriod;
}
