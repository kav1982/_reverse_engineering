using Unity.Entities;
using UnityEngine;

public class Monster16Authoring : MonoBehaviour
{
	public class Baker : Baker<Monster16Authoring>
	{
		public override void Bake(Monster16Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Monster16_Dots component = new Monster16_Dots
			{
				pattern = authoring.pattern,
				idleTime = authoring.idleTime,
				moveRandomRadius = authoring.moveRandomRadius,
				rockDistance = authoring.rockDistance,
				rockRotateSpeed = authoring.rockRotateSpeed,
				state = Monster16State.BornIdle
			};
			AddComponent(entity, in component);
			DynamicBuffer<Monster16_DotsRock> dynamicBuffer = AddBuffer<Monster16_DotsRock>(entity);
			for (int i = 0; i < authoring.tsf_Rocks.Length; i++)
			{
				dynamicBuffer.Add(new Monster16_DotsRock
				{
					entity = GetEntity(authoring.tsf_Rocks[i], TransformUsageFlags.Dynamic)
				});
			}
		}
	}

	[Space(50f)]
	public AIPattern pattern;

	public VariableFloat idleTime;

	public VariableFloat moveRandomRadius;

	[Header("Rock")]
	public Transform[] tsf_Rocks;

	public float rockDistance;

	public float rockRotateSpeed;

	[Header("Pattern3,4")]
	public float gravityRange;

	public float gravityPush;

	public VariableFloat gravityTime;

	public float gravityTimer;

	public bool stateQuit;
}
