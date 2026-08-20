public class NPC1Vivian : NPCBase
{
	protected override NPCPlot ImportantPlot => DataMgr.selectedWorldData.npc1VivianImportantPlot;

	protected override NPCPlot SchedulePlot => DataMgr.selectedWorldData.npc1VivianSchedulePlot;

	protected override NPCPlot CasualPlot => DataMgr.selectedWorldData.npc1VivianCasualPlot;

	protected override NPCPlot RandomPlot => DataMgr.selectedWorldData.npc1VivianRandomPlotV2;

	public override void UseNPCFunction()
	{
		GameUISingletonMono<UITalent>.ShowInit();
	}

	protected override void OnDialogFinish(int hdId)
	{
		base.OnDialogFinish(hdId);
		if (hdId == 9)
		{
			UseNPCFunction();
		}
	}
}
