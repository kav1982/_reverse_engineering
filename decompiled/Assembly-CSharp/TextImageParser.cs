using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class TextImageParser
{
	public class ContentBlock
	{
		public enum ContentType
		{
			Text,
			Image,
			Link
		}

		public ContentType Type;

		public string Content;

		public string Content2;

		public int Index { get; set; }

		public int Length { get; set; }
	}

	public class UpdateContent
	{
		public Dictionary<(int id1, int id2), string> IdUrlMap = new Dictionary<(int, int), string>();

		public string OutsideText = "";

		public string DefaultUrl;
	}

	private class MatchInfo
	{
		public int Index { get; set; }

		public int Length { get; set; }

		public ContentBlock.ContentType Type { get; set; }

		public string Content { get; set; }

		public string Text { get; set; }
	}

	public static UpdateContent GetUpdateContent(string text)
	{
		Debug.Log(text);
		UpdateContent updateContent = new UpdateContent();
		updateContent.IdUrlMap = new Dictionary<(int, int), string>();
		int num = text.IndexOf("%%%");
		if (num < 0)
		{
			updateContent.OutsideText = text.Trim();
			return updateContent;
		}
		updateContent.OutsideText = text.Substring(0, num).Trim();
		string[] array = text.Substring(num + "%%%".Length).Trim().Split(new char[2] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i].Trim();
			Debug.Log(text2);
			if (string.IsNullOrEmpty(text2))
			{
				continue;
			}
			if (text2.StartsWith(":"))
			{
				updateContent.DefaultUrl = text2.Substring(1).Trim();
				continue;
			}
			string[] array2 = text2.Split(':');
			if (array2.Length < 2)
			{
				Debug.LogWarning("格式错误：" + text2);
				continue;
			}
			if (!int.TryParse(array2[0], out var result))
			{
				Debug.LogWarning("id1 无效：" + text2);
				continue;
			}
			int result2 = 0;
			if (array2[1].Length > 0 && !int.TryParse(array2[1], out result2))
			{
				Debug.LogWarning("id2 无效：" + text2);
				continue;
			}
			string text3 = string.Join(":", array2.Skip(2));
			Debug.Log($"{result}:{result2}:{text3}");
			updateContent.IdUrlMap[(result, result2)] = text3;
		}
		return updateContent;
	}

	public static List<ContentBlock> ParseMixedContent(string input)
	{
		List<ContentBlock> list = new List<ContentBlock>();
		List<ContentBlock> list2 = new List<ContentBlock>();
		string pattern = "!\\[\\[data:image\\/\\w+;base64,([^]]+)\\]\\]";
		string pattern2 = "<a\\s+href=\"([^\"]+)\"[^>]*>([^<]+)</a>";
		foreach (Match item in Regex.Matches(input, pattern))
		{
			list2.Add(new ContentBlock
			{
				Index = item.Index,
				Length = item.Length,
				Type = ContentBlock.ContentType.Image,
				Content = item.Groups[1].Value,
				Content2 = null
			});
		}
		foreach (Match item2 in Regex.Matches(input, pattern2))
		{
			list2.Add(new ContentBlock
			{
				Index = item2.Index,
				Length = item2.Length,
				Type = ContentBlock.ContentType.Link,
				Content2 = item2.Groups[1].Value,
				Content = item2.Groups[2].Value
			});
		}
		list2 = list2.OrderBy((ContentBlock m) => m.Index).ToList();
		int num = 0;
		foreach (ContentBlock item3 in list2)
		{
			int index = item3.Index;
			if (index > num)
			{
				string text = input.Substring(num, index - num);
				if (!string.IsNullOrEmpty(text))
				{
					if (item3.Type == ContentBlock.ContentType.Link)
					{
						if (text.EndsWith("\r\n"))
						{
							text = text.Substring(0, text.Length - 2);
						}
						else if (text.EndsWith("\n"))
						{
							text = text.Substring(0, text.Length - 1);
						}
					}
					if (!string.IsNullOrEmpty(text))
					{
						list.Add(new ContentBlock
						{
							Type = ContentBlock.ContentType.Text,
							Content = text
						});
					}
				}
			}
			list.Add(new ContentBlock
			{
				Type = item3.Type,
				Content = item3.Content,
				Content2 = item3.Content2
			});
			num = index + item3.Length;
		}
		if (num < input.Length)
		{
			string text2 = input.Substring(num);
			if (!string.IsNullOrEmpty(text2))
			{
				Debug.Log("3");
				list.Add(new ContentBlock
				{
					Type = ContentBlock.ContentType.Text,
					Content = text2
				});
			}
		}
		return list;
	}
}
