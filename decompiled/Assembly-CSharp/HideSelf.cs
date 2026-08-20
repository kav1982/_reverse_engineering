using UnityEngine;

public class HideSelf : MonoBehaviour
{
	private bool willHide = true;

	private float duration;

	private float durationTimer;

	private void Update()
	{
		if (willHide)
		{
			durationTimer += Time.deltaTime;
			if (durationTimer >= duration)
			{
				base.gameObject.SetActive(value: false);
			}
		}
	}

	public void SetDuration(float duration)
	{
		this.duration = duration;
		if (duration > 0f)
		{
			willHide = true;
			durationTimer = 0f;
			base.enabled = true;
		}
		else
		{
			willHide = false;
			base.enabled = false;
		}
	}
}
