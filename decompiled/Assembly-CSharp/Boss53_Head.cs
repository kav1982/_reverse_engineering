using UnityEngine;

public class Boss53_Head : MonoBehaviour
{
	public enum HeadState
	{
		Normal,
		Droped,
		Backing
	}

	public Boss53 Boss;

	public float AttackInterval = 0.5f;

	public float AttackRange = 8f;

	public float DropForce = 10f;

	public float DropDrag = 5f;

	public float MoveSpeed = 2f;

	private Vector2 move;

	private float attackTimer;

	private float force;

	private float backTimer;

	public HeadState State { get; private set; }

	private void Update()
	{
		if (State == HeadState.Normal)
		{
			base.transform.position = Boss.headPoint.position;
			return;
		}
		force = Mathf.Lerp(force, 0f, Time.deltaTime * DropDrag);
		if (State == HeadState.Droped)
		{
			if (Boss.HaveTarget)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, Boss.TargetPoint, (MoveSpeed + force) * Time.deltaTime);
			}
		}
		else if (State == HeadState.Backing)
		{
			backTimer += Time.deltaTime;
			if (backTimer >= 1f)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, Boss.headPoint.position, (MoveSpeed + force) * Time.deltaTime);
				if (Vector3.Distance(base.transform.position, Boss.headPoint.position) < 0.1f)
				{
					State = HeadState.Normal;
				}
			}
		}
		attackTimer += Time.deltaTime;
		if (attackTimer >= AttackInterval)
		{
			attackTimer = 0f;
			ReleaseAttack();
		}
	}

	private void ReleaseAttack()
	{
	}

	public void Drop()
	{
		State = HeadState.Droped;
		force = DropForce;
	}

	public void Back()
	{
		State = HeadState.Backing;
		backTimer = 0f;
	}
}
