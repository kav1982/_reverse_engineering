using System.Text;

public static class SpellInfoExtend
{
	public static StringBuilder AppendLink(this StringBuilder self, params string[] strings)
	{
		foreach (string value in strings)
		{
			self.Append(value);
		}
		return self;
	}

	public static StringBuilder AppendIntField(this StringBuilder self, int fieldName, int fieldValue, DataTextColorType color, bool newLine = true, string newLineAdd = "", string valuePrefix = null, string valuePostfix = null)
	{
		string text = fieldValue.ToString();
		if (valuePrefix != null)
		{
			text = valuePrefix + text;
		}
		if (valuePostfix != null)
		{
			text += valuePostfix;
		}
		return self.AppendStringField(fieldName, text, color, newLine, newLineAdd);
	}

	public static StringBuilder AppendFloatField(this StringBuilder self, int fieldName, float fieldValue, DataTextColorType color, bool newLine = true, string newLineAdd = "", string valuePrefix = null, string valuePostfix = null)
	{
		string text = GeneralTool.FloatToRetainDecimals(fieldValue, 2);
		if (valuePrefix != null)
		{
			text = valuePrefix + text;
		}
		if (valuePostfix != null)
		{
			text += valuePostfix;
		}
		return self.AppendStringField(fieldName, text, color, newLine, newLineAdd);
	}

	public static StringBuilder AppendStringField(this StringBuilder self, int fieldName, string fieldValue, DataTextColorType color, bool newLine = true, string newLineAdd = "")
	{
		self.StartField(newLine, newLineAdd).Append(fieldName.GetText(forceApplyAlogia: true)).Append(": ")
			.Append(TextProcesser.GetColorText(fieldValue, color));
		return self;
	}

	public static StringBuilder StartField(this StringBuilder self, bool newLine = true, string StartAs = "")
	{
		if (newLine && self.Length > 0)
		{
			if (self[self.Length - 1] != '\n')
			{
				self.AppendLine();
			}
		}
		return self.Append(StartAs);
	}

	public static DataTextColorType ColorByValue_BigGood(float source, float current)
	{
		if (current - 0.001f > source)
		{
			return DataTextColorType.Green;
		}
		if (current + 0.001f < source)
		{
			return DataTextColorType.Red;
		}
		return DataTextColorType.Default;
	}

	public static DataTextColorType ColorByValue_LowGood(float source, float current)
	{
		if (current - 0.001f > source)
		{
			return DataTextColorType.Red;
		}
		if (current + 0.001f < source)
		{
			return DataTextColorType.Green;
		}
		return DataTextColorType.Default;
	}
}
