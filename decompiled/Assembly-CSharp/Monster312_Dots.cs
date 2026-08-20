using Unity.Entities;
using Unity.Mathematics;

public struct Monster312_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public Monster312State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public AIPattern aIPattern;

	public float3 moveDir;

	public float3 tpPosition;

	public Entity matEntity;

	public Entity warningEntity;

	public Entity warningEntity2;

	public float3 warningOffset;

	public float warningScale;

	public float warningScale2;

	public Monster312State state
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
