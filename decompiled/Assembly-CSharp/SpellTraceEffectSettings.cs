using System;
using UnityEngine;

[Serializable]
public class SpellTraceEffectSettings
{
	public SpriteRenderer Renderer;

	public float FadeTime;

	public AnimationCurve AlphaCurve;

	[HideInInspector]
	public float DefaultAlpha;
}
