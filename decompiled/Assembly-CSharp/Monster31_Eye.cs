using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;

public class Monster31_Eye : MonoBehaviour
{
	private enum UnitState
	{
		HideIdle,
		Show,
		ShowIdle,
		Hide
	}

	public MeshRenderer eye_Renderer;

	public LineRenderer lr_Tentacle;

	public LineRenderer lr_Shadow;

	public int tentacleNodeCount;

	public float rootOffsetY;

	public VariableFloat endPointHeight;

	public float middle1Height;

	public float middle1OffsetX;

	public float middle2Height;

	public float middle2OffsetX;

	[Header("Eye")]
	public MeshRenderer mr_Eye;

	public Transform tsf_EyeShadow;

	public float eyeHoverDistance;

	[Header("Flex")]
	public VariableFloat lerpSpeed;

	public VariableFloat flexDelay;

	public VariableFloat flexAmplitude;

	[Header("Follow")]
	public float followExtension;

	public float followLerp;

	public GameObject go_Light;

	public float lightRadius;

	public float lightHalfAngle;

	public float lightEffectDuration;

	public float lightCheckInterval;

	[Header("和谐模式")]
	public Sprite sprite_H;

	private UnitState state;

	private Monster31 monster31;

	private Vector3 rootPoint;

	private Vector3 endPoint;

	private Vector3 currentMiddle1Offset;

	private Vector3 currentMiddle2Offset;

	private float currentLerp;

	private float flexDelayTimer;

	private Vector3 currentEndOffset;

	private Vector3 targetEndOffset;

	private Vector3 targetMiddle1Offset;

	private Vector3 targetMiddle2Offset;

	private float lightCheckIntervalTimer;

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public Vector3 EyePoint { get; private set; }

