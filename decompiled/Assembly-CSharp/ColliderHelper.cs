using UnityEngine;

public static class ColliderHelper
{
	public static bool IsPlayerTrigger(this Collider collider)
	{
		if (collider.CompareTag("Player"))
		{
			return collider.isTrigger;
		}
		return false;
	}
}
