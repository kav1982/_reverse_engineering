using UnityEngine;

public class UIRanklistSpin : MonoBehaviour
{
	public float rotationSpeed = 5f;

	private void Start()
	{
	}

	private void Update()
	{
		Rotate();
	}

	private void Rotate()
	{
		float angle = rotationSpeed * Time.deltaTime;
		base.transform.Rotate(Vector3.forward, angle);
	}
}
