using UnityEngine;

public class Boss12_Bird : MonoBehaviour
{
	public Animator anim;

	public Vector3 targetPos;

	public bool countDown;

	public float countDownTime;

	public float countDownTimer;

	public float moveSpeed;

	private void OnEnable()
	{
		countDownTimer = 0f;
	}

	private void Update()
	{
		base.transform.position = Vector3.MoveTowards(base.transform.position, targetPos, moveSpeed * Time.deltaTime);
		if (countDown)
		{
			countDownTimer += Time.deltaTime;
			if (countDownTimer >= countDownTime)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
	}
}
