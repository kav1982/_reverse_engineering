using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct SpecialObj4NoLock : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public Entity ett_Close;

	public Entity ett_Open;

	public Entity ett_Anima;

	public Entity ett_Motion;

	public float flyTime;

	public float openTriggerTime;

	public bool isInitialized;

	public UnityObjectRef<GameObject> go_EF;

	public bool isTriggered;

	public bool isOpenByPotion;

	public bool alreadyHandleRoomEnter;

	public bool alreadyOpen;

	public bool isAnimaOpening;

	public float openTriggerTimer;

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
