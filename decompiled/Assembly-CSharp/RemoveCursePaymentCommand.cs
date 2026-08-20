using JetBrains.Annotations;

public class RemoveCursePaymentCommand : BLiveCommand
{
	public override BLiveCommandCacheType CacheType => BLiveCommandCacheType.RelicAndCurse;

	public RemoveCursePaymentCommand([CanBeNull] string user)
		: base(paid: true, user)
	{
	}

	public override void Execute()
	{
		int? num = AddOrRemoveCurseFreeCommand.GetRandomCurseIdFromPlayer(ItemDropType.Rare) ?? AddOrRemoveCurseFreeCommand.GetRandomCurseIdFromPlayer(ItemDropType.Common) ?? AddOrRemoveCurseFreeCommand.GetRandomCurseIdFromPlayer(ItemDropType.None);
		BLiveGiftMessage bLiveGiftMessage = default(BLiveGiftMessage);
		bLiveGiftMessage.Type = BLiveGiftType.RemoveCurse;
		bLiveGiftMessage.User = base.user;
		BLiveGiftMessage message = bLiveGiftMessage;
		if (!num.HasValue)
		{
			message.Id = -1;
		}
		else
		{
			message.Id = num.Value;
			PlayerMgr.Inst.ItemCtrller.CurseRemoveByID(num.Value, 1);
		}
		UIBLiveGiftMessageCtrl.Inst.AppendMessage(message);
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
		if (base.paid && !UIBLiveGiftMessageCtrl.Inst.CanAppendMessage())
		{
			return false;
		}
		return true;
	}
}
