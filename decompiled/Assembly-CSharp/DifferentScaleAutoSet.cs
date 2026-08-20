using UnityEngine;

public class DifferentScaleAutoSet : MonoBehaviour
{
	public float mobileScale;

	public float otherScale;

	private void OnEnable()
	{
		if (GameMgr.IsMobile_Static)
		{
			base.transform.localScale = new Vector3(mobileScale, mobileScale, mobileScale);
		}
		else
		{
			base.transform.localScale = new Vector3(otherScale, otherScale, otherScale);
		}
		Object.Destroy(this);
	}
}
