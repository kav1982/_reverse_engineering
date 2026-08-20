using UnityEngine;

public class Spell4014CrystalPowerUpCtrl : MonoBehaviour
{
	public ParticleSystem dust;

	public ParticleSystem aura;

	private ParticleSystem.MainModule dustMain;

	private void Awake()
	{
		dustMain = dust.main;
	}

	public void SetEffect(float timeRate)
	{
		dustMain.startSpeed = Mathf.Min(timeRate * 0.6f, 6f);
		aura.transform.localScale = Vector3.one * Mathf.Min(timeRate * 0.05f, 1.8f);
	}
}
