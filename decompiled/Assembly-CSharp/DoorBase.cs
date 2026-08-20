public class DoorBase : InteractiveObj
{
	protected RoomController belongRoom;

	public LevelRewardType RewardType { get; protected set; }

	public bool IsExtraDoor { get; set; }

	protected void DoorOpenSE()
	{
		SEMgr.Inst.openDoor_T0.PlaySE(base.transform.position);
	}

	public virtual void Initialize(RoomController belongRoom, LevelRewardType rewardType)
	{
		this.belongRoom = belongRoom;
		RewardType = rewardType;
		UpdateDisplay();
	}

	public virtual void UpdateDisplay()
	{
	}

	public virtual void ResetType(LevelRewardType rewardType)
	{
	}

	public virtual void Open()
	{
	}

	public virtual void OpenDirect()
	{
	}
}
