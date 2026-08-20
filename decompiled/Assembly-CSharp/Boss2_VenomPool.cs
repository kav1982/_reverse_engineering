using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss2_VenomPool : LayerCorrect, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	private enum VenomState
	{
		Largen,
		LargenBefore,
		Duration,
		Minify
	}

	[Space(50f)]
	public Transform tsf_PS;

	public float initialVolume;

	public float biggerSpeed;

	public float smallerSpeed;

	public float stopTime;

	private VenomState state;

	private float currentSize;

	private float durationTime;

	private float stopTimer;

	private float durationBeforeTime;

	private float durationBeforeTimer;

	public UnityEngine.CapsuleCollider cc;

	private bool venomTagAdded;

	public Entity thisEntity { get; set; }

	private void Start()
	{
		currentSize = initialVolume;
		Vector3 localScale = Vector3.one * Mathf.Sqrt(currentSize);
		tsf_PS.position = Tool2D.IgnoreZPoint(base.transform, 1.14f);
		tsf_Layer.localScale = localScale;
		tsf_PS.localScale = localScale;
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 128u;
		collisionFilter.CollidesWith = 1073741824u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, cc);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		if (!venomTagAdded && UnitDotsSyncSystem.EntityIsValid(thisEntity))
		{
			venomTagAdded = true;
			UnitDotsSyncSystem.entityMgr.AddComponent<VenomTag>(thisEntity);
		}
		if (UnitDotsSyncSystem.EntityIsValid(thisEntity))
		{
			LocalTransform componentData = UnitDotsSyncSystem.GetComponentData<LocalTransform>(thisEntity);
			componentData.Scale = Mathf.Sqrt(currentSize);
			UnitDotsSyncSystem.SetComponentData(componentData, thisEntity);
		}
		Vector3 one = Vector3.one;
		switch (state)
		{
		case VenomState.Largen:
			currentSize += biggerSpeed * Time.deltaTime;
			one = Vector3.one * Mathf.Sqrt(currentSize);
			tsf_Layer.localScale = one;
			tsf_PS.localScale = one;
			break;
		case VenomState.LargenBefore:
			currentSize += biggerSpeed * Time.deltaTime;
			one = Vector3.one * Mathf.Sqrt(currentSize);
			tsf_Layer.localScale = one;
			tsf_PS.localScale = one;
			durationBeforeTimer += Time.deltaTime;
			if (durationBeforeTimer >= durationBeforeTime)
			{
				durationBeforeTimer = 0f;
				state = VenomState.Duration;
			}
			break;
		case VenomState.Duration:
			stopTimer += Time.deltaTime;
			if (stopTimer >= durationTime)
			{
				stopTimer = 0f;
				state = VenomState.Minify;
			}
			break;
		case VenomState.Minify:
			currentSize -= smallerSpeed * Time.deltaTime;
			if (currentSize > 0f)
			{
				one = Vector3.one * Mathf.Sqrt(currentSize);
				tsf_Layer.localScale = one;
				tsf_PS.localScale = one;
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public void StopAndMinify()
	{
		state = VenomState.LargenBefore;
	}
}
