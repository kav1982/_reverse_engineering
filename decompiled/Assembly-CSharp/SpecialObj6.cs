using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj6 : LayerCorrect, IRoomObjExtraData, ITrap, IRoomCtrller, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	private enum UnitState
	{
		HorizentalIdle,
		HorizentalAttack,
		VerticleIdle,
		VerticleAttack,
		Rotating,
		OneDirIdle,
		OneDirAttack,
		Stop
	}

	[Space(50f)]
	public SpecialObj6Type so6Type;

	public Transform tsf_GroundEffect;

	public SpriteRenderer sr;

	public float idleTime;

	public float attackTime;

	public float attackInterval;

	public float stingOutForwardOffset;

	public float stingOutHeight;

	public UnityEngine.Collider thisCollider;

	[Header("Rotate")]
	public float rotateSpeed;

	private UnitState state;

	private Vector3 shootPointUp;

	private Vector3 shootPointRight;

	private Vector3 shootPointDown;

	private Vector3 shootPointLeft;

	private float idleTimer = 99999f;

	private float attackTimer;

	private float attackIntervalTimer;

	private bool isStop;

	private bool playerInRoom;

	private float currentRotation;

	private RoomController belongRoom;

	public Entity thisEntity { get; set; }

	private void Start()
	{
		shootPointUp = base.transform.position + Tool2D.GetDir(0f) * stingOutForwardOffset + new Vector3(0f, 0f, 0f - stingOutHeight);
		shootPointLeft = base.transform.position + Tool2D.GetDir(90f) * stingOutForwardOffset + new Vector3(0f, 0f, 0f - stingOutHeight);
		shootPointDown = base.transform.position + Tool2D.GetDir(180f) * stingOutForwardOffset + new Vector3(0f, 0f, 0f - stingOutHeight);
		shootPointRight = base.transform.position + Tool2D.GetDir(270f) * stingOutForwardOffset + new Vector3(0f, 0f, 0f - stingOutHeight);
		if (belongRoom.roomCfg.isFlipped)
		{
			if (so6Type == SpecialObj6Type.Left)
			{
				sr.material.SetFloat(GameConstManaged.shaderFlipXIndex, 1f);
				sr.flipX = false;
				so6Type = SpecialObj6Type.Right;
				tsf_GroundEffect.localScale = new Vector3(-1f, 1f, 1f);
			}
			else if (so6Type == SpecialObj6Type.Right)
			{
				sr.material.SetFloat(GameConstManaged.shaderFlipXIndex, -1f);
				sr.flipX = true;
				so6Type = SpecialObj6Type.Left;
				tsf_GroundEffect.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
		else if (so6Type == SpecialObj6Type.Left)
		{
			sr.material.SetFloat(GameConstManaged.shaderFlipXIndex, -1f);
		}
		else if (so6Type == SpecialObj6Type.Right)
		{
			sr.material.SetFloat(GameConstManaged.shaderFlipXIndex, 1f);
		}
		tsf_GroundEffect.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.GroundEffectLow);
		tsf_GroundEffect.SetParent(base.transform.parent);
		if (so6Type != 0)
		{
			state = UnitState.OneDirIdle;
		}
		if (so6Type == SpecialObj6Type.Rotate || so6Type == SpecialObj6Type.reRotate)
		{
			state = UnitState.Rotating;
		}
		if (so6Type == SpecialObj6Type.Verticle)
		{
			state = UnitState.VerticleIdle;
		}
		if (so6Type == SpecialObj6Type.Horizental)
		{
			state = UnitState.HorizentalIdle;
		}
		belongRoom.RoomEnterRegister(delegate
		{
			playerInRoom = true;
		});
		belongRoom.RoomLeaveRegister(delegate
		{
			playerInRoom = false;
		});
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
		switch (state)
		{
		case UnitState.Rotating:
			if (isStop)
			{
				state = UnitState.Stop;
			}
			if (so6Type == SpecialObj6Type.Rotate)
			{
				currentRotation += rotateSpeed * Time.deltaTime;
			}
			else
			{
				currentRotation -= rotateSpeed * Time.deltaTime;
			}
			attackIntervalTimer += Time.deltaTime;
			while (attackIntervalTimer >= attackInterval)
			{
				attackIntervalTimer -= attackInterval;
				shootPointUp = base.transform.position + Tool2D.GetDir(0f + currentRotation) * stingOutForwardOffset + new Vector3(0f, 0f, 0f - stingOutHeight);
				shootPointLeft = base.transform.position + Tool2D.GetDir(90f + currentRotation) * stingOutForwardOffset + new Vector3(0f, 0f, 0f - stingOutHeight);
				shootPointDown = base.transform.position + Tool2D.GetDir(180f + currentRotation) * stingOutForwardOffset + new Vector3(0f, 0f, 0f - stingOutHeight);
				shootPointRight = base.transform.position + Tool2D.GetDir(270f + currentRotation) * stingOutForwardOffset + new Vector3(0f, 0f, 0f - stingOutHeight);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointUp).GetComponent<StingOut>().Initialize(this, Tool2D.GetDir(0f + currentRotation));
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointLeft).GetComponent<StingOut>().Initialize(this, Tool2D.GetDir(90f + currentRotation));
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointDown).GetComponent<StingOut>().Initialize(this, Tool2D.GetDir(180f + currentRotation));
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointRight).GetComponent<StingOut>().Initialize(this, Tool2D.GetDir(270f + currentRotation));
			}
			break;
		case UnitState.HorizentalIdle:
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime)
			{
				idleTimer = 0f;
				state = UnitState.HorizentalAttack;
				attackIntervalTimer = 0f;
				if (playerInRoom)
				{
					SEMgr.Inst.so6.PlaySE();
				}
			}
			break;
		case UnitState.HorizentalAttack:
			attackIntervalTimer += Time.deltaTime;
			if (attackIntervalTimer >= attackInterval)
			{
				attackIntervalTimer = 0f;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointLeft).GetComponent<StingOut>().Initialize(this, Tool2D.GetDirByFourDir(FourDir.Left));
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointRight).GetComponent<StingOut>().Initialize(this, Tool2D.GetDirByFourDir(FourDir.Right));
			}
			attackTimer += Time.deltaTime;
			if (attackTimer >= attackTime)
			{
				attackTimer = 0f;
				if (isStop)
				{
					state = UnitState.Stop;
				}
				else if (so6Type == SpecialObj6Type.Full)
				{
					state = UnitState.VerticleIdle;
				}
				else
				{
					state = UnitState.HorizentalIdle;
				}
			}
			break;
		case UnitState.VerticleIdle:
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime)
			{
				idleTimer = 0f;
				state = UnitState.VerticleAttack;
				attackIntervalTimer = 0f;
				if (playerInRoom)
				{
					SEMgr.Inst.so6.PlaySE();
				}
			}
			break;
		case UnitState.VerticleAttack:
			attackIntervalTimer += Time.deltaTime;
			if (attackIntervalTimer >= attackInterval)
			{
				attackIntervalTimer = 0f;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointUp).GetComponent<StingOut>().Initialize(this, Tool2D.GetDirByFourDir(FourDir.Up));
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointDown).GetComponent<StingOut>().Initialize(this, Tool2D.GetDirByFourDir(FourDir.Down));
			}
			attackTimer += Time.deltaTime;
			if (attackTimer >= attackTime)
			{
				attackTimer = 0f;
				if (isStop)
				{
					state = UnitState.Stop;
				}
				else if (so6Type == SpecialObj6Type.Full)
				{
					state = UnitState.HorizentalIdle;
				}
				else
				{
					state = UnitState.VerticleIdle;
				}
			}
			break;
		case UnitState.OneDirIdle:
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime)
			{
				idleTimer = 0f;
				state = UnitState.OneDirAttack;
				attackIntervalTimer = 0f;
				if (playerInRoom)
				{
					SEMgr.Inst.so6.PlaySE();
				}
			}
			break;
		case UnitState.OneDirAttack:
			attackIntervalTimer += Time.deltaTime;
			if (attackIntervalTimer >= attackInterval)
			{
				attackIntervalTimer = 0f;
				switch (so6Type)
				{
				case SpecialObj6Type.Up:
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointUp).GetComponent<StingOut>().Initialize(this, Tool2D.GetDirByFourDir(FourDir.Up));
					break;
				case SpecialObj6Type.Right:
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointRight).GetComponent<StingOut>().Initialize(this, Tool2D.GetDirByFourDir(FourDir.Right));
					break;
				case SpecialObj6Type.Down:
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointDown).GetComponent<StingOut>().Initialize(this, Tool2D.GetDirByFourDir(FourDir.Down));
					break;
				case SpecialObj6Type.Left:
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOut", shootPointLeft).GetComponent<StingOut>().Initialize(this, Tool2D.GetDirByFourDir(FourDir.Left));
					break;
				default:
					Debug.LogError(so6Type);
					break;
				}
			}
			attackTimer += Time.deltaTime;
			if (attackTimer >= attackTime)
			{
				attackTimer = 0f;
				if (isStop)
				{
					state = UnitState.Stop;
				}
				else
				{
					state = UnitState.OneDirIdle;
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case UnitState.Stop:
			break;
		}
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (so6Type == SpecialObj6Type.Full && data1 == 1f)
		{
			state = UnitState.VerticleIdle;
		}
		idleTimer = idleTime - data2;
	}

	public void SetTrapInvalid()
	{
		isStop = true;
		UnitState unitState = state;
		if (unitState == UnitState.HorizentalIdle || unitState == UnitState.VerticleIdle || unitState == UnitState.OneDirIdle)
		{
			state = UnitState.Stop;
		}
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongRoom = levelCtrller;
	}
}
