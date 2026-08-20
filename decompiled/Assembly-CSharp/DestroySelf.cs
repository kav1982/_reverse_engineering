using UnityEngine;

public class DestroySelf : MonoBehaviour
{
	public float delay;

	private float durationTimer;

	private void Update()
	{
		durationTimer += Time.deltaTime;
		if (durationTimer >= delay)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
