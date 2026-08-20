using System;
using UnityEngine;

public class AnimaEvent : MonoBehaviour
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
