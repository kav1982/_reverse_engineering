using UnityEngine;

public class BloodSplat : LayerCorrect
{
	[Space(50f)]
	public bool rotate;

	public SpriteRenderer sr;

	public Sprite[] sprites;

	[Range(0f, 1f)]
	public float initialDark;

	[Header("Bigger")]
	[Range(0f, 1f)]
	public float initialScale;

	public float biggerSpeed;

	public AnimationCurve biggerCurve;

	public float biggerTime;

	[Range(0f, 1f)]
	[Header("Dark")]
	public float minDarkValue;

	public float darkSpeed;

	public float bloodMaxTransparency;

	private float existTime;

	private bool isBigger = true;

	private float currentScale;

	private float targetScale;

	private bool isDark = true;

	private float currentDarkValue = 1f;

	private void Update()
	{
		existTime += Time.deltaTime;
		if (isBigger)
		{
			if (existTime > biggerTime)
			{
				isBigger = false;
			}
			currentScale = Mathf.Lerp(initialScale, targetScale, biggerCurve.Evaluate(existTime / biggerTime));
			sr.transform.localScale = Vector3.one * currentScale;
		}
		if (isDark)
		{
			currentDarkValue -= darkSpeed * Time.deltaTime;
			if (currentDarkValue <= minDarkValue)
			{
				currentDarkValue = minDarkValue;
				isDark = false;
			}
			Color color = sr.material.color;
			color.a = currentDarkValue * bloodMaxTransparency;
			sr.material.color = color;
		}
	}

	public void Initialize(float targetScale)
	{
		existTime = 0f;
		this.targetScale = targetScale;
		isBigger = true;
		currentScale = targetScale * initialScale;
		sr.transform.localScale = Vector3.one * currentScale;
		isDark = true;
		currentDarkValue = initialDark;
		if (rotate)
		{
			sr.transform.rotation = Tool2D.GetRotation();
		}
	}

	public void Initialize(float targetScale, Vector3 dir)
	{
		Initialize(targetScale);
		tsf_Layer.up = dir;
	}
}
