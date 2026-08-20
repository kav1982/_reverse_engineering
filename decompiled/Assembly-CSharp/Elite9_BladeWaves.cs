using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Elite9_BladeWaves : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
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

	public bool isVertical;

	[Header("回收")]
	private float existTimer;

	public float lifeTime;

	public float pierceTime;

	private float pierceTimer;

	private bool recycle;

	private float recycleTimer;

	[Header("数值")]
	public CapsuleCollider thisCollider;

	public float speed;

	private Vector3 diration;

	public int damage;

	public float knockBack;

	private bool frame1;

	private UnitProperty master;

	public Entity thisEntity { get; set; }

	public virtual void OnEnable()
	{
		pierceTimer = pierceTime;
		trailParticle.Stop();
		trailParticle.Clear();
		recycle = false;
		existTimer = 0f;
		frame1 = false;
		bulletHead.transform.localScale = Vector3.one;
		shadow.transform.localScale = Vector3.one;
		recycleTimer = 0f;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterGroundWave, thisCollider);
	}

	public void Frame1Initialize()
	{
		bulletHead.SetActive(value: true);
		shadow.SetActive(value: true);
		trailParticle.Play();
	}

	public void Initialize(Vector3 diration, UnitProperty master)
	{
		this.diration = diration.normalized;
		this.master = master;
	}

	private void Update()
	{
		if (isVertical)
		{
			bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position);
			shadow.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect);
			shadow.transform.right = diration;
			mainRenderer.material.SetFloat("_RotateAngle", Tool2D.IgnoreZAngleWithSign(Vector3.right, diration));
			trailParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect);
		}
		else
		{
			bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight), LayerCorrectType.Coordinate);
			trailParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight + 0.1f), LayerCorrectType.Coordinate);
			shadow.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
			shadow.transform.right = diration;
			bulletHead.transform.right = diration;
		}
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
			if (isVertical)
			{
				if (shadowRenderer.sprite == shadowSprite1)
				{
					shadowRenderer.sprite = shadowSprite2;
				}
				else
				{
					shadowRenderer.sprite = shadowSprite1;
				}
			}
		}
		if (recycle)
		{
			if (trailParticle.isPlaying)
			{
				trailParticle.Stop();
			}
			recycleTimer += Time.deltaTime;
			bulletHead.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, recycleTimer / 0.33f);
			shadow.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, recycleTimer / 0.33f);
		}
		float num = (GameMgr.IsMobile_Static ? 0.8f : 1f);
		if (existTimer > lifeTime + trailRecycleTime)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		if (!recycle)
		{
			base.transform.position += Time.deltaTime * diration * speed * num;
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

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (pierceTimer == 0f || recycle)
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		string text = "EF_Monster51_Hit";
		if (GameMgr.IsHarmony_Static)
		{
			text = "EF_Monster51_Hit_H";
		}
		float3 position = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
		switch (layer)
		{
		case 256u:
			if (!isVertical)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, position, 3f);
			}
			recycle = true;
			break;
		case 65536u:
			if (isVertical)
			{
				recycle = true;
			}
			break;
		case 16777216u:
		{
			UnitDotsSyncSystem.ProcessHitSpell(other, damage, out var hitRollBall);
			if (hitRollBall)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, position, 3f);
			}
			SEMgr.Inst.spell3007Hit.PlaySE();
			break;
		}
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myEntity);
			info.damage = damage;
			info.knockbackForce = diration * knockBack;
			info.teammateTakeDamageRatio = 3f;
			if (layer != 32768)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, position, 3f);
				SEMgr.Inst.spell3007Hit.PlaySE();
				recycle = true;
				recycle = true;
			}
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
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

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}
}
