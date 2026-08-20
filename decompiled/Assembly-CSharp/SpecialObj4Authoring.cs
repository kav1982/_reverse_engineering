using Unity.Entities;
using UnityEngine;

public class SpecialObj4Authoring : MonoBehaviour
{
	private class Baker : Baker<SpecialObj4Authoring>
	{
		public override void Bake(SpecialObj4Authoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			SpecialObj4_Dots component = new SpecialObj4_Dots
			{
				chestType = authoring.chestType,
				ett_Close = GetEntity(authoring.ett_Close, TransformUsageFlags.Dynamic),
				ett_Open = GetEntity(authoring.ett_Open, TransformUsageFlags.Dynamic),
				ett_Anima = GetEntity(authoring.ett_Anima, TransformUsageFlags.Dynamic),
				ett_Motion = GetEntity(authoring.ett_Motion, TransformUsageFlags.Dynamic),
				flyTime = authoring.flyTime
			};
			AddComponent(entity, in component);
		}
	}

	public ChestType chestType;

	public GameObject ett_Close;

	public GameObject ett_Open;

	public GameObject ett_Anima;

	public GameObject ett_Motion;

	public float flyTime;
}
