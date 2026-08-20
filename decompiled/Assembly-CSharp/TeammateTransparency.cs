using UnityEngine;

public class TeammateTransparency : MonoBehaviour
{
	private static readonly int Alpha = Shader.PropertyToID("_Alpha");

	private SpriteRenderer spriteRenderer;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		float finalSummonTransparent = DataMgr.settingData.FinalSummonTransparent;
		spriteRenderer.material.SetFloat(Alpha, finalSummonTransparent);
	}
}
