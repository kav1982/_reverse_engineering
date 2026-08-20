using System;
using UnityEngine;

public class Boss5_BackTentacle : MonoBehaviour
{
	public VariableInt nodeCount;

	public LineRenderer thisLineRenderer;

	public float tentacleLengthPrecent;

	public VariableFloat lengthMinus;

	public VariableFloat nodeInterval;

	public float smoothTime;

	public float strengthLerp;

	public VariableFloat swingFrequency;

	public VariableFloat startSwing;

	private float swingTimer;

	private float swingAngle;

	public VariableFloat swingAmplitude;

	public bool reversed;

	private Vector3[] nodePoints;

	private Vector3[] nodeSpeed;

	public VariableFloat tentacleThick;

	public Vector3 rotateRootOffset;

	private float baseAngle;

	private bool isRecycled;

	public float tentacleGrowSpeed;

	public float growSingSpeedFix;

	private void OnEnable()
	{
		if (!isRecycled)
		{
			isRecycled = true;
			startSwing.RandomResult();
		}
		reversed = (double)UnityEngine.Random.Range(0f, 1f) < 0.5;
		swingAmplitude.RandomResult();
		nodeInterval.RandomResult();
		lengthMinus.RandomResult();
		swingFrequency.RandomResult();
		nodeCount.RandomResult();
		tentacleThick.RandomResult();
		nodePoints = new Vector3[nodeCount.result];
		nodeSpeed = new Vector3[nodeCount.result];
		nodePoints[0] = base.transform.position;
		for (int i = 1; i < nodeCount.result; i++)
		{
			nodePoints[i] = nodePoints[i - 1] + new Vector3(0f, nodeInterval.result, 0f);
		}
		thisLineRenderer.positionCount = nodeCount.result;
		baseAngle = Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.IgnoreZPoint(base.transform.localPosition - rotateRootOffset));
	}

	private void Update()
	{
		if (Boss5.Inst != null)
		{
			if (Boss5.Inst.myPpt.BaseColor != thisLineRenderer.startColor)
			{
				thisLineRenderer.startColor = Boss5.Inst.myPpt.BaseColor;
				thisLineRenderer.endColor = Boss5.Inst.myPpt.BaseColor;
			}
			if (Boss5.Inst.stageTentacleShow)
			{
				if (tentacleLengthPrecent < 1f)
				{
					tentacleLengthPrecent += Time.deltaTime * tentacleGrowSpeed;
				}
				if (!Boss5.Inst.stageTentacleShowDone)
				{
					swingTimer += Time.deltaTime * growSingSpeedFix;
				}
				else
				{
					swingTimer += Time.deltaTime;
				}
			}
		}
		thisLineRenderer.material.SetFloat("_Length", tentacleLengthPrecent - lengthMinus.result);
		nodePoints[0] = base.transform.position;
		float num = Mathf.Sin(swingTimer * swingFrequency.result * 2f * MathF.PI + startSwing.result);
		swingAngle = num * swingAmplitude.result * (float)((!reversed) ? 1 : (-1)) + baseAngle;
		nodePoints[1] = nodePoints[0] + Tool2D.GetDir(swingAngle) * nodeInterval.result;
		for (int i = 2; i < nodeCount.result; i++)
		{
			Vector3 b = nodePoints[i - 1] - nodePoints[i - 2];
			Vector3 vector = Vector3.Lerp(nodePoints[i] - nodePoints[i - 1], b, strengthLerp);
			Vector3 target = nodePoints[i - 1] + vector.normalized * nodeInterval.result;
			nodePoints[i] = Vector3.SmoothDamp(nodePoints[i], target, ref nodeSpeed[i], smoothTime);
		}
		for (int j = 0; j < nodeCount.result; j++)
		{
			thisLineRenderer.SetPosition(j, nodePoints[j]);
			thisLineRenderer.startWidth = tentacleThick.result;
			thisLineRenderer.endWidth = tentacleThick.result;
		}
	}
}
