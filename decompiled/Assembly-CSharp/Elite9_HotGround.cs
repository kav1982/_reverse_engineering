using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite9_HotGround : MonoBehaviour
{
	public float attackInterval;

	public float damageInteval;

	private float attackTimer;

	public int damage;

	public float totalExistTime;

	private float existTimer;

	public float attackRadius;

	public Vector3 startPoint;

	public LayerMask damageMask;

	public float fadeSpeed;

	public List<Entity> attackedPpts = new List<Entity>();

	private List<float> attackedPptsCd = new List<float>();

	private Vector3 diration;

	public Elite9 master;

	public ParticleSystem mainParticle;

	public bool frame1;

	public void OnEnable()
	{
		existTimer = 0f;
		attackTimer = 0f;
		startPoint = Tool2D.IgnoreZPoint(base.transform.position);
		mainParticle.Clear();
		frame1 = true;
	}

	public void OnDisable()
	{
		mainParticle.Stop();
	}

	private void Update()
	{
		if (master.myPpt.AlreadyDead)
		{
			base.gameObject.SetActive(value: false);
		}
		if (frame1)
		{
			frame1 = false;
			mainParticle.Play();
		}
		Debug.DrawLine(startPoint, base.transform.position);
		for (int num = attackedPptsCd.Count - 1; num >= 0; num--)
		{
			attackedPptsCd[num] -= Time.deltaTime;
			if (attackedPptsCd[num] < 0f)
			{
				attackedPptsCd.RemoveAt(num);
				attackedPpts.RemoveAt(num);
			}
		}
		existTimer += Time.deltaTime;
		if (existTimer < totalExistTime)
		{
			diration = (base.transform.position - startPoint).normalized;
			attackTimer += Time.deltaTime;
			if (attackTimer > attackInterval)
			{
				attackTimer = 0f;
				AttackOnce();
			}
		}
		else if ((base.transform.position - startPoint).sqrMagnitude > 1f)
		{
			startPoint += Time.deltaTime * fadeSpeed * diration;
			attackTimer += Time.deltaTime;
			if (attackTimer > attackInterval)
			{
				attackTimer = 0f;
				AttackOnce();
			}
		}
	}

	public void AddAttackedPpt(Entity ppt)
	{
		if (!attackedPpts.Contains(ppt))
		{
			attackedPpts.Add(ppt);
			attackedPptsCd.Add(damageInteval);
		}
	}

	private void AttackOnce()
	{
		bool flag = master.state == Elite9.MonsterState.LaserAttack;
		UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(startPoint, base.transform.position - startPoint, attackRadius, (base.transform.position - startPoint).magnitude - (flag ? (attackRadius * 2f) : 0f), GameConst.Filter_Friendly);
		for (int i = 0; i < array.Length; i++)
		{
			Entity entity = array[i].entity;
			if (!attackedPpts.Contains(entity))
			{
				attackedPpts.Add(entity);
				attackedPptsCd.Add(damageInteval);
				SEMgr.Inst.elite9Burn.PlaySE();
				Elite9.MiniPool.GetGO("Prefabs/EF/EF_Elite9_Hit" + (GameMgr.IsHarmony_Static ? "_H" : ""), array[i].point, 2f);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite9.Inst.myPpt.myEntity);
				info.teammateTakeDamageRatio = 3f;
				info.damage = damage;
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
			}
		}
	}
}
