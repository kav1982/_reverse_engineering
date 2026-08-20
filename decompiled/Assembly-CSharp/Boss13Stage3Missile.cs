using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss13Stage3Missile : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public Vector3 moveDir;

	public float startMoveSpeed;

	public float endMoveSpeed;

	public float speedChangeTime;

	private float nowSpeed;

	public float damage;

	public float damageRadius;

	public float knockBack;

	public ShockParam shockParam;

	public Transform rotateRoot;

	public Transform shadowRoot;

	[Header("延迟导弹")]
	public VariableFloat delayExplodeTime;

	public VariableFloat delayExplodeSlowDownTime;

	[Header("爆裂导弹")]
	public bool isDelayExplode;

	public int bulletAmount;

	public float bulletSpeed;

	private float existTime;

	[Header("拖尾")]
	public Transform particleRoot;

	private ParticleSystem bubbleEffect;

	private bool mobileNerfed;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		if (!mobileNerfed && GameMgr.IsMobile_Static && !isDelayExplode)
		{
			mobileNerfed = true;
			bulletAmount--;
		}
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228992u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
		existTime = 0f;
		if (isDelayExplode)
		{
			delayExplodeTime.RandomResult();
			speedChangeTime = delayExplodeSlowDownTime.RandomResult();
		}
		bubbleEffect = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13FollowMissileTrailStage3", particleRoot.position).GetComponent<ParticleSystem>();
		bubbleEffect.Clear();
		bubbleEffect.Play();
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		if (bubbleEffect != null)
		{
			bubbleEffect.transform.position = particleRoot.position;
			bubbleEffect.transform.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngle(Vector3.up, moveDir) - 90f);
		}
		existTime += Time.deltaTime;
		nowSpeed = Mathf.Lerp(startMoveSpeed, endMoveSpeed, existTime / speedChangeTime);
		base.transform.position += moveDir * nowSpeed * Time.deltaTime;
		rotateRoot.transform.localEulerAngles = Vector3.forward * Tool2D.IgnoreZAngleWithSign(Vector3.up, moveDir);
		if (isDelayExplode && existTime > delayExplodeTime.result)
		{
			ExplodeOnce(base.transform.position, createBullet: true);
		}
		shadowRoot.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		switch (layer)
		{
		case 256u:
		case 512u:
		case 32768u:
		case 2097152u:
		{
			CamController.Inst.SetShock(shockParam);
			bool createBullet = false;
			if (layer == 256)
			{
				createBullet = true;
			}
			ExplodeOnce(base.transform.position, createBullet);
			break;
		}
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}

	private void ExplodeOnce(Vector3 explodePoint, bool createBullet)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Explosion", explodePoint, 6f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Trace", explodePoint, 10f);
		CamController.Inst.SetShock(shockParam);
		SEMgr.Inst.boss13SmallExplosion.PlaySE(SEPlayMode.Replay, 3, 0.2f);
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, damageRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			Entity entity = distanceHits[i].entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			switch (layer)
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
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockBack;
				info.damage = damage;
				info.teammateTakeDamageRatio = 4f;
				if (layer == 131072)
				{
					info.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			}
			}
		}
		float num = Random.Range(0f, 360f);
		if (createBullet)
		{
			for (int j = 0; j < bulletAmount; j++)
			{
				Vector3 dir = Tool2D.GetDir(num + (float)(j * 360 / bulletAmount));
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13_Bullet", base.transform.position + dir * 0.6f).GetComponent<Boss13Bullet>().InitializeSimple(dir, bulletSpeed);
			}
		}
		if (bubbleEffect != null && ObjPoolMgr.Inst.gameObject.activeInHierarchy)
		{
			bubbleEffect.Stop();
			ObjPoolMgr.Inst.RecycleGO(bubbleEffect.transform.gameObject, 1f);
			bubbleEffect = null;
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}
}
