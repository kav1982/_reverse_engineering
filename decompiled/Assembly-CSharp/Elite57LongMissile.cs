using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite57LongMissile : MonoBehaviour
{
	private float flySpeed;

	private float startFallHeight;

	private Vector3 targetPos;

	private float explosionDamage;

	private float explosionRange;

	private float explosionWaitTime;

	private float subMissilePosDistance;

	private float bonusWaitTime;

	private int subMissileShootCount;

	private Vector3 subMissileMoveDir;

	private bool isFlyUp;

	private bool isStart;

	private float timer;

	private float currentHeight;

	private float fallStartTime;

	private Vector3 finalHitPosShift;

	private Vector3 finalSpawnDir;

	private bool useShortEffect;

	private void OnEnable()
	{
		isStart = false;
		isFlyUp = true;
		timer = 0f;
		base.transform.right = Tool2D.GetDir(0f);
		finalHitPosShift = default(Vector3);
		finalSpawnDir = default(Vector3);
		useShortEffect = false;
	}

	public void InitialBombData(float flySpeed, float initialHeight, float startFallHeight, float durationPeriod, float explosionRange, float explosionWaitTime, float explosionDamage, float explosionPosDistance, int subMissileCount, Vector3 moveDirection, float bonusWaitTime, Vector3 targetPosShift = default(Vector3), bool useShortEffect = false)
	{
		this.flySpeed = flySpeed;
		this.startFallHeight = startFallHeight;
		this.explosionRange = explosionRange;
		this.explosionWaitTime = explosionWaitTime;
		this.explosionDamage = explosionDamage;
		subMissilePosDistance = explosionPosDistance;
		base.transform.position = base.transform.position.IgnoreZ() + new Vector3(0f, 0f, 0f - initialHeight);
		subMissileShootCount = subMissileCount;
		fallStartTime = durationPeriod;
		subMissileMoveDir = moveDirection;
		this.bonusWaitTime = bonusWaitTime;
		isStart = true;
		finalHitPosShift = targetPosShift;
		this.useShortEffect = useShortEffect;
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
		this.bonusWaitTime = bonusWaitTime;
		this.useShortEffect = useShortEffect;
		StartCoroutine(DelayStart(bonusWaitTime));
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
			if (!(timer >= fallStartTime))
			{
				return;
			}
			isFlyUp = false;
			base.transform.right = Tool2D.GetDir(180f);
			base.transform.position = PlayerMgr.Inst.PlayerPoint.IgnoreZ() + new Vector3(0f, 0f, 0f - startFallHeight);
			for (int i = 0; i < subMissileShootCount; i++)
			{
				Vector3 vector = ((finalSpawnDir == default(Vector3)) ? subMissileMoveDir.normalized : finalSpawnDir);
				Vector3 vector2 = PlayerMgr.Inst.PlayerPoint.IgnoreZ();
				vector2 += vector * (((float)i - (float)subMissileShootCount / 2f) * subMissilePosDistance);
				vector2 += new Vector3(0f, 0f, (0f - (explosionWaitTime + bonusWaitTime * (float)i)) * flySpeed);
				if (finalHitPosShift != default(Vector3))
				{
					vector2 += finalHitPosShift;
				}
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_LongMissile", vector2).GetComponent<Elite57LongMissile>().InitialSubBombData(explosionDamage, explosionRange, explosionWaitTime, bonusWaitTime * (float)i, flySpeed, useShortEffect);
				if (useShortEffect)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_VShortMarker", vector2.IgnoreZ(), explosionWaitTime + bonusWaitTime * (float)i);
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_VMarker", vector2.IgnoreZ(), explosionWaitTime + bonusWaitTime * (float)i);
				}
			}
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			return;
		}
		base.transform.position += new Vector3(0f, 0f, flySpeed * Time.deltaTime);
		if (!(base.transform.position.z >= 0f))
		{
			return;
		}
		if (useShortEffect)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_VMissileExplosionShort", base.transform.position, Quaternion.identity, Vector3.one * explosionRange / 2f, 5f);
		}
		else
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_VMissileExplosion", base.transform.position, Quaternion.identity, Vector3.one * explosionRange / 2f, 5f);
		}
		SEMgr.Inst.elite57VMissileExplosion.PlaySE(SEPlayMode.Replay, 3, 0.16f);
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, explosionRange, GameConst.Filter_MonsterAoe, list);
		for (int j = 0; j < list.Count; j++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[j];
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
