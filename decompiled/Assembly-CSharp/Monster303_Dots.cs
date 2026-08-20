using Unity.Entities;
using Unity.Mathematics;

public struct Monster303_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public Monster303State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public float3 dashDir;

	public Entity warningEntity;

	public Monster303State state
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
