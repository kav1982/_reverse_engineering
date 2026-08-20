using UnityEngine;

public class SpecialObj41 : MonoBehaviour
{
	public GameObject[] pfbs;

	public VariableFloat offset;

	private void Start()
	{
		Vector3 position = base.transform.position + Tool2D.GetDir() * offset.RandomResult();
		Object.Instantiate(pfbs[Random.Range(0, pfbs.Length)], position, Quaternion.identity, base.transform.parent);
		Object.Destroy(base.gameObject);
	}
}
