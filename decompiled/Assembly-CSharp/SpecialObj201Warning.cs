using UnityEngine;

public class SpecialObj201Warning : MonoBehaviour
{
	public float shineInterval;

	public SpriteRenderer thisRenderer;

	public Color originColor;

	public Color originColor_H;

	public Color shineColor;

	private float shineTimer;

	private void OnEnable()
	{
		shineTimer = 0f;
		if (GameMgr.IsChAge14_Static)
		{
			originColor = originColor_H;
		}
		thisRenderer.color = originColor;
	}

	private void Update()
	{
		shineTimer += Time.deltaTime;
		if (shineTimer > shineInterval)
		{
			shineTimer -= shineInterval;
			if (thisRenderer.color == shineColor)
			{
				thisRenderer.color = originColor;
			}
			else
			{
				thisRenderer.color = shineColor;
			}
		}
	}
}
