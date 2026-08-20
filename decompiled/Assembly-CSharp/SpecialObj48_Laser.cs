using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj48_Laser : MonoBehaviour
{
	public Monster9Laser monster9Laser;

	public int laserDamage;

	public float laserRadius;

	public float laserLength;

	public float aimTime;

	public float damageInverval;

	private List<Entity> attackedEntities = new List<Entity>();

	private List<float> attackedTimer = new List<float>();

	private Vector3 startPoint;

	private Vector3 dir;

	private float existTimer;

	private float laserCheckIntervalTimer;

	public float LaserCheckInterval;

	private bool turnOff;

	public void Initialize(Vector3 startPoint, Vector3 dir)
	{
		SetLaser(startPoint, dir);
		this.startPoint = startPoint;
		this.dir = dir;
		existTimer = 0f;
		laserCheckIntervalTimer = LaserCheckInterval;
		attackedEntities.Clear();
		attackedTimer.Clear();
		turnOff = false;
	}

	public void SetLaser(Vector3 startPoint, Vector3 dir)
	{
		this.startPoint = startPoint;
		Vector3 vector = Tool2D.IgnoreZPoint(dir);
		this.dir = ((vector.sqrMagnitude > 0.0001f) ? vector.normalized : Vector3.right);
		base.transform.SetPositionAndRotation(startPoint, Quaternion.identity);
	}

	private void Update()
	{
		for (int num = attackedEntities.Count - 1; num >= 0; num--)
		{
			attackedTimer[num] -= Time.deltaTime;
			if (attackedTimer[num] < 0f)
			{
				attackedTimer.RemoveAt(num);
				attackedEntities.RemoveAt(num);
			}
		}
		existTimer += Time.deltaTime;
		if (!turnOff)
		{
			Vector3 endPoint = GetEndPoint();
			if (existTimer < aimTime)
			{
				monster9Laser.SetWarning(startPoint, endPoint);
				return;
			}
			monster9Laser.SetLaser(startPoint, endPoint);
			DamageCheck(Vector3.Distance(startPoint, endPoint));
		}
	}

	private Vector3 GetEndPoint()
	{
		if (UnitDotsSyncSystem.Raycast(startPoint, dir, laserLength, GameConst.Filter_Wall, out var result))
		{
			return result.point;
		}
		return startPoint + dir * laserLength;
	}

	private void DamageCheck(float damageLength)
	{
		if (laserCheckIntervalTimer < LaserCheckInterval)
		{
			laserCheckIntervalTimer += Time.deltaTime;
			return;
		}
		laserCheckIntervalTimer = 0f;
		CollisionFilter filter_Laser = GameConst.Filter_Laser;
		filter_Laser.CollidesWith |= 256u;
		UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(startPoint, dir, laserRadius, damageLength, filter_Laser);
		for (int i = 0; i < array.Length; i++)
		{
			UnitDotsSyncSystem.RayCastHitResult rayCastHitResult = array[i];
			switch (UnitDotsSyncSystem.GetLayer(rayCastHitResult.entity))
			{
			case 8388608u:
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(rayCastHitResult.entity, laserDamage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
			{
				if (!attackedEntities.Contains(rayCastHitResult.entity) && UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(rayCastHitResult.entity, out var _))
				{
					attackedEntities.Add(rayCastHitResult.entity);
					attackedTimer.Add(damageInverval);
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					info.isTrapDamage = true;
					info.damage = laserDamage;
					info.damage *= GameConstManaged.endlessMonsterDamageRatio;
					UnitDotsSyncSystem.AddTakeDamageRequest(rayCastHitResult.entity, info);
					CreateHitEffect(rayCastHitResult.point);
				}
				break;
			}
			}
		}
	}

	private void CreateHitEffect(Vector3 point)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_Hit", point, 3f);
	}

	public void RecycleSelf()
	{
		monster9Laser.Stop();
		turnOff = true;
		attackedEntities.Clear();
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, 1f);
	}
}
