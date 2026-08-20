using UnityEngine;

public class Liquid : LayerCorrect
{
	[Space(50f)]
	public MeshCollider mc;

	public void Initialize(float radius)
	{
		mc.transform.localScale = Vector3.one * radius * 2f;
		tsf_Layer.localScale = mc.transform.localScale;
	}

	public void Initialize(float radius, float length, Vector3 dir)
	{
		mc.transform.localScale = new Vector3(radius * 2f, length + 0.1f, 1f);
		tsf_Layer.localScale = mc.transform.localScale;
		mc.transform.rotation = Tool2D.GetRotation(Tool2D.GetDegree(dir));
		tsf_Layer.rotation = mc.transform.rotation;
	}
}
