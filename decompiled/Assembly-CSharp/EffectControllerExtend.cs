using UnityEngine;

public static class EffectControllerExtend
{
	public static Gradient GetGradientWithTransparent(this Gradient self, float[] defaultAlphas, float transparent)
	{
		GradientAlphaKey[] array = new GradientAlphaKey[defaultAlphas.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new GradientAlphaKey(defaultAlphas[i] * transparent, self.alphaKeys[i].time);
		}
		Gradient gradient = new Gradient();
		gradient.SetKeys(self.colorKeys, array);
		return gradient;
	}
}
