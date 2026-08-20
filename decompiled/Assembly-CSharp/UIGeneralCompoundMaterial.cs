using System;
using UnityEngine;
using UnityEngine.UI;

public class UIGeneralCompoundMaterial : MonoBehaviour
{
	public GameObject star1;

	public GameObject star2;

	public Text GeneralMaterialCountText;

	public RectTransform Cover;

	public RectTransform CenterRectTransform;

	[HideInInspector]
	public int CoverIndex = 2;

	public GameObject pfb_UIRecastSpell;

	public RectTransform SpellIconRectTransform;

	public Text UseCountText;

	public event Action OnUseCountChangeEvent;

	public void BoardToggle(bool toggle)
	{
		CenterRectTransform.gameObject.SetActive(toggle);
	}

	public void SetGeneralMaterialLevel(int level, int count)
	{
		SetMaterialStarSprite(level);
		SetMaterialCount(count);
	}

	public void SetMaterialStarSprite(int level)
	{
		star1.SetActive(value: false);
		star2.SetActive(value: false);
		switch (level)
		{
		case 2:
			star1.SetActive(value: true);
			break;
		case 3:
			star1.SetActive(value: true);
			star2.SetActive(value: true);
			break;
		}
	}

	public void SetMaterialCount(int Count)
	{
		GeneralMaterialCountText.text = "x" + Count;
	}

	public void SetCoverIndex(int useCount)
	{
		CoverIndex = useCount;
		if (this.OnUseCountChangeEvent != null)
		{
			this.OnUseCountChangeEvent();
		}
		Cover.localPosition = new Vector3(44 * (useCount - 1), 15f, 0f);
	}

	public void SetUseCount(int count)
	{
		UseCountText.text = count.ToString();
	}

	public void SpawnFlyUir(UIReroll_Spell targetSpell)
	{
		UIReroll_Spell component = UnityEngine.Object.Instantiate(pfb_UIRecastSpell, SpellIconRectTransform).GetComponent<UIReroll_Spell>();
		component.Initialize(0, 40201, 1);
		component.Fly(targetSpell);
		component.flyOutlookOnly = true;
		UnityEngine.Object.Destroy(component, 0.5f);
	}
}
