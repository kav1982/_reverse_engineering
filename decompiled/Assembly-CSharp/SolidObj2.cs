using System.Collections.Generic;
using UnityEngine;

public class SolidObj2 : UnitBase, IRoomObjExtraData
{
	private enum MoveDir
	{
		NoMotion,
		Up,
		Right,
		Down,
		Left
	}

	private MoveDir moveDir;

	private MoveDir forceDir;

	private SpecialObj7 so7_GoTrack;

	private List<UnitProperty> passengers = new List<UnitProperty>();

	public override void EveryInitialCallback()
	{
		passengers.Clear();
	}

	public override void Frame1InitialCallback()
	{
		Collider nearestColliderByTag = GeneralTool.GetNearestColliderByTag(base.transform.position, 1f, "SpikesTrack");
		if (nearestColliderByTag == null)
		{
			moveDir = MoveDir.NoMotion;
			return;
		}
		so7_GoTrack = nearestColliderByTag.GetComponent<SpecialObj7>();
		base.transform.position = so7_GoTrack.transform.position;
		if (!so7_GoTrack.IsInitialized)
		{
			so7_GoTrack.Initialize();
		}
		if (forceDir != 0)
		{
			moveDir = forceDir;
		}
		else if (so7_GoTrack.UpTrack != null)
		{
			moveDir = MoveDir.Up;
		}
		else if (so7_GoTrack.RightTrack != null)
		{
			moveDir = MoveDir.Right;
		}
		else if (so7_GoTrack.DownTrack != null)
		{
			moveDir = MoveDir.Down;
		}
		else if (so7_GoTrack.LeftTrack != null)
		{
			moveDir = MoveDir.Left;
		}
		else
		{
			moveDir = MoveDir.NoMotion;
		}
	}

	public override void Update()
	{
		base.Update();
	}

	public void FixedUpdate()
	{
		if (moveDir == MoveDir.NoMotion)
		{
			return;
		}
		float num = base.MoveSpeed * Time.deltaTime;
		Vector3 zero = Vector3.zero;
		if ((base.transform.position - so7_GoTrack.transform.position).sqrMagnitude <= num * num)
		{
			zero = so7_GoTrack.transform.position - base.transform.position;
			base.transform.position = so7_GoTrack.transform.position;
			switch (moveDir)
			{
			case MoveDir.Up:
				if (so7_GoTrack.UpTrack != null)
				{
					so7_GoTrack = so7_GoTrack.UpTrack;
				}
				else if (so7_GoTrack.RightTrack != null)
				{
					so7_GoTrack = so7_GoTrack.RightTrack;
					moveDir = MoveDir.Right;
				}
				else if (so7_GoTrack.LeftTrack != null)
				{
					so7_GoTrack = so7_GoTrack.LeftTrack;
					moveDir = MoveDir.Left;
				}
				else
				{
					so7_GoTrack = so7_GoTrack.DownTrack;
					moveDir = MoveDir.Down;
				}
				break;
			case MoveDir.Right:
				if (so7_GoTrack.RightTrack != null)
				{
					so7_GoTrack = so7_GoTrack.RightTrack;
				}
				else if (so7_GoTrack.DownTrack != null)
				{
					so7_GoTrack = so7_GoTrack.DownTrack;
					moveDir = MoveDir.Down;
				}
				else if (so7_GoTrack.UpTrack != null)
				{
					so7_GoTrack = so7_GoTrack.UpTrack;
					moveDir = MoveDir.Up;
				}
				else
				{
					so7_GoTrack = so7_GoTrack.LeftTrack;
					moveDir = MoveDir.Left;
				}
				break;
			case MoveDir.Down:
				if (so7_GoTrack.DownTrack != null)
				{
					so7_GoTrack = so7_GoTrack.DownTrack;
				}
				else if (so7_GoTrack.LeftTrack != null)
				{
					so7_GoTrack = so7_GoTrack.LeftTrack;
					moveDir = MoveDir.Left;
				}
				else if (so7_GoTrack.RightTrack != null)
				{
					so7_GoTrack = so7_GoTrack.RightTrack;
					moveDir = MoveDir.Right;
				}
				else
				{
					so7_GoTrack = so7_GoTrack.UpTrack;
					moveDir = MoveDir.Up;
				}
				break;
			case MoveDir.Left:
				if (so7_GoTrack.LeftTrack != null)
				{
					so7_GoTrack = so7_GoTrack.LeftTrack;
				}
				else if (so7_GoTrack.UpTrack != null)
				{
					so7_GoTrack = so7_GoTrack.UpTrack;
					moveDir = MoveDir.Up;
				}
				else if (so7_GoTrack.DownTrack != null)
				{
					so7_GoTrack = so7_GoTrack.DownTrack;
					moveDir = MoveDir.Down;
				}
				else
				{
					so7_GoTrack = so7_GoTrack.RightTrack;
					moveDir = MoveDir.Right;
				}
				break;
			default:
				Debug.LogError(moveDir);
				break;
			}
		}
		else
		{
			zero = (so7_GoTrack.transform.position - base.transform.position).normalized * num;
			base.transform.position += zero;
		}
		for (int num2 = passengers.Count - 1; num2 >= 0; num2--)
		{
			if (passengers[num2] != null)
			{
				passengers[num2].transform.position += zero;
			}
			else
			{
				passengers.Remove(passengers[num2]);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Monster" || other.tag == "Teammate")
		{
			passengers.Add(other.GetComponent<UnitProperty>());
		}
		else if (other.IsPlayerTrigger())
		{
			passengers.Add(other.GetComponent<UnitProperty>());
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Monster" || other.tag == "Teammate")
		{
			passengers.Remove(other.GetComponent<UnitProperty>());
		}
		else if (other.IsPlayerTrigger())
		{
			passengers.Remove(other.GetComponent<UnitProperty>());
		}
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 <= 2f)
		{
			if (data1 != 1f)
			{
				if (data1 == 2f)
				{
					forceDir = MoveDir.Right;
				}
			}
			else
			{
				forceDir = MoveDir.Up;
			}
		}
		else if (data1 != 3f)
		{
			if (data1 == 4f)
			{
				forceDir = MoveDir.Left;
			}
		}
		else
		{
			forceDir = MoveDir.Down;
		}
	}
}
