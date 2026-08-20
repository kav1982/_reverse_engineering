using System.Collections;
using System.Collections.Generic;
using PlayerLogger;
using UnityEngine;

public class SpecialObj218 : LayerCorrect, IRoomCtrller
{
	public class Point
	{
		public int id;

		public bool Connecting;

		public Vector3 Position;

		public List<int> Reach = new List<int>();
	}

	public class Line
	{
		public int pointindex1;

		public int pointindex2;

		public bool Connected;
	}

	private Coroutine restartCoroutine;

	private RoomController belongCtrller;

	public int PointNum;

	public int LineNum;

	public Transform LayerRoot;

	public GameObject Pfb_PointObjOriginal;

	public GameObject Pfb_PointObjOriginal_H;

	public GameObject Pfb_LineRendererAnswer;

	public GameObject Pfb_LineRendererCurrent;

	private LineRenderer LineRendererAnswer;

	public float z_LineRendererAnswer = 960f;

	public float z_LineRenderCurrentCharacter;

	public float z_LineRenderCurrent = 960f;

	public float MinDistance;

	public float MaxDistance;

	public float MinLineDistance;

	public float MaxLineDistance;

	public int inderactingPointIndex = -1;

	public List<SpecialObj218_PointObj> AllPoitObjs;

	public List<Point> Points = new List<Point>();

	public List<Line> Lines = new List<Line>();

	public List<LineRenderer> Linerenderers = new List<LineRenderer>();

	public List<Line> CurrentLines = new List<Line>();

	[Header("高级设置")]
	public bool MostNotStart;

	public int HardPointNum;

	public int minAngle = 15;

	public float minDistance = 0.5f;

	public float speed;

	private bool addingdanger;

	private int re1;

	private int re2;

	private int re3;

	private int re4;

	[Header("Art")]
	public float ColorChangeTime = 0.2f;

	public float timeColorFinish = 1f;

	[ColorUsage(true, true)]
	public Color ColorNormal;

	[ColorUsage(true, true)]
	public Color ColorSelected;

	[ColorUsage(true, true)]
	public Color ColorSolved;

	[ColorUsage(true, true)]
	public Color ColorSolvedEnd;

	[ColorUsage(true, true)]
	public Color ColorWrongLine;

	private GameObject Pfb_PointObj
	{
		get
		{
			if (GameMgr.IsHarmony_Static)
			{
				return Pfb_PointObjOriginal_H;
			}
			return Pfb_PointObjOriginal;
		}
	}

	public bool CheckCompletePoint(int Pointid)
	{
		int num = 0;
		int num2 = 0;
		foreach (Line line in Lines)
		{
			if (line.pointindex1 == Pointid || line.pointindex2 == Pointid)
			{
				num++;
			}
		}
		foreach (Line currentLine in CurrentLines)
		{
			if (currentLine.pointindex1 == Pointid || currentLine.pointindex2 == Pointid)
			{
				num2++;
			}
		}
		if (num == num2)
		{
			return true;
		}
		return false;
	}

	private void Start()
	{
		int num = 0;
		GeneratePuzzle(PointNum, LineNum);
		while (!CheckLevelGood() && num < 9999)
		{
			num++;
			GeneratePuzzle(PointNum, LineNum);
		}
		if (num == 9999)
		{
			Debug.LogError($"花了{num}次都没符合要求,降低点条件要求吧");
		}
		Debug.Log($"线的使用不达标{re1},线的总数不达标{re2},出线角度不达标{re3},线太靠近{re4}");
		re1 = 0;
		re2 = 0;
		re3 = 0;
		re4 = 0;
		CenterPoints(ref Points);
		GeneratePointObject();
		CreateLineRenderLines();
	}

	private void Update()
	{
		if (inderactingPointIndex != -1)
		{
			UpDateCurretnLineRenderEnd();
		}
		if (Lines.Count != CurrentLines.Count && Linerenderers.Count > 0 && belongCtrller != LevelMgr.Inst.CurrentRoomCtrller)
		{
			RestartLevel();
		}
	}

