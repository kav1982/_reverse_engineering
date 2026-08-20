using Unity.Entities;
using Unity.Mathematics;

public struct Monster314_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public Monster314State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public float cureHpPercent;

	public float cureRadius;

	public Entity followEntity;

	public float cureCDTimer;

	public float cureHpPercentTotal;

	public float3 randomMoveDir;

	public float randomMoveTimer;

	public Monster314State state
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
