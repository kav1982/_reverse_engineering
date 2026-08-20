using System;
using UnityEngine;

public class Boss4_Hair : MonoBehaviour
{
	public LineRenderer lr_Tentacle;

	public float rootOffset;

	public VariableInt nodeCount;

	public float segmentLength;

	public float lerp;

	public VariableFloat rotateAngle;

	public float zOffset;

	[Header("Mat")]
	public VariableFloat matTimeOffset;

	public VariableFloat matWaveSpeed;

	[Header("ChangeStage")]
	public float changeStageTime;

	public float changeStageSegmentLength;

	public float changeStageWaveSpeed;

	public float changeStageWaveSpeed2;

	[Range(0f, 1f)]
	[Header("Stage3GrowEye")]
	public float growChance;

	public Transform tsf_EyeRoot;

	public MeshRenderer mr_Eye;

	public Animator anima;

	public Material mat_Stage3;

	public VariableFloat eyeScale;

	public float growLerp;

	private Vector3[] nodePoints;

	private Vector3[] nodeSpeed;

	private Boss4 boss4;

	private float finalSegmentLength;

	private bool isStage2;

	private float changeSegmentLengthSpeed;

	private bool isGrow;

	[Header("晃动")]
	public float amplitude;

	public VariableFloat moveSpeed;

	private float startPhase;

	public VariableFloat frequency;

	public AnimationCurve blendStrength;

	public AnimationCurve glowBlendStrength;

	private float totalLength => finalSegmentLength * (float)nodeCount.result;

	private float GetOffsetByIndex(int index)
	{
		if (isGrow)
		{
			return glowBlendStrength.Evaluate((float)index / (float)nodeCount.result) * amplitude * totalLength * Mathf.Sin(frequency.result * (float)index / (float)nodeCount.result * MathF.PI * 2f + startPhase + Time.time * moveSpeed.result * MathF.PI * 2f);
		}
		return blendStrength.Evaluate((float)index / (float)nodeCount.result) * amplitude * totalLength * Mathf.Sin(frequency.result * (float)index / (float)nodeCount.result * MathF.PI * 2f + startPhase + Time.time * moveSpeed.result * MathF.PI * 2f);
	}

	private void LateUpdate()
	{
		float num = Tool2D.GetLayerPoint(boss4.transform).z + zOffset;
		for (int i = 0; i < nodeCount.result; i++)
		{
			if (i == 0)
			{
				nodePoints[i] = base.transform.position;
				lr_Tentacle.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), num));
			}
			else
			{
				nodePoints[i] = Vector3.SmoothDamp(nodePoints[i], nodePoints[i - 1] + Tool2D.GetDir(base.transform.up, rotateAngle.result * (float)i) * finalSegmentLength, ref nodeSpeed[i], 1f / lerp);
				lr_Tentacle.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i] + (Vector3)Vector2.Perpendicular((Vector2)(nodePoints[i] - nodePoints[i - 1])).normalized * GetOffsetByIndex(i)), num));
			}
			if (i == nodeCount.result - 1 && isGrow)
			{
				tsf_EyeRoot.position = Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i] + (Vector3)Vector2.Perpendicular((Vector2)(nodePoints[i] - nodePoints[i - 1])) * GetOffsetByIndex(i)), num - 0.1f);
			}
		}
		if (lr_Tentacle.startColor != boss4.myPpt.BaseColor)
		{
			lr_Tentacle.startColor = boss4.myPpt.BaseColor;
			lr_Tentacle.endColor = boss4.myPpt.BaseColor;
			if (isGrow)
			{
				mr_Eye.material.color = boss4.myPpt.BaseColor;
			}
		}
		if (isStage2 && finalSegmentLength != changeStageSegmentLength)
		{
			finalSegmentLength = Mathf.MoveTowards(finalSegmentLength, changeStageSegmentLength, changeSegmentLengthSpeed * Time.deltaTime);
		}
	}

	public void Initialize(Boss4 boss4)
	{
		this.boss4 = boss4;
		finalSegmentLength = segmentLength;
		moveSpeed.RandomResult();
		frequency.RandomResult();
		startPhase = UnityEngine.Random.Range(0f, MathF.PI * 2f);
		nodeCount.RandomResult();
		lr_Tentacle.positionCount = nodeCount.result;
		nodePoints = new Vector3[nodeCount.result];
		nodeSpeed = new Vector3[nodeCount.result];
		rotateAngle.RandomResult();
		rotateAngle.result *= (float)((UnityEngine.Random.Range(0f, 1f) > 0.5f) ? 1 : (-1));
		Vector3 dir = Tool2D.GetDir();
		base.transform.localPosition = dir * rootOffset;
		base.transform.up = dir;
		float z = Tool2D.GetLayerPoint(boss4.transform).z + zOffset;
		for (int i = 0; i < nodeCount.result; i++)
		{
			nodeSpeed[i] = new Vector3(0f, 0f, 0f);
			if (i == 0)
			{
				nodePoints[i] = base.transform.position;
			}
			else
			{
				nodePoints[i] = nodePoints[i - 1] + Tool2D.GetDir(base.transform.up, rotateAngle.result * (float)i) * segmentLength;
			}
			lr_Tentacle.SetPosition(i, Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(nodePoints[i]), z));
		}
		lr_Tentacle.material.SetFloat("_TimeOffset", matTimeOffset.RandomResult());
		lr_Tentacle.material.SetFloat("_WaveSpeed", matWaveSpeed.RandomResult());
		changeSegmentLengthSpeed = (changeStageSegmentLength - segmentLength) / changeStageTime;
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		for (int i = 0; i < nodePoints.Length; i++)
		{
			nodePoints[i] += changeValue;
		}
	}

	public void ChangeStageStart()
	{
		isStage2 = true;
		lr_Tentacle.material.SetFloat("_WaveSpeed", changeStageWaveSpeed);
		moveSpeed.result = changeStageWaveSpeed;
	}

	public void ChangeStageEnd()
	{
		lr_Tentacle.material.SetFloat("_WaveSpeed", changeStageWaveSpeed2);
		moveSpeed.result = changeStageWaveSpeed2;
	}

	public void GrowEye()
	{
		if (UnityEngine.Random.value <= growChance)
		{
			isGrow = true;
			UnityEngine.Object.Destroy(lr_Tentacle.material);
			lr_Tentacle.material = mat_Stage3;
			lr_Tentacle.material.SetFloat("_TimeOffset", matTimeOffset.result);
			lr_Tentacle.material.SetFloat("_WaveSpeed", matWaveSpeed.result);
			anima.SetTrigger("GrowEye");
			mr_Eye.transform.localScale = mr_Eye.transform.localScale * eyeScale.RandomResult();
			lerp = growLerp;
		}
	}

	public void Blink()
	{
		if (isGrow)
		{
			anima.SetTrigger("Blink");
		}
	}

	public void ReverseBeamWarning()
	{
		if (isGrow)
		{
			anima.SetTrigger("BeamWarning");
		}
	}

	public void ReverseBeaming()
	{
		if (isGrow)
		{
			anima.SetTrigger("Beaming");
		}
	}

	public void ReverseBeamFinish()
	{
		if (isGrow)
		{
			anima.SetTrigger("BeamFinish");
		}
	}
}
