using System.Collections.Generic;
using UnityEngine;

public class Spell4024DaveHarpoonChainCtrl : MonoBehaviour
{
	public List<LineRenderer> _lineRenders = new List<LineRenderer>();

	public void SetLinePos(Vector3[] linePositions, int index)
	{
		LineRenderer lineRenderer = _lineRenders[index];
		lineRenderer.positionCount = linePositions.Length;
		lineRenderer.SetPositions(linePositions);
	}
}
