using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster313_GroundFire : LayerCorrect
{
	[Header("数值")]
	public float range;

	public bool canBurn;

	public VariableFloat quickBurnTime;

	public VariableFloat burnTime;

	public int burnDamage;

	public float checkInterval;

	public float burnInterval;

	public ParticleSystem burnParticle;

	public ParticleSystem groundParticle;

	private float burnTimer;

	public List<Entity> attackedEntities = new List<Entity>();

	private List<float> attackedEntitiesCD = new List<float>();

	private float existTimer;

	private float existTime;

	private bool isQuick;

	private bool frame1Initialized;

	private List<UnitDotsSyncSystem.DistanceHitResult> targetsInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void Start()
	{
	}

	public void SetDuration(bool isQuick)
	{
		this.isQuick = isQuick;
	}

	public override void OnEnable()
	{
		base.OnEnable();
		canBurn = true;
		existTimer = 0f;
		if (SpecialObj301EndlessMonsterSpawner.Inst.StageFinished)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		else
		{
			SEMgr.Inst.monster313_GroundFire.PlaySE();
		}
		burnTime.RandomResult();
		quickBurnTime.RandomResult();
		frame1Initialized = false;
	}

	private void Update()
	{
		if (!frame1Initialized)
		{
			frame1Initialized = true;
			if (isQuick)
			{
				existTime = quickBurnTime.RandomResult();
			}
			else
			{
				existTime = burnTime.RandomResult();
			}
		}
		for (int num = attackedEntitiesCD.Count - 1; num >= 0; num--)
		{
			attackedEntitiesCD[num] -= Time.deltaTime;
			if (attackedEntitiesCD[num] < 0f)
			{
				attackedEntitiesCD.RemoveAt(num);
				attackedEntities.RemoveAt(num);
			}
		}
		existTimer += Time.deltaTime;
		if (existTimer > 0.5f && canBurn)
		{
			burnTimer += Time.deltaTime;
			if (burnTimer > checkInterval)
			{
				burnTimer = 0f;
				Burn();
			}
			if (existTimer > existTime || SpecialObj301EndlessMonsterSpawner.Inst.StageFinished)
			{
				canBurn = false;
				existTimer = 0f;
				burnParticle.Stop();
				groundParticle.Stop();
			}
		}
		if (!canBurn && existTimer > 3f)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	private void Burn()
	{
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, range, GameConst.Filter_MonsterAoeNoSpell, targetsInRange);
		for (int i = 0; i < targetsInRange.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = targetsInRange[i];
			if (!attackedEntities.Contains(distanceHitResult.entity) && !UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(distanceHitResult.entity).IsFly)
			{
				attackedEntities.Add(distanceHitResult.entity);
				attackedEntitiesCD.Add(burnInterval);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
				info.damage = burnDamage;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(distanceHitResult.entity, info);
				SEMgr.Inst.elite9Burn.PlaySE();
			}
		}
	}
}
