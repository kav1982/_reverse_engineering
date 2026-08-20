using UnityEngine;

public class Monster31_Tentacle_Old : MonoBehaviour
{
	private enum UnitState
	{
		Idle,
		Show,
		Hide
	}

	public LineRenderer lr_Tentacle;

	public LineRenderer lr_Shadow;

	public int tentacleCount;

	public int tentacleNodeCount;

	public float rootOffsetY;

	public float rootOffsetRadius;

	public VariableFloat endPointDistance;

	public VariableFloat middleHight;

	public float endPointOffset;

	public float tailLength;

	public float tailHalfAngleOffset;

	[Header("Flex")]
	public VariableFloat lerpSpeed;

	public VariableFloat flexDelay;

	public VariableFloat flexAmplitude;

	private UnitState state;

	private Monster31 monster31;

	private LineRenderer[] lr_Tentacles;

	private LineRenderer[] lr_Shadows;

	private Vector3[] endPointOffsets;

	private Vector3 rootPoint;

	private Vector3 endPoint;

	private Vector3 middlePoint;

	private float legRatio;

	private float currentLerp;

	private float flexDelayTimer;

	private void Update()
	{
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
			for (int j = 0; j < tentacleCount; j++)
			{
				lr_Tentacles[j].material.SetFloat("_WaveAmplitude", value2);
				lr_Shadows[j].material.SetFloat("_WaveAmplitude", value2);
			}
			if (currentLerp == 1f)
			{
				state = UnitState.Idle;
			}
			break;
		}
		case UnitState.Hide:
		{
			if (flexDelayTimer < flexDelay.result)
			{
				flexDelayTimer += Time.deltaTime;
				break;
			}
			currentLerp = Mathf.MoveTowards(currentLerp, 0f, lerpSpeed.result * Time.deltaTime);
			float value = Mathf.Lerp(flexAmplitude.value1, flexAmplitude.value2, currentLerp);
			for (int i = 0; i < tentacleCount; i++)
			{
				lr_Tentacles[i].material.SetFloat("_WaveAmplitude", value);
				lr_Shadows[i].material.SetFloat("_WaveAmplitude", value);
			}
			if (currentLerp == 0f)
			{
				state = UnitState.Idle;
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		case UnitState.Idle:
			break;
		}
		Vector3 vector = new Vector3(monster31.myPpt.Tsf_BeHit.localPosition.x, monster31.myPpt.Tsf_BeHit.localPosition.y, 0f);
		if (currentLerp <= legRatio)
		{
			float num = (endPointDistance.result + tailLength) * currentLerp / endPointDistance.result;
			for (int k = 0; k < tentacleCount; k++)
			{
				for (int l = 0; l < tentacleNodeCount; l++)
				{
					Vector3 v = GeneralTool.QuadraticBezierCurve(rootPoint + vector, middlePoint, endPoint + endPointOffsets[k], (float)l / ((float)tentacleNodeCount - 1f) * num);
					lr_Tentacles[k].SetPosition(l, Tool2D.GetLayerPoint(v));
					lr_Shadows[k].SetPosition(l, Tool2D.IgnoreZPoint(v, 1.05f));
				}
			}
		}
		else
		{
			float t = ((endPointDistance.result + tailLength) * currentLerp - endPointDistance.result) / tailLength;
			for (int m = 0; m < lr_Tentacles.Length; m++)
			{
				for (int n = 0; n < tentacleNodeCount; n++)
				{
					Vector3 zero = Vector3.zero;
					zero = ((n >= tentacleNodeCount - 1) ? Vector3.Lerp(endPoint + endPointOffsets[m], endPoint + endPointOffsets[m] + endPointOffsets[m].normalized * tailLength, t) : GeneralTool.QuadraticBezierCurve(rootPoint + vector, middlePoint, endPoint + endPointOffsets[m], (float)n / ((float)tentacleNodeCount - 2f)));
					lr_Tentacles[m].SetPosition(n, Tool2D.GetLayerPoint(zero));
					lr_Shadows[m].SetPosition(n, Tool2D.IgnoreZPoint(zero, 1.05f));
				}
			}
		}
		if (lr_Tentacle.startColor != monster31.myPpt.BaseColor)
		{
			for (int num2 = 0; num2 < tentacleCount; num2++)
			{
				lr_Tentacles[num2].startColor = monster31.myPpt.BaseColor;
				lr_Tentacles[num2].endColor = monster31.myPpt.BaseColor;
			}
		}
	}

	public void Initialize(Monster31 monster31)
	{
		this.monster31 = monster31;
		lr_Tentacles = new LineRenderer[tentacleCount];
		lr_Shadows = new LineRenderer[tentacleCount];
		endPointOffsets = new Vector3[tentacleCount];
		endPointDistance.RandomResult();
		for (int i = 0; i < tentacleCount; i++)
		{
			if (i == 0)
			{
				lr_Tentacle.positionCount = tentacleNodeCount;
				lr_Shadow.positionCount = tentacleNodeCount;
				lr_Tentacles[i] = lr_Tentacle;
				lr_Shadows[i] = lr_Shadow;
			}
			else
			{
				lr_Tentacles[i] = Object.Instantiate(lr_Tentacle, base.transform);
				lr_Shadows[i] = Object.Instantiate(lr_Shadow, base.transform);
			}
			endPointOffsets[i] = Tool2D.GetDir() * endPointOffset;
			for (int j = 0; j < tentacleNodeCount; j++)
			{
				lr_Tentacles[i].SetPosition(j, Vector3.zero);
				lr_Shadows[i].SetPosition(j, Vector3.zero);
			}
			float value = Random.Range(0f, 9999f);
			lr_Tentacles[i].material.SetFloat("_TimeOffset", value);
			lr_Shadows[i].material.SetFloat("_TimeOffset", value);
		}
	}

	public void Show()
	{
		state = UnitState.Show;
		lerpSpeed.RandomResult();
		Vector3 dir = Tool2D.GetDir();
		rootPoint = monster31.transform.position + new Vector3(0f, rootOffsetY, 0f) + dir * Random.Range(0f, rootOffsetRadius);
		endPoint = rootPoint + new Vector3(0f, 0f - rootOffsetY, 0f) + dir * endPointDistance.RandomResult();
		middlePoint = rootPoint + new Vector3(0f, 0f, 0f - middleHight.RandomResult());
		legRatio = endPointDistance.result / (endPointDistance.result + tailLength);
		flexDelay.RandomResult();
		flexDelayTimer = 0f;
	}

	public void Hide()
	{
		state = UnitState.Hide;
		flexDelay.RandomResult();
		flexDelayTimer = 0f;
	}
}
