using System.Collections.Generic;

namespace PlayerLogger.Events;

public class PlayerDeathLogger : InBattleEventModel
{
	public int death_counter;

	public List<int> monsters;

	public PlayerEquips equips;

	public ResourcesStatus resources;

	public bool click_return_camp;

	public override string GetEventName()
	{
		return "player_death";
	}
}
