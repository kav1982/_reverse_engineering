public class BoundaryBase : LayerCorrect
{
	public bool IsAccessBoundary { get; set; }

	public bool HaveDetail { get; protected set; }

	public virtual void Correct(Vector2Data selfPoint, RoomController roomCtrller)
	{
	}

	public virtual void Correct2(Vector2Data selfPoint, RoomController roomCtrller)
	{
	}
}
