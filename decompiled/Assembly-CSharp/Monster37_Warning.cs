using UnityEngine;

public class Monster37_Warning : MonoBehaviour
{
	public Transform tsf;

	private float existTimer;

	private float existTime;

	public bool attacked;

	public bool finished;

	public float attackTime;

	public bool canAttack;

	public void Initialize(float radius, float attackTime, float time = 999f)
	{
		canAttack = false;
		tsf.gameObject.SetActive(value: true);
		tsf.localScale = Vector3.one * radius * 2f;
		this.attackTime = attackTime;
		existTime = time;
		existTimer = 0f;
		attacked = false;
		finished = false;
	}

	public void Close()
	{
		tsf.gameObject.SetActive(value: false);
		finished = true;
	}

	private void Start()
	{
	}

	private void Update()
	{
		existTimer += Time.deltaTime;
		if (existTimer > attackTime)
		{
			canAttack = true;
		}
		if (existTimer > existTime)
		{
			Close();
		}
	}
}
