using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.U2D;

public class Tile_T8 : MonoBehaviour
{
	public Transform tsf_OrnamentsParent;

	public SpriteShapeController ssc;

	public MeshRenderer mr_Lava;

	public UnityEngine.Material mat_Ornament;

	[Header("Corner")]
	public float cornerDistance;

	public GameObject pfb_Corner_Inner_DL;

	public GameObject pfb_Corner_Inner_LU;

	public GameObject pfb_Corner_Inner_RD;

	public GameObject pfb_Corner_Inner_UR;

	public GameObject pfb_Corner_Outer_DL;

	public GameObject pfb_Corner_Outer_LU;

	public GameObject pfb_Corner_Outer_RD;

	public GameObject pfb_Corner_Outer_UR;

	[Header("UnderWall")]
	public MeshRenderer mr_UnderWall;

	public float underWallHeight;

	public float underWallExtraDistance;

	[Header("Access")]
	public float accessWidthRadius;

	private readonly float accessExtension = 12f;

	[Header("OuterOrnament")]
	public float equidistanceExtraDistanceRatio;

	public GameObject[] ornamentPfbs;

	public float[] ornamentDistances;

	public float[] ornamentIntervals;

	public float[] ornamentOffsets;

	public float[] ornamentAccessDontCreates;

	public float[] ornamentAccessDontCreateOffsetYs;

	[Header("T15LavaEdge")]
	public SpriteShapeController ssc2;

	public float ssc2OffsetY;

	public MeshRenderer mr_UnderWall2;

	public Vector3 underWall2Offset;

	public SpriteShapeController ssc3;

	private RoomController roomCtrller;

	private Entity cliffEtt;

	private Entity accessLeftEtt;

	private Entity accessRightEtt;

	private float3 accessLeftOriginalPosition;

	private float3 accessRightOriginalPosition;

	private Spline spline;

	private float halfWidth;

	private float halfHeight;

	private float cornerRadius;

	private float tangentPointDistance;

	private List<Vector3> edgePoints = new List<Vector3>();

	private GameObject go_RecreateGOParent;

