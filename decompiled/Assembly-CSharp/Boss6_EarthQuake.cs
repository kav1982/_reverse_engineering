using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss6_EarthQuake : MonoBehaviour
{
	[Header("数值")]
	public float speed;

	public VariableInt maxSplitTime;

	public VariableFloat splitDistanceRange;

	public VariableFloat splitAngle;

	public VariableFloat redirectDistanceRange;

	public VariableFloat redirectAngleRange;

	private float towardsRight;

	public float splitChance;

	public float angleConstraintRange;

	public int remainSplitTime;

	public float attackDistanceInterval;

	public float delayTime;

	public float damage;

	public float knockback;

	public float damageRange;

	private Vector3 originDir;

	private Vector3 originPoint;

	private Vector3 nowDir;

	private bool isOriginal;

	[Header("表现")]
	public Boss6_DirtGenerator dirt;

	public ShockParam shock;

	[Header("和谐")]
	public ParticleSystemRenderer dirtRenderer;

	public Material mat_DirtH;

	private bool isRecycle;

	private float beforeRecycleTimer;

	[Header("伤害记录点")]
	public List<Vector3> delayAttackPoints = new List<Vector3>();

	public List<float> delayAttackTime = new List<float>();

	public List<Vector3> delayEffectPoints = new List<Vector3>();

	public List<float> delayEffectTime = new List<float>();

	private float distanceCounter;

	private float splitDistanceCounter;

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void Initialize(bool isOriginal, Vector3 originDir, Vector3 nowDir, int remainSplitTime, float distanceCounter = 0f)
	{
		if (GameMgr.IsChAge14_Static)
		{
			Object.Destroy(dirtRenderer.material);
			dirtRenderer.material = mat_DirtH;
		}
		isRecycle = false;
		delayAttackPoints.Clear();
		delayAttackTime.Clear();
		beforeRecycleTimer = 0f;
		if (isOriginal)
		{
			this.remainSplitTime = maxSplitTime.RandomResult();
			this.distanceCounter = 0f;
		}
		else
		{
			this.distanceCounter = distanceCounter;
			this.remainSplitTime = remainSplitTime;
		}
		splitDistanceRange.RandomResult();
		this.originDir = originDir;
		this.nowDir = nowDir;
		towardsRight = ((!GeneralTool.ChanceResult(0.5f)) ? 1 : (-1));
		nowDir = Tool2D.GetDir(originDir, towardsRight * redirectAngleRange.RandomResult());
		redirectDistanceRange.RandomResult();
	}

	private void Damage(Vector3 damagePoint)
	{
		SEMgr.Inst.monster35Spike.PlaySE(SEPlayMode.Replay, 3, 0.2f);
		UnitDotsSyncSystem.GetCollidersInRange(damagePoint, damageRange, GameConst.Filter_MonsterAoe, results);
		for (int i = 0; i < results.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = results[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss6.Inst.myPpt.myEntity);
					info.damage = damage;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, damagePoint) * knockback;
					info.teammateTakeDamageRatio = 4f;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", distanceHitResult.point, 3f);
				}
				break;
			}
		}
	}

	private void Update()
	{
		for (int num = delayAttackTime.Count - 1; num >= 0; num--)
		{
			delayAttackTime[num] -= Time.deltaTime;
			if (delayAttackTime[num] < 0f)
			{
				Damage(delayAttackPoints[num]);
				delayAttackTime.RemoveAt(num);
				delayAttackPoints.RemoveAt(num);
			}
		}
		for (int num2 = delayEffectTime.Count - 1; num2 >= 0; num2--)
		{
			delayEffectTime[num2] -= Time.deltaTime;
			if (delayEffectTime[num2] < 0f)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SingleQuake" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position).GetComponent<Boss6_SingleQuake>().Initialize();
				delayEffectTime.RemoveAt(num2);
				delayEffectPoints.RemoveAt(num2);
			}
		}
		if (isRecycle)
		{
			beforeRecycleTimer += Time.deltaTime;
			if (beforeRecycleTimer > 2f)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			return;
		}
		CamController.Inst.SetShock(shock);
		base.transform.position += Time.deltaTime * speed * nowDir;
		distanceCounter += Time.deltaTime * speed;
		splitDistanceCounter += Time.deltaTime * speed;
		if (distanceCounter > attackDistanceInterval)
		{
			distanceCounter -= attackDistanceInterval;
			Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(base.transform.position);
			if ((base.transform.position - navMeshPointIngoreZ).sqrMagnitude < 0.25f)
			{
				float num3 = (GameMgr.IsMobile_Static ? 0.2f : 0f);
				delayEffectPoints.Add(navMeshPointIngoreZ);
				delayEffectTime.Add(num3);
				delayAttackPoints.Add(navMeshPointIngoreZ);
				delayAttackTime.Add(delayTime + num3);
			}
			else
			{
				isRecycle = true;
			}
		}
		if (splitDistanceCounter > redirectDistanceRange.result)
		{
			splitDistanceCounter -= redirectDistanceRange.result;
			redirectDistanceRange.RandomResult();
			towardsRight = 0f - towardsRight;
			nowDir = Tool2D.GetDir(originDir, towardsRight * redirectAngleRange.RandomResult());
		}
	}
}
