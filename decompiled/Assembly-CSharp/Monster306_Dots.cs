using Unity.Entities;

public struct Monster306_Dots : IComponentData, IQueryTypeParameter
{
	public RandomFloat turretRotateAngle;

	public float turretRotateSpeed;

	public float turretRotateInterval;

	public float attackCD;

	public float attackCDTimer;

	public Entity turretEntity;

	public Entity turretBackEntity;

	public float attackRange;

	public float shootInterval;

	public int shootCount;

	public bool Initialized;

	public Monster306State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public StateVariableMgr_Dots varMgr;

	public Monster306State state
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
			varMgr.Clear();
		}
	}
}
