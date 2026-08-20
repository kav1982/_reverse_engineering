using Unity.Entities;
using Unity.Mathematics;

public struct Monster327Missle_Dots : IComponentData, IQueryTypeParameter
{
	public Entity rotateRoot;

	public Entity rotateShadow;

	public float straightTime;

	public float straightSpeed;

	public float homingSpeed;

	public RandomFloat maxTurnAnglePerSecond;

	public float lifeTime;

	public float explosionEffectScale;

	public float explosionColliderRadius;

	public float explosionTouchDuration;

	public float3 explosionOffset;

	public bool initialized;

	public bool isExploding;

	public float lifeTimer;

	public float straightTimer;

	public float explosionTimer;

	public float3 currentDirection;

	public Monster327MissileState state;
}
