using Unity.Entities;
using Unity.Mathematics;

public struct Monster304_Dots : IComponentData, IQueryTypeParameter
{
	public bool Initialized;

	public float3 moveDir;

	public RandomFloat rotateSpeed;

	public Entity rotateRoot;

	public float flyTime;

	public float speedUpTimer;

	public float speedRotateFix;

	public bool speeding;

	public bool isPattern2;

	public Entity shadowLayer;

	public Entity shadowRotateRoot;

	public Entity flame1;

	public Entity flame2;
}
