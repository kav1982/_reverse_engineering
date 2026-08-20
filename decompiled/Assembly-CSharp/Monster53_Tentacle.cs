using System;
using UnityEngine;

public class Monster53_Tentacle : MonoBehaviour
{
	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public Vector3 offset;

	public VariableInt nodeCount;

	public VariableFloat segmentLength;

	public float lerp;

	public float smoothTime;

	public float strengthLerp;

	public VariableFloat swingFrequency;

	public VariableFloat startSwing;

	public VariableFloat swingAmplitude;

	private float swingAngle;

	private bool reversed;

	private float swingTimer;

	public Material mt_reverse;

	private Vector3[] nodePoints;

	private Vector3[] nodeSpeed;

	private Monster53 master;

	public void SingleInitial(Monster53 monster25, float angle)
	{
		master = monster25;
		base.transform.up = Tool2D.GetDir(angle);
		base.transform.position += offset;
		nodeCount.RandomResult();
		segmentLength.RandomResult();
		lr_Leg.positionCount = nodeCount.result;
		lr_Shadow.positionCount = nodeCount.result;
		nodePoints = new Vector3[nodeCount.result];
		nodeSpeed = new Vector3[nodeCount.result];
	}

	public void EveryInitial()
	{
		swingFrequency.RandomResult();
		swingAmplitude.RandomResult();
		startSwing.RandomResult();
		float z = Tool2D.GetLayerPoint(master.transform).z + master.tsf_Motion.localPosition.z + master.tsf_Model.localPosition.z + 0.1f;
		for (int i = 0; i < nodeCount.result; i++)
		{
			nodeSpeed[i] = Vector3.zero;
			if (i == 0)
			{
				nodePoints[i] = base.transform.position + new Vector3(0f, 0f, 0f - master.tsf_Motion.localPosition.y);
			}
			else
			{
				nodePoints[i] = nodePoints[i - 1] - base.transform.up * segmentLength.result;
			}
			lr_Leg.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(nodePoints[i], 1.05f));
		}
		if (GeneralTool.ChanceResult(0.5f))
		{
			UnityEngine.Object.Destroy(lr_Leg.material);
			UnityEngine.Object.Destroy(lr_Shadow.material);
			lr_Leg.material = mt_reverse;
			lr_Shadow.material = mt_reverse;
		}
	}

	private void Update()
	{
		if (lr_Leg.startColor != master.myPpt.BaseColor)
		{
			lr_Leg.startColor = master.myPpt.BaseColor;
			lr_Leg.startColor = master.myPpt.BaseColor;
			lr_Leg.endColor = master.myPpt.BaseColor;
		}
		if (master.myPpt.FronzenState == UnitProperty.Affect_FrozenState.Frozening)
		{
			return;
		}
		swingTimer += Time.deltaTime;
		float num = Mathf.Sin(swingTimer * swingFrequency.result * 2f * MathF.PI + startSwing.result);
		swingAngle = num * swingAmplitude.result * (float)((!reversed) ? 1 : (-1));
		float z = Tool2D.GetLayerPoint(master.transform).z + master.tsf_Motion.localPosition.z + master.tsf_Model.localPosition.z + 0.1f;
		for (int i = 0; i < nodeCount.result; i++)
		{
			switch (i)
			{
			case 0:
				nodePoints[i] = base.transform.position + new Vector3(0f, 0f, 0f - master.tsf_Motion.localPosition.y);
				break;
			case 1:
				nodePoints[1] = nodePoints[0] - Tool2D.GetDir(base.transform.up, swingAngle) * segmentLength.result;
				break;
			default:
			{
				Vector3 b = nodePoints[i - 1] - nodePoints[i - 2];
				Vector3 vector = Vector3.Lerp(nodePoints[i] - nodePoints[i - 1], b, strengthLerp);
				Vector3 target = nodePoints[i - 1] + vector.normalized * segmentLength.result;
				nodePoints[i] = Vector3.SmoothDamp(nodePoints[i], target, ref nodeSpeed[i], smoothTime);
				break;
			}
			}
			lr_Leg.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(nodePoints[i], 1.05f));
		}
		lr_Leg.material.SetFloat("_Transparency", master.anim.transparency);
		lr_Leg.material.SetFloat("_Blend", master.anim.blend);
		lr_Shadow.material.SetFloat("_Transparency", master.anim.transparency);
	}
}
