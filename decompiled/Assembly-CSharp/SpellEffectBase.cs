using System;
using System.Collections.Generic;
using System.Linq;
using SpriteEffectSystem;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(SpellBase))]
public class SpellEffectBase : MonoBehaviour
{
	[Serializable]
	public class PerformanceSettings
	{
		[Tooltip("????????????β")]
		public bool ClearTrailRender;

		[Tooltip("???????????????????????????????????????????????д??????????????????")]
		public bool CanShootFromMonster;
	}

	public enum FallingExplosionEffectSetting
	{
		CommonFall,
		CustomPlayerColor,
		CustomAllColor,
		CustomAllInOneColor,
		None
	}

	[Tooltip("?????? ID???????????Ч??")]
	public string EffectID;

	[Tooltip("??Ч?б?")]
	public SpellEffectSettings[] Effects = Array.Empty<SpellEffectSettings>();

	[Tooltip("???????Ч?б?????Щ??Ч???????λ??????????????Ч????????Ч??????????????")]
	public SpellSpriteEffectSettings[] SpriteEffects = Array.Empty<SpellSpriteEffectSettings>();

	[Tooltip("?????????????????????????????????")]
	public PerformanceSettings Performance = new PerformanceSettings();

	public bool UpdateInFixed;

	public bool IgnoreEffectCreateWhenFullTransparency = true;

	[Tooltip("??????????????Ч??????????????????????Ч???????????")]
	public FallingExplosionEffectSetting FallingExplosionEffectMode;

	public string FallingGroundSound = "Hit";

	protected readonly List<(Transform trans, SpellEffectSettings effect)> CurrentEffects = new List<(Transform, SpellEffectSettings)>();

	protected bool firstUpdateIsRun;

	public static bool FullTransparency => (DataMgr.settingData?.SpellTransparent ?? 1f) <= 0.01f;

	protected SpellBase Spell { get; private set; }

	public bool FirstFrameIsRun { get; private set; }

	public bool ShootFromPlayerOrTeammate
	{
		get
		{
			if ((bool)Spell.ownerPpt)
			{
				return Spell.ownerPpt.gameObject.CompareAnyTag("Player", "Teammate");
			}
			return false;
		}
	}

	protected virtual void Awake()
	{
		Spell = GetComponent<SpellBase>();
		Spell.OnWillRecycleIgnoreRecycle.Listen(OnSpellRecycle);
		Spell.OnFlyFinishIgnoreRecycle.Listen(OnSpellFlyFinish);
	}

	protected virtual void OnEnable()
	{
		FirstFrameIsRun = false;
		firstUpdateIsRun = false;
		SpawnEffect(Effects.Where((SpellEffectSettings e) => e.CreateTiming == SpellEffectSettings.CreateTimingType.OnEnable));
		CreateSpriteEffect(SpriteEffects.Where((SpellSpriteEffectSettings e) => e.CreateTiming == SpellSpriteEffectSettings.CreateTimingType.OnEnable));
	}

	protected virtual void EffectUpdate()
	{
		if (!FirstFrameIsRun && firstUpdateIsRun)
		{
			SpawnEffect(Effects.Where((SpellEffectSettings e) => e.CreateTiming == SpellEffectSettings.CreateTimingType.OnFirstFrame));
			CreateSpriteEffect(SpriteEffects.Where((SpellSpriteEffectSettings e) => e.CreateTiming == SpellSpriteEffectSettings.CreateTimingType.OnFirstFrame));
			OnFirstFrame();
			FirstFrameIsRun = true;
		}
		for (int i = 0; i < CurrentEffects.Count; i++)
		{
			(Transform, SpellEffectSettings) tuple = CurrentEffects[i];
			if (tuple.Item2.PositionMode != SpellEffectSettings.PositionType.Manual)
			{
				UpdatePosition(tuple.Item1, tuple.Item2);
			}
			if (tuple.Item2.RotationMode != 0)
			{
				UpdateRotation(tuple.Item1, tuple.Item2);
			}
			if (tuple.Item2.ScaleMode != SpellEffectSettings.ScaleType.Manual)
			{
				UpdateScale(tuple.Item1, tuple.Item2);
			}
		}
	}

