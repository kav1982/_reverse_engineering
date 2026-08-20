using UnityEngine;

public class Boss13_SubmarineWarning : MonoBehaviour
{
	public LineRenderer warningLine;

	public Transform startPoint;

	public Transform point1;

	public Transform point2;

	public Transform endPoint;

	public bool warningOn;

	public Vector3[] positions;

	private void Update()
	{
		if (warningOn)
		{
			positions[0] = startPoint.position;
			positions[1] = point1.position;
			positions[2] = point2.position;
			positions[3] = endPoint.position;
			warningLine.SetPositions(positions);
		}
	}

	public void ChargeEnd()
	{
		warningOn = false;
		warningLine.enabled = false;
	}

	public void WaringOn()
	{
		warningOn = true;
		warningLine.enabled = true;
	}
}
