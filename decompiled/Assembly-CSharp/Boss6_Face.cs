using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Face : MonoBehaviour
{
	public enum FaceState
	{
		Idle,
		Close,
		Open,
		OpenContinue,
		OpenContinueDead,
		CloseContinue
	}

	public Transform tsf_Head;

	public List<Transform> rightTransform = new List<Transform>();

	public List<Transform> leftTransform = new List<Transform>();

	public List<float> angleRange = new List<float>();

	public VariableFloat frequencyRange;

	public List<float> frequency = new List<float>();

	public List<float> startPhase = new List<float>();

	public List<float> startAngle = new List<float>();

	public List<float> nowAngle = new List<float>();

	public float bigTeethCloseAngle;

	public float bigTeethOpenAngle;

	public float idleCloseTime;

	public float closeOpenTime;

	public float OpenIdleTime;

	public AnimationCurve idleCloseCurve;

	public AnimationCurve closeOpenCurve;

	public AnimationCurve openIdleCurve;

	private bool teethFrozen;

	[Header("和谐")]
	public SpriteRenderer sr_Head;

	public Sprite sprite_H;

	[Header("状态")]
	public FaceState _state;

	private bool stateQuit;

	private bool changedState;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private float stateExistTime;

	public FaceState state
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
			varMgr.Clear();
		}
	}

	public void Initialize()
	{
		if (GameMgr.IsHarmony_Static)
		{
			sr_Head.sprite = sprite_H;
		}
		startPhase.Clear();
		frequency.Clear();
		startAngle.Clear();
		for (int i = 0; i < rightTransform.Count; i++)
		{
			startPhase.Add(MathF.PI * 2f * UnityEngine.Random.value);
			startAngle.Add(Tool2D.IgnoreZAngleWithSign(tsf_Head.up, rightTransform[i].up));
			frequency.Add(frequencyRange.RandomResult());
			if (i == 10)
			{
				frequency[i] /= 2f;
			}
			nowAngle.Add(0f);
		}
		state = FaceState.Idle;
	}

	public void SetClose()
	{
		state = FaceState.Close;
	}

	public void SetOpen()
	{
		state = FaceState.Open;
	}

	public void SetOpenContinue()
	{
		state = FaceState.OpenContinue;
	}

	public void SetCloseContinue()
	{
		state = FaceState.CloseContinue;
	}

	public void SetIdle()
	{
		state = FaceState.Idle;
	}

	private void Update()
	{
		if (!teethFrozen)
		{
			for (int i = 0; i < frequency.Count; i++)
			{
				nowAngle[i] = angleRange[i] * Mathf.Sin(Time.time * MathF.PI * 2f * frequency[i] + startPhase[i]);
				rightTransform[i].localEulerAngles = new Vector3(0f, 0f, startAngle[i] + nowAngle[i]);
				leftTransform[i].localEulerAngles = new Vector3(0f, 0f, 180f - startAngle[i] + (0f - nowAngle[i]));
			}
		}
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case FaceState.Idle:
		{
			_ = changedState;
			float num = startAngle[10] + Mathf.Lerp(bigTeethOpenAngle, nowAngle[10], openIdleCurve.Evaluate(stateExistTime / OpenIdleTime));
			rightTransform[10].localEulerAngles = new Vector3(0f, 0f, num);
			leftTransform[10].localEulerAngles = new Vector3(0f, 0f, 180f - num);
			break;
		}
		case FaceState.Close:
		{
			float num = startAngle[10] + Mathf.Lerp(nowAngle[10], bigTeethCloseAngle, idleCloseCurve.Evaluate(stateExistTime / idleCloseTime));
			rightTransform[10].localEulerAngles = new Vector3(0f, 0f, num);
			leftTransform[10].localEulerAngles = new Vector3(0f, 0f, 180f - num);
			break;
		}
		case FaceState.CloseContinue:
		{
			float num = startAngle[10] + Mathf.Lerp(nowAngle[10], bigTeethCloseAngle, idleCloseCurve.Evaluate(stateExistTime / idleCloseTime));
			rightTransform[10].localEulerAngles = new Vector3(0f, 0f, num);
			leftTransform[10].localEulerAngles = new Vector3(0f, 0f, 180f - num);
			if (stateExistTime > 0.5f)
			{
				state = FaceState.OpenContinueDead;
			}
			break;
		}
		case FaceState.Open:
		{
			float num = startAngle[10] + Mathf.Lerp(bigTeethCloseAngle, bigTeethOpenAngle, closeOpenCurve.Evaluate(stateExistTime / closeOpenTime));
			rightTransform[10].localEulerAngles = new Vector3(0f, 0f, num);
			leftTransform[10].localEulerAngles = new Vector3(0f, 0f, 180f - num);
			if (stateExistTime > closeOpenTime)
			{
				state = FaceState.Idle;
			}
			break;
		}
		case FaceState.OpenContinue:
		{
			float num = startAngle[10] + Mathf.Lerp(bigTeethCloseAngle, bigTeethOpenAngle, closeOpenCurve.Evaluate(stateExistTime / closeOpenTime));
			rightTransform[10].localEulerAngles = new Vector3(0f, 0f, num);
			leftTransform[10].localEulerAngles = new Vector3(0f, 0f, 180f - num);
			break;
		}
		case FaceState.OpenContinueDead:
		{
			float num = startAngle[10] + Mathf.Lerp(bigTeethCloseAngle, bigTeethOpenAngle, closeOpenCurve.Evaluate(stateExistTime / closeOpenTime));
			rightTransform[10].localEulerAngles = new Vector3(0f, 0f, num);
			leftTransform[10].localEulerAngles = new Vector3(0f, 0f, 180f - num);
			if (stateExistTime > closeOpenTime)
			{
				teethFrozen = true;
			}
			break;
		}
		}
	}
}