	protected virtual void Update()
	{
		firstUpdateIsRun = true;
		if (!UpdateInFixed)
		{
			EffectUpdate();
		}
	}

	protected virtual void FixedUpdate()
	{
		if (UpdateInFixed)
		{
			EffectUpdate();
		}
	}

	public void ManualCreateEffect(string effectName, float? scale = null, Vector3? position = null, Vector3? rotation = null)
	{
		SpellEffectSettings[] array = Effects.Where((SpellEffectSettings e) => e.CreateTiming == SpellEffectSettings.CreateTimingType.Manual && e.Name == effectName).ToArray();
		if (array.Length == 0)
		{
			Debug.LogWarning("??????????????Ч?????" + effectName);
		}
		else
		{
			SpawnEffect(array, scale, position, rotation);
		}
	}

	public void CreateSpriteEffect(string effectName, Vector3? position = null, Quaternion? rotation = null, float? size = null)
	{
		if (!FullTransparency || !IgnoreEffectCreateWhenFullTransparency || !ShootFromPlayerOrTeammate)
		{
			IEnumerable<SpellSpriteEffectSettings> effects = SpriteEffects.Where((SpellSpriteEffectSettings e) => e.Name == effectName && e.CreateTiming == SpellSpriteEffectSettings.CreateTimingType.Manual);
			CreateSpriteEffect(effects, position, rotation, size);
		}
	}

	public void ManualRecycleEffect(string effectName)
	{
		RecycleEffectWhere((SpellEffectSettings e) => e.Name == effectName);
	}

	public void FlushColor()
	{
		int count = CurrentEffects.Count;
		List<(Transform, SpellEffectSettings)> list = new List<(Transform, SpellEffectSettings)>();
		for (int i = 0; i < count; i++)
		{
			SpellEffectSettings item = CurrentEffects[0].effect;
			Transform item2 = CurrentEffects[0].trans;
			Transform transform = GetEffectGoFromPool(item).transform;
			transform.SetPositionAndRotation(item2.position, item2.rotation);
			transform.localScale = item2.localScale;
			RecycleEffect(0);
			list.Add((transform, item));
			OnChangeColor(item, transform);
		}
		CurrentEffects.AddRange(list);
	}

	private void CreateSpriteEffect(IEnumerable<SpellSpriteEffectSettings> effects, Vector3? position = null, Quaternion? rotation = null, float? size = null)
	{
		foreach (SpellSpriteEffectSettings effect in effects)
		{
			SpriteEffectAnima randomSpriteEffectAnima = GetRandomSpriteEffectAnima(effect);
			EffectPlayParam spriteEffectPlayParam = GetSpriteEffectPlayParam(effect, position, rotation, size);
			if ((bool)randomSpriteEffectAnima.material)
			{
				spriteEffectPlayParam.Material = randomSpriteEffectAnima.material;
			}
			SpellSpriteEffectController.Inst.PlayEffect(randomSpriteEffectAnima, spriteEffectPlayParam);
		}
	}

	private void OnSpellFlyFinish(SpellBase _)
	{
		RecycleEffectWhere((SpellEffectSettings e) => e.RecycleTiming == SpellEffectSettings.RecycleTimingType.OnFlyFinish);
		SpawnEffect(Effects.Where((SpellEffectSettings e) => e.CreateTiming == SpellEffectSettings.CreateTimingType.OnFlyFinish));
		CreateSpriteEffect(SpriteEffects.Where((SpellSpriteEffectSettings e) => e.CreateTiming == SpellSpriteEffectSettings.CreateTimingType.OnFlyFinish));
	}

	private void OnSpellRecycle(SpellBase _)
	{
		RecycleEffectWhere(delegate(SpellEffectSettings e)
		{
			SpellEffectSettings.RecycleTimingType recycleTiming = e.RecycleTiming;
			return recycleTiming == SpellEffectSettings.RecycleTimingType.OnRecycle || recycleTiming == SpellEffectSettings.RecycleTimingType.OnFlyFinish;
		});
		SpawnEffect(Effects.Where((SpellEffectSettings e) => e.CreateTiming == SpellEffectSettings.CreateTimingType.OnRecycle));
		CreateSpriteEffect(SpriteEffects.Where((SpellSpriteEffectSettings e) => e.CreateTiming == SpellSpriteEffectSettings.CreateTimingType.OnRecycle));
	}

