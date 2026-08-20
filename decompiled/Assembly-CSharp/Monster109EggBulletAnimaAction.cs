using UnityEngine;

public class Monster109EggBulletAnimaAction : MonoBehaviour
{
	public Monster109EggBullet monster109EggBullet;

	public void Crack()
	{
		monster109EggBullet.Crack(base.transform.position);
	}
}
