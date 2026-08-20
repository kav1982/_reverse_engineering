using UnityEngine;
using UnityEngine.UI;

public class HS_HittedObject : MonoBehaviour
{
	public float startHealth = 100f;

	private float health;

	public Image healthBar;

	private void Start()
	{
		health = startHealth;
	}

	private void Update()
	{
	}

	public void TakeDamage(float amount)
	{
		health -= amount;
		healthBar.fillAmount = health / startHealth;
		if (health <= 0f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
