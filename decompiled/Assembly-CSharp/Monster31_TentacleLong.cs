using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Monster31_TentacleLong : MonoBehaviour
{
	private enum UnitState
	{
		HideIdle,
		Show,
		ShowIdle,
		Hide
	}

	public LineRenderer lr_Tentacle;

	public LineRenderer lr_Shadow;

	public int tentacleNodeCount;

	public float rootOffsetY;

	public float initialAngle;

	public VariableFloat bodyAngle;

	public float oneSegmentLength;

	[Header("Flex")]
	public VariableFloat lerpSpeed;

	public VariableFloat flexDelay;

	public VariableFloat flexAmplitude;

	[Header("Tangle")]
	public GameObject go_Tangled;

	public float tangleMiddlePoint1Height;

	public float tangleMiddlePoint2Height;

	public float tangleGetDirExtraDistance;

	public float tangleToTargetSpeed;

	public float tangleToSelfSpeed;

	public float tangleChangeFormSpeed;

	public float hideWhenTargetDis;

	private UnitState state;

	private Monster31 monster31;

	private Vector3 rootPoint;

	private float currentLerp;

	private float flexDelayTimer;

	private float totalLength;

	private Vector3 targetTanglePoint;

	private Vector3 currentTanglePoint;

	private float targetFormLerp;

	private float currentFormLerp;

	private bool isTangled;

	private bool needDrag;

	private void FixedUpdate()
	{
		if (needDrag && monster31.Monster31HaveTarget)
		{
			LocalTransform componentData = monster31.GetComponentData<LocalTransform>(monster31.Monster31TargetEntity);
			componentData.Position += (float3)(Tool2D.IgnoreZV2ToV1Normal(base.transform.position, componentData.Position) * tangleToSelfSpeed * Time.deltaTime);
			monster31.SetComponentData(componentData, monster31.Monster31TargetEntity);
		}
		needDrag = false;
	}

	private void Update()
	{
		if (lr_Tentacle.startColor != monster31.myPpt.BaseColor)
		{
			lr_Tentacle.startColor = monster31.myPpt.BaseColor;
			lr_Tentacle.endColor = monster31.myPpt.BaseColor;
		}
		if (state != UnitState.ShowIdle && monster31.Monster31IsFrozen)
		{
			return;
		}
		switch (state)
		{
		case UnitState.Show:
		{
			if (flexDelayTimer < flexDelay.result)
			{
				flexDelayTimer += Time.deltaTime;
				break;
			}
			currentLerp = Mathf.MoveTowards(currentLerp, 1f, lerpSpeed.result * Time.deltaTime);
			float value2 = Mathf.Lerp(flexAmplitude.value1, flexAmplitude.value2, currentLerp);
			lr_Tentacle.material.SetFloat("_WaveAmplitude", value2);
			lr_Shadow.material.SetFloat("_WaveAmplitude", value2);
			if (currentLerp == 1f)
			{
				state = UnitState.ShowIdle;
			}
			break;
		}
		case UnitState.ShowIdle:
			if (monster31.Monster31HaveTarget)
			{
				Vector3 vector = monster31.GetComponentData<LocalTransform>(monster31.Monster31TargetEntity).Position;
				if (isTangled)
				{
					needDrag = false;
					if (!monster31.Monster31IsFrozen)
					{
						needDrag = true;
					}
					if ((monster31.transform.position - vector).sqrMagnitude < hideWhenTargetDis * hideWhenTargetDis)
					{
						monster31.state = Monster31.Monster31State.Hide;
						return;
					}
					if ((rootPoint - vector).sqrMagnitude < (totalLength + tangleGetDirExtraDistance) * (totalLength + tangleGetDirExtraDistance))
					{
						targetTanglePoint = vector;
						currentTanglePoint = targetTanglePoint;
						go_Tangled.transform.position = Tool2D.GetLayerPoint(vector);
					}
					else
					{
						isTangled = false;
						go_Tangled.SetActive(value: false);
						if (!monster31.Monster31IsFrozen)
						{
							targetTanglePoint = rootPoint + (vector - rootPoint).normalized * totalLength;
						}
					}
				}
				else if (!monster31.Monster31IsFrozen)
				{
					if ((rootPoint - vector).sqrMagnitude < totalLength * totalLength)
					{
						targetTanglePoint = vector;
					}
					else
					{
						targetTanglePoint = rootPoint + (vector - rootPoint).normalized * totalLength;
					}
				}
				targetFormLerp = 1f;
			}
			else
			{
				targetTanglePoint = rootPoint;
				targetFormLerp = 0f;
			}
			break;
		case UnitState.Hide:
		{
			if (flexDelayTimer < flexDelay.result)
			{
				flexDelayTimer += Time.deltaTime;
				break;
			}
			currentLerp = Mathf.MoveTowards(currentLerp, 0f, lerpSpeed.result * Time.deltaTime);
			float value = Mathf.Lerp(flexAmplitude.value1, flexAmplitude.value2, currentLerp);
			lr_Tentacle.material.SetFloat("_WaveAmplitude", value);
			lr_Shadow.material.SetFloat("_WaveAmplitude", value);
			if (currentLerp == 0f)
			{
				state = UnitState.HideIdle;
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		case UnitState.HideIdle:
			break;
		}
		lr_Tentacle.material.SetFloat("_Offset", 1f - currentLerp);
		lr_Shadow.material.SetFloat("_Offset", 1f - currentLerp);
		currentTanglePoint = Vector3.MoveTowards(currentTanglePoint, targetTanglePoint, tangleToTargetSpeed * Time.deltaTime);
		if (monster31.Monster31HaveTarget)
		{
			Vector3 vector2 = monster31.GetComponentData<LocalTransform>(monster31.Monster31TargetEntity).Position;
			if (currentTanglePoint == vector2)
			{
				isTangled = true;
				go_Tangled.SetActive(value: true);
				go_Tangled.transform.position = Tool2D.GetLayerPoint(vector2);
			}
		}
		currentFormLerp = Mathf.MoveTowards(currentFormLerp, targetFormLerp, tangleChangeFormSpeed * Time.deltaTime);
		float num = initialAngle;
		Vector3 a = rootPoint + monster31.myPpt.Tsf_BeHit.localPosition;
		Vector3 b = rootPoint + monster31.myPpt.Tsf_BeHit.localPosition;
		Vector3 v = Vector3.zero;
		Vector3 v2 = rootPoint + new Vector3(0f, 0f, 0f - tangleMiddlePoint1Height);
		Vector3 v3 = rootPoint + new Vector3(0f, 0f, 0f - tangleMiddlePoint2Height) + (rootPoint - currentTanglePoint).normalized * totalLength / 2f;
		for (int i = 0; i < tentacleNodeCount; i++)
		{
			if (i > 0)
			{
				num += Mathf.Lerp(bodyAngle.value1, bodyAngle.value2, (float)i / ((float)tentacleNodeCount - 1f));
				Vector3 vector3 = Tool2D.GetDir(num) * oneSegmentLength;
				vector3.z = 0f - vector3.y;
				vector3.y = 0f;
				a += vector3;
				b = GeneralTool.CubicBezierCurve(rootPoint, v2, v3, currentTanglePoint, (float)i / ((float)tentacleNodeCount - 1f));
			}
			v = Vector3.Lerp(a, b, currentFormLerp);
			lr_Tentacle.SetPosition(i, Tool2D.GetLayerPoint(v));
		}
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(rootPoint + monster31.myPpt.Tsf_BeHit.localPosition, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(v, 1.05f));
		if (lr_Tentacle.startColor != monster31.myPpt.BaseColor)
		{
			lr_Tentacle.startColor = monster31.myPpt.BaseColor;
			lr_Tentacle.endColor = monster31.myPpt.BaseColor;
		}
	}

	public void SingleInitial(Monster31 monster31)
	{
		this.monster31 = monster31;
		lr_Tentacle.positionCount = tentacleNodeCount;
		lr_Shadow.positionCount = 2;
		totalLength = (float)tentacleNodeCount * oneSegmentLength;
		float value = UnityEngine.Random.Range(0f, 9999f);
		lr_Tentacle.material.SetFloat("_TimeOffset", value);
		lr_Shadow.material.SetFloat("_TimeOffset", value);
		if (GameMgr.IsMobile_Static)
		{
			tangleToSelfSpeed *= 0.6f;
		}
	}

	public void EveryInitial()
	{
		state = UnitState.HideIdle;
		currentLerp = 0f;
		flexDelayTimer = 0f;
		targetFormLerp = 0f;
		currentFormLerp = 0f;
		isTangled = false;
		lr_Tentacle.material.SetFloat("_Offset", 1f - currentLerp);
		lr_Shadow.material.SetFloat("_Offset", 1f - currentLerp);
		go_Tangled.SetActive(value: false);
		targetTanglePoint = rootPoint;
		for (int i = 0; i < tentacleNodeCount; i++)
		{
			lr_Tentacle.SetPosition(i, Vector3.zero);
		}
		lr_Shadow.SetPosition(0, Vector3.zero);
		lr_Shadow.SetPosition(1, Vector3.zero);
	}

	public void Show()
	{
		state = UnitState.Show;
		rootPoint = monster31.transform.position + new Vector3(0f, rootOffsetY, 0f);
		currentTanglePoint = rootPoint;
		targetTanglePoint = currentTanglePoint;
		lerpSpeed.RandomResult();
		flexDelay.RandomResult();
		flexDelayTimer = 0f;
		lr_Tentacle.material.SetFloat("_Offset", 1f);
	}

	public void Hide()
	{
		state = UnitState.Hide;
		flexDelay.RandomResult();
		flexDelayTimer = 0f;
		targetTanglePoint = rootPoint;
		targetFormLerp = 0f;
		isTangled = false;
		go_Tangled.SetActive(value: false);
	}
}
