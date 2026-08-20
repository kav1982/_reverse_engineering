using Unity.Entities;
using Unity.Mathematics;

public struct SpecialObj17_Dots : IComponentData, IQueryTypeParameter
{
	public float3 daveOffset;

	public UnityObjectRef<SO17Mono> so17Mono;

	public bool isInitialized;

	public string GetName()
	{
		string text = 1001313.GetText();
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
