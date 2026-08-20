using UnityEngine;

public class Spell1018FallLightingAnima : MonoBehaviour
{
	public float value;

	public LineRenderer lineRenderer;

	public Material material;

	public float animaSpeed;

	private void Start()
	{
		material = lineRenderer.material;
	}

	private void OnEnable()
	{
		value = 0f;
	}

	private void Update()
	{
		value += Time.deltaTime * animaSpeed;
		if (value >= 1f)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		material.SetFloat("_HighlightsPosition", value);
	}
}
