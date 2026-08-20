using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using DG.Tweening;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Profiling;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public static class GeneralTool
{
	private static ProfilerMarker PM_GetColliderByTag = new ProfilerMarker("GetColliderByTag");

	private static UnityEngine.Collider[] colliderBuffer = new UnityEngine.Collider[256];

	private static int colliderBufferSize = 0;

	private static UnityEngine.RaycastHit[] raycastHitBuffer = new UnityEngine.RaycastHit[256];

	private static string[] ChineseNumberUnit = new string[6] { "万", "亿", "兆", "京", "垓", "秭" };

	private static string[] EnglishNumberUnit = new string[8] { "K", "M", "B ", "T ", "Qa", "Qi", "Sx", "Sp" };

	public static int GetWeightRandom(params float[] weights)
	{
		float num = 0f;
		for (int i = 0; i < weights.Length; i++)
		{
			if (weights[i] < 0f)
			{
				Debug.LogError("您传入的权重必须>=0");
			}
			num += weights[i];
		}
		float num2 = UnityEngine.Random.Range(0f, num);
		float num3 = 0f;
		for (int j = 0; j < weights.Length; j++)
		{
			num3 += weights[j];
			if (num2 <= num3)
			{
				return j;
			}
		}
		Debug.Log("! 这是不可能的");
		return 0;
	}

	public static int GetRandomEnhancedHarpoons()
	{
		List<int> list = new List<int>();
		if (UnityEngine.Random.Range(0f, 1f) <= 0.06f)
		{
			list.Add(965);
		}
		else if (UnityEngine.Random.Range(0f, 1f) <= 0.5f)
		{
			list.AddRange(new List<int> { 962, 963, 964, 966 });
		}
		else
		{
			list.Add(961);
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public static void TryHealTargetTeammates(UnityEngine.Vector3 healCenterPoint, int healPoint, float healPercent, float healRange, List<Entity> targets)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		for (int i = 0; i < targets.Count; i++)
		{
			if (entityManager.HasComponent<LocalTransform>(targets[i]))
			{
				UnitProperty_Dots componentData = entityManager.GetComponentData<UnitProperty_Dots>(targets[i]);
				LocalTransform componentData2 = entityManager.GetComponentData<LocalTransform>(targets[i]);
				ref float3 position = ref componentData2.Position;
				float3 @float = healCenterPoint;
				float num = DTool.IgnoreZDistance(in position, in @float);
				if (componentData.unitCfg.unitType != 0 && !(num > healRange) && !(componentData.unitCfg.currentHP >= componentData.unitCfg.maxHP))
				{
					float recoveryHP = componentData.unitCfg.maxHP * healPercent / 100f + (float)healPoint;
					UnitDotsSyncSystem.UnitRecoveryHP(targets[i], recoveryHP, entityManager);
				}
			}
		}
	}

	public static bool IsPlayerCanMotion()
	{
		if (!PlayerMgr.Inst.PlayerCtrller.isFrozen && !PlayerMgr.Inst.ItemCtrller.potion_Petrifaction && !PlayerMgr.Inst.PlayerCtrller.CanMotion)
		{
			return false;
		}
		return true;
	}

	public static int GetWeightRandomCompletion(params float[] weights)
	{
		float num = 0f;
		for (int i = 0; i < weights.Length; i++)
		{
			if (weights[i] < 0f)
			{
				Debug.LogError("您传入的权重必须>0");
			}
			num += weights[i];
		}
		if (num >= 1f)
		{
			return GetWeightRandom(weights);
		}
		List<float> list = new List<float>();
		for (int j = 0; j < weights.Length; j++)
		{
			list.Add(weights[j]);
		}
		list.Add(1f - num);
		weights = list.ToArray();
		float value = UnityEngine.Random.value;
		float num2 = 0f;
		for (int k = 0; k < weights.Length; k++)
		{
			num2 += weights[k];
			if (value <= num2)
			{
				return k;
			}
		}
		Debug.Log("! 这是不可能的");
		return 0;
	}

	public static float GetCommonSpellLowFpsTimeScale(float baseThreshHold = 60f, float LowFPSMaxTimeScale = 10f)
	{
		return (GameMgr.Inst.GetFps() >= baseThreshHold) ? 1 : Mathf.CeilToInt(LowFPSMaxTimeScale * (1f - GameMgr.Inst.GetFps() / baseThreshHold));
	}

	public static float GetLowFpsCurreentRatio(float threshold = 30f)
	{
		return Mathf.Min(1f, GameMgr.Inst.GetFps() / threshold);
	}

	public static void RandomizeList<T>(List<T> listToRandomize)
	{
		List<T> list = new List<T>(listToRandomize);
		for (int i = 0; i < listToRandomize.Count; i++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			listToRandomize[i] = list[index];
			list.RemoveAt(index);
		}
	}

	public static T GetRandomElement<T>(params T[] elements)
	{
		if (elements.Length == 0)
		{
			Debug.LogError("传入的数组长度必须大于0");
		}
		return elements[UnityEngine.Random.Range(0, elements.Length)];
	}

	public static bool IsLowFpsOptimizeActive(float fpsThreshold)
	{
		if (ScriptableObjMgr.Inst.testCtrller.DisableLowFrameDynamicOptimize)
		{
			return false;
		}
		return GameMgr.Inst.GetFps() < fpsThreshold;
	}

	public static string FloatToRetainDecimals(float floatNumber, int retainCount)
	{
		if (retainCount < 1)
		{
			Debug.LogError("需要保留的小数位数小于1，请直接用Mathf.Floor或CeilToInt");
			return "";
		}
		string text = floatNumber.ToString("f" + retainCount);
		for (int i = 0; i < retainCount + 1; i++)
		{
			if (text.Substring(text.Length - i - 1, 1) == CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator)
			{
				return text.Substring(0, text.Length - i - 1);
			}
			if (text.Substring(text.Length - i - 1, 1) != "0")
			{
				string text2 = text;
				int num = i;
				return text2.Substring(0, text2.Length - num);
			}
		}
		return text;
	}

	public static bool ChanceResult(float chance)
	{
		return UnityEngine.Random.Range(0f, 1f) < chance;
	}

	public static float HalfChanceNPOne()
	{
		if (!(UnityEngine.Random.Range(0f, 1f) < 0.5f))
		{
			return 1f;
		}
		return -1f;
	}

	public static void ResetTeammatesMotion()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(TeammateData));
		foreach (Entity item in entityQuery.ToEntityArray(Allocator.Temp))
		{
			PhysicsVelocity componentData = entityManager.GetComponentData<PhysicsVelocity>(item);
			componentData.Linear = float3.zero;
			entityManager.SetComponentData(item, componentData);
		}
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			if (wand.WandCfg != null && wand.passiveAutoWand && wand.PassiveWandSpiritEntity != Entity.Null)
			{
				PhysicsVelocity componentData2 = entityManager.GetComponentData<PhysicsVelocity>(wand.PassiveWandSpiritEntity);
				componentData2.Linear = float3.zero;
				entityManager.SetComponentData(wand.PassiveWandSpiritEntity, componentData2);
				UnitBase_Dots componentData3 = entityManager.GetComponentData<UnitBase_Dots>(wand.PassiveWandSpiritEntity);
				componentData3.currentMotion = float3.zero;
				componentData3.targetMotion = float3.zero;
				entityManager.SetComponentData(wand.PassiveWandSpiritEntity, componentData3);
			}
		}
	}

	public static void UpdateThroughMapTeammatesData()
	{
		using EntityQuery entityQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(TeammateData));
		foreach (Entity item in entityQuery.ToEntityArray(Allocator.Temp))
		{
			LevelMgr.Inst.CurrentRoomCtrller.UnitRegister(item);
		}
	}

	public static void SyncTeammatesPosition(UnityEngine.Vector3 movePos)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(TeammateData));
		foreach (Entity item in entityQuery.ToEntityArray(Allocator.Temp))
		{
			LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(item);
			if (entityManager.GetComponentData<SpellConfigComponentData>(item).AbilityType == SpellAbilityType.Summon4)
			{
				movePos = Tool2D.GetNavMeshPointIngoreZ(componentData.Position);
			}
			componentData.Position = movePos;
			entityManager.SetComponentData(item, componentData);
			switch (entityManager.GetComponentData<TeammateData>(item).TeammateType)
			{
			case TeammateType.teammate2:
			{
				Spell2002Data componentData3 = entityManager.GetComponentData<Spell2002Data>(item);
				componentData3.IsPortal = true;
				entityManager.SetComponentData(item, componentData3);
				break;
			}
			case TeammateType.teammate3:
			{
				LocalTransform componentData2 = componentData;
				componentData2.Position = movePos + UnityEngine.Random.insideUnitSphere.IgnoreZ();
				entityManager.SetComponentData(item, componentData2);
				break;
			}
			}
		}
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			if (wand.WandCfg != null && wand.passiveAutoWand && wand.PassiveWandSpiritEntity != Entity.Null)
			{
				LocalTransform componentData4 = entityManager.GetComponentData<LocalTransform>(wand.PassiveWandSpiritEntity);
				componentData4.Position = Tool2D.GetNavMeshPointIngoreZ(movePos) + new UnityEngine.Vector3(0f, 0f, -0.3f);
				entityManager.SetComponentData(wand.PassiveWandSpiritEntity, componentData4);
			}
		}
	}

	public static float GetPreAimTime(UnityEngine.Vector3 delta, float bulletSpeed, UnityEngine.Vector3 targetSpeed)
	{
		float num = targetSpeed.x * targetSpeed.x + targetSpeed.y * targetSpeed.y - bulletSpeed * bulletSpeed;
		float num2 = -2f * (targetSpeed.x * delta.x + targetSpeed.y * delta.y);
		float num3 = delta.x * delta.x + delta.y * delta.y;
		float num4 = num2 * num2 - 4f * num * num3;
		if (num4 < 0f)
		{
			return -1f;
		}
		float num5 = (0f - num2 + Mathf.Pow(num4, 0.5f)) / (2f * num);
		float num6 = (0f - num2 - Mathf.Pow(num4, 0.5f)) / (2f * num);
		if (num5 < 0f && num6 < 0f)
		{
			return -1f;
		}
		if (num5 * num6 < 0f)
		{
			return Mathf.Max(num5, num6);
		}
		return Mathf.Min(num5, num6);
	}

	public static float CannonLandTime(float startVerticalSpeed, float verticalDistance, float gravity)
	{
		float result = 0f;
		float num = gravity * 0.5f;
		float num2 = startVerticalSpeed * startVerticalSpeed - 4f * num * verticalDistance;
		if (num2 > 0f)
		{
			result = (0f - startVerticalSpeed - Mathf.Sqrt(num2)) / 2f / num;
		}
		else
		{
			Debug.LogError("迫击炮方程解有问题! 很可能是参数错误");
		}
		return result;
	}

	public static float CannonSpeed(float startVerticalSpeed, float verticalDistance, float gravity, float horizontalDistance)
	{
		float num = CannonLandTime(startVerticalSpeed, verticalDistance, gravity);
		return horizontalDistance / num;
	}

	public static float CannonAcceleration(float verticalDistance, float startVerticalSpeed, float time)
	{
		return (verticalDistance - startVerticalSpeed * time) * 2f / time / time;
	}

	public static float CannonInitialSpeed(float verticalDistance, float gravity, float time)
	{
		return (verticalDistance - 0.5f * gravity * time * time) / time;
	}

	public static UnityEngine.Collider HaveCollider(UnityEngine.Vector3 startPoint, float radius, string[] tagName)
	{
		UnityEngine.Collider[] array = Physics.OverlapSphere(startPoint, radius);
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < tagName.Length; j++)
			{
				if (array[i].tag == tagName[j])
				{
					return array[i];
				}
			}
		}
		return null;
	}

	public static UnityEngine.Collider HaveColliderInCurrentBuffer(string tagName)
	{
		for (int i = 0; i < colliderBufferSize; i++)
		{
			if (colliderBuffer[i].tag == tagName)
			{
				return colliderBuffer[i];
			}
		}
		return null;
	}

	public static UnityEngine.Collider HaveCollider(UnityEngine.Vector3 checkPoint, float radius, string tagName, string layerName)
	{
		return HaveCollider(checkPoint, radius, tagName, LayerMask.GetMask(layerName));
	}

	public static UnityEngine.Collider HaveCollider(UnityEngine.Vector3 checkPoint, float radius, string tagName, int layer)
	{
		colliderBufferSize = 0;
		while (true)
		{
			colliderBufferSize = Physics.OverlapSphereNonAlloc(checkPoint, radius, colliderBuffer, layer);
			if (colliderBufferSize < colliderBuffer.Length)
			{
				break;
			}
			colliderBuffer = new UnityEngine.Collider[colliderBuffer.Length * 2];
		}
		for (int i = 0; i < colliderBufferSize; i++)
		{
			if (colliderBuffer[i].tag == tagName)
			{
				return colliderBuffer[i];
			}
		}
		return null;
	}

	public static UnityEngine.Collider HaveColliderBox(UnityEngine.Vector3 startPoint, UnityEngine.Vector3 halfExtents, string tagName, int layer)
	{
		UnityEngine.Collider[] array = Physics.OverlapBox(startPoint, halfExtents, UnityEngine.Quaternion.identity, layer);
		if (array.Length != 0)
		{
			return array[0];
		}
		return null;
	}

	public static UnityEngine.Collider GetNearestColliderByTag(UnityEngine.Vector3 startPoint, float radius, params string[] tagNames)
	{
		UnityEngine.Collider[] array = Physics.OverlapSphere(startPoint, radius);
		List<UnityEngine.Collider> list = new List<UnityEngine.Collider>();
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < tagNames.Length; j++)
			{
				if (array[i].tag == tagNames[j])
				{
					list.Add(array[i]);
					break;
				}
			}
		}
		UnityEngine.Collider collider = null;
		if (list.Count > 0)
		{
			collider = list[0];
			for (int k = 1; k < list.Count; k++)
			{
				if ((startPoint - list[k].transform.position).sqrMagnitude < (startPoint - collider.transform.position).sqrMagnitude)
				{
					collider = list[k];
				}
			}
		}
		return collider;
	}

	public static UnityEngine.Collider GetNearestColliderByLayer(UnityEngine.Vector3 startPoint, float radius, UnityEngine.Collider exceptCollider, params string[] layerName)
	{
		UnityEngine.Collider[] array = Physics.OverlapSphere(startPoint, radius, LayerMask.GetMask(layerName));
		List<UnityEngine.Collider> list = new List<UnityEngine.Collider>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != exceptCollider)
			{
				list.Add(array[i]);
			}
		}
		UnityEngine.Collider collider = null;
		if (list.Count > 0)
		{
			collider = list[0];
			for (int j = 1; j < list.Count; j++)
			{
				if ((startPoint - list[j].transform.position).sqrMagnitude < (startPoint - collider.transform.position).sqrMagnitude)
				{
					collider = list[j];
				}
			}
		}
		return collider;
	}

	public static List<UnityEngine.Collider> GetCollidersByTag(UnityEngine.Vector3 startPoint, float radius, params string[] tagNames)
	{
		List<UnityEngine.Collider> list = new List<UnityEngine.Collider>();
		UnityEngine.Collider[] array = Physics.OverlapSphere(startPoint, radius);
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < tagNames.Length; j++)
			{
				if (array[i].CompareTag(tagNames[j]))
				{
					list.Add(array[i]);
					break;
				}
			}
		}
		return list;
	}

	public static int GetCollidersNonAlloc(UnityEngine.Vector3 startPoint, float radius, UnityEngine.Collider[] saveAs, HashSet<string> tagNames, int? layerMask = null)
	{
		int valueOrDefault = layerMask.GetValueOrDefault();
		if (!layerMask.HasValue)
		{
			valueOrDefault = -1;
			layerMask = valueOrDefault;
		}
		colliderBufferSize = 0;
		while (true)
		{
			colliderBufferSize = Physics.OverlapSphereNonAlloc(startPoint, radius, colliderBuffer, layerMask.Value);
			if (colliderBufferSize < colliderBuffer.Length)
			{
				break;
			}
			colliderBuffer = new UnityEngine.Collider[colliderBuffer.Length * 2];
		}
		int num = 0;
		for (int i = 0; i < colliderBufferSize; i++)
		{
			if (tagNames.Contains(colliderBuffer[i].tag))
			{
				if (num >= saveAs.Length)
				{
					Debug.LogError("Buffer 容量不足，丢失了一些碰撞数据！");
					break;
				}
				saveAs[num] = colliderBuffer[i];
				num++;
			}
		}
		return num;
	}

	public static int RaycastNonAlloc(UnityEngine.Vector3 startPoint, UnityEngine.Vector3 direction, float distance, UnityEngine.RaycastHit[] saveAs, HashSet<string> tagNames, int? layerMask = null, float? radius = null)
	{
		int valueOrDefault = layerMask.GetValueOrDefault();
		if (!layerMask.HasValue)
		{
			valueOrDefault = -1;
			layerMask = valueOrDefault;
		}
		colliderBufferSize = 0;
		while (true)
		{
			if (radius.HasValue)
			{
				colliderBufferSize = Physics.SphereCastNonAlloc(startPoint, radius.Value, direction, raycastHitBuffer, distance, layerMask.Value);
			}
			else
			{
				colliderBufferSize = Physics.RaycastNonAlloc(startPoint, direction, raycastHitBuffer, distance, layerMask.Value);
			}
			if (colliderBufferSize < raycastHitBuffer.Length)
			{
				break;
			}
			raycastHitBuffer = new UnityEngine.RaycastHit[raycastHitBuffer.Length * 2];
		}
		int num = 0;
		for (int i = 0; i < colliderBufferSize; i++)
		{
			if (!(raycastHitBuffer[i].collider == null) && tagNames.Contains(raycastHitBuffer[i].collider.tag))
			{
				if (num >= saveAs.Length)
				{
					Debug.LogError("Buffer 容量不足，丢失了一些碰撞数据！");
					break;
				}
				saveAs[num] = raycastHitBuffer[i];
				num++;
			}
		}
		return num;
	}

	public static List<UnityEngine.Collider> GetBoxCollidersByTag(UnityEngine.Vector3 startPoint, UnityEngine.Vector3 boxSize, quaternion rotate, LayerMask attackLayers, params string[] tagNames)
	{
		List<UnityEngine.Collider> list = new List<UnityEngine.Collider>();
		UnityEngine.Collider[] array = Physics.OverlapBox(startPoint, boxSize / 2f, rotate, attackLayers);
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < tagNames.Length; j++)
			{
				if (array[i].tag == tagNames[j])
				{
					list.Add(array[i]);
					break;
				}
			}
		}
		return list;
	}

	public static List<UnityEngine.Collider> GetCollidersByTagAndCheckCamp(UnityEngine.Vector3 startPoint, float radius, UnitType type, bool sameCamp, params string[] tagNames)
	{
		List<UnityEngine.Collider> list = new List<UnityEngine.Collider>();
		UnityEngine.Collider[] array = Physics.OverlapSphere(startPoint, radius);
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < tagNames.Length; j++)
			{
				if (!(array[i].tag == tagNames[j]))
				{
					continue;
				}
				if (sameCamp)
				{
					if (array[i].gameObject.GetComponentInParent<SpellBase>() != null)
					{
						if (IsSameCamp(array[i].gameObject.GetComponentInParent<SpellBase>().ownerPpt.unitCfg.unitType, type))
						{
							list.Add(array[i]);
						}
					}
					else if (array[i].gameObject.GetComponentInParent<UnitProperty>() != null && IsSameCamp(array[i].gameObject.GetComponentInParent<UnitProperty>().unitCfg.unitType, type))
					{
						list.Add(array[i]);
					}
				}
				else if (array[i].gameObject.GetComponentInParent<SpellBase>() != null)
				{
					if (!IsSameCamp(array[i].gameObject.GetComponentInParent<SpellBase>().ownerPpt.unitCfg.unitType, type))
					{
						list.Add(array[i]);
					}
				}
				else if (array[i].gameObject.GetComponentInParent<UnitProperty>() != null && !IsSameCamp(array[i].gameObject.GetComponentInParent<UnitProperty>().unitCfg.unitType, type))
				{
					list.Add(array[i]);
				}
				break;
			}
		}
		return list;
	}

	public static bool IsSameCamp(UnitType targetA, UnitType targetB)
	{
		if (targetA == UnitType.Player || targetA == UnitType.Teammate || targetA == UnitType.TeammateNotAttack || targetB == UnitType.NotAttack || targetB == UnitType.Brittleness)
		{
			if (targetB == UnitType.Player || targetB == UnitType.Teammate || targetB == UnitType.TeammateNotAttack || targetB == UnitType.NotAttack || targetB == UnitType.Brittleness)
			{
				return true;
			}
			return false;
		}
		if (targetA == UnitType.Monster || targetA == UnitType.Elite || targetA == UnitType.Boss || targetA == UnitType.WillAttack)
		{
			if (targetB == UnitType.Monster || targetB == UnitType.Elite || targetB == UnitType.Boss || targetB == UnitType.WillAttack)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static List<UnityEngine.Collider> GetCollidersByTagInAngle(UnityEngine.Vector3 startPoint, float radius, UnityEngine.Vector3 fromDir, float halfAngle, params string[] tagNames)
	{
		List<UnityEngine.Collider> list = new List<UnityEngine.Collider>();
		UnityEngine.Collider[] array = Physics.OverlapSphere(startPoint, radius);
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < tagNames.Length; j++)
			{
				if (array[i].tag == tagNames[j])
				{
					if (UnityEngine.Vector3.Angle(fromDir, Tool2D.IgnoreZV2ToV1Normal(array[i].transform.position, startPoint)) <= halfAngle)
					{
						list.Add(array[i]);
					}
					break;
				}
			}
		}
		return list;
	}

	public static UnityEngine.Vector3 QuadraticBezierCurve(UnityEngine.Vector3 v0, UnityEngine.Vector3 v1, UnityEngine.Vector3 v2, float t)
	{
		return (1f - t) * (1f - t) * v0 + 2f * (1f - t) * t * v1 + t * t * v2;
	}

	public static UnityEngine.Vector3 CubicBezierCurve(UnityEngine.Vector3 v0, UnityEngine.Vector3 v1, UnityEngine.Vector3 v2, UnityEngine.Vector3 v3, float t)
	{
		return (1f - t) * (1f - t) * (1f - t) * v0 + 3f * (1f - t) * (1f - t) * t * v1 + 3f * (1f - t) * t * t * v2 + t * t * t * v3;
	}

	public static int ArrangementNumber(int n, int m)
	{
		if (n == 0)
		{
			return 1;
		}
		int num = 1;
		for (int num2 = m; num2 > 0; num2--)
		{
			num *= n;
			n--;
		}
		return num;
	}

	public static int CombinationNumber(int n, int m)
	{
		if (n == 0)
		{
			Debug.LogError("n不能为0！");
			return 0;
		}
		m = Mathf.Min(m, n - m);
		return ArrangementNumber(n, m) / ArrangementNumber(m, m);
	}

	public static UnityEngine.Vector3 FreeBezierCurve(float t, params UnityEngine.Vector3[] points)
	{
		int num = points.Length;
		if (num < 3)
		{
			Debug.LogWarning("贝塞尔曲线需要3个及以上的点才能计算");
			return UnityEngine.Vector3.zero;
		}
		UnityEngine.Vector3 result = default(UnityEngine.Vector3);
		for (int i = 0; i < num; i++)
		{
			result += Mathf.Pow(1f - t, num - i - 1) * Mathf.Pow(t, i) * (float)CombinationNumber(num - 1, i) * points[i];
		}
		return result;
	}

	public static UnityEngine.Vector3 WorldToCanvasLocalPoint(UnityEngine.Vector3 worldPoint)
	{
		UnityEngine.Vector3 result = CamController.Inst.cam_Main.WorldToViewportPoint(worldPoint);
		result.x = (float)Display.main.renderingWidth / (float)Display.main.renderingHeight * 1080f * (result.x - 0.5f);
		result.y = 1080f * (result.y - 0.5f);
		result.z = 0f;
		return result;
	}

	public static UnityEngine.Vector3 ScreenPositionToCanvasPosition(UnityEngine.Vector3 _mousePoint, Canvas refCam, Camera camera)
	{
		if (GameMgr.IsMobile_Static)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(refCam.transform as RectTransform, _mousePoint, camera, out var localPoint);
			return localPoint;
		}
		_mousePoint.x = (float)Display.main.renderingWidth / (float)Display.main.renderingHeight * UIMgr.Inst.canvas_1Scaler.referenceResolution.y * (_mousePoint.x / (float)Screen.width - 0.5f);
		_mousePoint.y = UIMgr.Inst.canvas_1Scaler.referenceResolution.y * (_mousePoint.y / (float)Screen.height - 0.5f);
		return _mousePoint;
	}

	public static UnityEngine.Vector3 WorldToCanvasLocalPoint_FitDisplay(UnityEngine.Vector3 worldPoint, CanvasScaler canvasScaler = null)
	{
		if (canvasScaler != null)
		{
			UnityEngine.Vector3 result = CamController.Inst.cam_Main.WorldToViewportPoint(worldPoint);
			result.x = (float)Display.main.renderingWidth / (float)Display.main.renderingHeight * canvasScaler.referenceResolution.y * (result.x - 0.5f);
			result.y = canvasScaler.referenceResolution.y * (result.y - 0.5f);
			result.z = 0f;
			return result;
		}
		if (!GameMgr.IsMobile_Static && !GameMgr.IsSteamDeck_Static)
		{
			UnityEngine.Vector3 result2 = CamController.Inst.cam_Main.WorldToViewportPoint(worldPoint);
			result2.x = (float)Display.main.renderingWidth / (float)Display.main.renderingHeight * 1080f * (result2.x - 0.5f);
			result2.y = 1080f * (result2.y - 0.5f);
			result2.z = 0f;
			return result2;
		}
		UnityEngine.Vector3 result3 = CamController.Inst.cam_Main.WorldToViewportPoint(worldPoint);
		result3.x = (float)Display.main.renderingWidth * (float)MobileMgr.inst.scalerhight / (float)Display.main.renderingHeight * (result3.x - 0.5f);
		result3.y = (float)MobileMgr.inst.scalerhight * (result3.y - 0.5f);
		result3.z = 0f;
		return result3;
	}

	public static UnityEngine.Vector3 WorldToCanvasLocalPoint(UnityEngine.Vector3 worldPoint, Canvas canvas, Camera cam)
	{
		float width = canvas.gameObject.GetComponent<RectTransform>().rect.width;
		float height = canvas.gameObject.GetComponent<RectTransform>().rect.height;
		UnityEngine.Vector3 result = cam.WorldToViewportPoint(worldPoint);
		result.x = width * (result.x - 0.5f);
		result.y = height * (result.y - 0.5f);
		result.z = 0f;
		return result;
	}

	public static string GetRomanNumber(int number)
	{
		return number switch
		{
			1 => "Ⅰ", 
			2 => "Ⅱ", 
			3 => "Ⅲ", 
			4 => "Ⅳ", 
			5 => "Ⅴ", 
			6 => "Ⅵ", 
			7 => "Ⅶ", 
			8 => "Ⅷ", 
			9 => "Ⅸ", 
			_ => "Ⅹ", 
		};
	}

	public static Color GetRarityColor(ItemDropType type)
	{
		switch (type)
		{
		case ItemDropType.None:
			return GameConst.color_RarityCommon;
		case ItemDropType.Common:
			return GameConst.color_RarityCommon;
		case ItemDropType.Rare:
			return GameConst.color_RarityRare;
		case ItemDropType.Epic:
			return GameConst.color_RarityEpic;
		case ItemDropType.Special:
			return GameConst.color_RaritySpecial;
		default:
			Debug.LogError(type);
			return GameConst.color_RarityCommon;
		}
	}

	public static float Remap(UnityEngine.Vector2 inRange, UnityEngine.Vector2 outRange, float inValue)
	{
		if (inRange.x >= inRange.y)
		{
			Debug.LogError("inRange区间错误 x值应该小于y值");
			return 0f;
		}
		if (outRange.x >= outRange.y)
		{
			Debug.LogError("outRange区间错误 x值应该小于y值");
			return 0f;
		}
		if (inValue < inRange.x)
		{
			return outRange.x;
		}
		if (inValue > inRange.y)
		{
			return outRange.y;
		}
		float t = 1f - (inRange.y - inValue) / (inRange.y - inRange.x);
		return Mathf.Lerp(outRange.x, outRange.y, t);
	}

	public static List<T> ListShuffle<T>(List<T> list)
	{
		List<T> list2 = new List<T>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			list2.Add(list[index]);
			list.RemoveAt(index);
		}
		return list2;
	}

	public static void ListShuffle<T>(ref List<T> list)
	{
		List<T> list2 = list;
		List<T> list3 = new List<T>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			int index = UnityEngine.Random.Range(0, list2.Count);
			list3.Add(list2[index]);
			list2.RemoveAt(index);
		}
		list = list3;
	}

	public static HashSet<int> GetRandomIndex(int maxIndex, int maxCount)
	{
		HashSet<int> hashSet = new HashSet<int>();
		if (maxCount >= maxIndex)
		{
			for (int i = 0; i < maxIndex; i++)
			{
				hashSet.Add(i);
			}
		}
		else
		{
			while (hashSet.Count < maxCount)
			{
				hashSet.Add(UnityEngine.Random.Range(0, maxIndex));
			}
		}
		return hashSet;
	}

	public static void InitialSpriteMaterial(SpriteRenderer sprite)
	{
		UnityEngine.Material material = sprite.material;
		material = (sprite.material = UnityEngine.Object.Instantiate(material));
	}

	public static void InitialImageMaterial(Image sprite)
	{
		UnityEngine.Material material = sprite.material;
		material = (sprite.material = UnityEngine.Object.Instantiate(material));
	}

	public static float GetSpellRadiusToDamageRatio(float finalRadius, float decreaseRatio, float radiusToDamageRatio)
	{
		return finalRadius / decreaseRatio * (1f - decreaseRatio) * radiusToDamageRatio;
	}

	public static float GetSpellSpeedToBonusDuration(float finalSpeed, float decreaseRatio, float speedToDurationRatio)
	{
		return finalSpeed * (1f - decreaseRatio) * speedToDurationRatio;
	}

	public static string TimeStampToTime(string timeStamp, LanguageType languageType)
	{
		if (!long.TryParse(timeStamp, out var result))
		{
			throw new ArgumentException("Invalid timeStamp format.");
		}
		if (result.ToString().Length <= 10)
		{
			result *= 1000;
		}
		DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(result), TimeZoneInfo.Local);
		if (!GameConstManaged.LanguageStrings.TryGetValue(languageType, out var value))
		{
			return dateTime.ToString();
		}
		CultureInfo cultureInfo = new CultureInfo(value);
		return dateTime.ToString("F", cultureInfo);
	}

	public static string FormatTimestamp(long timestamp)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
	}

	public static void TextFormat(Text text)
	{
		string[] array = text.text.Split("@n");
		text.text = "";
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			text.text += text2;
			text.text += "\n";
		}
	}

	public static void TextFormat(Text text, int SizeDown)
	{
		string[] array = text.text.Split("@n");
		text.text = "";
		for (int i = 0; i < array.Length; i++)
		{
			if (i == 0)
			{
				text.text += array[i];
				continue;
			}
			text.text = text.text + "\n<size=" + (text.fontSize - SizeDown) + ">" + array[i] + "</size>";
		}
	}

	public static void ulongToTwoInts(ulong ulongValue, out int part1, out int part2)
	{
		part1 = (int)(ulongValue & 0xFFFFFFFFu);
		part2 = (int)(ulongValue >> 32);
	}

	public static ulong twoIntsToUlong(int part1, int part2)
	{
		return (ulong)(((long)part2 << 32) | (uint)part1);
	}

	public static float GetSpellEnhancedDamage(float InitialDamage, SpellBase targetbase)
	{
		return Mathf.Ceil(InitialDamage * targetbase.damageRatio * targetbase.finalDamageRatio);
	}

	public static float GetSpellEnhancedSize(float InitialSize, SpellBase targetBase)
	{
		return InitialSize * targetBase.radiusRatio * targetBase.finalRadiusRatio;
	}

	public static void Action<T>(this IEnumerable<T> source, Action<T> func)
	{
		foreach (T item in source)
		{
			func(item);
		}
	}

	public static bool IndexOutRange<T>(this T[] arr, int index)
	{
		if (index >= 0)
		{
			return index >= arr.Length;
		}
		return true;
	}

	public static bool IndexOutRange<T>(this List<T> arr, int index)
	{
		if (index >= 0)
		{
			return index >= arr.Count;
		}
		return true;
	}

	public static T GetOrDefault<T>(this T[] arr, int index, T def = default(T))
	{
		if (!arr.IndexOutRange(index))
		{
			return arr[index];
		}
		return def;
	}

	public static T GetOrDefault<T>(this List<T> arr, int index, T def = default(T))
	{
		if (!arr.IndexOutRange(index))
		{
			return arr[index];
		}
		return def;
	}

	public static TextGenerator PreRenderTextInRect(Text text, string textInfo, TextGenerationSettings? generationSettings = null)
	{
		TextGenerationSettings settings = (generationSettings.HasValue ? generationSettings.Value : text.GetGenerationSettings(new UnityEngine.Vector2(((RectTransform)text.transform).rect.width, ((RectTransform)text.transform).rect.height)));
		TextGenerator cachedTextGenerator = text.cachedTextGenerator;
		cachedTextGenerator.Populate(textInfo, settings);
		return cachedTextGenerator;
	}

	public static string FormatTextIfPublishTest(Text text, string textInfo)
	{
		text.text = textInfo;
		LayoutRebuilder.ForceRebuildLayoutImmediate(text.rectTransform);
		if (!string.IsNullOrEmpty(textInfo) && ScriptableObjMgr.Inst.testCtrller.publishTesting)
		{
			return FormatText(text, textInfo);
		}
		return textInfo;
	}

	private static string FormatText(Text text, string textInfo)
	{
		string text2 = textInfo;
		text.TryGetComponent<ContentSizeFitter>(out var component);
		TextGenerationSettings value = ((!(component != null) || component.horizontalFit != ContentSizeFitter.FitMode.PreferredSize) ? text.GetGenerationSettings(new UnityEngine.Vector2(((RectTransform)text.transform).rect.width, text.preferredHeight)) : text.GetGenerationSettings(new UnityEngine.Vector2(text.preferredWidth, text.preferredHeight)));
		TextGenerator textGenerator = PreRenderTextInRect(text, textInfo, value);
		int lineCount = textGenerator.lineCount;
		if (value.generationExtents.x == 0f)
		{
			Debug.Log(text.gameObject.activeInHierarchy);
			Debug.LogError(text2);
			return text2;
		}
		for (int i = 0; i < lineCount; i++)
		{
			if (lineCount > 20)
			{
				return text2;
			}
			UILineInfo uILineInfo = textGenerator.lines[i];
			if (textInfo.Length <= uILineInfo.startCharIdx)
			{
				continue;
			}
			if (uILineInfo.startCharIdx != 0 && uILineInfo.startCharIdx - 1 != 0 && (!CharCanLineEnd(textInfo[uILineInfo.startCharIdx - 1]) || (!CharCanCrossLine(textInfo[uILineInfo.startCharIdx - 1]) && char.IsDigit(textInfo[uILineInfo.startCharIdx]))))
			{
				int num = uILineInfo.startCharIdx;
				int num2 = 1;
				bool flag = false;
				for (int num3 = uILineInfo.startCharIdx - 2; num3 > 0; num3--)
				{
					if (textInfo[num3] == '>')
					{
						flag = true;
					}
					if (flag)
					{
						if (textInfo[num3] == '<')
						{
							flag = false;
						}
						num2++;
					}
					else
					{
						if (CharCanLineStart(textInfo[num3]) && CharCanCrossLine(textInfo[num3]))
						{
							num = num3 + 1;
							break;
						}
						num2++;
					}
				}
				if (num == uILineInfo.startCharIdx)
				{
					Debug.LogError("出错");
				}
				textInfo = textInfo.Insert(num, "\n");
				text.text = textInfo;
				value = text.GetGenerationSettings(new UnityEngine.Vector2(((RectTransform)text.transform).rect.width, text.preferredHeight));
				textGenerator = PreRenderTextInRect(text, textInfo, value);
				lineCount = textGenerator.lineCount;
			}
			if (CharCanLineStart(textInfo[uILineInfo.startCharIdx]) || uILineInfo.startCharIdx == 0)
			{
				continue;
			}
			int num4 = uILineInfo.startCharIdx;
			int num5 = 1;
			bool flag2 = false;
			for (int num6 = uILineInfo.startCharIdx - 1; num6 > 0; num6--)
			{
				if (textInfo[num6] == '>')
				{
					flag2 = true;
				}
				if (flag2)
				{
					if (textInfo[num6] == '<')
					{
						flag2 = false;
					}
					num5++;
				}
				else
				{
					if (CharCanLineStart(textInfo[num6]) && CharCanCrossLine(textInfo[num6]))
					{
						num4 = num6 + 1;
						break;
					}
					num5++;
				}
			}
			if (num4 == uILineInfo.startCharIdx)
			{
				num4--;
			}
			textInfo = textInfo.Insert(num4, "\n");
			text.text = textInfo;
			value = text.GetGenerationSettings(new UnityEngine.Vector2(((RectTransform)text.transform).rect.width, text.preferredHeight));
			textGenerator = PreRenderTextInRect(text, textInfo, value);
			lineCount = textGenerator.lineCount;
		}
		return textInfo;
	}

	public static bool CharCanLineStart(char c)
	{
		if (TextMgr.charCantAtStart.Contains(c))
		{
			return false;
		}
		return true;
	}

	private static bool CharCanLineEnd(char c)
	{
		return !TextMgr.charCantAtEnd.Contains(c);
	}

	private static bool CharCanCrossLine(char c)
	{
		if (!char.IsDigit(c) && !TextMgr.charCantAtEnd.Contains(c))
		{
			return !TextMgr.charCanEndCantCross.Contains(c);
		}
		return false;
	}

	public static bool ListContentEquals<T>(List<T> x, List<T> y)
	{
		if (x == null && y == null)
		{
			return true;
		}
		if (x == null || y == null)
		{
			return false;
		}
		if (x.Count != y.Count)
		{
			return false;
		}
		List<T> list = y.ToList();
		while (list.Count > 0)
		{
			if (!x.Contains(list[0]))
			{
				return false;
			}
			list.RemoveAt(0);
		}
		return true;
	}

	public static T RecursiveComponentSearchDepth<T>(Transform parent, int depthLeft = 99) where T : Component
	{
		if (depthLeft <= 0)
		{
			return null;
		}
		foreach (Transform item in parent)
		{
			T[] components = item.GetComponents<T>();
			int num = 0;
			if (num < components.Length)
			{
				return components[num];
			}
			T val = RecursiveComponentSearchDepth<T>(item, depthLeft - 1);
			if ((UnityEngine.Object)val != (UnityEngine.Object)null)
			{
				return val;
			}
		}
		return null;
	}

	public static GameObject FindChildrenByName(Transform parent, string nameToFind, int depth)
	{
		GameObject result = null;
		if (depth <= 0)
		{
			return result;
		}
		foreach (Transform item in parent)
		{
			if (item.gameObject.name == nameToFind)
			{
				return item.gameObject;
			}
			result = FindChildrenByName(item, nameToFind, depth - 1);
			if (result != null)
			{
				return result;
			}
		}
		return null;
	}

	public static void ScrollToPadSelected(ScrollRect scrollRect, RectTransform contentRect, RectTransform targetTransform, bool doTween = true)
	{
		float height = contentRect.rect.height;
		float height2 = scrollRect.viewport.rect.height;
		if (height <= height2)
		{
			return;
		}
		UnityEngine.Vector2 vector = contentRect.InverseTransformPoint(targetTransform.position);
		vector.y = 0f - vector.y;
		float height3 = scrollRect.viewport.rect.height;
		float y = Mathf.Clamp(vector.y - height3 / 2f, 0f, contentRect.rect.height - height3);
		if (doTween)
		{
			DOTween.To(() => contentRect.anchoredPosition, delegate(UnityEngine.Vector2 x)
			{
				contentRect.anchoredPosition = x;
			}, new UnityEngine.Vector2(contentRect.anchoredPosition.x, y), 0.2f).SetUpdate(isIndependentUpdate: true);
		}
		else
		{
			contentRect.anchoredPosition = new UnityEngine.Vector2(contentRect.anchoredPosition.x, y);
		}
	}

	public static bool LayerMaskContains(LayerMask baseMask, int layerToCheck)
	{
		return (baseMask.value & (1 << layerToCheck)) > 0;
	}

	public static string ToStringHP(this float hp)
	{
		if (hp > 0f && hp < 1f)
		{
			hp = 1f;
		}
		return hp.ToString("F0");
	}

	public static string ToStringDamage(this float damage)
	{
		return Mathf.Ceil(damage).FormatWithUnit();
	}

	public static string FormatWithUnit(this float number)
	{
		if (!number.IsValidFloat())
		{
			Debug.LogError($"异常的格式化数字：{number}\n" + Environment.StackTrace);
			return "0";
		}
		string unit;
		return ((double)number).FormatWithUnit(out unit) + unit;
	}

	public static string FormatWithUnit(this BigInteger number)
	{
		string unit;
		return ((double)number).FormatWithUnit(out unit) + unit;
	}

	public static string FormatWithUnit(this float number, out string unit)
	{
		return ((double)number).FormatWithUnit(out unit);
	}

	private static string FormatWithUnit(this double number, out string unit)
	{
		unit = null;
		double num = 1.0;
		number = ((number < 1.0) ? System.Math.Ceiling(number) : System.Math.Round(number));
		string text = "";
		LanguageType language = DataMgr.settingData.language;
		if (language == LanguageType.ChineseS || language == LanguageType.ChineseT)
		{
			for (int i = 0; i < ChineseNumberUnit.Length; i++)
			{
				if (number >= num * 10000.0)
				{
					num *= 10000.0;
					unit = ChineseNumberUnit[i];
				}
				int length = (number / num).ToString("F0").Length;
				text = ((length > 4) ? "9999" : (number / num).ToString("F" + (4 - length), CultureInfo.InvariantCulture));
				if (text.Contains('.'))
				{
					text = text.TrimEnd('0').TrimEnd('.');
				}
			}
		}
		else
		{
			for (int j = 0; j < EnglishNumberUnit.Length; j++)
			{
				if (number >= num * 1000.0)
				{
					num *= 1000.0;
					unit = EnglishNumberUnit[j];
				}
				int length2 = (number / num).ToString("F0").Length;
				text = ((length2 > 3) ? "999" : (number / num).ToString("F" + (3 - length2), CultureInfo.InvariantCulture));
				if (text.Contains('.'))
				{
					text = text.TrimEnd('0').TrimEnd('.');
				}
			}
		}
		return text;
	}

	public static Texture2D LoadTexturesFromBase64(string encoded)
	{
		try
		{
			byte[] data = Convert.FromBase64String(encoded);
			Texture2D texture2D = new Texture2D(2, 2);
			if (texture2D.LoadImage(data))
			{
				return texture2D;
			}
			Debug.LogError("图片解码失败");
			return null;
		}
		catch (Exception ex)
		{
			Debug.LogError("Base64 解码失败：" + ex.Message);
			return null;
		}
	}

	public static T AddComponent<T>(this Component component) where T : Component
	{
		return component.gameObject.AddComponent<T>();
	}

	public static bool IsDestroyed(this UnityEngine.Object target)
	{
		if ((object)target != null)
		{
			return target == null;
		}
		return false;
	}

	public static bool IsValidFloat(this float value)
	{
		if (float.IsFinite(value))
		{
			if (!float.IsNormal(value))
			{
				return value == 0f;
			}
			return true;
		}
		return false;
	}
}
