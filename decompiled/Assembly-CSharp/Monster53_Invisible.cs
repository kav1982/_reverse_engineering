using System.Collections;
using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;

public class Monster53_Invisible : UnitBase, IRoomObjExtraData
{
	[Header("高度场")]
	public static float[,] heightField;

	public static int roomID;

	public static Vector3 basePoint;

	public LayerMask wallMask;

	[Header("飞行轨迹队列")]
	public int flyPointCount;

	private Vector3[] nodePoints;

	private Vector3[] nodeSpeed;

	public float deltaLength;

	public float minDeltalength;

	private float nowDeltaLength;

	public float smoothTime;

	private int moveableNode;

	[Header("召唤子体")]
	public int summonID;

	public List<Monster53> livingChild = new List<Monster53>();

	public int maxChildCount;

	public int standardChildCount;

	private int summonedChildCount;

	public float summonInterval;

	public float summonDelay;

	private Vector3 summonPoint;

	private bool summonProtection;

	[Header("召唤传送门")]
	public Transform portalTransform;

	public ParticleSystem portalParticle;

	public Vector3 portalPoint;

	[Header("飞行控制")]
	public VariableFloat flyRelocateInterval;

	public VariableFloat flyRotateSpeed;

	private float flyRelocateIntervalTimer;

	private Vector3 FlyPoint;

	public float flyLocateRadius;

	private Vector3 CurrentDir;

	private Vector3 roomCenterPoint;

	private int roomWidth;

	private int roomHeight;

	public int GetIndex(Monster53 child)
	{
		return livingChild.IndexOf(child);
	}

	public Vector3 GetQueuePosition(int index, float horizonOffset)
	{
		Vector3 normalized;
		if (index == 0)
		{
			normalized = (nodePoints[0] - nodePoints[1]).normalized;
			return nodePoints[0] + Tool2D.GetDir(normalized, 90f) * horizonOffset;
		}
		int num = moveableNode;
		if (num == 0)
		{
			normalized = (nodePoints[0] - nodePoints[1]).normalized;
			return nodePoints[0] + Tool2D.GetDir(normalized, 90f) * horizonOffset;
		}
		int num2 = moveableNode;
		float num3 = 1f - (float)index / (float)livingChild.Count;
		int num4 = 0;
		while (num3 > 1f / (float)num && num4 < 100 && num2 > 1)
		{
			num3 -= 1f / (float)num;
			num2--;
			num4++;
		}
		normalized = (nodePoints[num2 - 1] - nodePoints[num2]).normalized;
		return nodePoints[num2] + normalized * num3 * (num - 1) * deltaLength + Tool2D.GetDir(normalized, 90f) * horizonOffset;
	}

	public void GetIndexByPos(Vector3 pos, ref int x, ref int y)
	{
		basePoint = roomCenterPoint - new Vector3(roomWidth / 2, roomHeight / 2, 0f);
		x = (int)(pos.x - basePoint.x);
		y = (int)(pos.y - basePoint.y);
		x = Mathf.Clamp(x, 0, roomWidth - 1);
		y = Mathf.Clamp(y, 0, roomHeight - 1);
	}

