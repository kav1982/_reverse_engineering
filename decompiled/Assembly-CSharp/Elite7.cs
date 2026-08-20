using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Elite7 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		EnterGround,
		PeepBoundary,
		Peep,
		PeepFinish,
		PeepHide,
		PeepAttackIdle,
		JumpBoundary,
		Jumping,
		JumpBoundaryOnGround,
		jumpBoundaryEnterGround,
		JumpGroundBefore,
		JumpGround
	}

	[Space(50f)]
	public Shadow shadow;

	public Animator anima_RT;

	public AnimaEvent animaEvent_RT;

	public float bornIdleTime;

	[Header("Performance")]
	public GameObject go_RT;

	public GameObject go_Performance;

	public GameObject go_RTMove;

	public GameObject go_Model;

	public GameObject go_GroundPortal;

	public SpriteRenderer sr_Body;

	public SpriteRenderer sr_GroundPotral;

	public SpriteRenderer sr_Body_Performance;

	public int RTPixelsPerMeter;

	[Header("Peep")]
	public GameObject go_PeepShowEF;

	public GameObject go_PeepHideEF;

	[Range(0f, 1f)]
	public float peepBoundaryChance;

	public float peepCornerDistance;

	public float peepGroundDistance;

	public VariableFloat peepShowTime;

	[Header("PeepAttack")]
	public GameObject pfb_Trap;

	public GameObject pfb_Trap_H;

	public MeshRenderer mr_Trap;

	public Vector2Int mrScale;

	public int trapShootCount;

	public int trapMaxCount;

	[Range(0f, 1f)]
	public float peepAttackChance;

	public float peepAttackOffset;

	public float peepAttackHeight;

	public float peepAttackMaxDistance;

	public float peepAttackTrapUpSpeed;

	public float peepAttackTrapGravity;

	public float peepAttackTrapRadius;

	public float peepAttackIdleTime;

	public VariableInt peepTimeToAction;

	[Header("JumpBoundary")]
	public Transform tsf_Motion;

	public float jumpBoundaryOffset;

	public float jumpBoundaryUpForce;

	public float jumpBoundaryHeight;

	public float jumpBoundaryGravity;

	public float jumpBoundaryNoTargetRetractDistance;

	public float jumpBoundaryOnGroundCCRadius;

	public float jumpBoundaryOnGroundDuration;

	public int jumpBoundarySpellCount;

	public float jumpBoundarySpellAngle;

	public ParticleSystem jumpBoundarySystem;

	public ParticleSystem jumpOnGroundParticle;

	public float jumpOnGroundImpact;

	public int jumpOnGroundDamage;

	[Header("JumpGround")]
	public float jumpGroundBeforeTime;

	public float jumpGroundUpForce;

	public float jumpGroundHeight;

	public float jumpGroundGravity;

	public float jumpGroundMinDistance;

	public ParticleSystem beforeJumpSystem;

	private bool jumpSpeedLocked;

	private Vector3 jumpedDir;

	[Header("JumpGroundTrap")]
	public int jumpGroundTrapCount;

	public VariableFloat jumpGroundTrapGravity;

	public VariableFloat jumpGroundTrapUpSpeed;

	public float jumpGroundTrapLandRadius;

	public float jumpGroundTrapRadius;

	[Header("死亡变脸")]
	public SpriteRenderer SR_Face;

	public SpriteRenderer SR_FaceRT;

	public Sprite Sprite_Dead;

	[Header("和谐模式")]
	public UnityEngine.Material mat_H;

	public static Elite7 Inst;

	public MonsterState state;

	private List<Elite7_Trap> traps = new List<Elite7_Trap>();

	private float originalCCRadius;

	private float peepShowTimer;

	private bool isPeepBoundary;

	private float peepAttackIdleTimer;

	private float peepTimer;

	private Vector3 jumpFlyingSpeed;

	private float jumpBoundaryOnGroundTimer;

	private float jumpGroundBeforeTimer;

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void Start()
	{
		go_RT.transform.parent = base.transform.parent;
		go_Performance.transform.parent = base.transform.parent;
		go_RT.transform.position = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		go_Performance.transform.position = Tool2D.IgnoreZPoint(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint, -1f);
		go_GroundPortal.transform.position = Tool2D.IgnoreZPoint(base.transform, 1.08f);
		myPpt.SR_Models = new SpriteRenderer[3] { sr_Body, sr_Body_Performance, sr_GroundPotral };
		myPpt.MR_Models = new MeshRenderer[0];
		shadow.CreateShadow();
		shadow.ShadowGO.SetActive(value: false);
		RoomController currentRoomCtrller = LevelMgr.Inst.CurrentRoomCtrller;
		Vector2Int vector2Int = new Vector2Int(mrScale.x * RTPixelsPerMeter, mrScale.y * RTPixelsPerMeter);
		RenderTexture renderTexture = new RenderTexture(vector2Int.x, vector2Int.y, GraphicsFormat.R8G8B8A8_UNorm, GraphicsFormat.D16_UNorm);
		RTCamController component = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/RTCam"), LevelMgr.Inst.CurrentRoomT).GetComponent<RTCamController>();
		component.cam.transform.parent.position = Tool2D.IgnoreZPoint(currentRoomCtrller.CenterPoint, -160f);
		component.cam.targetTexture = renderTexture;
		component.cam.orthographicSize = (float)mrScale.y / 2f;
		component.MaxFps = 60;
		mr_Trap.transform.position = Tool2D.IgnoreZPoint(currentRoomCtrller.CenterPoint, 1.11f);
		mr_Trap.transform.localScale = new Vector3(mrScale.x, mrScale.y, 1f);
		mr_Trap.material.SetTexture("_MainTex", renderTexture);
		mr_Trap.material.SetColor("_Color", Color.red);
		if (GameMgr.IsChAge14_Static)
		{
			Object.Destroy(SR_Face.material);
			SR_Face.material = mat_H;
			Object.Destroy(SR_FaceRT.material);
			SR_FaceRT.material = mat_H;
			pfb_Trap = pfb_Trap_H;
			mr_Trap.material.SetColor("_Color", Color.magenta);
		}
		mr_Trap.transform.parent = LevelMgr.Inst.CurrentRoomT;
		originalCCRadius = myPpt.CC_Self.radius;
		animaEvent_RT.DoAction = AnimaAction;
		peepShowTime.RandomResult();
		peepTimeToAction.RandomResult();
		Inst = this;
		if (GameMgr.IsMobile_Static)
		{
			trapMaxCount = Mathf.CeilToInt((float)trapMaxCount * 0.9f);
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= bornIdleTime)
			{
				state = MonsterState.EnterGround;
				base.Anima.SetTrigger("PeepGroundHide");
			}
			break;
		case MonsterState.PeepFinish:
			peepShowTimer += Time.deltaTime;
			if (peepShowTimer > peepShowTime.result)
			{
				peepShowTimer = 0f;
				peepShowTime.RandomResult();
				state = MonsterState.PeepHide;
				if (isPeepBoundary)
				{
					base.CC_Self.enabled = false;
					SetDotsCCEnable(isOpen: false);
					anima_RT.SetTrigger("PeepBoundaryHide");
				}
				else
				{
					base.Anima.SetTrigger("PeepGroundHide");
				}
			}
			break;
		case MonsterState.PeepAttackIdle:
			peepAttackIdleTimer += Time.deltaTime;
			if (peepAttackIdleTimer >= peepAttackIdleTime)
			{
				peepAttackIdleTimer = 0f;
				state = MonsterState.PeepHide;
				if (isPeepBoundary)
				{
					base.CC_Self.enabled = false;
					SetDotsCCEnable(isOpen: false);
					anima_RT.SetTrigger("PeepBoundaryHide");
				}
				else
				{
					base.Anima.SetTrigger("PeepGroundHide");
				}
			}
			break;
		case MonsterState.Jumping:
			base.transform.Translate(jumpFlyingSpeed * Time.deltaTime);
			SyncDotsPosition();
			if (base.transform.position.z >= 0f)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				JumpStop_Dots();
				state = MonsterState.JumpBoundaryOnGround;
				base.Anima.SetTrigger("JumpBoundaryOnGround");
				myPpt.correctType = LayerCorrectType.SlimeOnGround;
			}
			break;
		case MonsterState.JumpBoundaryOnGround:
			jumpBoundaryOnGroundTimer += Time.deltaTime;
			if (jumpBoundaryOnGroundTimer > jumpBoundaryOnGroundDuration)
			{
				jumpBoundaryOnGroundTimer = 0f;
				state = MonsterState.jumpBoundaryEnterGround;
				base.Anima.SetTrigger("JumpBoundaryEnterGround");
			}
			break;
		case MonsterState.JumpGroundBefore:
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				base.transform.Translate(ToTargetDir() * base.MoveSpeed * Time.deltaTime);
				SyncDotsPosition();
			}
			jumpGroundBeforeTimer += Time.deltaTime;
			if (jumpGroundBeforeTimer > jumpGroundBeforeTime)
			{
				beforeJumpSystem.Stop();
				jumpSpeedLocked = false;
				jumpGroundBeforeTimer = 0f;
				base.Anima.SetTrigger("JumpGround");
				state = MonsterState.JumpGround;
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = true;
				SetComponentData(componentData);
			}
			break;
		case MonsterState.JumpGround:
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget && !jumpSpeedLocked)
			{
				base.transform.Translate(ToTargetDir() * base.MoveSpeed * Time.deltaTime);
			}
			else
			{
				base.transform.Translate(jumpedDir * base.MoveSpeed * Time.deltaTime);
			}
			SyncDotsPosition();
			break;
		default:
			Debug.LogError(state);
			break;
		case MonsterState.EnterGround:
		case MonsterState.PeepBoundary:
		case MonsterState.Peep:
		case MonsterState.PeepHide:
		case MonsterState.JumpBoundary:
		case MonsterState.jumpBoundaryEnterGround:
			break;
		}
	}

	private unsafe void RandomPeep()
	{
		Vector3 vector = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		float num = (float)LevelMgr.Inst.CurrentRoomCfg.theme6Width / 2f;
		float num2 = (float)LevelMgr.Inst.CurrentRoomCfg.theme6Height / 2f;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
		if (Random.value <= peepBoundaryChance)
		{
			FourDir randomFourDir = Tool2D.GetRandomFourDir();
			PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
			Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)componentData2.ColliderPtr;
			CapsuleGeometry geometry = colliderPtr->Geometry;
			switch (randomFourDir)
			{
			case FourDir.Up:
				vector.x += Random.Range(0f - num + peepCornerDistance, num - peepCornerDistance);
				vector.y += num2;
				myPpt.CC_Self.direction = 1;
				geometry.Radius = base.CC_Self.radius;
				geometry.Vertex0 = base.CC_Self.center + new Vector3(0f, base.CC_Self.height / 2f, 0f);
				geometry.Vertex1 = base.CC_Self.center - new Vector3(0f, base.CC_Self.height / 2f, 0f);
				break;
			case FourDir.Right:
				vector.x += num;
				vector.y += Random.Range(0f - num2 + peepCornerDistance, num2 - peepCornerDistance);
				myPpt.CC_Self.direction = 0;
				geometry.Radius = base.CC_Self.radius;
				geometry.Vertex0 = base.CC_Self.center + new Vector3(base.CC_Self.height / 2f, 0f, 0f);
				geometry.Vertex1 = base.CC_Self.center - new Vector3(base.CC_Self.height / 2f, 0f, 0f);
				break;
			case FourDir.Down:
				vector.x += Random.Range(0f - num + peepCornerDistance, num - peepCornerDistance);
				vector.y += 0f - num2;
				myPpt.CC_Self.direction = 1;
				geometry.Radius = base.CC_Self.radius;
				geometry.Vertex0 = base.CC_Self.center + new Vector3(0f, base.CC_Self.height / 2f, 0f);
				geometry.Vertex1 = base.CC_Self.center - new Vector3(0f, base.CC_Self.height / 2f, 0f);
				break;
			case FourDir.Left:
				vector.x += 0f - num;
				vector.y += Random.Range(0f - num2 + peepCornerDistance, num2 - peepCornerDistance);
				myPpt.CC_Self.direction = 0;
				geometry.Radius = base.CC_Self.radius;
				geometry.Vertex0 = base.CC_Self.center + new Vector3(base.CC_Self.height / 2f, 0f, 0f);
				geometry.Vertex1 = base.CC_Self.center - new Vector3(base.CC_Self.height / 2f, 0f, 0f);
				break;
			default:
				Debug.LogError(randomFourDir);
				break;
			}
			colliderPtr->Geometry = geometry;
			SetComponentData(componentData2);
			go_RTMove.transform.position = Tool2D.IgnoreZPoint(vector, go_RTMove.transform.position.z);
			go_RTMove.transform.rotation = Tool2D.GetRotationByFourDirInverted(randomFourDir);
			if (peepTimer >= (float)peepTimeToAction.result)
			{
				peepTimer = 0f;
				peepTimeToAction.RandomResult();
				base.Anima.SetTrigger("TotalHide");
				state = MonsterState.JumpBoundary;
				anima_RT.SetTrigger("JumpBoundary");
				go_Model.transform.rotation = Quaternion.identity;
				myPpt.CC_Self.direction = 2;
				PhysicsCollider componentData3 = GetComponentData<PhysicsCollider>();
				Unity.Physics.CapsuleCollider* colliderPtr2 = (Unity.Physics.CapsuleCollider*)componentData3.ColliderPtr;
				CapsuleGeometry geometry2 = colliderPtr2->Geometry;
				geometry2.Radius = base.CC_Self.radius;
				geometry2.Vertex0 = base.CC_Self.center + new Vector3(0f, 0f, base.CC_Self.height / 2f);
				geometry2.Vertex1 = base.CC_Self.center - new Vector3(0f, 0f, base.CC_Self.height / 2f);
				colliderPtr2->Geometry = geometry2;
				SetComponentData(componentData3);
			}
			else
			{
				peepTimer += 1f;
				state = MonsterState.Peep;
				base.Anima.SetTrigger("TotalHide");
				anima_RT.SetTrigger("PeepBoundaryShow");
				go_Model.transform.rotation = go_RTMove.transform.rotation;
				isPeepBoundary = true;
			}
		}
		else
		{
			int num3 = 0;
			do
			{
				num3++;
				if (num3 >= 100)
				{
					break;
				}
				vector.x += Random.Range(0f - num + peepGroundDistance, num - peepGroundDistance);
				vector.y += Random.Range(0f - num2 + peepGroundDistance, num2 - peepGroundDistance);
			}
			while (!((vector - PlayerMgr.Inst.PlayerPoint).sqrMagnitude > peepGroundDistance));
			myPpt.CC_Self.direction = 2;
			PhysicsCollider componentData4 = GetComponentData<PhysicsCollider>();
			Unity.Physics.CapsuleCollider* colliderPtr3 = (Unity.Physics.CapsuleCollider*)componentData4.ColliderPtr;
			CapsuleGeometry geometry3 = colliderPtr3->Geometry;
			geometry3.Radius = base.CC_Self.radius;
			geometry3.Vertex0 = base.CC_Self.center + new Vector3(0f, 0f, base.CC_Self.height / 2f);
			geometry3.Vertex1 = base.CC_Self.center - new Vector3(0f, 0f, base.CC_Self.height / 2f);
			colliderPtr3->Geometry = geometry3;
			SetComponentData(componentData4);
			go_Model.transform.rotation = Quaternion.identity;
			base.transform.ChangeAllLayer("Monster");
			if (peepTimer >= (float)peepTimeToAction.result)
			{
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget && ToTargetDistance() < jumpGroundMinDistance)
				{
					vector = base.TargetPointIgnoreZ - ToTargetDir() * jumpGroundMinDistance;
				}
				peepTimer = 0f;
				peepTimeToAction.RandomResult();
				base.Anima.SetTrigger("JumpGroundBefore");
				state = MonsterState.JumpGroundBefore;
				myPpt.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
			}
			else
			{
				peepTimer += 1f;
				state = MonsterState.Peep;
				base.Anima.SetTrigger("PeepGroundShow");
				isPeepBoundary = false;
			}
		}
		base.transform.position = vector;
		SyncDotsPosition();
	}

	public unsafe override void AnimaAction(string animaName)
	{
		if (base.deadStayed || !EntityIsValid(myPpt.myEntity))
		{
			return;
		}
		switch (animaName)
		{
		case "PlayBounceSE":
			SEMgr.Inst.elite7Bounce.PlaySE();
			break;
		case "PlayHideSE":
			SEMgr.Inst.elite7Hide.PlaySE();
			break;
		case "PlayGroundSE":
			SEMgr.Inst.elite7Ground.PlaySE();
			break;
		case "PlayGroundEF":
			beforeJumpSystem.Play();
			break;
		case "PlayWormingSE":
			SEMgr.Inst.elite7Worming.PlaySE();
			break;
		case "PlayDropSE":
			JumpGroundImpact();
			SEMgr.Inst.elite7Drop.PlaySE();
			break;
		case "NoTouch":
		{
			UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
			componentData3.CanTouch = false;
			SetComponentData(componentData3);
			break;
		}
		case "HideShadow":
			shadow.ShadowGO.SetActive(value: false);
			break;
		case "HideToGroundFinish":
			RandomPeep();
			break;
		case "CanTouch":
		{
			UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
			componentData3.CanTouch = true;
			SetComponentData(componentData3);
			break;
		}
		case "EFPeepShow":
			go_PeepShowEF.SetActive(value: false);
			go_PeepShowEF.SetActive(value: true);
			break;
		case "EFPeepHide":
			go_PeepHideEF.SetActive(value: false);
			go_PeepHideEF.SetActive(value: true);
			break;
		case "PeepShowFinish":
			myPpt.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget && Random.value <= peepAttackChance)
			{
				if (isPeepBoundary)
				{
					anima_RT.SetTrigger("PeepBoundaryAttack");
				}
				else
				{
					base.Anima.SetTrigger("PeepGroundAttack");
				}
			}
			else
			{
				state = MonsterState.PeepFinish;
			}
			break;
		case "PeepHideFinish":
			RandomPeep();
			break;
		case "PeepAttack":
			GetNearestTarget();
			if (base.HaveTarget)
			{
				SEMgr.Inst.elite7Vomit.PlaySE();
				Vector3 position = base.transform.position + new Vector3(0f, 0f, 0f - peepAttackHeight);
				if (isPeepBoundary)
				{
					position += go_RTMove.transform.up * peepAttackOffset - Vector3.up * peepAttackHeight;
				}
				Vector3 vector2 = Tool2D.IgnoreZPoint(base.TargetPoint);
				if ((base.transform.position - vector2).sqrMagnitude > peepAttackMaxDistance * peepAttackMaxDistance)
				{
					vector2 = base.transform.position + ToTargetDir() * peepAttackMaxDistance;
				}
				Elite7_Trap component2 = Object.Instantiate(pfb_Trap, position, Quaternion.identity, base.transform.parent).GetComponent<Elite7_Trap>();
				component2.Initialize(myPpt, vector2, peepAttackTrapUpSpeed, peepAttackTrapGravity, peepAttackTrapRadius, midwayChangeLayer: true);
				traps.Add(component2);
			}
			break;
		case "PeepAttackFinish":
			state = MonsterState.PeepAttackIdle;
			break;
		case "JumpBoundaryOut":
		{
			jumpBoundarySystem.Play();
			SEMgr.Inst.elite7Jump.PlaySE();
			base.Anima.SetTrigger("JumpBoundaryOut");
			UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
			componentData3.CanTouch = true;
			SetComponentData(componentData3);
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			base.transform.position += go_RTMove.transform.up * jumpBoundaryOffset + new Vector3(0f, 0f - jumpBoundaryHeight, 0f - jumpBoundaryHeight);
			SyncDotsPosition();
			shadow.ShadowGO.SetActive(value: true);
			GetNearestTargetPlayerFirst();
			float num = GeneralTool.CannonLandTime(jumpBoundaryUpForce, jumpBoundaryHeight, jumpBoundaryGravity);
			if (base.HaveTarget)
			{
				jumpFlyingSpeed = ToTargetDir() * Tool2D.IgnoreZDistance(base.transform.position, base.TargetPoint) / num;
			}
			else
			{
				RoomConfig currentRoomCfg = LevelMgr.Inst.CurrentRoomCfg;
				float x = Random.Range((float)(-currentRoomCfg.theme6Width) / 2f + jumpBoundaryNoTargetRetractDistance, (float)currentRoomCfg.theme6Width / 2f - jumpBoundaryNoTargetRetractDistance);
				float y = Random.Range((float)(-currentRoomCfg.theme6Height) / 2f + jumpBoundaryNoTargetRetractDistance, (float)currentRoomCfg.theme6Height / 2f - jumpBoundaryNoTargetRetractDistance);
				Vector3 vector = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + new Vector3(x, y, 0f);
				jumpFlyingSpeed = ToPointDir(vector) * Tool2D.IgnoreZDistance(base.transform.position, vector) / num;
			}
			JumpStart_Dots(jumpBoundaryUpForce, jumpBoundaryGravity);
			state = MonsterState.Jumping;
			break;
		}
		case "JumpBoundaryOnGroundFinish":
		{
			myPpt.CC_Self.radius = jumpBoundaryOnGroundCCRadius;
			PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
			Unity.Physics.CapsuleCollider* colliderPtr2 = (Unity.Physics.CapsuleCollider*)componentData2.ColliderPtr;
			CapsuleGeometry geometry2 = colliderPtr2->Geometry;
			geometry2.Radius = jumpBoundaryOnGroundCCRadius;
			colliderPtr2->Geometry = geometry2;
			SetComponentData(componentData2);
			break;
		}
		case "RecoveryCCRadius":
		{
			myPpt.CC_Self.radius = originalCCRadius;
			PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
			Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)componentData.ColliderPtr;
			CapsuleGeometry geometry = colliderPtr->Geometry;
			geometry.Radius = originalCCRadius;
			colliderPtr->Geometry = geometry;
			SetComponentData(componentData);
			myPpt.correctType = LayerCorrectType.Coordinate;
			break;
		}
		case "JumpBoundaryEnterGroundFinish":
			RandomPeep();
			break;
		case "JumpGroundSpeedLocked":
			jumpSpeedLocked = true;
			if (base.HaveTarget)
			{
				jumpedDir = ToTargetDir();
			}
			break;
		case "JumpGroundSpell":
		{
			for (int i = 0; i < jumpGroundTrapCount; i++)
			{
				Elite7_Trap component = Object.Instantiate(pfb_Trap, base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<Elite7_Trap>();
				component.Initialize(myPpt, base.transform.position + Tool2D.GetDir() * Random.Range(0f, jumpGroundTrapLandRadius), jumpGroundTrapUpSpeed.RandomResult(), jumpGroundTrapGravity.RandomResult(), jumpGroundTrapRadius, midwayChangeLayer: false);
				traps.Add(component);
				if (traps.Count > trapMaxCount)
				{
					traps[0].Disappear();
					traps.RemoveAt(0);
				}
			}
			break;
		}
		case "JumpGroundFinish":
			RandomPeep();
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public void JumpGroundImpact()
	{
		jumpOnGroundParticle.Play();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, jumpBoundaryOnGroundCCRadius, GameConst.Filter_Friendly, results);
		TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
		info.damage = jumpOnGroundDamage;
		info.ignorePlayerInvincibleFrame = true;
		info.teammateTakeDamageRatio = 2f;
		for (int i = 0; i < results.Count; i++)
		{
			info.knockbackForce = Tool2D.IgnoreZPoint(results[i].point - base.transform.position).normalized * jumpOnGroundImpact;
			UnitDotsSyncSystem.AddTakeDamageRequest(results[i].entity, info);
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		for (int i = 0; i < traps.Count; i++)
		{
			traps[i].Disappear();
		}
		Object.Destroy(go_RT);
		Object.Destroy(go_Performance);
	}

	protected override void BossDeadStay()
	{
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		base.Anima.enabled = false;
		anima_RT.enabled = false;
		SR_Face.sprite = Sprite_Dead;
		SR_FaceRT.sprite = Sprite_Dead;
	}
}
