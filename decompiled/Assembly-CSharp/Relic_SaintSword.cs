using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Relic_SaintSword : LayerCorrect
{
	[Space(50f)]
	public Transform tsf_Model;

	public int damageTimePerSecond;

	public float efHeight;

	private MiniObjPool miniPool;

	private RelicConfig relicCfg;

	private float onceDamage;

	private float damageInterval;

	private float damageIntervalTimer;

	private float radius;

	private EntityManager ettMgr;

	private NativeList<Entity> targetEttList = new NativeList<Entity>(Allocator.Persistent);

	private int relicId;

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
		damageIntervalTimer += Time.deltaTime;
		if (!(damageIntervalTimer >= damageInterval))
		{
			return;
		}
		damageIntervalTimer -= damageInterval;
		UnitDotsSyncSystem.GetAttackableEntitiesInRange(base.transform.position, radius, UnitType.Player, containsBrittleness: true, ref targetEttList);
		bool flag = false;
		if (targetEttList.Length > 0)
		{
			using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(AllMixedEtt));
			AllMixedEtt singleton = entityQuery.GetSingleton<AllMixedEtt>();
			TakeDamageInfo_Dots damageInfo = TakeDamageInfo_Dots.NewInfo(PlayerMgr.Inst.PlayerEtt);
			damageInfo.damage = onceDamage * (1f + PlayerMgr.Inst.ExtraDamageRatio);
			damageInfo.extraCriticalChance = PlayerMgr.Inst.ExtraCriticalRatio;
			damageInfo.damageRecordId = relicId;
			foreach (Entity targetEtt2 in targetEttList)
			{
				Entity targetEtt = targetEtt2;
				SpellTools.HitType hitType = UnitDotsSyncSystem.TryAttackEntity(in targetEtt, in damageInfo, ettMgr);
				if (hitType == SpellTools.HitType.Unit || hitType == SpellTools.HitType.Brittleness || hitType == SpellTools.HitType.RollBall || hitType == SpellTools.HitType.Butterfly)
				{
					Entity entity = ettMgr.Instantiate(singleton.map["Relic_SaintSwordHit"]);
					ettMgr.SetComponentData(entity, LocalTransform.FromPosition(ettMgr.GetComponentData<LocalTransform>(targetEtt).Position));
					flag = true;
				}
			}
		}
		if (flag)
		{
			SEMgr.Inst.relic_SaintSwordHit.PlaySE();
		}
	}

	public void Initialize(RelicConfig relicCfg)
	{
		this.relicCfg = relicCfg;
		damageInterval = 1f / (float)damageTimePerSecond;
		onceDamage = relicCfg.float2.value * Mathf.Pow(2f, relicCfg.level - 1) / (float)damageTimePerSecond;
		relicId = this.relicCfg.id;
		UpdateRadius();
		if (miniPool == null)
		{
			miniPool = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Mixed/MiniObjPool")).GetComponent<MiniObjPool>();
		}
	}

	public void UpdateRadius()
	{
		radius = relicCfg.float1.result * (1f + PlayerMgr.Inst.ExtraRadiusOfInfluence(isSpell: false));
		tsf_Model.localScale = Vector3.one * radius * 2f;
	}

	public void DestroySelf()
	{
		if (targetEttList.IsCreated)
		{
			targetEttList.Dispose();
		}
		Object.Destroy(miniPool);
		Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (targetEttList.IsCreated)
		{
			targetEttList.Dispose();
		}
		Object.Destroy(miniPool);
	}
}
