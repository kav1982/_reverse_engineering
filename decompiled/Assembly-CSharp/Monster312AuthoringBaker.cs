using Unity.Entities;

internal class Monster312AuthoringBaker : Baker<Monster312Authoring>
{
	public override void Bake(Monster312Authoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster312_Dots component = new Monster312_Dots
		{
			aIPattern = authoring.aIPattern,
			matEntity = GetEntity(authoring.matObj, TransformUsageFlags.Dynamic),
			warningEntity = GetEntity(authoring.warningObj, TransformUsageFlags.NonUniformScale),
			warningEntity2 = GetEntity(authoring.warningObj2, TransformUsageFlags.NonUniformScale),
			warningOffset = authoring.warningObj.transform.localPosition,
			warningScale = authoring.warningObj.transform.localScale.x,
			warningScale2 = ((authoring.aIPattern == AIPattern.Pattern2) ? authoring.warningObj2.transform.localScale.x : 1f)
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 1
		};
		AddComponent(entity, in component2);
	}
}
