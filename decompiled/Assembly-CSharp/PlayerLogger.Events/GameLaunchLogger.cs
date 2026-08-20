using UnityEngine;

namespace PlayerLogger.Events;

public class GameLaunchLogger : EventModel
{
	public string os { get; private set; } = SystemInfo.operatingSystem;


	public string cpu { get; private set; } = SystemInfo.processorType;


	public int cpu_freq { get; private set; } = SystemInfo.processorFrequency;


	public int cpu_count { get; private set; } = SystemInfo.processorCount;


	public string gpu { get; private set; } = SystemInfo.graphicsDeviceName;


	public int mem { get; private set; } = SystemInfo.systemMemorySize;


	public override string GetEventName()
	{
		return "game_launch";
	}
}
