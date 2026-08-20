using UnityEngine;

public class SpecialObj311EndlessNpc7 : NPCBase
{
	protected override NPCPlot ImportantPlot => new NPCPlot(0);

	protected override NPCPlot SchedulePlot => new NPCPlot(0);

	protected override NPCPlot CasualPlot => DataMgr.selectedWorldData.npc7CasualPlot;

	protected override NPCPlot RandomPlot => DataMgr.selectedWorldData.npc7RandomPlotV2;

	public void Initialize()
	{
		if (!ScriptableObjMgr.Inst.testCtrller.UnlockAllNPC && (!DataMgr.selectedWorldData.finishedDifficulty.Contains(DifficultyType.Normal) || !DataMgr.selectedWorldData.storyHardFinishNPC7Appearance))
		{
			Hide();
		}
	}

	public override void UseNPCFunction()
	{
		GameUISingletonMono<UISpellDisable>.ShowInit();
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
