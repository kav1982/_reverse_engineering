using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class AddOrRemoveCurseFreeCommand : BLiveCommand
{
	private static readonly float[] _randomWeights = new float[3] { 40f, 50f, 10f };

	public override BLiveCommandCacheType CacheType => BLiveCommandCacheType.RelicAndCurse;

	public AddOrRemoveCurseFreeCommand([CanBeNull] string user)
		: base(paid: false, user)
	{
	}

	public override void Execute()
	{
		switch (GeneralTool.GetWeightRandom(_randomWeights))
		{
		case 0:
		{
			int randomCurseIdFromPool = GetRandomCurseIdFromPool(ItemDropType.Common);
			PlayerMgr.Inst.ItemCtrller.CurseAdd(randomCurseIdFromPool, PlayerMgr.Inst.PlayerPoint);
			string name2 = CurseConfig.dic[randomCurseIdFromPool].GetName();
			PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd("来自热心观众的诅咒：" + name2, UITextFloatType.Normal);
			break;
		}
		case 1:
		{
			int? num2 = RemoveRandomCurse(ItemDropType.Common);
			if (num2.HasValue)
			{
				string name3 = CurseConfig.dic[num2.Value].GetName();
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd("热心观众消除了一个诅咒：" + name3, UITextFloatType.Normal);
			}
			break;
		}
		case 2:
		{
			int? num = RemoveRandomCurse(ItemDropType.Rare);
			if (num.HasValue)
			{
				string name = CurseConfig.dic[num.Value].GetName();
				PlayerMgr.Inst.PlayerCtrller.TextFloatQueueAdd("热心观众消除了一个诅咒：" + name, UITextFloatType.Normal);
			}
			break;
		}
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
		return true;
	}

	public static int GetRandomCurseIdFromPool(ItemDropType curseType)
	{
		int num = 0;
		int num2 = -1;
		while (true)
		{
			num++;
			if (num > 100)
			{
				num2 = 999;
				break;
			}
			num2 = PlayerMgr.Inst.BaData.GetCurseFromPool(curseType);
			if (num2 != 30 && num2 != 31 && num2 != 38 && num2 != 61)
			{
				break;
			}
			PlayerMgr.Inst.BaData.BackCurseToPool(num2, 1);
		}
		return num2;
	}

	public static int? GetRandomCurseIdFromPlayer(ItemDropType curseType)
	{
		CurseConfig[] array = (from e in PlayerMgr.Inst.BaData.curseIDs
			select CurseConfig.dic[e] into e
			where e.dropType == curseType
			select e).ToArray();
		if (array.Length == 0)
		{
			return null;
		}
		return array[Random.Range(0, array.Length)].id;
	}

	private static int? RemoveRandomCurse(ItemDropType curseType)
	{
		int? randomCurseIdFromPlayer = GetRandomCurseIdFromPlayer(curseType);
		if (!randomCurseIdFromPlayer.HasValue)
		{
			return null;
		}
		PlayerMgr.Inst.ItemCtrller.CurseRemoveByID(randomCurseIdFromPlayer.Value, 1, textFloat: false);
		return randomCurseIdFromPlayer;
	}
}
