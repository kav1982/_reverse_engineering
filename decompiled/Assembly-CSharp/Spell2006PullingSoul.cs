using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Spell2006PullingSoul : MonoBehaviour
{
	private enum Spell2006PullTrailType
	{
		OneCurve,
		DoubleInsideCurve,
		InsideAndOutSideCurve
	}

	public class Spell2006PullingSoulBaker : Baker<Spell2006PullingSoul>
	{
		public override void Bake(Spell2006PullingSoul authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			Spell2006PullingSoulComponentData component = default(Spell2006PullingSoulComponentData);
			AddComponent(entity, in component);
		}
	}

	public GameObject TrailObj;

	private Transform startTransform;

	private Transform endTransform;

	private Entity startEntity;

	private Entity endEntity;

	private Vector3 startLastFramePoint = Vector3.zero;

	private Vector3 endLastFramePoint = Vector3.zero;

	private Spell2006PullTrailType currentType;

	private Vector3 firstLerpPoint = Vector3.zero;

	private Vector3 SecondLerpPoint = Vector3.zero;

	public float OneCurvePointMaxShiftDistance;

	public float DoubleInsidePointMaxShiftDistance;

	public float OutsidePointMaxShiftDistance;

	public float OutsidePointAngleShiftRange;

	public int TrailLerpTotalPointCount;

	public VariableFloat TrailLerpTimeRange;

	private float trailLerpTime;

	private float trailLerpTimer;

	private float currentLerpPoint;

	public TrailRenderer TrailEffect;

	public TrailRenderer TrailEffect2;

	public static int ActivedTrailCount;

	private void OnEnable()
	{
		startTransform = null;
		endTransform = null;
		startEntity = Entity.Null;
		endEntity = Entity.Null;
		startLastFramePoint = Vector3.zero;
		endLastFramePoint = Vector3.zero;
		trailLerpTime = 0f;
		trailLerpTimer = 0f;
		currentLerpPoint = 0f;
		TrailEffect.enabled = false;
		TrailEffect2.enabled = false;
		ActivedTrailCount++;
	}

	private void OnDisable()
	{
		ActivedTrailCount--;
	}

	private void Update()
	{
		UpdateCurrentPointState();
		UpdateStartAndEndPoint();
	}

	private void UpdateCurrentPointState()
	{
		trailLerpTimer += Time.deltaTime;
		if (!(trailLerpTimer < trailLerpTime))
		{
			switch (currentType)
			{
			case Spell2006PullTrailType.OneCurve:
				TrailEffect.transform.position = GeneralTool.QuadraticBezierCurve(startLastFramePoint, firstLerpPoint, endLastFramePoint, Mathf.Min(1f, currentLerpPoint / (float)TrailLerpTotalPointCount));
				break;
			case Spell2006PullTrailType.DoubleInsideCurve:
			case Spell2006PullTrailType.InsideAndOutSideCurve:
				TrailEffect.transform.position = GeneralTool.CubicBezierCurve(startLastFramePoint, firstLerpPoint, SecondLerpPoint, endLastFramePoint, Mathf.Min(1f, currentLerpPoint / (float)TrailLerpTotalPointCount));
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			currentLerpPoint += 1f / (trailLerpTime * (float)TrailLerpTotalPointCount) * 11f * Time.deltaTime;
		}
	}

	private void UpdateStartAndEndPoint()
	{
		if ((bool)startTransform && startTransform.gameObject.activeInHierarchy)
		{
			startLastFramePoint = startTransform.position;
		}
		if ((bool)endTransform && endTransform.gameObject.activeInHierarchy)
		{
			endLastFramePoint = endTransform.position;
		}
		if (UnitDotsSyncSystem.EntityIsValid(endEntity))
		{
			endLastFramePoint = UnitDotsSyncSystem.GetComponentData<LocalTransform>(endEntity).Position;
		}
		else
		{
			endEntity = Entity.Null;
		}
	}

	public void InitialLineData(Transform startTrans, Transform endTrans, float lerpTime, Entity targetEntity)
	{
		startTransform = startTrans;
		endTransform = endTrans;
		endEntity = targetEntity;
		startLastFramePoint = startTrans.position;
		if (endTrans == null)
		{
			endLastFramePoint = UnitDotsSyncSystem.GetComponentData<LocalTransform>(endEntity).Position;
		}
		else
		{
			endLastFramePoint = endTrans.position;
		}
		trailLerpTime = lerpTime / (float)TrailLerpTotalPointCount;
		TrailEffect.Clear();
		TrailEffect.enabled = true;
		TrailEffect.transform.position = startLastFramePoint;
		TrailEffect2.Clear();
		TrailEffect2.enabled = true;
		TrailEffect2.transform.position = startLastFramePoint + new Vector3(0f, 0f, 0.5f);
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, trailLerpTime * (float)TrailLerpTotalPointCount + 0.8f);
		int num = (int)(currentType = (Spell2006PullTrailType)UnityEngine.Random.Range(0, 3));
		switch (currentType)
		{
		case Spell2006PullTrailType.OneCurve:
		{
			Vector3 vector3 = startLastFramePoint + (endLastFramePoint - startLastFramePoint) * UnityEngine.Random.Range(0f, 1f);
			firstLerpPoint = vector3 + Tool2D.GetDir((endLastFramePoint - firstLerpPoint).normalized, 90f) * UnityEngine.Random.Range(-1f, 1f) * OneCurvePointMaxShiftDistance;
			break;
		}
		case Spell2006PullTrailType.DoubleInsideCurve:
		{
			Vector3 vector2 = startLastFramePoint + (endLastFramePoint - startLastFramePoint) * UnityEngine.Random.Range(0.05f, 0.95f);
			firstLerpPoint = vector2 + Tool2D.GetDir((endLastFramePoint - firstLerpPoint).normalized, 90f) * UnityEngine.Random.Range(-1f, 1f) * DoubleInsidePointMaxShiftDistance;
			vector2 = startLastFramePoint + (endLastFramePoint - startLastFramePoint) * UnityEngine.Random.Range(0.05f, 0.95f);
			SecondLerpPoint = vector2 + Tool2D.GetDir((endLastFramePoint - firstLerpPoint).normalized, 90f) * UnityEngine.Random.Range(-1f, 1f) * DoubleInsidePointMaxShiftDistance;
			break;
		}
		case Spell2006PullTrailType.InsideAndOutSideCurve:
		{
			Vector3 vector = startLastFramePoint + (endLastFramePoint - startLastFramePoint) * UnityEngine.Random.Range(0.05f, 0.95f);
			firstLerpPoint = vector + Tool2D.GetDir((endLastFramePoint - firstLerpPoint).normalized, 90f) * UnityEngine.Random.Range(-1f, 1f) * DoubleInsidePointMaxShiftDistance;
			SecondLerpPoint = endLastFramePoint + Tool2D.GetDir(endLastFramePoint - startLastFramePoint, UnityEngine.Random.Range((0f - OutsidePointAngleShiftRange) / 2f, OutsidePointAngleShiftRange / 2f)) * UnityEngine.Random.Range(0.02f, 0.6f);
			break;
		}
		}
	}
}
