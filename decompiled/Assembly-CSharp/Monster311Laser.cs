using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster311Laser : MonoBehaviour
{
	public bool isPattern2;

	public Entity attackEntity;

	public float aimTimer;

	public Monster9Laser monster9Laser;

	public Vector3 targetPosition;

	public Vector3 laseroffset;

	public float laserRadius;

	private float laserCheckIntervalTimer;

	private float laserCheckInterval = 0.1f;

	public int laserDamage;

	public float laserLength = 10f;

	public List<Entity> attackedEntities = new List<Entity>();

	private List<float> attackedTimer = new List<float>();

	public ShockParam shockParam;

	[Header("激光动画")]
	public LineRenderer laser;

	public LineRenderer laserShadow;

	public AnimationCurve attackWidthCurve;

	public AnimationCurve attackTransparentCurve;

	public float laserWidth;

	public float laserExistTime;

	public float aimTime;

	private bool effectPlayed;

	public ParticleSystem shootEffect;

	public ParticleSystem chargeEffect;

	private bool buffed;

	public void SetBuffed(bool buffed)
	{
		this.buffed = buffed;
	}

	private void OnEnable()
	{
		aimTimer = 0f;
		attackedEntities.Clear();
		attackedTimer.Clear();
		effectPlayed = false;
		shootEffect.transform.position = Tool2D.GetLayerPoint(base.transform.position + laseroffset);
		chargeEffect.transform.position = Tool2D.GetLayerPoint(base.transform.position + laseroffset);
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
		Vector3 vector = base.transform.position + laseroffset;
		Vector3 vector2 = Tool2D.IgnoreZV2ToV1Normal(targetPosition, base.transform.position);
		Vector3 point = vector + vector2 * laserLength;
		aimTimer += Time.deltaTime;
		if (aimTimer < aimTime)
		{
			monster9Laser.SetWarning(vector, point);
		}
		else
		{
			float num = aimTimer - aimTime;
			float num2 = laserExistTime - aimTime;
			if (!effectPlayed)
			{
				effectPlayed = true;
				shootEffect.Play();
				if (isPattern2)
				{
					SEMgr.Inst.spell1011Shoot.PlaySE();
					SEMgr.Inst.spell1011Loop.PlayLoopSE(num2 - 0.3f);
				}
				else
				{
					SEMgr.Inst.monster54_Laser.PlaySE();
				}
			}
			if (isPattern2 && num2 - num > 0.3f)
			{
				CamController.Inst.SetShock(shockParam);
			}
			monster9Laser.SetLaser(vector, point);
			laser.widthMultiplier = Mathf.Max(0f, attackWidthCurve.Evaluate(num / num2)) * laserWidth;
			laserShadow.widthMultiplier = Mathf.Max(0f, attackWidthCurve.Evaluate(num / num2)) * laserWidth;
			laser.material.SetFloat("_Transparency", attackTransparentCurve.Evaluate(num / num2));
			laserShadow.material.SetFloat("_Transparency", attackTransparentCurve.Evaluate(num / num2));
			for (int num3 = attackedEntities.Count - 1; num3 >= 0; num3--)
			{
				attackedTimer[num3] -= Time.deltaTime;
				if (attackedTimer[num3] < 0f)
				{
					attackedTimer.RemoveAt(num3);
					attackedEntities.RemoveAt(num3);
				}
			}
			if (num2 - num > 0.3f)
			{
				laserCheckIntervalTimer += Time.deltaTime;
			}
			if (laserCheckIntervalTimer >= laserCheckInterval)
			{
				laserCheckIntervalTimer = 0f;
				UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(vector, vector2, laserRadius, laserLength, GameConst.Filter_Laser);
				for (int i = 0; i < array.Length; i++)
				{
					UnitDotsSyncSystem.RayCastHitResult rayCastHitResult = array[i];
					if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(rayCastHitResult.entity))
					{
						if (attackedEntities.Contains(rayCastHitResult.entity))
						{
							ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_Hit", array[i].point, 3f);
							continue;
						}
						attackedEntities.Add(rayCastHitResult.entity);
						attackedTimer.Add(0.33f);
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_Hit", array[i].point, 3f);
						TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(attackEntity);
						info.damage = laserDamage;
						if (buffed)
						{
							info.damage *= 1f;
						}
						info.teammateTakeDamageRatio = 3f;
						UnitDotsSyncSystem.AddTakeDamageRequest(rayCastHitResult.entity, info);
					}
					else
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_Hit", array[i].point, 3f);
					}
				}
			}
		}
		if (!UnitDotsSyncSystem.EntityIsValid(attackEntity) || aimTimer > laserExistTime)
		{
			monster9Laser.StopImmediately();
			attackedEntities.Clear();
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}
}
