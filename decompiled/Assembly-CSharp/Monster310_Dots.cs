using Unity.Entities;
using Unity.Mathematics;

public struct Monster310_Dots : IComponentData, IQueryTypeParameter
{
	public struct Monster310_Data
	{
		public float maxJumpDistance;

		public float gravity;

		public float upSpeed;
	}

	public struct Monster310_Data_2
	{
		public float maxJumpDistance;

		public float gravity;

		public float upSpeed;
	}

	public BlobAssetReference<Monster310_Data> data;

	public BlobAssetReference<Monster310_Data_2> data2;

	public RandomFloat jumpOffsetRange;

	public AIPattern pattern;

	public bool Initialized;

	public Monster310State _state;

	public bool changedState;

	public float stateExistTime;

	public bool stateQuit;

	public float maxJumpDistance;

	public float3 lastAimPoint;

	public Monster310State state
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
