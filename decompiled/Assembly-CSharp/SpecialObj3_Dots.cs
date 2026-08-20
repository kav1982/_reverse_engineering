using Unity.Entities;

public struct SpecialObj3_Dots : IComponentData, IQueryTypeParameter
{
	public SO3Pattern pattern;

	public Entity layerEntity;

	public Entity matEntity;

	public Entity triggerEntity;

	public bool initialized;

	public bool daveDead;

	public bool mode1;

	public float nowAnimaIndex;

	public bool triggered;

	public SO3State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public float animaDelayTime;

	public StateVariableMgr_Dots varMgr;

	public SO3State state
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
