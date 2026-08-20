using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster35_Spike : LayerCorrect
{
	[Space(50f)]
	public Vector3 originRingPosition;

	public Animator anima;

	public int damage;

	public float radius;

	public bool onlyDirt;

	public bool BigDirt;

	public SpriteRenderer dirtSprite;

	public Sprite[] dirtTextures = new Sprite[4];

	public bool attacking;

	public List<Transform> allTsf = new List<Transform>();

	public List<Entity> attackedEntity = new List<Entity>();

	public float damageInterval;

	private float damageIntervalTimer;

	public Monster35 master;

	private List<UnitDotsSyncSystem.DistanceHitResult> _hits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void Start()
	{
		anima.GetComponent<AnimaEvent>().DoAction = AnimaAction;
	}

	private void Update()
	{
		if (attacking)
		{
			damageIntervalTimer += Time.deltaTime;
			if (damageIntervalTimer < damageInterval)
			{
				damageIntervalTimer = 0f;
				Damage();
			}
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		if (onlyDirt)
		{
			anima.Play("Monster35_MoveDirt");
			dirtSprite.sprite = dirtTextures[Random.Range(0, 4)];
			anima.speed = Random.Range(0.8f, 1.2f);
		}
		else if (BigDirt)
		{
			anima.Play("Monster35_JumpDirt");
			dirtSprite.sprite = dirtTextures[Random.Range(0, 4)];
			anima.speed = Random.Range(0.8f, 1.2f);
		}
		else
		{
			anima.Play("Monster35_SpikeAppear");
		}
		attackedEntity.Clear();
		damageIntervalTimer = 0f;
	}

	private void Damage()
	{
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, radius, GameConst.Filter_Friendly, _hits);
		for (int i = 0; i < _hits.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = _hits[i];
			if (!attackedEntity.Contains(distanceHitResult.entity))
			{
				UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(distanceHitResult.entity);
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
				info.damage = damage;
				info.knockbackForce = Vector3.Normalize((Vector3)UnitDotsSyncSystem.GetComponentData<LocalTransform>(distanceHitResult.entity).Position - base.transform.position) * 5f;
				UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
			}
		}
	}

	private void Recycle()
	{
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Damage":
			attacking = true;
			Damage();
			break;
		case "DamageDone":
			attacking = false;
			break;
		case "end":
			Recycle();
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
