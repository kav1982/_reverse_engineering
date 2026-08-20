using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite11_Tentacle : MonoBehaviour
{
	public enum TentacleState
	{
		WaveMute,
		WaveStart,
		Wave,
		WaveStop
	}

	[Header("旋转模式")]
	public float blockStartDistance;

	public float blockSpeed;

	public float blockOutSpeed;

	public float blockBackSpeed;

	public float blockAllLength;

	public float blockInterval;

	public float blockAngleAmplitude;

	public List<float> NodeAngle;

	public List<float> NodeDistance;

	public List<Vector3> NodePos;

	public float period;

	private float nowPhase;

	private float blockAllTime;

	private float nowPercent;

	private float singleNodePercent;

	private int allNodeCount;

	private float nodeDeltaPhase;

	private float fullPeriodPercent;

	public AnimationCurve blockDeacclerateCurve;

	public AnimationCurve blockAcclerateCurve;

	public AnimationCurve blockAngleFixCurve;

	private bool rotateRight;

	[Header("通用表现")]
	public LineRenderer lr_Main;

	public LineRenderer lr_Shadow;

	[Header("通用")]
	public float angleOffset;

	public float TentacleHeight;

	[Header("伤害")]
	public float attackRadius;

	public int damage;

	public float damageInterval;

	public List<Entity> attackedEnitites = new List<Entity>();

	public List<float> attackedCD = new List<float>();

	public LayerMask attackMask;

	public TentacleState _state;

	private bool stateQuit;

	private bool changedState;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private float stateExistTime;

	private float recordAllLength;

	public TentacleState state
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
		blockAllTime = blockAllLength / blockSpeed;
		allNodeCount = Mathf.FloorToInt(blockAllLength / (blockInterval * blockSpeed)) + 1;
		singleNodePercent = blockInterval / blockAllTime;
		nodeDeltaPhase = MathF.PI * 2f * blockInterval / (period * 2f);
		fullPeriodPercent = period * 2f / blockAllTime;
		NodeAngle = new List<float>();
		NodeDistance = new List<float>();
		NodePos = new List<Vector3>();
		for (int i = 0; i < allNodeCount; i++)
		{
			NodeAngle.Add(Mathf.Sin((float)i * nodeDeltaPhase));
			NodeDistance.Add(blockStartDistance + (float)i * blockInterval * blockSpeed);
			NodePos.Add(Vector3.zero);
		}
		lr_Main.positionCount = allNodeCount;
		lr_Shadow.positionCount = allNodeCount;
	}

	private void SetNodePhase(float phase)
	{
		for (int i = 0; i < allNodeCount; i++)
		{
			NodeAngle[i] = Mathf.Sin((float)i * nodeDeltaPhase + (0f - phase) * 2f * MathF.PI);
		}
	}

	public void LaunchWave(bool rotateRight, float angleOffset)
	{
		this.rotateRight = rotateRight;
		this.angleOffset = angleOffset;
		state = TentacleState.WaveStart;
	}

	public void StopWave()
	{
		state = TentacleState.WaveStop;
	}

	private void Update()
	{
		if (lr_Main.material.color != Elite11.Inst.myPpt.BaseColor)
		{
			lr_Main.material.color = Elite11.Inst.myPpt.BaseColor;
		}
		for (int num = attackedCD.Count - 1; num >= 0; num--)
		{
			attackedCD[num] -= Time.deltaTime;
			if (attackedCD[num] < 0f)
			{
				attackedCD.RemoveAt(num);
				attackedEnitites.RemoveAt(num);
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
		case TentacleState.WaveStart:
			if (changedState)
			{
				nowPhase = 0f;
			}
			nowPhase += Time.deltaTime / (period * 2f);
			nowPercent += blockOutSpeed * Time.deltaTime / blockAllLength * 1f;
			SetNodePhase(nowPhase);
			if (nowPercent > 1f)
			{
				nowPercent = 1f;
				state = TentacleState.Wave;
			}
			break;
		case TentacleState.Wave:
			_ = changedState;
			nowPhase += Time.deltaTime / (period * 2f);
			SetNodePhase(nowPhase);
			break;
		case TentacleState.WaveStop:
			_ = changedState;
			nowPhase += Time.deltaTime / (period * 2f);
			nowPercent -= blockBackSpeed * Time.deltaTime / blockAllLength * blockAcclerateCurve.Evaluate(stateExistTime);
			if (nowPercent < 0f)
			{
				nowPercent = 0f;
				state = TentacleState.WaveMute;
			}
			break;
		}
		if (nowPercent > 0f)
		{
			float a = Mathf.FloorToInt(nowPercent * (float)(allNodeCount - 1)) + 1;
			a = Mathf.Min(a, allNodeCount);
			float num2 = nowPercent - (a - 1f) * singleNodePercent;
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < allNodeCount; i++)
			{
				NodePos[i] = Elite11.elite11Position + Tool2D.GetDir(Vector3.up, angleOffset + blockAngleAmplitude * NodeAngle[i] * (float)((!rotateRight) ? 1 : (-1)) * blockAngleFixCurve.Evaluate((float)i / (float)allNodeCount)) * NodeDistance[i];
				lr_Main.SetPosition(i, Tool2D.GetLayerPoint(NodePos[i] + new Vector3(0f, 0f, 0f - TentacleHeight)));
				lr_Shadow.SetPosition(i, Tool2D.GetLayerPoint(NodePos[i], LayerCorrectType.Shadow));
				if (i > 0)
				{
					float magnitude = (NodePos[i] - NodePos[i - 1]).magnitude;
					num4 += magnitude;
					if ((float)i < a)
					{
						num3 += magnitude;
					}
					else if ((float)i == a)
					{
						num3 += magnitude * num2 / singleNodePercent;
					}
					if ((float)i < a)
					{
						UnitDotsSyncSystem.RayCastHitResult[] array = UnitDotsSyncSystem.SphereCastAll(NodePos[i - 1], NodePos[i] - NodePos[i - 1], attackRadius, magnitude, GameConst.Filter_MonsterAoeNoSpell);
						for (int j = 0; j < array.Length; j++)
						{
							Entity entity = array[j].entity;
							if (!attackedEnitites.Contains(entity))
							{
								attackedEnitites.Add(entity);
								attackedCD.Add(damageInterval);
								TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite11.Inst.myPpt.myEntity);
								info.damage = damage;
								info.teammateTakeDamageRatio = 4f;
								UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
								if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(entity, out var result) && result.unitCfg.IsSameCamp(UnitType.Player))
								{
									ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", array[j].point, 1f);
								}
							}
						}
					}
				}
				if (state == TentacleState.WaveStart || state == TentacleState.WaveStop)
				{
					lr_Main.material.SetFloat("_Fill", Mathf.Min(1f, num3 / num4));
					lr_Shadow.material.SetFloat("_Length", Mathf.Min(1f, num3 / num4));
				}
			}
		}
		else
		{
			for (int k = 0; k < allNodeCount; k++)
			{
				lr_Main.material.SetFloat("_Fill", 0f);
				lr_Shadow.material.SetFloat("_Length", 0f);
			}
		}
	}

	public void DieExplode()
	{
		if (!(nowPercent <= 0f))
		{
			Mathf.FloorToInt(nowPercent * (float)(allNodeCount - 1));
			for (int i = 0; i < allNodeCount; i++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Blood", NodePos[i]);
			}
		}
	}
}
