using UnityEngine;

public class Boss56TeleportUnitEffectController : MonoBehaviour
{
	public Transform ScaleTransform;

	private float fadeInDuration = 0.3f;

	private float fadeOutDuration = 0.3f;

	public void TeleportIn()
	{
		_ = ScaleTransform != null;
	}
}
