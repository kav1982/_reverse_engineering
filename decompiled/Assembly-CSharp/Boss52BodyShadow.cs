using System.Collections.Generic;
using UnityEngine;

public class Boss52BodyShadow : MonoBehaviour
{
	private static readonly int Transparency = Shader.PropertyToID("_Transparency");

	public List<SpriteRenderer> Sprites;

	public Transform ModelTransform;

	private float currentTransparency = 1f;

	private float transparencyDecreaseTimer;

	private float transparencyDecreaseInterval = 0.1f;

	private bool startFade;

	private float transparencyDecreaseAmount = 0.1f;

	private void OnEnable()
	{
		startFade = false;
		transparencyDecreaseTimer = 0f;
		transparencyDecreaseInterval = 0f;
		currentTransparency = 0f;
		foreach (SpriteRenderer sprite in Sprites)
		{
			sprite.material.SetFloat(Transparency, currentTransparency);
		}
	}

	public void StartFade(float initTransparency, float transparencyDecreaseInterval, float transparencyDecreaseAmount, float scaleX)
	{
		startFade = true;
		transparencyDecreaseTimer = 0f;
		this.transparencyDecreaseInterval = transparencyDecreaseInterval;
		this.transparencyDecreaseAmount = transparencyDecreaseAmount;
		currentTransparency = initTransparency;
		ModelTransform.localScale = new Vector3(scaleX, ModelTransform.localScale.y, ModelTransform.localScale.z);
	}

	private void Update()
	{
		if (!startFade)
		{
			return;
		}
		if (currentTransparency <= 0f)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		transparencyDecreaseTimer += Time.deltaTime;
		if (transparencyDecreaseTimer <= transparencyDecreaseInterval)
		{
			return;
		}
		transparencyDecreaseTimer -= transparencyDecreaseInterval;
		currentTransparency -= transparencyDecreaseAmount;
		currentTransparency = Mathf.Max(0f, currentTransparency);
		foreach (SpriteRenderer sprite in Sprites)
		{
			sprite.material.SetFloat(Transparency, currentTransparency);
		}
	}
}
