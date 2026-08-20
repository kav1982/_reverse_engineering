using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellEvent<T>
{
	private readonly List<Action<T>> _listeners = new List<Action<T>>();

	public void Invoke(T param)
	{
		foreach (Action<T> listener in _listeners)
		{
			listener(param);
		}
	}

	public void ClearListener()
	{
		_listeners.Clear();
	}

	public void Listen(Action<T> func)
	{
		if (_listeners.Contains(func))
		{
			Debug.LogError("不应该重复监听法术事件");
		}
		else
		{
			_listeners.Add(func);
		}
	}
}
