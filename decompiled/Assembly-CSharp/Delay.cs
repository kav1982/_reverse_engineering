using UnityEngine;

public class Delay : MonoBehaviour
{
	public float delayTime = 1f;

	private void Start()
	{
		base.gameObject.SetActive(value: false);
		Invoke("Active", delayTime);
	}

	private void Active()
	{
		base.gameObject.SetActive(value: true);
	}
}
