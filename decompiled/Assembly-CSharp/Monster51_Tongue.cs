using System;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class Monster51_Tongue : MonoBehaviour
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

	[Header("锁定")]
	public SpriteRenderer mainRenderer;

	public Transform trackTransform;

	public float offsetZ;

	private float startAngle;

	public bool locked;

	public bool flipx;

	private bool nowFilpx;

	public SkeletonUtility SU;

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

	public void Allmove()
	{
		List<SkeletonUtilityBone> list = new List<SkeletonUtilityBone>(SU.GetComponentsInChildren<SkeletonUtilityBone>());
		for (int i = 0; i < list.Count; i++)
		{
			list[i].DoUpdate(SkeletonUtilityBone.UpdatePhase.World);
		}
		base.transform.position = trackTransform.position + new Vector3(0f, 0f, offsetZ);
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
		startAngle = Tool2D.IgnoreZAngleWithSign(Vector3.right, trackTransform.right);
	}

	public void LateUpdate()
	{
		if (mainRenderer.flipX != flipx)
		{
			flipx = mainRenderer.flipX;
			for (int i = 0; i < allNode.Count; i++)
			{
				Vector3 vector = allNode[i].lastFramePosition - base.transform.position;
				vector.x *= -1f;
				allNode[i].lastFramePosition = base.transform.position + vector;
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
		base.transform.position = trackTransform.position + new Vector3(0f, 0f, offsetZ);
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
				allNode[k].tempPosition = allNode[k].transform.position;
				allNode[k].lastFramePosition = allNode[k].transform.position;
				continue;
			case 1:
			{
				allNode[k].tempPosition = Tool2D.IgnoreZPoint(Vector3.SmoothDamp(allNode[k].lastFramePosition, allNode[k - 1].tempPosition + Tool2D.GetDir(base.transform.up, allNode[k - 1].startAngle - 90f).normalized * allNode[k].startLength, ref allNode[k].velocity, 1f / dampSpeed), base.transform.position.z);
				Vector3 vector2 = Vector2.Perpendicular((Vector2)Tool2D.IgnoreZPoint(allNode[k].tempPosition - allNode[k - 1].tempPosition).normalized) * GetOffsetByIndex(k);
				allNode[k].lastFramePosition = allNode[k].tempPosition;
				Vector3 vector3 = allNode[k].tempPosition + vector2;
				vector3 -= base.transform.position;
				vector3.x *= -1f;
				vector3 = base.transform.position + vector3;
				Vector3 vector4 = (flipx ? vector3 : (allNode[k].tempPosition + vector2));
				allNode[k - 1].transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(base.transform.right, vector4 - allNode[k - 1].transform.position));
				allNode[k].transform.position = Tool2D.IgnoreZPoint(vector4, base.transform.position.z);
				continue;
			}
			}
			allNode[k].tempPosition = Tool2D.IgnoreZPoint(Vector3.SmoothDamp(allNode[k].lastFramePosition, allNode[k - 1].tempPosition + Tool2D.GetDir(allNode[k - 1].tempPosition - allNode[k - 2].tempPosition, allNode[k - 1].startAngle).normalized * allNode[k].startLength, ref allNode[k].velocity, 1f / dampSpeed), base.transform.position.z);
			allNode[k].lastFramePosition = allNode[k].tempPosition;
			Vector3 vector5 = Vector2.Perpendicular((Vector2)Tool2D.IgnoreZPoint(allNode[k].tempPosition - allNode[k - 1].tempPosition).normalized) * GetOffsetByIndex(k);
			Vector3 vector6 = allNode[k].tempPosition + vector5;
			vector6 -= base.transform.position;
			vector6.x *= -1f;
			vector6 = base.transform.position + vector6;
			Vector3 vector7 = (flipx ? vector6 : (allNode[k].tempPosition + vector5));
			allNode[k - 1].transform.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(allNode[k - 2].transform.right, vector7 - allNode[k - 1].transform.position));
			allNode[k].transform.position = Tool2D.IgnoreZPoint(vector7, base.transform.position.z);
			if (k == nodeCount - 1)
			{
				allNode[k].transform.right = Tool2D.IgnoreZPoint(allNode[k].transform.position - allNode[k - 1].transform.position);
			}
		}
	}
}
