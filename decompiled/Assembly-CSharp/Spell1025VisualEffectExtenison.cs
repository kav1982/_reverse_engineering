using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.VFX;

public static class Spell1025VisualEffectExtenison
{
	public static TweenerCore<float, float, FloatOptions> DoFloat(this VisualEffect ve, string name, float value, float time)
	{
		return DOTween.To(() => ve.GetFloat(name), delegate(float v)
		{
			ve.SetFloat(name, v);
		}, value, time);
	}
}
