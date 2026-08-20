using UnityEngine;

public class Spell3101ThunderChain : MonoBehaviour
{
	private LineRenderer trail;

	private LineRenderer shadowTrail;

	private GameObject vfxModel;

	public int linePointCount;

	[Min(0f)]
	public float lightningChainFloatingSpeedDownratio;

	private float originFloatingSpeed;

	private float dissolveProcess;

	public Vector3 chainBaseHeight;

	public Vector3 trailZaxisHeightShift;

	public float dissolveSpeed;

	private Transform sourceTransform;

	private Vector3 sourcePosition = Vector3.zero;

	private Transform targetTransform;

	private Vector3 targetPosition = Vector3.zero;

	private Vector3 lerppos1ShiftDirection = Vector3.zero;

	private Vector3 lerpPos = Vector3.zero;

	private Vector3 lerpPos2 = Vector3.zero;

	public Vector2 lerpSpeed;

	private float currentlerpSpeed;

	public float lerpposShiftSpeedDown;

	private float chainFlotingScale;

	private BezierCurvePointData point1Data;

	private BezierCurvePointData point2Data;

	public Vector2 Type2lerpRatio;

	public Vector2 Type2PercentInRange;

	public Vector2 Type2PointPosRandomness;

	public Vector2 Type2EffectScale;

	private void Update()
	{
		point1Data.Type2SetAndLerpToTargetPoint(sourcePosition, targetPosition);
		point2Data.Type2SetAndLerpToTargetPoint(sourcePosition, targetPosition);
		currentlerpSpeed = Mathf.Lerp(currentlerpSpeed, 0f, Time.deltaTime * lerpposShiftSpeedDown);
		chainFlotingScale = Mathf.Lerp(chainFlotingScale, 0.5f, Time.deltaTime * lerpposShiftSpeedDown);
		for (int i = 0; i < linePointCount; i++)
		{
			Vector3 vector = GeneralTool.CubicBezierCurve(sourcePosition, point1Data.currentPosition, point2Data.currentPosition, targetPosition, (float)i / ((float)linePointCount - 1f));
			trail.SetPosition(i, Tool2D.GetLayerPoint(vector) + trailZaxisHeightShift);
			shadowTrail.SetPosition(i, Tool2D.IgnoreZPoint(vector, 1.05f));
		}
		trail.material.SetFloat("_Speed", Mathf.Lerp(originFloatingSpeed, 0f, lightningChainFloatingSpeedDownratio * Random.Range(0.9f, 1.1f) * Time.deltaTime));
		trail.material.SetVector("_LightingShiftScale", new Vector4(chainFlotingScale, 0f, 0f, 0f));
		trail.material.SetFloat("_DissolveProcess", -0.5f + dissolveProcess);
		shadowTrail.material.SetFloat("_DissolveProcess", -0.5f + dissolveProcess);
		dissolveProcess += Time.deltaTime * dissolveSpeed;
	}

	private void LateUpdate()
	{
		if (base.gameObject != null && base.gameObject.activeInHierarchy)
		{
			if (sourceTransform != null && sourceTransform.gameObject != null && sourceTransform.gameObject.activeInHierarchy)
			{
				sourcePosition = sourceTransform.position;
			}
			else
			{
				sourceTransform = null;
			}
			if (targetTransform != null && targetTransform.gameObject != null && targetTransform.gameObject.activeInHierarchy)
			{
				targetPosition = targetTransform.position;
			}
			else
			{
				targetTransform = null;
			}
		}
	}

	public void SetChainTargetTransform(Transform casterTransform, Transform targetUnitTransform)
	{
		sourceTransform = casterTransform;
		sourcePosition = sourceTransform.position;
		targetTransform = targetUnitTransform;
		targetPosition = targetTransform.position;
	}

	public void SetTrailMaterial(LineRenderer Trail)
	{
		Material material = Trail.material;
		material = (Trail.material = Object.Instantiate(material));
	}

	public void SetChainWidth(float width)
	{
		trail.startWidth = width;
	}

	public void setColorType()
	{
		vfxModel = GetComponent<EffectController>().ECGetCurrentEffect();
		vfxModel.SetActive(value: false);
		trail = vfxModel.GetComponent<LineRenderer>();
		shadowTrail = vfxModel.transform.Find("Shadow").gameObject.GetComponent<LineRenderer>();
		point1Data = new BezierCurvePointData();
		point2Data = new BezierCurvePointData();
		point1Data.moveLerpSpeed = Random.Range(Type2lerpRatio.x, Type2lerpRatio.y);
		point1Data.percentInRange = Random.Range(0.5f, 1f);
		point1Data.currentPosition = sourcePosition + (targetPosition - sourcePosition) * Random.Range(Type2PointPosRandomness.x, Type2PointPosRandomness.y) + Tool2D.IgnoreZPoint(Random.insideUnitSphere) * Random.Range(Type2EffectScale.x, Type2EffectScale.y);
		point1Data.moveLerpSpeed = Random.Range(Type2lerpRatio.x, Type2lerpRatio.y);
		point2Data.percentInRange = Random.Range(0f, 0.5f);
		point2Data.currentPosition = sourcePosition + (targetPosition - sourcePosition) * Random.Range(Type2PointPosRandomness.x, Type2PointPosRandomness.y) + Tool2D.IgnoreZPoint(Random.insideUnitSphere) * Random.Range(Type2EffectScale.x, Type2EffectScale.y);
		SetTrailMaterial(shadowTrail);
		dissolveProcess = 0f;
		trail.positionCount = linePointCount;
		shadowTrail.positionCount = linePointCount;
		originFloatingSpeed = 2.5f;
		chainFlotingScale = 2.2f;
		trail.startWidth = 0f;
		trail.material.SetFloat("_Speed", 5f);
		currentlerpSpeed = Random.Range(lerpSpeed.x, lerpSpeed.y);
		for (int i = 0; i < trail.positionCount; i++)
		{
			trail.SetPosition(i, Vector3.zero);
		}
		lerpPos = Vector3.zero;
		lerppos1ShiftDirection = new Vector3(Random.insideUnitSphere.x, Random.Range(0.6f, 1f), 0f - Random.Range(0.6f, 1f));
		vfxModel.SetActive(value: true);
	}
}
