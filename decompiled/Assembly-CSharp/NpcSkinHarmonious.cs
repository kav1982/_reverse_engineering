using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class NpcSkinHarmonious : MonoBehaviour
{
	public enum Npc
	{
		NPC1,
		NPC2,
		NPC3,
		NPC4,
		NPC5,
		NPC6,
		NPC7,
		NPC8,
		NPC9
	}

	public Npc npcname;

	public SkeletonAnimation sAnima;

	public SkeletonAnimation sAnima_Outline;

	private static Dictionary<Npc, List<string>> dicNpcSkinSelect = new Dictionary<Npc, List<string>>
	{
		[Npc.NPC1] = new List<string> { "skin1", "skin_T1", "skin_T2", "skin_T3", "skin_T3" },
		[Npc.NPC2] = new List<string> { "skin1", "skin_T1", "skin_T2", "skin_T3", "skin_T3" },
		[Npc.NPC3] = new List<string> { "skin1", "skin_T1", "skin_T2", "skin_T3", "skin_T3" },
		[Npc.NPC4] = new List<string> { "skin1", "skin_T1", "skin_T2", "skin_T3", "skin_T3" },
		[Npc.NPC5] = new List<string> { "NPC_5", "skin_T1", "skin_T2", "skin_T3", "skin_T3" },
		[Npc.NPC6] = new List<string> { "NPC_6", "skin_T1", "skin_T2", "skin_T3", "skin_T3" },
		[Npc.NPC7] = new List<string> { "skin1", "skin_T1", "skin_T2", "skin_T3", "skin_T3" },
		[Npc.NPC9] = new List<string> { "skin1", "skin1", "skin1", "skin1", "skin1" }
	};

	private void Start()
	{
		string skinName = dicNpcSkinSelect[npcname][(int)GameMgr.CampSkinType];
		AdjustSkin(skinName);
		Object.Destroy(this);
	}

	public void AdjustSkin(string SkinName)
	{
		if (GameMgr.IsMobile_Static && GameMgr.IsChAge14_Static && sAnima.skeleton.Data.FindSkin(SkinName + "_HX") != null)
		{
			SkinName += "_HX";
		}
		if (sAnima.skeleton.Data.FindSkin(SkinName) != null)
		{
			sAnima.initialSkinName = SkinName;
			sAnima.Initialize(overwrite: true);
			if (sAnima_Outline != null)
			{
				sAnima_Outline.initialSkinName = SkinName;
				sAnima_Outline.Initialize(overwrite: true);
			}
		}
	}
}
