namespace PlayerLogger;

public abstract class GrowthEventModel : InWorldEventModel
{
	public ResourcesStatus before_resources { get; private set; }

	public ResourcesStatus after_resources { get; private set; }

	public ResourcesStatus flow_resources { get; private set; }

	public void AutoRecordBeforeResources()
	{
		before_resources = ResourcesStatus.CreateAuto();
	}

	public void AutoRecordAfterResourcesAndFlow()
	{
		after_resources = ResourcesStatus.CreateAuto();
		flow_resources = ResourcesStatus.CreateFlow(before_resources, after_resources);
	}
}
