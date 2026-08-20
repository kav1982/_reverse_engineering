using System.Collections.Generic;
using System.Linq;
using PlayerLogger;
using PlayerLogger.Events;
using Unity.Entities;
using UnityEngine;

public class SpecialObj102 : MonoBehaviour, IRoomCtrller
{
	[Space(50f)]
	public SpecialObj102Type type;

	public float distance;

	private List<ItemInfo> _infos;

	private List<Entity> items = new List<Entity>();

	private List<GameObject> hoverEffectGO = new List<GameObject>();

	private RoomController levelCtrller;

	private bool createdItem;

	private EntityManager entityMgr;

	private bool canCeateItem;

	private void Start()
	{
		levelCtrller.RoomEnterRegister(CreateItem);
		entityMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void AllowCreateItem()
	{
		canCeateItem = true;
	}

	private void CreateItem()
	{
		if (createdItem)
		{
			return;
		}
		createdItem = true;
		_infos = OutputMgr.GetSO102ItemInfos(type, BattleMgr.Inst.CurrentStage);
		for (int i = 0; i < _infos.Count; i++)
		{
			Vector3 vector = base.transform.position + new Vector3((float)(-(_infos.Count - 1)) * distance / 2f + distance * (float)i, 0f, 0f);
			Entity entity = QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, _infos[i], vector, isStore: true);
			items.Add(entity);
			switch (type)
			{
			case SpecialObj102Type.CurseRelic:
			{
				Item componentData = entityMgr.GetComponentData<Item>(entity);
				componentData.SetCurse(PlayerMgr.Inst.BaData.GetCurseFromPool(ItemDropType.Rare));
				entityMgr.SetComponentData(entity, componentData);
				hoverEffectGO.Add(ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_ItemCurse", vector));
				break;
			}
			case SpecialObj102Type.BloodRelic:
				if (!GameMgr.IsHarmony_Static)
				{
					hoverEffectGO.Add(ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_ItemBlood", vector));
				}
				else
				{
					hoverEffectGO.Add(null);
				}
				break;
			default:
				Debug.LogError(type);
				break;
			}
		}
		if (type != SpecialObj102Type.BloodRelic)
		{
			return;
		}
		RoomFinishLogger.SideRoomInfo sideRoomInfo = LevelMgr.Inst.RoomFinishLogger?.side_room.FirstOrDefault((RoomFinishLogger.SideRoomInfo e) => e.type == RoomType.BloodRelic);
		if (!(sideRoomInfo == null))
		{
			sideRoomInfo.reward.AddRange(_infos.Select((ItemInfo e) => new PlayerLogger.Item
			{
				id = e.id,
				type = PlayerLogger.Item.Type.Relic,
				number = 1
			}));
		}
	}

	private void Update()
	{
		if (!createdItem)
		{
			if (canCeateItem)
			{
				CreateItem();
			}
			return;
		}
		for (int num = items.Count - 1; num >= 0; num--)
		{
			if (!entityMgr.Exists(items[num]) && hoverEffectGO[num] != null && hoverEffectGO[num].activeSelf)
			{
				hoverEffectGO[num].SetActive(value: false);
				switch (type)
				{
				case SpecialObj102Type.CurseRelic:
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_ItemCursePickup", base.transform.position + new Vector3((float)(-(items.Count - 1)) * distance / 2f + distance * (float)num, 0f, 0f), 2f);
					break;
				case SpecialObj102Type.BloodRelic:
					if (!GameMgr.IsHarmony_Static)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_ItemBloodPickup", base.transform.position + new Vector3((float)(-(items.Count - 1)) * distance / 2f + distance * (float)num, 0f, 0f), 2f);
					}
					break;
				default:
					Debug.LogError(type);
					break;
				}
			}
		}
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		this.levelCtrller = levelCtrller;
	}
}