	private void UpdatePosition(IEnumerable<(Transform, SpellEffectSettings)> effects)
	{
		effects.Action(delegate((Transform, SpellEffectSettings) e)
		{
			UpdatePosition(e.Item1, e.Item2);
		});
	}

	protected virtual void UpdatePosition(Transform trans, SpellEffectSettings effect)
	{
		if (UpdateInFixed)
		{
			Vector3 position = effect.AttachTarget.position;
			if ((bool)Spell.rigid)
			{
				position += Spell.rigid.linearVelocity * Time.deltaTime;
			}
			trans.position = position;
		}
		else
		{
			trans.position = effect.AttachTarget.position;
		}
	}

	private void UpdateRotation(IEnumerable<(Transform, SpellEffectSettings)> effects)
	{
		effects.Action(delegate((Transform, SpellEffectSettings) e)
		{
			UpdateRotation(e.Item1, e.Item2);
		});
	}

	protected virtual void UpdateRotation(Transform trans, SpellEffectSettings effect)
	{
		switch (effect.RotationMode)
		{
		case SpellEffectSettings.RotationType.LookDirection:
		{
			float z = Vector2.SignedAngle(to: new Vector2(Spell.Direction.x * Spell.CurrentSpeed, Spell.CurrentUpSpeed + Spell.Direction.y * Spell.CurrentSpeed), from: Vector2.right);
			Quaternion quaternion2 = (trans.rotation = Quaternion.Euler(0f, 0f, z));
			break;
		}
		case SpellEffectSettings.RotationType.TargetRotation:
			trans.rotation = effect.AttachTarget.rotation;
			break;
		default:
			trans.rotation = trans.rotation;
			break;
		}
	}

	private void UpdateScale(IEnumerable<(Transform, SpellEffectSettings)> effects)
	{
		effects.Action(delegate((Transform, SpellEffectSettings) e)
		{
			UpdateScale(e.Item1, e.Item2);
		});
	}

	protected virtual void UpdateScale(Transform trans, SpellEffectSettings effect)
	{
		trans.localScale = effect.ScaleMode switch
		{
			SpellEffectSettings.ScaleType.TargetLocalScale => effect.AttachTarget.localScale, 
			SpellEffectSettings.ScaleType.TargetLossyScale => effect.AttachTarget.lossyScale, 
			_ => trans.localScale, 
		};
	}

	private void RecycleEffectWhere(Func<SpellEffectSettings, bool> rule)
	{
		foreach (int item in (from e in CurrentEffects.Select(((Transform trans, SpellEffectSettings effect) e, int i) => (e, i))
			where rule(e.e.effect)
			select e.i).ToArray().Reverse())
		{
			RecycleEffect(item);
		}
	}

	private void RecycleEffect(int index)
	{
		var (transform, spellEffectSettings) = CurrentEffects[index];
		if (spellEffectSettings.RecycleDelay <= 0f)
		{
			ObjPoolMgr.Inst.RecycleGO(transform.gameObject);
		}
		else
		{
			ParticleSystem[] allParticleSystem = GetAllParticleSystem(transform.gameObject);
			for (int i = 0; i < allParticleSystem.Length; i++)
			{
				allParticleSystem[i].Stop();
			}
			ObjPoolMgr.Inst.RecycleGO(transform.gameObject, spellEffectSettings.RecycleDelay);
		}
		CurrentEffects.RemoveAt(index);
		OnWillRecycleEffect(spellEffectSettings, transform);
	}

	protected void RecycleEffect(Transform trans)
	{
		for (int i = 0; i < CurrentEffects.Count; i++)
		{
			if (!(CurrentEffects[i].trans != trans))
			{
				RecycleEffect(i);
				break;
			}
		}
	}

