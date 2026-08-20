using System;
using System.Collections.Generic;

public static class DebugValues
{
	public class DValue
	{
		public string Name;

		public Type Type;

		public object Value;
	}

	public static readonly Dictionary<string, DValue> Values = new Dictionary<string, DValue>();

	public static T Get<T>(string name, T noDebugValue = default(T))
	{
		if (!Values.ContainsKey(name))
		{
			Values.Add(name, new DValue
			{
				Name = name,
				Type = typeof(T),
				Value = noDebugValue
			});
		}
		return (T)Values[name].Value;
	}
}
