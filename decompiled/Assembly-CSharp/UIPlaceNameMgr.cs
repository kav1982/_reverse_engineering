using UnityEngine;
using UnityEngine.UI;

public class UIPlaceNameMgr : MonoBehaviour
{
	public Animator anima;

	public Text text;

	public static UIPlaceNameMgr Inst { get; private set; }

	public void Initialize()
	{
		Inst = this;
	}

	public void Show(PlaceNameType nameType)
	{
		switch (nameType)
		{
		case PlaceNameType.Camp:
			text.text = 1001701.GetText();
			break;
		case PlaceNameType.Chapter1:
			text.text = 1001702.GetText();
			break;
		case PlaceNameType.Chapter2:
			text.text = 1001703.GetText();
			break;
		case PlaceNameType.Chapter3:
			text.text = 1001704.GetText();
			break;
		case PlaceNameType.Chapter4:
			text.text = 1001705.GetText();
			break;
		case PlaceNameType.Chapter5:
			text.text = 1001706.GetText();
			break;
		case PlaceNameType.Endless:
			text.text = 1001707.GetText();
			break;
		default:
			Debug.LogError(nameType);
			break;
		}
		anima.SetTrigger("Show");
	}

	public void HideDirect()
	{
		anima.SetTrigger("Idle");
	}
}
