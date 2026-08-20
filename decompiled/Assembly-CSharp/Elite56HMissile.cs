using Unity.Entities;
using UnityEngine;

public class Elite56HMissile : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public Transform CenterTransform;

	private Vector3 moveDirection;

	private float maxMoveRange;

	private float moveSpeed;

	private float missileDamage;

	private int subMissileCount;

	private float subMissileTotalScatter;

	private float subMissileDamage;

	private float subMissileExplosionRange;

	private float subMissileSpeed;

	private float subMissileMoveRange;

	private bool isInitialized;

	private float travelDistance;

	private Vector3 lastFramePosition;

	private bool isSubMissile;

	private float knockBackForce;

	public CapsuleCollider CC;

	private Entity owner;

	public Transform ShadowBombTransform;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		isInitialized = false;
		owner = Entity.Null;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterAoeNoSpell, CC);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void InitMissile(Vector3 direction, float range, float speed, float missileDamage, int subMissileCount, float subMissileTotalScatter, float subMissileDamage, float subMissileExplosionRange, float subMissileSpeed, float subMissileRange, Entity shooterEntity, float knockBackForce = 8f)
	{
		isInitialized = true;
		travelDistance = 0f;
		lastFramePosition = base.transform.position;
		isSubMissile = false;
		moveDirection = direction;
		maxMoveRange = range;
		moveSpeed = speed;
		this.missileDamage = missileDamage;
		this.subMissileCount = subMissileCount;
		this.subMissileTotalScatter = subMissileTotalScatter;
		this.subMissileDamage = subMissileDamage;
		this.subMissileExplosionRange = subMissileExplosionRange;
		this.subMissileSpeed = subMissileSpeed;
		subMissileMoveRange = subMissileRange;
		CenterTransform.right = moveDirection;
		ShadowBombTransform.right = moveDirection;
		owner = shooterEntity;
		base.transform.localScale = Vector3.one * 1.4f;
	}

	public void InitSubMissile(Vector3 direction, float subMissileDamage, float subMissileExplosionRange, float subMissileSpeed, float subMissileRange, Entity shooterEntity)
	{
		isInitialized = true;
		isSubMissile = true;
		moveDirection = direction;
		lastFramePosition = base.transform.position;
		travelDistance = 0f;
		this.subMissileDamage = subMissileDamage;
		this.subMissileExplosionRange = subMissileExplosionRange;
		this.subMissileSpeed = subMissileSpeed;
		subMissileMoveRange = subMissileRange;
		CenterTransform.right = moveDirection;
		ShadowBombTransform.right = moveDirection;
		owner = shooterEntity;
	}

	private void Update()
	{
		if (!isInitialized)
		{
			return;
		}
		if (isSubMissile)
		{
			base.transform.position += moveDirection * subMissileSpeed * Time.deltaTime;
			travelDistance += Tool2D.IgnoreZDistance(base.transform.position, lastFramePosition);
			lastFramePosition = base.transform.position;
			if (travelDistance >= subMissileMoveRange)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			return;
		}
		base.transform.position += moveDirection * moveSpeed * Time.deltaTime;
		travelDistance += Tool2D.IgnoreZDistance(base.transform.position, lastFramePosition);
		lastFramePosition = base.transform.position;
		if (travelDistance >= maxMoveRange)
		{
			float sectorAngle = subMissileTotalScatter / (float)(subMissileCount - 1);
			ShootSubMissile(sectorAngle);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_BombSplit", base.transform.position).transform.rotation = Quaternion.LookRotation(moveDirection);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	private void ShootSubMissile(float sectorAngle)
	{
		for (int i = 0; i < subMissileCount; i++)
		{
			Vector3 dir = Tool2D.GetDir(moveDirection, (0f - subMissileTotalScatter) / 2f + sectorAngle * (float)i);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_HMissile", base.transform.position).GetComponent<Elite56HMissile>().InitSubMissile(dir, subMissileDamage, subMissileExplosionRange, subMissileSpeed, subMissileMoveRange, owner);
		}
		SEMgr.Inst.elite56MissileSplit.PlaySE();
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		bool flag = false;
		switch (layer)
		{
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				flag = true;
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(owner);
				info.damage = (isSubMissile ? subMissileDamage : missileDamage);
				info.knockbackForce = moveDirection * knockBackForce;
				info.teammateTakeDamageRatio = 4f;
				if (result.unitCfg.unitType == UnitType.NotAttack)
				{
					info.ignoreFloatText = true;
					info.damage = 99999f;
				}
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info);
				if (result.unitCfg.unitType != UnitType.Brittleness)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_MissileHit", base.transform.position + new Vector3(0f, 0f, -0.5f), 3f).transform.right = moveDirection;
					SEMgr.Inst.elite56MissileExplosion.PlaySE();
				}
			}
			break;
		}
		}
		if (flag)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
