using UnityEngine;

public class NPC4 : NPCBase
{
	protected override NPCPlot ImportantPlot => DataMgr.selectedWorldData.npc4ImportantPlot;

	protected override NPCPlot SchedulePlot => DataMgr.selectedWorldData.npc4SchedulePlot;

	protected override NPCPlot CasualPlot => DataMgr.selectedWorldData.npc4CasualPlot;

	protected override NPCPlot RandomPlot => DataMgr.selectedWorldData.npc4RandomPlotV2;

	public override void UseNPCFunction()
	{
		GameUISingletonMono<UISet>.ShowInit();
	}

	protected override void OnDialogFinish(int hdId)
	{
		base.OnDialogFinish(hdId);
		if (hdId == 20)
		{
			DataMgr.selectedWorldData.story3NPC4GiveCloth = true;
			Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIGetNewSuit"), UIMgr.Inst.canvas_1Scaler.transform).GetComponent<UI_GetSuitPopOut>().text.text = 1002104.GetText() + ": " + SetConfig.dic[2].GetName();
		}
	}
}
