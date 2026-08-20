using System.Text;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public static class StringHelper
{
	public static AudioSource PlaySE(this string str, SEPlayMode playMode = SEPlayMode.Replay, int maxASCount = 3, float SEPlayMinInterval = 0.05f)
	{
		return SEMgr.Inst.PlaySE(str, playMode, maxASCount, SEPlayMinInterval);
	}

	public static AudioSource PlayLoopSE(this string str, float duration)
	{
		return SEMgr.Inst.PlayLoopSE(str, duration);
	}

	public static AudioSource PlaySE(this string[] str, SEPlayMode playMode = SEPlayMode.Replay, int maxASCount = 3, float SEPlayMinInterval = 0.05f)
	{
		if (str.Length == 1)
		{
			return SEMgr.Inst.PlaySE(str[0], playMode, maxASCount, SEPlayMinInterval);
		}
		return SEMgr.Inst.PlaySE(str[Random.Range(0, str.Length)], playMode, maxASCount, SEPlayMinInterval * (float)str.Length);
	}

	public static AudioSource PlaySE(this string str, Vector3 point, SEPlayMode playMode = SEPlayMode.Replay, int maxASCount = 3, float SEPlayMinInterval = 0f)
	{
		return SEMgr.Inst.PlaySE(str, point, playMode, maxASCount, SEPlayMinInterval);
	}

	public static AudioSource PlaySE(this string[] str, Vector3 point, SEPlayMode playMode = SEPlayMode.Replay, int maxASCount = 3, float SEPlayMinInterval = 0f)
	{
		return SEMgr.Inst.PlaySE(str[Random.Range(0, str.Length)], point, playMode, maxASCount, SEPlayMinInterval);
	}

	public static string AddZeroSpaceIfLanguageNeed(this string text)
	{
		StringBuilder stringBuilder = new StringBuilder(text);
		if (DataMgr.settingData.language == LanguageType.ChineseS || DataMgr.settingData.language == LanguageType.ChineseT || DataMgr.settingData.language == LanguageType.Japanese)
		{
			stringBuilder = stringBuilder.Replace(",", ", ");
			stringBuilder = stringBuilder.Replace("、", "、 ");
			stringBuilder = stringBuilder.Replace(".", ". ");
			stringBuilder = stringBuilder.Replace("。", "。 ");
			stringBuilder = stringBuilder.Replace(",", ", ");
			stringBuilder = stringBuilder.Replace("，", "， ");
			return stringBuilder.ToString();
		}
		return text;
	}

	public static AudioSource PlaySE(this FixedString128Bytes str, SEPlayMode playMode = SEPlayMode.Replay, int maxASCount = 3, float SEPlayMinInterval = 0.05f)
	{
		return SEMgr.Inst.PlaySE(str, playMode, maxASCount, SEPlayMinInterval);
	}

	public static AudioSource PlaySE(this ref BlobArray<FixedString128Bytes> str, SEPlayMode playMode = SEPlayMode.Replay, int maxASCount = 3, float SEPlayMinInterval = 0.05f)
	{
		if (str.Length == 1)
		{
			return SEMgr.Inst.PlaySE(str[0], playMode, maxASCount, SEPlayMinInterval);
		}
		return SEMgr.Inst.PlaySE(str[Random.Range(0, str.Length)], playMode, maxASCount, SEPlayMinInterval * (float)str.Length);
	}

	public static AudioSource PlaySE(this FixedString128Bytes str, Vector3 point)
	{
		return SEMgr.Inst.PlaySE(str, point);
	}

	public static AudioSource PlaySE(this ref BlobArray<FixedString128Bytes> str, Vector3 point)
	{
		return SEMgr.Inst.PlaySE(str[Random.Range(0, str.Length)], point);
	}
}
