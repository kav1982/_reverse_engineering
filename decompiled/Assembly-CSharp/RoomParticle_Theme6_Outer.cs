using UnityEngine;

public class RoomParticle_Theme6_Outer : AbsRoomSpecialize
{
	public ParticleSystem ps_Up;

	public ParticleSystem ps_Right;

	public ParticleSystem ps_Down;

	public ParticleSystem ps_Left;

	public float boundaryMaxWidth;

	public float boundaryMaxHeight;

	public float offset;

	public float centiareRate;

	public override void RoomSpecializeInitialize(RoomController roomCtrller)
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -2f);
		ParticleSystem.ShapeModule shape = ps_Up.shape;
		ParticleSystem.EmissionModule emission = ps_Up.emission;
		shape.scale = new Vector2(roomCtrller.roomCfg.theme6Width, (boundaryMaxHeight - (float)roomCtrller.roomCfg.theme6Height) / 2f);
		emission.rateOverTime = shape.scale.x * shape.scale.y * centiareRate;
		ps_Up.transform.localPosition = new Vector3(0f, (float)roomCtrller.roomCfg.theme6Height / 2f + shape.scale.y / 2f + offset, 0f);
		shape = ps_Down.shape;
		emission = ps_Down.emission;
		shape.scale = new Vector2(roomCtrller.roomCfg.theme6Width, (boundaryMaxHeight - (float)roomCtrller.roomCfg.theme6Height) / 2f);
		emission.rateOverTime = shape.scale.x * shape.scale.y * centiareRate;
		ps_Down.transform.localPosition = new Vector3(0f, (float)(-roomCtrller.roomCfg.theme6Height) / 2f - shape.scale.y / 2f - offset, 0f);
		shape = ps_Right.shape;
		emission = ps_Right.emission;
		shape.scale = new Vector2((boundaryMaxWidth - (float)roomCtrller.roomCfg.theme6Width) / 2f, boundaryMaxHeight);
		emission.rateOverTime = shape.scale.x * shape.scale.y * centiareRate;
		ps_Right.transform.localPosition = new Vector3((float)roomCtrller.roomCfg.theme6Width / 2f + shape.scale.x / 2f + offset, 0f, 0f);
		shape = ps_Left.shape;
		emission = ps_Left.emission;
		shape.scale = new Vector2((boundaryMaxWidth - (float)roomCtrller.roomCfg.theme6Width) / 2f, boundaryMaxHeight);
		emission.rateOverTime = shape.scale.x * shape.scale.y * centiareRate;
		ps_Left.transform.localPosition = new Vector3((float)(-roomCtrller.roomCfg.theme6Width) / 2f - shape.scale.x / 2f - offset, 0f, 0f);
		Object.Destroy(this);
	}
}
