using Unity.Entities;

internal class Monster304AuthoringBaker : Baker<Monster304Authoring>
{
	public override void Bake(Monster304Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster304_Dots component = new Monster304_Dots
		{
			moveDir = Tool2D.GetDir(),
			rotateSpeed = new RandomFloat
			{
				value1 = authoring.rotateSpeed.value1,
				value2 = authoring.rotateSpeed.value2
			},
			isPattern2 = authoring.isPattern2,
			shadowLayer = GetEntity(authoring.shadowLayer, TransformUsageFlags.NonUniformScale),
			shadowRotateRoot = GetEntity(authoring.shadowRotateRoot, TransformUsageFlags.NonUniformScale),
			flame1 = GetEntity(authoring.flame1, TransformUsageFlags.NonUniformScale),
			flame2 = GetEntity(authoring.flame2, TransformUsageFlags.NonUniformScale),
			speedRotateFix = authoring.speedRotateFix
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 1
		};
		AddComponent(entity, in component2);
	}
}
