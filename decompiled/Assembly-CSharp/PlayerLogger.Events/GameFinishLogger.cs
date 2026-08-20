namespace PlayerLogger.Events;

public class GameFinishLogger : InBattleEventModel
{
	public PlayerEquips equips;

	public ResourcesStatus resources;

	public override string GetEventName()
	{
		return "game_finish";
	}
}
