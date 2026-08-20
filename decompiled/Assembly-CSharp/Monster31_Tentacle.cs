using UnityEngine;

public class Monster31_Tentacle : MonoBehaviour
{
	private enum UnitState
	{
		Idle,
		Show,
		Hide
	}

	public LineRenderer lr_Tentacle;

	public LineRenderer lr_Shadow;

	public int tentacleNodeCount;

	public VariableFloat dirOneSideAngle;

	public float rootOffsetY;

	public VariableFloat rootOffsetRadius;

	public VariableFloat endPointDistance;

	public VariableFloat middleHight;

	public float tailLength;

	public float tailHalfAngleOffset;

	[Header("Flex")]
	public VariableFloat lerpSpeed;

	public VariableFloat flexDelay;

	public VariableFloat flexAmplitude;

	private UnitState state;

	private Monster31 monster31;

	private Vector3 rootPoint;

	private Vector3 endPoint;

	private Vector3 middlePoint;

	private Vector3 tailOffset;

	private float currentLerp;

	private float flexDelayTimer;

	private void Update()
	{
		if (lr_Tentacle.startColor != monster31.myPpt.BaseColor)
		{
			lr_Tentacle.startColor = monster31.myPpt.BaseColor;
			lr_Tentacle.endColor = monster31.myPpt.BaseColor;
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
			currentLerp = Mathf.MoveTowards(currentLerp, 1f, lerpSpeed.result * Time.deltaTime);
			float value2 = Mathf.Lerp(flexAmplitude.value1, flexAmplitude.value2, currentLerp);
			lr_Tentacle.material.SetFloat("_WaveAmplitude", value2);
			lr_Shadow.material.SetFloat("_WaveAmplitude", value2);
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
			lr_Tentacle.material.SetFloat("_WaveAmplitude", value);
			lr_Shadow.material.SetFloat("_WaveAmplitude", value);
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
		lr_Tentacle.material.SetFloat("_Offset", 1f - currentLerp);
		lr_Shadow.material.SetFloat("_Offset", 1f - currentLerp);
		if (lr_Tentacle.GetPosition(0) != Tool2D.GetLayerPoint(rootPoint + monster31.myPpt.Tsf_BeHit.localPosition))
		{
			RecorrectLR();
		}
	}

	private void RecorrectLR()
	{
		for (int i = 0; i < tentacleNodeCount; i++)
		{
			Vector3 zero = Vector3.zero;
			zero = ((i >= tentacleNodeCount - 1) ? (endPoint + tailOffset) : GeneralTool.QuadraticBezierCurve(rootPoint + monster31.myPpt.Tsf_BeHit.localPosition, middlePoint, endPoint, (float)i / ((float)tentacleNodeCount - 2f)));
			lr_Tentacle.SetPosition(i, Tool2D.GetLayerPoint(zero));
		}
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(rootPoint + monster31.myPpt.Tsf_BeHit.localPosition, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(endPoint, 1.05f));
	}

	public void SingleInitial(Monster31 monster31)
	{
		this.monster31 = monster31;
		float value = Random.Range(0f, 9999f);
		lr_Tentacle.positionCount = tentacleNodeCount;
		lr_Shadow.positionCount = 2;
		lr_Tentacle.material.SetFloat("_TimeOffset", value);
		lr_Shadow.material.SetFloat("_TimeOffset", value);
	}

	public void EveryInitial()
	{
		state = UnitState.Idle;
		currentLerp = 0f;
		flexDelayTimer = 0f;
		lr_Tentacle.material.SetFloat("_Offset", 1f - currentLerp);
		lr_Shadow.material.SetFloat("_Offset", 1f - currentLerp);
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
		Vector3 dir = Tool2D.GetDir((float)((Random.Range(0, 2) == 0) ? 1 : (-1)) * Random.Range(dirOneSideAngle.value1, dirOneSideAngle.value2));
		rootPoint = monster31.transform.position + new Vector3(0f, rootOffsetY, 0f) + dir * rootOffsetRadius.RandomResult();
		endPoint = rootPoint + new Vector3(0f, 0f - rootOffsetY, 0f) + dir * endPointDistance.RandomResult();
		middlePoint = rootPoint + new Vector3(0f, 0f, 0f - middleHight.RandomResult());
		tailOffset = Tool2D.GetDir(dir, Random.Range(0f - tailHalfAngleOffset, tailHalfAngleOffset)) * tailLength;
		RecorrectLR();
		lerpSpeed.RandomResult();
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
