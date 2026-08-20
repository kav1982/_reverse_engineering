using UnityEngine;
using UnityEngine.UI;

public class UI_EndlessChallengePatternSlideBar : MonoBehaviour
{
	public Image ProgressBar;

	public Slider BarSlider;

	public HorizontalLayoutGroup Blockers;

	public GameObject BlockObj;

	public GameObject BlockParent;

	[HideInInspector]
	public int BarCurrentLevel;

	[HideInInspector]
	public int BarMaxLevel;

	public void InitBarState(int currentLevel, int maxLevel)
	{
		BarCurrentLevel = currentLevel;
		BarMaxLevel = maxLevel;
		Blockers.spacing = ((BarMaxLevel <= 1) ? 0f : ((498f - BlockObj.GetComponent<RectTransform>().rect.width * (float)BarMaxLevel) / (float)BarMaxLevel));
		for (int num = BlockParent.transform.childCount - 1; num >= 0; num--)
		{
			Object.Destroy(BlockParent.transform.GetChild(num).gameObject);
		}
		for (int i = 0; i < BarMaxLevel - 1; i++)
		{
			Object.Instantiate(BlockObj, BlockParent.transform, worldPositionStays: true).GetComponent<RectTransform>().localScale = Vector3.one;
		}
		ProgressBar.fillAmount = ((maxLevel > 0) ? ((float)currentLevel / (float)maxLevel) : 0f);
		BarSlider.value = 0f;
		BarSlider.onValueChanged.AddListener(UpdateSliderBarVisualEffect);
	}

	private void UpdateSliderBarVisualEffect(float value)
	{
		float num = 1f / (float)BarMaxLevel;
		ProgressBar.fillAmount = ((value <= num / 2f) ? 0f : ((float)Mathf.CeilToInt(value / num) * num));
	}
}
