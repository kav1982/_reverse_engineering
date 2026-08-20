using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss50HiveMissile : MonoBehaviour
{
	[Header("表现")]
	public VariableFloat pointOffset;

	public VariableFloat point1Height;

	public VariableFloat point2Height;

	public VariableFloat startDirRange;

	public VariableFloat startDistanceRange;

	public Transform tsf_Layer;

	public SpriteRenderer sr_BulletHead;

	public Shadow shadow;

	public ParticleSystem trailParticle;

	[Header("伤害")]
	public float damage;

	public float knockBack;

	public float damageRadius;

	public ShockParam shockParam;

	private Vector3 startPoint;

	private Vector3 middlePoint1;

	private Vector3 middlePoint2;

	private Vector3 endPoint;

	private float lifeTime;

	private float existTime;

	private bool exploded;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void Initialize(float lifeTime, Vector3 hiveDir, Vector3 endPoint)
	{
		exploded = false;
		existTime = 0f;
		this.lifeTime = lifeTime;
		sr_BulletHead.enabled = true;
		startPoint = base.transform.position;
		middlePoint1 = base.transform.position + Tool2D.GetDir(hiveDir, startDirRange.RandomResult()) * startDistanceRange.RandomResult();
		middlePoint2 = (endPoint + middlePoint1) / 2f;
		this.endPoint = endPoint;
		middlePoint1 = Tool2D.IgnoreZPoint(middlePoint1) + Tool2D.GetDir() * pointOffset.RandomResult() + Vector3.back * point1Height.RandomResult();
		middlePoint2 = Tool2D.IgnoreZPoint(middlePoint2) + Tool2D.GetDir() * pointOffset.RandomResult() + Vector3.back * point2Height.RandomResult();
		trailParticle.Play();
		ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle", endPoint).GetComponent<WarningArea>().Initialize(damageRadius, lifeTime);
		shadow.Show();
	}

	private void Start()
	{
	}

	private void Update()
	{
		existTime += Time.deltaTime;
		if (existTime <= lifeTime)
		{
			tsf_Layer.position = Tool2D.GetLayerPoint(base.transform.position);
			base.transform.position = GeneralTool.FreeBezierCurve(existTime / lifeTime, startPoint, middlePoint1, middlePoint2, endPoint);
			Vector3 dir = GeneralTool.FreeBezierCurve((existTime + Time.deltaTime) / lifeTime, startPoint, middlePoint1, middlePoint2, endPoint) - base.transform.position;
			dir.y -= dir.z;
			dir.z = 0f;
			tsf_Layer.eulerAngles = Tool2D.GetEulerAngleByDir(dir) + Vector3.forward * 90f;
		}
		if (existTime > lifeTime && !exploded)
		{
			exploded = true;
			shadow.Hide();
			DealDamage();
			trailParticle.Stop();
			ObjPoolMgr.Inst.RecycleGO(base.gameObject, 2f);
			sr_BulletHead.enabled = false;
		}
	}

	private void DealDamage()
	{
		CamController.Inst.SetShock(shockParam);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss50Explosion", base.transform.position, Quaternion.identity, Vector3.one * 1f, 3f);
		SEMgr.Inst.monster34Explosion.PlaySE();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, damageRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			Entity entity = distanceHits[i].entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss50.Inst.myPpt.myEntity);
			switch (layer)
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 2097152u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, base.transform.position) * knockBack;
				info.damage = damage;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
				break;
			case 131072u:
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, base.transform.position) * knockBack;
				info.damage = damage * 9999f;
				info.ignoreFloatText = true;
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
				break;
			}
		}
	}
}
