using UnityEngine;

public class Spell1018Ring : MonoBehaviour
{
	private const float FastRotateSpeed = 50f;

	private const float SlowRotateSpeed = 15f;

	private Transform FastRotateTsf;

	private Transform SlowRotateTsf;

	private void Awake()
	{
		FastRotateTsf = base.transform.Find("Ring1");
		SlowRotateTsf = base.transform.Find("Ring2");
	}

	private void Update()
	{
		FastRotateTsf.Rotate(0f, 0f, 50f * Time.deltaTime);
		SlowRotateTsf.Rotate(0f, 0f, 15f * Time.deltaTime);
	}
}
