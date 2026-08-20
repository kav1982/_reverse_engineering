using DG.Tweening;
using UnityEngine;

public class SpellTraceTransparentController : MonoBehaviour
{
	public bool ControlByTransparent;

	public SpellTraceEffectSettings[] Settings;

	public bool RandomFilpX;

	public bool RandomFilpY;

	public bool RandomRotation;

	private void Awake()
	{
		SpellTraceEffectSettings[] settings = Settings;
		foreach (SpellTraceEffectSettings obj in settings)
		{
			obj.DefaultAlpha = obj.Renderer.color.a;
		}
	}

	private void OnEnable()
	{
		SpellTraceEffectSettings[] settings = Settings;
		foreach (SpellTraceEffectSettings s in settings)
		{
			StartFade(s);
		}
		if (RandomRotation)
		{
			base.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
		}
		if (RandomFilpX)
		{
			bool flipX = Random.Range(0, 2) == 0;
			settings = Settings;
			for (int i = 0; i < settings.Length; i++)
			{
				settings[i].Renderer.flipX = flipX;
			}
		}
		if (RandomFilpY)
		{
			bool flipY = Random.Range(0, 2) == 0;
			settings = Settings;
			for (int i = 0; i < settings.Length; i++)
			{
				settings[i].Renderer.flipY = flipY;
			}
		}
	}

	private void StartFade(SpellTraceEffectSettings s)
	{
		Color color = s.Renderer.color;
		color.a = s.DefaultAlpha;
		if (ControlByTransparent)
		{
			color.a *= DataMgr.settingData.FinalSpellTransparent;
		}
		s.Renderer.color = color;
		Color endValue = color;
		endValue.a = 0f;
		if (s.AlphaCurve != null && s.AlphaCurve.keys.Length >= 2)
		{
			s.Renderer.DOColor(endValue, s.FadeTime).SetEase(s.AlphaCurve);
		}
		else
		{
			s.Renderer.DOColor(endValue, s.FadeTime);
		}
	}
}
