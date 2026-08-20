using Unity.Entities;
using UnityEngine;

public class IRoomCtrllerAuthoring : MonoBehaviour
{
	private class Baker : Baker<IRoomCtrllerAuthoring>
	{
		public override void Bake(IRoomCtrllerAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			IRoomCtrller_Dots component = default(IRoomCtrller_Dots);
			AddComponent(entity, in component);
		}
	}
}
