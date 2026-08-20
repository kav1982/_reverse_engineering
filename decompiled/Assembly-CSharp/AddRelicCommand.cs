using System;
using JetBrains.Annotations;
using UnityEngine;

public class AddRelicCommand : BLiveCommand
{
	public override BLiveCommandCacheType CacheType => BLiveCommandCacheType.RelicAndCurse;

	public AddRelicCommand(bool paid, [CanBeNull] string user)
		: base(paid, user)
	{
	}

	public override void Execute()
	{
		if (base.paid)
		{
			ItemDropType relicType = ((UnityEngine.Random.Range(0f, 1f) >= 0.7f) ? ItemDropType.Epic : ItemDropType.Rare);
			BLiveGiftMessage msg = new BLiveGiftMessage
			{
				Type = BLiveGiftType.AddRelic,
				Id = GetRandomRelicIdFromPool(relicType),
				User = base.user
			};
			ref Action<RectTransform> onAction = ref msg.OnAction;
			onAction = (Action<RectTransform>)Delegate.Combine(onAction, (Action<RectTransform>)delegate(RectTransform rect)
			{
				PlayerMgr.Inst.ItemCtrller.AddRewardFly(msg.Id, RollRewardFly.DropType.Relic, rect.position, CamController.Inst.cam_UI);
			});
			UIBLiveGiftMessageCtrl.Inst.AppendMessage(msg);
		}
		else
		{
			ItemDropType relicType2 = ((!(UnityEngine.Random.Range(0f, 1f) >= 0.7f)) ? ItemDropType.Common : ItemDropType.Rare);
			int randomRelicIdFromPool = GetRandomRelicIdFromPool(relicType2);
			string name = RelicConfig.dic[randomRelicIdFromPool].GetName();
			PlayerMgr.Inst.ItemCtrller.AddRewardFly(randomRelicIdFromPool, RollRewardFly.DropType.Relic, PlayerMgr.Inst.PlayerCtrller.transform.position);
			PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd("热心观众送来遗物：" + name, UITextFloatType.Normal);
		}
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

	private int GetRandomRelicIdFromPool(ItemDropType relicType)
	{
		return PlayerMgr.Inst.BaData.GetRelicFromPool(relicType, new int[1] { 69 });
	}
}
