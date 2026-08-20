using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss56_Elite50Bomb : MonoBehaviour
{
	public ParticleSystem fireParticle;

	private float attackCDTimer;

	private List<Vector3> TargetShootPointsList;

	private float shootInterval;

	private float shootTimer;

	private Entity shooterEntity;

	private float landTime;

	private float bombRange;

	private void OnEnable()
	{
		shootTimer = 0f;
		bombRange = 0f;
		shooterEntity = Entity.Null;
	}

	public void InitialData(Entity shooterEntity, List<Vector3> targetPoints, float interval, float landTime, float bombRange, float DelayShootInterval)
	{
		TargetShootPointsList = targetPoints;
		shootInterval = interval;
		this.shooterEntity = shooterEntity;
		this.landTime = landTime;
		this.bombRange = bombRange;
		attackCDTimer = 0f - DelayShootInterval;
	}

	public void Update()
	{
		if (TargetShootPointsList.Count > 0 && !(shooterEntity == Entity.Null))
		{
			attackCDTimer += Time.deltaTime;
			if (!(attackCDTimer < shootInterval))
			{
				attackCDTimer -= shootInterval;
				fireParticle.Play();
				SEMgr.Inst.monster309_Cannon.PlaySE();
				Vector3 vector = new Vector3(fireParticle.transform.position.x, base.transform.position.y, 0f - fireParticle.transform.position.y + base.transform.position.y);
				Vector3 endPoint = TargetShootPointsList[0];
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite50_Cannon", vector).GetComponent<Monster309_Cannon>().InitializeCannon(vector, endPoint, landTime, shooterEntity, buffed: false, bombRange);
				TargetShootPointsList.RemoveAt(0);
			}
		}
	}
}
