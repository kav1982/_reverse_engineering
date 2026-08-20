using UnityEngine;

public class Spell1099Shield : MonoBehaviour
{
	private float duration;

	private float durationTimer;

	public int ShieldValue { get; set; }

	private void Update()
	{
		durationTimer += Time.deltaTime;
		if (durationTimer >= duration)
		{
			durationTimer = 0f;
			base.gameObject.SetActive(value: false);
		}
	}

	public void UpdateShield(float duration, int shieldValue)
	{
		if (base.gameObject.activeSelf)
		{
			if (duration > this.duration - durationTimer)
			{
				this.duration = duration;
				durationTimer = 0f;
			}
		}
		else
		{
			base.gameObject.SetActive(value: true);
			this.duration = duration;
			ShieldValue = shieldValue;
		}
	}
}
