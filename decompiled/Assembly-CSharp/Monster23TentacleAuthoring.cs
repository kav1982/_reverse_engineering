using Unity.Entities;
using UnityEngine;

public class Monster23TentacleAuthoring : MonoBehaviour
{
	public class Baker : Baker<Monster23TentacleAuthoring>
	{
		public override void Bake(Monster23TentacleAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Monster23_Dots_WaveSpeed component = new Monster23_Dots_WaveSpeed
			{
				waveSpeed = authoring.tantacleWaveSpeed.RandomResult()
			};
			AddComponent(entity, in component);
			Monster23_Dots_WaveRatio component2 = new Monster23_Dots_WaveRatio
			{
				tantacleWaveRatio = authoring.tantacleWaveRatio.RandomResult()
			};
			AddComponent(entity, in component2);
		}
	}

	[Header("Tentacle")]
	public VariableFloat tantacleWaveSpeed;

	public VariableFloat tantacleWaveRatio;
}
