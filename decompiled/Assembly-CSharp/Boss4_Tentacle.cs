using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Boss4_Tentacle : UnitBase
{
	private enum UnitState
	{
		BornAppear,
		Show,
		ShowIdle,
		Struggle
	}

	[Space(50f)]
	public LineRenderer lr_Tentacle;

	public LineRenderer lr_Shadow;

	public float rootHeight;

	public int tentacleNodeCount;

	public float initialAngle;

	public VariableFloat bodyAngle;

	public float oneSegmentLength;

	public float appearSpeed;

	[Header("Tangle")]
	public GameObject go_Tangled;

	public float tangleCheckDistance;

	public float tangleMiddlePoint1Height;

	public float tangleMiddlePoint2Height;

	public float tangleToTargetSpeed;

	public float tangleToSelfSpeed;

	public float tangleChangeFormSpeed;

	public float tangleIdleDistance;

	public float tangleStruggleExtraDistance;

	public float tangleStruggleReboundDistance;

	public float tangleStruggleSpeedRatio;

	private float totalLength;

	private UnitState state;

	private Boss4 boss4;

	private Vector3 rootPoint;

	private float appearLerp;

	private Vector3 currentEndPoint;

	private Vector3 struggleReboundPoint;

	private float targetFormLerp;

	private float currentFormLerp;

	private bool isTangled;

	private bool needDrag;

	private Vector3 RootPoint => rootPoint + myPpt.Tsf_BeHit.localPosition;

	public override void SingleInitialCallback()
	{
		lr_Tentacle.positionCount = tentacleNodeCount;
		lr_Shadow.positionCount = tentacleNodeCount;
		totalLength = (float)tentacleNodeCount * oneSegmentLength;
		float value = UnityEngine.Random.Range(0f, 9999f);
		lr_Tentacle.material.SetFloat("_TimeOffset", value);
		lr_Shadow.material.SetFloat("_TimeOffset", value);
		lr_Tentacle.material.SetFloat("_Offset", 1f);
		if (GameMgr.IsMobile_Static)
		{
			tangleToSelfSpeed *= 0.6f;
		}
	}

	public override void EveryInitialCallback()
	{
		state = UnitState.BornAppear;
		appearLerp = 0f;
		rootPoint = base.transform.position + new Vector3(0f, 0f, 0f - rootHeight);
		targetFormLerp = 0f;
		currentFormLerp = 0f;
		isTangled = false;
		for (int i = 0; i < tentacleNodeCount; i++)
		{
			lr_Tentacle.SetPosition(i, Vector3.zero);
			lr_Tentacle.SetPosition(i, Vector3.zero);
		}
		currentEndPoint = base.transform.position;
		go_Tangled.SetActive(value: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
	}

	private void FixedUpdate()
	{
		if (needDrag && EntityIsValid(targetEntity))
		{
			LocalTransform componentData = GetComponentData<LocalTransform>(targetEntity);
			componentData.Position += (float3)(Tool2D.IgnoreZV2ToV1Normal(base.transform.position, componentData.Position) * tangleToSelfSpeed * Time.deltaTime);
			SetComponentData(componentData, targetEntity);
		}
		needDrag = false;
	}

	public override void Update()
	{
		if (lr_Tentacle.startColor != myPpt.BaseColor)
		{
			lr_Tentacle.startColor = myPpt.BaseColor;
			lr_Tentacle.endColor = myPpt.BaseColor;
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case UnitState.BornAppear:
			appearLerp += appearSpeed * Time.deltaTime;
			if (appearLerp >= 1f)
			{
				appearLerp = 1f;
				state = UnitState.ShowIdle;
			}
			lr_Tentacle.material.SetFloat("_Offset", 1f - appearLerp);
			lr_Shadow.material.SetFloat("_Offset", 1f - appearLerp);
			break;
		case UnitState.ShowIdle:
			if (base.HaveTarget)
			{
				Vector3 targetPoint = base.TargetPoint;
				if (isTangled)
				{
					needDrag = false;
					if ((base.transform.position - targetPoint).sqrMagnitude > tangleIdleDistance * tangleIdleDistance)
					{
						currentEndPoint = targetPoint;
						needDrag = true;
					}
					if ((base.transform.position - targetPoint).sqrMagnitude < (totalLength + tangleStruggleExtraDistance) * (totalLength + tangleStruggleExtraDistance))
					{
						currentEndPoint = targetPoint;
						go_Tangled.transform.position = Tool2D.GetLayerPoint(targetPoint);
					}
					else
					{
						isTangled = false;
						go_Tangled.SetActive(value: false);
						state = UnitState.Struggle;
						Vector3 normalized = (targetPoint - base.transform.position).normalized;
						struggleReboundPoint = base.transform.position + normalized * totalLength - normalized * tangleStruggleReboundDistance;
					}
				}
				else
				{
					if ((base.transform.position - targetPoint).sqrMagnitude < totalLength * totalLength)
					{
						currentEndPoint = Vector3.MoveTowards(currentEndPoint, targetPoint, tangleToTargetSpeed * Time.deltaTime);
						if (currentEndPoint == targetPoint && currentFormLerp == 1f)
						{
							isTangled = true;
							go_Tangled.SetActive(value: true);
							go_Tangled.transform.position = Tool2D.GetLayerPoint(targetPoint);
						}
					}
					else
					{
						currentEndPoint = Vector3.MoveTowards(currentEndPoint, base.transform.position + (targetPoint - base.transform.position).normalized * totalLength, tangleToTargetSpeed * Time.deltaTime);
					}
					checkTargetIntervalTimer += Time.deltaTime;
					if (ToTargetDistanceSqr() > tangleCheckDistance * tangleCheckDistance)
					{
						targetEntity = Entity.Null;
					}
				}
				targetFormLerp = 1f;
			}
			else
			{
				targetFormLerp = 0f;
				checkTargetIntervalTimer += Time.deltaTime;
				if (checkTargetIntervalTimer >= 1f)
				{
					GetNearestTarget();
					if (base.HaveTarget && ToTargetDistanceSqr() > tangleCheckDistance * tangleCheckDistance)
					{
						targetEntity = Entity.Null;
					}
				}
			}
			currentFormLerp = Mathf.MoveTowards(currentFormLerp, targetFormLerp, tangleChangeFormSpeed * Time.deltaTime);
			break;
		case UnitState.Struggle:
			currentEndPoint = Vector3.MoveTowards(currentEndPoint, struggleReboundPoint, tangleToTargetSpeed * tangleStruggleSpeedRatio * Time.deltaTime);
			if (currentEndPoint == struggleReboundPoint)
			{
				state = UnitState.ShowIdle;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
		float num = initialAngle;
		Vector3 a = RootPoint;
		Vector3 b = RootPoint;
		Vector3 v = RootPoint + new Vector3(0f, 0f, 0f - tangleMiddlePoint1Height);
		Vector3 v2 = currentEndPoint + new Vector3(0f, 0f, 0f - tangleMiddlePoint2Height) + (RootPoint - currentEndPoint).normalized * totalLength / 2f;
		for (int i = 0; i < tentacleNodeCount; i++)
		{
			if (i > 0)
			{
				num += Mathf.Lerp(bodyAngle.value1, bodyAngle.value2, (float)i / ((float)tentacleNodeCount - 1f));
				Vector3 vector = Tool2D.GetDir(num) * oneSegmentLength;
				vector.z = 0f - vector.y;
				vector.y = 0f;
				a += vector;
				b = GeneralTool.CubicBezierCurve(RootPoint, v, v2, currentEndPoint, (float)i / ((float)tentacleNodeCount - 1f));
			}
			Vector3 v3 = Vector3.Lerp(a, b, currentFormLerp);
			lr_Tentacle.SetPosition(i, Tool2D.GetLayerPoint(v3));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(v3, 1.05f));
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		boss4.TentacleDead();
	}

	public void SetMother(Boss4 boss4)
	{
		this.boss4 = boss4;
	}
}
