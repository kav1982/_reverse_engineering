using UnityEngine;

public class Access_T0 : AccessBase
{
	[Space(50f)]
	public GameObject go_Highlight;

	public Animator anima;

	public AnimaEvent animaEvent;

	public BoxCollider bc;

	public TriggerIn triggerIn;

	public float triggerOffset;

	public float modelOffset;

	public GameObject go_WallCollider;

	[Header("NotNeedKey")]
	public SpriteRenderer sr;

	public Sprite sprite_NoKey;

	[Header("Light")]
	public GameObject go_LightR;

	public GameObject go_LightL;

	public GameObject pfb_Light_Right;

	public GameObject pfb_HideBoundaryLight_Right;

	private void TriggerEnter(Collider other)
	{
		if (other.tag == "Player" && other.isTrigger)
		{
			LevelMgr.Inst.PlayerEnterAccess(base.Dir);
		}
		else if (other.tag == "Item")
		{
			switch (base.Dir)
			{
			case FourDir.Up:
				other.transform.position = base.transform.position + new Vector3(0f, -1f);
				break;
			case FourDir.Right:
				other.transform.position = base.transform.position + new Vector3(-1f, 0f);
				break;
			case FourDir.Down:
				other.transform.position = base.transform.position + new Vector3(0f, 1f);
				break;
			case FourDir.Left:
				other.transform.position = base.transform.position + new Vector3(1f, 0f);
				break;
			default:
				Debug.LogError(base.Dir);
				break;
			}
		}
	}

	private void AnimaAction(string animaName)
	{
		if (!(animaName == "OpenFinish"))
		{
			if (!(animaName == "CloseFinish"))
			{
				Debug.LogError(animaName);
			}
			return;
		}
		bc.enabled = false;
		go_WallCollider.SetActive(value: false);
		tsf_Layer.position = Tool2D.IgnoreZPoint(base.transform, 1.23f);
		if (LevelMgr.Inst.CurrentRoomCtrller == base.BelongRoom)
		{
			triggerIn.gameObject.SetActive(value: true);
		}
	}

