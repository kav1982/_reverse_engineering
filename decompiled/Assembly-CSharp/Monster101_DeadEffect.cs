using UnityEngine;

public class Monster101_DeadEffect : MonoBehaviour
{
	public Monster101 monster101;

	public void Bang()
	{
		base.gameObject.SetActive(value: false);
		monster101.myPpt.AnnouncedDeath();
	}
}
