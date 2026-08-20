using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tool2D
{
	public static Vector3 V3MultV3(Vector3 v1, Vector3 v2)
	{
		return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
	}

	public static bool IsClockWiseGapAngleSmaller(Vector3 fromDirection, Vector3 toDirection)
	{
		Vector3 vector = fromDirection.IgnoreZ();
		Vector3 vector2 = toDirection.IgnoreZ();
		if (!(vector.x * vector2.y - vector.y * vector2.x < 0f))
		{
			return false;
		}
		return true;
	}

	public static Vector3 Project(Vector3 a, Vector3 b)
	{
		return Vector3.Dot(a, b) / Vector3.Dot(b, b) * b;
	}

	public static Vector3 Projection(Vector3 direction, Vector3 normal)
	{
		return direction - normal * Vector3.Dot(normal, direction);
	}

	public static Vector3 GetDir()
	{
		return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f).normalized;
	}

	public static Vector3 GetDir(float degree)
	{
		return Quaternion.Euler(0f, 0f, degree) * Vector2.up;
	}

	public static Vector3 GetDir(Vector3 oldDir, float degree)
	{
		return Quaternion.Euler(0f, 0f, degree) * oldDir;
	}

	public static Vector3 GetDirByFourDir(FourDir dir)
	{
		switch (dir)
		{
		case FourDir.Up:
			return Vector3.up;
		case FourDir.Right:
			return Vector3.right;
		case FourDir.Down:
			return Vector3.down;
		case FourDir.Left:
			return Vector3.left;
		default:
			Debug.LogError(dir);
			return GetDir(0f);
		}
	}

	public static Vector3 GetDirByFourDirInverted(FourDir dir)
	{
		switch (dir)
		{
		case FourDir.Up:
			return GetDir(180f);
		case FourDir.Right:
			return GetDir(90f);
		case FourDir.Down:
			return GetDir(0f);
		case FourDir.Left:
			return GetDir(270f);
		default:
			Debug.LogError(dir);
			return GetDir(0f);
		}
	}

	public static FourDir GetRandomFourDir()
	{
		int num = UnityEngine.Random.Range(0, 4);
		switch (num)
		{
		case 0:
			return FourDir.Up;
		case 1:
			return FourDir.Left;
		case 2:
			return FourDir.Down;
		case 3:
			return FourDir.Right;
		default:
			Debug.LogError(num);
			return FourDir.Up;
		}
	}

	public static float GetDegree()
	{
		return UnityEngine.Random.Range(0f, 360f);
	}

	public static float GetDegree(Vector2 dir)
	{
		dir = GetDir(dir, -90f);
		float num = Mathf.Atan2(dir.y, dir.x) / MathF.PI * 180f;
		if (num < 0f)
		{
			num += 360f;
		}
		return num;
	}

	public static float GetDegree(float x, float y)
	{
		Vector2 vector = GetDir(new Vector2(x, y), -90f);
		float num = Mathf.Atan2(vector.y, vector.x) / MathF.PI * 180f;
		if (num < 0f)
		{
			num += 360f;
		}
		return num;
	}

	public static Vector2 RadianToVector2(float radian)
	{
		return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
	}

	public static Vector2 DegreeToVector2(float degree)
	{
		return RadianToVector2(degree * (MathF.PI / 180f));
	}

	public static Quaternion GetRotation()
	{
		return Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
	}

	public static Quaternion GetRotation(float z)
	{
		return Quaternion.Euler(0f, 0f, z);
	}

	public static Quaternion GetRotation(Quaternion oldRotation, float angle)
	{
		return Quaternion.Euler(oldRotation.eulerAngles.x, oldRotation.eulerAngles.y, oldRotation.eulerAngles.z + angle);
	}

	public static Quaternion GetRotationByFourDir(FourDir dir)
	{
		switch (dir)
		{
		case FourDir.Up:
			return GetRotation(0f);
		case FourDir.Right:
			return GetRotation(270f);
		case FourDir.Down:
			return GetRotation(180f);
		case FourDir.Left:
			return GetRotation(90f);
		default:
			Debug.LogError(dir);
			return GetRotation(0f);
		}
	}

	public static Quaternion GetRotationByFourDirInverted(FourDir dir)
	{
		switch (dir)
		{
		case FourDir.Up:
			return GetRotation(180f);
		case FourDir.Right:
			return GetRotation(90f);
		case FourDir.Down:
			return GetRotation(0f);
		case FourDir.Left:
			return GetRotation(270f);
		default:
			Debug.LogError(dir);
			return GetRotation(0f);
		}
	}

	public static float IgnoreZAngleWithSign(Vector3 to)
	{
		Vector3 up = Vector3.up;
		to = IgnoreZPoint(to);
		Vector3 rhs = -Vector3.Cross(up, to);
		float num = Vector3.Angle(IgnoreZPoint(up), IgnoreZPoint(to));
		float num2 = Mathf.Sign(Vector3.Dot(new Vector3(0f, 0f, -1f), rhs));
		return num * num2;
	}

	public static float IgnoreZAngleWithSign(Vector3 from, Vector3 to)
	{
		from = IgnoreZPoint(from);
		to = IgnoreZPoint(to);
		Vector3 rhs = -Vector3.Cross(from, to);
		float num = Vector3.Angle(IgnoreZPoint(from), IgnoreZPoint(to));
		float num2 = Mathf.Sign(Vector3.Dot(new Vector3(0f, 0f, -1f), rhs));
		return num * num2;
	}

	public static float IgnoreZAngle360(Vector3 from, Vector3 to)
	{
		float num = IgnoreZAngleWithSign(from, to);
		if (num < 0f)
		{
			num = 360f + num;
		}
		return num;
	}

	public static Vector3 GetEulerAngleByDir(Vector3 dir)
	{
		dir = IgnoreZPoint(dir);
		return new Vector3(0f, 0f, IgnoreZAngleWithSign(Vector3.up, dir));
	}

	public static float IgnoreZAngle(Vector3 from, Vector3 to)
	{
		return Vector3.Angle(IgnoreZPoint(from), IgnoreZPoint(to));
	}

	public static float IgnoreZAngleClamp90(Vector3 from, Vector3 to)
	{
		float num = IgnoreZAngle(from, to);
		if (num > 90f)
		{
			return 180f - num;
		}
		return num;
	}

	public static Vector2 IgnoreZAngleXYClamp90(Vector3 dir)
	{
		return new Vector2(IgnoreZAngleClamp90(dir, Vector3.right), IgnoreZAngleClamp90(dir, Vector3.up));
	}

	public static Vector3 IgnoreZPoint(Vector3 v, float z)
	{
		return new Vector3(v.x, v.y, z);
	}

	public static Vector3 IgnoreZPoint(Vector3 v)
	{
		return IgnoreZPoint(v, 0f);
	}

	public static Vector3 IgnoreZPoint(Transform t)
	{
		return IgnoreZPoint(t.position, 0f);
	}

	public static Vector3 IgnoreZPoint(Transform t, float z)
	{
		return IgnoreZPoint(t.position, z);
	}

	public static Vector3 IgnoreZV2ToV1Normal(Vector3 v1, Vector3 v2)
	{
		return (IgnoreZPoint(v1) - IgnoreZPoint(v2)).normalized;
	}

	public static Vector3 IgnoreZV2ToV1Normal(Transform t1, Transform t2)
	{
		return IgnoreZV2ToV1Normal(t1.position, t2.position);
	}

	public static Vector3 IgnoreZV2ToV1(Vector3 v1, Vector3 v2)
	{
		return IgnoreZPoint(v1) - IgnoreZPoint(v2);
	}

	public static Vector3 IgnoreZV2ToV1(Transform t1, Transform t2)
	{
		return IgnoreZV2ToV1(t1.position, t2.position);
	}

	public static float IgnoreZDistance(Vector3 v1, Vector3 v2)
	{
		return Vector3.Distance(IgnoreZPoint(v1), IgnoreZPoint(v2));
	}

	public static float IgnoreZDistance(Transform t1, Transform t2)
	{
		return IgnoreZDistance(t1.position, t2.position);
	}

	public static float IgnoreZDistanceSqr(Vector3 v1, Vector3 v2)
	{
		return (IgnoreZPoint(v1) - IgnoreZPoint(v2)).sqrMagnitude;
	}

	public static float IgnoreZDistanceSqr(Transform t1, Transform t2)
	{
		return IgnoreZDistanceSqr(t1.position, t2.position);
	}

	public static float NavMaskZOffset(int navAreaMask)
	{
		return navAreaMask switch
		{
			8 => 4.35f, 
			16 => -0.05f, 
			32 => 4.25f, 
			_ => -1f, 
		};
	}

	public static NavMeshPath GetNavMeshPath(Vector3 startPoint, Vector3 endPoint, int navAreaMask = 16)
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		Vector3 navMeshPoint = GetNavMeshPoint(startPoint, navAreaMask);
		Vector3 navMeshPoint2 = GetNavMeshPoint(endPoint, navAreaMask);
		if (!NavMesh.CalculatePath(navMeshPoint, navMeshPoint2, navAreaMask, navMeshPath))
		{
			Debug.Log("没有路径 这不应该发生！");
		}
		return navMeshPath;
	}

	public static bool IsTargetPointBlockByWall(Vector3 startPoint, Vector3 endPoint)
	{
		float num = IgnoreZDistanceSqr(startPoint, endPoint);
		Ray ray = new Ray(endPoint, IgnoreZV2ToV1Normal(startPoint, endPoint));
		Collider[] array = new Collider[32];
		HashSet<string> tagNames = new HashSet<string> { "Wall" };
		int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(startPoint, 0.5f, array, tagNames, LayerMask.GetMask("Wall"));
		for (int i = 0; i < collidersNonAlloc; i++)
		{
			if (array[i] != null)
			{
				return true;
			}
		}
		if (Physics.Raycast(ray, out var hitInfo, 100f, LayerMask.GetMask("Wall")))
		{
			return num > (ray.origin - hitInfo.point).sqrMagnitude;
		}
		return false;
	}

	public static Vector3 PointWithinRange(Vector3 originPoint, Vector3 centerPoint, float width, float height)
	{
		return new Vector3(Mathf.Clamp(originPoint.x, centerPoint.x - width / 2f, centerPoint.x + width / 2f), Mathf.Clamp(originPoint.y, centerPoint.y - height / 2f, centerPoint.y + height / 2f), 0f);
	}

	public static bool PointOnNavMesh(Vector3 startPoint, int navAreaMask = 16)
	{
		if (NavMesh.SamplePosition(startPoint, out var hit, 99999f, navAreaMask))
		{
			if ((IgnoreZPoint(hit.position) - startPoint).sqrMagnitude < 0.01f)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static Vector3 GetNavMeshPoint(Vector3 startPoint, int navAreaMask = 16)
	{
		if (NavMesh.SamplePosition(startPoint, out var hit, 99999f, navAreaMask))
		{
			return hit.position;
		}
		Debug.LogWarning("为什么没有找到点 请检查！");
		return startPoint;
	}

	public static Vector3 GetNavMeshPoint(Vector3 startPoint, float radius, int navAreaMask = 16)
	{
		Vector3 vector = Vector3.zero;
		int num = 0;
		NavMeshHit hit;
		do
		{
			num++;
			if (num >= 30)
			{
				Debug.LogWarning("没找到可以行走的点,最接近的点");
				return GetNavMeshPoint(vector, navAreaMask);
			}
			vector = IgnoreZPoint(startPoint, NavMaskZOffset(navAreaMask)) + GetDir() * radius;
		}
		while (!NavMesh.SamplePosition(vector, out hit, 0.1f, navAreaMask));
		return hit.position;
	}

	public static Vector3 GetNavMeshPoint(Vector3 startPoint, VariableFloat radius, int navAreaMask = 16)
	{
		Vector3 vector = Vector3.zero;
		int num = 0;
		NavMeshHit hit;
		do
		{
			num++;
			if (num >= 30)
			{
				Debug.LogWarning("没找到可以行走的点,返回最接近的点");
				return GetNavMeshPoint(vector, navAreaMask);
			}
			vector = IgnoreZPoint(startPoint, NavMaskZOffset(navAreaMask)) + GetDir() * radius.RandomResult();
		}
		while (!NavMesh.SamplePosition(vector, out hit, 0.1f, navAreaMask));
		return hit.position;
	}

	public static Vector3 GetNavMeshPoint(Vector3 startPoint, float radius, Vector3 from, float angle, int navAreaMask = 16)
	{
		Vector3 vector = Vector3.zero;
		int num = 0;
		NavMeshHit hit;
		do
		{
			num++;
			if (num >= 30)
			{
				Debug.LogWarning("没找到可以行走的点,返回最接近的点");
				return GetNavMeshPoint(vector, navAreaMask);
			}
			vector = IgnoreZPoint(startPoint, NavMaskZOffset(navAreaMask)) + GetDir(from, UnityEngine.Random.Range(0f - angle, angle)) * radius;
		}
		while (!NavMesh.SamplePosition(vector, out hit, 0.1f, navAreaMask));
		return hit.position;
	}

	public static Vector3 GetNavMeshPoint(Vector3 startPoint, VariableFloat radius, Vector3 from, float angle, int navAreaMask = 16)
	{
		Vector3 vector = Vector3.zero;
		int num = 0;
		NavMeshHit hit;
		do
		{
			num++;
			if (num >= 30)
			{
				Debug.LogWarning("没找到可以行走的点,返回最接近的点");
				return GetNavMeshPoint(vector, navAreaMask);
			}
			vector = IgnoreZPoint(startPoint, NavMaskZOffset(navAreaMask)) + GetDir(from, UnityEngine.Random.Range(0f - angle, angle)) * radius.RandomResult();
		}
		while (!NavMesh.SamplePosition(vector, out hit, 0.1f, navAreaMask));
		return hit.position;
	}

	public static Vector3 GetNavMeshPointIngoreZ(Vector3 startPoint, int navAreaMask = 16)
	{
		return IgnoreZPoint(GetNavMeshPoint(startPoint, navAreaMask));
	}

	public static Vector3 GetNavMeshPointIngoreZ(Vector3 startPoint, float radius, int navAreaMask = 16)
	{
		return IgnoreZPoint(GetNavMeshPoint(startPoint, radius, navAreaMask));
	}

	public static Vector3 GetNavMeshPointIngoreZ(Vector3 startPoint, VariableFloat radius, int navAreaMask = 16)
	{
		return IgnoreZPoint(GetNavMeshPoint(startPoint, radius, navAreaMask));
	}

	public static Vector3 GetNavMeshPointIngoreZ(Vector3 startPoint, float radius, Vector3 from, float angle, int navAreaMask = 16)
	{
		return IgnoreZPoint(GetNavMeshPoint(startPoint, radius, from, angle, navAreaMask));
	}

	public static Vector3 GetNavMeshPointIngoreZ(Vector3 startPoint, VariableFloat radius, Vector3 from, float angle, int navAreaMask = 16)
	{
		return IgnoreZPoint(GetNavMeshPoint(startPoint, radius, from, angle, navAreaMask));
	}

	public static Vector3[] GetCircleDancePoints(Vector3 centerPoint, int dancerCount, float dancerRadius)
	{
		switch (dancerCount)
		{
		case 1:
			return new Vector3[1] { centerPoint };
		case 2:
			return new Vector3[2]
			{
				centerPoint + Vector3.left * dancerRadius,
				centerPoint + Vector3.right * dancerRadius
			};
		default:
		{
			float num = 360f / (float)dancerCount;
			float num2 = dancerRadius / Mathf.Sin(num / 2f * (MathF.PI / 180f));
			Vector3[] array = new Vector3[dancerCount];
			for (int i = 0; i < dancerCount; i++)
			{
				array[i] = centerPoint + GetDir((float)i * num) * num2;
			}
			return array;
		}
		}
	}

	public static Vector3 RotateTowardsAroundZAxis(Vector3 from, Vector3 to, float maxDegree)
	{
		float f = IgnoreZAngleWithSign(from, to);
		if (Mathf.Abs(maxDegree) > Mathf.Abs(f))
		{
			return IgnoreZPoint(to);
		}
		float num = Mathf.Sign(f) * (Mathf.Abs(f) - maxDegree);
		return IgnoreZPoint(GetDir(to, 0f - num));
	}

	public static Vector3 RotateTowardsAroundZAxisSmooth(Vector3 from, Vector3 to, float speedLimit, float startLerpAngle)
	{
		float f = IgnoreZAngleWithSign(from, to);
		float num = speedLimit * Mathf.Lerp(0f, 1f, Mathf.Abs(f) / startLerpAngle);
		if (Mathf.Abs(num) > Mathf.Abs(f))
		{
			return IgnoreZPoint(to);
		}
		float num2 = Mathf.Sign(f) * (Mathf.Abs(f) - num);
		return IgnoreZPoint(GetDir(to, 0f - num2));
	}

	public static Vector3 Slerp(Vector3 dir1, Vector3 dir2, float slerp)
	{
		Vector3 result = Vector3.Slerp(dir1, dir2, slerp);
		if (result.x == 0f)
		{
			result.x = result.z;
			result.z = 0f;
		}
		return result;
	}

	public static Vector3 DirMoveTowards(Vector3 dir1, Vector3 dir2, float maxDelta)
	{
		float degree = GetDegree(dir1);
		float degree2 = GetDegree(dir2);
		return GetDir(Mathf.MoveTowardsAngle(degree, degree2, maxDelta));
	}

	public static Vector3 DirMoveTowardsTargetInCounterClockWise(Vector3 dir1, Vector3 dir2, float maxDelta)
	{
		float degree = GetDegree(dir1);
		float degree2 = GetDegree(dir2);
		return GetDir(default(Mathf).MoveTowardsAngleCounterClockWise(degree, degree2, maxDelta));
	}

	public static Vector3 DirMoveTowardsTargetInCounterClockWiseSmoothLerp(Vector3 dir1, Vector3 dir2, float lerpSpeed)
	{
		float degree = GetDegree(dir1);
		float degree2 = GetDegree(dir2);
		(float, float) tuple = default(Mathf).MoveTowardsAngleCounterClockWiseReTurn2Angle(degree, degree2, lerpSpeed);
		return GetDir(Mathf.Lerp(tuple.Item1, tuple.Item2, lerpSpeed));
	}

	public static Vector3 DirMoveTowardsTargetInClockWise(Vector3 dir1, Vector3 dir2, float maxDelta)
	{
		float degree = GetDegree(dir1);
		float degree2 = GetDegree(dir2);
		return GetDir(default(Mathf).MoveTowardsAngleClockWise(degree, degree2, maxDelta));
	}

	public static Vector3 GetRoomCornerPoint(MapCornerType type)
	{
		Vector3 centerPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		float num = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		float num2 = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		return type switch
		{
			MapCornerType.UpperLeft => centerPoint + new Vector3((0f - num) / 2f, num2 / 2f, 0f), 
			MapCornerType.UpperCenter => centerPoint + new Vector3(0f, num2 / 2f, 0f), 
			MapCornerType.UpperRight => centerPoint + new Vector3(num / 2f, num2 / 2f, 0f), 
			MapCornerType.MiddleLeft => centerPoint + new Vector3((0f - num) / 2f, 0f, 0f), 
			MapCornerType.MiddleCenter => centerPoint, 
			MapCornerType.MiddleRight => centerPoint + new Vector3(num / 2f, 0f, 0f), 
			MapCornerType.LowerLeft => centerPoint + new Vector3((0f - num) / 2f, (0f - num2) / 2f, 0f), 
			MapCornerType.LowerCenter => centerPoint + new Vector3(0f, (0f - num2) / 2f, 0f), 
			MapCornerType.LowerRight => centerPoint + new Vector3(num / 2f, (0f - num2) / 2f, 0f), 
			_ => centerPoint, 
		};
	}

	public static Vector3 DirMoveTowardsTargetInClockWiseSmoothLerp(Vector3 dir1, Vector3 dir2, float lerpSpeed)
	{
		float degree = GetDegree(dir1);
		float degree2 = GetDegree(dir2);
		(float, float) tuple = default(Mathf).MoveTowardsAngleClockWiseReTurn2Angle(degree, degree2, lerpSpeed);
		return GetDir(Mathf.Lerp(tuple.Item1, tuple.Item2, lerpSpeed));
	}

	public static Vector3 DirMoveTowardsTargetInMinAnlgeClockWiseSmoothLerp(Vector3 current, Vector3 to, float lerpSpeed)
	{
		Vector3 zero = Vector3.zero;
		if (Vector2.SignedAngle((Vector2)current, (Vector2)to) < 0f)
		{
			return DirMoveTowardsTargetInClockWiseSmoothLerp(current, to, lerpSpeed);
		}
		return DirMoveTowardsTargetInCounterClockWiseSmoothLerp(current, to, lerpSpeed);
	}

	public static Vector3 DirMoveTowards(float dir1, float dir2, float maxDelta)
	{
		return GetDir(Mathf.MoveTowardsAngle(dir1, dir2, maxDelta));
	}

	public static Vector3 GetTsfForward(Transform tsfTsf)
	{
		return GetDir(tsfTsf.localRotation.eulerAngles.z);
	}

	public static Vector3 GetLayerPoint(Vector3 rootPoint)
	{
		return GetLayerPoint(rootPoint, LayerCorrectType.Coordinate);
	}

	public static Vector3 GetLayerPoint(Transform rootT)
	{
		return GetLayerPoint(rootT.position, LayerCorrectType.Coordinate);
	}

	public static Vector3 GetLayerPoint(Transform rootT, LayerCorrectType type)
	{
		return GetLayerPoint(rootT.position, type);
	}

	public static float GetAngleBetweenTwoDirection(Vector3 dir1, Vector3 dir2)
	{
		return Mathf.Acos(Vector3.Dot(dir1.normalized, dir2.normalized)) * 57.29578f;
	}

	public static Vector3 GetLayerPoint(Vector3 rootPoint, LayerCorrectType type)
	{
		return type switch
		{
			LayerCorrectType.Coordinate => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, (rootPoint.y + rootPoint.z) * 0.01f), 
			LayerCorrectType.Lava0 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.4f), 
			LayerCorrectType.Lava1 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.39f), 
			LayerCorrectType.Lava2 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.38f), 
			LayerCorrectType.Lava3 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.36f), 
			LayerCorrectType.Cliff => GetLayerPoint(rootPoint, LayerCorrectType.Coordinate) + new Vector3(0f, 0f, 1.37f), 
			LayerCorrectType.Tile0 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.35f), 
			LayerCorrectType.Tile1 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.34f), 
			LayerCorrectType.Tile2 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.33f), 
			LayerCorrectType.Tile3 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.32f), 
			LayerCorrectType.Tile4 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.31f), 
			LayerCorrectType.BoundaryAO => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.3f), 
			LayerCorrectType.Tile5_AboveAO => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.29f), 
			LayerCorrectType.Tile6_AboveAO => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.28f), 
			LayerCorrectType.Tile7_AboveAO => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.27f), 
			LayerCorrectType.Tile8_AboveAO => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.26f), 
			LayerCorrectType.Tile9_AboveAO => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.25f), 
			LayerCorrectType.ExplosionTrace => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.24f), 
			LayerCorrectType.AccessOpen => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.23f), 
			LayerCorrectType.SO13 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.22f), 
			LayerCorrectType.SO7 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.21f), 
			LayerCorrectType.SO15 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.2f), 
			LayerCorrectType.Corpse => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.19f), 
			LayerCorrectType.Blood => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.17f), 
			LayerCorrectType.T6Door => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.07f), 
			LayerCorrectType.Water => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.18f), 
			LayerCorrectType.Mucus => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.16f), 
			LayerCorrectType.Venom => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.15f), 
			LayerCorrectType.SO38 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.13f), 
			LayerCorrectType.GroundEffectLow => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.12f), 
			LayerCorrectType.Elite7Trap => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.11f), 
			LayerCorrectType.SO8_Abyss => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.09f), 
			LayerCorrectType.WarningArea => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.1f), 
			LayerCorrectType.GroundEffect => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.08f), 
			LayerCorrectType.Shadow => new Vector3(rootPoint.x, rootPoint.y, 1.05f), 
			LayerCorrectType.TreeRoot => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.04f), 
			LayerCorrectType.BoundaryLow => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.03f), 
			LayerCorrectType.SlimeOnGround => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1.02f), 
			LayerCorrectType.EndlessBoundary => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, 1f), 
			LayerCorrectType.BoundaryHigh => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, -1f), 
			LayerCorrectType.Chapter1Leaf => GetLayerPoint(rootPoint, LayerCorrectType.Coordinate) + new Vector3(0f, 0f, -1.01f), 
			LayerCorrectType.Ghost => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, -1.02f), 
			LayerCorrectType.RoomParticle => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, -2.01f), 
			LayerCorrectType.Chapter3Boundary => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, -2f), 
			LayerCorrectType.RT_Blood => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, -105f), 
			LayerCorrectType.RT_Water => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, -110f), 
			LayerCorrectType.RT_Mucus => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, -120f), 
			LayerCorrectType.RT_Venom => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, -130f), 
			LayerCorrectType.RT_Player => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, -150f), 
			LayerCorrectType.RT_Elite7Trap => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, -160f), 
			LayerCorrectType.RT_Boss3Stage2 => new Vector3(rootPoint.x, rootPoint.y - rootPoint.z, -170f), 
			LayerCorrectType.Ignore => rootPoint, 
			_ => Vector3.zero, 
		};
	}
}
