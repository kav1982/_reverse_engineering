using UnityEngine;

public class Elite9_Body : MonoBehaviour
{
	[Header("腿相关")]
	public GameObject pfb_Leg;

	public Elite9_Leg leftLeg;

	public Elite9_Leg rightLeg;

	public float rootHeight;

	public float firstHeightFix;

	public Elite9 master;

	private Vector3 lastFramePosition;

	private Vector3 temp;

	[Header("多节相关")]
	public Vector3 startDir;

	public Elite9_Body front;

	public float bodyInterval;

	public float closeLerp;

	public bool isTail;

	public Vector3 moveDir
	{
		get
		{
			if (HaveFront)
			{
				return Tool2D.IgnoreZPoint(front.transform.position - base.transform.position).normalized;
			}
			return master.CurrentMotion.normalized;
		}
	}

	public bool HaveFront => front != null;

	public void Initialize(Elite9 master, Elite9_Body front = null, bool isTail = false)
	{
		this.front = front;
		this.isTail = isTail;
		this.master = master;
		if (front != null)
		{
			base.transform.position = front.transform.position + startDir.normalized * bodyInterval;
		}
		lastFramePosition = base.transform.position;
		if (!isTail)
		{
			leftLeg = Object.Instantiate(pfb_Leg, base.transform).GetComponent<Elite9_Leg>();
			leftLeg.SingleInitial(master, this, leftLeg: true);
			rightLeg = Object.Instantiate(pfb_Leg, base.transform).GetComponent<Elite9_Leg>();
			rightLeg.SingleInitial(master, this, leftLeg: false);
			leftLeg.Frame1Initail();
			rightLeg.Frame1Initail();
		}
		if (front == null)
		{
			rootHeight *= firstHeightFix;
		}
	}

	private void Update()
	{
		if (master == null)
		{
			return;
		}
		if (HaveFront)
		{
			if ((front.transform.position - base.transform.position).sqrMagnitude > bodyInterval * bodyInterval)
			{
				Vector3 vector = Vector3.Lerp(base.transform.position, front.transform.position + (-front.transform.position + base.transform.position).normalized * bodyInterval, closeLerp);
				if (master.state != Elite9.MonsterState.DoubleSlashChase && (base.transform.position - vector).sqrMagnitude > Mathf.Pow(master.myPpt.MoveSpeed * Time.deltaTime, 2f))
				{
					base.transform.position += (front.transform.position - base.transform.position).normalized * master.myPpt.MoveSpeed * Time.deltaTime;
				}
				else if (master.state == Elite9.MonsterState.DoubleSlashChase && (base.transform.position - vector).sqrMagnitude > Mathf.Pow(master.myPpt.MoveSpeed * master.doubleSlashSpeedFix * Time.deltaTime, 2f))
				{
					base.transform.position += (front.transform.position - base.transform.position).normalized * master.myPpt.MoveSpeed * master.doubleSlashSpeedFix * Time.deltaTime;
				}
				else
				{
					base.transform.position = vector;
				}
			}
		}
		else
		{
			base.transform.position = master.transform.position;
		}
	}
}
