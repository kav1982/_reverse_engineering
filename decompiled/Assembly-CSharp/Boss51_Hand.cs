using UnityEngine;

public class Boss51_Hand : MonoBehaviour
{
	public enum HandState
	{
		Idle,
		AttackPrepare,
		Attack
	}

	public Transform tsf_Hand;

	public Boss51 master;

	public HandState state;

	private Vector3 nowDir;

	private Vector3 targetDir;

	private float masterFlip;

	public void SetTargetDir(Vector3 dir)
	{
		targetDir = dir;
	}

	public void OnFlip()
	{
		nowDir.x = 0f - nowDir.x;
	}

	private void Start()
	{
		masterFlip = Mathf.Sign(master.tsf_Model.localScale.x);
		targetDir = Vector3.right;
		nowDir = Vector3.right;
	}

	private void Update()
	{
		if (!master.IsLocked)
		{
			if (masterFlip != Mathf.Sign(master.tsf_Model.localScale.x))
			{
				OnFlip();
				masterFlip = Mathf.Sign(master.tsf_Model.localScale.x);
			}
			tsf_Hand.localEulerAngles = new Vector3(0f, 0f, masterFlip * (Tool2D.IgnoreZAngleWithSign(nowDir) + 90f) - (float)((!(master.tsf_Model.localScale.x > 0f)) ? 180 : 0));
			switch (state)
			{
			case HandState.Idle:
				tsf_Hand.localPosition = new Vector3(0f, 0f, 0.005f);
				nowDir = Tool2D.RotateTowardsAroundZAxisSmooth(nowDir, masterFlip * Vector3.right, 180f * Time.deltaTime, 5f);
				break;
			case HandState.Attack:
				nowDir = Tool2D.RotateTowardsAroundZAxisSmooth(nowDir, targetDir, 180f * Time.deltaTime, 10f);
				tsf_Hand.localPosition = new Vector3(0f, 0f, 0.015f) * ((nowDir.y > 0f) ? 1 : (-1));
				break;
			}
		}
	}
}
