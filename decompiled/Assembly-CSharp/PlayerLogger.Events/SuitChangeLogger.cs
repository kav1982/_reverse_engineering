using System.Collections.Generic;

namespace PlayerLogger.Events;

public class SuitChangeLogger : GrowthEventModel
{
	public List<Suit> before_unlocked;

	public List<Suit> after_unlocked;

	public override string GetEventName()
	{
		return "suit_change";
	}

	public override bool CanReport()
	{
		bool flag = GeneralTool.ListContentEquals(before_unlocked, after_unlocked);
		if (base.CanReport())
		{
			return !flag;
		}
		return false;
	}
}
