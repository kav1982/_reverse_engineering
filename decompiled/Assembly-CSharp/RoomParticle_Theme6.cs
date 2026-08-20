using UnityEngine;

public class RoomParticle_Theme6 : AbsRoomSpecialize
{
	public ParticleSystem ps;

	public float centiareRate;

	public override void RoomSpecializeInitialize(RoomController roomCtrller)
	{
		ParticleSystem.EmissionModule emission = ps.emission;
		ParticleSystem.ShapeModule shape = ps.shape;
		emission.rateOverTime = roomCtrller.RoomScale.x * roomCtrller.RoomScale.y * centiareRate;
		shape.scale = roomCtrller.RoomScale;
		base.transform.position = Tool2D.GetLayerPoint(roomCtrller.CenterPoint, LayerCorrectType.RoomParticle);
		Object.Destroy(this);
	}
}
