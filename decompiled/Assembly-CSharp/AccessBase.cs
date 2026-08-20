public class AccessBase : InteractiveObj
{
	public RoomController BelongRoom { get; protected set; }

	public AccessType AccessType { get; protected set; }

	public FourDir Dir { get; protected set; }

	public bool IsConditionComplete { get; protected set; } = true;


	protected void AccessOpenSE()
	{
		SEMgr.Inst.openDoor_T0.PlaySE(base.transform.position);
	}

	public virtual void Initialize(RoomController belongRoom, AccessType type, FourDir dir)
	{
		BelongRoom = belongRoom;
		AccessType = type;
		Dir = dir;
		belongRoom.accesses.Add(this);
	}

	public virtual void Open()
	{
	}

	public virtual void OpenDirect()
	{
	}

	public virtual void Close()
	{
	}

	public virtual void CloseDirect()
	{
	}

	public virtual void ConditionComplete()
	{
		IsConditionComplete = true;
	}
}
