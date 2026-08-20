using Unity.Entities;
using UnityEngine;

internal class SelfScaleShakeAuthoring : MonoBehaviour
{
	private class SelfScaleShakeAuthoringBaker : Baker<SelfScaleShakeAuthoring>
	{
		public override void Bake(SelfScaleShakeAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SelfScaleShakeData component = new SelfScaleShakeData
			{
				BaseScale = authoring.BaseScale,
				BonusScale = authoring.BonusScale,
				ShakeSpeed = authoring.ShakeSpeed
			};
			AddComponent(entity, in component);
		}
	}

	public float BaseScale;

	public float BonusScale;

	public float ShakeSpeed;
}
