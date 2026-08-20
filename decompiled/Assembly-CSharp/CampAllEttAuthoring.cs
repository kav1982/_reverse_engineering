using Unity.Entities;
using UnityEngine;

public class CampAllEttAuthoring : MonoBehaviour
{
	private class Baker : Baker<CampAllEttAuthoring>
	{
		public override void Bake(CampAllEttAuthoring authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			DynamicBuffer<CampSkinBED> dynamicBuffer = AddBuffer<CampSkinBED>(entity);
			for (int i = 0; i < authoring.AllCampThemeGo.Length; i++)
			{
				CampEttSO campEttSO = authoring.AllCampThemeGo[i];
				CampSkinBED elem = default(CampSkinBED);
				elem.ett_AccessL = TryGetEntity(campEttSO.ett_AccessL);
				elem.positionOverride_AccessL = campEttSO.positionOverride_AccessL;
				elem.ett_AccessR = TryGetEntity(campEttSO.ett_AccessR);
				elem.positionOverride_AccessR = campEttSO.positionOverride_AccessR;
				elem.ett_AccessR_StoneRoad = TryGetEntity(campEttSO.ett_AccessR_StoneRoad);
				elem.positionOverride_AccessR_StoneRoad = campEttSO.positionOverride_AccessR_StoneRoad;
				elem.ett_Camp = TryGetEntity(campEttSO.ett_Camp);
				elem.positionOverride_Camp = campEttSO.positionOverride_Camp;
				elem.ett_Decoration_CampStage0 = TryGetEntity(campEttSO.ett_Decoration_CampStage0);
				elem.positionOverride_Decoration_CampStage0 = campEttSO.positionOverride_Decoration_CampStage0;
				elem.ett_Decoration_CampStage1 = TryGetEntity(campEttSO.ett_Decoration_CampStage1);
				elem.positionOverride_Decoration_CampStage1 = campEttSO.positionOverride_Decoration_CampStage1;
				elem.ett_Decoration_CampStage2 = TryGetEntity(campEttSO.ett_Decoration_CampStage2);
				elem.positionOverride_Decoration_CampStage2 = campEttSO.positionOverride_Decoration_CampStage2;
				elem.ett_Decoration_CampStage3 = TryGetEntity(campEttSO.ett_Decoration_CampStage3);
				elem.positionOverride_Decoration_CampStage3 = campEttSO.positionOverride_Decoration_CampStage3;
				elem.ett_Decoration_NPC4Stage1 = TryGetEntity(campEttSO.ett_Decoration_NPC4Stage1);
				elem.positionOverride_Decoration_NPC4Stage1 = campEttSO.positionOverride_Decoration_NPC4Stage1;
				elem.ett_Decoration_NPC4Stage2 = TryGetEntity(campEttSO.ett_Decoration_NPC4Stage2);
				elem.positionOverride_Decoration_NPC4Stage2 = campEttSO.positionOverride_Decoration_NPC4Stage2;
				elem.ett_Decoration_NPC4Stage3 = TryGetEntity(campEttSO.ett_Decoration_NPC4Stage3);
				elem.positionOverride_Decoration_NPC4Stage3 = campEttSO.positionOverride_Decoration_NPC4Stage3;
				elem.ett_Decoration_Npc6GroundStage1 = TryGetEntity(campEttSO.ett_Decoration_Npc6GroundStage1);
				elem.positionOverride_Decoration_Npc6GroundStage1 = campEttSO.positionOverride_Decoration_Npc6GroundStage1;
				elem.ett_Decoration_Npc6GroundStage2 = TryGetEntity(campEttSO.ett_Decoration_Npc6GroundStage2);
				elem.positionOverride_Decoration_Npc6GroundStage2 = campEttSO.positionOverride_Decoration_Npc6GroundStage2;
				elem.ett_Decoration_Npc6GroundStage3 = TryGetEntity(campEttSO.ett_Decoration_Npc6GroundStage3);
				elem.positionOverride_Decoration_Npc6GroundStage3 = campEttSO.positionOverride_Decoration_Npc6GroundStage3;
				elem.ett_Decoration_Npc6TentacleStage1 = TryGetEntity(campEttSO.ett_Decoration_Npc6TentacleStage1);
				elem.positionOverride_Decoration_Npc6TentacleStage1 = campEttSO.positionOverride_Decoration_Npc6TentacleStage1;
				elem.ett_Decoration_Npc6TentacleStage2 = TryGetEntity(campEttSO.ett_Decoration_Npc6TentacleStage2);
				elem.positionOverride_Decoration_Npc6TentacleStage2 = campEttSO.positionOverride_Decoration_Npc6TentacleStage2;
				elem.ett_Decoration_Npc6TentacleStage3 = TryGetEntity(campEttSO.ett_Decoration_Npc6TentacleStage3);
				elem.positionOverride_Decoration_Npc6TentacleStage3 = campEttSO.positionOverride_Decoration_Npc6TentacleStage3;
				elem.ett_Decoration_Tent1 = TryGetEntity(campEttSO.ett_Decoration_Tent1);
				elem.positionOverride_Decoration_Tent1 = campEttSO.positionOverride_Decoration_Tent1;
				elem.ett_Decoration_Tent2 = TryGetEntity(campEttSO.ett_Decoration_Tent2);
				elem.positionOverride_Decoration_Tent2 = campEttSO.positionOverride_Decoration_Tent2;
				elem.ett_Decoration_WoodStage2 = TryGetEntity(campEttSO.ett_Decoration_WoodStage2);
				elem.positionOverride_Decoration_WoodStage2 = campEttSO.positionOverride_Decoration_WoodStage2;
				elem.ett_Decoration_WoodStage3 = TryGetEntity(campEttSO.ett_Decoration_WoodStage3);
				elem.positionOverride_Decoration_WoodStage3 = campEttSO.positionOverride_Decoration_WoodStage3;
				elem.ett_Leaf_2 = TryGetEntity(campEttSO.ett_Leaf_2);
				elem.positionOverride_Leaf_2 = campEttSO.positionOverride_Leaf_2;
				elem.ett_ResearchTable = TryGetEntity(campEttSO.ett_ResearchTable);
				elem.positionOverride_ResearchTable = campEttSO.positionOverride_ResearchTable;
				elem.ett_TrainingRoom = TryGetEntity(campEttSO.ett_TrainingRoom);
				elem.positionOverride_TrainingRoom = campEttSO.positionOverride_TrainingRoom;
				elem.ett_WallR_Access_Training = TryGetEntity(campEttSO.ett_WallR_Access_Training);
				elem.positionOverride_WallR_Access_Training = campEttSO.positionOverride_WallR_Access_Training;
				elem.ett_灯柱 = TryGetEntity(campEttSO.ett_灯柱);
				elem.positionOverride_灯柱 = campEttSO.positionOverride_灯柱;
				elem.ett_训练房木桩 = TryGetEntity(campEttSO.ett_训练房木桩);
				elem.positionOverride_训练房木桩 = campEttSO.positionOverride_训练房木桩;
				elem.ett_Billboard = TryGetEntity(campEttSO.ett_Billboard);
				elem.positionOverride_Billboard = campEttSO.positionOverride_Billboard;
				elem.ett_CampMirror = TryGetEntity(campEttSO.ett_CampMirror);
				elem.positionOverride_CampMirror = campEttSO.positionOverride_CampMirror;
				elem.ett_CampMirrorStand = TryGetEntity(campEttSO.ett_CampMirrorStand);
				elem.positionOverride_CampMirrorStand = campEttSO.positionOverride_CampMirrorStand;
				elem.ett_CampSkinChanger = TryGetEntity(campEttSO.ett_CampSkinChanger);
				elem.positionOverride_CampSkinChanger = campEttSO.positionOverride_CampSkinChanger;
				elem.ett_DoorCamp = TryGetEntity(campEttSO.ett_DoorCamp);
				elem.positionOverride_DoorCamp = campEttSO.positionOverride_DoorCamp;
				elem.ett_Gallery = TryGetEntity(campEttSO.ett_Gallery);
				elem.positionOverride_Gallery = campEttSO.positionOverride_Gallery;
				elem.ett_GiftSet = TryGetEntity(campEttSO.ett_GiftSet);
				elem.positionOverride_GiftSet = campEttSO.positionOverride_GiftSet;
				elem.ett_RankingList = TryGetEntity(campEttSO.ett_RankingList);
				elem.positionOverride_RankingList = campEttSO.positionOverride_RankingList;
				elem.ett_ResourceChanger = TryGetEntity(campEttSO.ett_ResourceChanger);
				elem.positionOverride_ResourceChanger = campEttSO.positionOverride_ResourceChanger;
				elem.ett_ResourceChanger = TryGetEntity(campEttSO.ett_ResourceChanger);
				elem.positionOverride_ResourceChanger = campEttSO.positionOverride_ResourceChanger;
				elem.ett_DownWallClose = TryGetEntity(campEttSO.ett_DownWallClose);
				elem.positionOverride_DownWallClose = campEttSO.positionOverride_下方阻挡;
				elem.ett_DownWallOpen = TryGetEntity(campEttSO.ett_DownWallOpen);
				elem.positionOverride_DownWallOpen = campEttSO.positionOverride_DownWallOpen;
				elem.ett_AccessD = TryGetEntity(campEttSO.ett_AccessD);
				elem.positionOverride_AccessD = campEttSO.positionOverride_AccessD;
				dynamicBuffer.Add(elem);
			}
			CampAllEtt component = new CampAllEtt
			{
				ett_EndlessCamp = GetEntity(authoring.ett_EndlessCamp, TransformUsageFlags.Dynamic),
				ett_EndlessGate = GetEntity(authoring.ett_EndlessGate, TransformUsageFlags.Dynamic),
				ett_EndlessGallery = GetEntity(authoring.ett_EndlessGallery, TransformUsageFlags.Dynamic),
				ett_EndlessRankingList = GetEntity(authoring.ett_EndlessRankingList, TransformUsageFlags.Dynamic)
			};
			AddComponent(entity, in component);
			Entity TryGetEntity(GameObject go)
			{
				if (!(go == null))
				{
					return GetEntity(go, TransformUsageFlags.Dynamic);
				}
				return Entity.Null;
			}
		}
	}

	public CampEttSO[] AllCampThemeGo;

	public GameObject ett_EndlessCamp;

	public GameObject ett_EndlessGate;

	public GameObject ett_EndlessGallery;

	public GameObject ett_EndlessRankingList;
}
