using UnityEngine;

public class SpecialObj216Matrix02 : LayerCorrect
{
	[Space(50f)]
	private Vector3 motion;

	private Vector3 lastFramePos;

	private Vector3 newFramePos;

	private float speed;

	private void OnTriggerEnter(Collider other)
	{
		PlayerController component = other.GetComponent<PlayerController>();
		if (component != null && other.IsPlayerTrigger())
		{
			speed = component.myPpt.MoveSpeed;
			newFramePos = other.transform.position;
			if (lastFramePos != Vector3.zero)
			{
				motion = (lastFramePos - newFramePos).normalized * speed * Time.deltaTime;
			}
			lastFramePos = newFramePos;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		PlayerController component = other.GetComponent<PlayerController>();
		if (component != null && other.IsPlayerTrigger())
		{
			speed = component.myPpt.MoveSpeed;
			newFramePos = other.transform.position;
			if (lastFramePos != Vector3.zero)
			{
				motion = (lastFramePos - newFramePos).normalized * speed * Time.deltaTime;
			}
			Debug.Log(motion);
			lastFramePos = newFramePos;
			other.transform.position -= motion;
		}
	}
}
