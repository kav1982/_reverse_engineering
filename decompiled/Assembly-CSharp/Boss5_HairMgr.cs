using System.Collections.Generic;
using UnityEngine;

public class Boss5_HairMgr : MonoBehaviour
{
	public float amplitude;

	public VariableFloat moveSpeed;

	public VariableFloat frequency;

	public AnimationCurve blendStrength;

	public List<Boss5_Hair> allHairs = new List<Boss5_Hair>();

	public float dampSpeed;

	[Header("三阶段颜色变换")]
	public float stageHeadBlendSpeed;

	public bool stageColorChange;

	public float stage3Blend;

	public List<SpriteRenderer> allRenderers = new List<SpriteRenderer>();

	[Header("眼睛颜色图像变换")]
	public SpriteRenderer centerEyeRenderer;

	public SpriteRenderer leftEyeRenderer;

	public SpriteRenderer rightEyeRenderer;

	public SpriteRenderer centerEyeBackRenderer;

	public SpriteRenderer leftEyeBackRenderer;

	public SpriteRenderer rightEyeBackRenderer;

	public Sprite centerEyeSprite;

	public Sprite leftEyeSprite;

	public Sprite rightEyeSprite;

	public Sprite centerEyeBackSprite;

	public Sprite leftEyeBackSprite;

	public Sprite rightEyeBackSprite;

	public float eyeLightFix;

	public float eyeGlow;

	private Color originGlowColor;

	private void Start()
	{
		allHairs.AddRange(GetComponentsInChildren<Boss5_Hair>());
		originGlowColor = centerEyeRenderer.material.GetColor("_GlowColor");
		foreach (Boss5_Hair allHair in allHairs)
		{
			allHair.amplitude = amplitude;
			allHair.frequency = frequency;
			allHair.moveSpeed = moveSpeed;
			allHair.blendStrength = blendStrength;
			allHair.dampSpeed = dampSpeed;
			allHair.frequency.RandomResult();
			allHair.moveSpeed.RandomResult();
		}
	}

	private void Update()
	{
		if (stageColorChange)
		{
			stage3Blend += Time.deltaTime * stageHeadBlendSpeed;
		}
		foreach (SpriteRenderer allRenderer in allRenderers)
		{
			allRenderer.material.SetFloat("_Blend", Mathf.Min(stage3Blend, 1f));
			Color value = originGlowColor * eyeGlow;
			centerEyeRenderer.material.SetColor("_GlowColor", value);
			leftEyeRenderer.material.SetColor("_GlowColor", value);
			rightEyeRenderer.material.SetColor("_GlowColor", value);
		}
	}

	public void ChangeEye()
	{
		centerEyeRenderer.sprite = centerEyeSprite;
		leftEyeRenderer.sprite = leftEyeSprite;
		rightEyeRenderer.sprite = rightEyeSprite;
		centerEyeBackRenderer.sprite = centerEyeBackSprite;
		leftEyeBackRenderer.sprite = leftEyeBackSprite;
		rightEyeBackRenderer.sprite = rightEyeBackSprite;
	}
}
