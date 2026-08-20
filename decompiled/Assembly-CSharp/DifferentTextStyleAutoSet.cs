using UnityEngine;
using UnityEngine.UI;

public class DifferentTextStyleAutoSet : MonoBehaviour
{
	private void OnEnable()
	{
		if (GameMgr.IsMobile_Static)
		{
			base.transform.GetComponent<Text>().fontStyle = FontStyle.Normal;
		}
		Object.Destroy(this);
	}
}
