using Unity.Entities;
using UnityEngine;

public class Access_T13Authoring : MonoBehaviour
{
	private class Baker : Baker<Access_T13Authoring>
	{
		public override void Bake(Access_T13Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Access_T13_Dots component = default(Access_T13_Dots);
			AddComponent(entity, in component);
		}
	}
}
