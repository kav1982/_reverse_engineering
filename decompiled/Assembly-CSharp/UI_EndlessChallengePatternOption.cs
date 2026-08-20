using UnityEngine;
using UnityEngine.UI;

public class UI_EndlessChallengePatternOption : MonoBehaviour
{
	public Image Icon;

	public Text Text_Name;

	public UI_EndlessChallengePatternSlideBar SlideBar;

	public void SetIcon(Sprite sprite)
	{
		Icon.sprite = sprite;
	}

	public void SetName(string name)
	{
		Text_Name.text = name;
	}

	public void InitSlideBar(int currentLevel, int maxLevel)
	{
		SlideBar.InitBarState(currentLevel, maxLevel);
	}
}
