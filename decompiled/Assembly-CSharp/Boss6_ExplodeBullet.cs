using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss6_ExplodeBullet : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Header("分裂机制")]
	public List<float> splitScale = new List<float>();

	public List<int> damageScale = new List<int>();

	public List<float> knockbackScale = new List<float>();

	public CapsuleCollider thisCollider;

	public Transform tsf_BulletRoot;

	public Shadow thisShadow;

	public int remainSplitTimes;

	private float splitDirAngle;

	public SpriteRenderer directionSprite;

	private bool hasSplit;

	[Header("数值")]
	public float beforeSplitTime;

	public VariableFloat bulletLifeTime;

	private int damage;

	private Vector3 direction;

	public float bulletSpeed;

	private bool hasHit;

	private float durationTimer;

	private float beforeRecycleTimer;

	[Header("表现")]
	public ParticleSystem existParticle;

	public ParticleSystem explodeParticle;

	public ParticleSystem directionalParticle;

	public ParticleSystem noSplitParticle;

	public float bulletHeight;

	private bool frame1Initialized;

	public Entity thisEntity { get; set; }

	public void Initialize(Vector3 direction, float speed, int remainSplitTimes)
	{
		this.direction = direction;
		bulletSpeed = speed;
		this.remainSplitTimes = remainSplitTimes;
		bulletLifeTime.RandomResult();
		damage = damageScale[remainSplitTimes];
		tsf_BulletRoot.localScale = Vector3.one * splitScale[remainSplitTimes];
		thisShadow.SetScale(splitScale[remainSplitTimes] * 2f);
		thisShadow.Show();
		thisCollider.radius = splitScale[remainSplitTimes];
		thisCollider.enabled = true;
		directionSprite.enabled = true;
		directionSprite.color = new Color(1f, 1f, 1f, 0f);
		durationTimer = 0f;
		beforeRecycleTimer = 0f;
		hasSplit = false;
		hasHit = false;
		splitDirAngle = Random.Range(0, 360);
		if (remainSplitTimes > 0)
		{
			existParticle.Play();
		}
		else
		{
			noSplitParticle.Play();
		}
		directionalParticle.transform.eulerAngles = new Vector3(0f, 0f, splitDirAngle);
		if (remainSplitTimes == 0)
		{
			directionalParticle.Stop();
		}
		frame1Initialized = false;
	}

	public void OnDisable()
	{
		if (frame1Initialized)
		{
			UnitPhysicsSyncSystem.UnregisterReciever(this);
		}
	}

	public void Split(bool hitPlayer)
	{
		thisShadow.Hide();
		directionSprite.enabled = false;
		existParticle.Stop();
		existParticle.Clear();
		noSplitParticle.Stop();
		noSplitParticle.Clear();
		explodeParticle.Play();
		if (remainSplitTimes > 0 && !hitPlayer)
		{
			SEMgr.Inst.boss6_ExplodeBulletSplit.PlaySE();
			for (int i = 0; i < 3; i++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_ExplodeBullet" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position).GetComponent<Boss6_ExplodeBullet>().Initialize(Tool2D.GetDir(splitDirAngle + (float)(120 * i)), bulletSpeed, remainSplitTimes - 1);
			}
		}
	}

	private void Update()
	{
		if (!frame1Initialized)
		{
			frame1Initialized = true;
			UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterAoeNoSpell, thisCollider);
		}
		tsf_BulletRoot.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight));
		directionSprite.transform.localEulerAngles = new Vector3(0f, 0f, splitDirAngle);
		if (!hasHit)
		{
			durationTimer += Time.deltaTime;
		}
		if (remainSplitTimes > 0)
		{
			directionSprite.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, (durationTimer - beforeSplitTime) / (bulletLifeTime.result - beforeSplitTime)));
		}
		if (durationTimer >= bulletLifeTime.result && !hasSplit)
		{
			thisCollider.enabled = false;
			hasSplit = true;
			if (remainSplitTimes > 0)
			{
				Split(hitPlayer: false);
			}
		}
		if (hasSplit || hasHit)
		{
			if (remainSplitTimes == 0 && !hasHit)
			{
				base.transform.localScale = Vector3.one * (base.transform.localScale.x - 5f * Time.deltaTime);
				thisShadow.SetScale(base.transform.localScale.x);
				if (base.transform.localScale.x <= 0f)
				{
					ObjPoolMgr.Inst.RecycleGO(base.gameObject);
				}
			}
			else
			{
				beforeRecycleTimer += Time.deltaTime;
				if (beforeRecycleTimer > 3f)
				{
					ObjPoolMgr.Inst.RecycleGO(base.gameObject);
				}
			}
		}
		else
		{
			base.transform.position += direction * bulletSpeed * Time.deltaTime;
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (hasHit)
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		switch (layer)
		{
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			hasHit = true;
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss6_Stage2.Inst.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = direction.normalized * knockbackScale[remainSplitTimes];
			info.teammateTakeDamageRatio = 4f;
			if (layer == 131072)
			{
				info.damage = 999999f;
			}
			SEMgr.Inst.elite11BulletHit.PlaySE();
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			Split(other == PlayerMgr.Inst.PlayerEtt);
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
}
