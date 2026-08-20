using Unity.Entities;
using Unity.Mathematics;

public struct Monster1_Dots : IComponentData, IQueryTypeParameter
{
	public RandomFloat idleTime;

	public RandomFloat randomWalkDistance;

	public RandomFloat randomWalkTime;

	public float followTargetChance;

	public float followTargetDistance;

	public float followTargetTime;

	public bool isInitialized;

	public float bornIdleTimer;

	public float checkIntervalTimer;

	public float idleTimer;

	public float randomWalkTimer;

	public float3 randomWalkPosition;

	public float followTargetTimer;

	public Monster1State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public Monster1State state
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
