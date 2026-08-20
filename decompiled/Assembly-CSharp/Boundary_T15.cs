using UnityEngine;

public class Boundary_T15 : BoundaryBase
{
	public override void Correct(Vector2Data selfPoint, RoomController roomCtrller)
	{
		Object.Destroy(this);
	}
}
