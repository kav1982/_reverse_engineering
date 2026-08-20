using Unity.Entities;
using UnityEngine;

public class AccessBaseAuthoring : MonoBehaviour
{
	private class Baker : Baker<AccessBaseAuthoring>
	{
		public override void Bake(AccessBaseAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AccessBase_Dots component = new AccessBase_Dots
			{
				ett_AccessTriggerLR = GetEntity(authoring.ett_AccessTriggerLR, TransformUsageFlags.Dynamic),
				ett_AccessTriggerUD = GetEntity(authoring.ett_AccessTriggerUD, TransformUsageFlags.Dynamic),
				ett_Anima = GetEntity(authoring.ett_Anima, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_AccessTriggerLR;

	public GameObject ett_AccessTriggerUD;

	public GameObject ett_Anima;
}
