using SpriteEffectSystem;
using UnityEngine;

public class Spell3101PullCrystal : LayerCorrect
{
	public SpriteEffectAnimaByColorType hitAnimas;

	public MaterialsByColorType materials;

	public LineRenderer trail;

	public LineRenderer shadowTrail;

	public Vector3 chainBaseHeight;

	private Transform sourceTransform;

	private Vector3 sourcePosition = Vector3.zero;

	private Transform targetTransform;

	private Vector3 targetPosition = Vector3.zero;

	private float dissolveProcess;

	public Vector3 trailZaxisHeightShift;

	public float dissolveSpeed;

	public float dissolveStartAt;

	public float maxDissolveAmount;

	private float currentDissolve;

	public Vector2 pos1Range;

	public Vector2 pos2Range;

	public Vector2 pos1Distance;

	public Vector2 pos2Distance;

	public Vector2 posAngle;

	private Vector3 horizontalMoveDir = Vector3.zero;

	public Vector2 horizontalmoveSpeedRange;

	private Vector3 sourcePointShift = Vector3.zero;

	private Vector3 targetPointShift = Vector3.zero;

	public float basePointShiftRange;

	private float horizontalmoveSpeed;

	private BezierCurvePointData point1Data;

	private BezierCurvePointData point2Data;

	public int linePointCount;

	public float dissolveStartDelay;

	private float delayStartTimer;

	public float lineWidth;

	private static readonly int DissolveProcessID = Shader.PropertyToID("_DissolveProcess");

	public override void OnEnable()
	{
		horizontalmoveSpeed = 0f;
		horizontalMoveDir = Vector3.zero;
		dissolveProcess = 0f;
		delayStartTimer = 0f;
		currentDissolve = dissolveStartAt;
	}

	private void Update()
	{
		if (delayStartTimer < dissolveStartDelay)
		{
			delayStartTimer += Time.deltaTime;
			for (int i = 0; i < linePointCount; i++)
			{
				Vector3 vector = GeneralTool.CubicBezierCurve(sourcePosition, point1Data.currentPosition, point2Data.currentPosition, targetPosition, (float)i / ((float)linePointCount - 1f));
				trail.SetPosition(i, Tool2D.GetLayerPoint(vector) + trailZaxisHeightShift);
				shadowTrail.SetPosition(i, Tool2D.IgnoreZPoint(vector, 1.05f));
			}
			return;
		}
		point1Data.Type1PointShift();
		point1Data.currentPosition += horizontalMoveDir * horizontalmoveSpeed * Time.deltaTime;
		point2Data.Type1PointShift();
		point1Data.currentPosition -= horizontalMoveDir * horizontalmoveSpeed * Time.deltaTime;
		for (int j = 0; j < linePointCount; j++)
		{
			Vector3 vector2 = GeneralTool.CubicBezierCurve(sourcePosition + sourcePointShift, point1Data.currentPosition, point2Data.currentPosition, targetPosition + targetPointShift, (float)j / ((float)linePointCount - 1f));
			trail.SetPosition(j, Tool2D.GetLayerPoint(vector2) + trailZaxisHeightShift);
			shadowTrail.SetPosition(j, Tool2D.IgnoreZPoint(vector2, 1.05f));
		}
		currentDissolve = Mathf.Lerp(currentDissolve, maxDissolveAmount, dissolveSpeed * Time.deltaTime);
		trail.material.SetFloat(DissolveProcessID, currentDissolve);
		shadowTrail.material.SetFloat(DissolveProcessID, Mathf.Min(maxDissolveAmount, dissolveStartAt + dissolveProcess));
		dissolveProcess += Time.deltaTime * dissolveSpeed;
	}

	public override void LateUpdate()
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
		InitialChain();
	}

	public void SetChainTargetPosition(Vector3 casterPos, Vector3 targetUnitPos)
	{
		sourceTransform = null;
		sourcePosition = casterPos;
		targetTransform = null;
		targetPosition = targetUnitPos;
		InitialChain();
	}

	public void InitialChain()
	{
		point1Data = new BezierCurvePointData();
		point2Data = new BezierCurvePointData();
		Vector3 normalized = (targetPosition - sourcePosition).normalized;
		float num = Random.Range(pos1Range.x, pos1Range.y);
		float num2 = Random.Range(pos2Range.x, pos2Range.y);
		int num3 = ((Random.Range(-1f, 1f) >= 0f) ? 1 : (-1));
		Vector3 dir = Tool2D.GetDir(normalized, Random.Range(posAngle.x, posAngle.y) * (float)num3);
		point1Data.currentPosition = sourcePosition + (targetPosition - sourcePosition) * num;
		point1Data.percentInRange = num;
		point1Data.pointShiftDir = dir;
		point1Data.pointShiftSpeed = Random.Range(pos1Distance.x, pos1Distance.y);
		point2Data.currentPosition = sourcePosition + (targetPosition - sourcePosition) * num2;
		point2Data.percentInRange = num2;
		point2Data.pointShiftDir = -dir;
		point2Data.pointShiftSpeed = Random.Range(pos2Distance.x, pos2Distance.y);
		horizontalmoveSpeed = Random.Range(horizontalmoveSpeedRange.x, horizontalmoveSpeedRange.y);
		sourcePointShift = chainBaseHeight + Tool2D.IgnoreZPoint(Random.insideUnitSphere * Random.Range(0f, basePointShiftRange));
		targetPointShift = chainBaseHeight + Tool2D.IgnoreZPoint(Random.insideUnitSphere * Random.Range(0f, basePointShiftRange));
		SetChainWidth(lineWidth);
		dissolveProcess = 0f;
		trail.positionCount = linePointCount;
		shadowTrail.positionCount = linePointCount;
		trail.material.SetFloat(DissolveProcessID, dissolveStartAt);
		for (int i = 0; i < trail.positionCount; i++)
		{
			trail.SetPosition(i, Vector3.zero);
		}
	}

	public void SetChainWidth(float width)
	{
		trail.startWidth = width;
	}

	public void SetColor(SpellColorType color)
	{
		trail.material = materials.Get(color);
	}

	public void CreateHitEffect(SpellColorType color, Vector3 position)
	{
		EffectPlayParam param = new EffectPlayParam
		{
			Position = position,
			Rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)),
			Scale = new Vector3(0.7f, 0.7f, 0.7f),
			Color = new Color(1f, 1f, 1f, DataMgr.settingData.FinalSpellTransparent)
		};
		SpellSpriteEffectController.Inst.PlayEffectIgnoreSpellBase(hitAnimas.Get(color), param);
	}
}
