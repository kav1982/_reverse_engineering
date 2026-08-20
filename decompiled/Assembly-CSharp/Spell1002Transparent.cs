using UnityEngine;

public class Spell1002Transparent : EffectTransparencyController
{
	private MeshRenderer mr;

	private static readonly int AlphaID = Shader.PropertyToID("_Alpha");

	protected override void InitTransparencyCustom()
	{
		mr = GetComponent<MeshRenderer>();
		if (!mr)
		{
			mr = GetComponentInChildren<MeshRenderer>();
		}
	}

	protected override void SetCustomTransparency(float transparency)
	{
		base.SetCustomTransparency(transparency);
		mr.materials[0].SetFloat(AlphaID, transparency);
		mr.materials[1].SetFloat(AlphaID, transparency);
	}
}
