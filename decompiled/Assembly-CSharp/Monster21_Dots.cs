using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Monster21_Dots : IComponentData, IQueryTypeParameter
{
	public AIPattern pattern;

	public RandomFloat maxAngleDuration;

	public float moveAngleOffset;

	public float moveAngleOffsetSpeed;

	public float3 randomMoveTrackPoint;

	public RandomFloat blinkInterval;

	public float blinkToPlayerBackAngle;

	public bool angleToLeft;

	public float angleCounter;

	public float maxAngleDurationTimer;

	public Vector3 blinkPoint;

	public float blinkIntervalTimer;

	public Monster21State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public Monster21State state
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
