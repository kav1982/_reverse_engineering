using Unity.Entities;
using Unity.Mathematics;

public struct SpecialObj46 : IComponentData, IQueryTypeParameter
{
	public Entity ett_Appearance_Chapter1;

	public Entity ett_Appearance_Chapter2;

	public Entity ett_Appearance_Chapter3;

	public Entity ett_Appearance_Chapter4;

	public Entity ett_Appearance_Chapter5;

	public float3 npc8Offset;

	public Entity ett_Sushi;

	public Entity ett_SushiOutline;

	public bool isInitialized;

	public UnityObjectRef<SO46Mono> so46Mono;

	public int forcePlayMusicWaitFrameTimer;

	public bool isShowSusui;

	public string GetName()
	{
		string text = 1001322.GetText();
		bool flag = true;
		for (int i = 0; i < DataMgr.selectedWorldData.researchedIDs.Count; i++)
		{
			if (ResearchConfig.dic[DataMgr.selectedWorldData.researchedIDs[i]].abilityType == ResearchAbilityType.Spring)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					text += "+";
				}
			}
		}
		return text;
	}

	public string GetDesc()
	{
		return 1001314.GetText().Replace("int1", DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Spring).ToString());
	}
}
