using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss55_Laser : MonoBehaviour
{
	public enum LaserState
	{
		Warning,
		Attack,
		Fade
	}

	[Header("表现")]
	public LineRenderer lr_Warning;

	public LineRenderer lr_WarningShadow;

	public LineRenderer lr_Shadow;

	public LineRenderer lr_Line;

	public ParticleSystem warningParticle;

	public ParticleSystem attackParticle;

	public ShockParam shockParam;

	private float shockTimer;

	public float baseWidth;

	private float checkIntervalTimer;

	private float widthMultiplier;

	private float transparency;

	private Vector3 direction;

	private List<Vector3> laserPoints = new List<Vector3>();

	private List<Vector3> laserTargetPoints = new List<Vector3>();

	private List<Entity> currentHitEntity = new List<Entity>();

	private List<Vector3> laserSpeed = new List<Vector3>();

	[Header("状态")]
	public LaserState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("小激光")]
	public float autoLaserWarningTime;

	public float autoLaserDuration;

	[Header("逻辑")]
	public float fadeTime;

	public float lineHeight;

	public float laserDistance;

	public int laserPointsCount;

	public float laserPointLerpTime;

	public float damage;

	public float damageRadius;

	public float damageInterval;

	public float checkInterval;

	private List<Entity> attackedEntity = new List<Entity>();

	private List<float> attackedTimer = new List<float>();

	private UnitBase master;

	private bool isAutoLaser;

	public LaserState state
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
		}
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
		warningParticle.Stop();
		warningParticle.Clear();
		attackParticle.Stop();
		attackParticle.Clear();
		lr_Shadow.enabled = false;
		lr_Warning.enabled = false;
		lr_Line.enabled = false;
		lr_WarningShadow.enabled = false;
	}

	public void Initialize(Vector3 direction, bool isAutoLaser)
	{
		this.isAutoLaser = isAutoLaser;
		master = Boss55.Inst;
		this.direction = direction;
		lr_Shadow.positionCount = 2;
		lr_Warning.positionCount = 2;
		lr_WarningShadow.positionCount = 2;
		lr_Line.positionCount = 2;
		Vector3 vector = this.direction;
		warningParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * lineHeight) + Vector3.back * 0.03f;
		attackParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * lineHeight) + Vector3.back * 0.03f;
		lr_Shadow.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow));
		lr_Shadow.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + this.direction * laserDistance, LayerCorrectType.Shadow));
		lr_WarningShadow.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow));
		lr_WarningShadow.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + vector * laserDistance, LayerCorrectType.Shadow));
		lr_Warning.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position + Vector3.back * lineHeight));
		lr_Warning.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + vector * laserDistance + Vector3.back * lineHeight));
		lr_Line.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position + Vector3.back * lineHeight));
		lr_Line.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + this.direction * laserDistance + Vector3.back * lineHeight));
		state = LaserState.Warning;
		checkIntervalTimer = 0f;
		attackedEntity.Clear();
		attackedTimer.Clear();
		lr_Shadow.enabled = false;
		lr_Warning.enabled = false;
		lr_Line.enabled = false;
		lr_WarningShadow.enabled = false;
	}

	public void SetStartAndDir(Vector3 startPoint, Vector3 dir)
	{
		base.transform.position = startPoint;
		direction = dir;
		if (state == LaserState.Attack || state == LaserState.Fade)
		{
			SetLaserTargetPoints();
		}
	}

	public void Update()
	{
		if (master.myPpt.AlreadyDead)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		else if (master.deadStayed)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		else
		{
			if (master.IsLocked)
			{
				return;
			}
			for (int num = attackedEntity.Count - 1; num >= 0; num--)
			{
				attackedTimer[num] -= Time.deltaTime;
				if (attackedTimer[num] < 0f)
				{
					attackedTimer.RemoveAt(num);
					attackedEntity.RemoveAt(num);
				}
			}
			stateExistTime += Time.deltaTime;
			if (stateQuit)
			{
				stateQuit = false;
				changedState = true;
			}
			else
			{
				changedState = false;
			}
			switch (state)
			{
			case LaserState.Warning:
				if (changedState)
				{
					lr_Warning.enabled = true;
					lr_WarningShadow.enabled = true;
					warningParticle.Play();
				}
				if (isAutoLaser && stateExistTime > autoLaserWarningTime)
				{
					state = LaserState.Attack;
				}
				break;
			case LaserState.Attack:
				if (changedState)
				{
					attackParticle.Play();
					warningParticle.Stop();
					lr_Warning.enabled = false;
					lr_WarningShadow.enabled = false;
					lr_Line.enabled = true;
					lr_Shadow.enabled = true;
					InitLaserPoints();
					checkIntervalTimer = 0f;
					widthMultiplier = 1f;
					transparency = 1f;
					DealDamage();
					SEMgr.Inst.spell1011Shoot.PlaySE();
					Boss55.Inst.AS_LaserLoop.Play();
					CamController.Inst.SetShock(shockParam);
					shockTimer = 0f;
				}
				shockTimer += Time.deltaTime;
				if (shockTimer > shockParam.time)
				{
					CamController.Inst.SetShock(shockParam);
					shockTimer -= shockParam.time;
				}
				checkIntervalTimer += Time.deltaTime;
				if (checkIntervalTimer > checkInterval)
				{
					attackParticle.Play();
					checkIntervalTimer = 0f;
					DealDamage();
				}
				if (isAutoLaser && stateExistTime > autoLaserDuration)
				{
					state = LaserState.Fade;
				}
				break;
			case LaserState.Fade:
				if (changedState)
				{
					Boss55.Inst.AS_LaserLoop.Stop();
					SEMgr.Inst.elite11LaserEnd.PlaySE();
					attackParticle.Stop();
				}
				widthMultiplier = 1f - stateExistTime / fadeTime;
				transparency = 1f - stateExistTime / fadeTime;
				if (stateExistTime > fadeTime)
				{
					ObjPoolMgr.Inst.RecycleGO(base.gameObject);
				}
				break;
			}
		}
	}

	private void LateUpdate()
	{
		if (state == LaserState.Attack || state == LaserState.Fade)
		{
			SetLaserTargetPoints();
			LerpLaserPoints();
		}
		lr_WarningShadow.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow));
		lr_WarningShadow.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + direction * laserDistance, LayerCorrectType.Shadow));
		lr_Warning.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position + Vector3.back * lineHeight));
		lr_Warning.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + direction * laserDistance + Vector3.back * lineHeight));
		SetLineRendererPoints();
		lr_Line.widthMultiplier = Mathf.Max(0f, baseWidth * widthMultiplier);
		lr_Shadow.widthMultiplier = Mathf.Max(0f, baseWidth * widthMultiplier);
		lr_Line.material.SetFloat("_Transparency", transparency);
		lr_Shadow.material.SetFloat("_Transparency", transparency);
	}

	private void InitLaserPoints()
	{
		lr_Line.positionCount = laserPointsCount + 1;
		lr_Shadow.positionCount = laserPointsCount + 1;
		SetLaserTargetPoints();
		laserPoints.Clear();
		laserSpeed.Clear();
		for (int i = 0; i < laserTargetPoints.Count; i++)
		{
			laserPoints.Add(laserTargetPoints[i]);
			laserSpeed.Add(Vector3.zero);
		}
		SetLineRendererPoints();
	}

	private void SetLaserTargetPoints()
	{
		laserTargetPoints.Clear();
		Vector3 vector = base.transform.position + Vector3.back * lineHeight;
		for (int i = 0; i <= laserPointsCount; i++)
		{
			laserTargetPoints.Add(vector + direction * laserDistance * ((float)i / (float)laserPointsCount));
		}
	}

	private void LerpLaserPoints()
	{
		for (int i = 0; i < laserPoints.Count; i++)
		{
			Vector3 currentVelocity = laserSpeed[i];
			laserPoints[i] = Vector3.SmoothDamp(laserPoints[i], laserTargetPoints[i], ref currentVelocity, Mathf.Lerp(0f, laserPointLerpTime, (float)i / (float)laserPoints.Count));
			laserSpeed[i] = currentVelocity;
		}
	}

	private void SetLineRendererPoints()
	{
		if (state == LaserState.Attack || state == LaserState.Fade)
		{
			for (int i = 0; i < laserPoints.Count; i++)
			{
				lr_Line.SetPosition(i, Tool2D.GetLayerPoint(laserPoints[i]));
				lr_Shadow.SetPosition(i, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(laserPoints[i]), LayerCorrectType.Shadow));
			}
		}
	}

	public void DealDamage()
	{
		currentHitEntity.Clear();
		for (int i = 0; i < laserPoints.Count - 1; i++)
		{
			Vector3 vector = laserPoints[i + 1] - laserPoints[i];
			UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(laserPoints[i], vector.normalized, damageRadius, vector.magnitude + 0.1f, GameConst.Filter_MonsterAoeUndiffer);
			for (int j = 0; j < array.Length; j++)
			{
				Entity entity = array[j].entity;
				if (currentHitEntity.Contains(entity))
				{
					continue;
				}
				currentHitEntity.Add(entity);
				if (attackedEntity.Contains(entity))
				{
					continue;
				}
				uint layer = UnitDotsSyncSystem.GetLayer(entity);
				switch (layer)
				{
				case 16777216u:
				{
					UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
					break;
				}
				case 512u:
				case 2097152u:
				{
					TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
					info2.damage = damage;
					if (layer == 131072)
					{
						info2.ignoreFloatText = true;
					}
					UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info2);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_Hit H", array[j].point + new Vector3(0f, 0f, -0.3f), 1f);
					SEMgr.Inst.monster305_Hit.PlaySE();
					attackedEntity.Add(entity);
					attackedTimer.Add(damageInterval);
					break;
				}
				case 32768u:
				case 131072u:
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(master.myPpt.myEntity);
					info.damage = damage * 2f;
					if (layer == 131072)
					{
						info.ignoreFloatText = true;
					}
					UnitDotsSyncSystem.AddTakeDamageRequestEndless(entity, info);
					attackedEntity.Add(entity);
					attackedTimer.Add(damageInterval);
					break;
				}
				}
			}
		}
	}
}
