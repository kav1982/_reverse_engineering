using Unity.Entities;

public struct Monster302_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public Monster302State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public RandomFloat MoveRange;

	public RandomFloat AimRange;

	public float aimTime;

	public float shootIntervalTimer;

	public Monster302State state
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
