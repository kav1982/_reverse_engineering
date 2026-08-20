using System;
using System.Collections.Generic;
using UnityEngine;

public class Spell1030Rope : MonoBehaviour
{
	public Spell1030Harpoons Harpoons;

	public float chainLength;

	public float ikIterationTime;

	public float threshold;

	public LineRenderer chainLine;

	public LineRenderer shadowLine;

	private int chainCount;

	private List<Vector3> points = new List<Vector3>();

	private float totalLength;

	private List<GameObject> chainList = new List<GameObject>();

	private bool spawnEnd;

	private static readonly int IsSharpHarpoon = Shader.PropertyToID("_IsSharpHarpoon");

	private static readonly int UseMainTexColor = Shader.PropertyToID("_UseMainTexColor");

	private void OnEnable()
	{
		chainLine.positionCount = 0;
		shadowLine.positionCount = 0;
		spawnEnd = false;
	}

	public void ActiveChain(Material chainMaterial, bool isFuseHapoon)
	{
		chainLine.material = chainMaterial;
		chainCount = Mathf.CeilToInt(Harpoons.ropeLength / chainLength);
		if (isFuseHapoon)
		{
			chainLine.material.SetFloat(UseMainTexColor, 1f);
			chainLine.material.SetFloat(IsSharpHarpoon, 1f);
			shadowLine.material.SetFloat(IsSharpHarpoon, 1f);
		}
		totalLength = (float)chainCount * chainLength;
		points.Clear();
		if (!Harpoons.SIP.spellIsFall)
		{
			Vector3 vector = Harpoons.transform.position + new Vector3(0f, 0.3f, -0.3f);
			Vector3 vector2 = Harpoons.GetAroundTargetBasePoint() + new Vector3(0f, 0.3f, -0.3f);
			points.Add(vector);
			points.Add(vector2);
			chainLine.positionCount = 2;
			chainLine.SetPosition(0, vector);
			chainLine.SetPosition(1, vector2);
		}
		spawnEnd = true;
	}

	private void Update()
	{
		if (spawnEnd)
		{
			if (Harpoons.SIP.spellIsFall)
			{
				UpdateFallChainState();
			}
			else
			{
				UpdateChainState();
			}
		}
	}

	private void LateUpdate()
	{
	}

