using UnityEngine;

public class Spell1004LaserCustomCtrl : MonoBehaviour
{
	public LineRenderer LineRenderer;

	public AnimationCurve WidthCurve;

	public bool isChanging;

	public float totalTime = 0.2f;

	public float currentTime;

	private void OnEnable()
	{
		isChanging = false;
		currentTime = 0f;
	}

	private void Update()
	{
		if (isChanging)
		{
			currentTime += Time.deltaTime;
			if ((double)currentTime < 0.1)
			{
				LineRenderer.widthMultiplier = WidthCurve.Evaluate(Mathf.Clamp(currentTime / 0.2f, 0.1f, 1f));
				return;
			}
			if (currentTime < totalTime - 0.1f)
			{
				LineRenderer.widthMultiplier = WidthCurve.Evaluate(0.5f);
				return;
			}
			float time = Mathf.Clamp((currentTime - (totalTime - 0.2f)) / 0.2f, 0.1f, 1f);
			LineRenderer.widthMultiplier = WidthCurve.Evaluate(time);
		}
	}
}
