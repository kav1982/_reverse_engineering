using Unity.Entities;
using UnityEngine;

public class SpecialObj4NoLockAuthoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj4NoLockAuthoring>
	{
		public override void Bake(SpecialObj4NoLockAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj4NoLock component = new SpecialObj4NoLock
			{
				ett_Close = GetEntity(authoring.ett_Close, TransformUsageFlags.Dynamic),
				ett_Open = GetEntity(authoring.ett_Open, TransformUsageFlags.Dynamic),
				ett_Anima = GetEntity(authoring.ett_Anima, TransformUsageFlags.Dynamic),
				ett_Motion = GetEntity(authoring.ett_Motion, TransformUsageFlags.Dynamic),
				flyTime = authoring.flyTime,
				openTriggerTime = authoring.openTriggerTime
			};
			AddComponent(entity, in component);
		}
	}

	public GameObject ett_Close;

	public GameObject ett_Open;

	public GameObject ett_Anima;

	public GameObject ett_Motion;

	public float flyTime;

	public float openTriggerTime;
}