	private void UpdateFallChainState()
	{
		Vector3 position = Harpoons.transform.position;
		Vector3 vector = position + new Vector3(0f, 0f - Harpoons.transform.position.z, 0f);
		switch (Harpoons.currentState)
		{
		case Spell1030Harpoons.HarpoonsState.Shooting:
			chainLine.positionCount++;
			shadowLine.positionCount++;
			chainLine.SetPosition(chainLine.positionCount - 1, vector);
			shadowLine.SetPosition(chainLine.positionCount - 1, vector);
			points.Add(vector);
			break;
		case Spell1030Harpoons.HarpoonsState.Holding:
		case Spell1030Harpoons.HarpoonsState.HookHolding:
		{
			Vector3 vector4 = (Harpoons.initialFallPosition + new Vector3(0f, 7f, 0f) - vector) / points.Count;
			_ = (Harpoons.initialFallPosition - vector) / points.Count;
			for (int j = 0; j < points.Count; j++)
			{
				points[j] = Vector3.Lerp(points[j], vector + vector4 * j, 5f * Time.deltaTime);
				MonoBehaviour.print(points[j].ToString() + " " + Tool2D.GetLayerPoint(points[j], LayerCorrectType.Coordinate));
				chainLine.SetPosition(j, points[j]);
				shadowLine.SetPosition(j, vector + new Vector3(0f, 0f, 1.05f));
			}
			break;
		}
		case Spell1030Harpoons.HarpoonsState.PullingBack:
		{
			MonoBehaviour.print(2222);
			Vector3 vector2 = (Harpoons.initialFallPosition + new Vector3(0f, 7f, 0f) - vector) / points.Count;
			Vector3 vector3 = (Harpoons.initialFallPosition - vector) / points.Count;
			for (int i = 0; i < points.Count; i++)
			{
				points[i] = Vector3.Lerp(points[i], vector + vector2 * i, 30f * Time.deltaTime);
				_ = position + vector3 * i;
				chainLine.SetPosition(i, points[i]);
				shadowLine.SetPosition(i, vector + new Vector3(0f, 0f, 1.05f));
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void UpdateChainState()
	{
		Vector3 vector = Harpoons.transform.position + new Vector3(0f, 0.3f, 0f);
		Vector3 vector2 = Harpoons.GetAroundTargetBasePoint() + new Vector3(0f, 0.3f, -0.3f);
		if ((Harpoons.GetAroundTargetBasePoint() - Harpoons.transform.position).sqrMagnitude < totalLength * totalLength)
		{
			float num = Mathf.Max(Tool2D.IgnoreZDistance(Harpoons.GetAroundTargetBasePoint(), Harpoons.transform.position), Harpoons.ropeTravelDistance);
			if (Harpoons.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				num /= 3.14f;
			}
			int num2 = Mathf.Min(Mathf.CeilToInt(num / chainLength), chainCount) + 6;
			if (Harpoons.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				num2 = Mathf.Min(num2, Mathf.CeilToInt(Harpoons.spellAroundOwnerRadius * 3.14f * 8f));
			}
			if (num2 > chainLine.positionCount && Harpoons.currentState != Spell1030Harpoons.HarpoonsState.PullingBack)
			{
				for (int i = 0; i < num2 - chainLine.positionCount; i++)
				{
					points.Add(vector2);
				}
				chainLine.positionCount = num2;
			}
			for (int j = 0; (float)j < ikIterationTime; j++)
			{
				points[0] = vector;
				points[points.Count - 1] = vector2;
				if (Harpoons.currentSpellMovement == SpellSpecialMovementType.Rotation)
				{
					if ((Harpoons.currentState == Spell1030Harpoons.HarpoonsState.PullingBack && Tool2D.IgnoreZDistance(Harpoons.transform.position, Harpoons.GetAroundTargetBasePoint()) < 1.3f) || Harpoons.currentState == Spell1030Harpoons.HarpoonsState.HookHolding)
					{
						Vector3 vector3 = (vector2 - vector) / points.Count;
						for (int k = 1; k < points.Count - 1; k++)
						{
							points[k] = Vector3.Lerp(points[k], vector + vector3 * k, 6f * Time.deltaTime);
						}
						continue;
					}
					for (int l = 1; l < points.Count - 1; l++)
					{
						points[l] = points[l - 1] + (points[l] - points[l - 1]).normalized * chainLength;
					}
					for (int num3 = points.Count - 2; num3 >= 0; num3--)
					{
						points[num3] = points[num3 + 1] + (points[num3] - points[num3 + 1]).normalized * chainLength;
					}
					continue;
				}
				switch (Harpoons.currentState)
				{
				case Spell1030Harpoons.HarpoonsState.Shooting:
				case Spell1030Harpoons.HarpoonsState.Holding:
				{
					for (int n = 1; n < points.Count - 1; n++)
					{
						points[n] = points[n - 1] + (points[n] - points[n - 1]).normalized * chainLength;
					}
					for (int num4 = points.Count - 2; num4 > 0; num4--)
					{
						points[num4] = points[num4 + 1] + (points[num4] - points[num4 + 1]).normalized * chainLength;
					}
					break;
				}
				case Spell1030Harpoons.HarpoonsState.HookHolding:
				{
					Vector3 vector5 = (vector2 - vector) / points.Count;
					for (int num5 = 0; num5 < points.Count - 1; num5++)
					{
						float num6 = Mathf.Clamp(((float)points.Count - (float)num5) / (float)points.Count, 0.5f, 1f);
						points[num5] = Vector3.Lerp(points[num5], vector + vector5 * num5, 16f * Time.deltaTime * num6);
					}
					break;
				}
				case Spell1030Harpoons.HarpoonsState.PullingBack:
				{
					Vector3 vector4 = (vector2 - vector) / points.Count;
					for (int m = 1; m < points.Count - 1; m++)
					{
						points[m] = Vector3.Lerp(points[m], vector + vector4 * m, 20f * Time.deltaTime);
					}
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}
		else
		{
			Vector3 normalized = (vector2 - vector).normalized;
			points[0] = vector;
			for (int num7 = 1; num7 < points.Count; num7++)
			{
				points[num7] = points[num7 - 1] + normalized * chainLength;
			}
		}
		if (shadowLine.positionCount != chainLine.positionCount)
		{
			shadowLine.positionCount = chainLine.positionCount;
		}
		for (int num8 = 0; num8 < points.Count; num8++)
		{
			chainLine.SetPosition(num8, points[num8]);
			shadowLine.SetPosition(num8, points[num8] + new Vector3(0f, -0.3f, 1.05f));
		}
	}
}
