using UnityEngine;

public class Boundary_T6 : BoundaryBase
{
	[Space(50f)]
	public MeshRenderer mr;

	public int maxScreenRoomHeight;

	public float maxScreenShaderHeight;

	public float extraHeight;

	public override void Correct(Vector2Data selfPoint, RoomController roomCtrller)
	{
		Vector2 vector = new Vector2((float)roomCtrller.roomCfg.theme6Width / (float)maxScreenRoomHeight * maxScreenShaderHeight + extraHeight, (float)roomCtrller.roomCfg.theme6Height / (float)maxScreenRoomHeight * maxScreenShaderHeight + extraHeight);
		mr.material.SetVector("_WidthHeight", vector);
	}
}