	private void SpawnEffect(IEnumerable<SpellEffectSettings> effects, float? overrideScale = null, Vector3? overridePosition = null, Vector3? overrideDirection = null)
	{
		if (FullTransparency && IgnoreEffectCreateWhenFullTransparency && ShootFromPlayerOrTeammate)
		{
			return;
		}
		foreach (SpellEffectSettings effect in effects)
		{
			GameObject effectGoFromPool = GetEffectGoFromPool(effect);
			if (effect.RecycleTiming == SpellEffectSettings.RecycleTimingType.OnStart)
			{
				ObjPoolMgr.Inst.RecycleGO(effectGoFromPool, effect.RecycleDelay);
			}
			else
			{
				CurrentEffects.Add((effectGoFromPool.transform, effect));
			}
			SetNewEffectTransform(effect, effectGoFromPool.transform, overrideScale, overridePosition, overrideDirection);
			OnSpawnEffect(effect, effectGoFromPool.transform);
		}
	}

	protected GameObject GetEffectGoFromPool(SpellEffectSettings effects)
	{
		string effectPrefabName = GetEffectPrefabName(effects);
		string path = "Prefabs/Spell/" + EffectID + "/" + effectPrefabName;
		GameObject gameObject = null;
		gameObject = ((!Performance.ClearTrailRender) ? ObjPoolMgr.Inst.GetGO(path) : ObjPoolMgr.Inst.GetGO(path, StopAllTrailRender));
		if (Performance.CanShootFromMonster)
		{
			EffectTransparencyController component = gameObject.GetComponent<EffectTransparencyController>();
			if ((bool)component)
			{
				component.ForceNoTransparent = (bool)Spell.ownerPpt && !Spell.ownerPpt.gameObject.CompareAnyTag("Player", "Teammate");
				component.UpdateTransparent();
			}
		}
		return gameObject;
	}

	private void SetNewEffectTransform(SpellEffectSettings effect, Transform trans, float? overrideScale = null, Vector3? overridePosition = null, Vector3? overrideDirection = null)
	{
		(Transform, SpellEffectSettings)[] effects = new(Transform, SpellEffectSettings)[1] { (trans, effect) };
		if (effect.ScaleMode == SpellEffectSettings.ScaleType.Manual && overrideScale.HasValue)
		{
			trans.localScale = Vector3.one * overrideScale.Value;
		}
		else
		{
			UpdateScale(effects);
		}
		if (effect.PositionMode == SpellEffectSettings.PositionType.Manual && overridePosition.HasValue)
		{
			trans.position = overridePosition.Value;
		}
		else
		{
			UpdatePosition(effects);
		}
		if (effect.RotationMode == SpellEffectSettings.RotationType.Manual && overrideDirection.HasValue)
		{
			trans.right = overrideDirection.Value.normalized;
		}
		else
		{
			UpdateRotation(effects);
		}
	}

