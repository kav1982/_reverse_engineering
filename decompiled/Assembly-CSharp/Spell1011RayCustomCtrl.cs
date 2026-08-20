using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Spell1011RayCustomCtrl : MonoBehaviour
{
	public List<LineRenderer> _lineRenders = new List<LineRenderer>();

	public List<Transform> _lineSingleNodes = new List<Transform>();

	private void OnEnable()
	{
		ShowLine();
	}

	public void SetLinePos(Vector3[] linePositions, int index)
	{
		LineRenderer lineRenderer = _lineRenders[index];
		lineRenderer.positionCount = linePositions.Length;
		lineRenderer.SetPositions(linePositions);
	}

	public void SetNode(float3 nodePos, int index)
	{
		_lineSingleNodes[index].gameObject.SetActive(value: true);
		_lineSingleNodes[index].position = nodePos;
	}

	public void HideLine()
	{
		foreach (LineRenderer lineRender in _lineRenders)
		{
			lineRender.gameObject.SetActive(value: false);
		}
		foreach (Transform lineSingleNode in _lineSingleNodes)
		{
			lineSingleNode.gameObject.SetActive(value: false);
		}
	}

	private void ShowLine()
	{
		foreach (LineRenderer lineRender in _lineRenders)
		{
			lineRender.gameObject.SetActive(value: true);
		}
		foreach (Transform lineSingleNode in _lineSingleNodes)
		{
			lineSingleNode.gameObject.SetActive(value: true);
		}
	}
}
