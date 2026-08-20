using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster35_SpikeWave : MonoBehaviour
{
	public Monster35 master;

	public Entity targetEntity;

	private Vector3 spikePoint;

	public float spikeSpeed;

	public float spikeDeltaDistance;

	public Vector3 spikeDiration;

	public GameObject spikePrefab;

	public GameObject spikeGameObject;

	public GameObject BarrierPrefab;

	public int spikeCount;

	public float spikeRotateSpeed;

	public int maxSpikeCount;

	public bool spiking;

	private Vector3 lastSpikePosition;

	private List<Monster35_Spike> spikePool = new List<Monster35_Spike>();

	public bool useable;

	public bool tracking;

	public ParticleSystem moveParticle;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private void Start()
	{
		spikePoint = Tool2D.IgnoreZPoint(master.transform.position);
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		moveParticle.Clear();
	}

	private void Update()
	{
		useable = true;
		if (spiking)
		{
			useable = false;
			if (!moveParticle.isPlaying)
			{
				moveParticle.Play();
			}
		}
		moveParticle.transform.position = Tool2D.GetLayerPoint(spikePoint);
		if (!spiking)
		{
			moveParticle.Stop();
			for (int i = 0; i < spikePool.Count; i++)
			{
				if (spikePool[i].gameObject.activeSelf)
				{
					useable = false;
				}
			}
		}
		if (spiking)
		{
			if (UnitDotsSyncSystem.EntityIsValid(targetEntity) && tracking)
			{
				spikeDiration = Vector3.RotateTowards(spikeDiration, Tool2D.IgnoreZPoint((Vector3)UnitDotsSyncSystem.GetComponentData<LocalTransform>(targetEntity).Position - spikePoint), MathF.PI / 180f * spikeRotateSpeed * spikeSpeed * Time.deltaTime, 0f).normalized;
			}
			spikePoint += Time.deltaTime * spikeSpeed * spikeDiration;
			if (!spiking || !((spikePoint - lastSpikePosition).sqrMagnitude > spikeDeltaDistance * spikeDeltaDistance))
			{
				return;
			}
			lastSpikePosition = spikePoint;
			if (spikeCount < maxSpikeCount)
			{
				if (!tracking)
				{
					UnityEngine.Object.Instantiate(BarrierPrefab, spikePoint, Quaternion.identity, LevelMgr.Inst.CurrentRoomT);
				}
				else
				{
					SEMgr.Inst.monster35Spike.PlaySE();
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster35_Spike", spikePoint, 4f).GetComponent<Monster35_Spike>().master = master;
				}
				spikeCount++;
			}
			else
			{
				spiking = false;
				spikeCount = 0;
			}
		}
		else
		{
			spikeDiration = Tool2D.GetDir().normalized;
			spikePoint = Tool2D.IgnoreZPoint(master.transform.position);
			lastSpikePosition = Tool2D.IgnoreZPoint(master.transform.position);
		}
	}
}
