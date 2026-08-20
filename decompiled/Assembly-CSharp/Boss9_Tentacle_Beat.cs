using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss9_Tentacle_Beat : MonoBehaviour
{
	public float damage;

	public float TVPackageDamageFactor;

	public Transform startPoint;

	public Transform endPoint;

	public Transform controlPoint1;

	public Transform controlPoint2;

	public int segmentCount = 10;

	public float waveAmplitude = 0.2f;

	public float waveFrequency = 5f;

	public float waveSpeed = 2f;

	public float height = 0.4f;

	public LayerMask attackMask;

	public LineRenderer lineRenderer;

	public LineRenderer shadowLineRenderer;

	[SerializeField]
	private List<Vector3> points;

	[SerializeField]
	private List<Vector3> shadowPoints;

	public bool damageCheck;

	public List<UnitProperty> attackTarget = new List<UnitProperty>();

	public float detectionRadius;

	public float knockBack;

	public Boss9 boss9;

	public string startAnima;

	public Animator anim;

	public ShockParam shockParam;

	public Transform warningPivot;

	public GameObject dirtEffect;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void Start()
	{
		if (ScriptableObjMgr.Inst.testCtrller.isBW)
		{
			damage *= TVPackageDamageFactor;
		}
		lineRenderer.positionCount = segmentCount;
		shadowLineRenderer.positionCount = segmentCount;
		anim.Play(startAnima);
	}

	private void Update()
	{
		if (boss9 != null)
		{
			lineRenderer.material.color = boss9.myPpt.BaseColor;
		}
		for (int i = 0; i < segmentCount; i++)
		{
			float num = (float)i / (float)(segmentCount - 1);
			Vector3 vector = BezierCurve(endPoint.position, controlPoint2.position, num);
			float num2 = Mathf.Lerp(0f, 1f, num);
			float num3 = Mathf.Sin((num + Time.time * waveSpeed) * waveFrequency) * waveAmplitude * num2;
			vector += Vector3.Cross(Tool2D.IgnoreZV2ToV1Normal(controlPoint2, endPoint), Vector3.forward).normalized * num3;
			points[i] = Tool2D.GetLayerPoint(vector - new Vector3(0f, 0f, height), LayerCorrectType.Coordinate);
			shadowPoints[i] = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(vector), LayerCorrectType.Shadow);
		}
		lineRenderer.SetPositions(points.ToArray());
		shadowLineRenderer.SetPositions(shadowPoints.ToArray());
	}

	private Vector3 BezierCurve(Vector3 p0, Vector3 p1, float t)
	{
		float num = 1f - t;
		float num2 = num * num;
		float num3 = num2 * num;
		float num4 = t * t;
		float num5 = num4 * t;
		return num3 * p0 + 3f * num2 * t * p1 + 3f * num * num4 * controlPoint1.position + num5 * startPoint.position;
	}

	public void EnableDamageCheck()
	{
		damageCheck = true;
		attackTarget.Clear();
	}

	public void DisableDamageCheck()
	{
		damageCheck = false;
	}

	public void BulletAttack()
	{
		boss9.BulletAttack(warningPivot.position);
		CamController.Inst.SetShock(shockParam);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss9_BeatDust", warningPivot.position);
		UnitDotsSyncSystem.GetCollidersInRange(warningPivot.position, detectionRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		foreach (UnitDotsSyncSystem.DistanceHitResult distanceHit in distanceHits)
		{
			Entity entity = distanceHit.entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss9.Inst.myPpt.myEntity);
			info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHit.point, warningPivot.position) * knockBack;
			info.damage = damage;
			switch (layer)
			{
			case 512u:
			case 2097152u:
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", distanceHit.point, 1f);
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			case 32768u:
			case 131072u:
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			}
		}
	}

	public void BodyBeatAnim()
	{
		boss9.BodyBeatAnim();
	}

	public void SetInvincible()
	{
		boss9.SetInvincible();
	}

	public void SetUnInvincible()
	{
		boss9.SetUnInvincible();
	}

	public void SetTentacleIdle()
	{
		boss9.isTentalceIdle = true;
	}

	public void BeatStart()
	{
		boss9.isTentalceIdle = false;
		SEMgr.Inst.boss9_Smash.PlaySE();
		SEMgr.Inst.boss9_Bubble.PlaySE();
	}

	public void BeatEnd()
	{
		boss9.generalAttackCDTimer = 0f;
		boss9.canAttack = true;
		boss9.isTentalceIdle = true;
	}

	public void SetWarningLine()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsHarmony_Static ? " Purple" : ""), warningPivot.position).GetComponent<WarningArea>().Initialize(4f, 1.5f);
	}
}
