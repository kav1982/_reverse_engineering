using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectBullet : MonoBehaviour
{
	public enum BulletState
	{
		Processing,
		Fade,
		Hit
	}

	[Header("\u0368\ufffd\ufffd")]
	public float damage;

	public float knockBack;

	public Vector3 direction;

	public float speed;

	public Collider thisTrigger;

	public Rigidbody rigid;

	public int chapter;

	private float chapterDamage;

	public LayerMask attackLayers;

	public UnitProperty ownerPpt;

	[Header("״\u032c\ufffd\ufffd")]
	public BulletState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	protected bool changedState;

	protected float stateExistTime;

	[Header("\ufffd\ufffd\ufffd\ufffd")]
	public Transform tsf_BulletRoot;

	public Transform tsf_Shadow;

	public Transform tsf_BulletHead;

	public ParticleSystem trailParticle;

	public ParticleSystem hitParticle;

	[Header("\ufffd\ufffd\u0378\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd")]
	public bool hitCliff;

	public bool hitWall;

	public bool pierceRollball;

	public bool pierce;

	public List<UnitProperty> attackedPpts;

	public List<float> attackedPptsCd;

	public float attackCheckCD;

	[Header("\ufffd\ufffd\ufffd\ufffd")]
	public float recycleTime;

	public MiniObjPool ownerPool;

	private UnitProperty ppt;

	private SpellBase spellBase;

	private TakeDamageInfo info;

	public BulletState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
			varMgr.Clear();
		}
	}

	public void OnEnable()
	{
		chapterDamage = damage;
		if (chapter == 3)
		{
			chapterDamage = damage * 2f;
		}
		else if (chapter == 4)
		{
			chapterDamage = damage * 3f;
		}
		else if (chapter == 5)
		{
			chapterDamage = damage * 4f;
		}
		if (pierce)
		{
			attackedPpts.Clear();
			attackedPptsCd.Clear();
		}
		thisTrigger.enabled = true;
		state = BulletState.Processing;
	}

	public virtual void Initialize()
	{
		rigid.linearVelocity = direction.normalized * speed;
	}

	public virtual void Update()
	{
		if (pierce)
		{
			for (int num = attackedPptsCd.Count - 1; num >= 0; num--)
			{
				attackedPptsCd[num] -= Time.deltaTime;
				if (attackedPptsCd[num] < 0f)
				{
					attackedPptsCd.RemoveAt(num);
					attackedPpts.RemoveAt(num);
				}
			}
		}
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		StateProcess();
		AfterStateProcess();
	}

	public virtual void StateProcess()
	{
		switch (state)
		{
		case BulletState.Processing:
			if (changedState)
			{
				tsf_BulletHead.gameObject.SetActive(value: true);
				tsf_Shadow.gameObject.SetActive(value: true);
				tsf_BulletRoot.localScale = Vector3.one;
				tsf_Shadow.localScale = Vector3.one;
				trailParticle.Play();
			}
			break;
		case BulletState.Fade:
			if (changedState)
			{
				rigid.linearVelocity = Vector3.zero;
				thisTrigger.enabled = false;
				tsf_BulletHead.gameObject.SetActive(value: false);
				tsf_Shadow.gameObject.SetActive(value: false);
				trailParticle.Stop();
			}
			tsf_BulletHead.localScale = Vector3.one * Mathf.Lerp(1f, 0f, stateExistTime / 0.2f);
			tsf_Shadow.localScale = tsf_BulletHead.localScale;
			if (stateExistTime > recycleTime)
			{
				ownerPool.RecycleGO(base.gameObject);
			}
			break;
		case BulletState.Hit:
			if (changedState)
			{
				rigid.linearVelocity = Vector3.zero;
				tsf_BulletHead.gameObject.SetActive(value: false);
				tsf_Shadow.gameObject.SetActive(value: false);
				trailParticle.Stop();
				hitParticle.Play();
			}
			if (stateExistTime > recycleTime)
			{
				ownerPool.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	public virtual void AfterStateProcess()
	{
		tsf_BulletRoot.position = Tool2D.GetLayerPoint(base.transform.position);
		tsf_Shadow.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (state != 0)
		{
			return;
		}
		switch (other.tag)
		{
		case "Player":
			if (other.IsPlayerTrigger())
			{
				DoDamage();
			}
			break;
		case "Teammate":
		case "Brittleness":
		case "Destructible":
			DoDamage();
			break;
		case "RollBall":
			spellBase = other.GetComponentInParent<SpellBase>();
			((Spell1002RollBall)spellBase).TakeDamage(damage);
			HitSolid(other.tag);
			break;
		case "ButterFly":
			spellBase = other.GetComponentInParent<SpellBase>();
			((Spell1003Butterfly)spellBase).HitEFAndRecycle();
			break;
		case "Wall":
			if (hitWall)
			{
				HitSolid(other.tag);
			}
			break;
		case "Cliff":
			if (hitCliff)
			{
				HitSolid(other.tag);
			}
			break;
		}
		void DoDamage()
		{
			ppt = other.GetComponent<UnitProperty>();
			info = new TakeDamageInfo();
			if (!attackedPpts.Contains(ppt))
			{
				float num = damage;
				if (other.tag != "Player")
				{
					num = chapterDamage;
				}
				if (ownerPpt != null)
				{
					ppt.TakeDamage(num, ownerPpt, info);
				}
				else
				{
					ppt.TakeDamage(num, AttackerType.NothingSpecial, info);
				}
				if (other.tag != "Brittleness")
				{
					HitSolid(other.tag);
				}
			}
		}
	}

	public virtual void HitSolid(string hitTag)
	{
		if (!pierce || (hitTag == "RollBall" && !pierceRollball))
		{
			state = BulletState.Hit;
			thisTrigger.enabled = false;
		}
		else
		{
			hitParticle.Play();
		}
	}
}
