using UnityEngine;
using UnityEngine.UI;

public class UIBLiveSummonEnemyTimeCtrl : MonoBehaviour
{
	public Text text;

	private void Awake()
	{
		if (BLiveMgr.Inst == null)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		BLiveMgr inst = BLiveMgr.Inst;
		if ((object)inst != null && inst.HasLineError)
		{
			text.text = "<color=red>直播互动网络异常</color>";
			return;
		}
		if (!(BattleMgr.Inst == null) && !(LevelMgr.Inst == null))
		{
			LevelMgr inst2 = LevelMgr.Inst;
			if ((object)inst2 != null)
			{
				RoomController currentRoomCtrller = inst2.CurrentRoomCtrller;
				if ((object)currentRoomCtrller != null && currentRoomCtrller.IsFinish)
				{
					goto IL_006d;
				}
			}
			inst2 = LevelMgr.Inst;
			if ((object)inst2 == null || inst2.CurrentRoomCfg.isFinalRoom)
			{
				if (GetRemainTime() <= 0)
				{
					text.text = "怪物召唤已暂停";
				}
				else
				{
					text.text = GetRemainTimeFormated() + " 秒内可以召唤怪物";
				}
				return;
			}
		}
		goto IL_006d;
		IL_006d:
		text.text = "进入战斗开启怪物召唤互动玩法";
	}

	private int GetRemainTime()
	{
		return Mathf.Max(0, 60 - Mathf.RoundToInt(DataMgr.selectedWorldData.timeuse - LevelMgr.Inst.BattleStartTime));
	}

	private string GetRemainTimeFormated()
	{
		return $"<size=42><i>{GetRemainTime()}</i></size>";
	}
}
