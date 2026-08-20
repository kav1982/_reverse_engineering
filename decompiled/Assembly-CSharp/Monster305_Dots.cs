using Unity.Entities;

public struct Monster305_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public Monster305State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public Monster305State state
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
