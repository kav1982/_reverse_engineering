using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster51_Spear : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	private float speed;

	private Vector3 diration;

	public float knockBack;

	public int damage;

	private bool recycle;

	public BoxCollider thisCollider;

	public Transform colliderTransform;

	public float lifeTime;

	private float lifeTimer;

	public SpriteRenderer mainSprite;

	public SpriteRenderer shadowSprite;

	public Transform mainTransform;

	public Transform headTransform;

	public Transform shadowTransform;

	public ParticleSystem trailParticle;

	public ParticleSystem ExplodeParticle;

	private bool frame1;

	public Monster51 master;

	public Entity thisEntity { get; set; }

	public void Initialize(Vector3 diration, float speed, Monster51 master)
	{
		recycle = false;
		this.diration = diration.normalized;
		this.speed = speed;
		colliderTransform.up = diration;
		thisCollider.enabled = true;
		lifeTimer = 0f;
		mainSprite.enabled = true;
		shadowSprite.enabled = true;
		frame1 = false;
		this.master = master;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterEffectBullet, thisCollider);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Frame1Initialize()
	{
		trailParticle.Play();
	}

	private void Die(bool playSound = true)
	{
		recycle = true;
		trailParticle.Stop();
		Invoke("Fade", 0.5f);
		thisCollider.enabled = false;
		if (playSound)
		{
			SEMgr.Inst.monster51_SpearEnd.PlaySE();
		}
	}

	private void Fade()
	{
		ExplodeParticle.Play();
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, 2f);
		mainSprite.enabled = false;
		shadowSprite.enabled = false;
	}

	private void Update()
	{
		if (!frame1)
		{
			frame1 = true;
			Frame1Initialize();
		}
		lifeTimer += Time.deltaTime;
		if (lifeTimer > lifeTime && !recycle)
		{
			Die(playSound: false);
		}
		mainTransform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, -0.5f));
		shadowTransform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		colliderTransform.position = base.transform.position;
		colliderTransform.up = diration;
		mainTransform.up = diration;
		shadowTransform.up = diration;
		if (!recycle)
		{
			base.transform.position += Time.deltaTime * diration * speed;
		}
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		if (recycle)
		{
			return;
		}
		string text = "EF_Monster51_Hit";
		if (GameMgr.IsChAge14_Static)
		{
			text = "EF_Monster51_Hit_H";
		}
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 256u:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, Tool2D.IgnoreZPoint(headTransform.position) + new Vector3(0f, 0f, -0.5f), 3f);
			Die();
			break;
		case 16777216u:
		{
			UnitDotsSyncSystem.ProcessHitSpell(other, damage, out var _);
			break;
		}
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (!UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				break;
			}
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = diration * knockBack;
			info.teammateTakeDamageRatio = 3f;
			if (result.unitCfg.unitType == UnitType.NotAttack)
			{
				info.damage *= 6f;
				if (result.unitCfg.currentHP > info.damage)
				{
					Die();
				}
			}
			if (result.unitCfg.unitType != UnitType.Brittleness)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, (Vector3)UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position + new Vector3(0f, 0f, -0.5f), 3f);
				SEMgr.Inst.elite9BladeHit.PlaySE();
			}
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
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
}
