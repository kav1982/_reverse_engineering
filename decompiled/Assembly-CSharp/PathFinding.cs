using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct PathFinding : IComponentData, IQueryTypeParameter
{
	public bool needUpdatePath;

	public float3 startPosition;

	public float3 endPosition;

	public float3 lastEndPosition;

	public float3 trueEndPosition;

	public int navArea;

	public float moveThreshold;

	public float3 walkToPoint;

	public float updateIntervalTimer;

	public NavMeshPointRequest samplePointRequest;

	public float3 recordPoint0;

	public float3 recordPoint1;

	public float3 recordPoint2;

	public int recordPointIndex;

	public bool allCornerArrived;

	public void SetPathPoints(Vector3 value, int index)
	{
		switch (index)
		{
		case 0:
			recordPoint0 = value;
			break;
		case 1:
			recordPoint1 = value;
			break;
		case 2:
			recordPoint2 = value;
			break;
		}
	}

	public float3 GetRecordPoints(int index)
	{
		return index switch
		{
			0 => recordPoint0, 
			1 => recordPoint1, 
			2 => recordPoint2, 
			_ => recordPoint0, 
		};
	}

	public void UpdatePath(float3 startPosition, float3 endPosition, int navArea, bool needUpdateNow = false)
	{
		needUpdatePath = true;
		if (needUpdateNow)
		{
			updateIntervalTimer = 999f;
		}
		this.startPosition = startPosition;
		this.endPosition = endPosition;
		this.navArea = navArea;
	}
}
