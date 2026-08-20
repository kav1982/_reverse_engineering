using Spine;
using UnityEngine;

public class Relic_MirrorOfSoul : MonoBehaviour
{
	private RelicConfig relicConfig;

	private bool needUpdateWandCount;

	private Slot slotUpperLegL;

	private Slot slotUpperLegR;

	private Slot slotLowerLegL;

	private Slot slotLowerLegR;

	private Slot slotShoesL;

	private Slot slotShoesR;

	private Slot slotShoesL2;

	private Slot slotShoesR2;

	private Slot slotHandL;

	private Slot slotHandR;

	private void Start()
	{
		PlayerMgr.Inst.FlyRegister();
	}

	private void Update()
	{
		if (slotHandR.A != 0f)
		{
			slotUpperLegL.A = 0f;
			slotUpperLegR.A = 0f;
			slotLowerLegL.A = 0f;
			slotLowerLegR.A = 0f;
			slotShoesL.A = 0f;
			slotShoesR.A = 0f;
			slotShoesL2.A = 0f;
			slotShoesR2.A = 0f;
			slotHandL.A = 0f;
			slotHandR.A = 0f;
		}
		if (needUpdateWandCount)
		{
			UpdateWandCount(relicConfig.int1.result);
			needUpdateWandCount = false;
		}
	}

	public void Initialize(RelicConfig config)
	{
		if (slotUpperLegL == null)
		{
			slotUpperLegL = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("tui_l");
			slotUpperLegR = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("tui_r");
			slotLowerLegL = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("xiaotui_l");
			slotLowerLegR = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("xiaotui_r");
			slotShoesL = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("xie_l");
			slotShoesR = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("xie_r");
			slotShoesL2 = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("bilibili_xie_l");
			slotShoesR2 = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("bilibili_xie_r");
			slotHandL = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("Hand_L");
			slotHandR = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("Hand_R");
		}
		relicConfig = config;
		needUpdateWandCount = true;
	}

	public static void UpdateWandCount(int extraWandCount)
	{
		int num = 1;
		if (DataMgr.selectedWorldData.levelOfWandLimit > 0)
		{
			num += ScriptableObjMgr.Inst.talentUpgrade2.wandLimit[DataMgr.selectedWorldData.levelOfWandLimit - 1].value;
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_LessWandMoreSlot != null)
		{
			num -= PlayerMgr.Inst.ItemCtrller.relicCfg_LessWandMoreSlot.int1.result;
		}
		int num2 = num + extraWandCount - PlayerMgr.Inst.BaData.wandMaxCount;
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				PlayerMgr.Inst.AddExtraWand(WandConfig.GetConfig(51), fullMp: true);
			}
		}
		else
		{
			if (num2 >= 0)
			{
				return;
			}
			for (int j = 0; j < -num2; j++)
			{
				int num3 = PlayerMgr.Inst.BaData.wandMaxCount - 1;
				if (PlayerMgr.Inst.BaData.wandCfgs[num3] != null)
				{
					PlayerMgr.Inst.DropWand(num3, BattleMgr.Inst);
				}
				PlayerMgr.Inst.BaData.wandMaxCount--;
				PlayerMgr.Inst.BaData.wandCfgs.RemoveAt(PlayerMgr.Inst.BaData.wandCfgs.Count - 1);
				PlayerMgr.Inst.Wands.RemoveAt(PlayerMgr.Inst.Wands.Count - 1);
				UIPlayerDataMgr.Inst.WandReset();
			}
		}
	}

	public void DestroySelf()
	{
		UpdateWandCount(0);
		PlayerMgr.Inst.FlyUnregister();
		slotUpperLegL.A = 1f;
		slotUpperLegR.A = 1f;
		slotLowerLegL.A = 1f;
		slotLowerLegR.A = 1f;
		slotShoesL.A = 1f;
		slotShoesR.A = 1f;
		slotShoesL2.A = 1f;
		slotShoesR2.A = 1f;
		slotHandL.A = 1f;
		slotHandR.A = 1f;
		Object.Destroy(base.gameObject);
	}
}
