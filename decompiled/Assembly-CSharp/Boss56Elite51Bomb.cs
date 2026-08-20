using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss56Elite51Bomb : MonoBehaviour
{
	public enum Elite51State
	{
		BornIdle,
		MoveToTarget,
		DashExplosion,
		LockMissile
	}

	private Elite51State state;

	public Transform RotateTransform;

	public Transform ShadowTransform;

	private Vector3 moveDir;

	private float skillCastTimer;

	private float idleTimer;

	private float angleSpeedRatio = 1f;

	public List<ParticleSystem> DashChargeParticles;

	public List<ParticleSystem> DashParticles;

	public List<TrailRenderer> DashTrails;

	public float BornWaitTime;

	private bool isInitialize;

	private Vector3 currentMotion;

	private Entity shooterEntity;

	private float newWaveBonusWaitTime;

	[Header("轰炸冲锋")]
	public float DashSpeed;

	public float DashChargeTime;

	public float DashShootInterval;

	private float dashShootTimer;

	public float DashTime;

	public float DashRotateSpeed;

	public float DashStopLockTime;

	private float dashChargeTimer;

	private float dashTimer;

	public LineRenderer DashWarningArea;

	public float WarningAreaMaxDistance;

	private float damageRadius;

	private void OnEnable()
	{
		foreach (TrailRenderer dashTrail in DashTrails)
		{
			dashTrail.Clear();
		}
		shooterEntity = Entity.Null;
		state = Elite51State.MoveToTarget;
		dashChargeTimer = 0f;
		dashTimer = 0f;
		skillCastTimer = 0f;
		DashWarningArea.transform.localScale = new Vector3(1f, 0f, 1f);
		idleTimer = 0f;
		dashShootTimer = 0f;
		angleSpeedRatio = 1f;
		currentMotion = Vector3.zero;
		isInitialize = false;
	}

	private void OnDisable()
	{
		foreach (ParticleSystem dashParticle in DashParticles)
		{
			dashParticle.Stop();
		}
		foreach (ParticleSystem dashChargeParticle in DashChargeParticles)
		{
			dashChargeParticle.Stop();
		}
		foreach (TrailRenderer dashTrail in DashTrails)
		{
			dashTrail.emitting = false;
			dashTrail.Clear();
		}
	}

	public void InitialData(Entity shooterEntity, Vector3 moveDir, float newWaveBonusWaitTime, float damageRadius)
	{
		this.shooterEntity = shooterEntity;
		this.moveDir = moveDir;
		this.newWaveBonusWaitTime = newWaveBonusWaitTime;
		this.damageRadius = damageRadius;
		foreach (TrailRenderer dashTrail in DashTrails)
		{
			dashTrail.Clear();
			dashTrail.emitting = true;
		}
		RotateTransform.right = moveDir;
		ShadowTransform.right = moveDir;
	}

	public void Update()
	{
		switch (state)
		{
		case Elite51State.MoveToTarget:
			skillCastTimer += Time.deltaTime;
			if (skillCastTimer >= BornWaitTime + newWaveBonusWaitTime)
			{
				skillCastTimer = 0f;
				CastSkill();
			}
			break;
		case Elite51State.DashExplosion:
			if (dashChargeTimer <= DashChargeTime)
			{
				if (dashChargeTimer == 0f)
				{
					foreach (ParticleSystem dashChargeParticle in DashChargeParticles)
					{
						dashChargeParticle.Play();
					}
					SEMgr.Inst.elite51DashCharge.PlaySE();
				}
				dashChargeTimer += Time.deltaTime;
				DashWarningArea.startWidth = Mathf.Lerp(DashWarningArea.startWidth, 3.5f, 8f * Time.deltaTime);
				if (dashChargeTimer >= DashChargeTime)
				{
					foreach (ParticleSystem dashParticle in DashParticles)
					{
						dashParticle.Play();
					}
				}
				SetMove(Vector3.zero);
				DashWarningArea.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position.IgnoreZ(), LayerCorrectType.GroundEffect));
				DashWarningArea.SetPosition(1, Tool2D.GetLayerPoint((base.transform.position + moveDir * (dashChargeTimer / DashChargeTime) * WarningAreaMaxDistance).IgnoreZ(), LayerCorrectType.GroundEffect));
				break;
			}
			DashWarningArea.startWidth = Mathf.Lerp(DashWarningArea.startWidth, 0f, 15f * Time.deltaTime);
			if (DashWarningArea.startWidth < 0.1f)
			{
				DashWarningArea.startWidth = 0f;
			}
			SetMove(moveDir * DashSpeed, instantLerp: true);
			dashShootTimer += Time.deltaTime;
			if (dashShootTimer >= DashShootInterval)
			{
				dashShootTimer -= DashShootInterval;
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite50_Cannon_Big", base.transform.position).GetComponent<Monster309_Cannon>().InitializeCannon(base.transform.position, base.transform.position + moveDir * 0.5f, 0.7f, shooterEntity, buffed: false, damageRadius);
			}
			dashTimer += Time.deltaTime;
			if (!(dashTimer >= DashTime))
			{
				break;
			}
			state = Elite51State.MoveToTarget;
			foreach (ParticleSystem dashParticle2 in DashParticles)
			{
				dashParticle2.Stop();
			}
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			break;
		}
		RotateTransform.right = moveDir;
		ShadowTransform.right = moveDir;
	}

	public void SetMove(Vector3 motion, bool instantLerp = false, float motionLerp = 0f)
	{
		float num = ((motionLerp > 0f) ? motionLerp : 5f);
		currentMotion = Tool2D.IgnoreZPoint(currentMotion);
		currentMotion = Vector3.Lerp(currentMotion, motion, instantLerp ? 1f : (num * Time.deltaTime));
		base.transform.position += currentMotion * Time.deltaTime;
	}

	private void CastSkill()
	{
		state = Elite51State.DashExplosion;
		dashTimer = 0f;
		dashChargeTimer = 0f;
		DashWarningArea.startWidth = 0f;
		dashShootTimer = DashShootInterval - 0.1f;
	}
}
