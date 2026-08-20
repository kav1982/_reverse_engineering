using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class UIDamageRecordBar : MonoBehaviour
{
	public float positionLerp = 0.15f;

	public Image icon;

	public Text damageText;

	public Image bar;

	private int index;

	private RectTransform rect;

	private void Start()
	{
		rect = GetComponent<RectTransform>();
	}

	private void Update()
	{
		UnityEngine.Vector2 anchoredPosition = rect.anchoredPosition;
		anchoredPosition.y = (0f - rect.sizeDelta.y) * (float)index;
		rect.anchoredPosition = UnityEngine.Vector2.Lerp(rect.anchoredPosition, anchoredPosition, positionLerp);
	}

	public void Initialize(int damageTypeId, int inRankIndex, BigInteger damage, BigInteger totalDamage)
	{
		index = inRankIndex;
		if (damageTypeId >= 1000)
		{
			icon.sprite = ABResources.LoadAsset<Sprite>("Textures/SpellIcons/" + damageTypeId);
		}
		else
		{
			icon.sprite = ABResources.LoadAsset<Sprite>("Textures/RelicIcons/" + damageTypeId);
		}
		bar.fillAmount = (float)damage / (float)totalDamage;
		string arg = damage.FormatWithUnit();
		damageText.text = $"{arg} ({(float)damage / (float)totalDamage * 100f:F0}%)";
		if (icon.sprite == null)
		{
			damageText.text += $" (?{damageTypeId})";
		}
	}
}
