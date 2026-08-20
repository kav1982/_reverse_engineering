using System.Collections.Generic;
using UnityEngine;

public class Monster17_Chain : MonoBehaviour
{
	public Transform tsf_IronBall;

	public Transform tsf_Chain1;

	public Transform tsf_Chain2;

	public int chainCount;

	public float chainLength;

	public float ikIterationTime;

	public float threshold;

	public GameObject mainTail;

	public Color chainColor = new Color(1f, 1f, 1f, 1f);

	private List<MeshRenderer> allImages = new List<MeshRenderer>();

	private Transform[] tsf_Chains;

	private Vector3[] points;

	private float totalLength;

	private Vector3 IconBallPoint
	{
		get
		{
			return points[points.Length - 1];
		}
		set
		{
			points[points.Length - 1] = value;
		}
	}

	private void Awake()
	{
		totalLength = (float)chainCount * chainLength;
		tsf_Chains = new Transform[chainCount + 1];
		points = new Vector3[chainCount + 1];
		tsf_Chains[0] = tsf_Chain1;
		tsf_Chains[1] = tsf_Chain2;
		for (int i = 2; i < chainCount + 1; i++)
		{
			if (i == chainCount)
			{
				tsf_Chains[i] = tsf_IronBall;
			}
			else if (i % 2 == 0)
			{
				tsf_Chains[i] = Object.Instantiate(tsf_Chain1, base.transform.position, Quaternion.identity, base.transform);
			}
			else
			{
				tsf_Chains[i] = Object.Instantiate(tsf_Chain2, base.transform.position, Quaternion.identity, base.transform);
			}
		}
		for (int j = 0; j < chainCount + 1; j++)
		{
			points[j] = base.transform.position;
		}
		for (int k = 0; k < tsf_Chains.Length; k++)
		{
			allImages.Add(tsf_Chains[k].GetComponentInChildren<MeshRenderer>());
		}
	}

	private void Update()
	{
		if (mainTail == null)
		{
			if (chainColor != allImages[0].material.color)
			{
				for (int i = 0; i < chainCount; i++)
				{
					allImages[i].material.color = chainColor;
				}
			}
			return;
		}
		if (allImages[0].material.color != chainColor)
		{
			for (int j = 0; j < chainCount; j++)
			{
				allImages[j].material.color = chainColor;
			}
		}
		IconBallPoint = Tool2D.IgnoreZPoint(tsf_IronBall.position);
		if ((Tool2D.IgnoreZPoint(mainTail.transform.position) - IconBallPoint).sqrMagnitude < totalLength * totalLength)
		{
			for (int k = 0; (float)k < ikIterationTime; k++)
			{
				for (int l = 0; l < points.Length - 1; l++)
				{
					if (l == 0)
					{
						points[l] = Tool2D.IgnoreZPoint(mainTail.transform.position);
					}
					else
					{
						points[l] = points[l - 1] + (points[l] - points[l - 1]).normalized * chainLength;
					}
				}
				for (int num = points.Length - 2; num >= 0; num--)
				{
					points[num] = points[num + 1] + (points[num] - points[num + 1]).normalized * chainLength;
				}
				if ((Tool2D.IgnoreZPoint(mainTail.transform.position) - points[0]).sqrMagnitude < threshold * threshold)
				{
					break;
				}
			}
		}
		else
		{
			points[0] = Tool2D.IgnoreZPoint(mainTail.transform.position);
			for (int m = 1; m < points.Length; m++)
			{
				points[m] = points[m - 1] + (points[m] - points[m - 1]).normalized * chainLength;
			}
		}
		for (int n = 0; n < tsf_Chains.Length; n++)
		{
			tsf_Chains[n].position = Tool2D.IgnoreZPoint(points[n], 1.06f);
			if (n < tsf_Chains.Length - 1)
			{
				tsf_Chains[n].right = Tool2D.IgnoreZV2ToV1Normal(points[n + 1], points[n]);
			}
			else
			{
				tsf_Chains[n].right = tsf_Chains[n - 1].right;
			}
		}
	}
}
