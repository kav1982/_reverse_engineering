using Unity.Entities;
using Unity.Mathematics;

public struct Monster317_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public Monster317State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public float relativeDistance;

	public float spellSpeed;

	public int shootCount;

	public float shootInterval;

	public float shootDistanceInterval;

	public bool isPattern2;

	public float attackShootTimer;

	public int attackShootCount;

	public float3 attackTargetPoint;

	public float3 attackTargetPoint2;

	public float3 startShootPoint;

	public float flyTime;

	public Monster317State state
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
