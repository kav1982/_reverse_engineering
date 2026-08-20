namespace PlayerLogger.Events;

public class ResourceConvertLogger : GrowthEventModel
{
	public override string GetEventName()
	{
		return "resource_convert";
	}

	public override bool CanReport()
	{
		if (base.CanReport())
		{
			return base.before_resources != base.after_resources;
		}
		return false;
	}
}