	public void GenerateHeightMap()
	{
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		roomID = LevelMgr.Inst.CurrentRoomCfg.id;
		if (LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		heightField = new float[roomWidth, roomHeight];
		for (int i = 0; i < roomWidth; i++)
		{
			for (int j = 0; j < roomHeight; j++)
			{
				heightField[i, j] = 0f;
			}
		}
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 256u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitDotsSyncSystem.OverlapBox(roomCenterPoint, new Vector3(roomWidth / 2 + 1, roomHeight + 1, 1f), Quaternion.identity, filter, list);
		Debug.Log(list.Count);
		for (int k = 0; k < list.Count; k++)
		{
			int x = 0;
			int y = 0;
			GetIndexByPos(list[k].point, ref x, ref y);
			heightField[x, y] = 1f;
		}
		for (int l = 0; l < roomWidth; l++)
		{
			for (int m = 0; m < roomHeight; m++)
			{
				if (heightField[l, m] == 1f)
				{
					continue;
				}
				for (int n = -1; n < 2; n++)
				{
					for (int num = -1; num < 2; num++)
					{
						if (heightField[Mathf.Clamp(l + n, 0, roomWidth - 1), Mathf.Clamp(m + num, 0, roomHeight - 1)] == 1f && heightField[l, m] == 0f)
						{
							heightField[l, m] = 0.5f;
						}
					}
				}
			}
		}
	}

	public float GetHeightByNode(float x, float y)
	{
		if (x < 0f || x >= (float)roomWidth || y < 0f || y >= (float)roomHeight)
		{
			return 0f;
		}
		return heightField[(int)x, (int)y];
	}

	public float GetHeightValue(Vector3 pos)
	{
		int x = 0;
		int y = 0;
		GetIndexByPos(pos, ref x, ref y);
		Vector3 vector = pos - basePoint - new Vector3(x, y, 0f);
		Vector3 vector2 = new Vector3(x, y, 0f);
		Vector3 vector3 = new Vector3(x + 1, y, 0f);
		Vector3 vector4 = new Vector3(x, y + 1, 0f);
		Vector3 vector5 = new Vector3(x + 1, y + 1, 0f);
		Vector3[] array = new Vector3[4] { vector2, vector3, vector4, vector5 };
		vector2 = new Vector3(x, y, 0f);
		vector3 = new Vector3(x + 1, y, 0f);
		vector4 = new Vector3(x, y + 1, 0f);
		vector5 = new Vector3(x + 1, y + 1, 0f);
		vector.x = Mathf.Clamp(vector.x, -1f, 1f);
		vector.y = Mathf.Clamp(vector.y, -1f, 1f);
		Vector3 vector6 = new Vector3(0f, 0f, 0f);
		vector6 = ((vector.x >= 0f && vector.y >= 0f) ? new Vector3(0f, 0f, 0f) : ((vector.x >= 0f && vector.y <= 0f) ? new Vector3(0f, -1f, 0f) : ((!(vector.x <= 0f) || !(vector.y >= 0f)) ? new Vector3(-1f, 1f, 0f) : new Vector3(-1f, 0f, 0f))));
		for (int i = 0; i < 4; i++)
		{
			array[i] += vector6;
		}
		if (vector.x < 0f)
		{
			vector.x += 1f;
		}
		if (vector.y < 0f)
		{
			vector.y += 1f;
		}
		float num = (1f - vector.x) * (1f - vector.y);
		float num2 = vector.x * (1f - vector.y);
		float num3 = (1f - vector.x) * vector.y;
		float num4 = vector.x * vector.y;
		float num5 = num + num2 + num3 + num4;
		return GetHeightByNode(vector2.x, vector2.y) * num / num5 + GetHeightByNode(vector4.x, vector4.y) * num3 / num5 + GetHeightByNode(vector3.x, vector3.y) * num2 / num5 + GetHeightByNode(vector5.x, vector5.y) * num4 / num5;
	}

	public IEnumerator SummonChild()
	{
		SEMgr.Inst.monster53_Portal.PlaySE();
		portalParticle.Play();
		yield return new WaitForSeconds(summonDelay);
		for (int i = 0; i < maxChildCount; i++)
		{
			if (i % 2 == 0)
			{
				SEMgr.Inst.monster7Blink.PlaySE();
			}
			Monster53 component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + summonID, Tool2D.GetNavMeshPointIngoreZ(summonPoint)).GetComponent<Monster53>();
			livingChild.Add(component);
			component.Initialize(this);
			summonedChildCount++;
			yield return new WaitForSeconds(summonInterval);
		}
		portalParticle.Stop();
		summonProtection = false;
	}

	public override void SingleInitialCallback()
	{
		if (GameMgr.IsMobile_Static)
		{
			maxChildCount = Mathf.CeilToInt((float)maxChildCount * 0.67f);
		}
	}

	public override void EveryInitialCallback()
	{
		GenerateHeightMap();
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		componentData.InvincibleRegister();
		SetComponentData(componentData);
		nodePoints = new Vector3[flyPointCount];
		nodeSpeed = new Vector3[flyPointCount];
		for (int i = 0; i < flyPointCount; i++)
		{
			nodePoints[i] = base.transform.position;
			nodeSpeed[i] = Vector3.zero;
		}
		flyRotateSpeed.RandomResult();
		flyRelocateInterval.RandomResult();
		portalPoint = Tool2D.GetLayerPoint(base.transform.position);
		summonPoint = base.transform.position;
		livingChild.Clear();
		summonProtection = true;
		StartCoroutine(SummonChild());
		moveableNode = 0;
		summonedChildCount = 1;
		LocateFlyPoint();
		CurrentDir = Tool2D.GetDir();
	}

