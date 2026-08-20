using JetBrains.Annotations;
using UnityEngine;

public class AddCursePaymentCommand : BLiveCommand
{
	public override BLiveCommandCacheType CacheType => BLiveCommandCacheType.RelicAndCurse;

	public AddCursePaymentCommand([CanBeNull] string user)
		: base(paid: true, user)
	{
	}

	public override void Execute()
	{
		BLiveGiftMessage msg = new BLiveGiftMessage
		{
			Type = BLiveGiftType.AddCurse,
			Id = AddOrRemoveCurseFreeCommand.GetRandomCurseIdFromPool(ItemDropType.Rare),
			User = base.user
		};
		msg.OnAction = delegate(RectTransform rect)
		{
			PlayerMgr.Inst.ItemCtrller.AddRewardFly(msg.Id, RollRewardFly.DropType.Curse, rect.position, CamController.Inst.cam_UI);
		};
		UIBLiveGiftMessageCtrl.Inst.AppendMessage(msg);
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
