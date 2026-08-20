using UnityEngine;

public class Access_T8 : AccessBase
{
	[Space(50f)]
	public FourDir dir;

	public GameObject go_Highlight;

	public Animator anima;

	public BoxCollider bc_Slef;

	public TriggerIn triggerIn;

	public SpriteRenderer sr_Fog;

	public GameObject go_WallCollider;

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
				other.transform.position = base.transform.position + new Vector3(0f, -1.5f);
				break;
			case FourDir.Right:
				other.transform.position = base.transform.position + new Vector3(-1.5f, 0f);
				break;
			case FourDir.Down:
				other.transform.position = base.transform.position + new Vector3(0f, 1.5f);
				break;
			case FourDir.Left:
				other.transform.position = base.transform.position + new Vector3(1.5f, 0f);
				break;
			default:
				Debug.LogError(base.Dir);
				break;
			}
		}
	}

	public void InitializeT8(RoomController roomCtrller, AccessType type, Tile_T8 tile_T8)
	{
		Initialize(roomCtrller, type, dir);
		triggerIn.Initialize(TriggerEnter);
		switch (base.AccessType)
		{
		case AccessType.Normal:
			base.IsConditionComplete = true;
			break;
		case AccessType.Key:
			base.IsConditionComplete = false;
			break;
		default:
			Debug.LogError(base.AccessType);
			break;
		}
	}

	public override void Open()
	{
		if (base.Dir == FourDir.Down || !base.gameObject.activeSelf)
		{
			return;
		}
		base.gameObject.tag = "Wall";
		if (base.AccessType == AccessType.Key)
		{
			if (base.IsConditionComplete)
			{
				anima.Play("Open");
				triggerIn.gameObject.SetActive(value: false);
				bc_Slef.enabled = false;
				AccessOpenSE();
			}
			else
			{
				base.gameObject.tag = "InteractiveObj";
			}
		}
		else
		{
			anima.Play("Open");
			triggerIn.gameObject.SetActive(value: false);
			bc_Slef.enabled = false;
			AccessOpenSE();
		}
	}

	public override void OpenDirect()
	{
		if (base.Dir == FourDir.Down || !base.gameObject.activeSelf)
		{
			return;
		}
		base.gameObject.tag = "InteractiveObj";
		if (base.AccessType == AccessType.Key)
		{
			if (base.IsConditionComplete)
			{
				anima.Play("OpenDirect");
				go_WallCollider.SetActive(value: false);
				bc_Slef.enabled = false;
				tsf_Layer.position = Tool2D.IgnoreZPoint(base.transform, 1.23f);
				if (LevelMgr.Inst.CurrentRoomCtrller == base.BelongRoom)
				{
					triggerIn.gameObject.SetActive(value: true);
				}
			}
		}
		else
		{
			anima.Play("OpenDirect");
			go_WallCollider.SetActive(value: false);
			bc_Slef.enabled = false;
			tsf_Layer.position = Tool2D.IgnoreZPoint(base.transform, 1.23f);
			if (LevelMgr.Inst.CurrentRoomCtrller == base.BelongRoom)
			{
				triggerIn.gameObject.SetActive(value: true);
			}
		}
	}

	public override void Close()
	{
		if (base.gameObject.activeSelf && (base.BelongRoom.roomCfg.type != RoomType.Boss || (base.Dir != FourDir.Left && base.Dir != FourDir.Right)))
		{
			base.gameObject.tag = "Wall";
			anima.Play("Close");
			go_WallCollider.SetActive(value: true);
			bc_Slef.enabled = true;
			triggerIn.gameObject.SetActive(value: false);
			tsf_Layer.position = Tool2D.GetLayerPoint(base.transform);
			AccessOpenSE();
		}
	}

	public override void CloseDirect()
	{
		if (base.gameObject.activeSelf && (base.BelongRoom.roomCfg.type != RoomType.Boss || (base.Dir != FourDir.Left && base.Dir != FourDir.Right)))
		{
			base.gameObject.tag = "Wall";
			anima.Play("CloseDirect");
			triggerIn.gameObject.SetActive(value: false);
			bc_Slef.enabled = true;
			tsf_Layer.position = Tool2D.GetLayerPoint(base.transform);
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

	private void _OpenFinish()
	{
		go_WallCollider.SetActive(value: false);
		tsf_Layer.position = Tool2D.IgnoreZPoint(base.transform, 1.23f);
		if (LevelMgr.Inst.CurrentRoomCtrller == base.BelongRoom)
		{
			triggerIn.gameObject.SetActive(value: true);
		}
	}
}
