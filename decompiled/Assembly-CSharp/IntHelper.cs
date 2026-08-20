using System.Text;

public static class IntHelper
{
	public static string GetText(this int i, bool forceApplyAlogia = false)
	{
		return TextConfig.GetText(i, forceApplyAlogia);
	}

	public static string GetFormatText(this int i, bool forceApplyAlogia = false)
	{
		return new StringBuilder(TextConfig.GetText(i, forceApplyAlogia)).Replace("\\n", "\n").Replace("\\KeySprint", ControlMgr.GetKeyDisplayName(ControlMgr.Inst.inputActions.Player.Sprint, 0)).ToString();
	}

	public static string GetRomanNumber(this int i)
	{
		return GeneralTool.GetRomanNumber(i);
	}
}
