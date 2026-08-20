using Unity.Entities;
using Unity.Mathematics;

public struct Monster8_Dots : IComponentData, IQueryTypeParameter
{
	public float moveThreshold;

	public float checkRadius;

	public float amazeRadius;

	public RandomFloat idleTime;

	public RandomFloat randomWalkTime;

	public RandomFloat randomWalkRadius;

	public float followTargetChance;

	public float followTargetTime;

	public bool isInitialized;

	public float checkIntervalTimer;

	public float3 randomWalkPosition;

	public Monster8State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public Monster8State state
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
