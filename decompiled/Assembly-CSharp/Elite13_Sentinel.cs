using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Elite13_Sentinel : MonoBehaviour
{
	[Header("飞行")]
	public VariableFloat flyDistance;

	public VariableFloat flyTime;

	public AnimationCurve flySpeedCurve;

	private Vector3 originPoint;

	private Vector3 targetPoint;

	[Header("检测和发射")]
	public float checkTime;

	public float checkRadius;

	public float checkInterval;

	private float checkTimer;

	public float shootDelay;

	public VariableFloat aimOffset;

	[Header("子弹")]
	public float bulletSpeed;

	public float bulletDuration;

	public float bulletHeight;

	public int bulletDamage;

	public float knockback;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	private Vector3 shootDir;

	private Vector3 bulletPosition;

	public Transform bulletTransform;

	public Transform shadowTransform;

	[Header("表现")]
	public ParticleSystem shootParticle;

	public ParticleSystem existParticle;

	public Shadow mainShadow;

	private float existTime;

	public bool prepared;

	public bool ended;

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void Initialize(bool useGivenDir, Vector3 dir)
	{
		flyDistance.RandomResult();
		flyTime.RandomResult();
		targetPoint = Tool2D.GetNavMeshPointIngoreZ((useGivenDir ? dir : Tool2D.GetDir()) * flyDistance.result + base.transform.position);
		originPoint = base.transform.position;
		prepared = false;
		ended = false;
		mainShadow.CreateShadow();
		mainShadow.Show();
		shadowTransform.gameObject.SetActive(value: false);
		existTime = 0f;
		bulletPosition = base.transform.position;
		bulletTransform.position = Tool2D.GetLayerPoint(bulletPosition + new Vector3(0f, 0f, 0f - bulletHeight));
		shadowTransform.position = Tool2D.GetLayerPoint(bulletPosition, LayerCorrectType.Shadow);
	}

	private void Update()
	{
		existTime += Time.deltaTime;
		if (!prepared)
		{
			base.transform.position = Vector3.Lerp(originPoint, targetPoint, flySpeedCurve.Evaluate(existTime / flyTime.result));
			if (existTime > flyTime.result)
			{
				prepared = true;
				existTime = 0f;
			}
		}
		else if (!ended)
		{
			bulletPosition = base.transform.position;
			checkTimer += Time.deltaTime;
			if (checkTimer > checkInterval)
			{
				checkTimer = 0f;
				CheckTarget();
			}
			if (existTime > checkTime)
			{
				Entity nearestFriendlyEntity = LevelMgr.Inst.CurrentRoomCtrller.GetNearestFriendlyEntity(base.transform.position);
				Vector3 vector = base.transform.position;
				if (nearestFriendlyEntity != Entity.Null && (!(nearestFriendlyEntity == PlayerMgr.Inst.PlayerEtt) || PlayerMgr.Inst.PlayerCtrller.IsVisible))
				{
					vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(nearestFriendlyEntity).Position;
				}
				Shoot(Tool2D.GetDir() * aimOffset.RandomResult() + vector);
				ended = true;
				existParticle.Stop();
				mainShadow.Hide();
				existTime = 0f;
			}
		}
		else if (ended)
		{
			if (shadowTransform.gameObject.activeSelf)
			{
				shadowTransform.gameObject.SetActive(value: false);
			}
			if (existTime > 2f)
			{
				Elite13.MiniPool.RecycleGO(base.gameObject);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!ended)
		{
			switch (other.tag)
			{
			case "Player":
			case "Teammate":
			case "Destructible":
				Shoot(other.transform.position);
				break;
			}
		}
	}

	private void Shoot(Vector3 position)
	{
		if (!ended)
		{
			existParticle.Stop();
			shootParticle.Play();
			shootDir = (position - base.transform.position).normalized;
			ended = true;
			existTime = 0f;
			mainShadow.Hide();
			Elite13.MiniPool.GetGO("Prefabs/EF/EF_Elite13_SmallArrow" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position).GetComponent<Elite13_Arrow>().Initialize(shootDir, bulletSpeed);
			SEMgr.Inst.elite13Shoot.PlaySE();
		}
	}

	private void CheckTarget()
	{
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, checkRadius, GameConst.Filter_Friendly, results);
		for (int i = 0; i < results.Count; i++)
		{
			if (!(results[i].entity == PlayerMgr.Inst.PlayerEtt) || PlayerMgr.Inst.PlayerCtrller.IsVisible)
			{
				Shoot(results[i].point + Tool2D.GetDir() * aimOffset.RandomResult());
			}
		}
	}
}
