using Unity.Entities;
using UnityEngine;

public class Monster23Authoring : MonoBehaviour
{
	public class Baker : Baker<Monster23Authoring>
	{
		public override void Bake(Monster23Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Monster23_Dots component = new Monster23_Dots
			{
				pattern = authoring.pattern,
				roamRadius = authoring.roamRadius,
				deadSpellCount = authoring.deadSpellCount,
				tantacleWaveSpeed = authoring.tantacleWaveSpeed,
				state = Monster23State.BornIdle
			};
			AddComponent(entity, in component);
		}
	}

	public VariableFloat roamRadius;

	public RandomFloat deadSpellCount;

	[Header("Tentacle")]
	public VariableFloat tantacleWaveSpeed;

	public VariableFloat tantacleWaveRatio;

	public AIPattern pattern;

	[Header("pattern2")]
	public VariableInt beHitSpellCount;

	[Header("Spell Butterfly")]
	public float spellHeight;

	public float spellOffset;

	public VariableFloat spellSpeed;

	public float spellDuration;

	public float spellMinSpeed;

	public float spellDamageRatio;
}
