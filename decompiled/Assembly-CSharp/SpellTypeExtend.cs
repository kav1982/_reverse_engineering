using UnityEngine;

public static class SpellTypeExtend
{
	public static Color SlotColor(this SpellType type)
	{
		return type switch
		{
			SpellType.Missile => GameConst.color_SpellRingTypeMissle, 
			SpellType.Summon => GameConst.color_SpellRingTypeMissle, 
			SpellType.Enhance => GameConst.color_SpellRingTypeEnhance, 
			SpellType.Passive => GameConst.color_SpellRingTypePassive, 
			_ => default(Color), 
		};
	}
}
