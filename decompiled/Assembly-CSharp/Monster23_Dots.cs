using Unity.Entities;

public struct Monster23_Dots : IComponentData, IQueryTypeParameter
{
	public VariableFloat roamRadius;

	public RandomFloat deadSpellCount;

	public VariableFloat tantacleWaveSpeed;

	public RandomFloat tantacleWaveRatio;

	public bool initialized;

	public Monster23State _state;

	public bool changedState;

	public float stateExistTime;

	public AIPattern pattern;

	public bool stateQuit;

	public Monster23State state
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
