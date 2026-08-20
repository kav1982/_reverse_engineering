using UnityEngine;

public class CityCamera : MonoBehaviour
{
	public Animator animator;

	[Header("MouseOffset")]
	public Transform tsf_MouseOffset;

	public float mouseZOffset;

	public float mouseLerp;

	[Header("follow")]
	public Transform tsf_Move;

	public float xLimit;

	public float zLimit;

	public float followLerp;

	public int localMaxX;

	public int localMinX;

	public int localMaxZ;

	public int localMinZ;

	private Transform followTargetT;

	private float halfScreenHeight;

	private bool isVibration;

	private float vibrationTime;

	private float vibrationTimer;

	public static CityCamera Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
		halfScreenHeight = (float)Screen.height / 2f;
	}

	private void Update()
	{
		Vibration();
	}

	private void Vibration()
	{
		if (isVibration)
		{
			vibrationTimer += Time.deltaTime;
			if (vibrationTimer >= vibrationTime)
			{
				vibrationTimer = 0f;
				isVibration = false;
				animator.SetTrigger("Idle");
			}
		}
	}

	private void FixedUpdate()
	{
		FollowTarget();
	}

	private void FollowTarget()
	{
		if (!(followTargetT == null))
		{
			Vector3 targetLocalPoint = GetTargetLocalPoint();
			if (targetLocalPoint.x < (float)localMinX + xLimit)
			{
				targetLocalPoint.x = (float)localMinX + xLimit;
			}
			else if (targetLocalPoint.x > (float)localMaxX - xLimit)
			{
				targetLocalPoint.x = (float)localMaxX - xLimit;
			}
			if (targetLocalPoint.z < (float)localMinZ + zLimit)
			{
				targetLocalPoint.z = (float)localMinZ + zLimit;
			}
			else if (targetLocalPoint.z > (float)localMaxZ - zLimit)
			{
				targetLocalPoint.z = (float)localMaxZ - zLimit;
			}
			tsf_Move.localPosition = Vector3.Lerp(tsf_Move.localPosition, targetLocalPoint, followLerp);
			float num = (Input.mousePosition.y - halfScreenHeight) / halfScreenHeight;
			float num2 = Mathf.Pow(Mathf.Abs(num), 1.5f) * mouseZOffset;
			Vector3 zero = Vector3.zero;
			zero = ((!(num < 0f)) ? new Vector3(0f, 0f, num2) : new Vector3(0f, 0f, 0f - num2));
			tsf_MouseOffset.localPosition = Vector3.Lerp(tsf_MouseOffset.localPosition, zero, mouseLerp);
		}
	}

	private Vector3 GetTargetLocalPoint()
	{
		return base.transform.InverseTransformPoint(Tool2D.IgnoreZPoint(followTargetT));
	}

	public void SetFollow(Transform followTargetT)
	{
		this.followTargetT = followTargetT;
	}

	public void VibrationScreen(float time = 0.2f)
	{
		isVibration = true;
		vibrationTime = time;
		vibrationTimer = 0f;
		animator.SetTrigger("Shock");
	}
}
