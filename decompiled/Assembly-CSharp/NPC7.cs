using Unity.Entities;
using UnityEngine;

public class NPC7 : NPCBase
{
	[Header("Hole")]
	public GameObject go_Hole;

	public GameObject go_HoleSmall;

	public GameObject go_HoleBig;

	protected override NPCPlot ImportantPlot => DataMgr.selectedWorldData.npc7ImportantPlot;

	protected override NPCPlot SchedulePlot => DataMgr.selectedWorldData.npc7SchedulePlot;

	protected override NPCPlot CasualPlot => DataMgr.selectedWorldData.npc7CasualPlot;

	protected override NPCPlot RandomPlot => DataMgr.selectedWorldData.npc7RandomPlotV2;

	public void Initialize()
	{
		if (ScriptableObjMgr.Inst.testCtrller.UnlockAllNPC)
		{
			ChangeHoleBig();
		}
		else if (!DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Normal) || !DataMgr.selectedWorldData.storyHardFinishNPC7Appearance)
		{
			Hide();
		}
		else if (DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Normal) && DataMgr.selectedWorldData.storyHardFinishNPC7Appearance && !DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction)
		{
			Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Storys/Story_NPC7OpenFunction"));
		}
		else
		{
			ChangeHoleBig();
		}
	}

	public void EnterDoorDestroy()
	{
		if (belongEtt != Entity.Null)
		{
			World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(belongEtt);
		}
		Object.Destroy(go_Hole);
		Object.Destroy(base.gameObject);
	}

	public void ChangeHoleBig()
	{
		go_HoleSmall.SetActive(value: false);
		go_HoleBig.SetActive(value: true);
	}

	public void MonsterAIPause()
	{
		for (int i = 0; i < LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count; i++)
		{
			Entity entity = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList[i];
			if (UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(entity).unitCfg.id / 100 == 1008)
			{
				Monster8 monster = UnitDotsSyncSystem.GetComponentObject<UnitPptReference>(entity).unitPpt.UnitBas as Monster8;
				if (monster != null)
				{
					monster.NPC7Pause();
				}
			}
		}
	}

	public void MonsterAIRecovery()
	{
		for (int i = 0; i < LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count; i++)
		{
			Entity entity = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList[i];
			if (UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(entity).unitCfg.id / 100 == 1008)
			{
				Monster8 monster = UnitDotsSyncSystem.GetComponentObject<UnitPptReference>(entity).unitPpt.UnitBas as Monster8;
				if (monster != null)
				{
					monster.NPC7PuaseRecovery();
				}
			}
		}
	}

	public override void UseNPCFunction()
	{
		GameUISingletonMono<UISpellDisable>.ShowInit();
	}

	protected override void OnDialogFinish(int hdId)
	{
		base.OnDialogFinish(hdId);
		if (hdId == 59)
		{
			DataMgr.selectedWorldData.npc7SchedulePlot.SetNewState(66);
			GameUISingletonMono<UISpellDisable>.ShowInit();
		}
	}

	public void OnDestroy()
	{
		Object.Destroy(go_HDBubble);
		if (GameUISingletonMono<UISpellDisable>.Inited)
		{
			GameUISingletonMono<UISpellDisable>.DestroyUI();
		}
	}
}
