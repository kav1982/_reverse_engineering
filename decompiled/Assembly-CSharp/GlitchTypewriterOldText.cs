using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GlitchTypewriterOldText : MonoBehaviour
{
	[Header("文本设置")]
	[SerializeField]
	public string originalText = "System Initialization Complete.";

	[Header("速度设置")]
	[Tooltip("每个字符完全定型所需的时间")]
	[SerializeField]
	private float revealSpeed = 0.05f;

	[Tooltip("乱码闪烁的频率（秒/次）")]
	[SerializeField]
	private float glitchSpeed = 0.02f;

	[SerializeField]
	[Header("高级设置")]
	[Tooltip("未定型字符前方保持多少个乱码字符同时闪烁")]
	private int glitchWindowSize = 1;

	[Tooltip("用于生成乱码的字符池")]
	private readonly string glitchChars = "!@#$%^&*()_+{}|:<>?-=[]\\;',./ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

	private Text uiText;

	private Coroutine typewriterCoroutine;

	private float typeSpeed;

	private void Awake()
	{
		uiText = GetComponent<Text>();
	}

	public void StartType(Action onTypeFinished, float _typeSpeed = 1f)
	{
		if ((object)uiText == null)
		{
			uiText = GetComponent<Text>();
		}
		uiText.text = "";
		if (typewriterCoroutine != null)
		{
			StopCoroutine(typewriterCoroutine);
		}
		typewriterCoroutine = StartCoroutine(PlayGlitchTypewriter(onTypeFinished));
		typeSpeed = _typeSpeed;
	}

	private void OnDisable()
	{
		if (typewriterCoroutine != null)
		{
			StopCoroutine(typewriterCoroutine);
		}
	}

	public bool IsEnglishLetter(string token)
	{
		if (token.Length == 1)
		{
			char c = token[0];
			if (c < 'a' || c > 'z')
			{
				if (c >= 'A')
				{
					return c <= 'Z';
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private List<string> TokenizeRichText(string text)
	{
		List<string> list = new List<string>();
		int num = 0;
		while (num < text.Length)
		{
			if (text[num] == '<')
			{
				int num2 = text.IndexOf('>', num);
				if (num2 != -1)
				{
					list.Add(text.Substring(num, num2 - num + 1));
					num = num2 + 1;
				}
				else
				{
					list.Add(text[num].ToString());
					num++;
				}
			}
			else
			{
				list.Add(text[num].ToString());
				num++;
			}
		}
		return list;
	}

	private bool IsRichTextTag(string token)
	{
		if (token.Length > 1 && token[0] == '<')
		{
			return token[token.Length - 1] == '>';
		}
		return false;
	}

	private IEnumerator PlayGlitchTypewriter(Action onTypeFinished)
	{
		List<string> tokens = TokenizeRichText(originalText);
		List<float> revealTimestamps = new List<float>();
		float num = 0f;
		foreach (string item in tokens)
		{
			if (!IsRichTextTag(item))
			{
				float num2 = (IsEnglishLetter(item) ? revealSpeed : (revealSpeed * 1.5f));
				num += num2;
				revealTimestamps.Add(num);
			}
		}
		int totalVisibleChars = revealTimestamps.Count;
		float elapsedTime = 0f;
		float lastGlitchTime = 0f;
		int revealedCharCount = 0;
		while (true)
		{
			for (elapsedTime += Time.unscaledDeltaTime * typeSpeed; revealedCharCount < totalVisibleChars && elapsedTime >= revealTimestamps[revealedCharCount]; revealedCharCount++)
			{
			}
			if (Time.unscaledTime - lastGlitchTime >= glitchSpeed)
			{
				lastGlitchTime = Time.unscaledTime;
				StringBuilder stringBuilder = new StringBuilder();
				int num3 = 0;
				int num4 = glitchWindowSize;
				foreach (string item2 in tokens)
				{
					if (IsRichTextTag(item2))
					{
						stringBuilder.Append(item2);
						continue;
					}
					if (num3 < revealedCharCount)
					{
						stringBuilder.Append(item2);
					}
					else if (num4 > 0)
					{
						stringBuilder.Append((item2 == " ") ? ' ' : glitchChars[UnityEngine.Random.Range(0, glitchChars.Length)]);
						num4--;
					}
					num3++;
				}
				uiText.text = stringBuilder.ToString();
			}
			if (revealedCharCount >= totalVisibleChars)
			{
				break;
			}
			yield return null;
		}
		uiText.text = originalText;
		onTypeFinished?.Invoke();
	}
}
