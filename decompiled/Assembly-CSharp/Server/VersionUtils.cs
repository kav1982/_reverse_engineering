using System.Collections.Generic;

namespace Server;

public static class VersionUtils
{
	public static string FormatToString(int version)
	{
		List<string> list = new List<string>();
		if (version % 100 != 0)
		{
			list.Add($"f{version % 100}");
		}
		version /= 100;
		list.Insert(0, (version % 100).ToString());
		list.Insert(0, ".");
		version /= 100;
		list.Insert(0, (version % 1000).ToString());
		list.Insert(0, ".");
		version /= 1000;
		list.Insert(0, version.ToString());
		return string.Join("", list);
	}

	public static int ParseToInt(string version)
	{
		string[] array = version.Trim().ToLower().Split(".");
		int num = int.Parse(array[0]);
		int num2 = int.Parse(array[1]);
		array = array[2].Split('f');
		int num3 = int.Parse(array[0]);
		return ((array.Length > 1) ? int.Parse(array[1]) : 0) + num3 * 100 + num2 * 10000 + num * 10000000;
	}
}
