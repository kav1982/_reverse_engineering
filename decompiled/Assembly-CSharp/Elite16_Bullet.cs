using System;
using UnityEngine;

public class Elite16_Bullet : MonoBehaviour
{
	public Action<Collision> onCollisionEnter;

	private void OnCollisionStay(Collision collision)
	{
		onCollisionEnter?.Invoke(collision);
	}
}
