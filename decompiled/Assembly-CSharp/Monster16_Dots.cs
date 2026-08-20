using Rukhanka;
using Unity.Entities;

public struct Monster16_Dots : IComponentData, IQueryTypeParameter
{
	public AIPattern pattern;

	public VariableFloat idleTime;

	public VariableFloat moveRandomRadius;

	public float rockDistance;

	public float rockRotateSpeed;

	public float rockAngle;

	public Monster16State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public bool AnimaNeedReset;

	public Monster16State state
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

	public void AnimaReset(Entity entity, EntityManager entityMgr)
	{
		DynamicBuffer<AnimatorControllerParameterComponent> buffer = entityMgr.GetBuffer<AnimatorControllerParameterComponent>(entity);
		AnimatorControllerParameterComponent value = buffer[0];
		value.IntValue = 0;
		buffer[0] = value;
		AnimaNeedReset = false;
	}

	public void AnimaPlay(Entity entity, EntityManager entityMgr, Monster16AnimaName name)
	{
		DynamicBuffer<AnimatorControllerParameterComponent> buffer = entityMgr.GetBuffer<AnimatorControllerParameterComponent>(entity);
		AnimatorControllerParameterComponent value = buffer[0];
		value.IntValue = (int)name;
		buffer[0] = value;
		AnimaNeedReset = true;
	}
}
