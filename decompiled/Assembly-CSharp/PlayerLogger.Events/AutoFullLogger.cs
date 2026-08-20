using System.Collections.Generic;

namespace PlayerLogger.Events;

public class AutoFullLogger : InBattleEventModel
{
	public ResourcesStatus resources;

	public List<int> before_in_bag_spells;

	public List<Wand> before_wands;

	public List<int> after_in_bag_spells;

	public List<Wand> after_wands;

	public override string GetEventName()
	{
		return "auto_full";
	}
}
