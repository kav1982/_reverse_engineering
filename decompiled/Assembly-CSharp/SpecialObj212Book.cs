using PlayerLogger;
using Unity.Entities;
using UnityEngine;

public class SpecialObj212Book : InteractiveObj
{
	[Space(50f)]
	public GameObject go_HighLight;

	public Animator anima;

	public AnimaEvent animaEvent;

	public BoxCollider thisCollider;

	[Header("Reward")]
	public GameObject go_RewardShow;

	public SpriteRenderer sr_SpellIcon;

	public GameObject go_SpellStar;

	public GameObject go_SpellStar2;

	public GameObject go_RewardClose;

	[Header("Curse")]
	public GameObject go_CurseShow;

	public GameObject go_CurseClose;

	[Header("Hover")]
	public Transform tsf_Model;

	public float hoverSpeed;

	public float hoverHeigh;

	private SO212BookType bookType;

	private RoomController belongCtrller;

	private int id;

	private bool isOpend;

	public Entity thisEntity;

	private void Update()
	{
		tsf_Model.transform.localPosition = new Vector3(0f, Mathf.Sin(Time.timeSinceLevelLoad * hoverSpeed) * hoverHeigh, 0f);
	}

	private void AnimaAction(string animaName)
	{
		if (animaName == "Open")
		{
			if (bookType == SO212BookType.Reward)
			{
				ItemInfo itemInfo = default(ItemInfo);
				itemInfo.type = ItemType.Spell;
				itemInfo.id = id;
				ItemInfo info = itemInfo;
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, info, base.transform.position);
				LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(id));
				SEMgr.Inst.puzzleSucceed.PlaySE();
			}
			else
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_ItemCursePickup", base.transform.position);
				PlayerMgr.Inst.ItemCtrller.CurseAdd(id, base.transform.position);
				LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(new PlayerLogger.Item
				{
					type = PlayerLogger.Item.Type.Curse,
					number = 1,
					id = id
				});
				id = 0;
				SEMgr.Inst.puzzleFail.PlaySE();
			}
		}
	}

	private void CurseRecycle()
	{
		if (id != 0)
		{
			PlayerMgr.Inst.BaData.BackCurseToPool(id, 1);
		}
	}

	private void Start()
	{
		thisEntity = RegisterDotsInteractiveObj(thisCollider, InteractiveObjType.SpecialObj212Book);
	}

	public void Initialize(SO212BookType bookType, RoomController roomCtrller)
	{
		this.bookType = bookType;
		belongCtrller = roomCtrller;
		animaEvent.DoAction = AnimaAction;
		if (bookType == SO212BookType.Reward)
		{
			id = OutputMgr.GetSpecialRoomSpell();
			sr_SpellIcon.sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[id].GetIconPath());
			if (SpellConfig.dic[id].level > 1)
			{
				go_SpellStar.SetActive(value: true);
			}
			else if (SpellConfig.dic[id].level > 2)
			{
				go_SpellStar2.SetActive(value: true);
			}
			Object.Destroy(go_CurseShow);
			Object.Destroy(go_CurseClose);
		}
		else
		{
			id = PlayerMgr.Inst.BaData.GetCurseFromPool(ItemDropType.Common);
			roomCtrller.RoomRecycleRegister(CurseRecycle);
			Object.Destroy(go_RewardShow);
			Object.Destroy(go_RewardClose);
		}
	}

	public void ShowReward()
	{
		anima.SetTrigger("ShowReward");
	}

	public override void Select()
	{
		if (!isOpend)
		{
			go_HighLight.gameObject.SetActive(value: true);
		}
	}

	public override void Unselect()
	{
		go_HighLight.gameObject.SetActive(value: false);
	}

	public override void Interact()
	{
		if (!isOpend)
		{
			isOpend = true;
			anima.SetTrigger("Open");
			SetDotsObjLayer(thisEntity, isOpen: false);
		}
	}
}
