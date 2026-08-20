using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster49_Missile : MonoBehaviour
{
	public ParticleSystem[] ps_MissileShadow;

	public Transform tsf_LayerMissile;

	public Transform tsf_LayerShadow;

	public Transform tsf_LayerExplosion;

	public Transform tsf_LayerCrack;

	public float middlePointHeight;

	public float flySpeed;

	public float landPointRadius;

	[Header("Explosion")]
	public float explosionRadius;

	public int explosionDamageForPlayer;

	public float frozenTime;

	public float explosionWaitTime;

	public ShockParam shock;

	[Header("Audio")]
	public AudioSource as_Fly;

	private bool isFram1Initial;

	private Vector3 startPoint;

	private Vector3 endPoint;

	private Vector3 middlePoint;

	private bool isFly = true;

	private float flyLerpSpeed;

	private float currentLerp;

	private float waitTimer;

	public Monster49 master;

	private void OnEnable()
	{
		isFram1Initial = false;
		tsf_LayerMissile.gameObject.SetActive(value: false);
		tsf_LayerShadow.gameObject.SetActive(value: false);
		tsf_LayerExplosion.gameObject.SetActive(value: false);
		tsf_LayerCrack.gameObject.SetActive(value: false);
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_Fly.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			flySpeed *= 0.9f;
		}
	}

	private void Update()
	{
		if (!isFram1Initial)
		{
			isFram1Initial = true;
			tsf_LayerMissile.gameObject.SetActive(value: true);
			tsf_LayerShadow.gameObject.SetActive(value: true);
			isFly = true;
			currentLerp = 0f;
			waitTimer = 0f;
			as_Fly.Play();
			startPoint = base.transform.position;
			if (PlayerMgr.Inst.PlayerCtrller.IsVisible)
			{
				endPoint = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, landPointRadius);
			}
			else
			{
				UnitProperty nearestTargetablePpt = LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(base.transform.position);
				if (nearestTargetablePpt != null)
				{
					endPoint = nearestTargetablePpt.transform.position + Tool2D.GetDir() * UnityEngine.Random.Range(0f, landPointRadius);
				}
				else
				{
					Vector3 a = LevelMgr.Inst.CurrentRoomCtrller.RoomScale;
					Vector3 vector = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + Vector3.Scale(a, new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0f));
					endPoint = Tool2D.IgnoreZPoint(Tool2D.GetNavMeshPoint(vector));
				}
			}
			middlePoint = base.transform.position + new Vector3(0f, 0f, 0f - middlePointHeight);
			float num = Vector3.Distance(startPoint, endPoint);
			flyLerpSpeed = 1f / (num / flySpeed);
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsChAge14_Static ? " purple" : ""), endPoint).GetComponent<WarningArea>().Initialize(explosionRadius, num / flySpeed);
		}
		if (!isFly)
		{
			return;
		}
		currentLerp += flyLerpSpeed * Time.deltaTime;
		base.transform.position = GeneralTool.QuadraticBezierCurve(startPoint, middlePoint, endPoint, currentLerp);
		tsf_LayerMissile.position = Tool2D.GetLayerPoint(base.transform);
		tsf_LayerShadow.position = Tool2D.IgnoreZPoint(base.transform, 1.05f);
		if (currentLerp >= 1f)
		{
			isFly = false;
			for (int i = 0; i < ps_MissileShadow.Length; i++)
			{
				ps_MissileShadow[i].Stop();
			}
			CamController.Inst.SetShock(shock);
			tsf_LayerExplosion.gameObject.SetActive(value: true);
			tsf_LayerCrack.gameObject.SetActive(value: true);
			tsf_LayerExplosion.position = Tool2D.GetLayerPoint(base.transform);
			tsf_LayerCrack.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.ExplosionTrace);
			as_Fly.Stop();
			SEMgr.Inst.monster49_Missile.PlaySE();
			List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
			UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, explosionRadius, GameConst.Filter_MonsterAoe, list);
			for (int j = 0; j < list.Count; j++)
			{
				UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[j];
				Entity entity = distanceHitResult.entity;
				switch (UnitDotsSyncSystem.GetLayer(entity))
				{
				case 16777216u:
				{
					UnitDotsSyncSystem.ProcessHitSpell(entity, explosionDamageForPlayer, out var _);
					break;
				}
				case 512u:
				case 32768u:
				case 131072u:
				case 2097152u:
				{
					if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(distanceHitResult.entity, out var result))
					{
						result.SetFrozen(frozenTime);
						UnitDotsSyncSystem.SetComponentData(result, distanceHitResult.entity);
						TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
						info.damage = explosionDamageForPlayer;
						info.teammateTakeDamageRatio = 3f;
						UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
					}
					break;
				}
				}
			}
		}
		else
		{
			waitTimer += Time.deltaTime;
			if (waitTimer >= explosionWaitTime)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
	}
}
