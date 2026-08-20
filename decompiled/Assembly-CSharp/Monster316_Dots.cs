using Unity.Entities;
using Unity.Mathematics;

public struct Monster316_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public Monster316State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public float3 moveDir;

	public float moveTimer;

	public float checkTimer;

	public Monster316State state
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
