using UnityEngine;

public class Spell9038SelfRotate : MonoBehaviour
{
	public bool RandomRotateWhenOnEnable = true;

	public float RotateSpeed;

	private void OnEnable()
	{
		base.transform.rotation = (RandomRotateWhenOnEnable ? Quaternion.Euler(0f, 0f, Random.Range(0, 360)) : Quaternion.identity);
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * RotateSpeed));
	}
}
