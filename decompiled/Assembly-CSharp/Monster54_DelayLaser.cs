using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster54_DelayLaser : MonoBehaviour
{
	public enum LaserState
	{
		Fly,
		Charge,
		Attack,
		End
	}

	public Vector3 diration;

	public float attackDelay;

	public float laserExistTime;

	public LineRenderer warningRenderer;

	public LineRenderer warningShadowRenderer;

	public LineRenderer laserRenderer;

	public LineRenderer shadowRenderer;

	public ParticleSystem mainParticle;

	public float mainParticleFadeTime;

	public ParticleSystem chargeParticle;

	public ParticleSystem attackParticle;

	public Shadow shadow;

	public float laserHeight;

	public float laserCheckRadius;

	public float warningMaxAlpha;

	public float warningAttackMaxAlpha;

	public AnimationCurve attackWidthCurve;

	public AnimationCurve attackTransparentCurve;

	public float attackExistTime;

	public float attackCheckInterval;

	private float attackCheckTimer;

	private List<Entity> attackedTarget = new List<Entity>();

	private List<float> attackedTime = new List<float>();

	private List<float> attackedTimeQuick = new List<float>();

	public float damageInterval;

	private bool attackFinish;

	private Vector3 endPoint;

	private float blockDistance;

	[Header("状态机")]
	private LaserState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("飞行")]
	private Vector3 flyEndPoint;

	private Vector3 flyDiration;

	public float flyDuration;

	private float nowHeight;

	private float speed;

	private float upSpeed;

	public float gravity;

	[Header("伤害")]
	public LayerMask blockMask;

	public LayerMask attackMask;

	public int damage;

	public VariableFloat aimOffset;

	[Header("二模式")]
	public bool isSecondMode;

	public int nodeCount;

	public float laserDistance;

	public float middlePointCount;

	public VariableFloat offsetRange;

	public List<Vector3> middlePoints = new List<Vector3>();

	public List<Vector3> middlePointSpeed = new List<Vector3>();

	public List<Vector3> allNode = new List<Vector3>();

	public VariableFloat middlePointSpeedRange;

	public AnimationCurve middlePointSpeedCurve;

	public AnimationCurve secondModeAttackWidthCurve;

	public AnimationCurve secondModeAttackTransparentCurve;

	private float stopNode;

	private Vector3 stopPoint;

	public ShockParam shockParam;

	private UnitProperty masterPpt;

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

	public void SetAllNode()
	{
		Vector3[] points = middlePoints.ToArray();
		allNode.Clear();
		for (int i = 0; i < nodeCount; i++)
		{
			allNode.Add(GeneralTool.FreeBezierCurve((float)i / (float)(nodeCount - 1), points));
		}
		bool flag = false;
		stopNode = nodeCount - 1;
		stopPoint = middlePoints[middlePoints.Count - 1];
		for (int j = 0; j < nodeCount - 1; j++)
		{
			if (!flag)
			{
				Vector3 vector = allNode[j + 1] - allNode[j];
				if (UnitDotsSyncSystem.Raycast(allNode[j], vector.normalized, vector.magnitude, GameConst.Filter_Wall, out var result))
				{
					blockDistance = (result.point - allNode[j]).magnitude;
					stopPoint = result.point;
					stopNode = j + 1;
					flag = true;
				}
			}
		}
	}

	public void Initialize(Vector3 diration, float startHeight, UnitProperty masterPpt)
	{
		nowHeight = startHeight;
		flyEndPoint = diration;
		flyDiration = Tool2D.IgnoreZPoint(flyEndPoint - base.transform.position).normalized;
		upSpeed = GeneralTool.CannonInitialSpeed(laserHeight - startHeight, gravity, flyDuration);
		speed = Tool2D.IgnoreZPoint(base.transform.position - flyEndPoint).magnitude / flyDuration;
		mainParticle.Play();
		shadow.Show();
		mainParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - nowHeight));
		chargeParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - nowHeight));
		attackParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - nowHeight));
		this.masterPpt = masterPpt;
		if (isSecondMode)
		{
			warningRenderer.positionCount = nodeCount;
			warningShadowRenderer.positionCount = nodeCount;
			shadowRenderer.positionCount = nodeCount;
			laserRenderer.positionCount = nodeCount;
		}
		else
		{
			warningRenderer.positionCount = 2;
			warningShadowRenderer.positionCount = 2;
			shadowRenderer.positionCount = 2;
			laserRenderer.positionCount = 2;
		}
		warningRenderer.enabled = false;
		warningShadowRenderer.enabled = false;
		shadowRenderer.enabled = false;
		laserRenderer.enabled = false;
		state = LaserState.Fly;
		attackedTarget.Clear();
		attackedTime.Clear();
		attackedTimeQuick.Clear();
	}

	private void Update()
	{
		for (int num = attackedTime.Count - 1; num >= 0; num--)
		{
			attackedTime[num] -= Time.deltaTime;
			if (attackedTime[num] < 0f)
			{
				attackedTime.RemoveAt(num);
				attackedTarget.RemoveAt(num);
			}
		}
		for (int num2 = attackedTimeQuick.Count - 1; num2 >= 0; num2--)
		{
			attackedTimeQuick[num2] -= Time.deltaTime;
			if (attackedTimeQuick[num2] < 0f)
			{
				attackedTimeQuick.RemoveAt(num2);
			}
		}
		mainParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - nowHeight));
		chargeParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - nowHeight));
		attackParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - nowHeight));
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
		case LaserState.Fly:
			nowHeight += Time.deltaTime * upSpeed;
			upSpeed += Time.deltaTime * gravity;
			if (nowHeight < laserHeight)
			{
				nowHeight = laserHeight;
				state = LaserState.Charge;
			}
			else
			{
				base.transform.position += Time.deltaTime * flyDiration * speed;
			}
			break;
		case LaserState.Charge:
			if (changedState)
			{
				chargeParticle.Play();
				Entity nearestFriendlyEntity = LevelMgr.Inst.CurrentRoomCtrller.GetNearestFriendlyEntity(base.transform.position);
				diration = Tool2D.GetDir();
				if (nearestFriendlyEntity != Entity.Null)
				{
					Vector3 vector = ((!(nearestFriendlyEntity == PlayerMgr.Inst.PlayerEtt)) ? Vector3.zero : PlayerMgr.Inst.PlayerCtrller.CurrentMotion);
					diration = Tool2D.GetDir() * aimOffset.RandomResult() + Tool2D.IgnoreZPoint((Vector3)UnitDotsSyncSystem.GetComponentData<LocalTransform>(nearestFriendlyEntity).Position - base.transform.position) + vector * attackDelay;
				}
				if (isSecondMode)
				{
					middlePoints.Clear();
					for (int k = 0; (float)k < middlePointCount; k++)
					{
						Vector3 item = Vector3.Lerp(base.transform.position + new Vector3(0f, 0f, 0f - laserHeight), base.transform.position + new Vector3(0f, 0f, 0f - laserHeight) + diration.normalized * laserDistance, (float)k / (middlePointCount - 1f));
						middlePoints.Add(item);
					}
					SetAllNode();
				}
				else
				{
					blockDistance = 30f;
					if (UnitDotsSyncSystem.Raycast(base.transform.position + new Vector3(0f, 0f, 0f - laserHeight), diration, 30f, GameConst.Filter_Wall, out var result))
					{
						blockDistance = (result.point - base.transform.position).magnitude;
						endPoint = Tool2D.IgnoreZPoint(result.point);
					}
					else
					{
						endPoint = base.transform.position + diration * blockDistance;
					}
				}
				laserRenderer.widthMultiplier = warningRenderer.widthMultiplier;
				warningRenderer.material.SetFloat("_Transparency", stateExistTime / attackDelay * warningMaxAlpha);
				warningShadowRenderer.material.SetFloat("_Transparency", stateExistTime / attackDelay * warningMaxAlpha * 0.4f);
				warningRenderer.enabled = true;
				warningShadowRenderer.enabled = true;
				laserRenderer.enabled = false;
			}
			warningRenderer.material.SetFloat("_Transparency", stateExistTime / attackDelay * warningMaxAlpha);
			warningShadowRenderer.material.SetFloat("_Transparency", stateExistTime / attackDelay * warningMaxAlpha * 0.4f);
			if (stateExistTime > attackDelay)
			{
				state = LaserState.Attack;
			}
			break;
		case LaserState.Attack:
			if (changedState)
			{
				attackParticle.Play();
				chargeParticle.Stop();
				warningRenderer.enabled = false;
				warningShadowRenderer.enabled = false;
				shadowRenderer.enabled = true;
				laserRenderer.enabled = true;
				if (isSecondMode)
				{
					SEMgr.Inst.spell1011Shoot.PlaySE();
					SEMgr.Inst.spell1011Loop.PlayLoopSE(attackExistTime);
				}
				else
				{
					SEMgr.Inst.monster54_Laser.PlaySE();
				}
				middlePointSpeed.Clear();
				for (int i = 0; (float)i < middlePointCount; i++)
				{
					middlePointSpeed.Add(Tool2D.GetDir() * middlePointSpeedRange.RandomResult());
				}
				attackCheckTimer = 0f;
				attackFinish = false;
			}
			if (isSecondMode)
			{
				SetAllNode();
				for (int j = 0; (float)j < middlePointCount; j++)
				{
					if (j != 0 && (float)j != middlePointCount - 1f)
					{
						middlePoints[j] += Time.deltaTime * middlePointSpeed[j] * middlePointSpeedCurve.Evaluate(stateExistTime / laserExistTime);
					}
				}
				laserRenderer.widthMultiplier = Mathf.Max(0f, secondModeAttackWidthCurve.Evaluate(stateExistTime / laserExistTime));
				shadowRenderer.widthMultiplier = Mathf.Max(0f, secondModeAttackWidthCurve.Evaluate(stateExistTime / laserExistTime));
				laserRenderer.material.SetFloat("_Transparency", secondModeAttackTransparentCurve.Evaluate(stateExistTime / laserExistTime));
				shadowRenderer.material.SetFloat("_Transparency", secondModeAttackTransparentCurve.Evaluate(stateExistTime / laserExistTime));
				warningRenderer.material.SetFloat("_Transparency", secondModeAttackTransparentCurve.Evaluate(stateExistTime / laserExistTime) * warningAttackMaxAlpha);
			}
			else
			{
				laserRenderer.widthMultiplier = Mathf.Max(0f, attackWidthCurve.Evaluate(stateExistTime / laserExistTime));
				shadowRenderer.widthMultiplier = Mathf.Max(0f, attackWidthCurve.Evaluate(stateExistTime / laserExistTime));
				laserRenderer.material.SetFloat("_Transparency", attackTransparentCurve.Evaluate(stateExistTime / laserExistTime));
				shadowRenderer.material.SetFloat("_Transparency", attackTransparentCurve.Evaluate(stateExistTime / laserExistTime));
			}
			if (stateExistTime < attackExistTime)
			{
				if (isSecondMode)
				{
					CamController.Inst.SetShock(shockParam);
				}
				Damage();
			}
			else if (!attackFinish)
			{
				if (isSecondMode)
				{
					SEMgr.Inst.spell1011End.PlaySE();
				}
				shadow.Hide();
				attackFinish = true;
				mainParticle.Stop();
			}
			if (stateExistTime > laserExistTime)
			{
				attackParticle.Stop();
				warningRenderer.enabled = false;
				warningShadowRenderer.enabled = false;
				shadowRenderer.enabled = false;
				laserRenderer.enabled = false;
			}
			break;
		}
		if (isSecondMode)
		{
			for (int l = 0; l < allNode.Count; l++)
			{
				if (state == LaserState.Attack)
				{
					if ((float)l < stopNode)
					{
						laserRenderer.SetPosition(l, Tool2D.GetLayerPoint(allNode[l]));
						warningRenderer.SetPosition(l, Tool2D.GetLayerPoint(stopPoint));
						warningShadowRenderer.SetPosition(l, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(allNode[l]), LayerCorrectType.Shadow));
						shadowRenderer.SetPosition(l, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(allNode[l]), LayerCorrectType.Shadow));
					}
					else if ((float)l == stopNode)
					{
						laserRenderer.SetPosition(l, Tool2D.GetLayerPoint(stopPoint));
						warningRenderer.SetPosition(l, Tool2D.GetLayerPoint(stopPoint));
						warningShadowRenderer.SetPosition(l, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(stopPoint), LayerCorrectType.Shadow));
						shadowRenderer.SetPosition(l, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(stopPoint), LayerCorrectType.Shadow));
					}
					else
					{
						laserRenderer.SetPosition(l, Tool2D.GetLayerPoint(stopPoint));
						warningRenderer.SetPosition(l, Tool2D.GetLayerPoint(allNode[l]));
						warningShadowRenderer.SetPosition(l, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(stopPoint), LayerCorrectType.Shadow));
						shadowRenderer.SetPosition(l, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(stopPoint), LayerCorrectType.Shadow));
					}
				}
				else
				{
					laserRenderer.SetPosition(l, Tool2D.GetLayerPoint(allNode[l]));
					warningRenderer.SetPosition(l, Tool2D.GetLayerPoint(allNode[l]));
					warningShadowRenderer.SetPosition(l, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(allNode[l]), LayerCorrectType.Shadow));
					shadowRenderer.SetPosition(l, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(allNode[l]), LayerCorrectType.Shadow));
				}
			}
		}
		else
		{
			laserRenderer.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - laserHeight)));
			laserRenderer.SetPosition(1, Tool2D.GetLayerPoint(endPoint + new Vector3(0f, 0f, 0f - laserHeight)));
			warningRenderer.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - laserHeight)));
			warningRenderer.SetPosition(1, Tool2D.GetLayerPoint(endPoint + new Vector3(0f, 0f, 0f - laserHeight)));
			warningShadowRenderer.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow));
			warningShadowRenderer.SetPosition(1, Tool2D.GetLayerPoint(endPoint, LayerCorrectType.Shadow));
			shadowRenderer.SetPosition(0, Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow));
			shadowRenderer.SetPosition(1, Tool2D.GetLayerPoint(endPoint, LayerCorrectType.Shadow));
		}
	}

	private void Damage()
	{
		attackCheckTimer -= Time.deltaTime;
		if (attackCheckTimer > 0f)
		{
			return;
		}
		attackCheckTimer = attackCheckInterval;
		if (isSecondMode)
		{
			for (int i = 0; (float)i < stopNode; i++)
			{
				UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(allNode[i], allNode[i + 1] - allNode[i], laserCheckRadius, (allNode[i + 1] - allNode[i]).magnitude + 0.1f, GameConst.Filter_Laser);
				for (int j = 0; j < array.Length; j++)
				{
					UnitDotsSyncSystem.RayCastHitResult rayCastHitResult = array[j];
					if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(rayCastHitResult.entity))
					{
						if (!attackedTarget.Contains(rayCastHitResult.entity))
						{
							attackedTarget.Add(rayCastHitResult.entity);
							attackedTime.Add(damageInterval);
							ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_Hit_2", array[j].point, 3f);
							TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(masterPpt.myEntity);
							info.damage = damage;
							info.teammateTakeDamageRatio = 3f;
							UnitDotsSyncSystem.AddTakeDamageRequest(rayCastHitResult.entity, info);
						}
					}
					else
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_Hit_2", array[j].point, 3f);
					}
				}
			}
			return;
		}
		UnitDotsSyncSystem.RayCastHitResult[] array2 = UnitDotsSyncSystem.SphereCastAll(base.transform.position + new Vector3(0f, 0f, 0f - laserHeight), diration, laserCheckRadius, (endPoint - base.transform.position).magnitude + 0.1f, GameConst.Filter_Laser);
		for (int k = 0; k < array2.Length; k++)
		{
			UnitDotsSyncSystem.RayCastHitResult rayCastHitResult2 = array2[k];
			if (!attackedTarget.Contains(rayCastHitResult2.entity))
			{
				attackedTarget.Add(rayCastHitResult2.entity);
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(rayCastHitResult2.entity))
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_Hit" + (GameMgr.IsHarmony_Static ? " H" : ""), array2[k].point, 3f);
					TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(masterPpt.myEntity);
					info2.damage = damage;
					info2.teammateTakeDamageRatio = 3f;
					UnitDotsSyncSystem.AddTakeDamageRequest(rayCastHitResult2.entity, info2);
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster54_Hit" + (GameMgr.IsHarmony_Static ? " H" : ""), array2[k].point, 3f);
				}
			}
		}
	}
}
