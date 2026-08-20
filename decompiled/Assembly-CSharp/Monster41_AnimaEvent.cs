using System;
using UnityEngine;

public class Monster41_AnimaEvent : MonoBehaviour
{
	public Action<string> DoAction;

	private void DoEvent(string eventName)
	{
		if (DoAction != null)
		{
			DoAction(eventName);
		}
	}
}
