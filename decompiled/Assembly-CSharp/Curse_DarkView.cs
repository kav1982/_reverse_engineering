using UnityEngine;

public class Curse_DarkView : MonoBehaviour
{
	private void Start()
	{
		LevelMgr.Inst.ChangeGlobalLightColor(LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType);
	}

	private void Update()
	{
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}

	public void OnDestroy()
	{
		PlayerMgr.Inst.ItemCtrller.curse_DarkView = null;
		LevelMgr.Inst.ChangeGlobalLightColor(LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType);
	}
}
