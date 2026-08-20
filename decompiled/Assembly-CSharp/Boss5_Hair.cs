using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss5_Hair : MonoBehaviour
{
	[Serializable]
	public class Node
	{
		public float startAngle;

		public Transform transform;

		public float startLength;

		public Vector3 velocity;

		public Vector3 tempPosition;

		public Vector3 lastFramePosition;

		public float allLength;
	}

	public Transform rootTransform;

	public List<Transform> allTransform = new List<Transform>();

	public List<Node> allNode = new List<Node>();

	public float dampSpeed;

	[Header("晃动")]
	public float amplitude;

	public VariableFloat moveSpeed;

	public VariableFloat frequency;

	public AnimationCurve blendStrength;

	private float startPhase;

	public int nodeCount => allNode.Count;

	private float totalLength
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < nodeCount; i++)
			{
				num += allNode[i].startLength;
			}
			return num;
		}
	}

	private float GetOffsetByIndex(int index)
	{
		return blendStrength.Evaluate(allNode[index].allLength / totalLength) * amplitude * totalLength * Mathf.Sin(frequency.result * (allNode[index].allLength / totalLength) * MathF.PI * 2f + startPhase + Time.time * moveSpeed.result * MathF.PI * 2f);
	}

	private void GetAllNode()
	{
		allTransform.Clear();
		allNode.Clear();
		allTransform.Add(rootTransform);
		Node node = new Node();
		allNode.Add(node);
		node.startAngle = rootTransform.localEulerAngles.z;
		node.transform = rootTransform;
		node.lastFramePosition = rootTransform.position;
		Transform child = rootTransform.GetChild(0);
		while (child != null)
		{
			allTransform.Add(child);
			Node node2 = new Node();
			allNode.Add(node2);
			node2.startAngle = child.localEulerAngles.z;
			node2.transform = child;
			node2.startLength = (child.position - allTransform[allTransform.IndexOf(child) - 1].position).magnitude;
			node2.lastFramePosition = child.position;
			node2.allLength = node2.startLength + allNode[allNode.IndexOf(node2) - 1].allLength;
			if (child.childCount > 0)
			{
				child = child.GetChild(0);
				continue;
			}
			break;
		}
	}

	private void Start()
	{
		frequency.RandomResult();
		moveSpeed.RandomResult();
		GetAllNode();
	}

	private void Update()
	{
		for (int i = 0; i < nodeCount; i++)
		{
			switch (i)
			{
			case 0:
				allNode[i].tempPosition = allNode[i].transform.position;
				allNode[i].lastFramePosition = allNode[i].transform.position;
				continue;
			case 1:
			{
				allNode[i].tempPosition = Tool2D.IgnoreZPoint(Vector3.SmoothDamp(allNode[i].lastFramePosition, allNode[i - 1].tempPosition + Tool2D.GetDir(base.transform.right, allNode[i - 1].startAngle).normalized * allNode[i].startLength, ref allNode[i].velocity, 1f / dampSpeed), base.transform.position.z);
				Vector3 vector = Vector2.Perpendicular((Vector2)Tool2D.IgnoreZPoint(allNode[i].tempPosition - allNode[i - 1].tempPosition).normalized) * GetOffsetByIndex(i);
				allNode[i].lastFramePosition = allNode[i].tempPosition;
				allNode[i - 1].transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(base.transform.parent.right, allNode[i].tempPosition + vector - allNode[i - 1].transform.position));
				allNode[i].transform.position = Tool2D.IgnoreZPoint(allNode[i].tempPosition + vector, base.transform.position.z);
				continue;
			}
			}
			allNode[i].tempPosition = Tool2D.IgnoreZPoint(Vector3.SmoothDamp(allNode[i].lastFramePosition, allNode[i - 1].tempPosition + Tool2D.GetDir(allNode[i - 1].tempPosition - allNode[i - 2].tempPosition, allNode[i - 1].startAngle).normalized * allNode[i].startLength, ref allNode[i].velocity, 1f / dampSpeed), base.transform.position.z);
			allNode[i].lastFramePosition = allNode[i].tempPosition;
			Vector3 vector2 = Vector2.Perpendicular((Vector2)Tool2D.IgnoreZPoint(allNode[i].tempPosition - allNode[i - 1].tempPosition).normalized) * GetOffsetByIndex(i);
			allNode[i - 1].transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(allNode[i - 2].transform.right, allNode[i].tempPosition + vector2 - allNode[i - 1].transform.position));
			allNode[i].transform.position = Tool2D.IgnoreZPoint(allNode[i].tempPosition + vector2, base.transform.position.z);
			if (i == nodeCount - 1)
			{
				allNode[i].transform.right = Tool2D.IgnoreZPoint(allNode[i].tempPosition - allNode[i - 1].tempPosition);
			}
		}
	}
}
