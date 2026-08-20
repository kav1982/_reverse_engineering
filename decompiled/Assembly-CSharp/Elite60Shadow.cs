using System;
using UnityEngine;

public class Elite60Shadow : MonoBehaviour
{
	public GameObject shadowPrefab;

	public float shadowScale = 1f;

	public bool rotateFollow;

	public bool controlBySpellTransparent;

	[Header("Elite60 Shadow")]
	public bool rotateVisualOnly = true;

	private float transparentCache = 1f;

	private float defaultTransparent = -1f;

	private MeshRenderer shadowRenderer;

	private Transform shadowVisualTransform;

	private GameObject shadowGO;

	private bool isCreated;

	public GameObject ShadowGO
	{
		get
		{
			if (shadowGO == null)
			{
				CreateShadow();
			}
			return shadowGO;
		}
	}

	public bool IsShow
	{
		get
		{
			if (shadowGO != null)
			{
				return shadowGO.activeSelf;
			}
			return false;
		}
	}

	private void OnEnable()
	{
		CreateShadow();
		Show();
		LateUpdate();
		UpdateTransparent();
	}

	private void OnDisable()
	{
		Hide();
	}

	private void OnDestroy()
	{
		if (shadowGO != null)
		{
			UnityEngine.Object.Destroy(shadowGO);
			shadowGO = null;
		}
	}

	private void UpdateTransparent()
	{
		if (controlBySpellTransparent && (bool)shadowGO && !(Math.Abs(transparentCache - DataMgr.settingData.FinalSpellTransparent) < 0.01f))
		{
			transparentCache = DataMgr.settingData.FinalSpellTransparent;
			CacheRenderer();
			if (defaultTransparent < 0f)
			{
				defaultTransparent = 0.4f;
			}
			shadowRenderer.material.SetFloat(GameConstManaged.shaderTransparencyIndex, defaultTransparent * DataMgr.settingData.FinalSpellTransparent);
		}
	}

	private void LateUpdate()
	{
		if (shadowGO == null)
		{
			return;
		}
		shadowGO.transform.position = Tool2D.IgnoreZPoint(base.transform, 1.05f);
		shadowGO.transform.position += Vector3.down * 0.7f;
		if (rotateFollow)
		{
			Quaternion rotation = Quaternion.Euler(0f, 0f, base.transform.eulerAngles.z);
			if (rotateVisualOnly && shadowVisualTransform != null && shadowVisualTransform != shadowGO.transform)
			{
				shadowGO.transform.rotation = Quaternion.identity;
				shadowVisualTransform.rotation = rotation;
			}
			else
			{
				shadowGO.transform.rotation = rotation;
			}
		}
		else
		{
			shadowGO.transform.rotation = Quaternion.identity;
			if (rotateVisualOnly && shadowVisualTransform != null)
			{
				shadowVisualTransform.rotation = Quaternion.identity;
			}
		}
	}

	public void SetTransparency(float value)
	{
		CacheRenderer();
		if (shadowRenderer != null)
		{
			shadowRenderer.material.SetFloat(GameConstManaged.shaderTransparencyIndex, value);
		}
	}

	public void CreateShadow()
	{
		if (!isCreated)
		{
			isCreated = true;
			if (shadowPrefab == null)
			{
				Debug.LogWarning(base.name + " 没有设置 shadow prefab");
				return;
			}
			shadowGO = UnityEngine.Object.Instantiate(shadowPrefab, Tool2D.IgnoreZPoint(base.transform, 1.05f), Quaternion.identity);
			shadowGO.transform.localScale = Vector3.one * shadowScale;
			CacheRenderer();
		}
	}

	private void CacheRenderer()
	{
		if (!(shadowGO == null))
		{
			if (!shadowRenderer)
			{
				shadowRenderer = shadowGO.GetComponent<MeshRenderer>();
			}
			if (!shadowRenderer)
			{
				shadowRenderer = shadowGO.GetComponentInChildren<MeshRenderer>();
			}
			shadowVisualTransform = ((shadowRenderer != null) ? shadowRenderer.transform : shadowGO.transform);
		}
	}

	public void SetScale(float shadowScale)
	{
		this.shadowScale = shadowScale;
		if (shadowGO != null)
		{
			shadowGO.transform.localScale = Vector3.one * shadowScale;
		}
	}

	public void Hide()
	{
		if (shadowPrefab != null && shadowGO != null)
		{
			shadowGO.SetActive(value: false);
		}
	}

	public void Show()
	{
		if (shadowPrefab != null && shadowGO != null)
		{
			shadowGO.SetActive(value: true);
		}
	}
}
