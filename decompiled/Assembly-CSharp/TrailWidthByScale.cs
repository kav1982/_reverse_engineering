using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
[DisallowMultipleComponent]
public class TrailWidthByScale : MonoBehaviour
{
	private TrailRenderer trailRenderer;

	private float initWidth;

	private float initScale;

	private void Awake()
	{
		trailRenderer = GetComponent<TrailRenderer>();
		initWidth = trailRenderer.widthMultiplier;
		initScale = base.transform.lossyScale.x;
	}

	private void Update()
	{
		if (initScale > 0f)
		{
			trailRenderer.widthMultiplier = base.transform.lossyScale.x / initScale * initWidth;
		}
	}
}
