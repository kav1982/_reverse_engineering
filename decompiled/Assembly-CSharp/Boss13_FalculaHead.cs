using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss13_FalculaHead : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public enum FalculaState
	{
		Out,
		Hit,
		Miss,
		HitWall,
		HitStay,
		HitBack,
		MissBack,
		Hide
	}

	public UnityEngine.CapsuleCollider CC;

	public float falculaMoveSpeed;

	public Vector3 falculaMoveDir;

	public float hitStayTime;

	public float maxDistance;

	public float maxMoveTime;

	public float slowDownTime;

	public float backTime;

	public float missDelay;

	public bool bindPlayer;

	public bool hasHitted;

	public float missFallSpeed;

	public Transform falculaBody;

	public Transform falculaShadow;

	public float hideTime;

	public float endPointHeight;

	[Header("线")]
	public LineRenderer line;

	public LineRenderer shadow;

	public int pointAmount;

	public Vector3[] pointsPositions;

	public Vector3[] pointsBodyPositions;

	public Vector3[] pointsShadowPositions;

	public float maxPointDistance;

	private float currentHeight;

	private float hitBackHeight;

	private float hitBackDistance;

	[Header("QTE")]
	public GameObject qteBarObj;

	public Transform qteProgressBar;

	public GameObject[] qteTips;

	private float currentQteProgress;

	public float playerKeyWeight;

	public float stickWeight;

	public float mobileWeight;

	public float timeWeight;

	public float timeWeightOrigin;

	public float weightFixedTime;

	public float weightIncreaseSpeed;

	private Vector2 lastStickPosition;

	public FalculaState _state;

	public SpriteRenderer bar;

	private bool mobileNerfed;

	[Header("视野控制")]
	public float zoomAmplitude;

	public ShockParam continuousShock;

	public ShockParam breakShock;

	private Entity pillarEtt;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public Entity thisEntity { get; set; }

	private Vector3 startPoint => new Vector3(Boss13_Stage2.Inst.falculaPivot.position.x, Boss13_Stage2.Inst.transform.position.y, 0f - Boss13_Stage2.Inst.falculaPivot.position.y + Boss13_Stage2.Inst.transform.position.y);

	private Vector3 endPoint => Tool2D.IgnoreZPoint(base.transform.position, currentHeight);

	public FalculaState state
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
			varMgr.Clear();
		}
	}

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228992u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		CC.enabled = true;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
		maxMoveTime = maxDistance / falculaMoveSpeed;
		maxPointDistance = maxDistance / (float)pointAmount;
		SetShow(value: true);
		hasHitted = false;
		falculaShadow.right = falculaMoveDir;
		falculaBody.right = falculaMoveDir;
		if (GameMgr.IsMobile_Static && !mobileNerfed)
		{
			mobileNerfed = true;
			timeWeight *= 0.8f;
			timeWeightOrigin *= 0.8f;
		}
	}

	private void OnDisable()
	{
		falculaShadow.gameObject.SetActive(value: false);
		falculaBody.gameObject.SetActive(value: false);
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
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case FalculaState.Out:
			if (changedState)
			{
				Boss13_Stage2.Inst.PlayAnim("Falculaing");
				SEMgr.Inst.boss13FalculaAttack.PlaySE();
				line.gameObject.SetActive(value: true);
			}
			base.transform.position += falculaMoveDir * falculaMoveSpeed * Time.deltaTime * Mathf.Lerp(1f, 0f, stateExistTime + slowDownTime - maxMoveTime);
			currentHeight = Mathf.Lerp(startPoint.z, endPointHeight, stateExistTime / maxMoveTime);
			if (stateExistTime > maxMoveTime + slowDownTime)
			{
				state = FalculaState.Miss;
			}
			break;
		case FalculaState.Hit:
			if (!changedState)
			{
				break;
			}
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			state = FalculaState.HitStay;
			bindPlayer = true;
			qteBarObj.SetActive(value: true);
			currentQteProgress = 0.5f;
			qteProgressBar.transform.localScale = new Vector3(0.5f, 1f, 1f);
			new CameraFocusSizeData(zoomAmplitude, 1, 1000000f);
			foreach (Boss13Stage3FollowMissile followMissile in Boss13Stage3FollowMissile.followMissiles)
			{
				followMissile.DotsAnnouncedDeath();
			}
			timeWeight = timeWeightOrigin;
			CamController.Inst.FocusOn(5f, 0.5f, PlayerMgr.Inst.PlayerPoint + Vector3.up * 1.2f);
			currentHeight = endPointHeight;
			break;
		case FalculaState.Miss:
			if (stateExistTime > missDelay)
			{
				state = FalculaState.MissBack;
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13_Stage2.Inst.myPpt.myEntity);
				info.damage = 999999f;
				info.ignoreFloatText = true;
				info.knockbackForce = -falculaMoveDir;
				UnitDotsSyncSystem.AddTakeDamageRequest(pillarEtt, info);
			}
			break;
		case FalculaState.HitStay:
			if (changedState)
			{
				Boss13_Stage2.Inst.PlayAnim("FalculaHitBack");
				lastStickPosition = ControlMgr.Inst.GetInputWASD();
				GameObject[] array = qteTips;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(value: false);
				}
				if (GameMgr.IsMobile_Static)
				{
					qteTips[2].SetActive(value: true);
				}
				else if (ControlMgr.Inst.InputType == PlayerInputType.Keyboard)
				{
					qteTips[0].SetActive(value: true);
				}
				else if (ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
				{
					qteTips[1].SetActive(value: true);
				}
			}
			PlayerMgr.Inst.SetPlayerPoint(Tool2D.IgnoreZPoint(base.transform.position));
			currentQteProgress -= timeWeight * Time.deltaTime;
			qteProgressBar.transform.localScale = new Vector3(currentQteProgress, 1f, 1f);
			bar.color = new Color(1f, currentQteProgress, currentQteProgress);
			CamController.Inst.SetShock(continuousShock);
			if (GameMgr.IsMobile_Static)
			{
				if (Input.GetMouseButtonDown(0))
				{
					CamController.Inst.SetShock(breakShock);
					currentQteProgress += mobileWeight;
				}
			}
			else if (ControlMgr.Inst.InputType == PlayerInputType.Keyboard)
			{
				if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
				{
					CamController.Inst.SetShock(breakShock);
					currentQteProgress += playerKeyWeight;
				}
			}
			else if (ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				float num2 = Vector2.Distance(ControlMgr.Inst.GetInputWASD(), lastStickPosition);
				if (num2 > 0.1f)
				{
					CamController.Inst.SetShock(breakShock);
					currentQteProgress += num2 * stickWeight * Time.deltaTime;
				}
				lastStickPosition = ControlMgr.Inst.GetInputWASD();
			}
			if (currentQteProgress >= 1f)
			{
				bindPlayer = false;
				PlayerMgr.Inst.PlayerCtrller.StartMotion();
				qteBarObj.SetActive(value: false);
				state = FalculaState.HitBack;
			}
			if (currentQteProgress <= 0f)
			{
				state = FalculaState.HitBack;
			}
			if (stateExistTime > weightFixedTime)
			{
				timeWeight += weightIncreaseSpeed * Time.deltaTime;
			}
			break;
		case FalculaState.HitBack:
		{
			if (changedState)
			{
				qteBarObj.SetActive(value: false);
				SEMgr.Inst.boss13FalculaReturn.PlaySE();
				CamController.Inst.FocusRecover(0.5f);
				hitBackDistance = Tool2D.IgnoreZDistance(base.transform.position, startPoint);
				hitBackHeight = currentHeight;
			}
			if (stateExistTime < backTime)
			{
				base.transform.position += Tool2D.IgnoreZDistance(base.transform.position, startPoint) * Tool2D.IgnoreZV2ToV1Normal(base.transform.position, startPoint) * Time.deltaTime / (stateExistTime - backTime);
			}
			else
			{
				base.transform.position = Tool2D.IgnoreZPoint(startPoint);
			}
			float num = Tool2D.IgnoreZDistance(base.transform.position, startPoint);
			currentHeight = Mathf.Lerp(startPoint.z, hitBackHeight, num / hitBackDistance);
			if (bindPlayer)
			{
				PlayerMgr.Inst.SetPlayerPoint(Tool2D.IgnoreZPoint(base.transform.position));
			}
			if (!(num < 1.7f))
			{
				break;
			}
			if (Boss13_Stage2.Inst.state != Boss13_Stage2.MonsterState.Dead)
			{
				if (bindPlayer)
				{
					Boss13_Stage2.Inst.state = Boss13_Stage2.MonsterState.FalculaFail;
				}
				else
				{
					Boss13_Stage2.Inst.FalculaEnd();
				}
			}
			line.gameObject.SetActive(value: false);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			break;
		}
		case FalculaState.MissBack:
			if (changedState)
			{
				Boss13_Stage2.Inst.PlayAnim("FalculaMissBack");
				falculaMoveDir = Tool2D.IgnoreZV2ToV1Normal(Boss13_Stage2.Inst.falculaPivot, base.transform);
				SEMgr.Inst.boss13FalculaReturn.PlaySE();
				hitBackDistance = Tool2D.IgnoreZDistance(base.transform.position, startPoint);
				hitBackHeight = currentHeight;
			}
			if (stateExistTime < backTime)
			{
				base.transform.position += Tool2D.IgnoreZDistance(base.transform.position, startPoint) * Tool2D.IgnoreZV2ToV1Normal(base.transform.position, startPoint) * Time.deltaTime / (stateExistTime - backTime);
			}
			else
			{
				base.transform.position = Tool2D.IgnoreZPoint(startPoint);
				SetShow(value: false);
			}
			currentHeight = Mathf.Lerp(startPoint.z, hitBackHeight, Tool2D.IgnoreZDistance(base.transform.position, startPoint) / hitBackDistance);
			if ((double)Tool2D.IgnoreZDistanceSqr(startPoint, base.transform.position) < 0.1 && stateExistTime > 1f)
			{
				state = FalculaState.Hide;
				Boss13_Stage2.Inst.FalculaEnd();
				SetShow(value: false);
			}
			break;
		case FalculaState.Hide:
			if (stateExistTime > hideTime)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			break;
		}
		for (int j = 0; j < pointsPositions.Length; j++)
		{
			pointsPositions[j] = Vector3.Lerp(startPoint, endPoint, (float)j / (float)(pointsPositions.Length - 1));
			pointsShadowPositions[j] = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(pointsPositions[j]), LayerCorrectType.Shadow);
			pointsBodyPositions[j] = Tool2D.GetLayerPoint(pointsPositions[j], LayerCorrectType.Coordinate);
		}
		line.SetPositions(pointsBodyPositions);
		shadow.SetPositions(pointsShadowPositions);
		Vector3 vector = endPoint - startPoint;
		if (vector != Vector3.zero)
		{
			falculaBody.right = new Vector3(vector.x, vector.y - vector.z);
			falculaShadow.right = falculaMoveDir;
		}
		falculaShadow.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		falculaBody.position = Tool2D.GetLayerPoint(endPoint, LayerCorrectType.Coordinate);
	}

	public void SetShow(bool value)
	{
		line.gameObject.SetActive(value);
		shadow.gameObject.SetActive(value);
		falculaBody.gameObject.SetActive(value);
		falculaShadow.gameObject.SetActive(value);
	}

	private void FixedUpdate()
	{
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		if (state != 0)
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		switch (layer)
		{
		case 256u:
		case 131072u:
			state = FalculaState.Miss;
			if (layer == 131072)
			{
				pillarEtt = other;
			}
			CC.enabled = false;
			break;
		case 512u:
		case 2097152u:
			if (other == PlayerMgr.Inst.PlayerEtt)
			{
				if (!hasHitted)
				{
					state = FalculaState.Hit;
					hasHitted = true;
				}
			}
			else
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
				info.damage = 999f;
				info.ignoreFloatText = true;
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			}
			break;
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
