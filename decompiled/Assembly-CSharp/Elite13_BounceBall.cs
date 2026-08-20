using Unity.Entities;
using Unity.Physics.Stateful;
using UnityEngine;

public class Elite13_BounceBall : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever, IDotsCollisionReceiver
{
	[Header("表现")]
	public ParticleSystem trailParticle;

	public float trailRecycleTime;

	public GameObject bulletHead;

	public GameObject shadow;

	public float bulletHeight;

	[Header("回收")]
	private float existTimer;

	private float recycleTimer;

	public float lifeTime;

	private bool recycle;

	[Header("伤害判定和反弹")]
	public CapsuleCollider BounceCollider;

	public CapsuleCollider triggerCollider;

	public Rigidbody rigid;

	public int bounceTime;

	public int bounceTimeCounter;

	[Header("数值")]
	public float speed;

	private Vector3 diration;

	public int damage;

	public float knockBack;

	private bool frame1;

	private bool bounced;

	public Entity thisEntity { get; set; }

	public void Frame1Initialize()
	{
		bulletHead.SetActive(value: true);
		shadow.SetActive(value: true);
		trailParticle.Play();
	}

	public void Initialize(Vector3 diration, float speed)
	{
		trailParticle.Stop();
		trailParticle.Clear();
		recycle = false;
		existTimer = 0f;
		frame1 = false;
		this.speed = speed;
		this.diration = diration.normalized;
		rigid.linearVelocity = diration * speed;
		if (GameMgr.IsMobile_Static)
		{
			rigid.linearVelocity *= 0.85f;
		}
		shadow.transform.localScale = Vector3.one;
		bulletHead.transform.localScale = Vector3.one;
		bounceTimeCounter = 0;
		recycleTimer = 0f;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterAoeNoSpell, triggerCollider);
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_Wall, BounceCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		bounced = false;
		bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight));
		trailParticle.transform.position = bulletHead.transform.position;
		shadow.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect);
		if (!frame1)
		{
			frame1 = true;
			Frame1Initialize();
		}
		_ = recycle;
		if (recycle)
		{
			if (trailParticle.isPlaying)
			{
				trailParticle.Stop();
			}
			bulletHead.SetActive(value: false);
			shadow.SetActive(value: false);
			recycleTimer += Time.deltaTime;
			if (recycleTimer > trailRecycleTime)
			{
				Elite13.MiniPool.RecycleGO(base.gameObject);
			}
		}
		existTimer += Time.deltaTime;
		if (existTimer > lifeTime)
		{
			recycle = true;
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!frame1 || recycle)
		{
			return;
		}
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				string text = "EF_Elite13_HitBig";
				if (GameMgr.IsHarmony_Static)
				{
					text += " H";
				}
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite13.Inst.myPpt.myEntity);
				info.damage = damage;
				info.knockbackForce = diration * knockBack;
				info.teammateTakeDamageRatio = 4f;
				if (result.unitCfg.unitType == UnitType.NotAttack)
				{
					info.ignoreFloatText = true;
					info.damage = 99999f;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
				if (result.unitCfg.unitType != UnitType.Brittleness)
				{
					Elite13.MiniPool.GetGO("Prefabs/EF/" + text, base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight), 3f);
					SEMgr.Inst.elite13Hit.PlaySE();
					recycle = true;
				}
			}
			break;
		}
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	void IDotsCollisionReceiver.OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		rigid.linearVelocity = Vector3.Reflect(rigid.linearVelocity, (Vector3)(-collision.GetNormalFrom(thisEntity)));
		if (frame1 && UnitDotsSyncSystem.GetLayer(collision.GetOtherEntity(thisEntity)) == 256 && !bounced)
		{
			bounced = true;
			bounceTimeCounter++;
			if (bounceTimeCounter > bounceTime)
			{
				recycle = true;
			}
			string text = "EF_Elite13_HitBig" + (GameMgr.IsHarmony_Static ? " H" : "");
			Elite13.MiniPool.GetGO("Prefabs/EF/" + text, base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight), 3f);
			SEMgr.Inst.elite13Miss.PlaySE(SEPlayMode.Unique);
		}
	}

	void IDotsCollisionReceiver.OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	void IDotsCollisionReceiver.OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}
}
