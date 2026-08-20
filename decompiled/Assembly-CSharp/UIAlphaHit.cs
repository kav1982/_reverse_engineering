using UnityEngine;
using UnityEngine.UI;

public class UIAlphaHit : MonoBehaviour
{
	public Image theButton;

	private void Start()
	{
		theButton.alphaHitTestMinimumThreshold = 0.5f;
	}
}
