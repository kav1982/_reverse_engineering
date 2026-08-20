using UnityEngine;

public class TestMono : MonoBehaviour
{
	public float moveSpeed;

	private void Update()
	{
		base.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
	}
}
