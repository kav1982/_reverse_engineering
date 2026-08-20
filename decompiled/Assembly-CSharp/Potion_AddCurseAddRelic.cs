using UnityEngine;

public class Potion_AddCurseAddRelic : MonoBehaviour
{
	public float getInterval;

	private float getIntervalTimer = 10f;

	private int curseCount;

	private int relicCount;

	private int curseCounter;

	private int relicCounter;

	public void Initialize(PotionConfig potionCfg)
	{
		curseCount = Random.Range(1, potionCfg.int1 + 1);
		relicCount = Random.Range(1, potionCfg.int2 + 1);
	}

	private void Update()
	{
		getIntervalTimer += Time.deltaTime;
		if (!(getIntervalTimer >= getInterval))
		{
			return;
		}
		getIntervalTimer = 0f;
		if (curseCounter < curseCount)
		{
			int num = 0;
			int num2;
			while (true)
			{
				num++;
				if (num > 100)
				{
					num2 = 999;
					break;
				}
				num2 = PlayerMgr.Inst.BaData.GetCurseFromPool(ItemDropType.Common);
				if (num2 != 30)
				{
					break;
				}
				PlayerMgr.Inst.BaData.BackCurseToPool(num2, 1);
			}
			PlayerMgr.Inst.ItemCtrller.CurseAdd(num2, PlayerMgr.Inst.PlayerPoint);
			curseCounter++;
		}
		else
		{
			int relicFromPool = PlayerMgr.Inst.BaData.GetRelicFromPool(ItemDropType.Common);
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Relic, relicFromPool), PlayerMgr.Inst.PlayerPointIgnoreZ);
			relicCounter++;
			if (relicCounter >= relicCount)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
