using UnityEngine;

public class Monster102_Gun : MonoBehaviour
{
	[SerializeField]
	private Monster102 monster102;

	[SerializeField]
	private ParticleSystem attack;

	public void GunAttack()
	{
		monster102.Attack();
		attack.Play();
	}

	public void ThrowBulletShell()
	{
		monster102.ThrowBulletShell();
	}

	public void PlayReloadSound()
	{
		SEMgr.Inst.monster12Split.PlaySE();
	}
}
