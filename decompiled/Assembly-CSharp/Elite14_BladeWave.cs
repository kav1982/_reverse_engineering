using System.Collections.Generic;
using UnityEngine;

public class Elite14_BladeWave : MonoBehaviour
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
	public float startSpeed;

	public float maxSpeed;

	public float accleration;

	private float nowSpeed;

	private Vector3 direction;

	public int damage;

	public float knockBack;

	public List<UnitProperty> attackedPpts = new List<UnitProperty>();

	private bool frame1;

	[Header("判定")]
	public Transform tsf_TriggerRoot;

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
		attackedPpts.Clear();
	}

	public void Frame1Initialize()
	{
		bulletHead.SetActive(value: true);
		shadow.SetActive(value: true);
		trailParticle.Play();
	}

	public void Initialize(Vector3 direction)
	{
		this.direction = direction.normalized;
		nowSpeed = startSpeed;
	}

	private void Update()
	{
		bulletHead.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight), LayerCorrectType.Coordinate);
		trailParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight + 0.1f), LayerCorrectType.Coordinate);
		shadow.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		shadow.transform.right = direction;
		trailParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - bulletHeight), LayerCorrectType.Coordinate);
		trailParticle.transform.right = direction;
		tsf_TriggerRoot.transform.right = direction;
		bulletHead.transform.right = direction;
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
			Elite14_Stage2.MiniPool.RecycleGO(base.gameObject);
		}
		if (!recycle)
		{
			nowSpeed = Mathf.MoveTowards(nowSpeed, maxSpeed, accleration * Time.deltaTime);
			base.transform.position += Time.deltaTime * direction * nowSpeed;
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

	private void OnTriggerEnter(Collider other)
	{
		if (pierceTimer == 0f || recycle)
		{
			return;
		}
		TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
		takeDamageInfo.damage = damage;
		takeDamageInfo.knockbackForce = direction * knockBack;
		takeDamageInfo.teammateTakeDamageRatio = 3f;
		string text = "EF_Monster51_Hit";
		if (GameMgr.IsHarmony_Static)
		{
			text = "EF_Monster51_Hit_H";
		}
		switch (other.tag)
		{
		case "Player":
			if (other.IsPlayerTrigger())
			{
				UnitProperty component = other.GetComponent<UnitProperty>();
				if (!attackedPpts.Contains(component))
				{
					attackedPpts.Add(component);
					Elite14_Stage2.MiniPool.GetGO("Prefabs/EF/" + text, component.transform.position, 3f);
					SEMgr.Inst.elite9BladeHit.PlaySE();
					component.TakeDamage(damage, Elite14_Stage2.Inst.myPpt, takeDamageInfo);
				}
			}
			break;
		case "Teammate":
		{
			UnitProperty component = other.GetComponent<UnitProperty>();
			if (!attackedPpts.Contains(component))
			{
				attackedPpts.Add(component);
				Elite14_Stage2.MiniPool.GetGO("Prefabs/EF/" + text, component.transform.position, 3f);
				SEMgr.Inst.elite9BladeHit.PlaySE();
				component.TakeDamage(damage, Elite14_Stage2.Inst.myPpt, takeDamageInfo);
			}
			break;
		}
		case "Brittleness":
		{
			UnitProperty component = other.GetComponent<UnitProperty>();
			component.TakeDamage(damage, Elite14_Stage2.Inst.myPpt, takeDamageInfo);
			break;
		}
		case "Wall":
			Elite14_Stage2.MiniPool.GetGO("Prefabs/EF/" + text, other.transform.position, 3f);
			recycle = true;
			break;
		case "Destructible":
		{
			UnitProperty component = other.GetComponent<UnitProperty>();
			if (!attackedPpts.Contains(component))
			{
				attackedPpts.Add(component);
				Elite14_Stage2.MiniPool.GetGO("Prefabs/EF/" + text, component.transform.position, 3f);
				SEMgr.Inst.elite9BladeHit.PlaySE();
				takeDamageInfo.isFloatText = false;
				component.TakeDamage(999f, Elite14_Stage2.Inst.myPpt, takeDamageInfo);
			}
			break;
		}
		case "Cliff":
			break;
		}
	}
}