	private void Update()
	{
		if (lr_Tentacle.startColor != monster31.myPpt.BaseColor)
		{
			lr_Tentacle.startColor = monster31.myPpt.BaseColor;
			lr_Tentacle.endColor = monster31.myPpt.BaseColor;
			eye_Renderer.material.color = monster31.myPpt.BaseColor;
		}
		if (monster31.Monster31IsFrozen)
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
			if (!mr_Eye.gameObject.activeSelf)
			{
				mr_Eye.gameObject.SetActive(value: true);
				tsf_EyeShadow.gameObject.SetActive(value: true);
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
				LocalTransform componentData = monster31.GetComponentData<LocalTransform>(monster31.Monster31TargetEntity);
				targetEndOffset = Tool2D.IgnoreZV2ToV1Normal(componentData.Position, endPoint) * followExtension;
				if (componentData.Position.x > rootPoint.x)
				{
					targetMiddle1Offset = new Vector3(0f - Mathf.Abs(currentMiddle1Offset.x), 0f, currentMiddle1Offset.z);
					targetMiddle2Offset = new Vector3(0f - Mathf.Abs(currentMiddle2Offset.x), 0f, currentMiddle1Offset.z);
				}
				else
				{
					targetMiddle1Offset = new Vector3(Mathf.Abs(currentMiddle1Offset.x), 0f, currentMiddle1Offset.z);
					targetMiddle2Offset = new Vector3(Mathf.Abs(currentMiddle2Offset.x), 0f, currentMiddle1Offset.z);
				}
				if (!go_Light.activeSelf)
				{
					go_Light.SetActive(value: true);
				}
				go_Light.transform.position = Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(EyePoint));
				go_Light.transform.rotation = mr_Eye.transform.rotation;
				lightCheckIntervalTimer += Time.deltaTime;
				if (!(lightCheckIntervalTimer >= lightCheckInterval))
				{
					break;
				}
				lightCheckIntervalTimer = 0f;
				UnitDotsSyncSystem.GetCollidersInRangeWithAngle(EyePoint, lightRadius, go_Light.transform.up, lightHalfAngle, GameConst.Filter_Friendly, results);
				for (int i = 0; i < results.Count; i++)
				{
					UnitProperty_Dots componentData2 = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(results[i].entity);
					componentData2.SetReverseMove(lightEffectDuration);
					UnitDotsSyncSystem.SetComponentData(componentData2, results[i].entity);
					if (results[i].entity == PlayerMgr.Inst.PlayerEtt)
					{
						PlayerMgr.Inst.PlayerCtrller.myPpt.SetReverseMove(lightEffectDuration);
						break;
					}
				}
			}
			else
			{
				targetEndOffset = Vector3.zero;
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
				mr_Eye.gameObject.SetActive(value: false);
				tsf_EyeShadow.gameObject.SetActive(value: false);
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
		currentEndOffset = Vector3.Lerp(currentEndOffset, targetEndOffset, followLerp * Time.deltaTime);
		currentMiddle1Offset = Vector3.Lerp(currentMiddle1Offset, targetMiddle1Offset, followLerp * Time.deltaTime);
		currentMiddle2Offset = Vector3.Lerp(currentMiddle2Offset, targetMiddle2Offset, followLerp * Time.deltaTime);
		float z = Tool2D.GetLayerPoint(endPoint + currentEndOffset).z;
		for (int j = 0; j < tentacleNodeCount; j++)
		{
			Vector3 vector = GeneralTool.CubicBezierCurve(rootPoint + monster31.myPpt.Tsf_BeHit.localPosition, endPoint + currentMiddle1Offset, endPoint + currentMiddle2Offset, endPoint + currentEndOffset, (float)j / ((float)tentacleNodeCount - 1f));
			lr_Tentacle.SetPosition(j, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(vector), z));
		}
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(rootPoint + monster31.myPpt.Tsf_BeHit.localPosition, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(endPoint + currentEndOffset, 1.05f));
		EyePoint = GeneralTool.CubicBezierCurve(rootPoint + monster31.myPpt.Tsf_BeHit.localPosition, endPoint + currentMiddle1Offset, endPoint + currentMiddle2Offset, endPoint + currentEndOffset, currentLerp);
		Vector3 v = GeneralTool.CubicBezierCurve(rootPoint + monster31.myPpt.Tsf_BeHit.localPosition, endPoint + currentMiddle1Offset, endPoint + currentMiddle2Offset, endPoint + currentEndOffset, currentLerp - 0.05f);
		mr_Eye.transform.position = Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(EyePoint), z - 0.01f);
		tsf_EyeShadow.position = Tool2D.IgnoreZPoint(EyePoint, 1.05f);
		if (EyePoint.y == v.y)
		{
			Vector3 v2 = new Vector3(EyePoint.x, EyePoint.z, 0f);
			Vector3 v3 = new Vector3(v.x, v.z, 0f);
			mr_Eye.transform.up = Tool2D.IgnoreZV2ToV1Normal(v2, v3);
		}
		else
		{
			mr_Eye.transform.up = Tool2D.IgnoreZV2ToV1Normal(EyePoint, v);
		}
		tsf_EyeShadow.transform.rotation = mr_Eye.transform.rotation;
		if (lr_Tentacle.startColor != monster31.myPpt.BaseColor)
		{
			lr_Tentacle.startColor = monster31.myPpt.BaseColor;
			lr_Tentacle.endColor = monster31.myPpt.BaseColor;
			mr_Eye.material.color = monster31.myPpt.BaseColor;
		}
	}

	public void SingleInitial(Monster31 monster31)
	{
		this.monster31 = monster31;
		lr_Tentacle.positionCount = tentacleNodeCount;
		lr_Shadow.positionCount = 2;
		float value = Random.Range(0f, 9999f);
		lr_Tentacle.material.SetFloat("_TimeOffset", value);
		lr_Shadow.material.SetFloat("_TimeOffset", value);
		if (GameMgr.IsMobile_Static)
		{
			go_Light.transform.localScale = Vector3.one * 0.66f;
			lightRadius = 4f;
		}
	}

	public void EveryInitial()
	{
		state = UnitState.HideIdle;
		currentLerp = 0f;
		flexDelayTimer = 0f;
		lr_Tentacle.material.SetFloat("_Offset", 1f - currentLerp);
		lr_Shadow.material.SetFloat("_Offset", 1f - currentLerp);
		mr_Eye.gameObject.SetActive(value: false);
		tsf_EyeShadow.gameObject.SetActive(value: false);
		go_Light.SetActive(value: false);
		for (int i = 0; i < tentacleNodeCount; i++)
		{
			lr_Tentacle.SetPosition(i, Vector3.zero);
		}
		lr_Shadow.SetPosition(0, Vector3.zero);
		lr_Shadow.SetPosition(1, Vector3.zero);
		if (GameMgr.IsHarmony_Static)
		{
			eye_Renderer.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_H.texture);
		}
	}

	public void Show()
	{
		state = UnitState.Show;
		Tool2D.GetDir();
		rootPoint = monster31.transform.position + new Vector3(0f, rootOffsetY, 0f);
		endPoint = rootPoint + new Vector3(0f, 0f - rootOffsetY, 0f - endPointHeight.RandomResult());
		int num = ((Random.Range(0, 2) == 0) ? 1 : (-1));
		targetMiddle1Offset = new Vector3((float)(-num) * middle1OffsetX, 0f, 0f - middle1Height);
		targetMiddle2Offset = new Vector3((float)(-num) * middle2OffsetX, 0f, 0f - middle2Height);
		currentMiddle1Offset = targetMiddle1Offset;
		currentMiddle2Offset = targetMiddle2Offset;
		lerpSpeed.RandomResult();
		flexDelay.RandomResult();
		flexDelayTimer = 0f;
		lr_Tentacle.material.SetFloat("_Offset", 1f);
		currentEndOffset = Vector3.zero;
	}

	public void Hide()
	{
		state = UnitState.Hide;
		flexDelay.RandomResult();
		flexDelayTimer = 0f;
		targetEndOffset = Vector3.zero;
		if (go_Light.activeSelf)
		{
			go_Light.SetActive(value: false);
		}
	}
}
