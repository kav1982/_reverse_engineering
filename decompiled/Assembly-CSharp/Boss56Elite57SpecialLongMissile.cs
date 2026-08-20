using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss56Elite57SpecialLongMissile : MonoBehaviour
{
	private float flySpeed;

	private float startFallHeight;

	private Vector3 targetPos;

	private float explosionDamage;

	private float explosionRange;

	private float explosionWaitTime;

	private bool isFlyUp;

	private bool isStart;

	private float timer;

	private float currentHeight;

	private float fallStartTime;

	private Vector3 finalSpawnDir;

	private bool useShortEffect;

	public ParticleSystem ParentParticle;

	private void OnEnable()
	{
		isStart = false;
		isFlyUp = true;
		timer = 0f;
		base.transform.right = Tool2D.GetDir(0f);
		finalSpawnDir = default(Vector3);
		useShortEffect = false;
		ParentParticle.Stop();
	}

	private void OnDisable()
	{
		ParentParticle.Stop();
	}

	public void InitialBombData(float flySpeed, float initialHeight, float startFallHeight, float durationPeriod, float explosionRange, float explosionWaitTime, float explosionDamage, float explosionPosDistance, int subMissileCount, Vector3 moveDirection, float bonusWaitTime, Vector3 targetPosShift = default(Vector3), bool useShortEffect = false)
	{
		this.flySpeed = flySpeed;
		this.startFallHeight = startFallHeight;
		this.explosionRange = explosionRange;
		this.explosionWaitTime = explosionWaitTime;
		this.explosionDamage = explosionDamage;
		base.transform.position = base.transform.position.IgnoreZ() + new Vector3(0f, 0f, 0f - initialHeight);
		fallStartTime = durationPeriod;
		isStart = true;
		this.useShortEffect = useShortEffect;
		ParentParticle.Play();
	}

	public void InitialSubBombData(float explosionDamage, float explosionRange, float explosionWaitTime, float bonusWaitTime, float moveSpeed, bool useShortEffect = false)
	{
		base.transform.right = Tool2D.GetDir(180f);
		flySpeed = moveSpeed;
		this.explosionRange = explosionRange;
		this.explosionWaitTime = explosionWaitTime + bonusWaitTime;
		this.explosionDamage = explosionDamage;
		isStart = true;
		isFlyUp = false;
		this.useShortEffect = useShortEffect;
		StartCoroutine(DelayStart(bonusWaitTime));
		base.transform.right = Tool2D.GetDir(180f);
		ParentParticle.Play();
	}

	private IEnumerator DelayStart(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<WarningArea>().Initialize(explosionRange, explosionWaitTime - delayTime);
		SEMgr.Inst.elite56PopWave.PlaySE(SEPlayMode.Replay, 3, 0.1f);
	}

	private void Update()
	{
		if (!isStart)
		{
			return;
		}
		timer += Time.deltaTime;
		if (isFlyUp)
		{
			base.transform.position += new Vector3(0f, 0f, (0f - flySpeed) * Time.deltaTime);
			if (timer >= fallStartTime)
			{
				isFlyUp = false;
				base.transform.right = Tool2D.GetDir(180f);
				base.transform.position = PlayerMgr.Inst.PlayerPoint.IgnoreZ() + new Vector3(0f, 0f, 0f - startFallHeight);
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			return;
		}
		base.transform.position += new Vector3(0f, 0f, flySpeed * Time.deltaTime);
		if (!(base.transform.position.z >= 0f))
		{
			return;
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_VMissileExplosionShort", base.transform.position.IgnoreZ(), Quaternion.identity, Vector3.one * explosionRange / 2f, 5f);
		SEMgr.Inst.elite57VMissileExplosion.PlaySE(SEPlayMode.Replay, 3, 0.16f);
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, explosionRange, GameConst.Filter_MonsterAoe, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, explosionDamage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					info.damage = explosionDamage;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}
}
