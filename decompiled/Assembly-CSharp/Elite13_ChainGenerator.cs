using System.Collections.Generic;
using UnityEngine;

public class Elite13_ChainGenerator : MonoBehaviour
{
	[Header("通用")]
	public Elite13 master;

	public float distanceInterval;

	public float timeInterval;

	private float lightningTimes;

	private float existTime;

	[Header("直线电流，可能会做")]
	public bool isStraight;

	public LayerMask wallMask;

	private Vector3 moveDiration;

	private Vector3 verticalDiration;

	[Header("圆形电流")]
	public int circleSplit;

	public int minIterateTime;

	private List<Vector3> generatePoints = new List<Vector3>();

	private Vector3 startPoint;

	private float circlePointsInterval;

	private float roomWidth;

	private float roomHeight;

	private Vector3 roomCenterPoint;

	[Header("预警线")]
	public float maxColorAlpha;

	public LineRenderer LR_Warning;

	public LineRenderer LR_Warning1;

	public float warningTime;

	private float warningTimer;

	private float warningTimer1;

	private bool useFirstLr;

	public void InitializeStraight(Vector3 startPoint, Vector3 diration, Elite13 master)
	{
		LR_Warning.enabled = false;
		LR_Warning1.enabled = false;
		this.startPoint = startPoint;
		verticalDiration = Tool2D.GetDir(diration, 90f);
		moveDiration = diration;
		StraightIterate();
		this.master = master;
	}

	private void StraightIterate()
	{
		Vector3 point = startPoint;
		Vector3 point2 = startPoint;
		bool flag = false;
		if (Tool2D.PointOnNavMesh(startPoint))
		{
			flag = true;
		}
		if (flag)
		{
			if (UnitDotsSyncSystem.Raycast(startPoint, verticalDiration, 30f, GameConst.Filter_Wall, out var result))
			{
				point = result.point;
			}
			if (UnitDotsSyncSystem.Raycast(startPoint, -verticalDiration, 30f, GameConst.Filter_Wall, out result))
			{
				point2 = result.point;
			}
			Elite13.MiniPool.GetGO("Prefabs/EF/EF_Elite13_Chain" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position).GetComponent<Elite13_LightningChain>().Initialize(point, point2);
			LineRenderer obj = (useFirstLr ? LR_Warning : LR_Warning1);
			if (useFirstLr)
			{
				warningTimer = 0f;
			}
			else
			{
				warningTimer1 = 0f;
			}
			useFirstLr = !useFirstLr;
			obj.positionCount = 2;
			obj.enabled = true;
			obj.material.color = new Color(1f, 1f, 1f, 0f);
			obj.SetPosition(0, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(point), LayerCorrectType.GroundEffect));
			obj.SetPosition(1, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(point2), LayerCorrectType.GroundEffect));
		}
		else
		{
			Elite13.MiniPool.RecycleGO(base.gameObject);
		}
		startPoint += moveDiration * distanceInterval;
	}

	public void InitializeCircle(Vector3 startPoint, Elite13 master)
	{
		LR_Warning.enabled = false;
		LR_Warning1.enabled = false;
		lightningTimes = 0f;
		circlePointsInterval = distanceInterval * 1.41421f;
		generatePoints.Clear();
		for (int i = 0; i < circleSplit; i++)
		{
			generatePoints.Add(startPoint);
		}
		this.startPoint = startPoint;
		existTime = 0f;
		isStraight = false;
		CircleIterate();
		this.master = master;
	}

	private void CircleIterate()
	{
		lightningTimes += 1f;
		for (int i = 0; i < generatePoints.Count; i++)
		{
			generatePoints[i] = startPoint + Tool2D.GetDir(i * (360 / generatePoints.Count)) * lightningTimes * circlePointsInterval;
		}
		bool flag = false;
		for (int j = 0; j < circleSplit; j++)
		{
			if (Tool2D.PointOnNavMesh(generatePoints[j]))
			{
				flag = true;
				break;
			}
		}
		if (flag || lightningTimes < (float)minIterateTime)
		{
			LineRenderer lineRenderer = (useFirstLr ? LR_Warning : LR_Warning1);
			if (useFirstLr)
			{
				warningTimer = 0f;
			}
			else
			{
				warningTimer1 = 0f;
			}
			useFirstLr = !useFirstLr;
			lineRenderer.positionCount = generatePoints.Count;
			lineRenderer.enabled = true;
			lineRenderer.material.color = new Color(1f, 1f, 1f, 0f);
			for (int k = 0; k < generatePoints.Count; k++)
			{
				Elite13_LightningChain component = Elite13.MiniPool.GetGO("Prefabs/EF/EF_Elite13_Chain" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position).GetComponent<Elite13_LightningChain>();
				if (k < generatePoints.Count - 1)
				{
					component.Initialize(generatePoints[k], generatePoints[k + 1]);
				}
				else
				{
					component.Initialize(generatePoints[k], generatePoints[0]);
				}
				lineRenderer.SetPosition(k, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(generatePoints[k]), LayerCorrectType.GroundEffect));
			}
		}
		else
		{
			Elite13.MiniPool.RecycleGO(base.gameObject);
		}
	}

	private void Update()
	{
		if (master != null && master.myPpt.AlreadyDead)
		{
			Elite13.MiniPool.RecycleGO(base.gameObject);
		}
		LR_Warning.material.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, maxColorAlpha, warningTimer / warningTime));
		LR_Warning1.material.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, maxColorAlpha, warningTimer1 / warningTime));
		warningTimer += Time.deltaTime;
		if (warningTimer > warningTime)
		{
			LR_Warning.enabled = false;
		}
		warningTimer1 += Time.deltaTime;
		if (warningTimer1 > warningTime)
		{
			LR_Warning1.enabled = false;
		}
		existTime += Time.deltaTime;
		if (existTime > timeInterval)
		{
			existTime -= timeInterval;
			if (!isStraight)
			{
				CircleIterate();
			}
			else
			{
				StraightIterate();
			}
		}
	}
}
