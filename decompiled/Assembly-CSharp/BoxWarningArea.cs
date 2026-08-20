using UnityEngine;

public class BoxWarningArea : MonoBehaviour
{
	private static readonly int ProgressId = Shader.PropertyToID("_Progress");

	private static readonly int AlphaId = Shader.PropertyToID("_Alpha");

	private static readonly int ExpandFromCenterId = Shader.PropertyToID("_ExpandFromCenter");

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
			ApplyShaderValues(0f, 0f, expandFromCenter: false);
		}
	}

	private void OnDisable()
	{
		isRegistered = false;
	}

	public void Register(Vector3 direction, float length, float width, float progressDuration, bool expandFromCenter)
	{
		RegisterInternal(new Vector2(direction.x, direction.y), length, width, progressDuration, expandFromCenter);
	}

	private void RegisterInternal(Vector2 direction, float length, float width, float progressDuration, bool expandFromCenter)
	{
		EnsureInitialized();
		duration = Mathf.Max(0.0001f, progressDuration);
		timer = 0f;
		isRegistered = true;
		float z = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
		base.transform.rotation = Quaternion.Euler(0f, 0f, z);
		Vector3 localScale = base.transform.localScale;
		localScale.x = Mathf.Max(0.0001f, length);
		localScale.y = Mathf.Max(0.0001f, width);
		base.transform.localScale = localScale;
		ApplyShaderValues(0f, 0f, expandFromCenter);
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

	private void ApplyShaderValues(float progress, float alpha, bool expandFromCenter)
	{
		EnsureInitialized();
		if (!(targetRenderer == null))
		{
			targetRenderer.GetPropertyBlock(propertyBlock);
			propertyBlock.SetFloat(ProgressId, progress);
			propertyBlock.SetFloat(AlphaId, alpha);
			propertyBlock.SetFloat(ExpandFromCenterId, expandFromCenter ? 1 : 0);
			targetRenderer.SetPropertyBlock(propertyBlock);
		}
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
