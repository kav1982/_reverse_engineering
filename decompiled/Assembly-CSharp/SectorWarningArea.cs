using UnityEngine;

public class SectorWarningArea : MonoBehaviour
{
	private static readonly int AngleId = Shader.PropertyToID("_Angle");

	private static readonly int ProgressId = Shader.PropertyToID("_Progress");

	private static readonly int AlphaId = Shader.PropertyToID("_Alpha");

	private static readonly int UseAngularProgressId = Shader.PropertyToID("_UseAngularProgress");

	private static readonly int UseRadialProgressId = Shader.PropertyToID("_UseRadialProgress");

	private static readonly int ReverseAngularProgressId = Shader.PropertyToID("_ReverseAngularProgress");

	private static readonly int InnerRadiusId = Shader.PropertyToID("InnerRadius");

	private static readonly int LegacyInnerRadiusId = Shader.PropertyToID("_InnerRadius");

	private static readonly int RadiusInnerId = Shader.PropertyToID("_RadiusInner");

	private static readonly int RadiusOuterId = Shader.PropertyToID("_RadiusOuter");

	private const float DefaultInnerRadiusRatio = 0f;

	private const float DefaultOuterRadius = 0.5f;

	[SerializeField]
	private Renderer targetRenderer;

	[Header("Fade")]
	public float fadeInTime = 0.08f;

	public float fadeOutTime = 0.12f;

	private MaterialPropertyBlock propertyBlock;

	private float duration;

	private float timer;

	private bool isRegistered;

	private void Awake()
	{
		EnsureInitialized();
	}

	private void OnEnable()
	{
		EnsureInitialized();
		if (!isRegistered)
		{
			timer = 0f;
			ApplyShaderValues(90f, 0f, 0f, useRadialProgress: false, useAngularProgress: false, clockwise: false, 0f);
		}
	}

	private void OnDisable()
	{
		isRegistered = false;
	}

	public void RegisterRadial(float sectorAngle, float progressDuration)
	{
		Register(sectorAngle, progressDuration, useRadialProgress: true, useAngularProgress: false, clockwise: false);
	}

	public void RegisterDonutRadial(float sectorAngle, float innerRadiusRatio, float progressDuration)
	{
		Register(sectorAngle, progressDuration, useRadialProgress: true, useAngularProgress: false, clockwise: false, Mathf.Clamp01(innerRadiusRatio));
	}

	public void RegisterDonutAngular(float sectorAngle, float innerRadiusRatio, float progressDuration, bool clockwise)
	{
		Register(sectorAngle, progressDuration, useRadialProgress: false, useAngularProgress: true, clockwise, Mathf.Clamp01(innerRadiusRatio));
	}

	public void RegisterAngular(float sectorAngle, float progressDuration, bool clockwise)
	{
		Register(sectorAngle, progressDuration, useRadialProgress: false, useAngularProgress: true, clockwise);
	}

	private void Register(float sectorAngle, float progressDuration, bool useRadialProgress, bool useAngularProgress, bool clockwise, float? innerRadiusRatio = null)
	{
		EnsureInitialized();
		duration = Mathf.Max(0.0001f, progressDuration);
		timer = 0f;
		isRegistered = true;
		ApplyShaderValues(Mathf.Clamp(sectorAngle, 0f, 360f), 0f, 0f, useRadialProgress, useAngularProgress, clockwise, innerRadiusRatio.GetValueOrDefault());
	}

	private void Update()
	{
		if (isRegistered)
		{
			EnsureInitialized();
			timer += Time.deltaTime;
			float value = Mathf.Clamp01(timer / duration);
			float fadeAlpha = GetFadeAlpha(timer, duration);
			if (targetRenderer != null)
			{
				targetRenderer.GetPropertyBlock(propertyBlock);
				propertyBlock.SetFloat(ProgressId, value);
				propertyBlock.SetFloat(AlphaId, fadeAlpha);
				targetRenderer.SetPropertyBlock(propertyBlock);
			}
			if (timer >= duration)
			{
				isRegistered = false;
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
	}

	private float GetFadeAlpha(float elapsed, float totalDuration)
	{
		float num = 1f;
		if (fadeInTime > 0f)
		{
			num = Mathf.Min(num, Mathf.Clamp01(elapsed / fadeInTime));
		}
		if (fadeOutTime > 0f)
		{
			num = Mathf.Min(num, Mathf.Clamp01((totalDuration - elapsed) / fadeOutTime));
		}
		return num;
	}

	private void ApplyShaderValues(float sectorAngle, float progress, float alpha, bool useRadialProgress, bool useAngularProgress, bool clockwise, float innerRadiusRatio)
	{
		EnsureInitialized();
		if (!(targetRenderer == null))
		{
			targetRenderer.GetPropertyBlock(propertyBlock);
			propertyBlock.Clear();
			propertyBlock.SetFloat(AngleId, sectorAngle);
			propertyBlock.SetFloat(ProgressId, progress);
			propertyBlock.SetFloat(AlphaId, alpha);
			propertyBlock.SetFloat(UseRadialProgressId, useRadialProgress ? 1 : 0);
			propertyBlock.SetFloat(UseAngularProgressId, useAngularProgress ? 1 : 0);
			propertyBlock.SetFloat(ReverseAngularProgressId, clockwise ? 1 : 0);
			ApplyRadiusShaderValues(propertyBlock, innerRadiusRatio);
			targetRenderer.SetPropertyBlock(propertyBlock);
		}
	}

	private void ApplyRadiusShaderValues(MaterialPropertyBlock block, float innerRadiusRatio)
	{
		float num = 0.5f;
		float num2 = Mathf.Clamp01(innerRadiusRatio);
		block.SetFloat(InnerRadiusId, num2);
		block.SetFloat(LegacyInnerRadiusId, num2);
		block.SetFloat(RadiusOuterId, num);
		block.SetFloat(RadiusInnerId, num2 * num);
	}

	private void EnsureInitialized()
	{
		if (targetRenderer == null)
		{
			targetRenderer = GetComponentInChildren<Renderer>();
		}
		if (propertyBlock == null)
		{
			propertyBlock = new MaterialPropertyBlock();
		}
	}
}
