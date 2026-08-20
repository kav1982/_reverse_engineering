public static class TextProcesser
{
	public static string GetColorText(string source, DataTextColorType type)
	{
		if (GetColorCode(type) == null)
		{
			return source;
		}
		return GetColorStart(type) + source + GetColorEnd(type);
	}

	public static DataTextColorType GetColor_BigIsGood(float source, float current)
	{
		if (current > source)
		{
			return DataTextColorType.Green;
		}
		if (current < source)
		{
			return DataTextColorType.Red;
		}
		return DataTextColorType.Default;
	}

	public static DataTextColorType GetColor_SmallIsGood(float source, float current)
	{
		if (current > source)
		{
			return DataTextColorType.Red;
		}
		if (current < source)
		{
			return DataTextColorType.Green;
		}
		return DataTextColorType.Default;
	}

	public static string GetColorStart(DataTextColorType type)
	{
		if (type == DataTextColorType.Default)
		{
			return "";
		}
		return "<color=" + GetColorCode(type) + ">";
	}

	public static string GetColorEnd(DataTextColorType type)
	{
		if (type == DataTextColorType.Default)
		{
			return "";
		}
		return "</color>";
	}

	public static string GetColorCode(DataTextColorType type)
	{
		return type switch
		{
			DataTextColorType.Red => "#FF2A2A", 
			DataTextColorType.Green => "#05FF00", 
			DataTextColorType.Grey => "#999999", 
			DataTextColorType.Dark => "#666666", 
			_ => null, 
		};
	}
}
