using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss13FCMissile : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public VariableFloat duration;

	public float timer;

	public float shakeDuration;

	public float shakeTimer;

	public VariableFloat moveSpeed;

	public Vector3 moveDir;

	public Transform mine;

	public float shakeFrequency;

	public float shakeAmplitude;

	private Vector3 originModelLocalPosition;

	public Animator anim;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228736u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
		originModelLocalPosition = mine.transform.localPosition;
		timer = 0f;
		shakeTimer = 0f;
		moveSpeed.RandomResult();
		duration.RandomResult();
	}

	private void OnDisable()
	{
		mine.transform.localPosition = originModelLocalPosition;
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
