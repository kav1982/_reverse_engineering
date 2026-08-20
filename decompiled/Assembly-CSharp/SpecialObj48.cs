using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj48 : LayerCorrect, IRoomObjExtraData, ITrap, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	public enum SpecialObj48Type
	{
		Horizontal,
		Vertical,
		Full,
		Rotate,
		RotateSingle
	}

	private enum UnitState
	{
		Idle,
		HorizontalAttack,
		VerticalAttack,
		Rotating,
		RotatingSingle,
		Stop
	}

	[Space(50f)]
	public SpecialObj48Type so48Type = SpecialObj48Type.Full;

	public float idleTime;

	public float attackTime;

	public float laserForwardOffset;

	public float LaserHeight;

	public UnityEngine.Collider thisCollider;

	[Header("Rotate")]
	public float rotateSpeed;

	private UnitState _state;

	private static readonly float[] HorizontalLaserAngles = new float[2] { 90f, 270f };

	private static readonly float[] VerticalLaserAngles = new float[2] { 0f, 180f };

	private static readonly float[] RotatingLaserAngles = new float[4] { 0f, 90f, 180f, 270f };

	private float stateExistTimer;

	private bool stateQuit;

	private bool changedState;

	private bool extraDataInitialized;

	private float extraDataIdleOffset;

	private float currentRotation;

	private int rotateDir = 1;

	private bool fullNextHorizontalAttack = true;

	private List<SpecialObj48_Laser> lasers = new List<SpecialObj48_Laser>();

	private UnitState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTimer = 0f;
			stateQuit = true;
			_state = value;
		}
	}

	public Entity thisEntity { get; set; }

	private void Start()
	{
		rotateDir = ((!(Random.value < 0.5f)) ? 1 : (-1));
		state = UnitState.Idle;
		stateExistTimer = (extraDataInitialized ? (0f - extraDataIdleOffset) : 0f);
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 256u;
		collisionFilter.CollidesWith = DTool.GetCollidesWith(256u);
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTimer += Time.deltaTime;
		switch (state)
		{
		case UnitState.Rotating:
			if (changedState)
			{
				CreateLasers(RotatingLaserAngles, currentRotation);
			}
			currentRotation += rotateSpeed * (float)rotateDir * Time.deltaTime;
			UpdateLasers(RotatingLaserAngles, currentRotation);
			if (stateExistTimer >= attackTime)
			{
				RecycleLasers();
				state = UnitState.Idle;
			}
			break;
		case UnitState.RotatingSingle:
			if (changedState)
			{
				CreateLasers(HorizontalLaserAngles, currentRotation);
			}
			currentRotation += rotateSpeed * (float)rotateDir * Time.deltaTime;
			UpdateLasers(HorizontalLaserAngles, currentRotation);
			break;
		case UnitState.Idle:
			if (stateExistTimer >= idleTime)
			{
				state = GetNextAttackState();
			}
			break;
		case UnitState.HorizontalAttack:
			if (changedState)
			{
				CreateLasers(HorizontalLaserAngles);
			}
			UpdateLasers(HorizontalLaserAngles);
			if (stateExistTimer >= attackTime)
			{
				RecycleLasers();
				state = UnitState.Idle;
			}
			break;
		case UnitState.VerticalAttack:
			if (changedState)
			{
				CreateLasers(VerticalLaserAngles);
			}
			UpdateLasers(VerticalLaserAngles);
			if (stateExistTimer >= attackTime)
			{
				RecycleLasers();
				state = UnitState.Idle;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case UnitState.Stop:
			break;
		}
	}

	private void CreateLasers(float[] angles, float rotation = 0f)
	{
		RecycleLasers();
		for (int i = 0; i < angles.Length; i++)
		{
			float num = angles[i] + rotation;
			SpawnLaser(GetShootPoint(num), Tool2D.GetDir(num));
		}
	}

	private UnitState GetNextAttackState()
	{
		switch (so48Type)
		{
		case SpecialObj48Type.Horizontal:
			return UnitState.HorizontalAttack;
		case SpecialObj48Type.Vertical:
			return UnitState.VerticalAttack;
		case SpecialObj48Type.Full:
		{
			int result = (fullNextHorizontalAttack ? 1 : 2);
			fullNextHorizontalAttack = !fullNextHorizontalAttack;
			return (UnitState)result;
		}
		case SpecialObj48Type.Rotate:
			return UnitState.Rotating;
		case SpecialObj48Type.RotateSingle:
			return UnitState.RotatingSingle;
		default:
			Debug.LogError(so48Type);
			return UnitState.HorizontalAttack;
		}
	}

	private void UpdateLasers(float[] angles, float rotation = 0f)
	{
		if (lasers.Count >= angles.Length)
		{
			for (int i = 0; i < angles.Length; i++)
			{
				float num = angles[i] + rotation;
				lasers[i].SetLaser(GetShootPoint(num), Tool2D.GetDir(num));
			}
		}
	}

	private Vector3 GetShootPoint(float angle)
	{
		return base.transform.position + Tool2D.GetDir(angle) * laserForwardOffset + new Vector3(0f, 0f, 0f - LaserHeight);
	}

	private void SpawnLaser(Vector3 startPoint, Vector3 dir)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_SpecialObj48Laser", delegate(GameObject go)
		{
			go.transform.SetPositionAndRotation(startPoint, Quaternion.identity);
			SpecialObj48_Laser component = go.GetComponent<SpecialObj48_Laser>();
			component.Initialize(startPoint, dir);
			lasers.Add(component);
		});
	}

	private void RecycleLasers()
	{
		for (int i = 0; i < lasers.Count; i++)
		{
			lasers[i].RecycleSelf();
		}
		lasers.Clear();
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (so48Type == SpecialObj48Type.Full)
		{
			fullNextHorizontalAttack = data2 == 0f;
		}
		extraDataInitialized = true;
		extraDataIdleOffset = data1;
	}

	public void SetTrapInvalid()
	{
		RecycleLasers();
		state = UnitState.Stop;
	}
}
