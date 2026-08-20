using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;

public class Elite57MiniMissile : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	private static readonly int GroundHiddenHeight = Shader.PropertyToID("_GroundHiddenHeight");

	private static readonly int IsEnableEffect = Shader.PropertyToID("_IsEnableEffect");

	private static readonly int IsLandFromHigherPos = Shader.PropertyToID("_IsLandFromHigherPos");

	public Transform ModelTransform;

	public Transform BackTransform;

	public Transform FrontTransform;

	public SpriteRenderer BackSprite;

	public SpriteRenderer FrontSprite;

	private float p1RemainDistance;

	private float p1Speed;

	private Vector3 currentDir;

	private Vector3 lastFramePosition;

	private float damage;

	private float explosionRange;

	private Entity owner;

	private float explosionWaitTime;

	private bool finishedInitialize;

	public CapsuleCollider CC;

	private bool isLand;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		BackTransform.gameObject.SetActive(value: true);
		finishedInitialize = false;
		isLand = false;
		owner = Entity.Null;
		BackSprite.material.SetFloat(IsEnableEffect, 0f);
		FrontSprite.material.SetFloat(IsEnableEffect, 0f);
		UnitPhysicsSyncSystem.RegisterReciever(this, GameConst.Filter_MonsterAoeNoSpell, CC);
	}

	public void MissileInitialize(float phase1Speed, float phase1FlyDistance, Vector3 startDir, float hitDamage, float explosionRange, float explosionWaitTime, Entity owner, float initialHeight)
	{
		finishedInitialize = true;
		p1Speed = phase1Speed;
		p1RemainDistance = phase1FlyDistance;
		this.explosionRange = explosionRange;
		damage = hitDamage;
		currentDir = startDir;
		ModelTransform.right = currentDir;
		this.owner = owner;
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, initialHeight);
		lastFramePosition = base.transform.position;
		this.explosionWaitTime = explosionWaitTime;
		base.transform.DOLocalMoveZ(0f, phase1FlyDistance / phase1Speed);
	}

	private void Update()
	{
		if (finishedInitialize)
		{
			base.transform.position += currentDir * p1Speed * Time.deltaTime;
			p1RemainDistance -= Tool2D.IgnoreZDistance(base.transform.position, lastFramePosition);
			lastFramePosition = base.transform.position;
			if (p1RemainDistance <= 0f && !isLand)
			{
				isLand = true;
				p1Speed = 0f;
				StartCoroutine(LandExplosion(explosionWaitTime));
				BackSprite.material.SetFloat(GroundHiddenHeight, base.transform.position.y);
				FrontSprite.material.SetFloat(GroundHiddenHeight, base.transform.position.y);
				BackSprite.material.SetFloat(IsLandFromHigherPos, (!(currentDir.y >= 0f)) ? 1 : 0);
				FrontSprite.material.SetFloat(IsLandFromHigherPos, (!(currentDir.y >= 0f)) ? 1 : 0);
				BackSprite.material.SetFloat(IsEnableEffect, 1f);
				FrontSprite.material.SetFloat(IsEnableEffect, 1f);
			}
		}
	}

	private IEnumerator LandExplosion(float waitTime)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<WarningArea>().Initialize(explosionRange, explosionWaitTime);
		yield return new WaitForSeconds(waitTime);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster309_Explosion", base.transform.position, Quaternion.identity, Vector3.one * explosionRange / 2f, 3f);
		SEMgr.Inst.elite57MissileExplosion.PlaySE(SEPlayMode.Replay, 5, 0.16f);
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, explosionRange, GameConst.Filter_MonsterAoe, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					info.damage = damage;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, base.transform.position) * 8f;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		if (isLand)
		{
			return;
		}
		Debug.Log(111111);
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		bool flag = false;
		switch (layer)
		{
		case 512u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				flag = true;
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(owner);
				info.damage = damage;
				info.knockbackForce = currentDir * 8f;
				info.teammateTakeDamageRatio = 4f;
				if (result.unitCfg.unitType == UnitType.NotAttack)
				{
					info.ignoreFloatText = true;
					info.damage = 99999f;
				}
				UnitDotsSyncSystem.AddTakeDamageRequestEndless(other, info);
				if (result.unitCfg.unitType != UnitType.Brittleness)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite56_MissileHit", base.transform.position + new Vector3(0f, 0f, -0.5f), 3f).transform.right = currentDir;
					SEMgr.Inst.elite56MissileExplosion.PlaySE();
				}
			}
			break;
		}
		}
		if (flag)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
