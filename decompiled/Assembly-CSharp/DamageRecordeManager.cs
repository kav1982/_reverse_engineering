using UnityEngine;

public static class DamageRecordeManager
{
	public static DamageRecorder historyDamageRecorder
	{
		get
		{
			WorldData selectedWorldData = DataMgr.selectedWorldData;
			if (selectedWorldData == null || selectedWorldData.battleData9 == null)
			{
				return currentDamageRecorder;
			}
			return DataMgr.selectedWorldData.battleData9.damageRecorderV2;
		}
	}

	public static DamageRecorder currentDamageRecorder { get; } = new DamageRecorder();


	public static void Recorde(int spellOrRelicId, double damage, int unitId)
	{
		currentDamageRecorder.Record(spellOrRelicId, damage);
		if (unitId != 10501)
		{
			historyDamageRecorder.Record(spellOrRelicId, damage);
		}
	}

	public static void ClearAllRecorde()
	{
		historyDamageRecorder.Clear();
		currentDamageRecorder.Clear();
	}

	public static void ClearCurrentRecorde()
	{
		currentDamageRecorder.Clear();
	}

	[RuntimeInitializeOnLoadMethod]
	private static void Initialize()
	{
		ClearAllRecorde();
	}
}
