using UnityEngine;

public class MainCamera : MonoBehaviour
{
	public LevelMgr battleManager;

	public float followLerp = 0.1f;

	private Transform followT;

	private Animator animator;

	private bool isFollow = true;

	private bool isShocking;

	private float shockTime;

	private float shockTimer;

	private void FixedUpdate()
	{
		FollowTarget();
		ShockScreen();
	}

	private void FollowTarget()
	{
		if (isFollow && followT != null)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, followT.position, followLerp);
		}
	}

	private void ShockScreen()
	{
		if (isShocking)
		{
			shockTimer += Time.deltaTime;
			if (shockTimer >= shockTime)
			{
				shockTimer = 0f;
				isShocking = false;
				animator.SetTrigger("Idle");
			}
		}
	}

	public void Initialize(Transform followT)
	{
		this.followT = followT;
		animator = GetComponent<Animator>();
		base.transform.position = followT.position;
	}

	public void SetShock(float shockTime)
	{
		this.shockTime = shockTime;
		isShocking = true;
		animator.SetTrigger("Shock");
	}
}
