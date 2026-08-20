using System;
using System.Collections.Generic;
using UnityEngine;

public class Elite3_Tentacle : MonoBehaviour
{
	public class NodeInfo
	{
		public float rootStartRotation;

		public Elite3_Tentacle master;

		private bool isStart;

		private bool isFinal;

		public NodeInfo preNode;

		public NodeInfo prepreNode;

		private float angleReversed;

		public Transform node;

		public float rotateAmplitude;

		public float rotateSpeed;

		public float startAngle;

		public float startLength;

		public float nowAngle;

		public Vector3 dampSpeed;

		public Vector3 truePosition;

		public Vector3 lastFramePosition;

		public void Initialize(bool isFinal = false)
		{
			this.isFinal = isFinal;
			if (preNode != null)
			{
				startLength = (preNode.node.position - node.position).magnitude;
			}
			angleReversed = ((!(UnityEngine.Random.Range(0f, 1f) > 0.5f)) ? 1 : (-1));
			lastFramePosition = node.transform.position;
		}

		public void UpdateRotation()
		{
			nowAngle = Mathf.Sin(rotateSpeed * MathF.PI * 2f * Time.time) * rotateAmplitude * angleReversed + startAngle;
		}

		public void SetPosition()
		{
			if (preNode == null)
			{
				return;
			}
			if (prepreNode == null)
			{
				Vector3 b = Tool2D.IgnoreZPoint(Tool2D.GetDir(master.transform.up, preNode.nowAngle - 90f)) * startLength;
				Vector3 vector = Vector3.Lerp(Tool2D.IgnoreZPoint(lastFramePosition - preNode.node.position), b, master.tentacleStrengthDamp).normalized * startLength;
				Vector3 vector2 = Tool2D.IgnoreZPoint(Vector3.SmoothDamp(Tool2D.IgnoreZPoint(lastFramePosition), Tool2D.IgnoreZPoint(preNode.node.position + vector), ref dampSpeed, Time.deltaTime * master.tentacleDampTime));
				vector2 = Tool2D.IgnoreZPoint((vector2 - Tool2D.IgnoreZPoint(preNode.node.position)).normalized * startLength + preNode.node.position);
				preNode.node.right = Tool2D.IgnoreZPoint(vector2) - Tool2D.IgnoreZPoint(preNode.node.position);
				node.position = Tool2D.IgnoreZPoint(vector2) + new Vector3(0f, 0f, master.transform.position.z);
				lastFramePosition = Tool2D.IgnoreZPoint(vector2);
				return;
			}
			Vector3 dir = Tool2D.GetDir(Tool2D.IgnoreZPoint(preNode.node.position - prepreNode.node.position), preNode.nowAngle);
			Vector3 vector3 = Vector3.Lerp(Tool2D.IgnoreZPoint(lastFramePosition - preNode.node.position), dir, master.tentacleStrengthDamp).normalized * startLength;
			Vector3 vector4 = Tool2D.IgnoreZPoint(Vector3.SmoothDamp(Tool2D.IgnoreZPoint(lastFramePosition), Tool2D.IgnoreZPoint(preNode.node.position + vector3), ref dampSpeed, Time.deltaTime * master.tentacleDampTime));
			vector4 = Tool2D.IgnoreZPoint((vector4 - Tool2D.IgnoreZPoint(preNode.node.position)).normalized * startLength + preNode.node.position);
			preNode.node.right = vector4 - Tool2D.IgnoreZPoint(preNode.node.position);
			node.position = Tool2D.IgnoreZPoint(vector4) + new Vector3(0f, 0f, master.transform.position.z);
			lastFramePosition = Tool2D.IgnoreZPoint(vector4);
			if (isFinal)
			{
				node.right = vector4 - Tool2D.IgnoreZPoint(preNode.node.position);
			}
		}
	}

	public float rotateSpeed = 540f;

	public GameObject body;

	public List<Transform> tentacles0 = new List<Transform>();

	public List<Transform> tentacles1 = new List<Transform>();

	public List<Transform> tentacles2 = new List<Transform>();

	public List<Transform> tentacles3 = new List<Transform>();

	public bool attacking;

	private List<NodeInfo> nodes0 = new List<NodeInfo>();

	private List<NodeInfo> nodes1 = new List<NodeInfo>();

