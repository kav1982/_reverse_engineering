using UnityEngine;

public class Access_Theme11 : AccessBase
{
	[Space(50f)]
	public TriggerIn triggerIn;

	public float triggerOffset;

	private void TriggerEnter(Collider other)
	{
		if (other.IsPlayerTrigger())
		{
			LevelMgr.Inst.PlayerEnterAccess(base.Dir);
		}
	}

	public override void Initialize(RoomController levelCtrller, AccessType type, FourDir dir)
	{
		base.Initialize(levelCtrller, type, dir);
		triggerIn.Initialize(TriggerEnter);
		switch (dir)
		{
		case FourDir.Up:
			triggerIn.transform.localPosition = new Vector3(0f, triggerOffset);
			break;
		case FourDir.Right:
			triggerIn.transform.localPosition = new Vector3(triggerOffset, 0f);
			break;
		case FourDir.Down:
			triggerIn.transform.localPosition = new Vector3(0f, 0f - triggerOffset);
			break;
		case FourDir.Left:
			triggerIn.transform.localPosition = new Vector3(0f - triggerOffset, 0f);
			break;
		default:
			Debug.LogError(dir);
			break;
		}
	}

	public override void Open()
	{
	}

	public override void OpenDirect()
	{
	}

	public override void Close()
	{
	}

	public override void CloseDirect()
	{
	}

	public override void ConditionComplete()
	{
	}
}
