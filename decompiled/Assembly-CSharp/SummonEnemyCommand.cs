using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class SummonEnemyCommand : BLiveCommand
{
	private static readonly int[] freeSummonIDsChapter1 = new int[10] { 100101, 100102, 100401, 100402, 100501, 100502, 100801, 101005, 101006, 105501 };

	private static readonly int[] freeSummonIDsChapter2 = new int[9] { 100701, 100601, 101005, 101006, 101801, 101901, 102301, 102101, 101501 };

	private static readonly int[] freeSummonIDsChapter3 = new int[5] { 102501, 102701, 103801, 104001, 102901 };

	private static readonly int[] freeSummonIDsChapter4 = new int[6] { 104201, 104401, 105001, 105101, 105201, 103601 };

	private static readonly int[] freeSummonIDsChapter5 = freeSummonIDsChapter3.Concat(freeSummonIDsChapter4).ToArray();

	private static readonly int[] paidSummonIDsChapter1 = new int[8] { 100103, 100301, 100802, 101004, 101201, 101802, 104101, 105502 };

	private static readonly int[] paidSummonIDsChapter2 = new int[9] { 103901, 100902, 101542, 101543, 101602, 102203, 102302, 103401, 102601 };

	private static readonly int[] paidSummonIDsChapter3 = new int[9] { 104002, 102502, 102601, 102602, 102702, 102801, 102802, 103002, 103802 };

	private static readonly int[] paidSummonIDsChapter4 = new int[8] { 104202, 104302, 104403, 104901, 105102, 105202, 105402, 103602 };

	private static readonly int[] paidSummonIDsChapter5 = paidSummonIDsChapter3.Concat(paidSummonIDsChapter4).ToArray();

	public override BLiveCommandCacheType CacheType => BLiveCommandCacheType.SummonEnemy;

	public SummonEnemyCommand(bool paid, [CanBeNull] string user)
		: base(paid, user)
	{
	}

	public override void Execute()
	{
		Vector3 vector = Tool2D.GetNavMeshPoint(PlayerMgr.Inst.PlayerCtrller.transform.position + Random.insideUnitSphere.IgnoreZ() * Random.Range(8f, 12f)).IgnoreZ();
		int randomMonsterId = GetRandomMonsterId();
		string name = UnitConfig.map[randomMonsterId].GetName();
		if (base.paid)
		{
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("来自" + base.user + "召唤的 " + name, UITextFloatType.Normal, vector + new Vector3(0f, 0.5f, 0f));
			LevelMgr.Inst.CurrentRoomCtrller.CreateUnit(vector, randomMonsterId, delegate(UnitProperty ppt)
			{
				ppt.ShowBLiveSummonerName(base.user);
			});
		}
		else
		{
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("来自热心观众召唤的 " + name, UITextFloatType.Normal, vector + new Vector3(0f, 0.5f, 0f));
			LevelMgr.Inst.CurrentRoomCtrller.CreateUnit(vector, randomMonsterId);
		}
	}

	private int GetRandomMonsterId()
	{
		int[] array = ((!base.paid) ? (BattleMgr.Inst.CurrentStage switch
		{
			1 => freeSummonIDsChapter1, 
			2 => freeSummonIDsChapter1, 
			3 => freeSummonIDsChapter2, 
			4 => freeSummonIDsChapter2, 
			5 => freeSummonIDsChapter3, 
			6 => freeSummonIDsChapter3, 
			7 => freeSummonIDsChapter4, 
			8 => freeSummonIDsChapter4, 
			_ => freeSummonIDsChapter5, 
		}) : (BattleMgr.Inst.CurrentStage switch
		{
			1 => paidSummonIDsChapter1, 
			2 => paidSummonIDsChapter1, 
			3 => paidSummonIDsChapter2, 
			4 => paidSummonIDsChapter2, 
			5 => paidSummonIDsChapter3, 
			6 => paidSummonIDsChapter3, 
			7 => paidSummonIDsChapter4, 
			8 => paidSummonIDsChapter4, 
			_ => paidSummonIDsChapter5, 
		}));
		int[] array2 = array;
		return array2[Random.Range(0, array2.Length)];
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
		if (LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Count + MonsterBorn.InRoomCount > GetMaxMonsterCountInRoom())
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
		float num = DataMgr.selectedWorldData.timeuse - LevelMgr.Inst.BattleStartTime;
		if (num < 1f || num > 60f)
		{
			return false;
		}
		return true;
	}

	private static int GetMaxMonsterCountInRoom()
	{
		if (BattleMgr.Inst == null)
		{
			return 0;
		}
		switch (BattleMgr.Inst.CurrentStage)
		{
		case 1:
		case 2:
			return 30;
		case 3:
		case 4:
			return 40;
		default:
			return 50;
		}
	}
}
