using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITranslatedText : MonoBehaviour
{
	public int textId;

	public string replacePattern;

	public string replaceText;

	public string prefix;

	public string postfix;

	private Text _text;

	private void Start()
	{
		_text = GetComponent<Text>();
		FlashText();
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(FlashText));
	}

	private void OnDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(FlashText));
	}

	private void FlashText()
	{
		string text = textId.GetText();
		if (!string.IsNullOrEmpty(replacePattern))
		{
			text = text.Replace(replacePattern, replaceText);
		}
		text = prefix + text + postfix;
		_text.text = text;
	}
}
