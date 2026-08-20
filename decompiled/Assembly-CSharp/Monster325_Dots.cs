using Unity.Entities;
using Unity.Mathematics;

public struct Monster325_Dots : IComponentData, IQueryTypeParameter
{
	public RandomFloat idleTime;

	public RandomFloat randomWalkDistance;

	public RandomFloat randomWalkTime;

	public float randomWalkToTargetAngle;

	public float closeRandomWalkTriggerDistance;

	public RandomFloat closeRandomWalkDistance;

	public float closeRandomWalkToTargetAngle;

	public float moveSpeedRatio;

	public bool Initialized;

	public float3 randomWalkPoint;

	public float targetCheckTimer;

	public Monster325State _state;

	public bool changedState;

	public bool stateQuit;

	public float stateExistTime;

	public Monster325State state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
		}
	}
}
