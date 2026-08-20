using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Boss4_CurseAttack : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public Transform tsf_Shadow;

	public float pattern1Scale;

	public float pattern2Scale;

	public float duration;

	public int damage;

	public float knockback;

	public float moveSpeed;

	[Header("Pattern2")]
	[Range(0f, 1f)]
	public float pattern2Chance;

	[Range(0f, 1f)]
	public float pattern2ChanceStage3;

	public GameObject go_Pattern2Face;

	public float pattern2Duration;

	public float pattern2RotateSpeed;

	public float changeScaleSpeed;

	public CapsuleCollider cc;

	private Boss4 boss4;

	private bool isHit;

	private float durationTimer;

	private bool isPattern2;

	private Entity targetEntity;

	private Vector3 currentDir;

	private float pattern2DurationTimer;

	private float checkTargetIntervalTimer;

	private bool HaveTarget => UnitDotsSyncSystem.EntityIsValid(targetEntity);

	public Entity thisEntity { get; set; }

	public override void OnEnable()
	{
		base.OnEnable();
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_Friendly, cc);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		if (!isHit && boss4.myPpt.AlreadyDead)
		{
			isHit = true;
			boss4.MiniPool.GetGO("Prefabs/EF/EF_Boss4_CurseAttackHit", base.transform.position, 1f);
			boss4.MiniPool.RecycleGO(base.gameObject);
		}
		if (isPattern2)
		{
			if (!HaveTarget)
			{
				checkTargetIntervalTimer += Time.deltaTime;
				if (checkTargetIntervalTimer >= 1f)
				{
					checkTargetIntervalTimer = 0f;
					targetEntity = LevelMgr.Inst.CurrentRoomCtrller.GetNearestFriendlyEntity(base.transform.position);
				}
			}
			if (HaveTarget)
			{
				currentDir = Tool2D.DirMoveTowards(currentDir, Tool2D.IgnoreZV2ToV1Normal(UnitDotsSyncSystem.GetComponentData<LocalTransform>(targetEntity).Position, base.transform.position), pattern2RotateSpeed * Time.deltaTime);
			}
			else
			{
				currentDir = Tool2D.GetDir(currentDir, pattern2RotateSpeed * Time.deltaTime);
			}
			base.transform.Translate(currentDir * moveSpeed * Time.deltaTime, Space.World);
			Vector3 chapter3RepositionChangeValue = LevelMgr.Inst.CurrentRoomCtrller.GetChapter3RepositionChangeValue(base.transform);
			if (chapter3RepositionChangeValue != Vector3.zero)
			{
				base.transform.position += chapter3RepositionChangeValue;
			}
			pattern2DurationTimer += Time.deltaTime;
			if (pattern2DurationTimer >= pattern2Duration)
			{
				isPattern2 = false;
				go_Pattern2Face.SetActive(value: false);
				tsf_Layer.ChangeAllLayer("Default");
			}
		}
		else
		{
			if (base.transform.localScale.x != pattern1Scale)
			{
				float num = Mathf.MoveTowards(base.transform.localScale.x, pattern1Scale, changeScaleSpeed * Time.deltaTime);
				base.transform.localScale = Vector3.one * num;
			}
			base.transform.Translate(currentDir * moveSpeed * Time.deltaTime, Space.World);
			durationTimer += Time.deltaTime;
			if (durationTimer >= duration)
			{
				boss4.MiniPool.RecycleGO(base.gameObject);
			}
		}
	}

	public override void LateUpdate()
	{
		tsf_Shadow.position = Tool2D.IgnoreZPoint(base.transform, 1.05f);
		base.LateUpdate();
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (isHit)
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		if (layer == 512 || layer == 2097152)
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(boss4.myPpt.myEntity);
			info.damage = damage;
			Vector3 v = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
			info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(v, base.transform.position) * knockback;
			info.teammateTakeDamageRatio = 2f;
			info.teammateTakeDamageRatio *= 5f;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			if (isPattern2 && other == PlayerMgr.Inst.PlayerEtt)
			{
				PlayerMgr.Inst.ItemCtrller.CurseAdd(PlayerMgr.Inst.BaData.GetCurseFromPool(ItemDropType.Common), PlayerMgr.Inst.PlayerPoint);
			}
			boss4.MiniPool.GetGO("Prefabs/EF/EF_Boss4_CurseAttackHit", base.transform.position, 1f);
			boss4.MiniPool.RecycleGO(base.gameObject);
			SEMgr.Inst.boss4_CurseHit.PlaySE();
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	public void Initialize(Boss4 boss4, Vector3 dir)
	{
		durationTimer = 0f;
		pattern2DurationTimer = 0f;
		isHit = false;
		this.boss4 = boss4;
		currentDir = dir;
		if (boss4.BossStage == Boss4Stage.Stage3)
		{
			isPattern2 = ((Random.value <= pattern2ChanceStage3) ? true : false);
		}
		else
		{
			isPattern2 = ((Random.value <= pattern2Chance) ? true : false);
		}
		if (isPattern2)
		{
			go_Pattern2Face.SetActive(value: true);
			base.transform.localScale = Vector3.one * pattern2Scale;
			tsf_Layer.ChangeAllLayer("Model");
		}
		else
		{
			go_Pattern2Face.SetActive(value: false);
			base.transform.localScale = Vector3.one * pattern1Scale;
			tsf_Layer.ChangeAllLayer("Default");
		}
	}
}
