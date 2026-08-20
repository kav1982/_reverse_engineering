using Steamworks;
using UnityEngine;

namespace PlayerLogger;

public abstract class EventModel
{
	private bool _reported;

	public string steam_id { get; private set; }

	public int game_version { get; private set; }

	protected EventModel()
	{
		if (SteamManager.Initialized)
		{
			steam_id = SteamUser.GetSteamID().ToString();
		}
		else
		{
			steam_id = "";
		}
		game_version = GameVersion.Final;
	}

	public abstract string GetEventName();

	public virtual bool CanReport()
	{
		return true;
	}

	public void Report()
	{
		RuntimePlatform platform = Application.platform;
		if ((platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer) && CanReport())
		{
			if (_reported)
			{
				Debug.LogError(GetEventName() + " 已经上报过了，不能重复上报");
				return;
			}
			Logger.ReportForce(this);
			_reported = true;
		}
	}
}
