namespace PlayerLogger.Events;

public class GuideStageFinish : InWorldEventModel
{
	public int stage;

	public int spend_seconds;

	public int room_id;

	public override string GetEventName()
	{
		return "guide_stage_finish";
	}

	public GuideStageFinish(int stage, int room_id)
	{
		this.stage = stage;
		this.room_id = room_id;
	}
}
