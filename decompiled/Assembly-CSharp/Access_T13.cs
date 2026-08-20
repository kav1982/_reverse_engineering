using UnityEngine;

public class Access_T13 : AccessBase
{
	[Space(50f)]
	public GameObject go_Fog;

	public BoxCollider bc;

	public TriggerIn triggerIn;

	public float triggerOffset;

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
		switch (dir)
		{
		case FourDir.Up:
			triggerIn.transform.localPosition = new Vector3(0f, triggerOffset);
			break;
		case FourDir.Down:
			go_Fog.SetActive(value: false);
			triggerIn.transform.localPosition = new Vector3(0f, 0f - triggerOffset);
			break;
		default:
			Debug.LogError(dir);
			break;
		case FourDir.Left:
		case FourDir.Right:
			break;
		}
	}

	public override void Open()
	{
		if (base.Dir != FourDir.Down)
		{
			OpenDirect();
		}
	}

	public override void OpenDirect()
	{
		if (base.Dir != FourDir.Down)
		{
			base.gameObject.tag = "InteractiveObj";
			bc.enabled = false;
			if (LevelMgr.Inst.CurrentRoomCtrller == base.BelongRoom)
			{
				triggerIn.gameObject.SetActive(value: true);
			}
		}
	}

	public override void Close()
	{
		CloseDirect();
	}

	public override void CloseDirect()
	{
		base.gameObject.tag = "Untagged";
		triggerIn.gameObject.SetActive(value: false);
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
