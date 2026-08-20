using System;
using System.Collections;
using UnityEngine;

public abstract class GameUISingletonMono<T> : GameUI, IGameUISingleton where T : GameUISingletonMono<T>
{
	private static T _i;

	public static bool StaticIsOpen
	{
		get
		{
			if (Inited)
			{
				return Inst.IsOpen;
			}
			return false;
		}
	}

	public static bool Inited => (UnityEngine.Object)_i != (UnityEngine.Object)null;

	public static T Inst
	{
		get
		{
			if ((UnityEngine.Object)_i != (UnityEngine.Object)null)
			{
				return _i;
			}
			_i = CreateInstance();
			_i.StartCoroutine(_i.Init());
			return _i;
		}
	}

	public new static void ShowInit(object obj = null)
	{
		if (!Inited)
		{
			_i = CreateInstance();
			_i.StartCoroutine(ShowInitIE(obj));
		}
		else
		{
			_i.Show(obj);
		}
	}

	public void InitFromLoadObj()
	{
		if (Inited)
		{
			return;
		}
		Type type = GetType();
		while (type != null)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(GameUISingletonMono<>) && this is T i)
			{
				_i = i;
				break;
			}
			type = type.BaseType;
		}
		if ((UnityEngine.Object)_i != (UnityEngine.Object)null)
		{
			_i.StartCoroutine(OnlyInitIE());
		}
		else
		{
			Debug.LogError($"Failed to initialize: No matching GameUISingletonMono<{typeof(T)}> component found on {base.gameObject.name}");
		}
	}

	private static IEnumerator ShowInitIE(object obj = null)
	{
		yield return _i.StartCoroutine(_i.Init());
		_i.Show(obj);
	}

	private static IEnumerator OnlyInitIE()
	{
		yield return _i.StartCoroutine(_i.Init());
	}

	private static T CreateInstance()
	{
		GameUISingletonPrefab gameUISingletonPrefab = (GameUISingletonPrefab)typeof(T).GetCustomAttributes(typeof(GameUISingletonPrefab), inherit: false)[0];
		return GameMgr.Inst.LoadUIObj(gameUISingletonPrefab.prefabPath).GetComponent<T>();
	}

	public static void DestroyUI(float time = 0f)
	{
		UnityEngine.Object.Destroy(_i.gameObject, time);
	}

	public static void HideIfInited()
	{
		if (Inited)
		{
			_i.Hide();
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_i = null;
	}
}
