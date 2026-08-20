using UnityEngine;

public class HS_CallBackParent : MonoBehaviour
{
	[SerializeField]
	protected Transform parentObject;

	protected virtual void OnParticleSystemStopped()
	{
		if (parentObject != null)
		{
			base.transform.parent = parentObject;
			base.transform.localPosition = Vector3.zero;
			base.transform.localEulerAngles = Vector3.zero;
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
