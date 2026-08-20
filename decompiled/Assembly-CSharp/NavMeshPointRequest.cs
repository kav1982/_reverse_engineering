using UnityEngine;

public struct NavMeshPointRequest
{
	public NavMeshRequestState requestState;

	public int requsetType;

	public Vector3 startPoint;

	public float radius;

	public int navAreaMask;

	public VariableFloat radiusRange;

	public Vector3 from;

	public float angle;

	public Vector3 result;

	public void Reset()
	{
		requestState = NavMeshRequestState.Unused;
	}

	public void SetRequest(Vector3 startPoint, int navAreaMask = 16)
	{
		requsetType = 0;
		requestState = NavMeshRequestState.Solving;
		this.startPoint = startPoint;
		this.navAreaMask = navAreaMask;
	}

	public void SetRequest(Vector3 startPoint, float radius, int navAreaMask = 16)
	{
		requsetType = 1;
		requestState = NavMeshRequestState.Solving;
		this.startPoint = startPoint;
		this.radius = radius;
		this.navAreaMask = navAreaMask;
	}

	public void SetRequest(Vector3 startPoint, RandomFloat radius, int navAreaMask = 16)
	{
		requsetType = 2;
		requestState = NavMeshRequestState.Solving;
		this.startPoint = startPoint;
		radiusRange = new VariableFloat
		{
			type = VariableType.Random,
			value1 = radius.value1,
			value2 = radius.value2
		};
		this.navAreaMask = navAreaMask;
	}

	public void SetRequest(Vector3 startPoint, float radius, Vector3 from, float angle, int navAreaMask = 16)
	{
		requsetType = 3;
		requestState = NavMeshRequestState.Solving;
		this.startPoint = startPoint;
		this.radius = radius;
		this.from = from;
		this.angle = angle;
		this.navAreaMask = navAreaMask;
	}

	public void SetRequest(Vector3 startPoint, RandomFloat radius, Vector3 from, float angle, int navAreaMask = 16)
	{
		requsetType = 4;
		requestState = NavMeshRequestState.Solving;
		this.startPoint = startPoint;
		radiusRange = new VariableFloat
		{
			type = VariableType.Random,
			value1 = radius.value1,
			value2 = radius.value2
		};
		this.from = from;
		this.angle = angle;
		this.navAreaMask = navAreaMask;
	}
}
