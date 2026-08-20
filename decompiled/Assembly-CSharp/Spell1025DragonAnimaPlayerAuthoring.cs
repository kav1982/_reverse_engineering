using Unity.Entities;
using UnityEngine;

public class Spell1025DragonAnimaPlayerAuthoring : MonoBehaviour
{
	public class Spell1025Baker : Baker<Spell1025DragonAnimaPlayerAuthoring>
	{
		public override void Bake(Spell1025DragonAnimaPlayerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell1025FirePointAnimaData component = new Spell1025FirePointAnimaData
			{
				MinIndex = authoring.MinFrame,
				MaxIndex = authoring.MaxFrame
			};
			AddComponent(entity, in component);
			Spell1025AnimaDataInitTag component2 = default(Spell1025AnimaDataInitTag);
			AddComponent(entity, in component2);
			Spell1025ChangeFrameAnim component3 = default(Spell1025ChangeFrameAnim);
			AddComponent(entity, in component3);
			SetComponentEnabled<Spell1025AnimaDataInitTag>(entity, enabled: true);
		}
	}

	public int MinFrame;

	public int MaxFrame;
}
