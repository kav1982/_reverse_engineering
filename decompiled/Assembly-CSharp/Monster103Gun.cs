using UnityEngine;

public class Monster103Gun : MonoBehaviour
{
	[SerializeField]
	private Monster103 monster103;

	[SerializeField]
	private ParticleSystem attack;

	public void GunAttack()
	{
		monster103.Attack();
		attack.Play();
	}

	public void ThrowBulletShell()
	{
		monster103.ThrowBulletShell();
	}

	public void PlayReloadSound()
	{
		SEMgr.Inst.monster12Split.PlaySE();
	}

	public void PlayAttackAnim()
	{
		monster103.aimLine.gameObject.SetActive(value: false);
		monster103.aimLineShadow.gameObject.SetActive(value: false);
		monster103.gunAnima.Play("Attack");
	}
}
