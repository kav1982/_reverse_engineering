using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Boss5_Bubble : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public int damage;

	public float speed;

	private float nowSpeed;

	public float maxSpeed;

	public float acceleration;

	public Vector3 targetPoint;

	public GameObject shadow;

	public float knockback;

	public ParticleSystem bodyParticle;

	public ParticleSystem explodeParticle;

	public bool dying;

	private float recycleTimer;

	public bool playDeadSE = true;

	public Boss5 master;

	public CapsuleCollider CC;

	public Entity thisEntity { get; set; }

	public void Initialize(Vector3 destination)
	{
		recycleTimer = 0f;
		targetPoint = destination;
		shadow.SetActive(value: true);
		nowSpeed = speed;
		dying = false;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_Friendly, CC);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Die(bool explode = false)
	{
		if (!dying)
		{
			dying = true;
			if (shadow != null)
			{
				shadow.SetActive(value: false);
			}
			if (bodyParticle != null && explode)
			{
				explodeParticle.Play();
			}
			if (explode || playDeadSE)
			{
				SEMgr.Inst.boss5_BubbleHit.PlaySE();
			}
			if (bodyParticle != null)
			{
				bodyParticle.Stop();
			}
		}
	}

	private void Update()
	{
		Vector3 vector = targetPoint - base.transform.position;
		if (!dying)
		{
			if (nowSpeed < maxSpeed)
			{
				nowSpeed += Time.deltaTime * acceleration;
			}
			base.transform.position += vector.normalized * nowSpeed * Time.deltaTime;
		}
		else
		{
			recycleTimer += Time.deltaTime;
			if (recycleTimer > 3f)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
		if (vector.sqrMagnitude < 1f)
		{
			Die();
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!dying)
		{
			uint layer = UnitDotsSyncSystem.GetLayer(other);
			if (layer == 512 || layer == 2097152)
			{
				Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss5.Inst.myPpt.myEntity);
				info.damage = damage;
				info.teammateTakeDamageRatio = 3f;
				info.knockbackForce = (vector - base.transform.position).normalized * knockback;
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
				Die(explode: true);
			}
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
