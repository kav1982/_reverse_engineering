using JetBrains.Annotations;

public class RecoverFromLikes : BLiveCommand
{
	public override BLiveCommandCacheType CacheType => BLiveCommandCacheType.NoCache;

	public RecoverFromLikes([CanBeNull] string user)
		: base(paid: false, user)
	{
	}

	public override void Execute()
	{
		if (PlayerMgr.Inst.PlayerPpt.unitCfg.currentHP < PlayerMgr.Inst.PlayerPpt.unitCfg.maxHP)
		{
			PlayerMgr.Inst.PlayerPpt.unitCfg.currentHP += 1f;
			UIPlayerDataMgr.Inst.UpdateHP();
		}
		UIBLiveHpMessageCtrl.Inst.NewText();
	}

	public override bool CanExecute()
	{
		if (BattleMgr.Inst == null)
		{
			return false;
		}
		if (LevelMgr.Inst == null)
		{
			return false;
		}
		if (LevelMgr.Inst.CurrentRoomCtrller.IsFinish)
		{
			return false;
		}
		if (!LevelMgr.Inst.CurrentRoomCfg.isFinalRoom)
		{
			return false;
		}
		if (TimeScaleMgr.Inst.GamePaused)
		{
			return false;
		}
		return true;
	}
}
