using UnityEngine;

public class SpecialObj25 : MonoBehaviour
{
	public GameObject[] pfbs;

	private void Start()
	{
		Object.Instantiate(pfbs[Random.Range(0, pfbs.Length)], base.transform.position, Quaternion.identity, base.transform.parent);
		Object.Destroy(base.gameObject);
	}
}
