using UnityEngine;

public class Monster101_Gun : MonoBehaviour
{
	[SerializeField]
	private Monster101 monster101;

	[SerializeField]
	private ParticleSystem attack;

	public void GunAttack()
	{
		monster101.Attack();
		attack.Play();
	}

	public void ThrowBulletShell()
	{
		monster101.ThrowBulletShell();
	}

	public void PlayReloadSound()
	{
		SEMgr.Inst.monster12Split.PlaySE();
	}
}
