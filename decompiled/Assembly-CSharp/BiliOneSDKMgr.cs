using UnityEngine;

public class BiliOneSDKMgr : MonoBehaviour
{
	private static BiliOneSDKMgr instance;

	private static BiliOneSDK BiliOneSDK;

	private static bool Inited
	{
		get
		{
			if (instance != null)
			{
				return BiliOneSDK != null;
			}
			return false;
		}
	}

	private void Awake()
	{
		if (GameMgr.IsUseBiliOneSDK && !GameMgr.IsMobile_Static)
		{
			if (Inited)
			{
				Object.Destroy(base.gameObject);
			}
			instance = this;
			BiliOneSDK = new BiliOneSDK();
			BiliOneSDK.Init();
		}
	}

	public static void Logout()
	{
		if (Inited)
		{
			BiliOneSDK.LogOutReLog();
		}
	}

	public static void OneSDKUnInit()
	{
		if (Inited)
		{
			BiliOneSDK.UnInitAndQuit();
		}
	}
}
