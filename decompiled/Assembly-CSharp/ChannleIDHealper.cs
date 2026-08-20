public static class ChannleIDHealper
{
	public static string ChannleID(this PluginActivity.ChannleID channleID)
	{
		int num = (int)channleID;
		return num.ToString();
	}
}
