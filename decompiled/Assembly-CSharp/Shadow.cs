using System;
using UnityEngine;

public class Shadow : MonoBehaviour
{
	public GameObject shadowPrefab;

	public float shadowScale = 1f;

	public bool rotateFollow;

	public bool controlBySpellTransparent;

	private float transparentCache = 1f;

	private float defaultTransparent = -1f;

	private MeshRenderer shadowRenderer;

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

	public bool IsShow => shadowGO.activeSelf;

	private void OnEnable()
	{
		CreateShadow();
		LateUpdate();
		UpdateTransparent();
	}

	private void UpdateTransparent()
	{
		if (controlBySpellTransparent && (bool)shadowGO && !(Math.Abs(transparentCache - DataMgr.settingData.FinalSpellTransparent) < 0.01f))
		{
			transparentCache = DataMgr.settingData.FinalSpellTransparent;
			if (!shadowRenderer)
			{
				shadowRenderer = shadowGO.GetComponent<MeshRenderer>();
			}
			if (!shadowRenderer)
			{
				shadowRenderer = shadowGO.GetComponentInChildren<MeshRenderer>();
			}
			if (defaultTransparent < 0f)
			{
				defaultTransparent = 0.4f;
			}
			_ = defaultTransparent;
			_ = DataMgr.settingData.FinalSpellTransparent;
			shadowRenderer.material.SetFloat(GameConstManaged.shaderTransparencyIndex, defaultTransparent * DataMgr.settingData.FinalSpellTransparent);
		}
	}

	private void LateUpdate()
	{
		if (shadowGO != null)
		{
			shadowGO.transform.position = Tool2D.IgnoreZPoint(base.transform, 1.05f);
			if (!rotateFollow)
			{
				shadowGO.transform.rotation = Quaternion.identity;
			}
		}
	}

	public void SetTransparency(float value)
	{
		if (!shadowRenderer)
		{
			shadowRenderer = shadowGO.GetComponent<MeshRenderer>();
		}
		if (!shadowRenderer)
		{
			shadowRenderer = shadowGO.GetComponentInChildren<MeshRenderer>();
		}
		shadowRenderer.material.SetFloat(GameConstManaged.shaderTransparencyIndex, value);
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
			shadowGO = UnityEngine.Object.Instantiate(shadowPrefab, Tool2D.IgnoreZPoint(base.transform, 1.05f), Quaternion.identity, base.transform);
			shadowGO.transform.localScale = Vector3.one * shadowScale;
		}
	}

	public void SetScale(float shadowScale)
	{
		this.shadowScale = shadowScale;
		shadowGO.transform.localScale = Vector3.one * shadowScale;
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
