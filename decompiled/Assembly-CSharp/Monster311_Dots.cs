using Unity.Entities;
using Unity.Mathematics;

public struct Monster311_Dots : IComponentData, IQueryTypeParameter
{
	public BlobAssetReference<Monster311_Data> data;

	public bool Initialized;

	public Monster311State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public bool isPattern2;

	public float laserOffset;

	public float3 moveDir;

	public Monster311State state
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
