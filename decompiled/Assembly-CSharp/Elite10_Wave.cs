using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Elite10_Wave : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
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

	public CapsuleCollider thisCollider;

	private float spriteChangeTimer;

	[Header("回收")]
	private float existTimer;

	public float lifeTime;

	public float recycleTime;

	private float recycleTimer;

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
	}

	public void Frame1Initialize()
	{
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
		recycleTimer = 0f;
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterGroundWave, thisCollider);
	}

	public void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		shadow.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect);
		shadow.transform.right = diration;
		mainRenderer.material.SetFloat("_RotateAngle", Tool2D.IgnoreZAngleWithSign(Vector3.right, diration));
		trailParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect);
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
			recycleTimer += Time.deltaTime;
			bulletHead.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, recycleTimer / 0.33f);
			shadow.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, recycleTimer / 0.33f);
		}
		if (existTimer > lifeTime + trailRecycleTime)
		{
			Elite10.MiniPool.RecycleGO(base.gameObject);
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

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		if (pierceTimer == 0f || recycle || !UnitDotsSyncSystem.EntityIsValid(other))
		{
			return;
		}
		string text = "EF_Monster51_Hit";
		if (GameMgr.IsHarmony_Static)
		{
			text = "EF_Monster51_Hit_H";
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		float3 position = UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position;
		switch (layer)
		{
		case 256u:
		case 65536u:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, base.transform.position, 3f);
			recycle = true;
			break;
		case 16777216u:
		{
			UnitDotsSyncSystem.ProcessHitSpell(other, damage, out var hitRollBall);
			if (hitRollBall)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, position, 3f);
				SEMgr.Inst.spell3007Hit.PlaySE();
			}
			break;
		}
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite10.Inst.myPpt.myEntity);
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
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, position, 3f);
					SEMgr.Inst.spell3007Hit.PlaySE();
					recycle = true;
				}
			}
			break;
		}
		}
	}
}
