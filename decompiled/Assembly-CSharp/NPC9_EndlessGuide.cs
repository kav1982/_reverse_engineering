public class NPC9_EndlessGuide : NPCBase
{
	protected override NPCPlot ImportantPlot => DataMgr.selectedWorldData.npc9ImportantPlot;

	protected override NPCPlot SchedulePlot => DataMgr.selectedWorldData.npc9SchedulePlot;

	protected override NPCPlot CasualPlot => DataMgr.selectedWorldData.npc9CasualPlot;

	protected override NPCPlot RandomPlot => DataMgr.selectedWorldData.npc9RandomPlotV2;

	public override void UseNPCFunction()
	{
		GameUISingletonMono<UIEndlessTalent>.ShowInit();
	}
}
