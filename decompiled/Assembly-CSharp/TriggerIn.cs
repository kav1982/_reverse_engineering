using System;
using UnityEngine;

public class TriggerIn : MonoBehaviour
{
	public Collider colliderObject;

	private Action<Collider> action;

	private void OnTriggerEnter(Collider other)
	{
		if (action != null && other.gameObject != null)
		{
			action(other);
		}
	}

	public void Initialize(Action<Collider> action)
	{
		this.action = action;
	}
}
