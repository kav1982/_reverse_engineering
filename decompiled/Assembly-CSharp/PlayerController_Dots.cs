using Unity.Entities;
using Unity.Mathematics;

public struct PlayerController_Dots : IComponentData, IQueryTypeParameter
{
	public float interactiveRadius;

	public UnityObjectRef<PlayerController> playerCtrllerMono;

	public bool needUpdateShieldUI;

	public bool needUpdateTempShieldUI;

	public bool isPlayerDropBlood;

	public bool isTriggerResurgence;

	public bool isHurtThisFrameForPlayer;

	public bool holdingShoot;

	public float3 mousePosition;
}
