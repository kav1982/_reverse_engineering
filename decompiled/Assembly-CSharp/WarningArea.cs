using UnityEngine;

public class WarningArea : LayerCorrect
{
	[Space(50f)]
	public MeshRenderer mr_Area;

	public Transform tsf_Fill;

	private float radius;

	private float duration;

	private float durationTimer;

	private bool zoomDirect;

	private void OnDisable()
	{
		mr_Area.transform.localScale = Vector3.zero;
		tsf_Fill.localScale = Vector3.zero;
		durationTimer = 0f;
	}

	private void Update()
	{
		if (zoomDirect)
		{
			durationTimer += Time.deltaTime;
			tsf_Fill.localScale = Vector3.one * Mathf.Lerp(0f, radius * 2f, durationTimer / duration);
			if (durationTimer >= duration)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
	}

	public void Initialize(float radius, float duration, bool zoomDirect = true)
	{
		this.radius = radius;
		this.duration = duration;
		this.zoomDirect = zoomDirect;
		mr_Area.material.SetFloat("_Radius", radius);
		mr_Area.transform.localScale = Vector3.one * radius * 2f;
		tsf_Fill.localScale = Vector3.zero;
	}

	public void BeginZoom(float duration)
	{
		this.duration = duration;
		zoomDirect = true;
	}
}
