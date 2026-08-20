namespace PlayerLogger.Events;

public class TalentChangeLogger : GrowthEventModel
{
	public TalentStatus before_talent;

	public TalentStatus after_talent;

	public override string GetEventName()
	{
		return "talent_change";
	}

	public override bool CanReport()
	{
		if (base.CanReport())
		{
			return before_talent != after_talent;
		}
		return false;
	}
}