	public int GetNode()
	{
		return Random.Range(0, flyPointCount);
	}

	public Vector3 GetPoint(int i)
	{
		return nodePoints[i];
	}

	private void LocateFlyPoint()
	{
		GetNearestTarget();
		if (base.HaveTarget)
		{
			FlyPoint = Tool2D.IgnoreZPoint(base.TargetPoint) + Tool2D.GetDir() * Random.Range(0f, flyLocateRadius);
		}
		else
		{
			FlyPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + Tool2D.GetDir() * Random.Range(0f, flyLocateRadius);
		}
	}

	public override void Update()
	{
		for (int i = 0; i < roomWidth; i++)
		{
			for (int j = 0; j < roomHeight; j++)
			{
				Debug.DrawLine(color: (heightField[i, j] != 0f) ? ((heightField[i, j] != 1f) ? Color.magenta : Color.red) : Color.blue, start: basePoint + new Vector3(i, j, 0f), end: basePoint + new Vector3(i, (float)j + 0.5f, 0f));
			}
		}
		for (int num = livingChild.Count - 1; num >= 0; num--)
		{
			if (livingChild[num] != null && livingChild[num].myPpt.AlreadyDead)
			{
				livingChild.RemoveAt(num);
			}
		}
		if (livingChild.Count <= 0 && !summonProtection)
		{
			DotsAnnouncedDeath();
			return;
		}
		if (livingChild.Count > 0)
		{
			base.transform.position = livingChild[0].transform.position;
			SyncDotsPosition();
		}
		if (base.IsLocked)
		{
			return;
		}
		flyRelocateIntervalTimer += Time.deltaTime;
		if (flyRelocateIntervalTimer >= flyRelocateInterval.result)
		{
			flyRelocateIntervalTimer = 0f;
			flyRelocateInterval.RandomResult();
			LocateFlyPoint();
		}
		if (livingChild.Count > 0 && (livingChild[0].transform.position - nodePoints[0]).sqrMagnitude < 4f)
		{
			CurrentDir = Tool2D.DirMoveTowards(CurrentDir, ToPointDir(FlyPoint), Time.deltaTime * flyRotateSpeed.result);
			nodePoints[0] += CurrentDir * base.MoveSpeed * Time.deltaTime;
		}
		if (!summonProtection)
		{
			nowDeltaLength = Mathf.Lerp(minDeltalength, deltaLength, (float)livingChild.Count / (float)summonedChildCount) * ((float)maxChildCount / (float)standardChildCount);
		}
		else
		{
			nowDeltaLength = deltaLength * ((float)maxChildCount / (float)standardChildCount);
		}
		for (int k = 0; k < flyPointCount; k++)
		{
			if (k == 0)
			{
				continue;
			}
			Debug.DrawLine(nodePoints[k - 1], nodePoints[k], Color.white);
			if (k > moveableNode)
			{
				if (!((nodePoints[k - 1] - nodePoints[k]).sqrMagnitude > nowDeltaLength * nowDeltaLength * 4f))
				{
					break;
				}
				moveableNode++;
			}
			if (k == 1)
			{
				Vector3 target = nodePoints[k - 1] + (nodePoints[k] - nodePoints[k - 1]).normalized * nowDeltaLength;
				nodePoints[k] = Vector3.SmoothDamp(nodePoints[k], target, ref nodeSpeed[k], smoothTime);
			}
			else
			{
				Vector3 vector = nodePoints[k - 1] - nodePoints[k - 2];
				Vector3 target2 = nodePoints[k - 1] + vector.normalized * nowDeltaLength;
				nodePoints[k] = Vector3.SmoothDamp(nodePoints[k], target2, ref nodeSpeed[k], smoothTime);
			}
		}
	}

	public void LateUpdate()
	{
		portalTransform.position = portalPoint;
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		info.immuneDamage = true;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		for (int i = 0; i < livingChild.Count; i++)
		{
			if (livingChild[i] != null)
			{
				livingChild[i].DotsAnnouncedDeath();
			}
		}
		base.AfterDead(ref info);
		StopAllCoroutines();
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 > 0f)
		{
			maxChildCount = (int)data1;
		}
	}
}
