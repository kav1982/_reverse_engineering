using Unity.Mathematics;
using UnityEngine;

public class Boundary_T1_Lava : AbsRoomSpecialize
{
	public Transform tsf_LayerLava;

	public Transform tsf_Lava;

	[Header("Bubble")]
	public Transform tsf_LayerBubble;

	public ParticleSystem ps_Bubble;

	public float bubbleExtraDistance;

	public float centiareRate;

	public override void RoomSpecializeInitialize(RoomController roomCtrller)
	{
		float3 rootPosition = roomCtrller.CenterPoint;
		float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Lava0);
		tsf_LayerLava.position = roomCtrller.CenterPoint + (Vector3)layerPosition;
		tsf_Lava.localScale = new Vector3(60f, 60f, 1f);
		rootPosition = roomCtrller.CenterPoint;
		float3 layerPosition2 = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Lava1);
		tsf_LayerBubble.position = roomCtrller.CenterPoint + (Vector3)layerPosition2;
		Vector3 localScale = roomCtrller.RoomScale + new Vector2(bubbleExtraDistance, bubbleExtraDistance) * 2f;
		ParticleSystem.EmissionModule emission = ps_Bubble.emission;
		emission.rateOverTime = localScale.x * localScale.y * centiareRate;
		ps_Bubble.transform.localScale = localScale;
		Object.Destroy(this);
	}
}
