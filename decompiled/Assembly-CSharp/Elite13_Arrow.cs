using Unity.Entities;
using UnityEngine;

public class Elite13_Arrow : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Header("表现")]
	public ParticleSystem trailParticle;

	public float trailRecycleTime;

	public GameObject bulletHead;

	public GameObject shadow;

	public float bulletHeight;

	public SpriteRenderer mainRenderer;

	public Sprite sprite1;

	public Sprite sprite2;

	public Sprite shadowSprite1;

	public Sprite shadowSprite2;

	public SpriteRenderer shadowRenderer;

	public float spriteChangeInterval;

	private float spriteChangeTimer;

	public bool isBig;

	public CapsuleCollider thisCollider;

	[Header("回收")]
	private float existTimer;

	public float lifeTime;

	public float pierceTime;

	private float pierceTimer;

	private bool recycle;

	[Header("数值")]
	public float speed;

	private Vector3 diration;

	public int damage;

	public float knockBack;

	private bool frame1;

	public Entity thisEntity { get; set; }

	public void OnEnable()
	{
		pierceTimer = pierceTime;
		trailParticle.Stop();
		trailParticle.Clear();
		recycle = false;
		existTimer = 0f;
		frame1 = false;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterEffectBulletNoSpell, thisCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void Frame1Initialize()
	{
		mainRenderer.enabled = true;
		bulletHead.SetActive(value: true);
		shadow.SetActive(value: true);
		trailParticle.Play();
	}

	public void Initialize(Vector3 diration, float speed)
	{
		this.diration = diration.normalized;
		shadow.transform.localScale = Vector3.one;
		bulletHead.transform.localScale = Vector3.one;
		this.speed = speed;
		if (GameMgr.IsMobile_Static)
		{
			this.speed *= 0.85f;
		}
	}

	private void Update()
	{
		bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight));
		shadow.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect);
		bulletHead.transform.up = diration;
		shadow.transform.up = diration;
		if (!frame1)
		{
			frame1 = true;
			Frame1Initialize();
		}
		spriteChangeTimer += Time.deltaTime;
		if (spriteChangeTimer > spriteChangeInterval)
		{
			spriteChangeTimer = 0f;
			if (mainRenderer.sprite == sprite1)
			{
				mainRenderer.sprite = sprite2;
			}
			else
			{
				mainRenderer.sprite = sprite1;
			}
			if (shadowRenderer.sprite == shadowSprite1)
			{
				shadowRenderer.sprite = shadowSprite2;
			}
			else
			{
				shadowRenderer.sprite = shadowSprite1;
			}
		}
		if (recycle)
		{
			if (trailParticle.isPlaying)
			{
				trailParticle.Stop();
			}
			mainRenderer.enabled = false;
			shadow.SetActive(value: false);
		}
		if (existTimer > lifeTime + trailRecycleTime)
		{
			Elite13.MiniPool.RecycleGO(base.gameObject);
		}
		if (!recycle)
		{
			base.transform.position += Time.deltaTime * diration * speed;
		}
		existTimer += Time.deltaTime;
		if (existTimer > lifeTime)
		{
			recycle = true;
		}
		if (pierceTimer == 0f)
		{
			recycle = true;
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (pierceTimer == 0f || recycle)
		{
			return;
		}
		string text = "EF_Elite13_Hit";
		if (isBig)
		{
			text = "EF_Elite13_HitBig";
		}
		if (GameMgr.IsHarmony_Static)
		{
			text += " H";
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		switch (layer)
		{
		case 256u:
			Elite13.MiniPool.GetGO("Prefabs/EF/" + text, base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight), 3f);
			SEMgr.Inst.elite13Miss.PlaySE(SEPlayMode.Unique);
			recycle = true;
			break;
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
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
				if (layer != 32768)
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
}