	public static void CenterPoints(ref List<Point> points)
	{
		if (points == null || points.Count == 0)
		{
			Debug.LogWarning("The list of points is null or empty.");
			return;
		}
		Vector3 vector = Vector2.zero;
		foreach (Point point in points)
		{
			vector += point.Position;
		}
		vector /= (float)points.Count;
		for (int i = 0; i < points.Count; i++)
		{
			points[i].Position -= vector;
		}
	}

	public void GeneratePuzzle(int PointNum, int LineNum)
	{
		Points.Clear();
		Lines.Clear();
		for (int i = 0; i < PointNum; i++)
		{
			Point point = AddAPoint();
			if (point.Position != Vector3.zero)
			{
				addingdanger = !addingdanger;
				point.id = Points.Count;
				Points.Add(point);
				if (Points.Count >= 2)
				{
					GenerateLine(Points[Points.Count - 2].id, Points[Points.Count - 1].id);
				}
			}
			CalCulateReachable(Points.Count - 1);
		}
		GenerateLinesUseAllContinue(LineNum);
	}

	public Point AddAPoint()
	{
		Point point = new Point();
		Vector3 vector = default(Vector3);
		vector.x = Random.Range(-4f, 4f);
		vector.y = Random.Range(-4f, 4f);
		int num = 0;
		while (!PointDiffer(vector))
		{
			num++;
			vector.x = Random.Range(-4f, 4f);
			vector.y = Random.Range(-4f, 4f);
			if (num > 999)
			{
				vector = Vector3.zero;
				break;
			}
		}
		point.Position = vector;
		return point;
	}

	public bool PointDiffer(Vector3 newposition)
	{
		bool result = true;
		if (Points.Count == 0)
		{
			return result;
		}
		for (int i = 0; i < Points.Count; i++)
		{
			if (Vector3.Distance(newposition, Points[i].Position) > MinDistance && Vector3.Distance(newposition, Points[i].Position) < MaxDistance && Vector3.Distance(newposition, Points[Points.Count - 1].Position) < MaxLineDistance)
			{
				result = true;
				continue;
			}
			result = false;
			break;
		}
		return result;
	}

	public void CalCulateReachable(int index)
	{
		foreach (Point point in Points)
		{
			if (Vector3.Distance(Points[index].Position, point.Position) > MinLineDistance && Vector3.Distance(Points[index].Position, point.Position) < MaxLineDistance)
			{
				point.Reach.Add(index);
				Points[index].Reach.Add(point.id);
			}
		}
	}

	public void GeneratePointObject()
	{
		for (int i = 0; i < Points.Count; i++)
		{
			GameObject gameObject = Object.Instantiate(Pfb_PointObj, Points[i].Position + LayerRoot.transform.position, Quaternion.identity, LayerRoot);
			AllPoitObjs.Add(gameObject.GetComponent<SpecialObj218_PointObj>());
			AllPoitObjs[AllPoitObjs.Count - 1].thisPoint = Points[i];
			AllPoitObjs[AllPoitObjs.Count - 1].Special218 = this;
			gameObject.name = i.ToString();
		}
	}

	public void GenerateLinesUseAllContinue(int LineCount)
	{
		int startPointIndex = Points[Points.Count - 1].id;
		for (int i = 0; i <= LineCount - Points.Count; i++)
		{
			int num = GenerateLine(startPointIndex);
			if (num != -1)
			{
				startPointIndex = num;
				continue;
			}
			break;
		}
	}

	public void GenerateLines(int LineCount)
	{
		if (LineCount <= Points.Count)
		{
			Debug.LogError("线的数量太少");
		}
		int startPointIndex = Random.Range(0, Points.Count);
		for (int i = 0; i < LineCount; i++)
		{
			int num = GenerateLine(startPointIndex);
			if (num == -1)
			{
				break;
			}
			startPointIndex = num;
		}
		Debug.Log($"生成了{Lines.Count}条线段");
	}

