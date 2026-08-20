using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class EffectController : LayerCorrect
{
	[Serializable]
	public class TransparentComponentController
	{
		public bool ParticleSystem = true;

		public bool SpriteRenderer = true;

		public bool LineRenderer = true;

		public bool TrailRenderer = true;

		public bool VisualEffect = true;
	}

	[Header("ColorTypeData")]
	public bool useECGOActice = true;

	public GameObject go_ECFrozen;

	public GameObject go_ECMonster;

	public GameObject go_ECMucus;

	public GameObject go_ECPlayer;

	public GameObject go_ECVenom;

	public GameObject go_ECStickFire;

	public GameObject go_ECStickThunder;

	public GameObject go_ECVoid;

	public List<ParticleSystem> ImmidiateStopAndClearList;

	[Header("Others")]
	protected bool playing;

	protected SpellColorType ecColorType;

	protected readonly List<ParticleSystem> ps_All = new List<ParticleSystem>();

	protected readonly List<VisualEffect> ve_All = new List<VisualEffect>();

	protected bool isInitialized;

	[Header("Spell Trasparent")]
	[SerializeField]
	protected bool UseTransparentControl = true;

	[Tooltip("大于 1 则受到透明度影响更强烈，小于 1 则不强烈")]
	[SerializeField]
	[Range(0f, 2f)]
	protected float TransparentControlFactor = 1f;

	public TransparentComponentController TransparentComponents = new TransparentComponentController();

	private bool _transparencyComponentIsInited;

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

	protected void ECInitialize()
	{
		if (!isInitialized)
		{
			isInitialized = true;
			ps_All.Clear();
			ps_All.AddRange(GetComponentsInChildren<ParticleSystem>(includeInactive: true));
			ve_All.Clear();
			ve_All.AddRange(GetComponentsInChildren<VisualEffect>(includeInactive: true));
		}
	}

	public virtual void ECStartEffect()
	{
		ECInitialize();
		if (playing)
		{
			return;
		}
		playing = true;
		foreach (ParticleSystem item in ps_All.Where((ParticleSystem e) => e.gameObject.activeInHierarchy))
		{
			item.Play();
		}
		foreach (VisualEffect item2 in ve_All.Where((VisualEffect e) => e.gameObject.activeInHierarchy))
		{
			item2.Play();
		}
	}

	public virtual void ECStopEffect()
	{
		ECInitialize();
		if (!playing)
		{
			return;
		}
		playing = false;
		foreach (ParticleSystem item in ps_All.Where((ParticleSystem e) => e.gameObject.activeInHierarchy))
		{
			if (!ImmidiateStopAndClearList.Contains(item))
			{
				item.Stop();
			}
			else
			{
				item.Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
		foreach (VisualEffect item2 in ve_All.Where((VisualEffect e) => e.gameObject.activeInHierarchy))
		{
			item2.Stop();
		}
	}

	protected virtual void CloseAllEffect()
	{
		if ((bool)go_ECPlayer)
		{
			go_ECPlayer.SetActive(value: false);
		}
		if ((bool)go_ECMonster)
		{
			go_ECMonster.SetActive(value: false);
		}
		if ((bool)go_ECMucus)
		{
			go_ECMucus.SetActive(value: false);
		}
		if ((bool)go_ECVenom)
		{
			go_ECVenom.SetActive(value: false);
		}
		if ((bool)go_ECFrozen)
		{
			go_ECFrozen.SetActive(value: false);
		}
		if ((bool)go_ECStickFire)
		{
			go_ECStickFire.SetActive(value: false);
		}
		if ((bool)go_ECStickThunder)
		{
			go_ECStickThunder.SetActive(value: false);
		}
		if ((bool)go_ECVoid)
		{
			go_ECVoid.SetActive(value: false);
		}
	}

	protected virtual GameObject GetColorObject(SpellColorType colorType)
	{
		GameObject gameObject = null;
		switch (ecColorType)
		{
		case SpellColorType.Player:
			gameObject = go_ECPlayer;
			break;
		case SpellColorType.Monster:
			gameObject = go_ECMonster;
			break;
		case SpellColorType.Mucus:
			gameObject = go_ECMucus;
			break;
		case SpellColorType.Venom:
			gameObject = go_ECVenom;
			break;
		case SpellColorType.Frozen:
			gameObject = go_ECFrozen;
			break;
		case SpellColorType.Fire:
			gameObject = go_ECStickFire;
			break;
		case SpellColorType.Thunder:
			gameObject = go_ECStickThunder;
			break;
		case SpellColorType.Void:
			gameObject = go_ECVoid;
			break;
		}
		if (gameObject != null)
		{
			return gameObject;
		}
		Debug.LogError("缺少对应染色的物体" + colorType);
		return go_ECPlayer;
	}

	public virtual void ECChangeColor(SpellColorType colorType)
	{
		ECInitialize();
		SetTransparency(DataMgr.settingData.FinalSpellTransparent);
		if (useECGOActice)
		{
			ecColorType = colorType;
			CloseAllEffect();
			GameObject colorObject = GetColorObject(colorType);
			if (colorObject != null)
			{
				colorObject.SetActive(value: true);
			}
		}
	}

	public virtual GameObject ECGetCurrentEffect()
	{
		switch (ecColorType)
		{
		case SpellColorType.Player:
			return go_ECPlayer;
		case SpellColorType.Monster:
			return go_ECMonster;
		case SpellColorType.Mucus:
			return go_ECMucus;
		case SpellColorType.Venom:
			return go_ECVenom;
		case SpellColorType.Frozen:
			return go_ECFrozen;
		case SpellColorType.Fire:
			return go_ECStickFire;
		case SpellColorType.Thunder:
			return go_ECStickThunder;
		case SpellColorType.Void:
			return go_ECVoid;
		default:
			Debug.LogError(ecColorType);
			return go_ECPlayer;
		}
	}

	public void ECRecycle(float delay = 0f)
	{
		if (base.gameObject != null && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(ECRecycleIE(delay));
		}
	}

	private IEnumerator ECRecycleIE(float delay = 0f)
	{
		yield return new WaitForSeconds(delay);
		StopCoroutine("ECStopEffectIE");
		if ((bool)go_ECStickFire)
		{
			go_ECStickFire.SetActive(value: false);
		}
		if ((bool)go_ECStickThunder)
		{
			go_ECStickThunder.SetActive(value: false);
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	private IEnumerator ECStopEffectIE(float delay)
	{
		yield return new WaitForSeconds(delay);
		ECStopEffect();
	}

	protected virtual void InitTransparencyComponent()
	{
		if (UseTransparentControl && !_transparencyComponentIsInited)
		{
			_transparencyComponentIsInited = true;
			if (TransparentComponents.ParticleSystem)
			{
				InitTransparencyParticleSystem();
			}
			if (TransparentComponents.SpriteRenderer)
			{
				InitTransparencySpriteRender();
			}
			if (TransparentComponents.LineRenderer)
			{
				InitTransparencyLineRender();
			}
			if (TransparentComponents.TrailRenderer)
			{
				InitTransparencyTrailRender();
			}
			if (TransparentComponents.VisualEffect)
			{
				InitTransparencyVisualEffect();
			}
			InitTransparencyCustom();
		}
	}

	protected virtual void InitTransparencyCustom()
	{
	}

	protected virtual void InitTransparencyParticleSystem()
	{
		_particleSystems.AddRange(GetComponents<ParticleSystem>());
		_particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>(includeInactive: true));
		foreach (ParticleSystem particleSystem in _particleSystems)
		{
			_particleSystemsDefaultColor.Add(particleSystem.main.startColor);
		}
	}

	protected virtual void InitTransparencySpriteRender()
	{
		_spriteRenders.AddRange(GetComponents<SpriteRenderer>());
		_spriteRenders.AddRange(GetComponentsInChildren<SpriteRenderer>(includeInactive: true));
		foreach (SpriteRenderer spriteRender in _spriteRenders)
		{
			_spriteRendersDefaultColor.Add(spriteRender.color);
		}
	}

	protected virtual void InitTransparencyLineRender()
	{
		_lineRenders.AddRange(GetComponents<LineRenderer>());
		_lineRenders.AddRange(GetComponentsInChildren<LineRenderer>(includeInactive: true));
		foreach (LineRenderer lineRender in _lineRenders)
		{
			_lineRendersDefaultAlpha.Add(lineRender.colorGradient.alphaKeys.Select((GradientAlphaKey e) => e.alpha).ToArray());
		}
	}

	protected virtual void InitTransparencyTrailRender()
	{
		_trailRenders.AddRange(GetComponents<TrailRenderer>());
		_trailRenders.AddRange(GetComponentsInChildren<TrailRenderer>(includeInactive: true));
		foreach (TrailRenderer trailRender in _trailRenders)
		{
			_trailRendersDefaultAlpha.Add(trailRender.colorGradient.alphaKeys.Select((GradientAlphaKey e) => e.alpha).ToArray());
		}
	}

	protected virtual void InitTransparencyVisualEffect()
	{
		List<VisualEffect> list = new List<VisualEffect>();
		list.AddRange(GetComponents<VisualEffect>());
		list.AddRange(GetComponentsInChildren<VisualEffect>(includeInactive: true));
		foreach (VisualEffect item in list.Where((VisualEffect ve) => ve.HasVector4("Color")))
		{
			_visualEffects.Add(item);
			_visualEffectsDefaultColor.Add(item.GetVector4("Color"));
		}
	}

	public virtual void UpdateTransparency()
	{
		if (ecColorType != SpellColorType.Monster && base.gameObject.activeSelf)
		{
			SetTransparency(DataMgr.settingData.FinalSpellTransparent);
		}
	}

	public virtual void SetTransparency(float transparency)
	{
		if (Math.Abs(transparency - _lastTransparent) < 0.01f)
		{
			return;
		}
		float value = Mathf.Pow(transparency, TransparentControlFactor);
		value = Mathf.Clamp(value, 0f, 1f);
		InitTransparencyComponent();
		if ((bool)tsf_Layer && UseTransparentControl)
		{
			if ((double)value <= 0.01)
			{
				ZeroTransparencyUnactiveTsf();
			}
			else
			{
				NoZeroTransparencyActiveTsf();
			}
		}
		if (!tsf_Layer || tsf_Layer.gameObject.activeSelf)
		{
			if (TransparentComponents.ParticleSystem)
			{
				SetParticleSystemTransparency(value);
			}
			if (TransparentComponents.SpriteRenderer)
			{
				SetSpriteRenderTransparency(value);
			}
			if (TransparentComponents.LineRenderer)
			{
				SetLineRenderTransparency(value);
			}
			if (TransparentComponents.TrailRenderer)
			{
				SetTrailRenderTransparency(value);
			}
			if (TransparentComponents.VisualEffect)
			{
				SetVisualEffectTransparency(value);
			}
			SetCustomTransparency(value);
		}
		_lastTransparent = transparency;
	}

	protected virtual void ZeroTransparencyUnactiveTsf()
	{
		tsf_Layer.gameObject.SetActive(value: false);
	}

	protected virtual void NoZeroTransparencyActiveTsf()
	{
		tsf_Layer.gameObject.SetActive(value: true);
	}

	protected virtual void SetParticleSystemTransparency(float transparency)
	{
		for (int i = 0; i < _particleSystems.Count; i++)
		{
			ParticleSystem.MainModule main = _particleSystems[i].main;
			switch (main.startColor.mode)
			{
			case ParticleSystemGradientMode.Color:
			{
				Color color = _particleSystemsDefaultColor[i].color;
				color.a *= transparency;
				main.startColor = color;
				break;
			}
			case ParticleSystemGradientMode.Gradient:
			case ParticleSystemGradientMode.RandomColor:
			{
				Gradient gradientWithTransparent = main.startColor.gradient.GetGradientWithTransparent(_particleSystemsDefaultColor[i].gradient.alphaKeys.Select((GradientAlphaKey e) => e.alpha).ToArray(), transparency);
				main.startColor = gradientWithTransparent;
				break;
			}
			case ParticleSystemGradientMode.TwoColors:
			{
				Color colorMin = _particleSystemsDefaultColor[i].colorMin;
				Color colorMax = _particleSystemsDefaultColor[i].colorMax;
				colorMin.a *= transparency;
				colorMax.a *= transparency;
				main.startColor = new ParticleSystem.MinMaxGradient(colorMin, colorMax);
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

	public override void OnEnable()
	{
		base.OnEnable();
		SetTransparency(DataMgr.settingData.FinalSpellTransparent);
		EventMgr.SpellTransparencyChange = (Action)Delegate.Combine(EventMgr.SpellTransparencyChange, new Action(UpdateTransparency));
	}

	public virtual void OnDisable()
	{
		EventMgr.SpellTransparencyChange = (Action)Delegate.Remove(EventMgr.SpellTransparencyChange, new Action(UpdateTransparency));
	}
}
