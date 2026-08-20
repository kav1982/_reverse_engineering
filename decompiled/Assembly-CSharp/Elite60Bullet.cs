using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Elite60Bullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Header("表现与碰撞")]
	public Transform rotateRoot;

	public Transform laserRoot;

	public LineRenderer laserLineRenderer;

	public LineRenderer shadowLineRenderer;

	public UnityEngine.BoxCollider thisCollider;

	public Transform particlesTransform;

	public ParticleSystem particle1;

	public ParticleSystem particle2;

	public float particleNumber;

	public ParticleSystem chargeEffect1;

	public ParticleSystem chargeEffect2;

	[Header("基础参数")]
	public float lifeTime;

	public float chargeTime;

	public float speed;

	public float damage;

	public float knockBack;

	public bool isPenetrate;

	public float rotateSpeed;

	public float changeScaleSpeed;

	public float MaxScale;

	public Vector3 offset;

	[Header("伤害检测")]
	public float damageCheckInterval = 0.05f;

	private Entity owner;

	private Vector3 moveDir;

	private float existTimer;

	private bool recycled;

	private bool hit;

	private float laserWidth;

	private bool isShoot;

	private float damageCheckTimer;

	private readonly List<UnitDotsSyncSystem.DistanceHitResult> damageCheckResults = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private readonly HashSet<Entity> damageCheckedEntities = new HashSet<Entity>();

	public Entity thisEntity { get; set; }

	public void Initialize(Vector3 pointA, Vector3 pointB, Vector3 shootDir, Entity owner, float width)
	{
		this.owner = owner;
		moveDir = shootDir.normalized;
		existTimer = 0f;
		recycled = false;
		hit = false;
		laserWidth = width;
		isShoot = false;
		damageCheckTimer = 0f;
		base.transform.localScale = Vector3.one;
		Vector3 position = base.transform.position;
		Vector3 vector = (pointA + pointB) * 0.5f;
		Vector3 normalized = (pointB - pointA).normalized;
		float x = Vector3.Distance(pointA, pointB);
		base.transform.position = position;
		rotateRoot.transform.position = position;
		rotateRoot.transform.right = normalized;
		laserRoot.transform.localPosition = offset;
		laserRoot.transform.right = normalized;
		particlesTransform.localPosition = laserRoot.InverseTransformPoint(vector + offset);
		float radius = Vector3.Distance(pointA, pointB) * 0.125f;
		ParticleSystem.ShapeModule shape = particle1.shape;
		shape.radius = radius;
		ParticleSystem.ShapeModule shape2 = particle2.shape;
		shape2.radius = radius;
		ParticleSystem.EmissionModule emission = particle1.emission;
		emission.rateOverTime = particleNumber;
		ParticleSystem.EmissionModule emission2 = particle2.emission;
		emission2.rateOverTime = particleNumber;
		chargeEffect1.transform.position = pointA + offset;
		chargeEffect2.transform.position = pointB + offset;
		chargeEffect1.gameObject.SetActive(value: true);
		chargeEffect1.Play();
		chargeEffect2.gameObject.SetActive(value: true);
		chargeEffect2.Play();
		if (thisCollider != null)
		{
			Vector3 center = thisCollider.transform.InverseTransformPoint(vector);
			thisCollider.enabled = true;
			thisCollider.center = center;
			thisCollider.size = new Vector3(x, width - 0.2f, 5f);
		}
		if (laserLineRenderer != null)
		{
			laserLineRenderer.useWorldSpace = false;
			laserLineRenderer.startWidth = 0f;
			laserLineRenderer.SetPosition(0, laserLineRenderer.transform.InverseTransformPoint(pointA + offset));
			laserLineRenderer.SetPosition(1, laserLineRenderer.transform.InverseTransformPoint(pointB + offset));
		}
		if (shadowLineRenderer != null)
		{
			shadowLineRenderer.useWorldSpace = false;
			shadowLineRenderer.startWidth = width;
			shadowLineRenderer.SetPosition(0, Vector3.forward + shadowLineRenderer.transform.InverseTransformPoint(pointA));
			shadowLineRenderer.SetPosition(1, Vector3.forward + shadowLineRenderer.transform.InverseTransformPoint(pointB));
		}
		SEMgr.Inst.boss52HDroneCharge.PlaySE();
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
		if (recycled)
		{
			return;
		}
		existTimer += Time.deltaTime;
		base.transform.position += moveDir * (speed * Time.deltaTime);
		if (rotateSpeed != 0f)
		{
			rotateRoot.transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
			laserRoot.transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
		}
		if (isShoot)
		{
			Vector3 vector = (Vector3.right + Vector3.up) * (changeScaleSpeed * Time.deltaTime);
			base.transform.localScale += vector;
			laserRoot.transform.localPosition = offset / base.transform.localScale.y;
			ParticleSystem.EmissionModule emission = particle1.emission;
			emission.rateOverTime = particleNumber * base.transform.localScale.y;
			ParticleSystem.EmissionModule emission2 = particle2.emission;
			emission2.rateOverTime = particleNumber * base.transform.localScale.y;
		}
		if (existTimer <= chargeTime)
		{
			float num = ((chargeTime <= 0f) ? 1f : Mathf.Clamp01(existTimer / chargeTime));
			if (laserLineRenderer != null)
			{
				laserLineRenderer.startWidth = num * (laserWidth + 0.2f);
			}
			if (shadowLineRenderer != null)
			{
				shadowLineRenderer.startWidth = num * (laserWidth + 0.2f);
			}
		}
		if (isShoot)
		{
			damageCheckTimer += Time.deltaTime;
			if (damageCheckTimer >= Mathf.Max(0.01f, damageCheckInterval))
			{
				damageCheckTimer = 0f;
				DamageCheckByShadowLine();
			}
		}
		if (existTimer > lifeTime)
		{
			Recycle();
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	private void DamageCheckByShadowLine()
	{
		if (recycled || shadowLineRenderer == null || shadowLineRenderer.positionCount < 2)
		{
			return;
		}
		Vector3 vector = shadowLineRenderer.transform.TransformPoint(shadowLineRenderer.GetPosition(0));
		Vector3 vector2 = shadowLineRenderer.transform.TransformPoint(shadowLineRenderer.GetPosition(1));
		Vector3 vector3 = vector2 - vector;
		float magnitude = vector3.magnitude;
		if (magnitude <= 0.001f)
		{
			return;
		}
		vector3 /= magnitude;
		float num = Mathf.Max(0.01f, shadowLineRenderer.startWidth);
		damageCheckResults.Clear();
		damageCheckedEntities.Clear();
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2261504u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitDotsSyncSystem.OverlapBox((vector + vector2) * 0.5f, new Vector3(magnitude * 0.5f, num * 0.5f, 1f), Quaternion.Euler(0f, 0f, Mathf.Atan2(vector3.y, vector3.x) * 57.29578f), filter, damageCheckResults);
		foreach (UnitDotsSyncSystem.DistanceHitResult damageCheckResult in damageCheckResults)
		{
			Entity entity = damageCheckResult.entity;
			if (damageCheckedEntities.Add(entity))
			{
				uint layer = UnitDotsSyncSystem.GetLayer(entity);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(owner);
				info.damage = damage;
				info.knockbackForce = moveDir * knockBack;
				if (layer == 131072)
				{
					info.damage = 999999f;
					info.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite60Hit", damageCheckResult.point).SetActive(value: true);
			}
		}
	}

	private void Recycle()
	{
		recycled = true;
		if (thisCollider != null)
		{
			thisCollider.enabled = false;
		}
		laserRoot.transform.localPosition = Vector3.zero;
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public void Rotate(Vector3 target, float rotateTime)
	{
		laserRoot.transform.DOLocalRotate(target, rotateTime, RotateMode.LocalAxisAdd);
		rotateRoot.transform.DOLocalRotate(target, rotateTime, RotateMode.LocalAxisAdd);
		chargeEffect1.Stop();
		chargeEffect1.gameObject.SetActive(value: false);
		chargeEffect2.Stop();
		chargeEffect2.gameObject.SetActive(value: false);
	}

	public void Shoot(float speed, float rotateSpeed, Vector3 shootDir)
	{
		this.speed = speed;
		this.rotateSpeed = rotateSpeed;
		moveDir = shootDir.normalized;
		isShoot = true;
		damageCheckTimer = Mathf.Max(0.01f, damageCheckInterval);
		SEMgr.Inst.boss52HDroneShoot.PlaySE();
	}
}
