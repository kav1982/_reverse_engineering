using UnityEngine;

public class Boss13BombWarning : MonoBehaviour
{
	public bool appear;

	public bool stopMove;

	public float apperaSpeed;

	public float currentScale;

	public float angleOffset;

	public float rotateSpeed;

	public Vector3 stayPosition;

	private void Update()
	{
		if (appear)
		{
			if (currentScale < 1f)
			{
				currentScale += Time.deltaTime * apperaSpeed;
			}
		}
		else if (currentScale > 0f)
		{
			currentScale -= Time.deltaTime * apperaSpeed;
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
		if (stopMove)
		{
			base.transform.position = stayPosition;
		}
		base.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
		angleOffset += Time.deltaTime * rotateSpeed;
		if (angleOffset > 360f || angleOffset < -360f)
		{
			angleOffset = 0f;
		}
		base.transform.eulerAngles = new Vector3(-50f, 0f, angleOffset);
	}
}
