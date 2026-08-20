using Unity.Entities;

public struct ScreenShakeData : IBufferElementData
{
	public float Radius;

	public float Speed;

	public float Time;
}
