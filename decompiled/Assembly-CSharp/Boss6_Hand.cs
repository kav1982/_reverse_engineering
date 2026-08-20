using UnityEngine;

public class Boss6_Hand : MonoBehaviour
{
	public float handPhaseOffset;

	public float rootOriginAngle;

	public float handOriginAngle;

	public float upRootOriginAngle;

	public float upHandOriginAngle;

	public float angleRange;

	public SpriteRenderer leftHandRoot;

	public SpriteRenderer rightHandRoot;

	public SpriteRenderer leftHand;

	public SpriteRenderer rightHand;

	public float handDistanceOffset;

	public Transform center;

	private bool faceDown;

	public void SetSort(bool isLeftUp, bool isFaceDown)
	{
		faceDown = isFaceDown;
		if (isFaceDown)
		{
			leftHand.sortingOrder = 2;
			rightHand.sortingOrder = 2;
		}
		else
		{
			leftHand.sortingOrder = -2;
			rightHand.sortingOrder = -2;
		}
		if (isFaceDown)
		{
			if (isLeftUp)
			{
				leftHandRoot.sortingOrder = 1;
				rightHandRoot.sortingOrder = -1;
			}
			else
			{
				leftHandRoot.sortingOrder = -1;
				rightHandRoot.sortingOrder = 1;
			}
		}
		else if (isLeftUp)
		{
			leftHandRoot.sortingOrder = -1;
			rightHandRoot.sortingOrder = -1;
		}
		else
		{
			leftHandRoot.sortingOrder = -1;
			rightHandRoot.sortingOrder = -1;
		}
	}

	public void SetAngle(float phase)
	{
		float num = angleRange * Mathf.Sin(phase);
		float num2 = angleRange * Mathf.Sin(phase + handPhaseOffset);
		leftHandRoot.transform.position = center.transform.position - new Vector3(handDistanceOffset, 0f, 0f);
		rightHandRoot.transform.position = center.transform.position + new Vector3(handDistanceOffset, 0f, 0f);
		if (faceDown)
		{
			leftHandRoot.transform.eulerAngles = new Vector3(0f, 0f, num + rootOriginAngle);
			rightHandRoot.transform.eulerAngles = new Vector3(0f, 0f, 0f - num - rootOriginAngle);
			leftHand.transform.localEulerAngles = new Vector3(0f, 0f, num2 + handOriginAngle);
			rightHand.transform.localEulerAngles = new Vector3(0f, 0f, 0f - num2 - handOriginAngle);
		}
		else
		{
			leftHandRoot.transform.eulerAngles = new Vector3(0f, 0f, num + upRootOriginAngle);
			rightHandRoot.transform.eulerAngles = new Vector3(0f, 0f, 0f - num - upRootOriginAngle);
			leftHand.transform.localEulerAngles = new Vector3(0f, 0f, num2 + upHandOriginAngle);
			rightHand.transform.localEulerAngles = new Vector3(0f, 0f, 0f - num2 - upHandOriginAngle);
		}
	}

	public void SetInvisible()
	{
		leftHandRoot.enabled = false;
		rightHandRoot.enabled = false;
		leftHand.enabled = false;
		rightHand.enabled = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
