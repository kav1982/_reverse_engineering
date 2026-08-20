using System.Collections.Generic;
using UnityEngine;

public class Spell4013HammerTransparencyController : EffectTransparencyController
{
	public List<SpriteRenderer> HammerBorderSpriteRenderers;

	private float bonusTransparencyRatio = 1f;

	private float baseTransparency = 1f;

	protected void OnEnable()
	{
		bonusTransparencyRatio = 1f;
		baseTransparency = 1f;
	}

	public void SetHammerBonusTransparencyRatio(float ratio)
	{
		bonusTransparencyRatio = ratio;
	}

	protected override void SetSpriteRenderTransparency(float transparency)
	{
		base.SetSpriteRenderTransparency(transparency);
		baseTransparency = transparency;
	}
}
