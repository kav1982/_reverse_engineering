using Unity.Entities;
using Unity.Mathematics;

public struct Monster309_Dots : IComponentData, IQueryTypeParameter
{
	public AIPattern pattern;

	public bool Initialized;

	public Monster309State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public float moveTime;

	public float3 moveDir;

	public float3 lastAimPoint;

	public Monster309State state
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
