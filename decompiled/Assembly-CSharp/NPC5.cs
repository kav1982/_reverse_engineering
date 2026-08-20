public class NPC5 : NPCBase
{
	protected override NPCPlot ImportantPlot => DataMgr.selectedWorldData.npc5ImportantPlot;

	protected override NPCPlot SchedulePlot => DataMgr.selectedWorldData.npc5SchedulePlot;

	protected override NPCPlot CasualPlot => DataMgr.selectedWorldData.npc5CasualPlot;

	protected override NPCPlot RandomPlot => DataMgr.selectedWorldData.npc5RandomPlotV2;

	public override void UseNPCFunction()
	{
		GameUISingletonMono<UITraining>.ShowInit();
	}
}
