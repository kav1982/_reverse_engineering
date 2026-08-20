using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Relic_DecelerateShield : LayerCorrect
{
	[Space(50f)]
	public Transform tsf_GroundEffect;

	public float keepDistance;

	public float moveLerp;

	public float decelerateLerp;

	private RelicConfig relicCfg;

	private float radius;

	private EntityManager ettMgr;

	private CollisionFilter collisionFilter;

	private void Start()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		collisionFilter = new CollisionFilter
		{
			BelongsTo = uint.MaxValue,
			CollidesWith = 8388608u,
			GroupIndex = 0
		};
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
		NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(PhysicsWorldSingleton));
		entityQuery.GetSingleton<PhysicsWorldSingleton>().OverlapSphere(base.transform.position, radius, ref outHits, collisionFilter);
		foreach (DistanceHit item in outHits)
		{
			if (ettMgr.HasComponent<SpellConfigComponentData>(item.Entity) && DTool.IsSameCamp(ettMgr.GetComponentData<SpellConfigComponentData>(item.Entity).ShooterType, UnitType.Monster))
			{
				SpellMovementComponentData componentData = ettMgr.GetComponentData<SpellMovementComponentData>(item.Entity);
				componentData.Speed = Mathf.Lerp(componentData.Speed, 0f, decelerateLerp * Time.deltaTime);
				componentData.CurrentFallSpeed = Mathf.Lerp(componentData.CurrentFallSpeed, 0f, decelerateLerp * Time.deltaTime);
				componentData.Gravity = Mathf.Lerp(componentData.Gravity, 0f, decelerateLerp * Time.deltaTime);
				ettMgr.SetComponentData(item.Entity, componentData);
			}
		}
	}

	public void Initialize(RelicConfig relicCfg)
	{
		this.relicCfg = relicCfg;
		UpdateRadius();
	}

	public void UpdateRadius()
	{
		radius = relicCfg.float1.result * (1f + PlayerMgr.Inst.ExtraRadiusOfInfluence(isSpell: false));
		base.transform.localScale = Vector3.one * radius * 2f;
		tsf_GroundEffect.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.GroundEffectLow);
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}
}
