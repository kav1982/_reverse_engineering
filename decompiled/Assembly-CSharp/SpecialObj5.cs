using UnityEngine;

public class SpecialObj5 : MonoBehaviour
{
	public Transform tsf_Light;

	private void Start()
	{
		tsf_Light.SetParent(base.transform.parent);
		Object.Destroy(base.gameObject);
	}
}
