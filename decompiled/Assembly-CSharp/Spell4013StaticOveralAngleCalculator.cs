using System.Collections.Generic;
using System.Linq;

public static class Spell4013StaticOveralAngleCalculator
{
	public static float OveralAngle = 0f;

	public static bool UpdatedOveralAngleInThisFrame = true;

	private static List<(Wand wand, float Angle)> WandAngleShiftList = new List<(Wand, float)>();

	public static List<(Wand wand, float angle)> GetWandAngleShiftList()
	{
		WandAngleShiftList = WandAngleShiftList.Where(delegate((Wand wand, float Angle) e)
		{
			if (!e.wand)
			{
				return false;
			}
			if (e.wand.WandCfg == null)
			{
				return false;
			}
			return e.wand.passiveRuneHammerEnable ? true : false;
		}).ToList();
		return WandAngleShiftList;
	}

	public static void AddNewAngleData(Wand wand, float angle)
	{
		GetWandAngleShiftList();
		WandAngleShiftList.Add((wand, angle));
	}
}
