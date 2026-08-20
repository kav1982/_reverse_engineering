using Unity.Mathematics;
using UnityEngine;

public class WandSpirit : MonoBehaviour
{
	public SpriteRenderer WandSprite;

	public SpriteRenderer SpecialWandSprite;

	public Transform WandRotateTransform;

	private void Update()
	{
	}

	public void InitialWandDate(Wand wandData)
	{
		if (wandData.WandCfg == null)
		{
			return;
		}
		WandSprite.gameObject.SetActive(value: true);
		SpecialWandSprite.gameObject.SetActive(value: false);
		WandSprite.sprite = ABResources.LoadAsset<Sprite>(wandData.WandCfg.GetIconPath());
		if (wandData.WandCfg != null)
		{
			WandAbility specialAbility = wandData.WandCfg.specialAbility;
			if (specialAbility == WandAbility.LongWand || specialAbility == WandAbility.LongWandAndSpellBreaker || GameConstManaged.SpecialLongWandIdList.Contains(wandData.WandCfg.id))
			{
				WandSprite.gameObject.SetActive(value: false);
				SpecialWandSprite.gameObject.SetActive(value: true);
				SpecialWandSprite.sprite = ABResources.LoadAsset<Sprite>(wandData.WandCfg.GetIconPath() + "L");
			}
		}
	}

	public void UpdateLookDirection(float3 lookDirection)
	{
		WandRotateTransform.transform.right = lookDirection;
	}
}
