using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		Object.Destroy(this);
	}
}
