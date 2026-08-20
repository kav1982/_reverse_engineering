using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class EffectTransparencyController : MonoBehaviour
{
	[Serializable]
	private class ComponentFilterSettings
	{
		public bool ParticleSystem = true;

		public bool SpriteRenderer = true;

		public bool LineRenderer = true;

		public bool TrailRenderer = true;

		public bool VisualEffect = true;
	}

	public enum ControlMode
	{
		Spell,
		Summon,
		Min,
		Max
	}

	[Tooltip("大于 1 则受到透明度影响更强烈，小于 1 则不强烈")]
	public float TransparentControlFactor = 1f;

	[Tooltip("控制模式，设定其受到哪个透明度参数影响。")]
	public ControlMode Mode;

	[Tooltip("勾上之后就强制不透明了，用于动态开关透明度")]
	public bool ForceNoTransparent;

	[Tooltip("为了性能，不要勾选多余的选项")]
	[SerializeField]
	private ComponentFilterSettings ComponentFilter;

	private float _lastTransparent = 1f;

	private readonly List<ParticleSystem> _particleSystems = new List<ParticleSystem>();

	private readonly List<ParticleSystem.MinMaxGradient> _particleSystemsDefaultColor = new List<ParticleSystem.MinMaxGradient>();

	private readonly List<SpriteRenderer> _spriteRenders = new List<SpriteRenderer>();

	private readonly List<Color> _spriteRendersDefaultColor = new List<Color>();

	private readonly List<LineRenderer> _lineRenders = new List<LineRenderer>();

	private readonly List<float[]> _lineRendersDefaultAlpha = new List<float[]>();

	private readonly List<TrailRenderer> _trailRenders = new List<TrailRenderer>();

	private readonly List<float[]> _trailRendersDefaultAlpha = new List<float[]>();

	private readonly List<VisualEffect> _visualEffects = new List<VisualEffect>();

	private readonly List<Color> _visualEffectsDefaultColor = new List<Color>();

	private static readonly int ColorID = Shader.PropertyToID("Color");

	private void Awake()
	{
		InitTransparencyComponent();
		UpdateTransparent();
	}

	private void OnEnable()
	{
		UpdateTransparent();
	}

	private void Update()
	{
		UpdateTransparent();
	}

	public void UpdateTransparent()
	{
		if (ForceNoTransparent)
		{
			SetTransparency(1f);
		}
		else
		{
			SetTransparency(Mode.GetTransparency());
		}
	}

	private void SetTransparency(float transparency)
	{
		if ((int)(transparency * 1000f) != (int)(_lastTransparent * 1000f))
		{
			_lastTransparent = transparency;
			float value = Mathf.Pow(transparency, TransparentControlFactor);
			value = Mathf.Clamp(value, 0f, 1f);
			if (ComponentFilter.ParticleSystem)
			{
				SetParticleSystemTransparency(value);
			}
			if (ComponentFilter.SpriteRenderer)
			{
				SetSpriteRenderTransparency(value);
			}
			if (ComponentFilter.LineRenderer)
			{
				SetLineRenderTransparency(value);
			}
			if (ComponentFilter.TrailRenderer)
			{
				SetTrailRenderTransparency(value);
			}
			if (ComponentFilter.VisualEffect)
			{
				SetVisualEffectTransparency(value);
			}
			SetCustomTransparency(value);
		}
	}

	private void InitTransparencyComponent()
	{
		if (ComponentFilter.ParticleSystem)
		{
			InitTransparencyParticleSystem();
		}
		if (ComponentFilter.SpriteRenderer)
		{
			InitTransparencySpriteRender();
		}
		if (ComponentFilter.LineRenderer)
		{
			InitTransparencyLineRender();
		}
		if (ComponentFilter.TrailRenderer)
		{
			InitTransparencyTrailRender();
		}
		if (ComponentFilter.VisualEffect)
		{
			InitTransparencyVisualEffect();
		}
		InitTransparencyCustom();
	}

	protected virtual void InitTransparencyParticleSystem()
	{
		_particleSystems.AddRange(GetComponents<ParticleSystem>());
		_particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>(includeInactive: true));
		_particleSystemsDefaultColor.AddRange(_particleSystems.Select((ParticleSystem e) => e.main.startColor));
	}

	protected virtual void InitTransparencySpriteRender()
	{
		_spriteRenders.AddRange(GetComponents<SpriteRenderer>());
		_spriteRenders.AddRange(GetComponentsInChildren<SpriteRenderer>(includeInactive: true));
		_spriteRendersDefaultColor.AddRange(_spriteRenders.Select((SpriteRenderer e) => e.color));
	}

	protected virtual void InitTransparencyLineRender()
	{
		_lineRenders.AddRange(GetComponents<LineRenderer>());
		_lineRenders.AddRange(GetComponentsInChildren<LineRenderer>(includeInactive: true));
		_lineRendersDefaultAlpha.AddRange(_lineRenders.Select((LineRenderer lr) => lr.colorGradient.alphaKeys.Select((GradientAlphaKey e) => e.alpha).ToArray()));
	}

	protected virtual void InitTransparencyTrailRender()
	{
		_trailRenders.AddRange(GetComponents<TrailRenderer>());
		_trailRenders.AddRange(GetComponentsInChildren<TrailRenderer>(includeInactive: true));
		_trailRendersDefaultAlpha.AddRange(_trailRenders.Select((TrailRenderer e) => e.colorGradient.alphaKeys.Select((GradientAlphaKey e) => e.alpha).ToArray()));
	}

	protected virtual void InitTransparencyVisualEffect()
	{
		List<VisualEffect> list = new List<VisualEffect>();
		list.AddRange(GetComponents<VisualEffect>());
		list.AddRange(GetComponentsInChildren<VisualEffect>(includeInactive: true));
		foreach (VisualEffect item in list.Where((VisualEffect ve) => ve.HasVector4(ColorID)))
		{
			_visualEffects.Add(item);
			_visualEffectsDefaultColor.Add(item.GetVector4(ColorID));
		}
	}

	protected virtual void InitTransparencyCustom()
	{
	}

	protected virtual void SetParticleSystemTransparency(float transparency)
	{
		for (int i = 0; i < _particleSystems.Count; i++)
		{
			ParticleSystem.MainModule main = _particleSystems[i].main;
			ParticleSystem.MinMaxGradient minMaxGradient = _particleSystemsDefaultColor[i];
			switch (main.startColor.mode)
			{
			case ParticleSystemGradientMode.Color:
			{
				Color color = minMaxGradient.color;
				color.a *= transparency;
				main.startColor = color;
				break;
			}
			case ParticleSystemGradientMode.Gradient:
			case ParticleSystemGradientMode.RandomColor:
			{
				Gradient gradientWithTransparent3 = main.startColor.gradient.GetGradientWithTransparent(minMaxGradient.gradient.alphaKeys.Select((GradientAlphaKey e) => e.alpha).ToArray(), transparency);
				main.startColor = gradientWithTransparent3;
				break;
			}
			case ParticleSystemGradientMode.TwoColors:
			{
				Color colorMin = minMaxGradient.colorMin;
				Color colorMax = minMaxGradient.colorMax;
				colorMin.a *= transparency;
				colorMax.a *= transparency;
				main.startColor = new ParticleSystem.MinMaxGradient(colorMin, colorMax);
				break;
			}
			case ParticleSystemGradientMode.TwoGradients:
			{
				Gradient gradientWithTransparent = main.startColor.gradientMin.GetGradientWithTransparent(minMaxGradient.gradientMin.alphaKeys.Select((GradientAlphaKey e) => e.alpha).ToArray(), transparency);
				Gradient gradientWithTransparent2 = main.startColor.gradientMax.GetGradientWithTransparent(minMaxGradient.gradientMax.alphaKeys.Select((GradientAlphaKey e) => e.alpha).ToArray(), transparency);
				main.startColor = new ParticleSystem.MinMaxGradient(gradientWithTransparent, gradientWithTransparent2);
				break;
			}
			default:
				Debug.LogError($"暂不支持处理 {main.startColor.mode} 这种颜色模式的透明度，({base.gameObject})");
				break;
			}
		}
	}

	protected virtual void SetSpriteRenderTransparency(float transparency)
	{
		for (int i = 0; i < _spriteRenders.Count; i++)
		{
			SpriteRenderer spriteRenderer = _spriteRenders[i];
			Color color = _spriteRendersDefaultColor[i];
			color.a *= transparency;
			spriteRenderer.color = color;
		}
	}

	protected virtual void SetLineRenderTransparency(float transparency)
	{
		for (int i = 0; i < _lineRenders.Count; i++)
		{
			_lineRenders[i].colorGradient = _lineRenders[i].colorGradient.GetGradientWithTransparent(_lineRendersDefaultAlpha[i], transparency);
		}
	}

	protected virtual void SetTrailRenderTransparency(float transparency)
	{
		for (int i = 0; i < _trailRenders.Count; i++)
		{
			_trailRenders[i].colorGradient = _trailRenders[i].colorGradient.GetGradientWithTransparent(_trailRendersDefaultAlpha[i], transparency);
		}
	}

	protected virtual void SetVisualEffectTransparency(float transparency)
	{
		for (int i = 0; i < _visualEffects.Count; i++)
		{
			VisualEffect visualEffect = _visualEffects[i];
			Color color = _visualEffectsDefaultColor[i];
			color.a *= transparency;
			visualEffect.SetVector4("Color", color);
		}
	}

	protected virtual void SetCustomTransparency(float transparency)
	{
	}
}
