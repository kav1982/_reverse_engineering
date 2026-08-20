using System;
using UnityEngine;

public class Boss4_Wing : MonoBehaviour
{
	public LineRenderer lr;

	public int nodeCount;

	public float offset;

	public float segmentLength;

	public float lerp;

	public float rotateSpeed;

	public float rotateHalfAngle;

	public float appearTime;

	[Header("QuikMotion")]
	public float quickMotionLerp;

	public float quickMotionRotateSpeed;

	[Header("Audio")]
	public AudioSource as_WingFlap;

	public float asQuickMotionPitch;

	[Header("和谐模式")]
	public Material mt_harmony;

	private Boss4 boss4;

	private Vector3 dir;

	private bool isLeft;

	private bool isIndex0;

	private float rotateTimer;

	private Vector3[] nodePoints;

	private float currentLerp;

	private float currentRotateSpeed;

	private bool isAppear;

	private float initialWidth;

	private float initialSegment;

	private float appearWidthSpeed;

	private float appearSegmentLengthSpeed;

	private void Update()
	{
		if (!isAppear)
		{
			return;
		}
		rotateTimer += currentRotateSpeed * Time.deltaTime;
		if (isIndex0 && rotateTimer >= MathF.PI * 2f)
		{
			rotateTimer -= MathF.PI * 2f;
			as_WingFlap.Play();
		}
		float z = Tool2D.GetLayerPoint(boss4.transform).z + 0.1f;
		float num = Mathf.Sin(rotateTimer) * rotateHalfAngle;
		Vector3 vector = Tool2D.GetDir(dir, isLeft ? num : (0f - num));
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = base.transform.position;
			}
			else
			{
				nodePoints[i] = Vector3.Lerp(nodePoints[i], nodePoints[i - 1] + vector * segmentLength, currentLerp * Time.deltaTime);
			}
			lr.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
		}
		if (lr.startColor != boss4.myPpt.BaseColor)
		{
			lr.startColor = boss4.myPpt.BaseColor;
			lr.endColor = boss4.myPpt.BaseColor;
		}
		if (lr.startWidth != initialWidth)
		{
			float num2 = Mathf.MoveTowards(lr.startWidth, initialWidth, appearWidthSpeed * Time.deltaTime);
			lr.startWidth = num2;
			lr.endWidth = num2;
		}
		if (segmentLength != initialSegment)
		{
			segmentLength = Mathf.MoveTowards(segmentLength, initialSegment, appearSegmentLengthSpeed * Time.deltaTime);
		}
	}

	public void Initialize(Boss4 boss4, Vector3 dir, bool isLeft, bool isIndex0)
	{
		this.boss4 = boss4;
		this.dir = dir;
		this.isLeft = isLeft;
		this.isIndex0 = isIndex0;
		lr.positionCount = nodeCount;
		nodePoints = new Vector3[nodeCount];
		SetNormalMotion();
		base.transform.localPosition = dir * offset;
		lr.gameObject.SetActive(value: false);
		initialWidth = lr.startWidth;
		initialSegment = segmentLength;
		appearWidthSpeed = initialWidth / appearTime;
		appearSegmentLengthSpeed = segmentLength / appearTime;
		segmentLength = 0f;
		if (isIndex0)
		{
			as_WingFlap.mute = true;
		}
		else
		{
			UnityEngine.Object.Destroy(as_WingFlap.gameObject);
		}
		if (GameMgr.IsHarmony_Static)
		{
			UnityEngine.Object.Destroy(lr.material);
			lr.material = mt_harmony;
		}
	}

	public void Appear()
	{
		isAppear = true;
		lr.gameObject.SetActive(value: true);
		float z = Tool2D.GetLayerPoint(boss4.transform).z + 0.1f;
		for (int i = 0; i < nodeCount; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = base.transform.position;
			}
			else
			{
				nodePoints[i] = nodePoints[i - 1] + dir * segmentLength;
			}
			lr.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
		}
		if (isIndex0)
		{
			as_WingFlap.mute = false;
		}
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		for (int i = 0; i < nodePoints.Length; i++)
		{
			nodePoints[i] += changeValue;
		}
	}

	public void SetQuickMotion()
	{
		currentLerp = quickMotionLerp;
		currentRotateSpeed = quickMotionRotateSpeed;
		if (isIndex0)
		{
			as_WingFlap.pitch = asQuickMotionPitch;
		}
	}

	public void SetNormalMotion()
	{
		currentLerp = lerp;
		currentRotateSpeed = rotateSpeed;
		if (isIndex0)
		{
			as_WingFlap.pitch = 1f;
		}
	}
}
