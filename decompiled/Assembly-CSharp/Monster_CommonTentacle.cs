using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster_CommonTentacle : MonoBehaviour
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

	private List<Transform> allTransform = new List<Transform>();

	public List<Node> allNode = new List<Node>();

	public float dampSpeed;

	[Header("晃动")]
	public float amplitude;

	public VariableFloat moveSpeed;

	public VariableFloat frequency;

	public AnimationCurve blendStrength;

	private float startPhase;

	[Header("旋转和锁定")]
	public SpriteRenderer mainRenderer;

	public Transform trackTransform;

	public float offsetZ;

	private float startAngle;

	public bool locked;

	public bool flipx;

	private bool nowFilpx;

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

	public void LockMotion()
	{
		locked = true;
	}

	public void UnlockMotion()
	{
		for (int i = 0; i < allNode.Count; i++)
		{
			allNode[i].lastFramePosition = allNode[i].transform.position;
		}
		locked = false;
	}

	public void OnEnable()
	{
		for (int i = 0; i < allNode.Count; i++)
		{
			allNode[i].velocity = Vector3.zero;
		}
		UnlockMotion();
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
		startAngle = Tool2D.IgnoreZAngleWithSign(Vector3.right, mainRenderer.transform.right);
	}

	private void Update()
	{
		if (mainRenderer.flipX != flipx)
		{
			flipx = mainRenderer.flipX;
			for (int i = 0; i < allNode.Count; i++)
			{
				if (i == 0)
				{
					Vector3 vector = allNode[i].transform.position - mainRenderer.transform.position;
					vector.x *= -1f;
					allNode[i].transform.position = mainRenderer.transform.position + vector;
				}
				Vector3 vector2 = allNode[i].lastFramePosition - mainRenderer.transform.position;
				vector2.x *= -1f;
				allNode[i].lastFramePosition = mainRenderer.transform.position + vector2;
				allNode[i].velocity.Scale(new Vector3(-1f, 1f, 1f));
				if (i == 0)
				{
					allNode[i].startAngle = -180f - allNode[i].startAngle;
				}
				else
				{
					allNode[i].startAngle = 0f - allNode[i].startAngle;
				}
			}
		}
		for (int j = 0; j < nodeCount; j++)
		{
			if (j < nodeCount - 1)
			{
				Debug.DrawLine(allNode[j].lastFramePosition, allNode[j + 1].lastFramePosition);
			}
		}
		if (locked)
		{
			return;
		}
		for (int k = 0; k < nodeCount; k++)
		{
			if (k < nodeCount - 1)
			{
				Debug.DrawLine(allNode[k].lastFramePosition, allNode[k + 1].lastFramePosition);
			}
			switch (k)
			{
			case 0:
				if (flipx)
				{
					Vector3 vector6 = allNode[k].transform.position - mainRenderer.transform.position;
					vector6.x *= -1f;
					allNode[k].transform.position = mainRenderer.transform.position + vector6;
				}
				allNode[k].tempPosition = allNode[k].transform.position;
				allNode[k].lastFramePosition = allNode[k].transform.position;
				if (flipx)
				{
					Vector3 vector7 = allNode[k].transform.position - mainRenderer.transform.position;
					vector7.x *= -1f;
					allNode[k].transform.position = mainRenderer.transform.position + vector7;
				}
				allNode[k].transform.position = Tool2D.IgnoreZPoint(allNode[k].transform.position, base.transform.position.z);
				break;
			case 1:
			{
				allNode[k].tempPosition = Tool2D.IgnoreZPoint(Vector3.SmoothDamp(allNode[k].lastFramePosition, allNode[k - 1].tempPosition + Tool2D.GetDir(base.transform.up, allNode[k - 1].startAngle - 90f).normalized * allNode[k].startLength, ref allNode[k].velocity, 1f / dampSpeed), base.transform.position.z);
				Vector3 vector8 = Vector2.Perpendicular((Vector2)Tool2D.IgnoreZPoint(allNode[k].tempPosition - allNode[k - 1].tempPosition).normalized) * GetOffsetByIndex(k);
				allNode[k].lastFramePosition = allNode[k].tempPosition;
				Vector3 vector9 = allNode[k].tempPosition + vector8;
				vector9 -= base.transform.position;
				vector9.x *= -1f;
				vector9 = base.transform.position + vector9;
				Vector3 vector10 = (flipx ? vector9 : (allNode[k].tempPosition + vector8));
				allNode[k - 1].transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(base.transform.right, vector10 - allNode[k - 1].transform.position));
				allNode[k].transform.position = Tool2D.IgnoreZPoint(vector10, base.transform.position.z);
				break;
			}
			default:
			{
				allNode[k].tempPosition = Tool2D.IgnoreZPoint(Vector3.SmoothDamp(allNode[k].lastFramePosition, allNode[k - 1].tempPosition + Tool2D.GetDir(allNode[k - 1].tempPosition - allNode[k - 2].tempPosition, allNode[k - 1].startAngle).normalized * allNode[k].startLength, ref allNode[k].velocity, 1f / dampSpeed), base.transform.position.z);
				allNode[k].lastFramePosition = allNode[k].tempPosition;
				Vector3 vector3 = Vector2.Perpendicular((Vector2)Tool2D.IgnoreZPoint(allNode[k].tempPosition - allNode[k - 1].tempPosition).normalized) * GetOffsetByIndex(k);
				Vector3 vector4 = allNode[k].tempPosition + vector3;
				vector4 -= base.transform.position;
				vector4.x *= -1f;
				vector4 = base.transform.position + vector4;
				Vector3 vector5 = (flipx ? vector4 : (allNode[k].tempPosition + vector3));
				allNode[k - 1].transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(allNode[k - 2].transform.right, vector5 - allNode[k - 1].transform.position));
				allNode[k].transform.position = Tool2D.IgnoreZPoint(vector5, base.transform.position.z);
				if (k == nodeCount - 1)
				{
					allNode[k].transform.right = Tool2D.IgnoreZPoint(allNode[k].transform.position - allNode[k - 1].transform.position);
				}
				break;
			}
			}
		}
	}
}
