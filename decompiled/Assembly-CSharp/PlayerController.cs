using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Spine;
using Spine.Unity;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	private static readonly int IsFlip = Shader.PropertyToID("_IsFlip");

	public bool InAbyss;

	public MeshRenderer mr;

	public GameObject AimLine;

	public UIPotionUse uiPotionUse;

	public Rigidbody rigid;

	public Transform tsf_WandRoot;

	public Transform tsf_WandPoint;

	public LayerCorrect layerC_PlayerRTRoot;

	public float motionLerp;

	[SerializeField]
	private float textFloatInterval;

	[Header("FollowObj")]
	public float followObjDistance;

	public float followObjLerp;

	[Header("Anima")]
	public VariableFloat emojiBlinkInterval;

	public float takeDamageAmazeTime;

	[Header("FootStep")]
	public float footstepSEPlayDistance;

	public ParticleSystem ps_FootStepSmoke;

	public VariableFloat footStepPitch;

	private float footstepSEPlayCounter;

	[Header("隐身层级处理")]
	public UnityEngine.Material mat_Normal;

	public UnityEngine.Material mat_Invisible;

	[Header("女流吃甘蔗")]
	public VariableFloat nvliuIdleTime;

	public float nvliuEatingTime;

	private float nvliuIdleTimer;

	private float nvliuEatingTimer;

	private bool isNvliuEating;

	[Header("戴夫鱼叉枪")]
	public Transform DaveHoldingHarpoonsGunTrans;

	public SpriteRenderer DaveHoldingHarpoonGunSprite;

	public SpriteRenderer DaveHoldingHarpoonsGunShadowSprite;

	private Slot leftHand;

	private Slot rightHand;

	private PlayerEmojiType emojiType = PlayerEmojiType.Normal;

	private Vector3 knockback;

	private float changeWandCooling;

	private float dashOverHeatTimer;

	private float lastDashOverHeatTimeMax;

	private float emojiBlinkIntervalTimer;

	private float takeDamageAmazeTimer;

	private List<TextFloatQueueContent> queueTypes = new List<TextFloatQueueContent>();

	private float textFloatTimer;

	private InputActions inputActions;

	public Vector3 inputRightStickLast;

	private Entity currentAimEntity;

	private bool isHoldMouse1;

	private float shootSlowdownTimer;

	private float shootSlowdownRateLastRecord = 1f;

	private bool usingPotion;

	private float usePotionTimer;

	private bool showdrinking;

	private Vector3 ShootWorldPointCurrentFrameAfterLerp;

	private Vector3 ShootWorldPointCurrentFrame;

	public Vector3 shotWorldPointAfterLerp;

	public Vector3 shotWorldPointWithOutLerp;

	public float mobileShotPointRotateAngle = MathF.PI;

	private Vector3 mouseWorldPoint;

	private SpriteRenderer[] invisibleSRs;

	private MeshRenderer[] invisibleMRs;

	private LineRenderer[] invisibleLRs;

	public int unmovableCounter;

	private int nonInteractiveCounter;

	private bool frozenBeforeKinematic;

	private Vector3 frozenBeforeRigid;

	private Vector3 frozenBeforeMotion;

	private Slot handSlotL;

	private Slot handSlotR;

	private PointerEventData eventData = new PointerEventData(EventSystem.current);

	private List<RaycastResult> rayacstResults = new List<RaycastResult>();

	private List<GameObject> followObjs = new List<GameObject>();

	private Vector3 lastFramePos = Vector3.zero;

	private Wand chargingWand;

	public float PlayerColliderDefaultRadius;

	public UnityEngine.Collider playerCollider;

	public UnityEngine.Collider playerTrigger;

	private float currentPlayerSize = 1f;

	public GameObject mobileCamControl;

	private float _lastShowAutoWandCannotCastTime;

	private EntityManager ettMgr;

	public SkeletonAnimation SAnima => myPpt.SAnima;

	public Vector3 CurrentDir { get; private set; }

	public Vector3 CurrentMoveDir { get; private set; } = Vector3.zero;


	public Vector3 CurrentMotion { get; set; }

	public PlayerBodyAnima CurrentBodyAnima { get; private set; }

	public float CurrentAnimaTimeScale { get; private set; } = 1f;


	private DynamicBuffer<UnitKeepCastingSpells> keepCastingEntities => World.DefaultGameObjectInjectionWorld.EntityManager.GetBuffer<UnitKeepCastingSpells>(PlayerMgr.Inst.PlayerEtt);

	public bool IsKeepCasting => keepCastingEntities.Length > 0;

	public UnitProperty myPpt { get; private set; }

	public float dashOverHeatProgressRatio => dashOverHeatTimer / lastDashOverHeatTimeMax;

	public bool isDashOverHeat => dashOverHeatTimer > 0f;

	public Vector2 inputLeftStick { get; private set; }

	public Vector3 inputLeftStickLast { get; private set; }

	public Vector3 inputRightStick { get; private set; } = Vector3.zero;


	public Vector3 inputStickLast { get; private set; } = Vector3.right;


	public bool isHoldMouse0 { get; private set; }

	public float shootSlowdownRate { get; private set; } = 1f;


	public bool castSpellLock { get; private set; }

	public Vector3 ShootWorldPoint
	{
		get
		{
			Vector3 vector = shotWorldPointAfterLerp;
			Vector3 result = vector;
			if (PlayerMgr.Inst.ItemCtrller.curse_IsIsaacVirus)
			{
				Vector3 b = new Vector3(0.5f, 0.5f, 0f);
				Vector3 b2 = new Vector3(0.5f, -0.5f, 0f);
				Vector3 a = vector - base.transform.position;
				Vector3 vector2 = (((!(base.transform.position.x < vector.x) || !(base.transform.position.y < vector.y)) && (!(base.transform.position.x > vector.x) || !(base.transform.position.y > vector.y))) ? Tool2D.Project(a, b2) : Tool2D.Project(a, b));
				result = vector2 + base.transform.position;
			}
			return result;
		}
	}

	public Vector3 ShootWorldPointOrigin => ShotWorldPointTarget;

	public Vector3 ShotWorldPointTarget
	{
		get
		{
			if (!GameMgr.IsMobile_Static && UIMgr.Inst.InputType != PlayerInputType.Gamepad)
			{
				return ShootPointPC;
			}
			return ShootPointStick;
		}
	}

	private Vector3 ShootPointStick
	{
		get
		{
			if (TopUI.inst.uI_AimSkill.useSkillDir)
			{
				return base.transform.position + TopUI.inst.uI_AimSkill.aimDir.normalized * 8f;
			}
			if (GameMgr.IsMobile_Static && !MobileMgr.inst.gamepadPlugged)
			{
				GameMgr.Inst.playerMgr.PlayerCtrller.AimLine.SetActive(isHoldMouse0 && inputRightStick != Vector3.zero);
			}
			if (inputRightStick != Vector3.zero)
			{
				return AimWithoutTarget(8f);
			}
			if (GameMgr.IsMobile_Static)
			{
				if (!isHoldMouse0)
				{
					currentAimEntity = Entity.Null;
					if (GameMgr.IsMobile_Static && DataMgr.settingData.Mobiledata.aimType == MobileData.AimType.WeakAutoAim)
					{
						return AimWithoutTarget(8f);
					}
				}
			}
			else
			{
				currentAimEntity = Entity.Null;
			}
			if (currentAimEntity != Entity.Null && ettMgr.Exists(currentAimEntity) && ettMgr.HasComponent<LocalTransform>(currentAimEntity))
			{
				if (!GameMgr.IsMobile_Static)
				{
					return Tool2D.IgnoreZPoint(ettMgr.GetComponentData<LocalTransform>(currentAimEntity).Position);
				}
				bool flag = true;
				Vector3 vector = ettMgr.GetComponentData<LocalTransform>(currentAimEntity).Position;
				if (GameMgr.IsMobile_Static && DataMgr.settingData.Mobiledata.halfAutoAimRange)
				{
					Vector3 to = vector - base.transform.position;
					flag = Vector3.Angle(inputLeftStickLast, to) <= 90f;
				}
				if (flag)
				{
					Vector3 vector2 = Tool2D.IgnoreZPoint(vector);
					inputStickLast = (vector2 - base.transform.position).normalized;
					return vector2;
				}
			}
			else
			{
				currentAimEntity = Entity.Null;
			}
			if (LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count != 0)
			{
				Vector3 vector2 = AimthRearchTarget();
				inputStickLast = (vector2 - base.transform.position).normalized;
				return vector2;
			}
			return AimWithoutTarget(8f);
		}
	}

	private Vector3 ShootPointPC => mouseWorldPoint;

	public bool IsVisible { get; private set; } = true;


	public bool CanMotion => unmovableCounter == 0;

	public bool CanInteractive
	{
		get
		{
			if (nonInteractiveCounter != 0)
			{
				return false;
			}
			return true;
		}
	}

	public bool isFrozen => myPpt.FronzenState == UnitProperty.Affect_FrozenState.Frozening;

	[HideInInspector]
	public float distanceMoveInLastFrame { get; private set; }

	[HideInInspector]
	public bool isStandInLastFrame { get; private set; }

	public bool IsCharging => chargingWand != null;

	public Entity PlayerAroundCenterEtt { get; private set; }

	public event Action<Vector3> OnWillTheme6Reposition;

	public event Action<Vector3> OnTheme6Reposition;

	public Vector3 AimWithoutTarget(float range)
	{
		if (inputRightStick != Vector3.zero)
		{
			if (PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot != null)
			{
				return base.transform.position + inputRightStick * PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot.RelicCfg.float1.result;
			}
			return base.transform.position + inputRightStick.normalized * range;
		}
		if (GameMgr.IsMobile_Static && PlayerMgr.Inst.inDashSpell)
		{
			return base.transform.position + inputRightStickLast.normalized * range;
		}
		if (inputLeftStick != Vector2.zero)
		{
			return base.transform.position + (Vector3)inputLeftStick.normalized * range;
		}
		if (inputStickLast != Vector3.zero)
		{
			return base.transform.position + inputStickLast.normalized * range;
		}
		return base.transform.position + Vector3.right.normalized * range;
	}

	public Vector3 AimthRearchTarget()
	{
		Vector3 result = base.transform.position + inputRightStick.normalized * 12f;
		float num = 230f;
		bool flag = false;
		foreach (var item in from ett in LevelMgr.Inst.CurrentRoomCtrller.targetableEttList
			where ettMgr.GetComponentData<UnitProperty_Dots>(ett).CanBeTarget
			select (ett, ettMgr.GetComponentData<LocalTransform>(ett).Position))
		{
			float num2 = DistanceSqr(item.Item2, base.transform.position);
			bool flag2 = true;
			if (GameMgr.IsMobile_Static && DataMgr.settingData.Mobiledata.halfAutoAimRange)
			{
				Vector3 to = (Vector3)item.Item2 - base.transform.position;
				flag2 = Vector3.Angle(inputLeftStickLast, to) <= 90f;
			}
			if (flag2 && num2 < num)
			{
				currentAimEntity = item.Item1;
				flag = true;
				num = num2;
				result = Tool2D.IgnoreZPoint(item.Item2);
			}
		}
		if (!flag)
		{
			return AimWithoutTarget(8f);
		}
		return result;
	}

	public void InitPadDirection()
	{
		inputStickLast = Vector3.right;
	}

	public float DistanceSqr(Vector3 a, Vector3 b)
	{
		return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
	}

	private void Awake()
	{
		myPpt = GetComponent<UnitProperty>();
	}

	private void OnEnable()
	{
		inputActions = ControlMgr.Inst.inputActions;
		isHoldMouse0 = false;
		inputActions.Player.WASD.performed += MovementPerformed;
		inputActions.Player.WASD.canceled += MovementCanceled;
		inputActions.Player.GamepadDirect.performed += MovementPerformed_Dpad;
		inputActions.Player.GamepadDirect.canceled += MovementCanceled_Dpad;
		inputActions.Player.LeftStick.performed += MovementPerformed;
		inputActions.Player.LeftStick.canceled += MovementCanceled;
		inputActions.Player.Shoot.performed += ShootPerformed;
		inputActions.Player.Shoot.canceled += ShootCanceld;
		inputActions.Player.Drink.performed += DrinkPerformed;
		inputActions.Player.Drink.canceled += DrinkCanceld;
		inputActions.Player.Alpha1.performed += Alpha1Performed;
		inputActions.Player.Alpha2.performed += Alpha2Performed;
		inputActions.Player.Alpha3.performed += Alpha3Performed;
		inputActions.Player.Alpha4.performed += Alpha4Performed;
		inputActions.Player.Alpha5.performed += Alpha5Performed;
		inputActions.Player.Alpha6.performed += Alpha6Performed;
		inputActions.Player.Alpha7.performed += Alpha7Performed;
		inputActions.Player.WandUp.performed += WandUpPerformed;
		inputActions.Player.WandDown.performed += WandDownPerformed;
		inputActions.Player.RightStick.performed += RightStickPerformed;
		inputActions.Player.RightStick.canceled += RightStickCancled;
		inputActions.Player.PotionUp.performed += PotionUpPerformed;
		inputActions.Player.PotionDown.performed += PotionDownPerformed;
		inputActions.Player.PotionDown.canceled += PotionDownCanceled;
	}

	private void OnDisable()
	{
		inputActions.Player.WASD.performed -= MovementPerformed;
		inputActions.Player.WASD.canceled -= MovementCanceled;
		inputActions.Player.GamepadDirect.performed -= MovementPerformed;
		inputActions.Player.GamepadDirect.canceled -= MovementCanceled;
		inputActions.Player.Shoot.performed -= ShootPerformed;
		inputActions.Player.Shoot.canceled -= ShootCanceld;
		inputActions.Player.Drink.performed -= DrinkPerformed;
		inputActions.Player.Drink.canceled -= DrinkCanceld;
		inputActions.Player.Alpha1.performed -= Alpha1Performed;
		inputActions.Player.Alpha2.performed -= Alpha2Performed;
		inputActions.Player.Alpha3.performed -= Alpha3Performed;
		inputActions.Player.Alpha4.performed -= Alpha4Performed;
		inputActions.Player.Alpha5.performed -= Alpha5Performed;
		inputActions.Player.Alpha6.performed -= Alpha6Performed;
		inputActions.Player.Alpha7.performed -= Alpha7Performed;
		inputActions.Player.WandUp.performed -= WandUpPerformed;
		inputActions.Player.WandDown.performed -= WandDownPerformed;
		inputActions.Player.RightStick.performed -= RightStickPerformed;
		inputActions.Player.RightStick.canceled -= RightStickCancled;
		inputActions.Player.PotionUp.performed -= PotionUpPerformed;
		inputActions.Player.PotionDown.performed -= PotionDownPerformed;
		inputActions.Player.PotionDown.canceled -= PotionDownCanceled;
		InAbyss = false;
	}

	private void MovementPerformed(InputAction.CallbackContext context)
	{
		inputLeftStick = context.ReadValue<Vector2>();
		inputLeftStickLast = inputLeftStick;
		inputStickLast = inputLeftStickLast;
	}

	private void MovementCanceled_Dpad(InputAction.CallbackContext context)
	{
		inputLeftStick = Vector2.zero;
	}

	private void MovementPerformed_Dpad(InputAction.CallbackContext context)
	{
		if (!UIPlayerDataMgr.Inst.IsBagOpen)
		{
			inputLeftStick = context.ReadValue<Vector2>();
		}
	}

	private void MovementCanceled(InputAction.CallbackContext context)
	{
		inputLeftStick = Vector2.zero;
	}

	private void ShootPerformed(InputAction.CallbackContext context)
	{
		isHoldMouse0 = true;
	}

	private void Start()
	{
		changeWandCooling = 0f;
		dashOverHeatTimer = 0f;
		unmovableCounter = 0;
		nonInteractiveCounter = 0;
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (PlayerAroundCenterEtt == default(Entity))
		{
			PlayerAroundCenterEtt = ettMgr.CreateEntity(typeof(LocalTransform));
		}
		UpdateAroundCenterEttPosition();
		if (GameMgr.IsMobile_Static)
		{
			mobileCamControl.SetActive(value: true);
		}
		if (ScriptableObjMgr.Inst.testCtrller.Chapter5Experience)
		{
			PlayerMgr.Inst.ItemCtrller.relic_ShootNotSlowdown = true;
			myPpt.unitCfg.moveSpeed = 6.6f;
		}
		if (ScriptableObjMgr.Inst.testCtrller.isBW)
		{
			PlayerMgr.Inst.ItemCtrller.relic_ShootNotSlowdown = true;
			myPpt.unitCfg.moveSpeed *= 1.1f;
		}
		SAnima.AnimationState.Event += SAnimaEvent;
		handSlotL = SAnima.skeleton.FindSlot("Hand_L");
		handSlotR = SAnima.skeleton.FindSlot("Hand_R");
		if ((!DataMgr.selectedWorldData.inBattle9 || (DataMgr.selectedWorldData.inBattle9 && DataMgr.selectedWorldData.battleData9.currentStage == 1 && DataMgr.selectedWorldData.battleData9.currentLevel == 0) || ScriptableObjMgr.Inst.testCtrller.BattleChapter) && DataMgr.selectedWorldData.GetSelectedSetCfg().relicID != 0)
		{
			PlayerMgr.Inst.RelicAddFromSet();
		}
		nvliuIdleTime.RandomResult();
	}

	private void Update()
	{
		if (UIMgr.Inst.InputType == PlayerInputType.Keyboard && !GameMgr.IsMobile_Static)
		{
			mouseWorldPoint = CamController.Inst.cam_Main.ScreenToWorldPoint(Input.mousePosition);
			mouseWorldPoint = new Vector3(ShootWorldPointOrigin.x, ShootWorldPointOrigin.y, 0f);
		}
		UpdateMobileCurrentShootPoint();
		Debug.DrawLine(base.transform.position, shotWorldPointAfterLerp, Color.yellow);
		ShootWorldPointCurrentFrameAfterLerp = shotWorldPointAfterLerp;
		ShootWorldPointCurrentFrame = ShootWorldPoint;
		CurrentDir = Tool2D.IgnoreZV2ToV1Normal(ShootWorldPointCurrentFrame, base.transform.position);
		if (CurrentDir == Vector3.zero)
		{
			if (CurrentMotion != Vector3.zero)
			{
				CurrentDir = Tool2D.IgnoreZPoint(CurrentMotion).normalized;
			}
			else
			{
				CurrentDir = Vector3.right;
			}
		}
		Shoot();
		ShootSlowdownHandle();
		DrinkPotion();
		Move();
		Rotate();
		EmojiBlink();
		TextFloatQueue();
		FollowObjHandle();
		NvliuEatSugarcane();
		TakeDamageAmaze();
		UpdateBodyColliderSize();
		UpdateAroundCenterEttPosition();
	}

	private void UpdateMobileCurrentShootPoint()
	{
		shotWorldPointWithOutLerp = ShotWorldPointTarget;
		shotWorldPointAfterLerp = shotWorldPointWithOutLerp;
		if ((!GameMgr.IsMobile_Static && UIMgr.Inst.InputType == PlayerInputType.Keyboard) || ((isHoldMouse0 || !GameMgr.IsMobile_Static || MobileMgr.inst.gamepadPlugged) && inputRightStick != Vector3.zero) || currentAimEntity == Entity.Null || TopUI.inst.uI_AimSkill.useSkillDir)
		{
			return;
		}
		Vector3 vector = ShootWorldPointCurrentFrameAfterLerp - base.transform.position;
		Vector3 vector2 = shotWorldPointWithOutLerp - base.transform.position;
		float magnitude = vector.magnitude;
		float num = Mathf.Atan2(vector.y, vector.x);
		float magnitude2 = vector2.magnitude;
		float num2 = Mathf.Atan2(vector2.y, vector2.x);
		float f = Mathf.DeltaAngle(num * 57.29578f, num2 * 57.29578f) * (MathF.PI / 180f);
		float num3 = Mathf.Abs(f) / mobileShotPointRotateAngle;
		if (num3 < Mathf.Epsilon)
		{
			shotWorldPointAfterLerp = shotWorldPointWithOutLerp;
			return;
		}
		float num4 = Mathf.Sign(f) * mobileShotPointRotateAngle * Time.deltaTime;
		if (Mathf.Abs(num4) >= Mathf.Abs(f))
		{
			num = num2;
			magnitude = magnitude2;
		}
		else
		{
			num += num4;
			float num5 = (magnitude2 - magnitude) / num3;
			magnitude += num5 * Time.deltaTime;
		}
		Vector3 vector3 = new Vector3(magnitude * Mathf.Cos(num), magnitude * Mathf.Sin(num), 0f);
		shotWorldPointAfterLerp = base.transform.position + vector3;
	}

	private unsafe void UpdateBodyColliderSize()
	{
		if (!(Mathf.Abs(currentPlayerSize - base.transform.localScale.x) < 0.1f))
		{
			currentPlayerSize = base.transform.localScale.x;
			((UnityEngine.CapsuleCollider)playerCollider).radius = Mathf.Min(PlayerColliderDefaultRadius, PlayerColliderDefaultRadius / (base.transform.localScale.x + 0.001f));
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(PlayerMgr.Inst.PlayerEtt);
			componentData.Scale = currentPlayerSize;
			ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
			PhysicsCollider componentData2 = ettMgr.GetComponentData<PhysicsCollider>(PlayerMgr.Inst.PlayerEtt);
			CompoundCollider* colliderPtr = (CompoundCollider*)componentData2.ColliderPtr;
			Unity.Physics.Collider* collider = colliderPtr->Children[0].Collider;
			if (collider->GetCollisionResponse() == CollisionResponsePolicy.Collide)
			{
				Unity.Physics.CapsuleCollider* ptr = (Unity.Physics.CapsuleCollider*)collider;
				CapsuleGeometry geometry = ptr->Geometry;
				geometry.Radius = ((UnityEngine.CapsuleCollider)playerCollider).radius;
				ptr->Geometry = geometry;
			}
			collider = colliderPtr->Children[1].Collider;
			if (collider->GetCollisionResponse() == CollisionResponsePolicy.Collide)
			{
				Unity.Physics.CapsuleCollider* ptr2 = (Unity.Physics.CapsuleCollider*)collider;
				CapsuleGeometry geometry2 = ptr2->Geometry;
				geometry2.Radius = ((UnityEngine.CapsuleCollider)playerCollider).radius;
				ptr2->Geometry = geometry2;
			}
			colliderPtr->RefreshCollisionFilter();
			ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData2);
		}
	}

	public void UpdateAroundCenterEttPosition()
	{
		if (PlayerAroundCenterEtt == default(Entity) || !ettMgr.HasComponent<LocalTransform>(PlayerMgr.Inst.PlayerEtt))
		{
			return;
		}
		float3 position = ettMgr.GetComponentData<LocalToWorld>(PlayerMgr.Inst.PlayerEtt).Position;
		PlayerMgr inst = PlayerMgr.Inst;
		bool flag = (object)inst != null && (object)inst.SelectedWand != null;
		bool flag2;
		bool flag3;
		if (flag)
		{
			PlayerItemController itemCtrller = PlayerMgr.Inst.ItemCtrller;
			flag2 = (object)itemCtrller != null && (object)itemCtrller.relic_RemoteShoot != null;
			if (!flag2)
			{
				Wand selectedWand = PlayerMgr.Inst.SelectedWand;
				if ((object)selectedWand != null)
				{
					WandConfig wandCfg = selectedWand.WandCfg;
					if (wandCfg != null)
					{
						WandAbility specialAbility = wandCfg.specialAbility;
						if ((uint)(specialAbility - 25) <= 1u)
						{
							flag3 = true;
							goto IL_00c2;
						}
					}
				}
				flag3 = false;
				goto IL_00c2;
			}
			goto IL_00c6;
		}
		goto IL_00c9;
		IL_00c6:
		flag = flag2;
		goto IL_00c9;
		IL_00c9:
		if (flag)
		{
			position = PlayerMgr.Inst.SelectedWand.ShootPosition;
		}
		ettMgr.SetComponentData(PlayerAroundCenterEtt, LocalTransform.FromPosition(position));
		return;
		IL_00c2:
		flag2 = flag3;
		goto IL_00c6;
	}

	private void Shoot()
	{
		changeWandCooling -= PlayerMgr.Inst.PlayerDeltaTime;
		dashOverHeatTimer -= PlayerMgr.Inst.PlayerDeltaTime;
		if (PlayerMgr.Inst.inDashSpell || isFrozen || !isHoldMouse0 || usingPotion || !CanMotion || UIPlayerDataMgr.Inst.IsDraging || IsKeepCasting || changeWandCooling > 0f || (PlayerMgr.Inst.ItemCtrller.curse_InjuredCantShoot != null && !PlayerMgr.Inst.ItemCtrller.curse_InjuredCantShoot.CanShoot) || (PlayerMgr.Inst.ItemCtrller.curse_CantShootEnterRoom != null && !PlayerMgr.Inst.ItemCtrller.curse_CantShootEnterRoom.CanShoot))
		{
			return;
		}
		if (!GameMgr.IsMobile_Static && EventSystem.current.IsPointerOverGameObject())
		{
			eventData.position = Input.mousePosition;
			EventSystem.current.RaycastAll(eventData, rayacstResults);
			for (int i = 0; i < rayacstResults.Count; i++)
			{
				UIPlayerSlider component = rayacstResults[i].gameObject.GetComponent<UIPlayerSlider>();
				UICurse component2 = rayacstResults[i].gameObject.GetComponent<UICurse>();
				if (component == null && component2 == null)
				{
					if (PlayerMgr.Inst.SelectedWand != null && PlayerMgr.Inst.SelectedWand.passiveChargeEnable)
					{
						SetShootSlowdown();
					}
					return;
				}
			}
		}
		if (PlayerMgr.Inst.SelectedWand == null)
		{
			return;
		}
		if (PlayerMgr.Inst.SelectedWand.passiveAutoWand)
		{
			if (Time.unscaledTime - _lastShowAutoWandCannotCastTime > 0.5f)
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002040.GetText(), UITextFloatType.Normal, PlayerMgr.Inst.PlayerPoint + new Vector3(0f, 0.8f, 0f));
				_lastShowAutoWandCannotCastTime = Time.unscaledTime;
			}
			return;
		}
		bool flag = false;
		flag = ((!PlayerMgr.Inst.SelectedWand.passiveChargeEnable) ? PlayerMgr.Inst.SelectedWand.TryShoot() : WandChargeEffect(chargeStart: true));
		if (flag || PlayerMgr.Inst.SelectedWand.passiveChargeEnable)
		{
			SetShootSlowdown();
			SetVisiable();
			NvliuQuitEating();
		}
	}

	private void ShootSlowdownHandle()
	{
		if (PlayerMgr.Inst.ItemCtrller.relic_ShootNotSlowdown)
		{
			if (shootSlowdownRate != 1f)
			{
				shootSlowdownRate = 1f;
				shootSlowdownRateLastRecord = shootSlowdownRate;
				SAnima.timeScale = shootSlowdownRateLastRecord;
			}
			return;
		}
		if (!CanMotion)
		{
			SetShootSlowdown();
		}
		bool flag = false;
		foreach (UnitKeepCastingSpells keepCastingEntity in keepCastingEntities)
		{
			if (keepCastingEntity.ReduceMoveSpeed)
			{
				flag = true;
				break;
			}
		}
		if (shootSlowdownTimer > 0f || flag)
		{
			shootSlowdownTimer -= PlayerMgr.Inst.PlayerDeltaTime;
			float b = 0.6f;
			if (PlayerMgr.Inst.ItemCtrller.curseCfg_ShootSlow != null)
			{
				b = 1f - (float)PlayerMgr.Inst.ItemCtrller.curseCfg_ShootSlow.int1.result / 100f;
			}
			shootSlowdownRate = Mathf.Lerp(shootSlowdownRate, b, 25f * PlayerMgr.Inst.PlayerDeltaTime);
		}
		else
		{
			shootSlowdownRate = Mathf.Lerp(shootSlowdownRate, 1f, 25f * PlayerMgr.Inst.PlayerDeltaTime);
		}
		if (shootSlowdownRateLastRecord != shootSlowdownRate)
		{
			shootSlowdownRateLastRecord = shootSlowdownRate;
			SAnima.timeScale = shootSlowdownRateLastRecord;
		}
	}

	public float GetTriggerFinalRadius()
	{
		return ((UnityEngine.SphereCollider)playerTrigger).radius * base.transform.localScale.x;
	}

	private void DrinkPotion()
	{
		if (GameMgr.IsMobile_Static)
		{
			IsHoldM1Pad();
		}
		else if (ControlMgr.Inst.usingpad)
		{
			IsHoldM1Pad();
		}
		else
		{
			IsHoldM1Key();
		}
		void IsHoldM1Key()
		{
			if (isHoldMouse1)
			{
				if (usingPotion)
				{
					SetShootSlowdown();
					usePotionTimer += PlayerMgr.Inst.PlayerDeltaTime;
					uiPotionUse.SetOutline(usePotionTimer / 1f);
					if (usePotionTimer >= 1f)
					{
						usePotionTimer = 0f;
						usingPotion = false;
						showdrinking = false;
						uiPotionUse.UseSuccess();
						PlayerMgr.Inst.ItemCtrller.PotionUse();
					}
				}
				else
				{
					usePotionTimer = 0f;
					usingPotion = false;
				}
			}
		}
		void IsHoldM1Pad()
		{
			if (isHoldMouse1)
			{
				if (usingPotion && !TopUI.inst.mobilePotionDragTutorial.activeInHierarchy)
				{
					usePotionTimer += PlayerMgr.Inst.PlayerDeltaTime;
					if (!(usePotionTimer < 0.2f))
					{
						SetShootSlowdown();
						if (!showdrinking)
						{
							showdrinking = true;
							uiPotionUse.Show(PlayerMgr.Inst.ItemCtrller.SelectedPotionID);
						}
						else
						{
							uiPotionUse.SetOutline(usePotionTimer / 1f);
							if (usePotionTimer >= 1f)
							{
								usePotionTimer = 0f;
								usingPotion = false;
								showdrinking = false;
								uiPotionUse.UseSuccess();
								PlayerMgr.Inst.ItemCtrller.PotionUse();
							}
						}
					}
				}
			}
			else if (usingPotion)
			{
				usingPotion = false;
				if (usePotionTimer < 0.2f)
				{
					if (uiPotionUse.showing)
					{
						uiPotionUse.Hide();
						Debug.Log("如果是键盘模式突然手柄切药");
					}
					PlayerMgr.Inst.ItemCtrller.PotionChangeSelect(nextOne: true);
					if (PlayerMgr.Inst.ItemCtrller.SelectedPotionID > 0)
					{
						UIPotionsController.Inst?.ShowInfoPanel(PlayerMgr.Inst.ItemCtrller.SelectedPotionID);
					}
				}
				else
				{
					showdrinking = false;
					uiPotionUse.Hide();
				}
				usePotionTimer = 0f;
				usingPotion = false;
			}
		}
	}

	private void Move()
	{
		isStandInLastFrame = true;
		PhysicsVelocity componentData = ettMgr.GetComponentData<PhysicsVelocity>(PlayerMgr.Inst.PlayerEtt);
		if (!CanMotion)
		{
			componentData.Linear = float3.zero;
			ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
			if (myPpt.Affect_InAbyss)
			{
				CurrentMoveDir = Vector3.zero;
			}
		}
		else
		{
			if (isFrozen || !PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
			{
				return;
			}
			Vector2 vector = ((!GameMgr.IsMobile_Static) ? ((ControlMgr.Inst.InputType == PlayerInputType.Keyboard) ? inputLeftStick.normalized : ((!(inputLeftStick.magnitude > 0.2f)) ? Vector2.zero : inputLeftStick)) : (DataMgr.settingData.Mobiledata.mobileStickMoveLerp ? Vector2.ClampMagnitude(inputLeftStick * 1.5f, 1f) : inputLeftStick.normalized));
			if (vector != Vector2.zero)
			{
				isStandInLastFrame = false;
			}
			bool flag = PlayerMgr.Inst.ItemCtrller.curseCfg_ZeroFriction != null;
			if (vector != Vector2.zero || !flag)
			{
				CurrentMoveDir = vector;
			}
			bool flag2 = vector == Vector2.zero && flag;
			Vector3 vector2 = CurrentMoveDir * Time.timeScale * (myPpt.MoveSpeed * shootSlowdownRate * (flag2 ? PlayerMgr.Inst.ItemCtrller.curseCfg_ZeroFriction.float1.result : 1f));
			if (GameMgr.IsMobile_Static)
			{
				CurrentMotion = vector2;
			}
			else
			{
				CurrentMotion = Vector3.Lerp(CurrentMotion, vector2, motionLerp * PlayerMgr.Inst.PlayerDeltaTime);
			}
			Vector3 currentMotion = CurrentMotion;
			if (PlayerMgr.Inst.ItemCtrller.uiRelic_LightArmor != null)
			{
				currentMotion += PlayerMgr.Inst.ItemCtrller.uiRelic_LightArmor.SprintForce;
			}
			if (Time.timeScale != 0f)
			{
				currentMotion /= Time.timeScale;
			}
			componentData.Linear = (float3)currentMotion + playerPpt.currentKnockback;
			ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
			float timeScale = 1f;
			if (CurrentMoveDir == Vector3.zero || flag2)
			{
				if (ShootWorldPoint.y > base.transform.position.y)
				{
					if (NeedShowFlyAnima(playerPpt))
					{
						SetBodyAnima(PlayerBodyAnima.FlyIdleUp);
						return;
					}
					Relic_AddMoveSpeed relic_AddMoveSpeed = PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed;
					if ((object)relic_AddMoveSpeed != null && !relic_AddMoveSpeed.HideBySettings && !DataMgr.settingData.SafeMode)
					{
						SetBodyAnima(PlayerBodyAnima.GroundIdleUp);
					}
					else
					{
						SetBodyAnima(PlayerBodyAnima.GroundIdleUp);
					}
				}
				else if (NeedShowFlyAnima(playerPpt))
				{
					SetBodyAnima(PlayerBodyAnima.FlyIdleDown);
				}
				else
				{
					Relic_AddMoveSpeed relic_AddMoveSpeed = PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed;
					if ((object)relic_AddMoveSpeed != null && !relic_AddMoveSpeed.HideBySettings)
					{
						SetBodyAnima(PlayerBodyAnima.GroundIdleDown);
					}
					else
					{
						SetBodyAnima(PlayerBodyAnima.GroundIdleDown);
					}
				}
				return;
			}
			if (GameMgr.IsMobile_Static || ControlMgr.Inst.InputType == PlayerInputType.Gamepad)
			{
				if (DataMgr.settingData.Mobiledata.mobileStickMoveLerp)
				{
					timeScale = inputLeftStick.magnitude;
					timeScale = Mathf.Clamp(timeScale, 0.2f, 1f);
				}
				else
				{
					timeScale = 1f;
				}
			}
			if (ShootWorldPoint.y > base.transform.position.y)
			{
				if (NeedShowFlyAnima(playerPpt))
				{
					SetBodyAnima(PlayerBodyAnima.FlyWalkUp, timeScale);
					return;
				}
				Relic_AddMoveSpeed relic_AddMoveSpeed = PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed;
				if ((object)relic_AddMoveSpeed != null && !relic_AddMoveSpeed.HideBySettings && DataMgr.settingData.SafeMode)
				{
					SetBodyAnima(PlayerBodyAnima.GroundIdleUp, timeScale);
				}
				else
				{
					SetBodyAnima(PlayerBodyAnima.GroundWalkUp, timeScale);
				}
			}
			else if (NeedShowFlyAnima(playerPpt))
			{
				SetBodyAnima(PlayerBodyAnima.FlyWalkDown, timeScale);
			}
			else
			{
				Relic_AddMoveSpeed relic_AddMoveSpeed = PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed;
				if ((object)relic_AddMoveSpeed != null && !relic_AddMoveSpeed.HideBySettings && DataMgr.settingData.SafeMode)
				{
					SetBodyAnima(PlayerBodyAnima.GroundIdleDown, timeScale);
				}
				else
				{
					SetBodyAnima(PlayerBodyAnima.GroundWalkDown, timeScale);
				}
			}
		}
	}

	public static bool NeedShowFlyAnima(UnitProperty_Dots _playerPpt)
	{
		if (DataMgr.selectedWorldData.IsDave && DataMgr.selectedWorldData.InDaveRoom)
		{
			return true;
		}
		if (!_playerPpt.IsFly)
		{
			Relic_AddMoveSpeed relic_AddMoveSpeed = PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed;
			if ((object)relic_AddMoveSpeed != null && !relic_AddMoveSpeed.HideBySettings)
			{
				return !DataMgr.settingData.SafeMode;
			}
			return false;
		}
		return true;
	}

	private void Rotate()
	{
		if (isNvliuEating)
		{
			return;
		}
		Quaternion localRotation = Quaternion.Euler(0f, 0f, Tool2D.GetDegree(CurrentDir));
		if (GameMgr.IsMobile_Static)
		{
			AimLine.transform.localRotation = localRotation;
		}
		if (!CanMotion || isFrozen)
		{
			return;
		}
		if (!(PlayerMgr.Inst.ItemCtrller.relic_MirrorOfSoul != null) && PlayerMgr.Inst.ItemCtrller.relic_Reaper == null && PlayerMgr.Inst.ItemCtrller.relic_Huang == null)
		{
			if (DaveHoldingHarpoonsGunTrans.gameObject.activeSelf)
			{
				if (handSlotL.A == 1f)
				{
					handSlotL.A = 0f;
				}
				if (handSlotR.A == 1f)
				{
					handSlotR.A = 0f;
				}
			}
			else if (PlayerMgr.Inst.SelectedWandIndex == -1 || ((bool)PlayerMgr.Inst.SelectedWand && PlayerMgr.Inst.SelectedWand.passiveAutoWand))
			{
				if (handSlotL.A == 0f)
				{
					handSlotL.A = 1f;
				}
				if (handSlotR.A == 0f)
				{
					handSlotR.A = 1f;
				}
			}
			else if (PlayerMgr.Inst.SelectedWandIndex != -1)
			{
				if (ShootWorldPointCurrentFrame.y > base.transform.position.y)
				{
					if (handSlotL.A != 1f)
					{
						handSlotL.A = 1f;
					}
					if (handSlotR.A != 0f)
					{
						handSlotR.A = 0f;
					}
				}
				else
				{
					if (handSlotL.A != 0f)
					{
						handSlotL.A = 0f;
					}
					if (handSlotR.A != 1f)
					{
						handSlotR.A = 1f;
					}
				}
			}
		}
		SetFlip(ShootWorldPointCurrentFrame.x < base.transform.position.x);
		tsf_WandRoot.localRotation = localRotation;
	}

	private void Knockback()
	{
		if (knockback != Vector3.zero)
		{
			knockback = Vector3.Lerp(knockback, Vector3.zero, 5f * Time.deltaTime);
		}
	}

	private void EmojiBlink()
	{
		if (PlayerMgr.Inst.ItemCtrller.potion_Petrifaction != null || isNvliuEating || (emojiType != PlayerEmojiType.Normal && emojiType != PlayerEmojiType.BlackEye))
		{
			return;
		}
		emojiBlinkIntervalTimer += PlayerMgr.Inst.PlayerDeltaTime;
		if (emojiBlinkIntervalTimer >= emojiBlinkInterval.result)
		{
			emojiBlinkIntervalTimer = 0f;
			emojiBlinkInterval.RandomResult();
			if (emojiType == PlayerEmojiType.Normal)
			{
				SAnima.AnimationState.SetAnimation(1, "Emoji/NormalBlink", loop: false);
			}
			else
			{
				SAnima.AnimationState.SetAnimation(1, "Emoji/BlackEyeBlink", loop: false);
			}
		}
	}

	private void TextFloatQueue()
	{
		textFloatTimer += Time.unscaledDeltaTime;
		if (queueTypes.Count > 0 && textFloatTimer > textFloatInterval)
		{
			textFloatTimer = 0f;
			ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(queueTypes[0].content, queueTypes[0].type, base.transform.position);
			queueTypes.RemoveAt(0);
		}
	}

	private void FollowObjHandle()
	{
		for (int i = 0; i < followObjs.Count; i++)
		{
			if (i == 0)
			{
				if (Tool2D.IgnoreZDistanceSqr(followObjs[i].transform.position, base.transform.position) > followObjDistance * followObjDistance)
				{
					followObjs[i].transform.position = Vector3.Lerp(followObjs[i].transform.position, base.transform.position, followObjLerp * Time.deltaTime);
				}
			}
			else if (Tool2D.IgnoreZDistanceSqr(followObjs[i].transform.position, followObjs[i - 1].transform.position) > followObjDistance * followObjDistance)
			{
				followObjs[i].transform.position = Vector3.Lerp(followObjs[i].transform.position, followObjs[i - 1].transform.position, followObjLerp * Time.deltaTime);
			}
		}
	}

	private void NvliuEatSugarcane()
	{
		if (DataMgr.selectedWorldData.playerLook != PlayerLook.Nvliu || DataMgr.selectedWorldData.selectedSetID == 7 || DataMgr.selectedWorldData.selectedSetID == 8 || DataMgr.selectedWorldData.selectedSetID == 9 || DataMgr.selectedWorldData.IsDave || PlayerMgr.Inst.IsHide || PlayerMgr.Inst.ItemCtrller.potion_Petrifaction != null)
		{
			return;
		}
		if (CurrentMotion == Vector3.zero)
		{
			if (isNvliuEating)
			{
				nvliuEatingTimer += Time.deltaTime;
				if (nvliuEatingTimer >= nvliuEatingTime)
				{
					nvliuEatingTimer = 0f;
					if (DataMgr.selectedWorldData.selectedSetID != 5 && PlayerMgr.Inst.SelectedWand != null)
					{
						PlayerMgr.Inst.SelectedWand.Display_Show();
					}
					SetToNormalAnime();
				}
				return;
			}
			nvliuIdleTimer += Time.deltaTime;
			if (nvliuIdleTimer >= nvliuIdleTime.result)
			{
				nvliuIdleTimer = 0f;
				nvliuIdleTime.RandomResult();
				if (myPpt.IsFly)
				{
					SetBodyAnima(PlayerBodyAnima.FlyIdleDown);
				}
				else
				{
					SetBodyAnima(PlayerBodyAnima.GroundIdleDown);
				}
				if (handSlotL.A == 0f)
				{
					handSlotL.A = 1f;
				}
				if (handSlotR.A == 0f)
				{
					handSlotR.A = 1f;
				}
				if (PlayerMgr.Inst.SelectedWand != null)
				{
					PlayerMgr.Inst.SelectedWand.Display_Hide();
				}
				isNvliuEating = true;
				SAnima.AnimationState.SetAnimation(1, "Emoji/IdleAction", loop: true);
			}
		}
		else
		{
			NvliuQuitEating();
		}
	}

	public void SetToNormalAnime()
	{
		if (isNvliuEating)
		{
			isNvliuEating = false;
			nvliuIdleTimer = 0f;
			nvliuEatingTimer = 0f;
			if (DataMgr.selectedWorldData.selectedSetID != 5)
			{
				PlayerMgr.Inst.SelectedWand.Display_Show();
			}
		}
		SAnima.AnimationState.SetAnimation(1, "Emoji/Normal", loop: false);
	}

	private void NvliuQuitEating()
	{
		if (isNvliuEating)
		{
			SetToNormalAnime();
		}
		nvliuIdleTimer = 0f;
		nvliuEatingTimer = 0f;
	}

	private void TakeDamageAmaze()
	{
		if (CanMotion && emojiType == PlayerEmojiType.Amaze)
		{
			takeDamageAmazeTimer += Time.deltaTime;
			if (takeDamageAmazeTimer >= takeDamageAmazeTime)
			{
				takeDamageAmazeTimer = 0f;
				SetEmoji(PlayerEmojiType.Normal);
			}
		}
	}

	private void LateUpdate()
	{
		CurrentDir = Tool2D.IgnoreZV2ToV1Normal(ShootWorldPointCurrentFrame, base.transform.position);
		Rotate();
		CheckMoveDistance();
		RTFollow();
	}

	private void CheckMoveDistance()
	{
		if (lastFramePos == Vector3.zero)
		{
			lastFramePos = base.transform.position;
			footstepSEPlayCounter = 0f;
			return;
		}
		distanceMoveInLastFrame = Vector3.Distance(lastFramePos, base.transform.position);
		if (distanceMoveInLastFrame > 5f)
		{
			distanceMoveInLastFrame = 5f;
			footstepSEPlayCounter -= distanceMoveInLastFrame;
		}
		footstepSEPlayCounter += distanceMoveInLastFrame;
		lastFramePos = base.transform.position;
		if (!(footstepSEPlayCounter >= footstepSEPlayDistance))
		{
			return;
		}
		footstepSEPlayCounter -= footstepSEPlayDistance;
		if (!PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
		{
			return;
		}
		if (!NeedShowFlyAnima(playerPpt))
		{
			AudioSource audioSource = SEMgr.Inst.playerFootStep.PlaySE();
			if ((bool)audioSource)
			{
				audioSource.pitch = footStepPitch.RandomResult();
			}
		}
		else if (DataMgr.selectedWorldData.IsDave && !PlayerMgr.Inst.inDashSpell)
		{
			AudioSource audioSource2 = SEMgr.Inst.playerFootStepDaveSwim.PlaySE(SEPlayMode.Unique, 2);
			if ((bool)audioSource2)
			{
				audioSource2.pitch = footStepPitch.RandomResult();
			}
		}
	}

	public void RTFollow()
	{
		layerC_PlayerRTRoot.transform.position = base.transform.position;
	}

	public bool WandChargeEffect(bool chargeStart)
	{
		Wand selectedWand = PlayerMgr.Inst.SelectedWand;
		if (!selectedWand)
		{
			Debug.LogError("没有手持法杖怎么蓄力？");
			return false;
		}
		if (chargeStart)
		{
			if (!selectedWand.IsCharging)
			{
				selectedWand.StartCharge();
			}
		}
		else if (selectedWand.IsCharging)
		{
			selectedWand.ReleaseCharge();
			shootSlowdownTimer = 0f;
		}
		return false;
	}

	private void SAnimaEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string @string = e.String;
		if (!(@string == "FootStep") && !(@string == "PickWandFinish"))
		{
			Debug.LogError(e.String);
		}
	}

	public void UnmotionableRegister()
	{
		if (unmovableCounter == 0)
		{
			inputLeftStick = Vector2.zero;
		}
		unmovableCounter++;
	}

	public void UnmotionableUnregister()
	{
		unmovableCounter--;
		if (unmovableCounter < 0)
		{
			unmovableCounter = 0;
			Debug.LogError("为什么unmovableCounter会小于0，如果手机的话，虚拟摇杆显示可能出问题，出了问题再改");
		}
	}

	public void NonInteractiveRegister()
	{
		nonInteractiveCounter++;
	}

	public void NonInteractiveUnregister()
	{
		nonInteractiveCounter--;
		if (nonInteractiveCounter < 0)
		{
			nonInteractiveCounter = 0;
			Debug.LogError("为什么可互动计数器会小于0");
		}
	}

	public void NonInteractiveClear()
	{
		nonInteractiveCounter = 0;
	}

	public int GetInteractCount()
	{
		return UIInteractiveObjMgr.Inst.InteractableCount;
	}

	public void SetInvisiable()
	{
		if (!IsVisible)
		{
			return;
		}
		IsVisible = false;
		UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
		componentData.CanBeTarget = false;
		ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Invisible", base.transform.position, 3f);
		invisibleSRs = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		invisibleMRs = GetComponentsInChildren<MeshRenderer>(includeInactive: true);
		invisibleLRs = GetComponentsInChildren<LineRenderer>(includeInactive: true);
		mr.material = mat_Invisible;
		mr.material.SetTexture("_MainTex", PlayerMgr.Inst.PlayerCtrller.mr.material.mainTexture);
		for (int i = 0; i < invisibleSRs.Length; i++)
		{
			invisibleSRs[i].material.SetFloat("_Alpha", 0.35f);
		}
		for (int j = 0; j < invisibleMRs.Length; j++)
		{
			if (invisibleMRs[j].gameObject.name == "OutLine")
			{
				invisibleMRs[j].gameObject.SetActive(value: true);
			}
			else
			{
				invisibleMRs[j].material.SetFloat("_Alpha", 0.35f);
			}
		}
		for (int k = 0; k < invisibleLRs.Length; k++)
		{
			invisibleLRs[k].startColor = new Color(invisibleLRs[k].startColor.r, invisibleLRs[k].startColor.g, invisibleLRs[k].startColor.b, 0.35f);
			invisibleLRs[k].endColor = new Color(invisibleLRs[k].endColor.r, invisibleLRs[k].endColor.g, invisibleLRs[k].endColor.b, 0.35f);
		}
		for (int l = 0; l < LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count; l++)
		{
			Entity entity = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList[l];
			if (UnitDotsSyncSystem.TryGetComponent<UnitBase_Dots>(entity, out var result))
			{
				result.targetEtt = Entity.Null;
				ettMgr.SetComponentData(entity, result);
			}
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(entity, out var result2) && result2.unitCfg.isHybirdUnit)
			{
				ettMgr.GetComponentObject<UnitPptReference>(entity).unitPpt.UnitBas.LosePlayerTarget();
			}
		}
	}

	public void SetVisiable()
	{
		if (PlayerMgr.Inst.ItemCtrller.potion_Invisible != null)
		{
			return;
		}
		mr.material = mat_Normal;
		mr.material.SetTexture("_MainTex", PlayerMgr.Inst.PlayerCtrller.mr.material.mainTexture);
		UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
		componentData.CanBeTarget = true;
		ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
		if (!IsVisible)
		{
			IsVisible = true;
			for (int i = 0; i < invisibleSRs.Length; i++)
			{
				invisibleSRs[i].material.SetFloat("_Alpha", 1f);
			}
			for (int j = 0; j < invisibleMRs.Length; j++)
			{
				if (invisibleMRs[j].gameObject.name == "OutLine")
				{
					invisibleMRs[j].gameObject.SetActive(value: false);
				}
				else
				{
					invisibleMRs[j].material.SetFloat("_Alpha", 1f);
				}
			}
			for (int k = 0; k < invisibleLRs.Length; k++)
			{
				invisibleLRs[k].startColor = new Color(invisibleLRs[k].startColor.r, invisibleLRs[k].startColor.g, invisibleLRs[k].startColor.b, 1f);
				invisibleLRs[k].endColor = new Color(invisibleLRs[k].endColor.r, invisibleLRs[k].endColor.g, invisibleLRs[k].endColor.b, 1f);
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_NoAttackStealth != null && IsVisible)
		{
			PlayerMgr.Inst.ItemCtrller.relicCfg_NoAttackStealth.floatTimer = 0f;
		}
	}

	public void CastLockRegister()
	{
		castSpellLock = true;
	}

	public void CastLockUnRegister()
	{
		castSpellLock = false;
	}

	public void SetFrozen()
	{
		frozenBeforeRigid = rigid.linearVelocity;
		frozenBeforeKinematic = rigid.isKinematic;
		frozenBeforeMotion = CurrentMotion;
		rigid.isKinematic = true;
		CurrentMotion = Vector3.zero;
		rigid.linearVelocity = Vector3.zero;
		if (SAnima != null)
		{
			SAnima.AnimationState.TimeScale = 0f;
		}
		SyncDotsKinematic();
	}

	public void SetUnfrozen()
	{
		rigid.isKinematic = frozenBeforeKinematic;
		rigid.linearVelocity = frozenBeforeRigid;
		CurrentMotion = frozenBeforeMotion;
		if (SAnima != null)
		{
			SAnima.AnimationState.TimeScale = 1f;
		}
		SyncDotsKinematic();
	}

	private void SyncDotsKinematic()
	{
		PhysicsMassOverride componentData = ettMgr.GetComponentData<PhysicsMassOverride>(PlayerMgr.Inst.PlayerEtt);
		if (myPpt.Rigid.isKinematic)
		{
			componentData.IsKinematic = 1;
			componentData.SetVelocityToZero = 1;
		}
		else
		{
			componentData.IsKinematic = 0;
			componentData.SetVelocityToZero = 0;
		}
		ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
	}

	public void ShowAndUpdateDaveHarpoonsGun()
	{
		if (leftHand == null || rightHand == null)
		{
			leftHand = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("Hand_L");
			rightHand = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("Hand_R");
		}
		if (!DaveHoldingHarpoonsGunTrans.gameObject.activeInHierarchy)
		{
			DaveHoldingHarpoonsGunTrans.gameObject.SetActive(value: true);
		}
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			wand.Display_Hide();
		}
		UpdateDaveHarpoonsGun();
		leftHand.A = 0f;
		rightHand.A = 0f;
	}

	public void UpdateDaveHarpoonsGun()
	{
		DaveHoldingHarpoonsGunTrans.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.right, ShootWorldPoint - base.transform.position));
		float z = ((ShootWorldPoint.y > base.transform.position.y) ? 0.3f : (-0.3f));
		DaveHoldingHarpoonsGunTrans.localPosition = DaveHoldingHarpoonsGunTrans.localPosition.IgnoreZ() + new Vector3(0f, 0f, z) * 0.01f;
		int num = ((PlayerMgr.Inst.GetMousePoint().x <= PlayerMgr.Inst.PlayerPoint.x) ? 1 : 0);
		DaveHoldingHarpoonGunSprite.material.SetFloat(IsFlip, num);
		DaveHoldingHarpoonsGunShadowSprite.material.SetFloat(IsFlip, num);
		float num2 = Mathf.Abs(PlayerMgr.Inst.PlayerPpt.transform.position.z) - PlayerMgr.Inst.PlayerCtrller.tsf_WandRoot.localPosition.z + 0.3f;
		DaveHoldingHarpoonsGunShadowSprite.transform.position = DaveHoldingHarpoonGunSprite.transform.position + new Vector3(0f, 0f - num2, 900f);
	}

	public void HideDaveHarpoonsGun()
	{
		if (leftHand == null || rightHand == null)
		{
			leftHand = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("Hand_L");
			rightHand = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("Hand_R");
		}
		DaveHoldingHarpoonsGunTrans.gameObject.SetActive(value: false);
		if (PlayerMgr.Inst.SelectedWandCfg != null && !PlayerMgr.Inst.SelectedWand.tsf_Layer.gameObject.activeInHierarchy && !PlayerMgr.Inst.SelectedWand.passiveAutoWand)
		{
			PlayerMgr.Inst.SelectedWand.Display_Show();
		}
		leftHand.A = 1f;
		rightHand.A = 1f;
	}

	public void ClearDashOverheat()
	{
		dashOverHeatTimer = 0f;
	}

	public void AfterTakeDamage(ref TakeDamageInfo_Dots info)
	{
		SetEmoji(PlayerEmojiType.Amaze);
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_InjuredAttackAll != null)
		{
			ObjPoolMgr.Inst.GetUIGO("Prefabs/Item/Relic_InjuredAttackAll_UI", 0.5f).transform.localPosition = Vector3.zero;
			for (int i = 0; i < LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count; i++)
			{
				Entity entity = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList[i];
				UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(entity);
				if (componentData.unitCfg.id != 104221 && componentData.unitCfg.id != 104222 && componentData.unitCfg.id != 105321 && componentData.unitCfg.id != 105322 && componentData.unitCfg.id != 300921 && componentData.unitCfg.id != 500622 && componentData.unitCfg.id != 501021 && componentData.unitCfg.id != 505021 && componentData.CanBeTarget)
				{
					LocalTransform componentData2 = ettMgr.GetComponentData<LocalTransform>(entity);
					ObjPoolMgr.Inst.GetGO("Prefabs/Item/Relic_InjuredAttackAll", componentData2.Position, 2f);
					SpriteRenderer componentInChildren = ObjPoolMgr.Inst.GetGO("Prefabs/Item/Relic_InjuredAttackAll_Trace", Tool2D.GetLayerPoint(componentData2.Position, LayerCorrectType.GroundEffectLow), 15f).GetComponentInChildren<SpriteRenderer>();
					Color color = componentInChildren.color;
					color.a = 0.3f;
					componentInChildren.color = color;
					componentInChildren.DOFade(0f, 5f).SetDelay(10f);
					float num = (float)PlayerMgr.Inst.ItemCtrller.relicCfg_InjuredAttackAll.int1.result * (PlayerMgr.Inst.ExtraDamageRatio + 1f);
					if (componentData.unitCfg.id != 10501)
					{
						num = ((componentData.unitCfg.unitType != UnitType.Elite && componentData.unitCfg.unitType != UnitType.Boss) ? (num + (float)Mathf.CeilToInt(componentData.unitCfg.maxHP * PlayerMgr.Inst.ItemCtrller.relicCfg_InjuredAttackAll.float1.result / 100f)) : (num + (float)Mathf.CeilToInt(componentData.unitCfg.maxHP * PlayerMgr.Inst.ItemCtrller.relicCfg_InjuredAttackAll.float2.result / 100f)));
					}
					TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
					info2.damageRecordId = PlayerMgr.Inst.ItemCtrller.relicCfg_InjuredAttackAll.id;
					info2.damage = num;
					info2.isPercentageDamage = true;
					UnitDotsSyncSystem.AddTakeDamageRequest(entity, info2);
				}
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_Variator != null && info.damage > 0f)
		{
			PlayerMgr.Inst.ItemCtrller.relic_Variator.BeHit();
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_InjuredAddMoveSpeed != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_InjuredAddMoveSpeed.PlayerTakeDamage();
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_InvisibleWing != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_InvisibleWing.PlayerTakeDamage();
		}
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_InjuredLoseCoin != null && info.damage > 0f && PlayerMgr.Inst.CoinCount > 0)
		{
			int num2 = PlayerMgr.Inst.ItemCtrller.curseCfg_InjuredLoseCoin.int1.result;
			if (num2 > PlayerMgr.Inst.CoinCount)
			{
				num2 = PlayerMgr.Inst.CoinCount;
			}
			PlayerMgr.Inst.ChangeCoin(-num2);
			if (GameMgr.IsSupportVFX)
			{
				QuickCreateSystem.Inst.CreateTextFloatVFX(-num2, UITextFloatType.DropCoin, PlayerMgr.Inst.PlayerPoint + new Vector3(0f, 0.5f, 0f));
			}
			else
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize("-" + num2, UITextFloatType.DropCoin, PlayerMgr.Inst.PlayerPoint + new Vector3(0f, 0.5f, 0f));
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_IsInjuredRandomPoint && info.damage > 0f)
		{
			Vector3 zero = Vector3.zero;
			zero = ((LevelMgr.Inst.CurrentRoomCfg.themeType != RoomThemeType.Theme6_Chapter3 && LevelMgr.Inst.CurrentRoomCfg.themeType != RoomThemeType.Theme22_Chapter3_Shortcut1) ? (LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(UnityEngine.Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.width / 2f), UnityEngine.Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.height / 2f), 0f)) : (LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(UnityEngine.Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width / 2f), UnityEngine.Random.Range((float)(-LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height) / 2f, (float)LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height / 2f), 0f)));
			zero = LevelMgr.Inst.CurrentRoomCtrller.GetDoorToWalkablePoint(zero);
			ObjPoolMgr.Inst.GetGO("Prefabs/Item/Curse_InjuredRandomPoint", base.transform.position, 2f);
			ObjPoolMgr.Inst.GetGO("Prefabs/Item/Curse_InjuredRandomPoint", zero, 2f);
			PlayerMgr.Inst.SetPlayerPoint(zero);
			SEMgr.Inst.curseInjuredRandomPoint.PlaySE();
		}
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_InjuredLoseMaxHP != null && info.damage > 0f && UnityEngine.Random.value * 100f <= (float)PlayerMgr.Inst.ItemCtrller.curseCfg_InjuredLoseMaxHP.int1.result)
		{
			PlayerMgr.Inst.ChangeHPMax(0f - info.damage);
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_InjuredCantShoot != null && info.damage > 0f)
		{
			PlayerMgr.Inst.ItemCtrller.curse_InjuredCantShoot.Injured();
		}
		if (PlayerMgr.Inst.GetRuneEffectLevel(PlayerMgr.Inst.GetPlayerRuneCount().GreenRune) >= 1)
		{
			foreach (Wand wand in PlayerMgr.Inst.Wands)
			{
				if (wand.passiveGreenRuneCount > 0)
				{
					PlayerMgr.Inst.ForceTriggerGreenRune(PlayerMgr.Inst.PlayerPoint);
					break;
				}
			}
		}
		EventMgr.PlayerHPOrShiledChange?.Invoke();
	}

	public void AfterMonsterDead(Entity monsterEtt)
	{
		UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(monsterEtt);
		if (componentData.unitCfg.IsSameCamp(UnitType.Player) || !componentData.unitCfg.triggerDeadEvent)
		{
			return;
		}
		LocalTransform componentData2 = ettMgr.GetComponentData<LocalTransform>(monsterEtt);
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_SuckBlood != null && UnityEngine.Random.value <= (float)PlayerMgr.Inst.ItemCtrller.relicCfg_SuckBlood.int1.result / 100f)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/Item/Relic_SuckBlood", base.transform.position, 2f, myPpt.transform);
			UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, PlayerMgr.Inst.ItemCtrller.relicCfg_SuckBlood.int2.result, ettMgr);
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_FollowGhost != null && UnityEngine.Random.value <= (float)PlayerMgr.Inst.ItemCtrller.relicCfg_FollowGhost.int1.result / 100f)
		{
			Entity entity = QuickCreateSystem.Inst.CreateMixedEtt("Relic_FollowGhost", Tool2D.IgnoreZPoint(componentData2.Position));
			Relic_FollowGhost componentData3 = ettMgr.GetComponentData<Relic_FollowGhost>(entity);
			componentData3.damage = (float)(PlayerMgr.Inst.ItemCtrller.relicCfg_FollowGhost.int2.value * (int)Mathf.Pow(2f, PlayerMgr.Inst.ItemCtrller.relicCfg_FollowGhost.level - 1)) * (1f + PlayerMgr.Inst.ExtraDamageRatio);
			componentData3.RelicId = PlayerMgr.Inst.ItemCtrller.relicCfg_FollowGhost.id;
			ettMgr.SetComponentData(entity, componentData3);
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_BlockSpellMono != null && UnityEngine.Random.value <= (float)PlayerMgr.Inst.ItemCtrller.relic_BlockSpellMono.RelicCfg.int1.result / 100f)
		{
			PlayerMgr.Inst.ItemCtrller.relic_BlockSpellMono.AddBlock(Tool2D.IgnoreZPoint(componentData2.Position));
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_GluttonousSnake != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_GluttonousSnake.CreateBody(componentData.unitCfg.maxHP);
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_Alchemy != null && UnityEngine.Random.value <= (float)PlayerMgr.Inst.ItemCtrller.relicCfg_Alchemy.int1.result / 100f)
		{
			Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(componentData2.Position);
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Resource, 11), navMeshPointIngoreZ);
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_KillShield != null)
		{
			PlayerMgr.Inst.ItemCtrller.relicCfg_KillShield.intTimer++;
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_KillShield.intTimer >= PlayerMgr.Inst.ItemCtrller.relicCfg_KillShield.int2.result)
			{
				PlayerMgr.Inst.ItemCtrller.relicCfg_KillShield.intTimer = 0;
				PlayerMgr.Inst.ChangeShield(PlayerMgr.Inst.ItemCtrller.relicCfg_KillShield.int3.result, TextFloatQueueType.DirectFloat);
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_KillBackMP != null)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/Item/Relic_KillBackMP", base.transform.position, 2f, base.transform);
			PlayerMgr.Inst.ChangeMPCurrent(PlayerMgr.Inst.ItemCtrller.relicCfg_KillBackMP.int1.result);
		}
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_KillTempShiled != null)
		{
			PlayerMgr.Inst.ItemCtrller.relicCfg_KillTempShiled.intTimer++;
			if (PlayerMgr.Inst.ItemCtrller.relicCfg_KillTempShiled.intTimer >= PlayerMgr.Inst.ItemCtrller.relicCfg_KillTempShiled.int1.result)
			{
				PlayerMgr.Inst.ItemCtrller.relicCfg_KillTempShiled.intTimer -= PlayerMgr.Inst.ItemCtrller.relicCfg_KillTempShiled.int1.result;
				ObjPoolMgr.Inst.GetGO("Prefabs/Item/Relic_KillTempShield", base.transform.position, 2f, base.transform);
				PlayerMgr.Inst.ChangeShieldTemp(PlayerMgr.Inst.ItemCtrller.relicCfg_KillTempShiled.int2.result);
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.curseCfg_RevengeGhost != null && UnityEngine.Random.value <= (float)PlayerMgr.Inst.ItemCtrller.curseCfg_RevengeGhost.int1.result / 100f)
		{
			Entity entity2 = QuickCreateSystem.Inst.CreateMixedEtt("Curse_RevengeGhost", Tool2D.IgnoreZPoint(componentData2.Position));
			Curse_RevengeGhost componentData4 = ettMgr.GetComponentData<Curse_RevengeGhost>(entity2);
			componentData4.duration = PlayerMgr.Inst.ItemCtrller.curseCfg_RevengeGhost.float1.result;
			ettMgr.SetComponentData(entity2, componentData4);
		}
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		ps_FootStepSmoke.Stop();
		Vector3 position = base.transform.position;
		this.OnWillTheme6Reposition?.Invoke(position + changeValue);
		PlayerMgr.Inst.SetPlayerPoint(base.transform.position + changeValue);
		this.OnTheme6Reposition?.Invoke(position);
		ps_FootStepSmoke.Play();
		if (PlayerMgr.Inst.ItemCtrller.relic_BlockSpellMono != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_BlockSpellMono.PointerToPlayer();
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_Fly != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_Fly.Theme6Reposition(changeValue);
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_InvisibleWing != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_InvisibleWing.Theme6Reposition(changeValue);
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_AddMoveSpeed.Theme6Reposition(changeValue);
		}
		if (PlayerMgr.Inst.ItemCtrller.curse_Shackle != null)
		{
			PlayerMgr.Inst.ItemCtrller.curse_Shackle.PointerToPlayer();
		}
		for (int i = 0; i < followObjs.Count; i++)
		{
			followObjs[i].transform.position += changeValue;
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Room6Teleport", base.transform.position, 3f);
		SEMgr.Inst.playerThroughT6.PlaySE();
	}

	public void SetEmoji(PlayerEmojiType emojiType)
	{
		if (this.emojiType != emojiType)
		{
			this.emojiType = emojiType;
			switch (emojiType)
			{
			case PlayerEmojiType.Other:
				SAnima.AnimationState.SetAnimation(1, "Emoji/Normal", loop: false);
				break;
			case PlayerEmojiType.Normal:
				SAnima.AnimationState.SetAnimation(1, "Emoji/Normal", loop: false);
				break;
			case PlayerEmojiType.BlackEye:
				SAnima.AnimationState.SetAnimation(1, "Emoji/BlackEye", loop: false);
				break;
			case PlayerEmojiType.Amaze:
				SAnima.AnimationState.SetAnimation(1, "Emoji/Amaze", loop: false);
				break;
			case PlayerEmojiType.Panic:
				SAnima.AnimationState.SetAnimation(1, "Emoji/Panic", loop: false);
				break;
			default:
				Debug.LogError(emojiType);
				break;
			}
		}
	}

	public void SetBodyAnima(PlayerBodyAnima bodyAnima, float timeScale = 1f)
	{
		if (isNvliuEating)
		{
			return;
		}
		if (CurrentBodyAnima == bodyAnima)
		{
			if (CurrentAnimaTimeScale != timeScale)
			{
				CurrentAnimaTimeScale = timeScale;
				SAnima.AnimationState.GetCurrent(0).TimeScale = timeScale;
			}
			return;
		}
		CurrentBodyAnima = bodyAnima;
		switch (bodyAnima)
		{
		case PlayerBodyAnima.GroundIdleDown:
			SAnima.AnimationState.SetAnimation(0, "GroundIdleDown", loop: true).TimeScale = timeScale;
			break;
		case PlayerBodyAnima.GroundIdleUp:
			SAnima.AnimationState.SetAnimation(0, "GroundIdleUp", loop: true).TimeScale = timeScale;
			break;
		case PlayerBodyAnima.GroundWalkDown:
			SAnima.AnimationState.SetAnimation(0, "GroundWalkDown", loop: true).TimeScale = timeScale;
			break;
		case PlayerBodyAnima.GroundWalkUp:
			SAnima.AnimationState.SetAnimation(0, "GroundWalkUp", loop: true).TimeScale = timeScale;
			break;
		case PlayerBodyAnima.FlyIdleDown:
			SAnima.AnimationState.SetAnimation(0, "FlyIdleDown", loop: true).TimeScale = timeScale;
			break;
		case PlayerBodyAnima.FlyIdleUp:
			SAnima.AnimationState.SetAnimation(0, "FlyIdleUp", loop: true).TimeScale = timeScale;
			break;
		case PlayerBodyAnima.FlyWalkDown:
			SAnima.AnimationState.SetAnimation(0, DataMgr.selectedWorldData.IsDave ? "FlyWalkDown_Dave" : "FlyWalkDown", loop: true).TimeScale = timeScale;
			break;
		case PlayerBodyAnima.FlyWalkUp:
			SAnima.AnimationState.SetAnimation(0, DataMgr.selectedWorldData.IsDave ? "FlyWalkUp_Dave" : "FlyWalkUp", loop: true).TimeScale = timeScale;
			break;
		case PlayerBodyAnima.Dead:
			SAnima.AnimationState.SetAnimation(0, "Dead", loop: false);
			break;
		default:
			Debug.LogError(bodyAnima);
			break;
		}
	}

	public void TextFloatQueueAdd(string content, UITextFloatType type)
	{
		TextFloatQueueAdd(new TextFloatQueueContent(content, type));
	}

	public void TextFloatQueueAdd(TextFloatQueueContent QueueContent)
	{
		queueTypes.Add(QueueContent);
	}

	public void TextFloatQueueClear()
	{
		queueTypes.Clear();
	}

	public void FollowObjRegister(GameObject go)
	{
		if (!followObjs.Contains(go))
		{
			followObjs.Add(go);
			if (followObjs.Count == 1)
			{
				go.transform.position = base.transform.position + Tool2D.GetDir() * followObjDistance;
			}
			else
			{
				go.transform.position = followObjs[followObjs.Count - 2].transform.position + Tool2D.GetDir() * followObjDistance;
			}
		}
	}

	public void FollowObjUnregister(GameObject go)
	{
		followObjs.Remove(go);
	}

	public void FollowObjThrough()
	{
		for (int i = 0; i < followObjs.Count; i++)
		{
			if (i == 0)
			{
				followObjs[i].transform.position = base.transform.position + Tool2D.GetDir() * followObjDistance;
			}
			else
			{
				followObjs[i].transform.position = followObjs[i - 1].transform.position + Tool2D.GetDir() * followObjDistance;
			}
		}
	}

	public void followObjClear()
	{
		followObjs.Clear();
	}

	public unsafe void OpenPlayerCollider()
	{
		playerCollider.enabled = true;
		playerTrigger.enabled = true;
		PhysicsCollider componentData = ettMgr.GetComponentData<PhysicsCollider>(PlayerMgr.Inst.PlayerEtt);
		CompoundCollider* colliderPtr = (CompoundCollider*)componentData.ColliderPtr;
		Unity.Physics.Collider* collider = colliderPtr->Children[0].Collider;
		if (collider->GetCollisionResponse() == CollisionResponsePolicy.Collide)
		{
			collider->SetCollisionFilter(GameConst.Filter_PlayerCollider);
		}
		else
		{
			collider->SetCollisionFilter(GameConst.Filter_PlayerTrigger);
		}
		collider = colliderPtr->Children[1].Collider;
		if (collider->GetCollisionResponse() == CollisionResponsePolicy.Collide)
		{
			collider->SetCollisionFilter(GameConst.Filter_PlayerCollider);
		}
		else
		{
			collider->SetCollisionFilter(GameConst.Filter_PlayerTrigger);
		}
		colliderPtr->RefreshCollisionFilter();
		ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
	}

	public unsafe void ClosePlayerCollider()
	{
		playerCollider.enabled = false;
		playerTrigger.enabled = false;
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.CollidesWith = 0u;
		collisionFilter.GroupIndex = 0;
		collisionFilter.BelongsTo = 512u;
		CollisionFilter collisionFilter2 = collisionFilter;
		PhysicsCollider componentData = ettMgr.GetComponentData<PhysicsCollider>(PlayerMgr.Inst.PlayerEtt);
		CompoundCollider* colliderPtr = (CompoundCollider*)componentData.ColliderPtr;
		colliderPtr->Children[0].Collider->SetCollisionFilter(collisionFilter2);
		colliderPtr->Children[1].Collider->SetCollisionFilter(collisionFilter2);
		colliderPtr->RefreshCollisionFilter();
		ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
	}

	public void StopMotion()
	{
		CurrentMotion = Vector3.zero;
		PhysicsVelocity componentData = ettMgr.GetComponentData<PhysicsVelocity>(PlayerMgr.Inst.PlayerEtt);
		componentData.Linear = float3.zero;
		ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
		UnmotionableRegister();
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveRegister();
		if (myPpt.IsFly)
		{
			SetBodyAnima(PlayerBodyAnima.FlyIdleDown);
		}
		else
		{
			SetBodyAnima(PlayerBodyAnima.GroundIdleDown);
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_Huang.SetStop(isStop: true);
		}
	}

	public void StartMotion()
	{
		UnmotionableUnregister();
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
		if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_Huang.SetStop(isStop: false);
		}
	}

	public void StopFace(bool isFlip)
	{
		SetBodyAnima(PlayerBodyAnima.GroundIdleDown);
		SAnima.skeleton.FindSlot("Hand_L").A = 0f;
		SAnima.skeleton.FindSlot("Hand_R").A = 1f;
		SetFlip(isFlip);
		if (isFlip)
		{
			tsf_WandRoot.up = Tool2D.GetDir(120f);
		}
		else
		{
			tsf_WandRoot.up = Tool2D.GetDir(240f);
		}
	}

	public void SetFlip(bool isFlip)
	{
		SAnima.transform.localScale = new Vector3(Mathf.Abs(SAnima.transform.localScale.x) * (float)((!isFlip) ? 1 : (-1)), SAnima.transform.localScale.y, SAnima.transform.localScale.z);
	}

	public void TakeKnockback(Vector3 knockback)
	{
		if (UnitDotsSyncSystem.EntityIsValid(PlayerMgr.Inst.PlayerEtt))
		{
			UnitProperty_Dots componentData = ettMgr.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
			componentData.TakeKnockback(knockback);
			ettMgr.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
		}
	}

	public void SetShootPerformedIfHanveShootGroup(bool shoot)
	{
		if (shoot)
		{
			if ((bool)PlayerMgr.Inst.SelectedWand)
			{
				bool flag = PlayerMgr.Inst.SelectedWand.passiveRuneHammerEnable || PlayerMgr.Inst.SelectedWand.LaserCrystalCount != 0;
				bool flag2 = PlayerMgr.Inst.SelectedWand.currentShootGroup != null;
				isHoldMouse0 = flag2 || !flag;
			}
		}
		else
		{
			isHoldMouse0 = false;
		}
	}

	public int GetGreenRuneDecreaseDamageAmount()
	{
		int item = PlayerMgr.Inst.GetPlayerRuneCount().GreenRune;
		if (PlayerMgr.Inst.GetRuneEffectLevel(item) >= 1)
		{
			return Mathf.FloorToInt((float)item / 7f);
		}
		return 0;
	}

	public void SetShootSlowdown()
	{
		shootSlowdownTimer = 0.2f;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1f, 1f, 1f, 0.1f);
		Gizmos.DrawCube(base.transform.position + new Vector3(0f, 0.58f, 0f), new Vector3(0.77f, 1.3f, 1f));
	}

	public void DashOverHeatRegister(float duration)
	{
		if (!(duration <= dashOverHeatTimer))
		{
			dashOverHeatTimer = duration;
			lastDashOverHeatTimeMax = duration;
		}
	}

	public void OnPlayerDash()
	{
		ClosePlayerCollider();
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveRegister();
		myPpt.SetUnitInvisibleState(state: true);
		PlayerMgr.Inst.SelectedWand.Display_Hide();
		PlayerMgr.Inst.inDashSpell = true;
		if (GameMgr.IsMobile_Static)
		{
			inputRightStickLast = inputLeftStickLast;
		}
	}

	public void OnPlayerDashEnd()
	{
		OpenPlayerCollider();
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
		myPpt.SetUnitInvisibleState(state: false);
		if (PlayerMgr.Inst.SelectedWand != null && PlayerMgr.Inst.SelectedWand.WandCfg != null)
		{
			PlayerMgr.Inst.SelectedWand.Display_Show();
		}
		PlayerMgr.Inst.inDashSpell = false;
	}

	private void ShootCanceld(InputAction.CallbackContext context)
	{
		isHoldMouse0 = false;
		CastLockUnRegister();
		if (!(PlayerMgr.Inst.SelectedWand == null))
		{
			WandChargeEffect(chargeStart: false);
		}
	}

	public void DrinkPerformed(InputAction.CallbackContext context)
	{
		DrinkPerformed();
	}

	public void DrinkPerformed()
	{
		isHoldMouse1 = true;
		if (GameMgr.IsMobile_Static)
		{
			if (PlayerMgr.Inst.ItemCtrller.SelectedPotionID != 0)
			{
				usingPotion = true;
			}
		}
		else if (PlayerMgr.Inst.ItemCtrller.SelectedPotionID != 0)
		{
			if (ControlMgr.Inst.usingpad)
			{
				usingPotion = true;
			}
			else if (!ControlMgr.Inst.Getcursorover())
			{
				showdrinking = true;
				UIPlayerDataMgr.Inst.UISlotPotionDragCancel();
				usingPotion = true;
				uiPotionUse.Show(PlayerMgr.Inst.ItemCtrller.SelectedPotionID);
			}
		}
	}

	public void DrinkCanceld(InputAction.CallbackContext context)
	{
		DrinkCanceld();
	}

	public void DrinkCanceld()
	{
		isHoldMouse1 = false;
		if (!ControlMgr.Inst.usingpad)
		{
			if (showdrinking)
			{
				usingPotion = false;
				showdrinking = false;
				uiPotionUse.Hide();
			}
			usePotionTimer = 0f;
		}
	}

	private void Alpha1Performed(InputAction.CallbackContext context)
	{
		if (PlayerMgr.Inst.CanSelectWand(0) && PlayerMgr.Inst.WandSelect(0))
		{
			changeWandCooling = 0.2f;
		}
	}

	private void Alpha2Performed(InputAction.CallbackContext context)
	{
		if (PlayerMgr.Inst.CanSelectWand(1) && PlayerMgr.Inst.WandSelect(1))
		{
			changeWandCooling = 0.2f;
		}
	}

	private void Alpha3Performed(InputAction.CallbackContext context)
	{
		if (PlayerMgr.Inst.CanSelectWand(2) && PlayerMgr.Inst.WandSelect(2))
		{
			changeWandCooling = 0.2f;
		}
	}

	private void Alpha4Performed(InputAction.CallbackContext context)
	{
		if (PlayerMgr.Inst.CanSelectWand(3) && PlayerMgr.Inst.WandSelect(3))
		{
			changeWandCooling = 0.2f;
		}
	}

	private void Alpha5Performed(InputAction.CallbackContext context)
	{
		if (PlayerMgr.Inst.CanSelectWand(4) && PlayerMgr.Inst.WandSelect(4))
		{
			changeWandCooling = 0.2f;
		}
	}

	private void Alpha6Performed(InputAction.CallbackContext context)
	{
		if (PlayerMgr.Inst.CanSelectWand(5) && PlayerMgr.Inst.WandSelect(5))
		{
			changeWandCooling = 0.2f;
		}
	}

	private void Alpha7Performed(InputAction.CallbackContext context)
	{
		if (PlayerMgr.Inst.CanSelectWand(6) && PlayerMgr.Inst.WandSelect(6))
		{
			changeWandCooling = 0.2f;
		}
	}

	private void WandUpPerformed(InputAction.CallbackContext context)
	{
		if (!UIPlayerDataMgr.Inst.HaveUIOpen())
		{
			PlayerMgr.Inst.WandSelectOffset(-1);
		}
	}

	private void WandDownPerformed(InputAction.CallbackContext context)
	{
		if (!UIPlayerDataMgr.Inst.HaveUIOpen())
		{
			PlayerMgr.Inst.WandSelectOffset(1);
		}
	}

	private void RightStickPerformed(InputAction.CallbackContext context)
	{
		inputRightStick = context.ReadValue<Vector2>();
		inputRightStickLast = inputRightStick;
		inputStickLast = inputRightStickLast;
		if (!GameMgr.IsMobile_Static || MobileMgr.inst.gamepadPlugged)
		{
			return;
		}
		if (MobileMgr.inst.topui.attackImage.sprite == MobileMgr.inst.topui.spriteAttack)
		{
			if (CanMotion && inputRightStick.sqrMagnitude > 0f)
			{
				SetShootPerformedIfHanveShootGroup(shoot: true);
			}
			else
			{
				SetShootPerformedIfHanveShootGroup(shoot: false);
			}
		}
		else
		{
			UIInteractiveObjMgr.Inst.Interact();
		}
	}

	private void RightStickCancled(InputAction.CallbackContext context)
	{
		inputRightStick = Vector3.zero;
	}

	private void PotionUpPerformed(InputAction.CallbackContext context)
	{
		PlayerMgr.Inst.ItemCtrller.PotionChangeSelect(nextOne: false);
	}

	private void PotionDownPerformed(InputAction.CallbackContext context)
	{
		PlayerMgr.Inst.ItemCtrller.PotionChangeSelect(nextOne: true);
	}

	private void PotionDownCanceled(InputAction.CallbackContext context)
	{
	}
}
