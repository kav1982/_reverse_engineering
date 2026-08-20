using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Spell4014RayCustomCtrl : MonoBehaviour
{
	public List<LineRenderer> _lineRenders = new List<LineRenderer>();

	public List<Transform> _lineSingleNodes = new List<Transform>();

	private void OnEnable()
	{
		ShowLine();
	}

	public void SetLinePos(float3[] linePositions, int index)
	{
		LineRenderer lineRenderer = _lineRenders[index];
		lineRenderer.positionCount = linePositions.Length;
		for (int i = 0; i < linePositions.Length; i++)
		{
			lineRenderer.SetPosition(i, linePositions[i]);
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

	public void SetLaserHitShader(bool isHit, int index)
	{
		if (isHit)
		{
			_lineRenders[index].material.SetFloat("_LaserSwitch", 0f);
		}
		else
		{
			_lineRenders[index].material.SetFloat("_LaserSwitch", 1f);
		}
	}

	public void SetLaserHitObject(bool isHit)
	{
		_lineRenders[2].gameObject.SetActive(!isHit);
		_lineRenders[0].gameObject.SetActive(isHit);
	}

	public void SetLaserWidth(float width, int index)
	{
		_lineRenders[index].widthMultiplier = width;
	}

	public void SetLaserNodePosAndScale(float width, bool isHit, float minWidth)
	{
		if (isHit)
		{
			_lineSingleNodes[0].transform.position = _lineRenders[0].GetPosition(0);
			_lineSingleNodes[1].transform.position = _lineRenders[0].GetPosition(_lineRenders[0].positionCount - 1);
			_lineSingleNodes[0].transform.localScale = Vector3.one * width;
			_lineSingleNodes[1].transform.localScale = Vector3.one * width;
		}
		else
		{
			_lineSingleNodes[0].transform.position = _lineRenders[2].GetPosition(0);
			_lineSingleNodes[1].transform.position = _lineRenders[2].GetPosition(_lineRenders[2].positionCount - 1);
			_lineSingleNodes[0].transform.localScale = Vector3.one * minWidth;
			_lineSingleNodes[1].transform.localScale = Vector3.one * minWidth;
		}
	}
}
