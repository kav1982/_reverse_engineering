using Unity.Entities;
using UnityEngine;

public struct Monster7_Dots : IComponentData, IQueryTypeParameter
{
	public RandomFloat blinkInterval;

	public float blinkIntervalTimer;

	public float blinkToPlayerBackAngle;

	public Vector3 blinkPoint;

	public RandomFloat idleTime;

	public RandomFloat randomMoveDistance;

	public Vector3 randomMovePoint;

	public Monster7State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public Monster7State state
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
