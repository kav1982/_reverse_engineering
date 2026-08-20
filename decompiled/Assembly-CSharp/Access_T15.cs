using UnityEngine;

public class Access_T15 : AccessBase
{
	[Space(50f)]
	public GameObject go_Highlight;

	public Animator anima;

	public BoxCollider bc;

	public TriggerIn triggerIn;

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

	private void _OpenFinish()
	{
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
		if (base.Dir == FourDir.Down)
		{
			return;
		}
		base.gameObject.tag = "Untagged";
		if (base.AccessType == AccessType.Key)
		{
			if (base.IsConditionComplete)
			{
				anima.Play("Open");
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
			anima.Play("Open");
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
				anima.Play("OpenDirect");
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
			anima.Play("OpenDirect");
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
		base.gameObject.tag = "Untagged";
		go_WallCollider.SetActive(value: true);
		anima.Play("Close");
		triggerIn.gameObject.SetActive(value: false);
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform);
		bc.enabled = true;
		AccessOpenSE();
	}

	public override void CloseDirect()
	{
		base.gameObject.tag = "Untagged";
		go_WallCollider.SetActive(value: true);
		anima.Play("CloseDirect");
		triggerIn.gameObject.SetActive(value: false);
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform);
		bc.enabled = true;
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