	private List<NodeInfo> nodes2 = new List<NodeInfo>();

	private List<NodeInfo> nodes3 = new List<NodeInfo>();

	public VariableFloat tentacleRotateSpeed;

	public VariableFloat tentacleRotateAmplitude;

	public float tentacleStrengthDamp;

	public float tentacleDampTime;

	public bool stopRotate;

	private void Start()
	{
		for (int i = 0; i < 8; i++)
		{
			NodeInfo nodeInfo = new NodeInfo();
			nodeInfo.node = tentacles0[i];
			tentacleRotateSpeed.RandomResult();
			nodeInfo.rotateSpeed = tentacleRotateSpeed.result;
			tentacleRotateAmplitude.RandomResult();
			nodeInfo.rotateAmplitude = tentacleRotateAmplitude.result;
			nodeInfo.startAngle = tentacles0[i].transform.localEulerAngles.z;
			nodeInfo.master = this;
			nodeInfo.rootStartRotation = Tool2D.GetAngleBetweenTwoDirection(base.transform.up, nodeInfo.node.right);
			nodes0.Add(nodeInfo);
			nodeInfo.Initialize();
			NodeInfo nodeInfo2 = new NodeInfo();
			nodeInfo2.node = tentacles1[i];
			nodeInfo2.startLength = (nodeInfo2.node.transform.position - nodeInfo.node.transform.position).magnitude;
			tentacleRotateSpeed.RandomResult();
			nodeInfo2.rotateSpeed = tentacleRotateSpeed.result;
			tentacleRotateAmplitude.RandomResult();
			nodeInfo2.rotateAmplitude = tentacleRotateAmplitude.result;
			nodeInfo2.startAngle = tentacles1[i].transform.localEulerAngles.z;
			nodeInfo2.master = this;
			nodes1.Add(nodeInfo2);
			nodeInfo2.preNode = nodeInfo;
			nodeInfo2.Initialize();
			NodeInfo nodeInfo3 = new NodeInfo();
			nodeInfo3.node = tentacles2[i];
			nodeInfo3.startLength = (nodeInfo3.node.transform.position - nodeInfo2.node.transform.position).magnitude;
			tentacleRotateSpeed.RandomResult();
			nodeInfo3.rotateSpeed = tentacleRotateSpeed.result;
			tentacleRotateAmplitude.RandomResult();
			nodeInfo3.rotateAmplitude = tentacleRotateAmplitude.result;
			nodeInfo3.startAngle = tentacles2[i].transform.localEulerAngles.z;
			nodeInfo3.master = this;
			nodes2.Add(nodeInfo3);
			nodeInfo3.preNode = nodeInfo2;
			nodeInfo3.prepreNode = nodeInfo;
			nodeInfo3.Initialize();
			NodeInfo nodeInfo4 = new NodeInfo();
			nodeInfo4.node = tentacles3[i];
			nodeInfo4.startLength = (nodeInfo4.node.transform.position - nodeInfo3.node.transform.position).magnitude;
			tentacleRotateSpeed.RandomResult();
			nodeInfo4.rotateSpeed = tentacleRotateSpeed.result;
			tentacleRotateAmplitude.RandomResult();
			nodeInfo4.rotateAmplitude = tentacleRotateAmplitude.result;
			nodeInfo4.startAngle = tentacles3[i].transform.localEulerAngles.z;
			nodeInfo4.master = this;
			nodes3.Add(nodeInfo4);
			nodeInfo4.preNode = nodeInfo3;
			nodeInfo4.prepreNode = nodeInfo2;
			nodeInfo4.Initialize(isFinal: true);
		}
	}

	private void Update()
	{
		if (!stopRotate)
		{
			if (!attacking)
			{
				body.transform.localEulerAngles += new Vector3(0f, 0f, Time.deltaTime * 135f);
			}
			else
			{
				body.transform.localEulerAngles += new Vector3(0f, 0f, Time.deltaTime * rotateSpeed);
			}
		}
		for (int i = 0; i < 8; i++)
		{
			nodes0[i].UpdateRotation();
			nodes1[i].UpdateRotation();
			nodes1[i].SetPosition();
			nodes2[i].UpdateRotation();
			nodes2[i].SetPosition();
			nodes3[i].UpdateRotation();
			nodes3[i].SetPosition();
		}
	}
}