	public void GenerateLine(int StartPointIndex, int EndPosition)
	{
		Line line = new Line();
		line.pointindex1 = StartPointIndex;
		line.pointindex2 = EndPosition;
		Lines.Add(line);
	}

	public int GenerateLine(int StartPointIndex)
	{
		foreach (int item in Points[StartPointIndex].Reach)
		{
			if (LineDiffer(StartPointIndex, item, Lines) && AddLineNotDanger(item))
			{
				Line line = new Line();
				line.pointindex1 = StartPointIndex;
				line.pointindex2 = item;
				Lines.Add(line);
				addingdanger = !addingdanger;
				return item;
			}
		}
		return -1;
	}

	public void GenerateLineTest()
	{
		for (int i = 0; i < Points.Count; i++)
		{
			foreach (int item in Points[i].Reach)
			{
				if (LineDiffer(i, item, Lines))
				{
					Line line = new Line();
					line.pointindex1 = i;
					line.pointindex2 = item;
					Lines.Add(line);
				}
			}
		}
		Debug.Log($"共有{Lines.Count}条可连线段");
	}

	public bool CheckLevelGood()
	{
		if (AllPointUsed(2) && ChekLindNum() && AngleCheck())
		{
			return DistanceCheck();
		}
		return false;
		bool AllPointUsed(int x)
		{
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			foreach (Point point in Points)
			{
				int num7 = 0;
				foreach (Line line in Lines)
				{
					if (line.pointindex1 == point.id || line.pointindex2 == point.id)
					{
						num7++;
					}
				}
				if (num7 < x)
				{
					re1++;
					return false;
				}
				if (num7 == 3)
				{
					num4++;
				}
				if (num7 == 5)
				{
					num5++;
				}
				if (num7 >= 3)
				{
					num6++;
				}
			}
			if (MostNotStart)
			{
				if (num6 >= HardPointNum && num4 > 0 && num5 == 0)
				{
					return true;
				}
				re1++;
				return false;
			}
			if (num4 == 0 && num5 == 0)
			{
				return false;
			}
			return true;
		}
		bool AngleCheck()
		{
			List<int> list = new List<int>();
			float num = 360f;
			foreach (Point point2 in Points)
			{
				list.Clear();
				foreach (Line line2 in Lines)
				{
					if (line2.pointindex1 == point2.id)
					{
						if (!list.Contains(line2.pointindex2))
						{
							list.Add(line2.pointindex2);
						}
					}
					else if (line2.pointindex2 == point2.id && !list.Contains(line2.pointindex1))
					{
						list.Add(line2.pointindex1);
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					int num2 = i + 1;
					for (; i < list.Count; i++)
					{
						if (i != num2)
						{
							float num3 = Angle(point2.Position - Points[list[i]].Position, point2.Position - Points[list[num2]].Position);
							if (num > num3)
							{
								num = num3;
							}
						}
					}
				}
			}
			if (num < (float)minAngle)
			{
				re3++;
				return false;
			}
			return true;
		}
		bool ChekLindNum()
		{
			if (Lines.Count == LineNum)
			{
				return true;
			}
			re2++;
			return false;
		}
		bool DistanceCheck()
		{
			foreach (Point point3 in Points)
			{
				foreach (Line line3 in Lines)
				{
					if (Between(Points[line3.pointindex1].Position, Points[line3.pointindex2].Position, point3.Position) && point3.id != Points[line3.pointindex1].id && point3.id != Points[line3.pointindex2].id && DistanceToLine(Points[line3.pointindex1].Position, Points[line3.pointindex2].Position, point3.Position) < minDistance)
					{
						re4++;
						return false;
					}
				}
			}
			return true;
		}
	}

	public bool LineDiffer(int index1, int index2, List<Line> lines)
	{
		bool result = true;
		foreach (Line line in lines)
		{
			if ((line.pointindex1 == index1 && line.pointindex2 == index2) || (line.pointindex1 == index2 && line.pointindex2 == index1))
			{
				return false;
			}
		}
		return result;
	}

	public bool AddLineNotDanger(int index2)
	{
		return true;
	}

	public bool LineCheck(int newPoint)
	{
		if (restartCoroutine != null)
		{
			return false;
		}
		if (inderactingPointIndex == -1)
		{
			foreach (Line line2 in Lines)
			{
				if (line2.pointindex1 == newPoint || line2.pointindex2 == newPoint)
				{
					StartLIneRenderCurrentLines(newPoint);
					inderactingPointIndex = newPoint;
					SEMgr.Inst.puzzleClick.PlaySE();
					return true;
				}
			}
			return false;
		}
		if (!LineDiffer(inderactingPointIndex, newPoint, Lines) && (CurrentLines.Count == 0 || LineDiffer(inderactingPointIndex, newPoint, CurrentLines)))
		{
			Line line = new Line();
			line.pointindex1 = inderactingPointIndex;
			line.pointindex2 = newPoint;
			CurrentLines.Add(line);
			AddLIneRenderCurrentLines(newPoint);
			SEMgr.Inst.puzzleClick.PlaySE();
			return true;
		}
		restartCoroutine = StartCoroutine(WrongLine());
		return false;
	}

	public void RestartLevel()
	{
		foreach (SpecialObj218_PointObj allPoitObj in AllPoitObjs)
		{
			allPoitObj.interacting = false;
			allPoitObj.SpriteRenterer.material.SetColor("_MainColor", ColorNormal);
			allPoitObj.LightDown();
		}
		CurrentLines.Clear();
		inderactingPointIndex = -1;
		for (int i = 0; i < Linerenderers.Count; i++)
		{
			Object.Destroy(Linerenderers[i].gameObject);
		}
		Linerenderers.Clear();
	}

	public bool CheckComplete()
	{
		if (Lines.Count == CurrentLines.Count)
		{
			Vector3 position = Points[inderactingPointIndex].Position;
			StartCoroutine(CompleteLightUp(position));
			return true;
		}
		return false;
	}

	private void OnDrawGizmos()
	{
		foreach (Point point in Points)
		{
			Gizmos.DrawSphere(Tool2D.IgnoreZPoint(base.transform.position + point.Position), 0.2f);
		}
	}

	public void CreateLineRenderLines()
	{
		GameObject gameObject = Object.Instantiate(Pfb_LineRendererAnswer, base.transform.position, Quaternion.identity, LayerRoot);
		LineRendererAnswer = gameObject.GetComponent<LineRenderer>();
		LineRendererAnswer.positionCount = Lines.Count + 1;
		LineRendererAnswer.SetPosition(0, Points[Lines[0].pointindex1].Position + new Vector3(0f, 0f, z_LineRendererAnswer));
		for (int i = 0; i < Lines.Count; i++)
		{
			LineRendererAnswer.SetPosition(i + 1, Points[Lines[i].pointindex2].Position + new Vector3(0f, 0f, z_LineRendererAnswer));
		}
	}

	public void StartLIneRenderCurrentLines(int StartPointIndex)
	{
		Debug.Log("设置初始点");
		Linerenderers.Add(Object.Instantiate(Pfb_LineRendererCurrent, LayerRoot.transform.position, Quaternion.identity, LayerRoot).GetComponent<LineRenderer>());
		Linerenderers[Linerenderers.Count - 1].material.SetColor("_MainColor", ColorSelected);
		Linerenderers[Linerenderers.Count - 1].positionCount = 2;
		Linerenderers[Linerenderers.Count - 1].SetPosition(0, Points[StartPointIndex].Position + new Vector3(0f, 0f, z_LineRenderCurrent));
		Linerenderers[Linerenderers.Count - 1].SetPosition(1, Points[StartPointIndex].Position + new Vector3(0f, 0f, z_LineRenderCurrentCharacter));
	}

	public void AddLIneRenderCurrentLines(int StartPointIndex)
	{
		Linerenderers.Insert(Linerenderers.Count - 1, Object.Instantiate(Pfb_LineRendererCurrent, LayerRoot.transform.position, Quaternion.identity, LayerRoot).GetComponent<LineRenderer>());
		Linerenderers[Linerenderers.Count - 2].positionCount = 2;
		Linerenderers[Linerenderers.Count - 2].material.SetColor("_MainColor", ColorSelected);
		Linerenderers[Linerenderers.Count - 2].SetPosition(0, Points[inderactingPointIndex].Position + new Vector3(0f, 0f, z_LineRenderCurrent));
		Linerenderers[Linerenderers.Count - 2].SetPosition(1, Points[StartPointIndex].Position + new Vector3(0f, 0f, z_LineRenderCurrent));
		Linerenderers[Linerenderers.Count - 1].SetPosition(0, Points[StartPointIndex].Position + new Vector3(0f, 0f, z_LineRenderCurrent));
	}

	public void UpDateCurretnLineRenderEnd()
	{
		Linerenderers[Linerenderers.Count - 1].SetPosition(1, base.transform.InverseTransformPoint(PlayerMgr.Inst.PlayerCtrller.transform.position + new Vector3(0f, 0f, z_LineRenderCurrentCharacter)));
	}

	public float Angle(Vector3 vec1, Vector3 vec2)
	{
		float num = Vector3.Angle(vec1, vec2);
		if (Vector3.Dot(Vector3.Cross(vec1, vec2), Vector3.down) < 0f)
		{
			num *= -1f;
			num += 360f;
		}
		return num;
	}

	public float DistanceToLine(Vector3 p1, Vector3 p2, Vector3 target)
	{
		Vector3 onNormal = p2 - p1;
		Vector3 vector = Vector3.Project(target - p1, onNormal);
		return Vector3.Distance(target, vector + p1);
	}

	private IEnumerator CompleteLightUp(Vector3 position)
	{
		inderactingPointIndex = -1;
		Object.Destroy(Linerenderers[Linerenderers.Count - 1].gameObject);
		Linerenderers.RemoveAt(Linerenderers.Count - 1);
		foreach (SpecialObj218_PointObj allPoitObj in AllPoitObjs)
		{
			allPoitObj.interacting = true;
		}
		yield return new WaitForSecondsRealtime(0.5f);
		DropRward(position);
		SEMgr.Inst.puzzleSucceed.PlaySE();
		yield return new WaitForSecondsRealtime(0.5f);
	}

	private IEnumerator WrongLine()
	{
		SEMgr.Inst.puzzleFail.PlaySE();
		Linerenderers[Linerenderers.Count - 1].material.SetColor("_MainColor", ColorWrongLine);
		yield return new WaitForSecondsRealtime(0.5f);
		RestartLevel();
		restartCoroutine = null;
	}

	public void DropRward(Vector3 position)
	{
		int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
		LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
		ItemInfo itemInfo = default(ItemInfo);
		itemInfo.type = ItemType.Spell;
		itemInfo.id = specialRoomSpell;
		ItemInfo info = itemInfo;
		QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, info, base.transform.position);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Puzzle_Correct", base.transform.position + position, 2f);
	}

	public bool Between(Vector3 p1, Vector3 p2, Vector3 target)
	{
		if (target.x < p2.x && target.x > p1.x)
		{
			return true;
		}
		if (target.x > p2.x && target.x < p1.x)
		{
			return true;
		}
		if (target.y < p2.y && target.y > p1.y)
		{
			return true;
		}
		if (target.y > p2.y && target.y < p1.y)
		{
			return true;
		}
		return false;
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongCtrller = roomCtrller;
	}
}
