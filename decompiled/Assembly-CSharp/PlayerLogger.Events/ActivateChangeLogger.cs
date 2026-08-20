using System.Collections.Generic;

namespace PlayerLogger.Events;

public class ActivateChangeLogger : GrowthEventModel
{
	public List<int> before_unlocked;

	public List<int> after_unlocked;

	public override string GetEventName()
	{
		return "activate_change";
	}

	public override bool CanReport()
	{
		if (base.CanReport())
		{
			return !GeneralTool.ListContentEquals(before_unlocked, after_unlocked);
		}
		return false;
	}
}
