using System.Collections.Generic;

namespace PlayerLogger.Events;

public class BattleStartLogger : InBattleEventModel
{
	public Suit suit;

	public List<int> ban_spells;

	public TalentStatus talent;

	public List<int> unlocked_research;

	public List<int> unlocked_activate;

	public override string GetEventName()
	{
		return "battle_start";
	}
}