	public override void Initialize(RoomController roomCtrller, AccessType type, FourDir dir)
	{
		base.Initialize(roomCtrller, type, dir);
		triggerIn.Initialize(TriggerEnter);
		animaEvent.DoAction = AnimaAction;
		switch (base.AccessType)
		{
		case AccessType.Normal:
			base.IsConditionComplete = true;
			if (dir == FourDir.Left || dir == FourDir.Right)
			{
				sr.sprite = sprite_NoKey;
			}
			break;
		case AccessType.Key:
			base.IsConditionComplete = false;
			break;
		default:
			Debug.LogError(base.AccessType);
			break;
		}
		switch (dir)
		{
		case FourDir.Up:
			triggerIn.transform.localPosition = new Vector3(0f, triggerOffset);
			break;
		case FourDir.Right:
			triggerIn.transform.localPosition = new Vector3(triggerOffset, 0f);
			anima.transform.position += new Vector3(modelOffset, 0f, 0f);
			if (roomCtrller.roomCfg.type != RoomType.Boss && roomCtrller.roomCfg.type != RoomType.BloodRelic)
			{
				go_LightR.SetActive(value: true);
			}
			if (roomCtrller.roomCfg.type != RoomType.Boss && roomCtrller.roomCfg.themeType != RoomThemeType.Theme1_Chapter2_Cliff && roomCtrller.roomCfg.themeType != RoomThemeType.Theme19_Chapter2_Shortcut2 && roomCtrller.roomCfg.type != RoomType.BloodRelic)
			{
				Object.Instantiate(pfb_Light_Right, base.transform.position + new Vector3(0f, -1f, 0f), Quaternion.identity, base.transform.parent);
				Object.Instantiate(pfb_Light_Right, base.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity, base.transform.parent);
			}
			else if (roomCtrller.roomCfg.type == RoomType.BloodRelic)
			{
				Object.Instantiate(pfb_HideBoundaryLight_Right, base.transform.position + new Vector3(0f, -1f, 0f), Quaternion.identity, base.transform.parent);
				Object.Instantiate(pfb_HideBoundaryLight_Right, base.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity, base.transform.parent);
			}
			break;
		case FourDir.Down:
			triggerIn.transform.localPosition = new Vector3(0f, 0f - triggerOffset);
			break;
		case FourDir.Left:
			triggerIn.transform.localPosition = new Vector3(0f - triggerOffset, 0f);
			anima.transform.position += new Vector3(0f - modelOffset, 0f, 0f);
			if (roomCtrller.roomCfg.type != RoomType.Boss && roomCtrller.roomCfg.type != RoomType.BloodRelic)
			{
				go_LightL.SetActive(value: true);
			}
			if (roomCtrller.roomCfg.type != RoomType.Boss && roomCtrller.roomCfg.themeType != RoomThemeType.Theme1_Chapter2_Cliff && roomCtrller.roomCfg.themeType != RoomThemeType.Theme19_Chapter2_Shortcut2 && roomCtrller.roomCfg.type != RoomType.BloodRelic)
			{
				Object.Instantiate(pfb_Light_Right, base.transform.position + new Vector3(0f, -1f, 0f), Quaternion.identity, base.transform.parent).transform.localScale = new Vector3(-1f, 1f, 1f);
				Object.Instantiate(pfb_Light_Right, base.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity, base.transform.parent).transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else if (roomCtrller.roomCfg.type == RoomType.BloodRelic)
			{
				Object.Instantiate(pfb_HideBoundaryLight_Right, base.transform.position + new Vector3(0f, -1f, 0f), Quaternion.identity, base.transform.parent).transform.localScale = new Vector3(-1f, 1f, 1f);
				Object.Instantiate(pfb_HideBoundaryLight_Right, base.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity, base.transform.parent).transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			break;
		default:
			Debug.LogError(dir);
			break;
		}
	}

	public override void Open()
	{
		if (base.Dir == FourDir.Down)
		{
			return;
		}
		base.gameObject.tag = "Untagged";
		if (base.AccessType == AccessType.Key)
		{
			if (base.IsConditionComplete)
			{
				anima.SetTrigger("Open");
				triggerIn.gameObject.SetActive(value: false);
				AccessOpenSE();
			}
			else
			{
				base.gameObject.tag = "InteractiveObj";
			}
		}
		else
		{
			anima.SetTrigger("Open");
			triggerIn.gameObject.SetActive(value: false);
			AccessOpenSE();
		}
	}

	public override void OpenDirect()
	{
		if (base.Dir == FourDir.Down)
		{
			return;
		}
		base.gameObject.tag = "InteractiveObj";
		if (base.AccessType == AccessType.Key)
		{
			if (base.IsConditionComplete)
			{
				anima.SetTrigger("OpenDirect");
				bc.enabled = false;
				go_WallCollider.SetActive(value: false);
				tsf_Layer.position = Tool2D.IgnoreZPoint(base.transform, 1.23f);
				if (LevelMgr.Inst.CurrentRoomCtrller == base.BelongRoom)
				{
					triggerIn.gameObject.SetActive(value: true);
				}
			}
		}
		else
		{
			anima.SetTrigger("OpenDirect");
			bc.enabled = false;
			go_WallCollider.SetActive(value: false);
			tsf_Layer.position = Tool2D.IgnoreZPoint(base.transform, 1.23f);
			if (LevelMgr.Inst.CurrentRoomCtrller == base.BelongRoom)
			{
				triggerIn.gameObject.SetActive(value: true);
			}
		}
	}

	public override void Close()
	{
		if (base.BelongRoom.roomCfg.type != RoomType.Boss || (base.Dir != FourDir.Left && base.Dir != FourDir.Right))
		{
			base.gameObject.tag = "Untagged";
			go_WallCollider.SetActive(value: true);
			anima.SetTrigger("Close");
			triggerIn.gameObject.SetActive(value: false);
			tsf_Layer.position = Tool2D.GetLayerPoint(base.transform);
			bc.enabled = true;
			AccessOpenSE();
		}
	}

	public override void CloseDirect()
	{
		if (base.BelongRoom.roomCfg.type != RoomType.Boss || (base.Dir != FourDir.Left && base.Dir != FourDir.Right))
		{
			base.gameObject.tag = "Untagged";
			go_WallCollider.SetActive(value: true);
			anima.SetTrigger("CloseDirect");
			triggerIn.gameObject.SetActive(value: false);
			tsf_Layer.position = Tool2D.GetLayerPoint(base.transform);
			bc.enabled = true;
		}
	}

	public override void ConditionComplete()
	{
		base.ConditionComplete();
		if (base.gameObject.tag == "InteractiveObj")
		{
			Open();
		}
	}

	public override void Select()
	{
		go_Highlight.SetActive(value: true);
	}

	public override void Unselect()
	{
		go_Highlight.SetActive(value: false);
	}

	public override void Interact()
	{
		switch (base.AccessType)
		{
		case AccessType.Key:
			if (PlayerMgr.Inst.IsKeyEnough())
			{
				PlayerMgr.Inst.ChangeKey(-PlayerMgr.Inst.NeedKeyCount(), TextFloatQueueType.DirectFloat);
				ConditionComplete();
			}
			break;
		default:
			Debug.LogError(base.AccessType);
			break;
		case AccessType.Normal:
			break;
		}
	}
}
