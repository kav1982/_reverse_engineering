using Unity.Entities;
using Unity.Mathematics;

public struct Monster308_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public Monster308State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public float moveTime;

	public float3 moveDir;

	public float3 dashDir;

	public Entity triggerEntity;

	public Entity warningEntity;

	public Monster308State state
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
