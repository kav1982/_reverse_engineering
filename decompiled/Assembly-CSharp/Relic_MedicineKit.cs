using UnityEngine;

public class Relic_MedicineKit : MonoBehaviour
{
	public RelicConfig cfg;

	private Coroutine updatePotionSlotCountCoroutine;

	public void Initialize(RelicConfig relicConfig)
	{
		cfg = relicConfig;
		PlayerMgr.Inst.BaData.potionMaxCount = PlayerMgr.Inst.ItemCtrller.CaculatePotionStorage();
		PlayerMgr.Inst.ItemCtrller.PotionChangeSlotDelay();
	}

	public void DestroySelf()
	{
		PlayerMgr.Inst.ItemCtrller.relic_MedicineKit = null;
		PlayerMgr.Inst.BaData.potionMaxCount = PlayerMgr.Inst.ItemCtrller.CaculatePotionStorage();
		PlayerMgr.Inst.ItemCtrller.PotionChangeSlotDelay();
	}

	public void OnFinishBattle(Vector3 rewardPosition, RoomController room)
	{
		cfg.intTimer += ((LevelMgr.Inst.CurrentRewardType != LevelRewardType.Shortcut) ? 1 : 4);
		while (cfg.intTimer >= cfg.int2.result)
		{
			cfg.intTimer -= cfg.int2.result;
			Vector3 doorToWalkablePoint = room.GetDoorToWalkablePoint(rewardPosition + Random.insideUnitSphere.IgnoreZ());
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Potion, PlayerMgr.Inst.BaData.GetPotionFromPool()), doorToWalkablePoint);
		}
	}
}
