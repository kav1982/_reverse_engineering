using Unity.Entities;
using UnityEngine;

public class IRoomObjExtraDataAuthoring : MonoBehaviour
{
	private class Baker : Baker<IRoomObjExtraDataAuthoring>
	{
		public override void Bake(IRoomObjExtraDataAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			IRoomObjExtraData_Dots component = default(IRoomObjExtraData_Dots);
			AddComponent(entity, in component);
		}
	}
}
