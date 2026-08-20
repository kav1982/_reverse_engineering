using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public bool Clicked;

	private int id;

	public Text DeveloperName;

	public Text Title;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (Clicked)
		{
			Clicked = false;
			DeveloperName.text = id.GetText().Split("@n")[0];
		}
		else
		{
			Clicked = true;
			DeveloperName.text = id.GetText();
			GeneralTool.TextFormat(DeveloperName, 10);
		}
	}

	public void Initialize(int _id)
	{
		id = _id;
		Clicked = false;
		DeveloperName.text = id.GetText().Split("@n")[0];
		Title.text = (id - 1).GetText();
	}
}