	protected virtual void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
	}

	protected virtual void OnChangeColor(SpellEffectSettings effect, Transform newColorTrans)
	{
	}

	protected virtual void OnWillRecycleEffect(SpellEffectSettings effect, Transform trans)
	{
	}

	protected virtual void OnFirstFrame()
	{
	}

	protected virtual string GetEffectPrefabName(SpellEffectSettings settings)
	{
		string text = "";
		text = ((!settings.IgnoreColorType) ? (EffectID + "_" + settings.Name + "_" + GetEffectPrefabColorPostfix(settings)) : (EffectID + "_" + settings.Name));
		if (GameMgr.IsHarmony_Static && settings.HarmonizedColors.Contains(Spell.ColorType))
		{
			text += "_H";
		}
		return text;
	}

	protected virtual string GetEffectPrefabColorPostfix(SpellEffectSettings settings)
	{
		return Spell.ColorType.ToString();
	}

	protected virtual Vector3 GetSpriteEffectPosition(SpellSpriteEffectSettings settings, Vector3? position = null)
	{
		Vector3? vector = position;
		Vector3 vector2;
		if (vector.HasValue)
		{
			vector2 = vector.GetValueOrDefault();
		}
		else
		{
			Vector3 vector3 = ((!settings.LayerSettings.EnableLayerCorrect) ? new Vector3(settings.AttachTarget.position.x, settings.AttachTarget.position.y, 0f) : (settings.LayerSettings.BaseMode switch
			{
				SpellSpriteEffectLayerSettings.BaseType.Spell => Spell.transform.position, 
				SpellSpriteEffectLayerSettings.BaseType.AttachTarget => new Vector3(settings.AttachTarget.position.x, settings.AttachTarget.position.y, 0f), 
				_ => throw new ArgumentOutOfRangeException(), 
			}));
			vector2 = vector3;
		}
		Vector3 vector4 = vector2;
		if (settings.RandomOffset > 0f)
		{
			vector4.x += UnityEngine.Random.Range(0f - settings.RandomOffset, settings.RandomOffset);
			vector4.y += UnityEngine.Random.Range(0f - settings.RandomOffset, settings.RandomOffset);
		}
		if (settings.LayerSettings.EnableLayerCorrect)
		{
			Vector3 layerPoint = Tool2D.GetLayerPoint(vector4, settings.LayerSettings.Layer);
			layerPoint.z += settings.LayerSettings.OffsetZ;
			return layerPoint;
		}
		return vector4;
	}

	protected virtual Quaternion GetSpriteEffectRotation(SpellSpriteEffectSettings settings, Quaternion? rotation = null)
	{
		return (Quaternion)(rotation ?? (settings.RotationMode switch
		{
			SpellSpriteEffectSettings.RotationType.Identity => (Quaternion)quaternion.identity, 
			SpellSpriteEffectSettings.RotationType.Random => Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f)), 
			SpellSpriteEffectSettings.RotationType.LookDirection => Quaternion.LookRotation(Spell.Direction) * Quaternion.Euler(0f, -90f, 0f), 
			SpellSpriteEffectSettings.RotationType.TargetRotation => settings.AttachTarget.rotation, 
			_ => throw new ArgumentOutOfRangeException(), 
		}));
	}

	protected virtual Vector3 GetSpriteEffectScale(SpellSpriteEffectSettings settings, float? scale = null)
	{
		Vector3 vector = default(Vector3);
		vector = ((!scale.HasValue) ? (settings.ScaleMode switch
		{
			SpellSpriteEffectSettings.ScaleType.TargetLossyScale => settings.AttachTarget.lossyScale, 
			SpellSpriteEffectSettings.ScaleType.TargetLocalScale => settings.AttachTarget.localScale, 
			SpellSpriteEffectSettings.ScaleType.Manual => Vector3.one, 
			SpellSpriteEffectSettings.ScaleType.EffectRadius => Vector3.one * Spell.spellCfg.radius * 2f, 
			_ => throw new ArgumentOutOfRangeException(), 
		}) : (scale.Value * Vector3.one));
		return vector * settings.Scale.RandomResult();
	}

	protected virtual EffectPlayParam GetSpriteEffectPlayParam(SpellSpriteEffectSettings settings, Vector3? position = null, Quaternion? rotation = null, float? scale = null)
	{
		float a = (((bool)Spell.ownerPpt && !Spell.ownerPpt.gameObject.CompareAnyTag("Player", "Teammate")) ? 1f : DataMgr.settingData.SpellTransparent);
		EffectPlayParam effectPlayParam = new EffectPlayParam
		{
			Position = GetSpriteEffectPosition(settings, position),
			Rotation = GetSpriteEffectRotation(settings, rotation),
			Scale = GetSpriteEffectScale(settings, scale),
			Material = settings.OverrideMaterial,
			Color = new Color(1f, 1f, 1f, a)
		};
		if (settings.RandomFilpX)
		{
			effectPlayParam.FilpX = UnityEngine.Random.Range(0, 2) == 0;
		}
		if (settings.RandomFilpY)
		{
			effectPlayParam.FilpY = UnityEngine.Random.Range(0, 2) == 0;
		}
		return effectPlayParam;
	}

	protected virtual SpriteEffectAnima GetRandomSpriteEffectAnima(SpellSpriteEffectSettings settings)
	{
		if (GameMgr.IsHarmony_Static && settings.HarmonizedAnimations.GetCount(Spell.ColorType) > 0)
		{
			return settings.HarmonizedAnimations.Get(Spell.ColorType);
		}
		return settings.Animations.Get(Spell.ColorType);
	}

	protected static void StopAllTrailRender(GameObject go)
	{
		TrailRenderer[] componentsInChildren = go.GetComponentsInChildren<TrailRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Clear();
		}
		if (go.TryGetComponent<TrailRenderer>(out var component))
		{
			component.Clear();
		}
	}

	protected static ParticleSystem[] GetAllParticleSystem(GameObject go)
	{
		List<ParticleSystem> list = go.GetComponentsInChildren<ParticleSystem>().ToList();
		if (go.TryGetComponent<ParticleSystem>(out var component))
		{
			list.Add(component);
		}
		return list.ToArray();
	}

	public void CreateFallingExplosion(Vector3? position = null)
	{
		if ((!FullTransparency || !IgnoreEffectCreateWhenFullTransparency || !ShootFromPlayerOrTeammate) && FallingExplosionEffectMode != FallingExplosionEffectSetting.None)
		{
			Vector3 fallingExplosionLayerPoint = GetFallingExplosionLayerPoint(position ?? base.transform.position.IgnoreZ());
			Vector3 vector = Spell.GetFallingGroundDamageRadius() * Vector3.one;
			string fallingExplosionPrefabName = GetFallingExplosionPrefabName();
			SpriteEffectAnima spriteEffectAnima = ABResources.LoadAsset<SpriteEffectAnima>(fallingExplosionPrefabName);
			if ((bool)spriteEffectAnima)
			{
				SpellSpriteEffectController.Inst.PlayEffect(spriteEffectAnima, CreateFallingExplosionParam(fallingExplosionLayerPoint, vector));
			}
			else
			{
				ObjPoolMgr.Inst.GetGO(fallingExplosionPrefabName, fallingExplosionLayerPoint, 3f).transform.localScale = vector;
			}
		}
	}

	protected virtual EffectPlayParam CreateFallingExplosionParam(Vector3 point, Vector3 scale)
	{
		float a = (((bool)Spell.ownerPpt && !Spell.ownerPpt.gameObject.CompareAnyTag("Player", "Teammate")) ? 1f : DataMgr.settingData.SpellTransparent);
		return new EffectPlayParam
		{
			Position = point,
			Scale = scale,
			FilpX = (UnityEngine.Random.Range(0, 2) == 0),
			FilpY = (UnityEngine.Random.Range(0, 2) == 0),
			Rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360)),
			Color = new Color(1f, 1f, 1f, a)
		};
	}

	protected virtual Vector3 GetFallingExplosionLayerPoint(Vector3 worldPosition)
	{
		return Tool2D.GetLayerPoint(worldPosition);
	}

	protected virtual string GetFallingExplosionPrefabName()
	{
		switch (FallingExplosionEffectMode)
		{
		case FallingExplosionEffectSetting.CommonFall:
			return string.Format("{0}31191/31191_FallExplosion_{1}", "Prefabs/Spell/", Spell.ColorType);
		case FallingExplosionEffectSetting.CustomPlayerColor:
			if (Spell.ColorType == SpellColorType.Player)
			{
				return string.Format("{0}{1}/{2}_FallExplosion_{3}", "Prefabs/Spell/", EffectID, EffectID, Spell.ColorType);
			}
			return string.Format("{0}31191/31191_FallExplosion_{1}", "Prefabs/Spell/", Spell.ColorType);
		case FallingExplosionEffectSetting.CustomAllColor:
			return string.Format("{0}{1}/{2}_FallExplosion_{3}", "Prefabs/Spell/", EffectID, EffectID, Spell.ColorType);
		case FallingExplosionEffectSetting.CustomAllInOneColor:
			return "Prefabs/Spell/" + EffectID + "/" + EffectID + "_FallExplosion";
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public virtual void PlayFallingGroundSound()
	{
		if (!string.IsNullOrEmpty(FallingGroundSound))
		{
			Spell.PlaySE(FallingGroundSound);
		}
	}
}
