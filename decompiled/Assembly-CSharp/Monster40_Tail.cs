using System.Collections.Generic;
using UnityEngine;

public class Monster40_Tail : MonoBehaviour
{
	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public Vector3 offset;

	public VariableInt nodeCount;

	public float segmentLength;

	public float lerp;

	public float springLerp;

	public float springFixer;

	public float tailrotateOffset;

	public float tailRotateSpeed;

	public GameObject bodyBall;

	public float dirationBlend;

	public List<Monster40_BodyBall> bodyBalls = new List<Monster40_BodyBall>();

	public float smoothTime;

	private Vector3[] nodePoints;

	private Vector3[] nodeSpeed;

	public Monster40 monster40;

	public Vector3 tempPosition;

	public Sprite tail;

	public Sprite tailShadow;

	private void Update()
	{
		float x = monster40.transform.lossyScale.x;
		for (int i = 0; i < nodeCount.result; lr_Leg.SetPosition(i, Tool2D.GetLayerPoint(nodePoints[i])), lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(nodePoints[i], 1.05f) + new Vector3(0f, monster40.tsf_TailRoot.localPosition.z + monster40.tsf_Motion.localPosition.y, 0f)), i++)
		{
			switch (i)
			{
			case 0:
				bodyBalls[i].transform.position = Tool2D.GetLayerPoint(nodePoints[i]) + new Vector3(0f, 0f, 0.01f);
				nodePoints[i] = Tool2D.IgnoreZPoint(monster40.tsf_TailRoot.transform.position);
				bodyBalls[i].srShadow.transform.position = Tool2D.IgnoreZPoint(nodePoints[i], 1.05f) - new Vector3(0f, monster40.tsf_Motion.localPosition.y, 0f);
				continue;
			case 1:
				nodePoints[i] = Vector3.SmoothDamp(nodePoints[i], nodePoints[i - 1] - ((1f - dirationBlend) * (nodePoints[i - 1] - nodePoints[i]).normalized + dirationBlend * monster40.faceDir).normalized * segmentLength * x, ref nodeSpeed[i], smoothTime);
				break;
			default:
				nodePoints[i] = Vector3.SmoothDamp(nodePoints[i], nodePoints[i - 1] - ((1f - dirationBlend) * (nodePoints[i - 1] - nodePoints[i]).normalized + dirationBlend * (nodePoints[i - 2] - nodePoints[i - 1])).normalized * segmentLength * x, ref nodeSpeed[i], smoothTime);
				break;
			}
			bodyBalls[i].transform.position = Tool2D.GetLayerPoint(nodePoints[i]) + new Vector3(0f, 0f, 0.01f);
			bodyBalls[i].srShadow.transform.position = Tool2D.IgnoreZPoint(nodePoints[i], 1.05f) - new Vector3(0f, monster40.tsf_Motion.localPosition.y, 0f);
			bodyBalls[i].transform.up = nodePoints[i - 1] - nodePoints[i];
			bodyBalls[i].transform.forward = Vector3.forward;
			bodyBalls[i].srShadow.transform.up = nodePoints[i - 1] - nodePoints[i];
		}
		if (lr_Leg.startColor != monster40.myPpt.BaseColor)
		{
			lr_Leg.startColor = monster40.myPpt.BaseColor;
			lr_Leg.endColor = monster40.myPpt.BaseColor;
		}
		if (bodyBalls[0].sr.material.color != monster40.myPpt.BaseColor)
		{
			for (int j = 0; j < bodyBalls.Count; j++)
			{
				bodyBalls[j].sr.material.color = monster40.myPpt.BaseColor;
			}
		}
	}

	public void EveryInitial()
	{
		for (int i = 0; i < nodeCount.result; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = Tool2D.IgnoreZPoint(monster40.tsf_TailRoot.transform.position) + new Vector3(0f, 0f, 0f - monster40.tsf_Motion.localPosition.z);
			}
			else
			{
				nodePoints[i] = nodePoints[i - 1] - monster40.faceDir * segmentLength;
			}
			bodyBalls[i].transform.position = nodePoints[i];
			lr_Leg.SetPosition(i, nodePoints[i]);
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(nodePoints[i], 1.05f));
		}
	}

	public void Initialize(Monster40 monster40, float angle)
	{
		this.monster40 = monster40;
		base.transform.position += offset;
		nodeCount.RandomResult();
		lr_Leg.positionCount = nodeCount.result;
		lr_Shadow.positionCount = nodeCount.result;
		nodePoints = new Vector3[nodeCount.result];
		nodeSpeed = new Vector3[nodeCount.result];
		for (int i = 0; i < nodeCount.result; i++)
		{
			bodyBalls.Add(Object.Instantiate(bodyBall, base.transform).GetComponent<Monster40_BodyBall>());
		}
		bodyBall.SetActive(value: false);
		bodyBalls[bodyBalls.Count - 1].sr.sprite = tail;
		bodyBalls[bodyBalls.Count - 1].srShadow.sprite = tailShadow;
		for (int j = 0; j < nodeCount.result; j++)
		{
			if (j == 0)
			{
				nodePoints[j] = Tool2D.IgnoreZPoint(monster40.tsf_TailRoot.transform.position) + new Vector3(0f, 0f, 0f - monster40.tsf_Motion.localPosition.z);
			}
			else
			{
				nodePoints[j] = nodePoints[j - 1] - monster40.faceDir * segmentLength;
			}
			bodyBalls[j].transform.position = nodePoints[j];
			lr_Leg.SetPosition(j, nodePoints[j]);
			lr_Shadow.SetPosition(j, Tool2D.IgnoreZPoint(nodePoints[j], 1.05f));
		}
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		for (int i = 0; i < nodeCount.result; i++)
		{
			nodePoints[i] += changeValue;
		}
	}
}
