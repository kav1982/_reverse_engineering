using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss54_Line : MonoBehaviour
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

	public int bubbleCountPerMeter;

	public AnimationCurve attackWidthCurve;

	public AnimationCurve attackTransparentCurve;

	public float bigLaserWidthRatio;

	[Header("大激光旋转")]
	public float aimRotateTime;

	public AnimationCurve aimAngleCurve;

	public VariableFloat startRotateAngle;

	private float aimAngleSign;

	[Header("状态")]
	public bool isBigLaser;

	public LaserState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("逻辑")]
	public float attackTime;

	public float fadeTime;

	public float lineHeight;

	public float damage;

	public float damageRadius;

	public float damageInterval;

	public float checkInterval;

	private List<Entity> attackedEntity = new List<Entity>();

	private List<float> attackedTimer = new List<float>();

	private float checkIntervalTimer;

	private Vector3 direction;

	private UnitBase master;

	private bool warningFinish;

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

	public void SetWarningFinish()
	{
		warningFinish = true;
	}

	public void Initialize(UnitBase master, Vector3 direction)
	{
		this.master = master;
		this.direction = direction;
		lr_Shadow.positionCount = 2;
		lr_Warning.positionCount = 2;
		lr_WarningShadow.positionCount = 2;
		lr_Line.positionCount = 2;
		Vector3 dir = this.direction;
		if (isBigLaser)
		{
			startRotateAngle.RandomResult();
			dir = Tool2D.GetDir(this.direction, startRotateAngle.result);
			aimAngleSign = GeneralTool.HalfChanceNPOne();
		}
		warningParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * lineHeight) + Vector3.back * 0.03f;
		attackParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + Vector3.back * lineHeight) + Vector3.back * 0.03f;
		lr_Shadow.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow));
		lr_Shadow.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + this.direction * 50f, LayerCorrectType.Shadow));
		lr_WarningShadow.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow));
		lr_WarningShadow.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + dir * 50f, LayerCorrectType.Shadow));
		lr_Warning.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position + Vector3.back * lineHeight));
		lr_Warning.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + dir * 50f + Vector3.back * lineHeight));
		lr_Line.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position + Vector3.back * lineHeight));
		lr_Line.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + this.direction * 50f + Vector3.back * lineHeight));
		state = LaserState.Warning;
		attackedEntity.Clear();
		attackedTimer.Clear();
		lr_Shadow.enabled = false;
		lr_Warning.enabled = false;
		lr_Line.enabled = false;
		lr_WarningShadow.enabled = false;
	}

	public void OnDisable()
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
					warningFinish = false;
				}
				if (isBigLaser)
				{
					Vector3 dir = Tool2D.GetDir(direction, startRotateAngle.result * aimAngleSign * aimAngleCurve.Evaluate(Mathf.Min(1f, stateExistTime / aimRotateTime)));
					lr_WarningShadow.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + dir * 50f, LayerCorrectType.Shadow));
					lr_Warning.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position + dir * 50f + Vector3.back * lineHeight));
				}
				if (warningFinish)
				{
					state = LaserState.Attack;
				}
				break;
			case LaserState.Attack:
			{
				if (changedState)
				{
					attackParticle.Play();
					warningParticle.Stop();
					lr_Warning.enabled = false;
					lr_WarningShadow.enabled = false;
					lr_Line.enabled = true;
					lr_Shadow.enabled = true;
					if (isBigLaser)
					{
						SEMgr.Inst.elite11LaserLoop.PlayLoopSE(attackTime);
					}
					else
					{
						SEMgr.Inst.monster54_Laser.PlaySE();
					}
					DealDamage();
				}
				if (attackTransparentCurve.Evaluate(stateExistTime / attackTime) > 0.8f)
				{
					checkIntervalTimer += Time.deltaTime;
				}
				if (checkIntervalTimer > damageInterval)
				{
					checkIntervalTimer = 0f;
					DealDamage();
				}
				float num2 = (isBigLaser ? bigLaserWidthRatio : 1f);
				lr_Line.widthMultiplier = Mathf.Max(0f, attackWidthCurve.Evaluate(stateExistTime / attackTime)) * num2;
				lr_Shadow.widthMultiplier = Mathf.Max(0f, attackWidthCurve.Evaluate(stateExistTime / attackTime)) * num2;
				lr_Line.material.SetFloat("_Transparency", attackTransparentCurve.Evaluate(stateExistTime / attackTime));
				lr_Shadow.material.SetFloat("_Transparency", attackTransparentCurve.Evaluate(stateExistTime / attackTime));
				if (stateExistTime > attackTime)
				{
					if (master is Boss54_Child)
					{
						(master as Boss54_Child).LaserFinish();
					}
					else if (master is Boss54)
					{
						(master as Boss54).LaserFinish();
					}
					state = LaserState.Fade;
				}
				break;
			}
			case LaserState.Fade:
				if (changedState)
				{
					if (isBigLaser)
					{
						SEMgr.Inst.elite11LaserEnd.PlaySE();
					}
					attackParticle.Stop();
					lr_Shadow.enabled = false;
					lr_Line.enabled = false;
				}
				if (stateExistTime > fadeTime)
				{
					ObjPoolMgr.Inst.RecycleGO(base.gameObject);
				}
				break;
			}
		}
	}

	public void DealDamage()
	{
		UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(base.transform.position + Vector3.back * lineHeight, direction, damageRadius, 50f, GameConst.Filter_MonsterAoeUndiffer);
		for (int i = 0; i < array.Length; i++)
		{
			Entity entity = array[i].entity;
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
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_Hit", array[i].point + new Vector3(0f, 0f, -0.3f), 1f);
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