	private EntityManager ettMgr;

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	private void SSCInsertAccessLeft(int insertIndex)
	{
		if (roomCtrller.roomCfg.type != RoomType.Boss && BattleMgr.Inst.CurrentStage != 300 && LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, -1, 0))
		{
			if (halfHeight - cornerRadius > accessWidthRadius)
			{
				spline.InsertPointAt(insertIndex, new Vector3(0f - halfWidth, accessWidthRadius));
				spline.InsertPointAt(insertIndex, new Vector3(0f - halfWidth - accessExtension, accessWidthRadius));
				spline.InsertPointAt(insertIndex, new Vector3(0f - halfWidth - accessExtension, 0f - accessWidthRadius));
				spline.InsertPointAt(insertIndex, new Vector3(0f - halfWidth, 0f - accessWidthRadius));
			}
			else if (cornerRadius == halfHeight)
			{
				float num = Mathf.Asin(accessWidthRadius / cornerRadius) / MathF.PI * 180f;
				Vector3 point = Tool2D.GetDir(90f - num) * cornerRadius;
				point.x -= halfWidth - cornerRadius;
				spline.RemovePointAt(insertIndex);
				spline.InsertPointAt(insertIndex, point);
				spline.InsertPointAt(insertIndex, new Vector3(0f - halfWidth - accessExtension, point.y));
				spline.InsertPointAt(insertIndex, new Vector3(0f - halfWidth - accessExtension, 0f - point.y));
				spline.InsertPointAt(insertIndex, new Vector3(point.x, 0f - point.y));
				spline.SetTangentMode(insertIndex + 3, ShapeTangentMode.Broken);
				spline.SetTangentMode(insertIndex, ShapeTangentMode.Broken);
				spline.SetRightTangent(insertIndex + 3, Tool2D.GetDir(0f - num) * (tangentPointDistance - accessWidthRadius));
				spline.SetLeftTangent(insertIndex, Tool2D.GetDir(180f + num) * (tangentPointDistance - accessWidthRadius));
			}
			else
			{
				spline.InsertPointAt(insertIndex, new Vector3(0f - halfWidth - accessExtension, accessWidthRadius));
				spline.InsertPointAt(insertIndex, new Vector3(0f - halfWidth - accessExtension, 0f - accessWidthRadius));
			}
			UnityEngine.Object.Instantiate(pfb_Corner_Inner_LU, base.transform.position + new Vector3(0f - halfWidth, accessWidthRadius), Quaternion.identity, go_RecreateGOParent.transform);
			UnityEngine.Object.Instantiate(pfb_Corner_Inner_DL, base.transform.position + new Vector3(0f - halfWidth, 0f - accessWidthRadius), Quaternion.identity, go_RecreateGOParent.transform);
		}
	}

	private void SSCInsertAccessDown(int insertIndex)
	{
		if (LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, -1))
		{
			if (halfWidth - cornerRadius > accessWidthRadius)
			{
				spline.InsertPointAt(insertIndex, new Vector3(0f - accessWidthRadius, 0f - halfHeight));
				spline.InsertPointAt(insertIndex, new Vector3(0f - accessWidthRadius, 0f - halfHeight - accessExtension));
				spline.InsertPointAt(insertIndex, new Vector3(accessWidthRadius, 0f - halfHeight - accessExtension));
				spline.InsertPointAt(insertIndex, new Vector3(accessWidthRadius, 0f - halfHeight));
			}
			else if (cornerRadius == halfWidth)
			{
				float num = Mathf.Asin(accessWidthRadius / cornerRadius) / MathF.PI * 180f;
				Vector3 point = Tool2D.GetDir(180f - num) * cornerRadius;
				point.y -= halfHeight - cornerRadius;
				spline.RemovePointAt(insertIndex);
				spline.InsertPointAt(insertIndex, point);
				spline.InsertPointAt(insertIndex, new Vector3(point.x, 0f - halfHeight - accessExtension));
				spline.InsertPointAt(insertIndex, new Vector3(0f - point.x, 0f - halfHeight - accessExtension));
				spline.InsertPointAt(insertIndex, new Vector3(0f - point.x, point.y));
				spline.SetTangentMode(insertIndex + 3, ShapeTangentMode.Broken);
				spline.SetTangentMode(insertIndex, ShapeTangentMode.Broken);
				spline.SetRightTangent(insertIndex + 3, Tool2D.GetDir(90f - num) * (tangentPointDistance - accessWidthRadius));
				spline.SetLeftTangent(insertIndex, Tool2D.GetDir(270f + num) * (tangentPointDistance - accessWidthRadius));
			}
			else
			{
				spline.InsertPointAt(insertIndex, new Vector3(0f - accessWidthRadius, 0f - halfHeight - accessExtension));
				spline.InsertPointAt(insertIndex, new Vector3(accessWidthRadius, 0f - halfHeight - accessExtension));
			}
		}
	}

	private void SSCInsertAccessRight(int insertIndex)
	{
		if (roomCtrller.roomCfg.type != RoomType.Boss && BattleMgr.Inst.CurrentStage != 300 && LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 1, 0))
		{
			if (halfHeight - cornerRadius > accessWidthRadius)
			{
				spline.InsertPointAt(insertIndex, new Vector3(halfWidth, 0f - accessWidthRadius));
				spline.InsertPointAt(insertIndex, new Vector3(halfWidth + accessExtension, 0f - accessWidthRadius));
				spline.InsertPointAt(insertIndex, new Vector3(halfWidth + accessExtension, accessWidthRadius));
				spline.InsertPointAt(insertIndex, new Vector3(halfWidth, accessWidthRadius));
			}
			else if (cornerRadius == halfHeight)
			{
				float num = Mathf.Asin(accessWidthRadius / cornerRadius) / MathF.PI * 180f;
				Vector3 point = Tool2D.GetDir(270f - num) * cornerRadius;
				point.x += halfWidth - cornerRadius;
				spline.RemovePointAt(insertIndex);
				spline.InsertPointAt(insertIndex, point);
				spline.InsertPointAt(insertIndex, new Vector3(halfWidth + accessExtension, point.y));
				spline.InsertPointAt(insertIndex, new Vector3(halfWidth + accessExtension, 0f - point.y));
				spline.InsertPointAt(insertIndex, new Vector3(point.x, 0f - point.y));
				spline.SetTangentMode(insertIndex + 3, ShapeTangentMode.Broken);
				spline.SetTangentMode(insertIndex, ShapeTangentMode.Broken);
				spline.SetRightTangent(insertIndex + 3, Tool2D.GetDir(180f - num) * (cornerRadius - accessWidthRadius) * 0.56f);
				spline.SetLeftTangent(insertIndex, Tool2D.GetDir(num) * (cornerRadius - accessWidthRadius) * 0.56f);
			}
			else
			{
				spline.InsertPointAt(insertIndex, new Vector3(halfWidth + accessExtension, 0f - accessWidthRadius));
				spline.InsertPointAt(insertIndex, new Vector3(halfWidth + accessExtension, accessWidthRadius));
			}
			UnityEngine.Object.Instantiate(pfb_Corner_Inner_UR, base.transform.position + new Vector3(halfWidth, accessWidthRadius), Quaternion.identity, go_RecreateGOParent.transform);
			UnityEngine.Object.Instantiate(pfb_Corner_Inner_RD, base.transform.position + new Vector3(halfWidth, 0f - accessWidthRadius), Quaternion.identity, go_RecreateGOParent.transform);
		}
	}

	private void SSCInsertAccessUp(int insertIndex)
	{
		if (LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, 1))
		{
			if (halfWidth - cornerRadius > accessWidthRadius)
			{
				spline.InsertPointAt(insertIndex, new Vector3(accessWidthRadius, halfHeight));
				spline.InsertPointAt(insertIndex, new Vector3(accessWidthRadius, halfHeight + accessExtension));
				spline.InsertPointAt(insertIndex, new Vector3(0f - accessWidthRadius, halfHeight + accessExtension));
				spline.InsertPointAt(insertIndex, new Vector3(0f - accessWidthRadius, halfHeight));
			}
			else if (cornerRadius == halfWidth)
			{
				float num = Mathf.Asin(accessWidthRadius / cornerRadius) / MathF.PI * 180f;
				Vector3 point = Tool2D.GetDir(0f - num) * cornerRadius;
				point.y += halfHeight - cornerRadius;
				spline.RemovePointAt(insertIndex);
				spline.InsertPointAt(insertIndex, point);
				spline.InsertPointAt(insertIndex, new Vector3(point.x, halfHeight + accessExtension));
				spline.InsertPointAt(insertIndex, new Vector3(0f - point.x, halfHeight + accessExtension));
				spline.InsertPointAt(insertIndex, new Vector3(0f - point.x, point.y));
				spline.SetTangentMode(insertIndex + 3, ShapeTangentMode.Broken);
				spline.SetTangentMode(insertIndex, ShapeTangentMode.Broken);
				spline.SetRightTangent(insertIndex + 3, Tool2D.GetDir(-90f - num) * (tangentPointDistance - accessWidthRadius));
				spline.SetLeftTangent(insertIndex, Tool2D.GetDir(90f + num) * (tangentPointDistance - accessWidthRadius));
			}
			else
			{
				spline.InsertPointAt(insertIndex, new Vector3(accessWidthRadius, halfHeight + accessExtension));
				spline.InsertPointAt(insertIndex, new Vector3(0f - accessWidthRadius, halfHeight + accessExtension));
			}
		}
	}

	private void EdgePointInsertAccessLeft(int insertIndex = -1)
	{
		if (roomCtrller.roomCfg.type == RoomType.Boss || BattleMgr.Inst.CurrentStage == 300 || !LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, -1, 0))
		{
			return;
		}
		if (halfHeight - cornerRadius > accessWidthRadius)
		{
			edgePoints.Insert(insertIndex, new Vector3(0f - halfWidth, accessWidthRadius));
			edgePoints.Insert(insertIndex, new Vector3(0f - halfWidth - accessExtension, accessWidthRadius));
			edgePoints.Insert(insertIndex, new Vector3(0f - halfWidth - accessExtension, 0f - accessWidthRadius));
			edgePoints.Insert(insertIndex, new Vector3(0f - halfWidth, 0f - accessWidthRadius));
		}
		else if (cornerRadius == halfHeight)
		{
			float num = Mathf.Asin(accessWidthRadius / cornerRadius) / MathF.PI * 180f;
			Vector3 item = Tool2D.GetDir(90f - num) * cornerRadius;
			item.x -= halfWidth - cornerRadius;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int num2 = edgePoints.Count - 1; num2 >= 0; num2--)
			{
				if (edgePoints[num2].x <= item.x)
				{
					flag3 = true;
					edgePoints[num2] = new Vector3(0f - halfWidth - accessExtension, edgePoints[num2].y);
					if (!flag)
					{
						flag = true;
						edgePoints.Insert(num2 + 1, item);
					}
				}
				else if (flag3)
				{
					flag3 = false;
					if (!flag2)
					{
						flag2 = true;
						edgePoints.Insert(num2 + 1, new Vector3(item.x, 0f - item.y));
					}
				}
			}
		}
		else
		{
			edgePoints.Insert(insertIndex, new Vector3(0f - halfWidth - accessExtension, accessWidthRadius));
			edgePoints.Insert(insertIndex, new Vector3(0f - halfWidth - accessExtension, 0f - accessWidthRadius));
		}
	}

	private void EdgePointInsertAccessDown(int insertIndex = -1)
	{
		if (!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, -1))
		{
			return;
		}
		if (halfWidth - cornerRadius > accessWidthRadius)
		{
			edgePoints.Insert(insertIndex, new Vector3(0f - accessWidthRadius, 0f - halfHeight));
			edgePoints.Insert(insertIndex, new Vector3(0f - accessWidthRadius, 0f - halfHeight - accessExtension));
			edgePoints.Insert(insertIndex, new Vector3(accessWidthRadius, 0f - halfHeight - accessExtension));
			edgePoints.Insert(insertIndex, new Vector3(accessWidthRadius, 0f - halfHeight));
		}
		else if (cornerRadius == halfWidth)
		{
			float num = Mathf.Asin(accessWidthRadius / cornerRadius) / MathF.PI * 180f;
			Vector3 item = Tool2D.GetDir(180f - num) * cornerRadius;
			item.y -= halfHeight - cornerRadius;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int num2 = edgePoints.Count - 1; num2 >= 0; num2--)
			{
				if (edgePoints[num2].y <= item.y)
				{
					flag3 = true;
					edgePoints[num2] = new Vector3(edgePoints[num2].x, 0f - halfHeight - accessExtension);
					if (!flag)
					{
						flag = true;
						edgePoints.Insert(num2 + 1, item);
					}
				}
				else if (flag3)
				{
					flag3 = false;
					if (!flag2)
					{
						flag2 = true;
						edgePoints.Insert(num2 + 1, new Vector3(0f - item.x, item.y));
					}
				}
			}
		}
		else
		{
			edgePoints.Insert(insertIndex, new Vector3(0f - accessWidthRadius, 0f - halfHeight - accessExtension));
			edgePoints.Insert(insertIndex, new Vector3(accessWidthRadius, 0f - halfHeight - accessExtension));
		}
	}

	private void EdgePointInsertAccessRight(int insertIndex = -1)
	{
		if (roomCtrller.roomCfg.type == RoomType.Boss || BattleMgr.Inst.CurrentStage == 300 || !LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 1, 0))
		{
			return;
		}
		if (halfHeight - cornerRadius > accessWidthRadius)
		{
			edgePoints.Insert(insertIndex, new Vector3(halfWidth, 0f - accessWidthRadius));
			edgePoints.Insert(insertIndex, new Vector3(halfWidth + accessExtension, 0f - accessWidthRadius));
			edgePoints.Insert(insertIndex, new Vector3(halfWidth + accessExtension, accessWidthRadius));
			edgePoints.Insert(insertIndex, new Vector3(halfWidth, accessWidthRadius));
		}
		else if (cornerRadius == halfHeight)
		{
			float num = Mathf.Asin(accessWidthRadius / cornerRadius) / MathF.PI * 180f;
			Vector3 item = Tool2D.GetDir(270f - num) * cornerRadius;
			item.x += halfWidth - cornerRadius;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int num2 = edgePoints.Count - 1; num2 >= 0; num2--)
			{
				if (edgePoints[num2].x >= item.x)
				{
					flag3 = true;
					edgePoints[num2] = new Vector3(halfWidth + accessExtension, edgePoints[num2].y);
					if (!flag)
					{
						flag = true;
						edgePoints.Insert(num2 + 1, item);
					}
				}
				else if (flag3)
				{
					flag3 = false;
					if (!flag2)
					{
						flag2 = true;
						edgePoints.Insert(num2 + 1, new Vector3(item.x, 0f - item.y));
					}
				}
			}
		}
		else
		{
			edgePoints.Insert(insertIndex, new Vector3(halfWidth + accessExtension, 0f - accessWidthRadius));
			edgePoints.Insert(insertIndex, new Vector3(halfWidth + accessExtension, accessWidthRadius));
		}
	}

	private void EdgePointInsertAccessUp(int insertIndex = -1)
	{
		if (!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, 1))
		{
			return;
		}
		if (halfWidth - cornerRadius > accessWidthRadius)
		{
			edgePoints.Insert(insertIndex, new Vector3(accessWidthRadius, halfHeight));
			edgePoints.Insert(insertIndex, new Vector3(accessWidthRadius, halfHeight + accessExtension));
			edgePoints.Insert(insertIndex, new Vector3(0f - accessWidthRadius, halfHeight + accessExtension));
			edgePoints.Insert(insertIndex, new Vector3(0f - accessWidthRadius, halfHeight));
		}
		else if (cornerRadius == halfWidth)
		{
			Vector3 item = Tool2D.GetDir(0f - Mathf.Asin(accessWidthRadius / cornerRadius) / MathF.PI * 180f) * cornerRadius;
			item.y += halfHeight - cornerRadius;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int num = edgePoints.Count - 1; num >= 0; num--)
			{
				if (edgePoints[num].y >= item.y)
				{
					flag3 = true;
					edgePoints[num] = new Vector3(edgePoints[num].x, halfHeight + accessExtension);
					if (!flag)
					{
						flag = true;
						edgePoints.Insert(num + 1, item);
					}
				}
				else if (flag3)
				{
					flag3 = false;
					if (!flag2)
					{
						flag2 = true;
						edgePoints.Insert(num + 1, new Vector3(0f - item.x, item.y));
					}
				}
			}
		}
		else
		{
			edgePoints.Insert(insertIndex, new Vector3(accessWidthRadius, halfHeight + accessExtension));
			edgePoints.Insert(insertIndex, new Vector3(0f - accessWidthRadius, halfHeight + accessExtension));
		}
	}

	private void CreateEquidistanceObj(GameObject objPrefab, float distance, float objInterval, float offset, float accessNoObj, float accessNoObjCorrectOffsetY, int ornamentIndex)
	{
		float num = halfWidth + distance;
		float num2 = halfHeight + distance;
		float num3 = cornerRadius + distance;
		Transform parent = tsf_OrnamentsParent.GetChild(ornamentIndex).transform;
		for (float num4 = 0f - num + num3; num4 < num - num3; num4 += objInterval)
		{
			Vector3 vector = new Vector3(num4, num2, 0f) + Tool2D.GetDir() * UnityEngine.Random.Range(0f, offset);
			if (!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, 1) || !(Mathf.Abs(vector.x) < accessNoObj))
			{
				UnityEngine.Object.Instantiate(objPrefab, base.transform.position + vector, Quaternion.identity, parent).GetComponent<IEightDir>()?.SetDirAndRoomCtrller(EightDir.Up, roomCtrller);
			}
		}
		for (float num5 = num2 - num3; num5 > 0f - num2 + num3; num5 -= objInterval)
		{
			Vector3 vector2 = new Vector3(num, num5, 0f) + Tool2D.GetDir(270f) * distance * equidistanceExtraDistanceRatio * 0.5f + Tool2D.GetDir() * UnityEngine.Random.Range(0f, offset);
			if (!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 1, 0) || !(Mathf.Abs(vector2.y - accessNoObjCorrectOffsetY) < accessNoObj) || roomCtrller.roomCfg.type == RoomType.Boss || BattleMgr.Inst.CurrentStage == 300)
			{
				UnityEngine.Object.Instantiate(objPrefab, base.transform.position + vector2, Quaternion.identity, parent).GetComponent<IEightDir>()?.SetDirAndRoomCtrller(EightDir.Right, roomCtrller);
			}
		}
		for (float num6 = num - num3; num6 > 0f - num + num3; num6 -= objInterval)
		{
			Vector3 vector3 = new Vector3(num6, 0f - num2, 0f) + Tool2D.GetDir(180f) * distance * equidistanceExtraDistanceRatio + Tool2D.GetDir() * UnityEngine.Random.Range(0f, offset);
			if (!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, -1) || !(Mathf.Abs(vector3.x) < accessNoObj))
			{
				UnityEngine.Object.Instantiate(objPrefab, base.transform.position + vector3, Quaternion.identity, parent).GetComponent<IEightDir>()?.SetDirAndRoomCtrller(EightDir.Down, roomCtrller);
			}
		}
		for (float num7 = 0f - num2 + num3; num7 < num2 - num3; num7 += objInterval)
		{
			Vector3 vector4 = new Vector3(0f - num, num7, 0f) + Tool2D.GetDir(90f) * distance * equidistanceExtraDistanceRatio * 0.5f + Tool2D.GetDir() * UnityEngine.Random.Range(0f, offset);
			if (!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, -1, 0) || !(Mathf.Abs(vector4.y - accessNoObjCorrectOffsetY) < accessNoObj) || roomCtrller.roomCfg.type == RoomType.Boss || BattleMgr.Inst.CurrentStage == 300)
			{
				UnityEngine.Object.Instantiate(objPrefab, base.transform.position + vector4, Quaternion.identity, parent).GetComponent<IEightDir>()?.SetDirAndRoomCtrller(EightDir.Left, roomCtrller);
			}
		}
		if (!(num3 > 0f))
		{
			return;
		}
		Vector3 vector5 = new Vector3(num - num3, num2 - num3, 0f);
		Vector3 vector6 = new Vector3(num - num3, 0f - num2 + num3, 0f);
		Vector3 vector7 = new Vector3(0f - num + num3, 0f - num2 + num3, 0f);
		Vector3 vector8 = new Vector3(0f - num + num3, num2 - num3, 0f);
		int num8 = (int)(MathF.PI * 2f * num3 / 4f / objInterval);
		float num9 = 90f / (float)num8;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < num8; i++)
		{
			zero = Tool2D.GetDir(0f - (float)i * num9);
			Vector3 vector9 = vector5 + zero * num3 + zero * Vector3.Angle(Vector3.up, zero) / 180f * distance * equidistanceExtraDistanceRatio + Tool2D.GetDir() * UnityEngine.Random.Range(0f, offset);
			if ((!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, 1) || !(Mathf.Abs(vector9.x) < accessNoObj)) && (!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 1, 0) || !(Mathf.Abs(vector9.y - accessNoObjCorrectOffsetY) < accessNoObj)))
			{
				UnityEngine.Object.Instantiate(objPrefab, base.transform.position + vector9, Quaternion.identity, parent).GetComponent<IEightDir>()?.SetDirAndRoomCtrller(EightDir.UpRight, roomCtrller);
			}
			zero = Tool2D.GetDir(270f - (float)i * num9);
			Vector3 vector10 = vector6 + zero * num3 + zero * Vector3.Angle(Vector3.up, zero) / 180f * distance * equidistanceExtraDistanceRatio + Tool2D.GetDir() * UnityEngine.Random.Range(0f, offset);
			if ((!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 1, 0) || !(Mathf.Abs(vector10.y - accessNoObjCorrectOffsetY) < accessNoObj)) && (!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, -1) || !(Mathf.Abs(vector10.x) < accessNoObj)))
			{
				UnityEngine.Object.Instantiate(objPrefab, base.transform.position + vector10, Quaternion.identity, parent).GetComponent<IEightDir>()?.SetDirAndRoomCtrller(EightDir.RightDown, roomCtrller);
			}
			zero = Tool2D.GetDir(180f - (float)i * num9);
			Vector3 vector11 = vector7 + zero * num3 + zero * Vector3.Angle(Vector3.up, zero) / 180f * distance * equidistanceExtraDistanceRatio + Tool2D.GetDir() * UnityEngine.Random.Range(0f, offset);
			if ((!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, -1) || !(Mathf.Abs(vector11.x) < accessNoObj)) && (!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, -1, 0) || !(Mathf.Abs(vector11.y - accessNoObjCorrectOffsetY) < accessNoObj)))
			{
				UnityEngine.Object.Instantiate(objPrefab, base.transform.position + vector11, Quaternion.identity, parent).GetComponent<IEightDir>()?.SetDirAndRoomCtrller(EightDir.DownLeft, roomCtrller);
			}
			zero = Tool2D.GetDir(90f - (float)i * num9);
			Vector3 vector12 = vector8 + zero * num3 + zero * Vector3.Angle(Vector3.up, zero) / 180f * distance * equidistanceExtraDistanceRatio + Tool2D.GetDir() * UnityEngine.Random.Range(0f, offset);
			if ((!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, -1, 0) || !(Mathf.Abs(vector12.y - accessNoObjCorrectOffsetY) < accessNoObj)) && (!LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, 1) || !(Mathf.Abs(vector12.x) < accessNoObj)))
			{
				UnityEngine.Object.Instantiate(objPrefab, base.transform.position + vector12, Quaternion.identity, parent).GetComponent<IEightDir>()?.SetDirAndRoomCtrller(EightDir.LeftUp, roomCtrller);
			}
		}
	}

	public void TileCorrect(RoomController roomCtrller, bool isRecreate = false)
	{
		this.roomCtrller = roomCtrller;
		spline = ssc.spline;
		spline.Clear();
		halfWidth = (float)roomCtrller.roomCfg.theme8Width / 2f;
		halfHeight = (float)roomCtrller.roomCfg.theme8Height / 2f;
		cornerRadius = roomCtrller.roomCfg.theme8CornerRadius;
		if (cornerRadius > Mathf.Min(halfWidth, halfHeight))
		{
			cornerRadius = Mathf.Min(halfWidth, halfHeight);
		}
		tangentPointDistance = cornerRadius * 0.56f;
		go_RecreateGOParent = new GameObject("RecreateGOParent");
		go_RecreateGOParent.transform.SetParent(base.transform);
		if (cornerRadius == 0f)
		{
			spline.InsertPointAt(0, new Vector3(0f - halfWidth, halfHeight));
			spline.InsertPointAt(1, new Vector3(halfWidth, halfHeight));
			spline.InsertPointAt(2, new Vector3(halfWidth, 0f - halfHeight));
			spline.InsertPointAt(3, new Vector3(0f - halfWidth, 0f - halfHeight));
			spline.SetTangentMode(0, ShapeTangentMode.Broken);
			spline.SetTangentMode(1, ShapeTangentMode.Broken);
			spline.SetTangentMode(2, ShapeTangentMode.Broken);
			spline.SetTangentMode(3, ShapeTangentMode.Broken);
			SSCInsertAccessLeft(4);
			SSCInsertAccessDown(3);
			SSCInsertAccessRight(2);
			SSCInsertAccessUp(1);
			UnityEngine.Object.Instantiate(pfb_Corner_Outer_LU, base.transform.position + new Vector3(0f - halfWidth, halfHeight), Quaternion.identity, go_RecreateGOParent.transform);
			UnityEngine.Object.Instantiate(pfb_Corner_Outer_UR, base.transform.position + new Vector3(halfWidth, halfHeight), Quaternion.identity, go_RecreateGOParent.transform);
			UnityEngine.Object.Instantiate(pfb_Corner_Outer_RD, base.transform.position + new Vector3(halfWidth, 0f - halfHeight), Quaternion.identity, go_RecreateGOParent.transform);
			UnityEngine.Object.Instantiate(pfb_Corner_Outer_DL, base.transform.position + new Vector3(0f - halfWidth, 0f - halfHeight), Quaternion.identity, go_RecreateGOParent.transform);
		}
		else if (cornerRadius == halfWidth && cornerRadius == halfHeight)
		{
			spline.InsertPointAt(0, new Vector3(0f, cornerRadius));
			spline.InsertPointAt(1, new Vector3(cornerRadius, 0f));
			spline.InsertPointAt(2, new Vector3(0f, 0f - cornerRadius));
			spline.InsertPointAt(3, new Vector3(0f - cornerRadius, 0f));
			spline.SetTangentMode(0, ShapeTangentMode.Broken);
			spline.SetTangentMode(1, ShapeTangentMode.Broken);
			spline.SetTangentMode(2, ShapeTangentMode.Broken);
			spline.SetTangentMode(3, ShapeTangentMode.Broken);
			spline.SetLeftTangent(0, new Vector3(0f - tangentPointDistance, 0f));
			spline.SetRightTangent(0, new Vector3(tangentPointDistance, 0f));
			spline.SetLeftTangent(1, new Vector3(0f, tangentPointDistance));
			spline.SetRightTangent(1, new Vector3(0f, 0f - tangentPointDistance));
			spline.SetLeftTangent(2, new Vector3(tangentPointDistance, 0f));
			spline.SetRightTangent(2, new Vector3(0f - tangentPointDistance, 0f));
			spline.SetLeftTangent(3, new Vector3(0f, 0f - tangentPointDistance));
			spline.SetRightTangent(3, new Vector3(0f, tangentPointDistance));
			SSCInsertAccessLeft(3);
			SSCInsertAccessDown(2);
			SSCInsertAccessRight(1);
			SSCInsertAccessUp(0);
		}
		else if (cornerRadius == halfWidth)
		{
			spline.InsertPointAt(0, new Vector3(0f, halfHeight));
			spline.InsertPointAt(1, new Vector3(halfWidth, halfHeight - cornerRadius));
			spline.InsertPointAt(2, new Vector3(halfWidth, 0f - halfHeight + cornerRadius));
			spline.InsertPointAt(3, new Vector3(0f, 0f - halfHeight));
			spline.InsertPointAt(4, new Vector3(0f - halfWidth, 0f - halfHeight + cornerRadius));
			spline.InsertPointAt(5, new Vector3(0f - halfWidth, halfHeight - cornerRadius));
			spline.SetTangentMode(0, ShapeTangentMode.Broken);
			spline.SetTangentMode(1, ShapeTangentMode.Broken);
			spline.SetTangentMode(2, ShapeTangentMode.Broken);
			spline.SetTangentMode(3, ShapeTangentMode.Broken);
			spline.SetTangentMode(4, ShapeTangentMode.Broken);
			spline.SetTangentMode(5, ShapeTangentMode.Broken);
			spline.SetLeftTangent(0, new Vector3(0f - tangentPointDistance, 0f));
			spline.SetRightTangent(0, new Vector3(tangentPointDistance, 0f));
			spline.SetLeftTangent(1, new Vector3(0f, tangentPointDistance));
			spline.SetRightTangent(2, new Vector3(0f, 0f - tangentPointDistance));
			spline.SetLeftTangent(3, new Vector3(tangentPointDistance, 0f));
			spline.SetRightTangent(3, new Vector3(0f - tangentPointDistance, 0f));
			spline.SetLeftTangent(4, new Vector3(0f, 0f - tangentPointDistance));
			spline.SetRightTangent(5, new Vector3(0f, tangentPointDistance));
			SSCInsertAccessLeft(5);
			SSCInsertAccessDown(3);
			SSCInsertAccessRight(2);
			SSCInsertAccessUp(0);
		}
		else if (cornerRadius == halfHeight)
		{
			spline.InsertPointAt(0, new Vector3(0f - halfWidth + cornerRadius, halfHeight));
			spline.InsertPointAt(1, new Vector3(halfWidth - cornerRadius, halfHeight));
			spline.InsertPointAt(2, new Vector3(halfWidth, 0f));
			spline.InsertPointAt(3, new Vector3(halfWidth - cornerRadius, 0f - halfHeight));
			spline.InsertPointAt(4, new Vector3(0f - halfWidth + cornerRadius, 0f - halfHeight));
			spline.InsertPointAt(5, new Vector3(0f - halfWidth, 0f));
			spline.SetTangentMode(0, ShapeTangentMode.Broken);
			spline.SetTangentMode(1, ShapeTangentMode.Broken);
			spline.SetTangentMode(2, ShapeTangentMode.Broken);
			spline.SetTangentMode(3, ShapeTangentMode.Broken);
			spline.SetTangentMode(4, ShapeTangentMode.Broken);
			spline.SetTangentMode(5, ShapeTangentMode.Broken);
			spline.SetLeftTangent(0, new Vector3(0f - tangentPointDistance, 0f));
			spline.SetRightTangent(1, new Vector3(tangentPointDistance, 0f));
			spline.SetLeftTangent(2, new Vector3(0f, tangentPointDistance));
			spline.SetRightTangent(2, new Vector3(0f, 0f - tangentPointDistance));
			spline.SetLeftTangent(3, new Vector3(tangentPointDistance, 0f));
			spline.SetRightTangent(4, new Vector3(0f - tangentPointDistance, 0f));
			spline.SetLeftTangent(5, new Vector3(0f, 0f - tangentPointDistance));
			spline.SetRightTangent(5, new Vector3(0f, tangentPointDistance));
			SSCInsertAccessLeft(5);
			SSCInsertAccessDown(4);
			SSCInsertAccessRight(2);
			SSCInsertAccessUp(1);
		}
		else
		{
			spline.InsertPointAt(0, new Vector3(0f - halfWidth, halfHeight - cornerRadius));
			spline.InsertPointAt(1, new Vector3(0f - halfWidth + cornerRadius, halfHeight));
			spline.InsertPointAt(2, new Vector3(halfWidth - cornerRadius, halfHeight));
			spline.InsertPointAt(3, new Vector3(halfWidth, halfHeight - cornerRadius));
			spline.InsertPointAt(4, new Vector3(halfWidth, 0f - halfHeight + cornerRadius));
			spline.InsertPointAt(5, new Vector3(halfWidth - cornerRadius, 0f - halfHeight));
			spline.InsertPointAt(6, new Vector3(0f - halfWidth + cornerRadius, 0f - halfHeight));
			spline.InsertPointAt(7, new Vector3(0f - halfWidth, 0f - halfHeight + cornerRadius));
			spline.SetTangentMode(0, ShapeTangentMode.Broken);
			spline.SetTangentMode(1, ShapeTangentMode.Broken);
			spline.SetTangentMode(2, ShapeTangentMode.Broken);
			spline.SetTangentMode(3, ShapeTangentMode.Broken);
			spline.SetTangentMode(4, ShapeTangentMode.Broken);
			spline.SetTangentMode(5, ShapeTangentMode.Broken);
			spline.SetTangentMode(6, ShapeTangentMode.Broken);
			spline.SetTangentMode(7, ShapeTangentMode.Broken);
			spline.SetRightTangent(0, new Vector3(0f, tangentPointDistance));
			spline.SetLeftTangent(1, new Vector3(0f - tangentPointDistance, 0f));
			spline.SetRightTangent(2, new Vector3(tangentPointDistance, 0f));
			spline.SetLeftTangent(3, new Vector3(0f, tangentPointDistance));
			spline.SetRightTangent(4, new Vector3(0f, 0f - tangentPointDistance));
			spline.SetLeftTangent(5, new Vector3(tangentPointDistance, 0f));
			spline.SetRightTangent(6, new Vector3(0f - tangentPointDistance, 0f));
			spline.SetLeftTangent(7, new Vector3(0f, 0f - tangentPointDistance));
			SSCInsertAccessLeft(8);
			SSCInsertAccessDown(6);
			SSCInsertAccessRight(4);
			SSCInsertAccessUp(2);
		}
		ssc.BakeMesh();
		if (cornerRadius == 0f)
		{
			edgePoints.Add(new Vector3(0f - halfWidth, halfHeight));
			edgePoints.Add(new Vector3(halfWidth, halfHeight));
			edgePoints.Add(new Vector3(halfWidth, 0f - halfHeight));
			edgePoints.Add(new Vector3(0f - halfWidth, 0f - halfHeight));
			EdgePointInsertAccessLeft(4);
			EdgePointInsertAccessDown(3);
			EdgePointInsertAccessRight(2);
			EdgePointInsertAccessUp(1);
		}
		else
		{
			float num = 2f * cornerRadius * MathF.PI;
			if (cornerRadius == halfWidth && cornerRadius == halfHeight)
			{
				int num2 = (int)(num / cornerDistance);
				for (int i = 0; i < num2; i++)
				{
					edgePoints.Add(Tool2D.GetDir(45f - 360f / (float)num2 * (float)i) * cornerRadius);
				}
				EdgePointInsertAccessLeft();
				EdgePointInsertAccessRight();
				EdgePointInsertAccessDown();
				EdgePointInsertAccessUp();
			}
			else if (cornerRadius == halfWidth)
			{
				int num3 = (int)(num / 2f / cornerDistance);
				for (int j = 0; j < num3; j++)
				{
					edgePoints.Add(new Vector3(0f, halfHeight - cornerRadius) + Tool2D.GetDir(90f - 180f / (float)num3 * (float)j) * cornerRadius);
				}
				edgePoints.Add(new Vector3(halfWidth, halfHeight - cornerRadius));
				for (int k = 0; k < num3; k++)
				{
					edgePoints.Add(new Vector3(0f, 0f - halfHeight + cornerRadius) + Tool2D.GetDir(-90f - 180f / (float)num3 * (float)k) * cornerRadius);
				}
				edgePoints.Add(new Vector3(0f - halfWidth, 0f - halfHeight + cornerRadius));
				EdgePointInsertAccessLeft(edgePoints.Count);
				EdgePointInsertAccessRight(num3 + 1);
				EdgePointInsertAccessDown();
				EdgePointInsertAccessUp();
			}
			else if (cornerRadius == halfHeight)
			{
				int num4 = (int)(num / 2f / cornerDistance);
				edgePoints.Add(new Vector3(0f - halfWidth + cornerRadius, halfHeight));
				for (int l = 0; l < num4; l++)
				{
					edgePoints.Add(new Vector3(halfWidth - cornerRadius, 0f) + Tool2D.GetDir(0f - 180f / (float)num4 * (float)l) * cornerRadius);
				}
				edgePoints.Add(new Vector3(halfWidth - cornerRadius, 0f - halfHeight));
				for (int m = 0; m < num4; m++)
				{
					edgePoints.Add(new Vector3(0f - halfWidth + cornerRadius, 0f) + Tool2D.GetDir(180f - 180f / (float)num4 * (float)m) * cornerRadius);
				}
				EdgePointInsertAccessDown(num4 + 2);
				EdgePointInsertAccessUp(1);
				EdgePointInsertAccessLeft();
				EdgePointInsertAccessRight();
			}
			else
			{
				int num5 = (int)(num / 4f / cornerDistance);
				for (int n = 0; n < num5; n++)
				{
					edgePoints.Add(new Vector3(0f - halfWidth + cornerRadius, halfHeight - cornerRadius) + Tool2D.GetDir(90f - 90f / (float)num5 * (float)n) * cornerRadius);
				}
				edgePoints.Add(new Vector3(0f - halfWidth + cornerRadius, halfHeight));
				for (int num6 = 0; num6 < num5; num6++)
				{
					edgePoints.Add(new Vector3(halfWidth - cornerRadius, halfHeight - cornerRadius) + Tool2D.GetDir(0f - 90f / (float)num5 * (float)num6) * cornerRadius);
				}
				edgePoints.Add(new Vector3(halfWidth, halfHeight - cornerRadius));
				for (int num7 = 0; num7 < num5; num7++)
				{
					edgePoints.Add(new Vector3(halfWidth - cornerRadius, 0f - halfHeight + cornerRadius) + Tool2D.GetDir(270f - 90f / (float)num5 * (float)num7) * cornerRadius);
				}
				edgePoints.Add(new Vector3(halfWidth - cornerRadius, 0f - halfHeight));
				for (int num8 = 0; num8 < num5; num8++)
				{
					edgePoints.Add(new Vector3(0f - halfWidth + cornerRadius, 0f - halfHeight + cornerRadius) + Tool2D.GetDir(180f - 90f / (float)num5 * (float)num8) * cornerRadius);
				}
				edgePoints.Add(new Vector3(0f - halfWidth, 0f - halfHeight + cornerRadius));
				EdgePointInsertAccessLeft(edgePoints.Count);
				EdgePointInsertAccessDown(num5 * 3 + 3);
				EdgePointInsertAccessRight(num5 * 2 + 2);
				EdgePointInsertAccessUp(num5 + 1);
			}
		}
		Vector3[] array = new Vector3[edgePoints.Count * 2];
		Vector3[] array2 = new Vector3[edgePoints.Count * 2 + 1];
		int[] array3 = new int[array2.Length * 3];
		for (int num9 = 0; num9 < edgePoints.Count; num9++)
		{
			array[num9 * 2] = new Vector3(edgePoints[num9].x, edgePoints[num9].y, -10f);
			array[num9 * 2 + 1] = new Vector3(edgePoints[num9].x, edgePoints[num9].y, 10f);
			array2[num9] = edgePoints[num9];
			if (num9 == edgePoints.Count - 1)
			{
				array2[num9 + 1] = Vector3.zero;
				array3[num9 * 3] = num9;
				array3[num9 * 3 + 1] = 0;
				array3[num9 * 3 + 2] = edgePoints.Count;
			}
			else
			{
				array3[num9 * 3] = num9;
				array3[num9 * 3 + 1] = num9 + 1;
				array3[num9 * 3 + 2] = edgePoints.Count;
			}
		}
		int[] array4 = new int[edgePoints.Count * 6];
		for (int num10 = 0; num10 < edgePoints.Count; num10++)
		{
			if (num10 == edgePoints.Count - 1)
			{
				array4[num10 * 6] = num10 * 2;
				array4[num10 * 6 + 1] = 0;
				array4[num10 * 6 + 2] = num10 * 2 + 1;
				array4[num10 * 6 + 3] = num10 * 2 + 1;
				array4[num10 * 6 + 4] = 0;
				array4[num10 * 6 + 5] = 1;
			}
			else
			{
				array4[num10 * 6] = num10 * 2;
				array4[num10 * 6 + 1] = num10 * 2 + 2;
				array4[num10 * 6 + 2] = num10 * 2 + 1;
				array4[num10 * 6 + 3] = num10 * 2 + 1;
				array4[num10 * 6 + 4] = num10 * 2 + 2;
				array4[num10 * 6 + 5] = num10 * 2 + 3;
			}
		}
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(AllSceneEtt));
		AllSceneEtt singleton = entityQuery.GetSingleton<AllSceneEtt>();
		if (cliffEtt != Entity.Null)
		{
			ettMgr.DestroyEntity(cliffEtt);
		}
		cliffEtt = ettMgr.Instantiate(singleton.ett_T8CliffCollider);
		GameObject obj = new GameObject("NavAction");
		GameObject gameObject = new GameObject("NavGround");
		GameObject gameObject2 = new GameObject("NavFly");
		ettMgr.SetComponentData(cliffEtt, new LocalTransform
		{
			Position = base.transform.position,
			Rotation = quaternion.identity,
			Scale = 1f
		});
		obj.transform.position = base.transform.position + new Vector3(0f, 0f, 4.4f);
		gameObject.transform.position = base.transform.position + new Vector3(0f, 0f, 0f);
		gameObject2.transform.position = base.transform.position + new Vector3(0f, 0f, 4.3f);
		obj.transform.SetParent(roomCtrller.tsf_Action);
		gameObject.transform.SetParent(roomCtrller.tsf_Ground);
		gameObject2.transform.SetParent(roomCtrller.tsf_Fly);
		obj.layer = LayerMask.NameToLayer("NavAction");
		gameObject.layer = LayerMask.NameToLayer("NavGround");
		gameObject2.layer = LayerMask.NameToLayer("NavFly");
		ConvertMeshToEttPC(mesh: new Mesh
		{
			vertices = array,
			triangles = array4
		}, ett: cliffEtt);
		Mesh mesh2 = new Mesh
		{
			vertices = array2,
			triangles = array3
		};
		obj.AddComponent<MeshFilter>().mesh = mesh2;
		obj.AddComponent<UnityEngine.MeshCollider>();
		Mesh mesh3 = new Mesh();
		mesh3.vertices = array2;
		mesh3.triangles = array3;
		gameObject.AddComponent<MeshFilter>().mesh = mesh3;
		gameObject.AddComponent<UnityEngine.MeshCollider>();
		Mesh mesh4 = new Mesh();
		mesh4.vertices = array2;
		mesh4.triangles = array3;
		gameObject2.AddComponent<MeshFilter>().mesh = mesh4;
		gameObject2.AddComponent<UnityEngine.MeshCollider>();
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		List<Vector3> list4 = new List<Vector3>();
		List<int> list5 = new List<int>();
		for (int num11 = 0; num11 < edgePoints.Count; num11++)
		{
			if (!(edgePoints[num11].y <= 0f))
			{
				continue;
			}
			Vector2 vector = edgePoints[num11];
			if (cornerRadius == 0f || Mathf.Abs(vector.x) >= halfWidth)
			{
				vector.x += ((vector.x < 0f) ? (0f - underWallExtraDistance) : underWallExtraDistance);
			}
			else if (Mathf.Abs(vector.x) > halfWidth - cornerRadius)
			{
				Vector2 vector2 = vector;
				vector2.y += halfHeight - cornerRadius;
				if (vector2.x < 0f)
				{
					vector2.x += halfWidth - cornerRadius;
					vector += vector2.normalized * underWallExtraDistance;
				}
				else
				{
					vector2.x -= halfWidth - cornerRadius;
					vector += vector2.normalized * underWallExtraDistance;
				}
			}
			else
			{
				vector.y -= underWallExtraDistance;
			}
			list.Add(vector);
			list2.Add(vector);
			list2.Add(vector + new Vector2(0f, 0f - underWallHeight));
			list3.Add(new Vector2(vector.x / underWallHeight, 1f));
			list3.Add(new Vector2(vector.x / underWallHeight, 0f));
			list4.Add(new Vector3(0f, 0f, -1f));
			list4.Add(new Vector3(0f, 0f, -1f));
		}
		for (int num12 = 0; num12 < list.Count - 1; num12++)
		{
			list5.Add(num12 * 2);
			list5.Add(num12 * 2 + 2);
			list5.Add(num12 * 2 + 1);
			list5.Add(num12 * 2 + 1);
			list5.Add(num12 * 2 + 2);
			list5.Add(num12 * 2 + 3);
		}
		Mesh mesh5 = new Mesh();
		mesh5.vertices = list2.ToArray();
		mesh5.triangles = list5.ToArray();
		mesh5.uv = list3.ToArray();
		mesh5.normals = list4.ToArray();
		mr_UnderWall.GetComponent<MeshFilter>().mesh = mesh5;
		if (!isRecreate && BattleMgr.Inst.CurrentStage != 300)
		{
			int themeType = (int)roomCtrller.roomCfg.themeType;
			string text = "Prefabs/Scene/Theme" + themeType;
			themeType = (int)roomCtrller.roomCfg.themeType;
			string text2 = "Prefabs/Scene/Theme" + themeType + "_H";
			_ = text + "/Door";
			_ = text2 + "/Door";
			_ = text + "/AccessL";
			_ = text2 + "/AccessL";
			_ = text + "/AccessR";
			_ = text2 + "/AccessR";
			_ = text + "/AccessU";
			_ = text2 + "/AccessU";
			_ = text + "/AccessD";
			_ = text2 + "/AccessD";
			Entity singletonEntity = entityQuery.GetSingletonEntity();
			SceneEttBED sceneEttBED = ettMgr.GetBuffer<SceneEttBED>(singletonEntity)[(int)roomCtrller.roomCfg.themeType];
			if (LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, -1, 0))
			{
				accessLeftEtt = ettMgr.Instantiate(sceneEttBED.ett_AccessL);
				LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(accessLeftEtt);
				componentData.Position = base.transform.position + new Vector3(0f - halfWidth + 0.5f, 0f, 0f);
				ettMgr.SetComponentData(accessLeftEtt, componentData);
				AccessBase_Dots componentData2 = ettMgr.GetComponentData<AccessBase_Dots>(accessLeftEtt);
				componentData2.Dir = FourDir.Left;
				componentData2.roomType = roomCtrller.roomCfg.type;
				componentData2.themeType = roomCtrller.roomCfg.themeType;
				if (roomCtrller.MapPos.x == 0 && roomCtrller.roomCfg.type == RoomType.Monster)
				{
					componentData2.needKey = true;
				}
				ettMgr.SetComponentData(accessLeftEtt, componentData2);
				roomCtrller.accessEttList.Add(accessLeftEtt);
			}
			if (LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 1, 0))
			{
				accessRightEtt = ettMgr.Instantiate(sceneEttBED.ett_AccessR);
				LocalTransform componentData3 = ettMgr.GetComponentData<LocalTransform>(accessRightEtt);
				componentData3.Position = base.transform.position + new Vector3(halfWidth - 0.5f, 0f, 0f);
				ettMgr.SetComponentData(accessRightEtt, componentData3);
				AccessBase_Dots componentData4 = ettMgr.GetComponentData<AccessBase_Dots>(accessRightEtt);
				componentData4.Dir = FourDir.Right;
				componentData4.roomType = roomCtrller.roomCfg.type;
				componentData4.themeType = roomCtrller.roomCfg.themeType;
				if (roomCtrller.MapPos.x == 0 && roomCtrller.roomCfg.type == RoomType.Monster)
				{
					componentData4.needKey = true;
				}
				ettMgr.SetComponentData(accessRightEtt, componentData4);
				roomCtrller.accessEttList.Add(accessRightEtt);
			}
			if (LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, 1))
			{
				Entity entity = ettMgr.Instantiate(sceneEttBED.ett_AccessU);
				LocalTransform componentData5 = ettMgr.GetComponentData<LocalTransform>(entity);
				componentData5.Position = base.transform.position + new Vector3(-0.5f, halfHeight + 0.5f, 0f);
				ettMgr.SetComponentData(entity, componentData5);
				AccessBase_Dots componentData6 = ettMgr.GetComponentData<AccessBase_Dots>(entity);
				componentData6.Dir = FourDir.Up;
				componentData6.roomType = roomCtrller.roomCfg.type;
				componentData6.themeType = roomCtrller.roomCfg.themeType;
				ettMgr.SetComponentData(entity, componentData6);
				roomCtrller.accessEttList.Add(entity);
			}
			if (LevelMgr.Inst.HaveNeighbor(roomCtrller.MapPos, 0, -1))
			{
				Entity entity2 = ettMgr.Instantiate(sceneEttBED.ett_AccessD);
				LocalTransform componentData7 = ettMgr.GetComponentData<LocalTransform>(entity2);
				componentData7.Position = base.transform.position + new Vector3(-0.5f, 0f - halfHeight - 0.5f, 0f);
				ettMgr.SetComponentData(entity2, componentData7);
				AccessBase_Dots componentData8 = ettMgr.GetComponentData<AccessBase_Dots>(entity2);
				componentData8.Dir = FourDir.Down;
				componentData8.roomType = roomCtrller.roomCfg.type;
				componentData8.themeType = roomCtrller.roomCfg.themeType;
				ettMgr.SetComponentData(entity2, componentData8);
				roomCtrller.accessEttList.Add(entity2);
			}
			if (roomCtrller.roomCfg.isFinalRoom && (DataMgr.selectedWorldData.selectedDifficulty != DifficultyType.Normal || BattleMgr.Inst.CurrentStage != 8 || roomCtrller.roomCfg.type != RoomType.Boss) && roomCtrller.roomCfg.themeType != RoomThemeType.Theme30_EndlessBattle && roomCtrller.roomCfg.accessUp != Vector2Data.Up1000)
			{
				for (int num13 = 0; num13 < LevelMgr.Inst.NextRewardTypes.Count; num13++)
				{
					Entity entity3 = ettMgr.Instantiate(sceneEttBED.ett_Door);
					LocalTransform componentData9 = ettMgr.GetComponentData<LocalTransform>(entity3);
					componentData9.Position = base.transform.position + roomCtrller.roomCfg.accessUp.GetVector3() + new Vector3((float)(-(LevelMgr.Inst.NextRewardTypes.Count - 1)) / 2f * GameConst.doorOffsetX + (float)num13 * GameConst.doorOffsetX, GameConst.doorOffsetY, 0f);
					ettMgr.SetComponentData(entity3, componentData9);
					DoorBase_Dots componentData10 = ettMgr.GetComponentData<DoorBase_Dots>(entity3);
					componentData10.rewardType = LevelMgr.Inst.NextRewardTypes[num13];
					ettMgr.SetComponentData(entity3, componentData10);
					roomCtrller.doorEttList.Add(entity3);
				}
			}
		}
		if (!isRecreate)
		{
			for (int num14 = 0; num14 < ornamentPfbs.Length; num14++)
			{
				new GameObject("OrnamentParent" + num14).transform.SetParent(tsf_OrnamentsParent);
				CreateEquidistanceObj(ornamentPfbs[num14], ornamentDistances[num14], ornamentIntervals[num14], ornamentOffsets[num14], ornamentAccessDontCreates[num14], ornamentAccessDontCreateOffsetYs[num14], num14);
			}
		}
		float3 rootPosition;
		if (!isRecreate)
		{
			Transform obj2 = ssc.transform;
			rootPosition = base.transform.position;
			obj2.localPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Tile8_AboveAO);
			Transform obj3 = mr_UnderWall.transform;
			rootPosition = base.transform.position;
			obj3.localPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Cliff);
			Transform obj4 = mr_Lava.transform;
			rootPosition = base.transform.position;
			obj4.localPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Lava0);
			mr_Lava.transform.localScale = new Vector3(60f, 60f, 1f);
			if (roomCtrller.roomCfg.themeType != RoomThemeType.Theme15_Chapter5_Boss)
			{
				mr_Lava.material.color = mat_Ornament.GetColor("_FogColor");
			}
		}
		if (roomCtrller.roomCfg.type != RoomType.Boss)
		{
			StartCoroutine(DestroySSCIE());
		}
		if (ssc2 != null)
		{
			if (!isRecreate)
			{
				ssc2.spline.Clear();
				for (int num15 = 0; num15 < ssc.spline.GetPointCount(); num15++)
				{
					ssc2.spline.InsertPointAt(num15, ssc.spline.GetPosition(num15));
					ssc2.spline.SetTangentMode(num15, ssc.spline.GetTangentMode(num15));
					ssc2.spline.SetLeftTangent(num15, ssc.spline.GetLeftTangent(num15));
					ssc2.spline.SetRightTangent(num15, ssc.spline.GetRightTangent(num15));
				}
				ssc2.transform.IgnoreZPoint(1.39f);
				ssc2.transform.position += new Vector3(0f, ssc2OffsetY, 0f);
				ssc2.BakeMesh();
			}
			mr_UnderWall2.transform.position = mr_UnderWall.transform.position + underWall2Offset;
			mr_UnderWall2.GetComponent<MeshFilter>().mesh = mr_UnderWall.GetComponent<MeshFilter>().mesh;
		}
		if (!(ssc3 != null))
		{
			return;
		}
		if (!isRecreate)
		{
			ssc3.spline.Clear();
			for (int num16 = 0; num16 < ssc.spline.GetPointCount(); num16++)
			{
				ssc3.spline.InsertPointAt(num16, ssc.spline.GetPosition(num16));
				ssc3.spline.SetTangentMode(num16, ssc.spline.GetTangentMode(num16));
				ssc3.spline.SetLeftTangent(num16, ssc.spline.GetLeftTangent(num16));
				ssc3.spline.SetRightTangent(num16, ssc.spline.GetRightTangent(num16));
			}
			ssc3.BakeMesh();
		}
		Transform obj5 = ssc3.transform;
		rootPosition = base.transform.position;
		obj5.localPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Tile9_AboveAO);
	}

	private IEnumerator DestroySSCIE()
	{
		yield return null;
		yield return null;
		UnityEngine.Object.Destroy(ssc);
		if (ssc2 != null)
		{
			UnityEngine.Object.Destroy(ssc2);
		}
		if (ssc3 != null)
		{
			UnityEngine.Object.Destroy(ssc3);
		}
		UnityEngine.Object.Destroy(this);
	}

	public void HideAccess()
	{
		if (accessLeftEtt != Entity.Null)
		{
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(accessLeftEtt);
			accessLeftOriginalPosition = componentData.Position;
			componentData.Position = new float3(150f, componentData.Position.y, 0f);
			ettMgr.SetComponentData(accessLeftEtt, componentData);
		}
		if (accessRightEtt != Entity.Null)
		{
			LocalTransform componentData2 = ettMgr.GetComponentData<LocalTransform>(accessRightEtt);
			accessRightOriginalPosition = componentData2.Position;
			componentData2.Position = new float3(150f, componentData2.Position.y, 0f);
			ettMgr.SetComponentData(accessRightEtt, componentData2);
		}
	}

	public void RecreateBossRoom()
	{
		spline.Clear();
		edgePoints.Clear();
		UnityEngine.Object.Destroy(go_RecreateGOParent);
		roomCtrller.tsf_Action.DestroyAllChild();
		roomCtrller.tsf_Ground.DestroyAllChild();
		roomCtrller.tsf_Fly.DestroyAllChild();
		roomCtrller.roomCfg.type = RoomType.Monster;
		TileCorrect(roomCtrller, isRecreate: true);
		if (accessLeftEtt != Entity.Null)
		{
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(accessLeftEtt);
			componentData.Position = accessLeftOriginalPosition;
			ettMgr.SetComponentData(accessLeftEtt, componentData);
			if (ettMgr.HasComponent<Access_T8_Dots>(accessLeftEtt))
			{
				Access_T8_Dots componentData2 = ettMgr.GetComponentData<Access_T8_Dots>(accessLeftEtt);
				LocalTransform componentData3 = ettMgr.GetComponentData<LocalTransform>(componentData2.ett_Trigger);
				componentData3.Position = componentData.Position + new float3(-1.5f, -0.5f, 0f);
				ettMgr.SetComponentData(componentData2.ett_Trigger, componentData3);
				componentData2.go_Torch1.Value.transform.position = componentData.Position + componentData2.torch1Offset;
				componentData2.go_Torch2.Value.transform.position = componentData.Position + componentData2.torch2Offset;
			}
			else if (ettMgr.HasComponent<Access_T15_Dots>(accessLeftEtt))
			{
				Access_T15_Dots componentData4 = ettMgr.GetComponentData<Access_T15_Dots>(accessLeftEtt);
				LocalTransform componentData5 = ettMgr.GetComponentData<LocalTransform>(componentData4.ett_Trigger);
				componentData5.Position = componentData.Position + new float3(-1.5f, -0.5f, 0f);
				ettMgr.SetComponentData(componentData4.ett_Trigger, componentData5);
				componentData4.go_Torch1.Value.transform.position = componentData.Position + componentData4.torch1Offset;
				componentData4.go_Torch2.Value.transform.position = componentData.Position + componentData4.torch2Offset;
			}
			else
			{
				Debug.LogError("到底是啥");
			}
		}
		if (accessRightEtt != Entity.Null)
		{
			LocalTransform componentData6 = ettMgr.GetComponentData<LocalTransform>(accessRightEtt);
			componentData6.Position = accessRightOriginalPosition;
			ettMgr.SetComponentData(accessRightEtt, componentData6);
			if (ettMgr.HasComponent<Access_T8_Dots>(accessRightEtt))
			{
				Access_T8_Dots componentData7 = ettMgr.GetComponentData<Access_T8_Dots>(accessRightEtt);
				LocalTransform componentData8 = ettMgr.GetComponentData<LocalTransform>(componentData7.ett_Trigger);
				componentData8.Position = componentData6.Position + new float3(1.5f, -0.5f, 0f);
				ettMgr.SetComponentData(componentData7.ett_Trigger, componentData8);
				componentData7.go_Torch1.Value.transform.position = componentData6.Position + componentData7.torch1Offset;
				componentData7.go_Torch2.Value.transform.position = componentData6.Position + componentData7.torch2Offset;
			}
			else if (ettMgr.HasComponent<Access_T15_Dots>(accessRightEtt))
			{
				Access_T15_Dots componentData9 = ettMgr.GetComponentData<Access_T15_Dots>(accessRightEtt);
				LocalTransform componentData10 = ettMgr.GetComponentData<LocalTransform>(componentData9.ett_Trigger);
				componentData10.Position = componentData6.Position + new float3(1.5f, -0.5f, 0f);
				ettMgr.SetComponentData(componentData9.ett_Trigger, componentData10);
				componentData9.go_Torch1.Value.transform.position = componentData6.Position + componentData9.torch1Offset;
				componentData9.go_Torch2.Value.transform.position = componentData6.Position + componentData9.torch2Offset;
			}
			else
			{
				Debug.LogError("到底是啥");
			}
		}
		for (int i = 0; i < ornamentPfbs.Length; i++)
		{
			Transform child = tsf_OrnamentsParent.GetChild(i);
			for (int num = child.childCount - 1; num >= 0; num--)
			{
				Transform child2 = child.GetChild(num);
				if (Mathf.Abs(child2.position.y - roomCtrller.CenterPoint.y) < ornamentAccessDontCreates[i])
				{
					if (accessLeftEtt != Entity.Null && child2.position.x - roomCtrller.CenterPoint.x < 0f)
					{
						UnityEngine.Object.Destroy(child2.gameObject);
					}
					if (accessRightEtt != Entity.Null && child2.position.x - roomCtrller.CenterPoint.x > 0f)
					{
						UnityEngine.Object.Destroy(child2.gameObject);
					}
				}
			}
		}
	}

	public void ConvertMeshToEttPC(Entity ett, Mesh mesh)
	{
		Vector3[] vertices = mesh.vertices;
		NativeArray<float3> vertices2 = new NativeArray<float3>(vertices.Length, Allocator.Temp);
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices2[i] = vertices[i];
		}
		int[] triangles = mesh.triangles;
		NativeArray<int3> triangles2 = new NativeArray<int3>(triangles.Length / 3, Allocator.Temp);
		for (int j = 0; j < triangles.Length; j += 3)
		{
			triangles2[j / 3] = new int3(triangles[j], triangles[j + 1], triangles[j + 2]);
		}
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 65536u;
		collisionFilter.CollidesWith = DTool.GetCollidesWith(65536u);
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		BlobAssetReference<Unity.Physics.Collider> value = Unity.Physics.MeshCollider.Create(vertices2, triangles2, filter, default(Unity.Physics.Material));
		ettMgr.AddComponentData(ett, new PhysicsCollider
		{
			Value = value
		});
		vertices2.Dispose();
		triangles2.Dispose();
	}
}
