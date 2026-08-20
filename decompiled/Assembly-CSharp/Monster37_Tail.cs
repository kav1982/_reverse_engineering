using System;
using UnityEngine;

public class Monster37_Tail : MonoBehaviour
{
	public int nodeCount;

	public LineRenderer thisLineRenderer;

	public float nodeInterval;

	public float smoothTime;

	public float strengthLerp;

	public float swingFrequency;

	private float swingTimer;

	private float swingAngle;

	public float swingAmplitude;

	public bool reversed;

	private Vector3[] nodePoints;

	private Vector3[] nodeSpeed;

	private float baseAngle;

	private float startPhase;

	public Monster37 master;

	public Transform tsf_TailRoot;

	public Material mat_normal;

	public Material mat_reversed;

	public Material mat_normal_H;

	public Material mat_reversed_H;

	private bool isFlipped;

	public void Initialize()
	{
		startPhase = UnityEngine.Random.Range(0f, MathF.PI * 2f);
		reversed = (double)UnityEngine.Random.Range(0f, 1f) < 0.5;
		nodePoints = new Vector3[nodeCount];
		nodeSpeed = new Vector3[nodeCount];
		nodePoints[0] = tsf_TailRoot.transform.position + new Vector3(0f, 0f, 0.1f);
		for (int i = 1; i < nodeCount; i++)
		{
			nodePoints[i] = nodePoints[i - 1] + new Vector3(0f, nodeInterval, 0f);
		}
		thisLineRenderer.positionCount = nodeCount;
		thisLineRenderer.enabled = true;
		if (GameMgr.IsHarmony_Static && mat_normal_H != null)
		{
			mat_normal = mat_normal_H;
			mat_reversed = mat_reversed_H;
		}
		isFlipped = master.IsFlipped;
		UnityEngine.Object.Destroy(thisLineRenderer.material);
		if (master.IsFlipped)
		{
			thisLineRenderer.material = mat_normal;
			baseAngle = -90f;
		}
		else
		{
			thisLineRenderer.material = mat_reversed;
			baseAngle = 90f;
		}
	}

	public void Hide()
	{
		thisLineRenderer.enabled = false;
		for (int i = 1; i < nodeCount; i++)
		{
			nodePoints[i] = Vector3.zero;
		}
	}

	private void Update()
	{
		if (master.myPpt.BaseColor != thisLineRenderer.startColor)
		{
			thisLineRenderer.startColor = master.myPpt.BaseColor;
			thisLineRenderer.endColor = master.myPpt.BaseColor;
		}
		thisLineRenderer.widthMultiplier = master.transform.lossyScale.x;
		for (int i = 0; i < nodeCount; i++)
		{
			thisLineRenderer.SetPosition(i, Tool2D.IgnoreZPoint(master.transform.lossyScale.x * (nodePoints[i] - nodePoints[0]) + nodePoints[0], nodePoints[0].z));
		}
		if (master.tailFrozen)
		{
			return;
		}
		if (isFlipped != master.IsFlipped)
		{
			isFlipped = master.IsFlipped;
			UnityEngine.Object.Destroy(thisLineRenderer.material);
			if (master.IsFlipped)
			{
				thisLineRenderer.material = mat_normal;
				baseAngle = -90f;
			}
			else
			{
				thisLineRenderer.material = mat_reversed;
				baseAngle = 90f;
			}
		}
		nodePoints[0] = tsf_TailRoot.transform.position + new Vector3(0f, 0f, 0.1f);
		swingTimer += Time.deltaTime;
		float num = Mathf.Sin(swingTimer * swingFrequency * 2f * MathF.PI + startPhase);
		swingAngle = num * swingAmplitude * (float)((!reversed) ? 1 : (-1)) + baseAngle;
		nodePoints[1] = nodePoints[0] + Tool2D.GetDir(swingAngle) * nodeInterval;
		for (int j = 2; j < nodeCount; j++)
		{
			Vector3 b = nodePoints[j - 1] - nodePoints[j - 2];
			Vector3 vector = Vector3.Lerp(nodePoints[j] - nodePoints[j - 1], b, strengthLerp);
			Vector3 target = nodePoints[j - 1] + vector.normalized * nodeInterval;
			nodePoints[j] = Tool2D.IgnoreZPoint(Vector3.SmoothDamp(nodePoints[j], target, ref nodeSpeed[j], smoothTime), nodePoints[0].z);
		}
	}
}
