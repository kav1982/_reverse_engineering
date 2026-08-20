using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Spell3129VoidExplosion : MonoBehaviour
{
	public class VoidExplosionData
	{
		public float HpToDmgRatio;

		public float ExplosionRange;

		public float InstantKillRatio;

		public bool ConstVoidEffect;

		public VoidExplosionData Copy()
		{
			return new VoidExplosionData
			{
				HpToDmgRatio = HpToDmgRatio,
				ExplosionRange = ExplosionRange,
				InstantKillRatio = InstantKillRatio,
				ConstVoidEffect = ConstVoidEffect
			};
		}
	}

	public struct VoidExplosionData_Dots
	{
		public float HpToDmgRatio;

		public float ExplosionRange;

		public float InstantKillRatio;

		public bool ConstVoidEffect;
	}

	public static readonly List<int> specialVoidExplosionTriggerableUnitIdList = new List<int>
	{
		100201, 100204, 100241, 100244, 100271, 100274, 101121, 101122, 103201, 102001,
		102002, 102003, 102004, 102041, 102042, 102043, 102044, 102071, 102072, 102073,
		102074, 103202, 103241, 103242, 103271, 103272, 500551, 500552, 500553, 500421,
		500425, 300421, 301421, 301422, 301423, 301424
	};

	public float ExplosionTime;

	private float explosionTimer;

	public float MaxDamageDistanceRatio;

	public float MinDamageRatio;

	private float range;

	private float baseDamage;

	private bool explosionEnd;

	private VoidExplosionData explosionData;

	private VoidExplosionData_Dots explosionData_Dots;

	public VariableFloat TrailFlyTime;

	public void DataInitialize(float targetMaxHp, VoidExplosionData data)
	{
		explosionData = null;
		VoidExplosionData voidExplosionData = null;
		voidExplosionData = data;
		if (voidExplosionData == null)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			return;
		}
		explosionEnd = false;
		explosionTimer = 0f;
		explosionData = voidExplosionData;
		explosionData.ConstVoidEffect = false;
		range = voidExplosionData.ExplosionRange;
		baseDamage = Mathf.Ceil(targetMaxHp * voidExplosionData.HpToDmgRatio);
		SEMgr.Inst.spell3129Charge.PlaySE();
	}

	public void DataInitialize_Dots(float targetMaxHp, VoidExplosionData_Dots data)
	{
		explosionData_Dots = data;
		if (data.InstantKillRatio <= 0f)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			return;
		}
		explosionEnd = false;
		explosionTimer = 0f;
		explosionData_Dots.ConstVoidEffect = false;
		range = explosionData_Dots.ExplosionRange;
		baseDamage = Mathf.Ceil(targetMaxHp * explosionData_Dots.HpToDmgRatio);
		SEMgr.Inst.spell3129Charge.PlaySE();
	}

	private void VoidExplosion()
	{
		SEMgr.Inst.spell3129Explosion.PlaySE();
		List<Entity> list = LevelMgr.Inst.CurrentRoomCtrller.GetTargetableInCircle_Dots(base.transform.position, range).ToList();
		if (list.Count <= 0)
		{
			return;
		}
		list = GeneralTool.ListShuffle(list);
		int num = Mathf.Min(5, list.Count);
		float realDamage = baseDamage / (float)num;
		for (int i = 0; i < num && i < list.Count; i++)
		{
			Entity entity = list[i];
			float num2 = TrailFlyTime.RandomResult();
			bool flag = true;
			if (GeneralTool.IsLowFpsOptimizeActive(10f) || Spell2006PullingSoul.ActivedTrailCount >= 60)
			{
				flag = false;
			}
			else if (GeneralTool.IsLowFpsOptimizeActive(30f))
			{
				flag = Random.Range(0f, 1f) <= GameMgr.Inst.GetFps() / 30f;
			}
			if (flag)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31291 + "/" + 31291 + "_Trail", 1f).GetComponent<Spell2006PullingSoul>().InitialLineData(base.transform, null, num2, entity);
			}
			StartCoroutine(DealDamageToTarget(num2 + 0.3f, entity, realDamage, UnitDotsSyncSystem.GetComponentData<LocalTransform>(entity).Position));
		}
	}

	private IEnumerator DealDamageToTarget(float delayTime, Entity targetEntity, float realDamage, Vector3 targetPosition)
	{
		yield return new WaitForSeconds(delayTime);
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31291 + "/" + 31291 + "_Hit", 0.8f).transform.position = Tool2D.IgnoreZPoint(targetPosition);
		if (UnitDotsSyncSystem.EntityIsValid(targetEntity))
		{
			UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(targetEntity);
			componentData.SetVoid(explosionData_Dots);
			UnitDotsSyncSystem.SetComponentData(componentData, targetEntity);
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
			info.damage = realDamage;
			UnitDotsSyncSystem.AddTakeDamageRequest(targetEntity, info);
		}
	}

	private void Update()
	{
		explosionTimer += Time.deltaTime;
		if (explosionTimer >= ExplosionTime && !explosionEnd)
		{
			VoidExplosion();
			explosionEnd = true;
		}
	}
}
