public class NPC6 : NPCBase
{
	protected override NPCPlot ImportantPlot => DataMgr.selectedWorldData.npc6ImportantPlot;

	protected override NPCPlot SchedulePlot => DataMgr.selectedWorldData.npc6SchedulePlot;

	protected override NPCPlot CasualPlot => DataMgr.selectedWorldData.npc6CasualPlot;

	protected override NPCPlot RandomPlot => DataMgr.selectedWorldData.npc6RandomPlotV2;

	public override void UseNPCFunction()
	{
		GameUISingletonMono<UIActivateGirl>.ShowInit();
	}
}
