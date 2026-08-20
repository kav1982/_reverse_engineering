using System;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster20 : UnitBase, IRoomObjExtraData
{
	private enum UnitState
	{
		BornIdle,
		Walk
	}

	[Space(50f)]
	public VariableFloat walkRadius;

	public MeshRenderer mr;

	public Sprite sprite_Head;

	public Sprite sprite_Body;

	public Sprite sprite_HeadVariation;

	public Sprite sprite_BodyVariation;

	public float bodyInterval;

	public static float bornEffectInterval = 0.3f;

	public static float bornEffectIntervalLarge = 0.4f;

	public int bodyCount;

	public static float bornEffectCount = 4f;

	public static float bornEffectCountLarge = 4f;

	public float bodyMoveSpeedRatio;

	public int invisibleID;

	public float closeLerp;

	[Range(0f, 1f)]
	public float variationChance;

	[Header("Leg")]
	public GameObject pfb_Leg;

	public Transform tsf_Motion;

	[Header("Spell")]
	public int spellCountEasy;

	public int spellCount;

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	[Header("Pattern2 floating")]
	public AIPattern pattern;

	public float floatingHeight;

	public float floatingAmplitude;

	public float floatingFrequency;

	public float nowHeight;

	public float nowPhase;

	public float heightLerp;

	public float deltaPhase;

	public GameObject pfb_WingL;

	public GameObject pfb_WingR;

	[Header("分裂蜈蚣加速buff")]
	public float speedBuffTime;

	public float speedBuffRatio;

	private float nowSpeedBuff;

	[HideInInspector]
	public float speedBuffTimer;

	private Monster20_Leg leftLeg;

	private Monster20_Leg rightLeg;

	private UnitState state;

	[HideInInspector]
	public Monster20 front;

	[HideInInspector]
	public Monster20_Invisible invisiblePpt;

	private bool isVariation;

	private float bodyCreateDirect;

	private int finalBodyCount;

	private SpellSpawnParams ssp;

	public Vector3 MoveDir
	{
		get
		{
			if (front == null)
			{
				return base.CurrentMotion.normalized;
			}
			return ToPointDir(front.transform);
		}
	}

	public bool HaveFront
	{
		get
		{
			if (front != null && front.gameObject.activeSelf)
			{
				return true;
			}
			return false;
		}
	}

	public override void SingleInitialCallback()
	{
		if (AIPattern.Pattern2 == pattern)
		{
			leftLeg = UnityEngine.Object.Instantiate(pfb_WingL, base.transform).GetComponent<Monster20_Leg>();
			leftLeg.SingleInitial(this, leftLeg: true);
			rightLeg = UnityEngine.Object.Instantiate(pfb_WingR, base.transform).GetComponent<Monster20_Leg>();
			rightLeg.SingleInitial(this, leftLeg: false);
		}
		else
		{
			leftLeg = UnityEngine.Object.Instantiate(pfb_Leg, base.transform).GetComponent<Monster20_Leg>();
			leftLeg.SingleInitial(this, leftLeg: true);
			rightLeg = UnityEngine.Object.Instantiate(pfb_Leg, base.transform).GetComponent<Monster20_Leg>();
			rightLeg.SingleInitial(this, leftLeg: false);
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public unsafe override void EveryInitialCallback()
	{
		nowPhase = 0f;
		state = UnitState.BornIdle;
		front = null;
		invisiblePpt = null;
		isVariation = false;
		bodyCreateDirect = 0f;
		SetExtraData(bodyCount, 0f, 0f);
		leftLeg.EveryInitial();
		rightLeg.EveryInitial();
		if (pattern == AIPattern.Pattern2)
		{
			leftLeg.state = Monster20_Leg.LegState.Floating;
			rightLeg.state = Monster20_Leg.LegState.Floating;
			navAreaMask = 32;
		}
		speedBuffTimer = 0f;
		PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
		CollisionFilter collisionFilter = componentData.ColliderPtr->GetCollisionFilter();
		if (pattern == AIPattern.Pattern1)
		{
			collisionFilter.BelongsTo = 2048u;
			collisionFilter.CollidesWith = DTool.GetCollidesWith(2048u);
		}
		else
		{
			collisionFilter.BelongsTo = 8192u;
			collisionFilter.CollidesWith = DTool.GetCollidesWith(8192u);
		}
		collisionFilter.GroupIndex = -102001;
		componentData.ColliderPtr->SetCollisionFilter(collisionFilter);
	}

	public override void Frame1InitialCallback()
	{
		isVariation = UnityEngine.Random.value <= variationChance * (GameMgr.IsMobile_Static ? 0.5f : 1f);
		if (front == null)
		{
			invisiblePpt = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + invisibleID).GetComponent<Monster20_Invisible>();
			invisiblePpt.BodyRegister(myPpt);
			Monster20 monster = this;
			Vector3 dir = Tool2D.GetDir(bodyCreateDirect);
			if (LevelMgr.Inst.CurrentRoomCfg.isFlipped)
			{
				dir.x = 0f - dir.x;
			}
			for (int i = 0; i < finalBodyCount; i++)
			{
				Monster20 component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + myPpt.unitCfg.id, base.transform.position + dir * bodyInterval * (i + 1)).GetComponent<Monster20>();
				component.InitialBody(monster, invisiblePpt);
				monster = component;
				invisiblePpt.BodyRegister(component.GetComponent<UnitProperty>());
			}
			if (isVariation)
			{
				SetMR(sprite_HeadVariation);
			}
			else
			{
				SetMR(sprite_Head);
			}
			if (pattern == AIPattern.Pattern2)
			{
				tsf_Motion.localPosition = new Vector3(0f, floatingHeight, 0f);
			}
		}
		else
		{
			if (isVariation)
			{
				SetMR(sprite_BodyVariation);
			}
			else
			{
				SetMR(sprite_Body);
			}
			state = UnitState.Walk;
		}
		leftLeg.Frame1Initail();
		rightLeg.Frame1Initail();
	}

	private void SetMR(Sprite sprite)
	{
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite.texture);
		mr.transform.localScale = new Vector3((float)sprite.texture.width / sprite.pixelsPerUnit, (float)sprite.texture.height / sprite.pixelsPerUnit, 1f);
	}

	public void SplitHeadReset()
	{
		front = null;
		if (isVariation)
		{
			SetMR(sprite_HeadVariation);
		}
		else
		{
			SetMR(sprite_Head);
		}
		GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, walkRadius, -MoveDir, 90f));
		speedBuffTimer = speedBuffTime;
	}

	public override void Update()
	{
		if (HaveFront && (front.transform.position - base.transform.position).sqrMagnitude > bodyInterval * bodyInterval)
		{
			Vector3 vector = Vector3.Lerp(base.transform.position, front.transform.position + (-front.transform.position + base.transform.position).normalized * bodyInterval, closeLerp);
			if ((base.transform.position - front.transform.position).sqrMagnitude < bodyInterval * bodyInterval)
			{
				base.transform.position = Tool2D.IgnoreZPoint(vector);
			}
			else if ((base.transform.position - vector).sqrMagnitude > Mathf.Pow(base.MoveSpeed * bodyMoveSpeedRatio * Time.deltaTime, 2f))
			{
				base.transform.position += (front.transform.position - base.transform.position).normalized * base.MoveSpeed * bodyMoveSpeedRatio * Time.deltaTime;
			}
			else
			{
				base.transform.position = vector;
			}
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			SetComponentData(componentData);
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (speedBuffTimer > 0f)
		{
			speedBuffTimer -= Time.deltaTime;
			nowSpeedBuff = Mathf.Lerp(1f, speedBuffRatio, speedBuffTimer / speedBuffTime);
		}
		else
		{
			nowSpeedBuff = 1f;
		}
		if (front != null && !front.gameObject.activeSelf)
		{
			do
			{
				Monster20 monster = front;
				front = front.front;
				if (front == null)
				{
					if (isVariation)
					{
						SetMR(sprite_HeadVariation);
					}
					else
					{
						SetMR(sprite_Head);
					}
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, walkRadius));
					speedBuffTimer = monster.speedBuffTimer;
					break;
				}
			}
			while (!front.gameObject.activeSelf);
		}
		switch (state)
		{
		case UnitState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = UnitState.Walk;
				if (!HaveFront)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, walkRadius));
				}
			}
			break;
		case UnitState.Walk:
			if (!HaveFront)
			{
				if (navInfo.allCornerArrived)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, walkRadius));
				}
				else
				{
					SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * nowSpeedBuff);
					CheckNavInfo();
				}
			}
			if (pattern != AIPattern.Pattern2)
			{
				break;
			}
			if (!HaveFront)
			{
				nowPhase += Time.deltaTime * 2f * MathF.PI * floatingFrequency;
				if (nowPhase > MathF.PI * 2f)
				{
					nowPhase -= MathF.PI * 2f;
				}
			}
			else
			{
				nowPhase = front.nowPhase + deltaPhase * 2f * MathF.PI;
				if (nowPhase > MathF.PI * 2f)
				{
					nowPhase -= MathF.PI * 2f;
				}
			}
			nowHeight = floatingHeight + Mathf.Sin(nowPhase) * floatingAmplitude;
			tsf_Motion.localPosition = new Vector3(0f, nowHeight, 0f);
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public void InitialBody(Monster20 front, Monster20_Invisible invisiblePpt)
	{
		this.front = front;
		this.invisiblePpt = invisiblePpt;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (invisiblePpt == null)
		{
			Frame1Initial();
			invisiblePpt.BodyUnregister(myPpt, ref info);
		}
		else
		{
			invisiblePpt.BodyUnregister(myPpt, ref info);
		}
		if (!isVariation)
		{
			return;
		}
		float num = UnityEngine.Random.Range(0, 360);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		if (pattern == AIPattern.Pattern1 && DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Easy)
		{
			for (int i = 0; i < spellCountEasy; i++)
			{
				sSPModifier.Direction = Tool2D.GetDir(num + (float)(360 / spellCountEasy * i));
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
		}
		else
		{
			for (int j = 0; j < spellCount; j++)
			{
				sSPModifier.Direction = Tool2D.GetDir(num + (float)(360 / spellCount * j));
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
		}
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 >= 1f)
		{
			finalBodyCount = (int)data1;
		}
		bodyCreateDirect = data2;
	}
}
