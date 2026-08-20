using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct SpecialObj4_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public ChestType chestType;

	public Entity ett_Close;

	public Entity ett_Open;

	public Entity ett_Anima;

	public Entity ett_Motion;

	public float flyTime;

	public bool isInitialized;

	public UnityObjectRef<GameObject> go_EF;

	public int curseID;

	public bool isOpenByPotion;

	public bool alreadyHandleRoomEnter;

	public bool alreadyOpen;

	public bool onFly;

	public bool isFlying;

	public float flySpeed;

	public float3 flyPosition;

	public void SetFly(float3 flyPosition)
	{
		onFly = true;
		this.flyPosition = flyPosition;
	}
}
